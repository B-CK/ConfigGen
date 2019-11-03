using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wrap;

namespace Tool.Export
{
    /// <summary>
    /// FlatBuffers相关导出
    /// 不支持键值对结构
    /// </summary>
    public static class FlatBuffers
    {
        public static void ExportSchema()
        {
            Dictionary<string, List<object>> namespacesDict = new Dictionary<string, List<object>>();
            foreach (var item in ClassWrap.Classes)
            {
                if (namespacesDict.ContainsKey(item.Value.Namespace))
                    namespacesDict[item.Value.Namespace].Add(item.Value);
                else
                    namespacesDict.Add(item.Value.Namespace, new List<object>() { item.Value });
            }
            foreach (var item in EnumWrap.Enums)
            {
                if (namespacesDict.ContainsKey(item.Value.Namespace))
                    namespacesDict[item.Value.Namespace].Add(item.Value);
                else
                    namespacesDict.Add(item.Value.Namespace, new List<object>() { item.Value });
            }

            string ReturnNamespace(string type, string selfNamespace)
            {
                if (ClassWrap.Classes.ContainsKey(type) && selfNamespace != ClassWrap.Classes[type].Namespace)
                    return ClassWrap.Classes[type].Namespace;
                else if (EnumWrap.Enums.ContainsKey(type) && selfNamespace != EnumWrap.Enums[type].Namespace)
                    return EnumWrap.Enums[type].Namespace;
                else
                    return "";
            }

            //搜索需要引用的命名空间
            Dictionary<string, StringBuilder> references = new Dictionary<string, StringBuilder>();
            foreach (var item in namespacesDict)
            {
                var list = item.Value;
                var refs = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    if (!(list[i] is ClassWrap)) continue;

                    var cls = list[i] as ClassWrap;
                    var fields = cls.Fields;
                    for (int k = 0; k < fields.Count; k++)
                    {
                        var types = fields[i].Types;
                        string selfNamespace = fields[i].Host.Namespace;
                        switch (types.Length)
                        {
                            case 1:
                                refs.AppendLine($"include \"{ReturnNamespace(types[0], selfNamespace)}.fbs\";");
                                break;
                            case 2:
                                refs.AppendLine($"include \"{ReturnNamespace(types[1], selfNamespace)}.fbs\";");
                                break;
                            case 3:
                                refs.AppendLine($"include \"{ReturnNamespace(types[2], selfNamespace)}.fbs\";");
                                break;
                            default:
                                Util.LogError($"字段类型未知:{fields[i]}");
                                break;
                        }
                    }
                }
                references.Add(item.Key, refs);
            }

            Dictionary<string, StringBuilder> dict = new Dictionary<string, StringBuilder>();
            foreach (var item in namespacesDict)
            {
                dict.Add(item.Key, new StringBuilder(references[item.Key].ToString()));
                var list = item.Value;
                for (int i = 0; i < list.Count; i++)
                {
                    switch (list[i])
                    {
                        case ClassWrap cls:
                            if (ConfigWrap.IsConfig(cls.FullType))
                            {
                                dict.Add(cls.FullType, new StringBuilder(references[item.Key].ToString()));
                                dict[cls.FullType].AppendLine($"include \"{cls.Namespace}.fbs\";");
                                dict[cls.FullType].AppendLine(GenClass(cls));
                                dict[cls.FullType].AppendLine($"root_type {cls.Name};");
                            }
                            else
                                dict[item.Key].AppendLine(GenClass(cls));
                            break;
                        case EnumWrap enm:
                            dict[item.Key].AppendLine(GenEnum(enm));
                            break;
                        default:
                            Util.LogError($"未知类型:{list[i]}");
                            break;
                    }
                }
            }

            //生成fbs文件
            int count = dict.Count;
            int num = 0;
            foreach (var item in dict)
            {
                var path = Path.Combine(Setting.Output, item.Key + ".fbs");
                Util.SaveFile(path, item.Value.ToString());
                Util.Log($"[{++num}/{count}]{path}");
            }
            Util.Log("配置文件生成完毕...");
        }

        static string GenClass(ClassWrap cls)
        {
            StringBuilder builder = new StringBuilder();
            if (cls.IsDynamic())
            {
                string exts = string.Join(" ,", cls.Chindren);
                builder.AppendLine($"union __{cls.Name}ExtType {{{exts}}}");
            }
            builder.AppendLine($"table {cls.Name} {{");
            var fields = cls.Fields;
            for (int i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                switch (field.Types.Length)
                {
                    case 1:
                        builder.AppendLine($"\t{field.Name}:{field.FullType};");
                        break;
                    case 2:
                        builder.AppendLine($"\t{field.Name}:[{field.Types[1]}];");
                        break;
                    default:
                        Util.LogError($"未知类型:{field}");
                        break;
                }
            }
            if (cls.IsDynamic())
            {
                builder.AppendLine($"\t__{cls.Name}Ext:__{cls.Name}ExtExtType;");
            }
            builder.AppendLine("}}");
            return builder.ToString();
        }

        static string GenEnum(EnumWrap enm)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"enum {enm.Name} {{");
            foreach (var item in enm.Values)
            {
                builder.AppendLine($"\t{item.Key} = {item.Value},");
            }
            builder.AppendLine("}}");
            return builder.ToString();
        }
    }
}
