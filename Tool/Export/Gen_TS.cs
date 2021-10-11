using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool.Wrap;

namespace Tool.Export
{
    public class Gen_TS
    {
        public static void Gen()
        {
            List<string> imports = new List<string>()
            {
                "import { Stream } from 'Config/Stream';"
            };

            var namespaces = Util.GetNamespaceClasses();
            foreach (var item in namespaces)
            {
                var builder = new StringBuilder();
                builder.AppendLine(string.Join("\n", imports));

                var configs = new StringBuilder();
                var structs = new StringBuilder();
                var decaCfg = new StringBuilder()
                    .AppendLine("declare module '../Config' {")
                    .IntervalLevel()
                    .AppendLine("interface ConfigMgr {");
                var decaStruct = new StringBuilder()
                    .AppendLine("declare module '../Stream' {")
                    .IntervalLevel()
                    .AppendLine("interface StreamBase {");
                
                foreach (var cls in item.Value)
                {
                    GenConfig(configs, cls);
                    GenDataStruct(structs, cls);
                    string cfgName = GetCfgName(cls.Name);
                    if (cls.IsConfig())
                        decaCfg.AppendLine($"get {cfgName}(): {cfgName};");
                    else
                        decaStruct.AppendLine($"Get{cls.Name}(): {cls.Name};");
                    //LinkModules(cls);
                    //引入其他模块
                    //格式module.class
                }
                decaCfg.IntervalLevel().AppendLine("}\n}");
                decaStruct.IntervalLevel().AppendLine("}\n}");
                string path = Path.Combine(Setting.CSDir, item.Key.Replace(Setting.DotSplit.ToString(), "") + ".ts");

               
                builder.AppendLine("/**************************************** 数据配置表 *****************************************/")
                    .AppendLine(configs.ToString())
                    .AppendLine("/**************************************** 数据结构定义 *****************************************/")
                    .AppendLine(structs.ToString())
                    .AppendLine("/**************************************** 声明与导出 *****************************************/")
                    .AppendLine(decaCfg.ToString())
                    .AppendLine(decaStruct.ToString());
                Util.SaveFile(path, configs.ToString());
            }
        }

        static string GetCfgName(string name) => $"{name}Cfg";
        static string GetType(FieldWrap field)
        {
            if (field.IsRaw || field.IsEnum || field.IsClass)
            {
                string type = field.OriginalType;
                switch (field.OriginalType)
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
                return type;
            }
            if (field.OriginalType == Setting.LIST)
                return $"{field.Types[1]}[]";
            else if (field.OriginalType == Setting.DICT)
                return $"Map<{field.Types[1]}, {field.Types[2]}>";
            else
                return "unknow";
        }
        static List<string> LinkModules(ClassWrap cls)
        {
            var list = new List<string>();
            foreach (var item in cls.Consts)
            {
                if (EnumWrap.IsEnum(item.FullName) && item.FullName.IndexOf(cls.Namespace) != 0)
                    list.Add(item.FullName);
            }
            foreach (var item in cls.Fields)
            {
                if ((item.IsEnum || item.IsClass) && item.FullName.IndexOf(cls.Namespace) != 0)
                    list.Add(item.FullName);
            }
            return list;
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
            string parseName = cls.IsDynamic() ? $"{cls.Name}Maker" : cls.Name;
            builder.AppendLine($"class {cfgName} extends Stream")
                .IntervalLevel().AppendLine($"static readonly relative = '{relative}';")
                .IntervalLevel().AppendLine($"private _cfgs: {cls.Name}[];")
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
            string extends = cls.Inherit.IsEmpty() ? " " : $" extends {cls.Inherit} ";
            builder.AppendLine($"class {cls.Name}{extends}{{");
            for (int j = 0; j < cls.Consts.Count; j++)
            {
                ConstWrap constant = cls.Consts[j];
                if (!Util.MatchGroups(constant.Group)) continue;
                string value = constant.Value;
                switch (constant.FullName)
                {
                    case Setting.BOOL:
                        value = value.ToLower();
                        break;
                    case Setting.STRING:
                        value = string.Format("'{0}'", value);
                        break;
                    default:
                        if (EnumWrap.IsEnum(constant.FullName))
                            value = EnumWrap.Enums[constant.FullName].GetValue(constant.Value);
                        break;
                }
                builder.IntervalLevel().AppendLine($"static readonly {constant.Name} = {value};");
            }
            foreach (var item in cls.Fields)
                builder.IntervalLevel().AppendLine($"readonly {item.Name}: {GetType(item)};");
            builder.IntervalLevel().AppendLine("constructor(stream: any) {");
            if (!cls.Inherit.IsEmpty())
                builder.IntervalLevel(2).AppendLine("super(stream);");
            foreach (var item in cls.Fields)
                builder.IntervalLevel(2).AppendLine($"this.{item.Name} = stream.Get{item.OriginalType}();");
            builder.IntervalLevel().AppendLine("}")
                .AppendLine("}");

            if (cls.IsDynamic())
                builder.AppendLine($"Object.defineProperty(Stream.prototype, 'Get{cls.Name}Maker', {{")
                   .IntervalLevel().AppendLine("value: (stream: any) => stream[`Get${stream.GetString()}`].bind(stream),")
                   .IntervalLevel().AppendLine("writable: false,")
                   .AppendLine("});");
            builder.AppendLine($"Object.defineProperty(Stream.prototype, 'Get{cls.Name}', {{")
                .IntervalLevel().AppendLine($"value: (stream: any) => new {cls.Name}(stream),")
                .IntervalLevel().AppendLine("writable: false,")
                .AppendLine("});");
        }
    }
}
