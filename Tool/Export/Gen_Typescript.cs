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
                    .IntervalLevel()
                    .AppendLine("interface CfgManager {");
                var decaStruct = new StringBuilder()
                    .AppendLine("declare module '../Stream' {")
                    .IntervalLevel()
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
                            decaCfg.IntervalLevel(2).AppendLine($"get {GetCfgName(fullName)}(): {cfgName};");
                            cfgTypes.Add(cfgName);
                        }
                        else
                            decaStruct.IntervalLevel(2).AppendLine($"Get{cls.Name}(): {cls.Name};");
                    }
                    else if (obj is EnumWrap enm)
                    {
                        GenEnum(enums, enm);
                    }
                }
                configs.AppendLine($"export let _CFG_CLASS_ = [{string.Join(", ", cfgTypes)}];");
                decaCfg.IntervalLevel().AppendLine("}\n}");
                decaStruct.IntervalLevel().AppendLine("}\n}");
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
        static string GetParseName(string fullName)
        {
            if (Setting.RawTypes.Contains(fullName))
                return Util.FirstCharUpper(fullName);
            else if (EnumWrap.IsEnum(fullName))
                return "Int";
            else if (ClassWrap.IsClass(fullName))
            {
                var cls = ClassWrap.Classes[fullName];
                string formatName = GetFormatFullName(cls.FullName);
                string parseName = cls.IsDynamic() ? $"{formatName}Maker" : formatName;
                return parseName;
            }
            else
                return "unknow:" + fullName;
        }
        static string GetParseFunc(FieldWrap field)
        {
            if (field.IsRaw)
                return $"stream.Get{Util.FirstCharUpper(field.FullName)}()";
            else if (field.IsEnum)
                return $"<{CorrectNamespace(field.FullName)}> stream.GetInt()";
            else if (field.IsClass)
            {
                var cls = ClassWrap.Classes[field.FullName];
                string formatName = GetFormatFullName(cls.FullName);
                string parseName = cls.IsDynamic() ? $"{formatName}Maker" : formatName;
                return $"stream.Get{parseName}()";
            }
            else if (field.OriginalType == Setting.LIST)
                return $"stream.Get{Util.FirstCharUpper(field.OriginalType)}('{GetParseName(field.Types[1])}')";
            else if (field.OriginalType == Setting.DICT)
                return $"stream.Get{Util.FirstCharUpper(field.OriginalType)}('{GetParseName(field.Types[1])}', '{GetParseName(field.Types[2])}')";
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

            string cfgName = GetCfgName(cls.Name);
            string relative = cls.FullName.Replace(Setting.DotSplit[0], '/').ToLower();
            string fullName = GetFormatFullName(cls.FullName);
            string parseName = cls.IsDynamic() ? $"{fullName}Maker" : fullName;
            builder.AppendLine($"/** {cls.Name}表数据类 */")
                .AppendLine($"export class {cfgName} extends Stream {{")
                .IntervalLevel().AppendLine($"static readonly relative = '{relative}{Setting.DataFileExt}';")
                .IntervalLevel().AppendLine($"static readonly refence = '{GetCfgName(fullName)}';")
                .IntervalLevel().AppendLine($"private _cfgs: {cls.Name}[] = [];")
                .IntervalLevel().AppendLine("constructor(rootDir: string) {")
                .IntervalLevel(2).AppendLine($"super(rootDir + {cfgName}.relative);")
                .IntervalLevel().AppendLine("}")
                .IntervalLevel().AppendLine($"Get(id: number): {cls.Name} | undefined {{")
                .IntervalLevel(2).AppendLine("if (this.hasLoaded) {")
                .IntervalLevel(3).AppendLine("return this._cfgs[id];")
                .IntervalLevel(2).AppendLine("}")
                .IntervalLevel(2).AppendLine("else {")
                .IntervalLevel(3).AppendLine("this.LoadConfig();")
                .IntervalLevel(3).AppendLine("return this._cfgs[id];")
                .IntervalLevel(2).AppendLine("}")
                .IntervalLevel().AppendLine("}")
                .IntervalLevel().AppendLine("protected ParseConfig() {")
                .IntervalLevel(2).AppendLine($"this._cfgs = this.GetList('{parseName}');")
                .IntervalLevel().AppendLine("}")
                .IntervalLevel().AppendLine("[Symbol.iterator]() { return this._cfgs.values(); }")
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
                builder.IntervalLevel().AppendLine($"/** {cst.Desc} */")
                    .IntervalLevel().AppendLine($"static readonly {cst.Name} = {value};");
            }
            foreach (var item in cls.Fields)
                builder.IntervalLevel().AppendLine($"/** {item.Desc} */")
                    .IntervalLevel().AppendLine($"readonly {item.Name}: {GetType(item)};");
            builder.IntervalLevel().AppendLine("constructor(stream: any) {");
            if (!cls.Inherit.IsEmpty())
                builder.IntervalLevel(2).AppendLine("super(stream);");
            foreach (var item in cls.Fields)
                builder.IntervalLevel(2).AppendLine($"this.{item.Name} = {GetParseFunc(item)};");
            builder.IntervalLevel().AppendLine("}")
                .AppendLine("}");

            if (cls.IsDynamic())
                builder.AppendLine($"Object.defineProperty(Stream.prototype, 'Get{GetParseName(cls.FullName)}', {{")
                   .IntervalLevel().AppendLine("value: (stream: any) => stream[`Get${stream.GetString()}`].bind(stream),")
                   .IntervalLevel().AppendLine("writable: false,")
                   .AppendLine("});");
            builder.AppendLine($"Object.defineProperty(Stream.prototype, 'Get{GetFormatFullName(cls.FullName)}', {{")
                .IntervalLevel().AppendLine($"value: (stream: any) => new {cls.Name}(stream),")
                .IntervalLevel().AppendLine("writable: false,")
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
                    builder.IntervalLevel().AppendLine($"/**{alias}*/");
                builder.IntervalLevel().AppendLine($"{item.Key} = {item.Value},");
            }
            builder.AppendLine("}");
        }
    }
}
