using ConfigGen.LocalInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigGen.Export
{
    public partial class ExportCSharp
    {
        private const string CFGOBJECT = "CfgObject";
        private const string DATASTREAM = "DataStream";
        private const string CFGMANAGER = "CfgManager";
        private static int _level = 0;

        /// <summary>
        /// 导出CSharp类型Csv存储类
        /// </summary>
        public static void Export_CsvOp()
        {
            StringBuilder builder = new StringBuilder();
            List<string> NameSpaces = new List<string>() { "System", "System.Collections.Generic" };

            //构建Csv存储类基础类CfgObject.cs
            string path = Path.Combine(Values.ExportCSharp, CFGOBJECT + ".cs");
            FillUsingNamespace(builder, NameSpaces);
            FillNameSpace(builder, Values.ConfigRootNode);
            FillClass(builder, "public abstract", CFGOBJECT);
            FillEndAll(builder);
            Util.SaveFile(path, builder.ToString());
            builder.Clear();
            NameSpaces.Add(Values.ConfigRootNode);

            //构造自定义类型
            List<BaseTypeInfo> typeInfos = new List<BaseTypeInfo>();
            typeInfos.AddRange(LocalInfoManager.Instance.TypeInfoLib.ClassInfos);
            typeInfos.AddRange(LocalInfoManager.Instance.TypeInfoLib.EnumInfos);
            for (int i = 0; i < typeInfos.Count; i++)
            {
                BaseTypeInfo baseType = typeInfos[i];
                if (baseType.TypeType != TypeType.Class && baseType.TypeType != TypeType.Enum)
                    continue;

                FillUsingNamespace(builder, NameSpaces);
                FillNameSpace(builder, string.Format("{0}.{1}", Values.ConfigRootNode, baseType.NamespaceName));
                FillClass(builder, "public sealed", baseType.Name, CFGOBJECT);
                if (baseType.TypeType == TypeType.Class)
                {
                    ClassTypeInfo classType = baseType as ClassTypeInfo;

                    string sReadonly = "public readonly";
                    for (int j = 0; j < classType.Fields.Count; j++)
                    {
                        FieldInfo field = classType.Fields[j];
                        switch (field.BaseInfo.TypeType)
                        {
                            case TypeType.Base:
                            case TypeType.Class:
                                FillField(builder, sReadonly, field.Type, field.Name);
                                break;
                            case TypeType.Enum:
                                FillField(builder, sReadonly, "int", field.Name);
                                break;
                            case TypeType.List:
                                {
                                    string type = field.Type.Replace("list", "List");
                                    ListTypeInfo listType = field.BaseInfo as ListTypeInfo;
                                    if (TypeInfo.GetTypeType(listType.ItemType) == TypeType.Enum)
                                        type = type.Replace(listType.ItemType, "int");

                                    string initValue = string.Format("new {0}()", type);
                                    FillField(builder, sReadonly, type, field.Name, initValue);
                                    break;
                                }
                            case TypeType.Dict:
                                {
                                    string type = field.Type.Replace("dict", "Dictionary");
                                    DictTypeInfo dictType = field.BaseInfo as DictTypeInfo;
                                    if (TypeInfo.GetTypeType(dictType.KeyType) == TypeType.Enum)
                                        type = type.Replace(dictType.KeyType, "int");
                                    if (TypeInfo.GetTypeType(dictType.ValueType) == TypeType.Enum)
                                        type = type.Replace(dictType.ValueType, "int");

                                    string initValue = string.Format("new {0}()", type);
                                    FillField(builder, sReadonly, type, field.Name, initValue);
                                    break;
                                }
                            case TypeType.None:
                            default:
                                break;
                        }
                    }

                    string argName = "data";
                    builder.AppendLine();
                    FillConstructor(builder, classType.Name, DATASTREAM, argName);
                    for (int j = 0; j < classType.Fields.Count; j++)
                    {
                        FieldInfo field = classType.Fields[j];
                        FillVariable(builder, field.BaseInfo, field.Type, field.Name, argName);
                    }
                }
                else
                {
                    EnumTypeInfo enumType = baseType as EnumTypeInfo;
                    for (int j = 0; j < enumType.KeyValuePair.Count; j++)
                    {
                        EnumKeyValue keyValue = enumType.KeyValuePair[j];
                        FillField(builder, "public const", "int", keyValue.Key, keyValue.Value);
                    }
                }

                FillEndAll(builder);

                string relDir = Path.GetDirectoryName(baseType.RelPath);
                path = Path.Combine(Values.ExportCSharp, relDir, baseType.Name + ".cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }

        }
        private static void FillIntervalLevel(StringBuilder builder)
        {
            for (int i = 0; i < _level; i++)
                builder.Append("\t");
        }
        private static void FillUsingNamespace(StringBuilder builder, List<string> vs)
        {
            for (int i = 0; i < vs.Count; i++)
                builder.AppendFormat("using {0};\n", vs[i]);
        }
        private static void FillNameSpace(StringBuilder builder, string name)
        {
            builder.AppendLine();
            builder.AppendFormat("namespace {0}\n", name);
            FillStart(builder);
        }
        private static void FillClass(StringBuilder builder, string modifier, string className, string inhert = null)
        {
            FillIntervalLevel(builder);
            if (string.IsNullOrWhiteSpace(inhert))
                builder.AppendFormat("{0} class {1}\n", modifier, className);
            else
                builder.AppendFormat("{0} class {1} : {2}\n", modifier, className, inhert);
            FillStart(builder);
        }
        private static void FillField(StringBuilder builder, string modifier, string type, string fieldName, string initValue = null)
        {
            FillIntervalLevel(builder);
            if (!string.IsNullOrWhiteSpace(initValue))
                builder.AppendFormat("{0} {1} {2} = {3};\n", modifier, type, fieldName, initValue);
            else
                builder.AppendFormat("{0} {1} {2};\n", modifier, type, fieldName);
        }
        private static void FillConstructor(StringBuilder builder, string funcName, string type, string arg)
        {
            FillIntervalLevel(builder);
            builder.AppendFormat("public {0}({1} {2})\n", funcName, type, arg);
            FillStart(builder);
        }
        private static void FillFunction(StringBuilder builder, string modifier, string funcName, string[] types, string[] args)
        {
            FillIntervalLevel(builder);
            if (types == null || args == null || types.Length == 0 || args.Length == 0)
                builder.AppendFormat("{0} {1}()\n", modifier, funcName);
            else
            {
                string[] array = types.Length < args.Length ? types : args;
                for (int i = 0; i < array.Length; i++)
                    builder.AppendFormat("{0} {1}({2} {3})\n", modifier, funcName, types[i], args[i]);
            }

            FillStart(builder);
        }
        private static void FillVariable(StringBuilder builder, BaseTypeInfo typeInfo, string type, string varName, string argName)
        {
            FillIntervalLevel(builder);
            TypeType typeType = typeInfo.TypeType;
            switch (typeType)
            {
                case TypeType.Base:
                    string baseFunc = string.Format("{0}.Get{1}{2}()", argName, Char.ToUpper(type[0]), type.Substring(1));
                    builder.AppendFormat("this.{0} = {1};\n", varName, baseFunc);
                    break;
                case TypeType.Enum:
                    builder.AppendFormat("this.{0} = {1}.GetInt();\n", varName, argName);
                    break;
                case TypeType.Class:
                    builder.AppendFormat("this.{0} =  new {0}({1});\n", varName, type, argName);
                    break;
                case TypeType.List:
                    builder.AppendFormat("for (int n = {0}.GetInt(); n-- > 0; )\n", argName);
                    FillStart(builder);
                    FillIntervalLevel(builder);
                    ListTypeInfo listType = typeInfo as ListTypeInfo;
                    BaseTypeInfo itemTypeInfo = TypeInfo.GetTypeInfo(listType.ItemType);
                    switch (itemTypeInfo.TypeType)
                    {
                        case TypeType.Base:
                            builder.AppendFormat("this.{0}.Add({1}.Get{2}{3}());\n", varName, argName,
                                Char.ToUpper(listType.ItemType[0]), listType.ItemType.Substring(1));
                            break;
                        case TypeType.Class:
                            builder.AppendFormat("this.{0}.Add(new {1}({2}));\n", varName, listType.ItemType, argName);
                            break;
                        case TypeType.Enum:
                            builder.AppendFormat("this.{0}.Add({1}.GetInt());\n", varName, argName);
                            break;
                        case TypeType.List:
                        case TypeType.Dict:
                        case TypeType.None:
                        default:
                            break;
                    }
                    FillEnd(builder);
                    break;
                case TypeType.Dict:
                    builder.AppendFormat("for (int n = {0}.GetInt(); n-- > 0;)\n", argName);
                    FillStart(builder);
                    FillIntervalLevel(builder);
                    DictTypeInfo dictType = typeInfo as DictTypeInfo;
                    if (TypeInfo.GetTypeType(dictType.KeyType) == TypeType.Base)
                        builder.AppendFormat("{0} k = {1}.Get{2}{3}();\n", dictType.KeyType, argName,
                            Char.ToUpper(dictType.KeyType[0]), dictType.KeyType.Substring(1));
                    else if (TypeInfo.GetTypeType(dictType.KeyType) == TypeType.Enum)
                        builder.AppendFormat("{0} k = {1}.GetInt();\n", "int", argName);

                    FillIntervalLevel(builder);
                    BaseTypeInfo vTypeInfo = TypeInfo.GetTypeInfo(dictType.ValueType);
                    switch (vTypeInfo.TypeType)
                    {
                        case TypeType.Base:
                            builder.AppendFormat("this.{0}[k] = new {1}.Get{2}{3}();\n", varName, argName,
                            Char.ToUpper(dictType.ValueType[0]), dictType.ValueType.Substring(1));
                            break;
                        case TypeType.Class:
                            builder.AppendFormat("this.{0}[k] = new {1}({2});\n", varName, dictType.ValueType, argName);
                            break;
                        case TypeType.Enum:
                            builder.AppendFormat("this.{0}[k] = {1}.GetInt();\n", varName, argName);
                            break;
                        case TypeType.List:
                        case TypeType.Dict:
                        case TypeType.None:
                        default:
                            break;
                    }
                    FillEnd(builder);
                    break;
                case TypeType.None:
                default:
                    break;
            }

        }
        public static void FillStart(StringBuilder builder)
        {
            FillIntervalLevel(builder);
            builder.AppendLine("{");
            _level++;
        }
        private static void FillEnd(StringBuilder builder)
        {
            _level--;
            FillIntervalLevel(builder);
            builder.AppendLine("}");
        }
        private static void FillEndAll(StringBuilder builder)
        {
            for (int i = 0; i <= _level + 1; i++)
                FillEnd(builder);
        }

        /// <summary>
        /// 数据流解析类,各种数据类型解析
        /// </summary>
        private static void DefineDataStream()
        {
            StringBuilder builder = new StringBuilder();
            List<string> NameSpaces = new List<string>() { "System", "System.Collections.Generic" };

            //构建Csv数据解析类DataStream.cs
            string path = Path.Combine(Values.ExportCSharp, DATASTREAM + ".cs");
            FillUsingNamespace(builder, NameSpaces);
            FillNameSpace(builder, Values.ConfigRootNode);
            FillClass(builder, "public", DATASTREAM);


            FillEndAll(builder);
            Util.SaveFile(path, builder.ToString());
            builder.Clear();
        }
        /// <summary>
        /// 配置管理类
        /// </summary>
        private static void DefineCfgManager()
        {
            StringBuilder builder = new StringBuilder();
            List<string> NameSpaces = new List<string>() { "System", "System.Collections.Generic" };

            //构建Csv数据解析类DataStream.cs
            string path = Path.Combine(Values.ExportCSharp, CFGMANAGER + ".cs");
            FillUsingNamespace(builder, NameSpaces);
            FillNameSpace(builder, Values.ConfigRootNode);
            FillClass(builder, "public", CFGMANAGER);

            List<BaseTypeInfo> typeInfos = new List<BaseTypeInfo>();
            typeInfos.AddRange(LocalInfoManager.Instance.TypeInfoLib.ClassInfos);
            for (int i = 0; i < typeInfos.Count; i++)
            {
                BaseTypeInfo baseType = typeInfos[i];
                if (baseType.TypeType != TypeType.Class)
                    continue;
                ClassTypeInfo classType = baseType as ClassTypeInfo;
                if (string.IsNullOrWhiteSpace(classType.IndexType))
                    continue;

                FillIntervalLevel(builder);
                builder.AppendFormat("public static readonly Dictionary<{0}, {1}> {2} = new Dictionary<{0}, {1}>();\n",
                    classType.IndexType, classType.GetClassName(), classType.Name);
            }
            builder.AppendLine();

            //加载单个配置
            string[] types = { "string", "Encoding" };
            string[] args = { "path", "encoding" };
            FillFunction(builder, "public static List<T>", "Load<T>", types, args);
            FillIntervalLevel(builder);
            builder.AppendFormat("{0} data = new {0}({1}, {2});\n", CFGMANAGER, args[0], args[1]);
            FillIntervalLevel(builder);
            builder.AppendFormat("List<T> list = new List<T>();\n");
            FillIntervalLevel(builder);
            builder.AppendFormat("for (int i = 0; i < data.Count; i++)\n");
            FillStart(builder);
            FillIntervalLevel(builder);
            builder.AppendFormat("string line = data.GetLine()\n");
            FillIntervalLevel(builder);
            builder.AppendFormat("list.Add(new T(line));\n");
            FillEnd(builder);
            FillIntervalLevel(builder);
            builder.AppendFormat("return list;\n");
            FillEnd(builder);

            //加载所有配置
            types = null;
            args = null;
            FillFunction(builder, "public static void", "LoadAll", types, args);
            for (int i = 0; i < typeInfos.Count; i++)
            {
                BaseTypeInfo baseType = typeInfos[i];
                if (baseType.TypeType != TypeType.Class)
                    continue;
                ClassTypeInfo classType = baseType as ClassTypeInfo;
                if (string.IsNullOrWhiteSpace(classType.IndexType))
                    continue;

                FillIntervalLevel(builder);
                builder.AppendFormat("public static readonly Dictionary<{0}, {1}> {2} = new Dictionary<{0}, {1}>();\n",
                    classType.IndexType, classType.GetClassName(), classType.Name);
            }
            FillEnd(builder);

            FillEndAll(builder);
            Util.SaveFile(path, builder.ToString());
            builder.Clear();
        }



    }
}
