using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool.Wrap;

namespace Tool.Export
{
    public class Gen_Typescript
    {
        static string currentspace;


        public static void Gen()
        {
            List<string> imports = new List<string>()
            {
                "import { Stream } from '../Stream';"
            };

            var namespaces = Util.GetNamespaceStruct();
            foreach (var item in namespaces)
            {
                var builder = new StringBuilder();
                builder.AppendLine(string.Join("\n", imports));

                var cfgTypes = new List<string>();
                var configs = new StringBuilder();
                var structs = new StringBuilder();
                var enums = new StringBuilder();
                var decaCfg = new StringBuilder()
                    .AppendLine("declare module '../CfgManager' {")
                    .Space()
                    .AppendLine("interface CfgManager {");
                var decaStruct = new StringBuilder()
                    .AppendLine("declare module '../Stream' {")
                    .Space()
                    .AppendLine("interface StreamBase {");

                foreach (var obj in item.Value)
                {
                    if (obj is ClassWrap cls)
                    {
                        currentspace = cls.Namespace + ".";
                        //导入其他模块
                        var others = LinkModules(cls).Distinct()
                            .Select(m => m.Replace(Setting.DotSplit[0].ToString(), ""))
                            .Select(m => $"import * as {m} from './{m}';");
                        if (others.Count() > 0)
                            builder.AppendLine(string.Join("\n", others));

                        //定义结构
                        GenConfig(configs, cls);
                        GenDataStruct(structs, cls);
                        if (cls.IsConfig())
                        {
                            string cfgName = GetCfgName(cls.Name);
                            string fullName = GetFormatFullName(cls.FullName);
                            decaCfg.Space(2).AppendLine($"get {GetCfgName(fullName)}(): {cfgName};");
                            cfgTypes.Add(cfgName);
                        }
                        else
                            decaStruct.Space(2).AppendLine($"Get{cls.Name}(): {cls.Name};");
                    }
                    else if (obj is EnumWrap enm)
                    {
                        GenEnum(enums, enm);
                    }
                }
                configs.AppendLine($"export let _CFG_CLASS_ = [{string.Join(", ", cfgTypes)}];");
                decaCfg.Space().AppendLine("}\n}");
                decaStruct.Space().AppendLine("}\n}");
                string path = Path.Combine(Setting.TSDir, item.Key.Replace(Setting.DotSplit[0].ToString(), "") + ".ts");

                builder.AppendLine("/**************************************** 数据配置表 *****************************************/")
                    .AppendLine(configs.ToString())
                    .AppendLine("/**************************************** 数据结构定义 *****************************************/")
                    .AppendLine(enums.ToString())
                    .AppendLine(structs.ToString())
                    .AppendLine("/**************************************** 声明与导出 *****************************************/")
                    .AppendLine(decaCfg.ToString())
                    .AppendLine(decaStruct.ToString());
                Util.SaveFile(path, builder.ToString());
            }
        }
        static List<string> LinkModules(ClassWrap cls)
        {
            var list = new List<string>();
            list.AddRange(cls.Consts.Where(c => EnumWrap.IsEnum(c.FullName))
               .Where(c => c.FullName.IndexOf(cls.Namespace) != 0)
               .Select(c => EnumWrap.Enums[c.FullName].Namespace));
            list.AddRange(cls.Fields.Where(f => f.IsEnum)
               .Where(f => f.FullName.IndexOf(cls.Namespace) != 0)
               .Select(c => EnumWrap.Enums[c.FullName].Namespace));
            list.AddRange(cls.Fields.Where(f => f.IsClass)
               .Where(f => f.FullName.IndexOf(cls.Namespace) != 0)
               .Select(c => ClassWrap.Classes[c.FullName].Namespace));
            return list;
        }
        static string CorrectNamespace(string fullName)
        {
            if (fullName.IndexOf(currentspace) == 0)
                return fullName.Replace(currentspace, "");
            else
                return fullName;
        }
        static string GetCfgName(string name) => $"{name}Cfg";
        static string GetFormatFullName(string fullName)
            => fullName.Replace(Setting.DotSplit[0].ToString(), "");
        static string GetRawType(string type)
        {
            switch (type)
            {
                case Setting.INT:
                case Setting.LONG:
                case Setting.FLOAT:
                    type = "number";
                    break;
                case Setting.BOOL:
                    type = "boolean";
                    break;
                case Setting.STRING:
                    type = "string";
                    break;
            }
            return CorrectNamespace(type);
        }
        static string GetType(FieldWrap field)
        {
            if (field.IsRaw || field.IsEnum || field.IsClass)
                return GetRawType(field.OriginalType);
            else if (field.OriginalType == Setting.LIST)
                return $"{GetRawType(field.Types[1])}[]";
            else if (field.OriginalType == Setting.DICT)
                return $"Map<{GetRawType(field.Types[1])}, {GetRawType(field.Types[2])}>";
            else
                return "unknow";
        }
        static string GetDynamicName(ClassWrap cls)
        {
            string formatName = GetFormatFullName(cls.FullName);
            string parseName = Setting.ModuleName + (cls.IsDynamic() ? $"{formatName}Maker" : formatName);
            return parseName;
        }
        static string GetDefineName(string fullName, bool isDynamic = true)
        {
            if (Setting.RawTypes.Contains(fullName))
                return Util.FirstCharUpper(fullName);
            else if (EnumWrap.IsEnum(fullName))
                return "Int";
            else if (ClassWrap.IsClass(fullName))
            {
                var cls = ClassWrap.Classes[fullName];
                if (isDynamic)
                    return GetDynamicName(cls);
                else
                    return Setting.ModuleName + GetFormatFullName(cls.FullName);
            }
            else
                return "unknow:" + fullName;
        }
        static string GetDefineName(FieldWrap field)
        {
            if (field.IsRaw)
                return $"o.Get{Util.FirstCharUpper(field.FullName)}()";
            else if (field.IsEnum)
                return $"<{CorrectNamespace(field.FullName)}>o.GetInt()";
            else if (field.IsClass)
            {
                var cls = ClassWrap.Classes[field.FullName];
                string parseName = GetDynamicName(cls);
                return $"o.Get{parseName}()";
            }
            else if (field.OriginalType == Setting.LIST)
                return $"o.Get{Util.FirstCharUpper(field.OriginalType)}('{GetDefineName(field.Types[1])}')";
            else if (field.OriginalType == Setting.DICT)
                return $"o.Get{Util.FirstCharUpper(field.OriginalType)}('{GetDefineName(field.Types[1])}', '{GetDefineName(field.Types[2])}')";
            else
                return "unknow:" + field.FullName;
        }


