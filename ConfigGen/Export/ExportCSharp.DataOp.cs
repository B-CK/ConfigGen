using System;
using System.IO;
using System.Text;
using ConfigGen.LocalInfo;
using System.Collections.Generic;

namespace ConfigGen.Export
{
    public partial class ExportCSharp
    {
        private const string CLASS_CFG_OBJECT = "CfgObject";
        private const string CLASS_DATA_STREAM = "DataStream";
        private const string CLASS_CFG_MANAGER = "CfgManager";
        private const string FIELD_CONFIG_DIR = "ConfigDir";
        private const string ARG_DATASTREAM = "data";
        private static int _level = 0;

        /// <summary>
        /// 导出CSharp类型Csv存储类
        /// </summary>
        public static void Export_CsvOp()
        {
            StringBuilder builder = new StringBuilder();
            List<string> NameSpaces = new List<string>() { "System", "System.Collections.Generic" };

            //构建Csv存储类基础类CfgObject.cs
            string path = Path.Combine(Values.ExportCSharp, CLASS_CFG_OBJECT + ".cs");
            FillUsingNamespace(builder, NameSpaces);
            FillNameSpace(builder, Values.ConfigRootNode);
            FillClass(builder, "public abstract", CLASS_CFG_OBJECT);
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
                if (baseType.TypeType == TypeType.Class)
                {

                    ClassTypeInfo classType = baseType as ClassTypeInfo;
                    bool isEmpty = string.IsNullOrWhiteSpace(classType.Inherit);
                    if (isEmpty)
                        FillClass(builder, "public", classType.Name, CLASS_CFG_OBJECT);
                    else
                        FillClass(builder, "public", classType.Name, classType.Inherit);

                    string sReadonly = "public readonly";
                    for (int j = 0; j < classType.Fields.Count; j++)
                    {
                        FieldInfo field = classType.Fields[j];
                        switch (field.BaseInfo.TypeType)
                        {
                            case TypeType.Base:
                                FillComments(builder, field.Des);
                                FillField(builder, sReadonly, field.Type, field.Name);
                                break;
                            case TypeType.Class:
                                {
                                    FillComments(builder, field.Des);
                                    FillField(builder, sReadonly, field.Type, field.Name);
                                }
                                break;
                            case TypeType.Enum:
                                FillComments(builder, field.Des);
                                FillField(builder, sReadonly, "int", field.Name);
                                break;
                            case TypeType.List:
                                {
                                    string type = field.Type.Replace("list", "List");
                                    ListTypeInfo listType = field.BaseInfo as ListTypeInfo;
                                    TypeType typeType = TypeInfo.GetTypeType(listType.ItemType);
                                    if (typeType == TypeType.Enum)
                                        type = type.Replace(listType.ItemType, "int");

                                    string initValue = string.Format("new {0}()", type);
                                    FillComments(builder, field.Des);
                                    FillField(builder, sReadonly, type, field.Name, initValue);
                                    break;
                                }
                            case TypeType.Dict:
                                {
                                    string type = field.Type.Replace("dict", "Dictionary");
                                    DictTypeInfo dictType = field.BaseInfo as DictTypeInfo;
                                    if (TypeInfo.GetTypeType(dictType.KeyType) == TypeType.Enum)
                                        type = type.Replace(dictType.KeyType, "int");
                                    TypeType typeType = TypeInfo.GetTypeType(dictType.ValueType);
                                    if (TypeInfo.GetTypeType(dictType.ValueType) == TypeType.Enum)
                                        type = type.Replace(dictType.ValueType, "int");

                                    string initValue = string.Format("new {0}()", type);
                                    FillComments(builder, field.Des);
                                    FillField(builder, sReadonly, type, field.Name, initValue);
                                    break;
                                }
                            case TypeType.None:
                            default:
                                break;
                        }
                    }

                    builder.AppendLine();
                    if (isEmpty)
                        FillFunction(builder, "public", classType.Name, new string[] { CLASS_DATA_STREAM }, new string[] { ARG_DATASTREAM });
                    else
                    {
                        string funcName = string.Format("{0} : base({1})", classType.Name, ARG_DATASTREAM);
                        FillFunction(builder, "public", classType.Name, new string[] { CLASS_DATA_STREAM }, new string[] { ARG_DATASTREAM }, new string[] { ARG_DATASTREAM });
                    }
                    for (int j = 0; j < classType.Fields.Count; j++)
                    {
                        FieldInfo field = classType.Fields[j];
                        FillVariable(builder, field.BaseInfo, field.Type, field.Name, ARG_DATASTREAM);
                    }
                }
                else
                {
                    FillClass(builder, "public sealed", baseType.Name);
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

            DefineDataStream();
            DefineCfgManager();
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
        private static void FillFunction(StringBuilder builder, string modifier, string funcName, string[] types, string[] args, string[] baseArgs = null)
        {
            StringBuilder strbuilder = new StringBuilder();
            FillIntervalLevel(builder);
            if (types == null || args == null || types.Length == 0 || args.Length == 0)
                builder.AppendFormat("{0} {1}()", modifier, funcName);
            else
            {
                string[] array = types.Length < args.Length ? types : args;
                for (int i = 0; i < array.Length; i++)
                {
                    if (i == 0)
                        strbuilder.AppendFormat("{0} {1}", types[i], args[i]);
                    else
                        strbuilder.AppendFormat(", {0} {1}", types[i], args[i]);
                }
                builder.AppendFormat("{0} {1}({2})", modifier, funcName, strbuilder.ToString());
            }
            if (baseArgs != null)
            {
                strbuilder.Clear();
                for (int i = 0; baseArgs != null && i < baseArgs.Length; i++)
                {
                    if (i == 0)
                        strbuilder.Append(baseArgs[i]);
                    else
                        strbuilder.AppendFormat(", {0}", baseArgs[i]);
                }
                builder.AppendFormat(" : base({0})\n", strbuilder.ToString());
            }
            else
            {
                builder.AppendFormat("\n");
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
                    builder.AppendFormat("this.{0} = {1};\n", varName, GetClassVarValue(typeInfo, type, argName));
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
                            builder.AppendFormat("this.{0}.Add({1});\n", varName, GetClassVarValue(itemTypeInfo, listType.ItemType, argName));
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
                            builder.AppendFormat("this.{0}[k] = {1}.Get{2}{3}();\n", varName, argName,
                            Char.ToUpper(dictType.ValueType[0]), dictType.ValueType.Substring(1));
                            break;
                        case TypeType.Class:
                            builder.AppendFormat("this.{0}[k] = {1};\n", varName, GetClassVarValue(vTypeInfo, dictType.ValueType, argName));
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
        private static void FillStart(StringBuilder builder)
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
        private static void FillComments(StringBuilder builder, string comments)
        {
            FillIntervalLevel(builder);
            builder.AppendLine("/// <summary>");
            FillIntervalLevel(builder);
            builder.AppendFormat("/// {0}\n", comments);
            FillIntervalLevel(builder);
            builder.AppendLine("/// <summary>");
        }
        private static string GetClassVarValue(BaseTypeInfo baseType, string type, string argName)
        {
            string value = "";
            ClassTypeInfo classType = baseType as ClassTypeInfo;
            if (classType.HasSubClass == false)
                value = string.Format("new {0}({1})", type, argName);
            else
                value = string.Format("({0}){1}.GetObject({1}.GetString())", type, argName);
            return value;
        }

        /// <summary>
        /// 数据流解析类,各种数据类型解析
        /// </summary>
        private static void DefineDataStream()
        {
            StringBuilder builder = new StringBuilder();
            List<string> NameSpaces = new List<string>() { "System", "System.Text", "System.IO", "System.Reflection" };

            //构建Csv数据解析类DataStream.cs
            string path = Path.Combine(Values.ExportCSharp, CLASS_DATA_STREAM + ".cs");
            FillUsingNamespace(builder, NameSpaces);
            FillNameSpace(builder, Values.ConfigRootNode);
            FillClass(builder, "public", CLASS_DATA_STREAM);

            string fRIndex = "_rIndex";
            string fCIndex = "_cIndex";
            string fRows = "_rows";
            string fColumns = "_columns";
            string[] types = { "string", "Encoding" };
            string[] args = { "path", "encoding" };
            FillFunction(builder, "public", CLASS_DATA_STREAM, types, args);
            FillIntervalLevel(builder);
            builder.AppendFormat("{0} = File.ReadAllLines({1}, {2});\n", fRows, args[0], args[1]);
            FillIntervalLevel(builder);
            builder.AppendFormat("{0} = {1} = 0;\n", fRIndex, fCIndex);
            FillIntervalLevel(builder);
            builder.AppendFormat("{0} = {1}[{2}].Split(\"{3}\".ToCharArray(),  StringSplitOptions.RemoveEmptyEntries);\n",
                fColumns, fRows, fRIndex, Values.CsvSplitFlag);
            FillEnd(builder);
            builder.AppendLine();

            FillIntervalLevel(builder);
            builder.AppendFormat("public int Count {{ get {{ return {0}.Length; }} }}\n", fRows);
            builder.AppendLine();

            FillFunction(builder, "public int", "GetInt", null, null);
            FillIntervalLevel(builder);
            builder.AppendLine("int result;");
            FillIntervalLevel(builder);
            builder.AppendLine("int.TryParse(Next(), out result);");
            FillIntervalLevel(builder);
            builder.AppendLine("return result;");
            FillEnd(builder);

            FillFunction(builder, "public long", "GetLong", null, null);
            FillIntervalLevel(builder);
            builder.AppendLine("long result;");
            FillIntervalLevel(builder);
            builder.AppendLine("long.TryParse(Next(), out result);");
            FillIntervalLevel(builder);
            builder.AppendLine("return result;");
            FillEnd(builder);

            FillFunction(builder, "public float", "GetFloat", null, null);
            FillIntervalLevel(builder);
            builder.AppendLine("float result;");
            FillIntervalLevel(builder);
            builder.AppendLine("float.TryParse(Next(), out result);");
            FillIntervalLevel(builder);
            builder.AppendLine("return result;");
            FillEnd(builder);

            FillFunction(builder, "public bool", "GetBool", null, null);
            FillIntervalLevel(builder);
            builder.AppendLine("string v = Next();");
            FillIntervalLevel(builder);
            builder.AppendLine("if (string.IsNullOrEmpty(v)) return false;");
            FillIntervalLevel(builder);
            builder.AppendLine("return !v.Equals(\"0\");");
            FillEnd(builder);

            FillFunction(builder, "public string", "GetString", null, null);
            FillIntervalLevel(builder);
            builder.AppendLine("return Next();");
            FillEnd(builder);

            types = new string[] { "string" };
            args = new string[] { "fullName" };
            FillFunction(builder, "public " + CLASS_CFG_OBJECT, "GetObject", types, args);
            FillIntervalLevel(builder);
            builder.AppendFormat("Type type = Type.GetType({0});\n", args[0]);
            FillIntervalLevel(builder);
            builder.AppendFormat("if (type == null)\n");
            FillStart(builder);
            FillIntervalLevel(builder);
            builder.AppendFormat("UnityEngine.Debug.LogErrorFormat(\"DataStream 解析{{0}}类型失败!\", {0});\n", args[0]);
            FillEnd(builder);
            FillIntervalLevel(builder);
            builder.AppendFormat("ConstructorInfo constructor = type.GetConstructor(new Type[] {{ type }});\n");
            FillIntervalLevel(builder);
            builder.AppendFormat("return ({0})constructor.Invoke(new object[] {{ this }});\n", CLASS_CFG_OBJECT);
            FillEnd(builder);
            builder.AppendLine();
            builder.AppendLine();

            FillField(builder, "private", "int", fRIndex);
            FillField(builder, "private", "int", fCIndex);
            FillField(builder, "private", "string[]", fRows);
            FillField(builder, "private", "string[]", fColumns);
            builder.AppendLine();
            FillFunction(builder, "private void", "NextRow", null, null);
            FillIntervalLevel(builder);
            builder.AppendFormat("if({0} >= {1}.Length) return;\n", fRIndex, fRows);
            FillIntervalLevel(builder);
            builder.AppendFormat("{0}++;\n", fRIndex);
            FillIntervalLevel(builder);
            builder.AppendFormat("{0} = 0;\n", fCIndex);
            FillIntervalLevel(builder);
            builder.AppendFormat("{0} = {1}[{2}].Split(\"{3}\".ToCharArray(),  StringSplitOptions.RemoveEmptyEntries);\n",
                fColumns, fRows, fRIndex, Values.CsvSplitFlag);
            FillEnd(builder);
            builder.AppendLine();

            FillFunction(builder, "private string", "Next", null, null);
            FillIntervalLevel(builder);
            builder.AppendFormat("if({0} >= {1}.Length) return string.Empty;\n", fCIndex, fColumns);
            FillIntervalLevel(builder);
            builder.AppendFormat("{0}++;\n", fCIndex);
            FillIntervalLevel(builder);
            builder.AppendFormat("return {0}[{1}];\n", fColumns, fRIndex, Values.CsvSplitFlag);
            FillEnd(builder);
            builder.AppendLine();

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
            List<string> NameSpaces = new List<string>() { "System", "System.Text", "System.Collections.Generic" };

            //构建Csv数据解析类DataStream.cs
            string path = Path.Combine(Values.ExportCSharp, CLASS_CFG_MANAGER + ".cs");
            FillUsingNamespace(builder, NameSpaces);
            FillNameSpace(builder, Values.ConfigRootNode);
            FillClass(builder, "public", CLASS_CFG_MANAGER);

            FillComments(builder, "配置文件文件夹路径");
            FillField(builder, "public static", "string", FIELD_CONFIG_DIR);
            builder.AppendLine();

            List<string> dictName = new List<string>();
            List<BaseTypeInfo> typeInfos = new List<BaseTypeInfo>();
            typeInfos.AddRange(LocalInfoManager.Instance.TypeInfoLib.ClassInfos);
            for (int i = 0; i < typeInfos.Count; i++)
            {
                BaseTypeInfo baseType = typeInfos[i];
                if (baseType.TypeType != TypeType.Class)
                    continue;
                ClassTypeInfo classType = baseType as ClassTypeInfo;
                if (classType.IndexField == null || string.IsNullOrWhiteSpace(classType.IndexField.Type))
                    continue;

                FillIntervalLevel(builder);
                builder.AppendFormat("public static readonly Dictionary<{0}, {1}> {2} = new Dictionary<{0}, {1}>();\n",
                    classType.IndexField.Type, classType.GetClassName(), classType.Name);
                dictName.Add(classType.Name);
            }
            builder.AppendLine();

            //加载单个配置
            string[] types = { "string", "Func<DataStream, T>" };
            string[] args = { "path", "constructor" };
            FillComments(builder, "constructor参数为指定类型的构造函数");
            FillFunction(builder, "public static List<T>", "Load<T>", types, args);
            FillIntervalLevel(builder);
            builder.AppendFormat("{0} data = new {0}({1}, Encoding.UTF8);\n", CLASS_DATA_STREAM, args[0]);
            FillIntervalLevel(builder);
            builder.AppendFormat("List<T> list = new List<T>();\n");
            FillIntervalLevel(builder);
            builder.AppendFormat("for (int i = 0; i < data.Count; i++)\n");
            FillStart(builder);
            FillIntervalLevel(builder);
            builder.AppendFormat("list.Add({0}(data));\n", args[1]);
            FillEnd(builder);
            FillIntervalLevel(builder);
            builder.AppendFormat("return list;\n");
            FillEnd(builder);
            builder.AppendLine();

            //加载所有配置
            FillFunction(builder, "public static void", "LoadAll", null, null);
            for (int i = 0; i < typeInfos.Count; i++)
            {
                BaseTypeInfo baseType = typeInfos[i];
                if (baseType.TypeType != TypeType.Class)
                    continue;
                ClassTypeInfo classType = baseType as ClassTypeInfo;
                if (classType.IndexField == null || string.IsNullOrWhiteSpace(classType.IndexField.Type))
                    continue;

                FillIntervalLevel(builder);
                string rel = classType.GetClassName().Replace(".", "/") + Values.CsvFileExt;
                builder.AppendFormat("var {0}s = Load({1} + \"{2}\", (d) => new {3}(d));\n",
                    classType.Name.ToLower(), FIELD_CONFIG_DIR, rel, classType.GetClassName());
                FillIntervalLevel(builder);
                builder.AppendFormat("{0}s.ForEach(v => {1}.Add(v.{2}, v));\n",
                    classType.Name.ToLower(), classType.Name, classType.IndexField.Name);
            }
            FillEnd(builder);
            builder.AppendLine();

            FillFunction(builder, "public static void", "Clear", null, null);
            for (int i = 0; i < dictName.Count; i++)
            {
                FillIntervalLevel(builder);
                builder.AppendFormat("{0}.Clear();\n", dictName[i]);
            }
            FillEnd(builder);
            builder.AppendLine();

            FillEndAll(builder);
            Util.SaveFile(path, builder.ToString());
            builder.Clear();
        }

        //仅限Xml格式
        //读 Csv - OK:是否有被继承
        //写 Csv - OK:是否有继承类,Excel中禁止使用被继承过的Class
        //读 Xml - OK:属性表达类型
        //写 Xml - OK:基类反射查询子类
    }
}