        /// <summary>
        /// 一张表的结构
        /// </summary>
        static void GenConfig(StringBuilder builder, ClassWrap cls)
        {
            if (!cls.IsConfig())
                return;

            ConfigWrap config = ConfigWrap.Get(cls.FullName);
            string cfgName = GetCfgName(cls.Name);
            string relative = cls.FullName.Replace(Setting.DotSplit[0], '/').ToLower();
            string fullName = GetFormatFullName(cls.FullName);
            string parseName = GetDynamicName(cls);
            builder.AppendLine($"/** {cls.Name}表数据类*/")
                .AppendLine($"export class {cfgName} extends Stream {{")
                .Space().AppendLine($"static readonly refence = '{GetCfgName(fullName)}';")
                .Space().AppendLine("get length() { return this._cfgs ? this._cfgs.length : 0; }")
                .Space().AppendLine("get cfgs() { return this._cfgs; }")
                .Space().AppendLine($"private _cfgs: {cls.Name}[] = [];")
                .Space().AppendLine($"private _key2idx: Map<number, number> = new Map();")
                .Space().AppendLine("constructor(rootDir: string) {")
                .Space(2).AppendLine($"super(rootDir + '{relative}{Setting.DataFileExt}');")
                .Space().AppendLine("}")
                .Space().AppendLine($"/**key索引数据(主键:{config.Index.Name})*/")
                .Space().AppendLine($"Get(key: number): {cls.Name} | undefined {{")
                .Space(2).AppendLine("let idx = this._key2idx.get(key);")
                .Space(2).AppendLine("if (idx == undefined) {")
                .Space(3).AppendLine("console.error(`${this.path} key does not exist:${key}`);")
                .Space(3).AppendLine("return undefined;")
                .Space(2).AppendLine("}")
                .Space(2).AppendLine("if (this.hasLoaded) {")
                .Space(3).AppendLine("return this._cfgs[idx];")
                .Space(2).AppendLine("}")
                .Space(2).AppendLine("else {")
                .Space(3).AppendLine("this.LoadConfig();")
                .Space(3).AppendLine("return this._cfgs[idx];")
                .Space(2).AppendLine("}")       
                .Space().AppendLine("}")       
                .Space().AppendLine("/**下标索引数据*/")       
                .Space().AppendLine("At(idx:number){")       
                .Space(2).AppendLine("if (this.hasLoaded) {")
                .Space(3).AppendLine("return this._cfgs[idx];")
                .Space(2).AppendLine("}")
                .Space(2).AppendLine("else {")
                .Space(3).AppendLine("this.LoadConfig();")
                .Space(3).AppendLine("return this._cfgs[idx];")
                .Space(2).AppendLine("}")
                .Space().AppendLine("}")
                .Space().AppendLine("protected ParseConfig() {")
                .Space(2).AppendLine($"this._cfgs = this.GetList('{parseName}');")
                .Space(2).AppendLine("for (let index = 0; index < this._cfgs.length; index++) {")
                .Space(3).AppendLine("const item = this._cfgs[index];")
                .Space(3).AppendLine($"this._key2idx.set(item.{config.Index.Name}, index);")
                .Space(2).AppendLine("}")
                .Space().AppendLine("}")
                .Space().AppendLine("[Symbol.iterator]() { return this._cfgs.values(); }")
                .AppendLine("}");
        }
        /// <summary>
        /// 数据结构
        /// </summary>
        static void GenDataStruct(StringBuilder builder, ClassWrap cls)
        {
            string extends = cls.Inherit.IsEmpty() ? " " : $" extends {CorrectNamespace(cls.Inherit)} ";
            builder.AppendLine($"/** {cls.Desc} */")
                .AppendLine($"export class {cls.Name}{extends}{{");
            for (int j = 0; j < cls.Consts.Count; j++)
            {
                ConstWrap cst = cls.Consts[j];
                if (!Util.MatchGroups(cst.Group)) continue;
                string value = cst.Value;
                switch (cst.FullName)
                {
                    case Setting.BOOL:
                        value = value.ToLower();
                        break;
                    case Setting.STRING:
                        value = string.Format("'{0}'", value);
                        break;
                    default:
                        if (EnumWrap.IsEnum(cst.FullName))
                        {
                            string enumName = EnumWrap.Enums[cst.FullName].GetEnumName(value);
                            enumName = enumName.IsEmpty() ? value : enumName;
                            value = string.Format("{0}.{1}", CorrectNamespace(cst.FullName), enumName);
                        }
                        break;
                }
                builder.Space().AppendLine($"/** {cst.Desc} */")
                    .Space().AppendLine($"static readonly {cst.Name} = {value};");
            }
            foreach (var item in cls.Fields)
                builder.Space().AppendLine($"/** {item.Desc} */")
                    .Space().AppendLine($"readonly {item.Name}: {GetType(item)};");
            builder.Space().AppendLine("constructor(o: any) {");
            if (!cls.Inherit.IsEmpty())
                builder.Space(2).AppendLine("super(o);");
            foreach (var item in cls.Fields)
                builder.Space(2).AppendLine($"this.{item.Name} = {GetDefineName(item)};");
            builder.Space().AppendLine("}")
                .AppendLine("}");

            if (cls.IsDynamic())
                builder.AppendLine($"Object.defineProperty(Stream.prototype, 'Get{GetDefineName(cls.FullName)}', {{")
                   .Space().AppendLine("value: function (this: any) { return this[`Get${this.GetString().replace(/[\\.]+/g, '')}`](); },")
                   .Space().AppendLine("writable: false,")
                   .AppendLine("});");
            builder.AppendLine($"Object.defineProperty(Stream.prototype, 'Get{GetDefineName(cls.FullName, false)}', {{")
                .Space().AppendLine($"value: function(this: any) {{ return new {cls.Name}(this); }},")
                .Space().AppendLine("writable: false,")
                .AppendLine("});");
        }
        static void GenEnum(StringBuilder builder, EnumWrap enm)
        {
            builder.AppendLine($"/** {enm.Desc} */")
                .AppendLine($"export enum {CorrectNamespace(enm.Name)}{{");
            foreach (var item in enm.Values)
            {
                var alias = enm.Items[item.Key].Alias;
                if (!alias.IsEmpty())
                    builder.Space().AppendLine($"/**{alias}*/");
                builder.Space().AppendLine($"{item.Key} = {item.Value},");
            }
            builder.AppendLine("}");
        }
    }
}
