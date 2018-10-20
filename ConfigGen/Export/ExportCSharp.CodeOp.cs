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
        private static readonly List<string> CsvNameSpaces = new List<string>() { "System", "System.Collections.Generic", Values.ConfigRootNode };

        class Base
        {
            public const string Int = "int";
            public const string Long = "long";
            public const string Float = "float";
            public const string Bool = "bool";
            public const string String = "string";
            public const string Void = "void";
            public const string Object = "object";
        }

        /// <summary>
        /// 导出CSharp类型Csv存储类
        /// </summary>
        public static void Export_CsvOp()
        {
            StringBuilder builder = new StringBuilder();

            //构建Csv存储类基础类CfgObject.cs
            string path = Path.Combine(Values.ExportCode, CLASS_CFG_OBJECT + ".cs");
            CodeWriter.UsingNamespace(builder, new List<string>(CsvNameSpaces));
            CodeWriter.NameSpace(builder, Values.ConfigRootNode);
            CodeWriter.ClassBase(builder, CodeWriter.Public, CodeWriter.Abstract, CLASS_CFG_OBJECT);
            CodeWriter.EndAll(builder);
            Util.SaveFile(path, builder.ToString());
            builder.Clear();


            GenClassScripts();
            DefineDataStream();
            DefineCfgManager();
        }

        //构造自定义类型
        private static void GenClassScripts()
        {
            StringBuilder builder = new StringBuilder();
            List<BaseTypeInfo> typeInfos = new List<BaseTypeInfo>();
            typeInfos.AddRange(TypeInfo.Instance.ClassInfos);
            typeInfos.AddRange(TypeInfo.Instance.EnumInfos);
            for (int i = 0; i < typeInfos.Count; i++)
            {
                BaseTypeInfo baseType = typeInfos[i];
                if (baseType.EType != TypeType.Class && baseType.EType != TypeType.Enum)
                    continue;

                CodeWriter.UsingNamespace(builder, CsvNameSpaces);
                builder.AppendLine();
                CodeWriter.NameSpace(builder, Util.GetFullNamespace(Values.ConfigRootNode, baseType.NamespaceName));
                if (baseType.EType == TypeType.Class)
                {
                    ClassTypeInfo classInfo = baseType as ClassTypeInfo;
                    if (classInfo.Inherit == null)
                        CodeWriter.ClassChild(builder, CodeWriter.Public, classInfo.Name, CLASS_CFG_OBJECT);
                    else
                    {
                        string fullType = CodeWriter.GetFullNamespace(Values.ConfigRootNode, classInfo.Inherit.GetFullName());
                        CodeWriter.ClassChild(builder, CodeWriter.Public, classInfo.Name, fullType);
                    }

                    //常量字段
                    for (int j = 0; j < classInfo.Consts.Count; j++)
                    {
                        ConstInfo field = classInfo.Consts[j];
                        CodeWriter.Comments(builder, field.Des);
                        string value = field.Value;
                        switch (field.Type)
                        {
                            case Base.Float:
                                value = string.Format("{0}f", field.Value);
                                break;
                            case Base.String:
                                value = string.Format("@\"{0}\"", field.Value);
                                break;
                        }
                        CodeWriter.FieldInit(builder, CodeWriter.Public, CodeWriter.Const, field.Type, field.Name, value);
                    }

                    //普通字段
                    for (int j = 0; j < classInfo.Fields.Count; j++)
                    {
                        FieldInfo field = classInfo.Fields[j];
                        switch (field.BaseInfo.EType)
                        {
                            case TypeType.Base:
                                CodeWriter.Comments(builder, field.Des);
                                CodeWriter.Field(builder, CodeWriter.Public, CodeWriter.Readonly, field.Type, field.Name);
                                break;
                            case TypeType.Class:
                                {
                                    CodeWriter.Comments(builder, field.Des);
                                    string fullType = CodeWriter.GetFullNamespace(Values.ConfigRootNode, field.Type);
                                    CodeWriter.Field(builder, CodeWriter.Public, fullType, field.Name);
                                }
                                break;
                            case TypeType.Enum:
                                CodeWriter.Comments(builder, field.Des);
                                CodeWriter.Field(builder, CodeWriter.Public, CodeWriter.Readonly, Base.Int, field.Name);
                                break;
                            case TypeType.List:
                                {
                                    ListTypeInfo listType = field.BaseInfo as ListTypeInfo;
                                    string type = string.Format("List<{0}>", listType.ItemType);
                                    TypeType typeType = TypeInfo.GetTypeType(listType.ItemType);
                                    if (typeType == TypeType.Enum)
                                        type = type.Replace(listType.ItemType, Base.Int);
                                    string fullType = CodeWriter.GetFullNamespace(Values.ConfigRootNode, listType.ItemType);
                                    type = type.Replace(listType.ItemType, fullType);

                                    string initValue = string.Format("new {0}()", type);
                                    CodeWriter.Comments(builder, field.Des);
                                    CodeWriter.FieldInit(builder, CodeWriter.Public, CodeWriter.Readonly, type, field.Name, initValue);
                                    break;
                                }
                            case TypeType.Dict:
                                {
                                    DictTypeInfo dictType = field.BaseInfo as DictTypeInfo;
                                    string type = string.Format("Dictionary<{0}, {1}>", dictType.KeyType, dictType.ValueType);
                                    if (TypeInfo.GetTypeType(dictType.KeyType) == TypeType.Enum)
                                        type = type.Replace(dictType.KeyType, Base.Int);
                                    TypeType typeType = TypeInfo.GetTypeType(dictType.ValueType);
                                    if (TypeInfo.GetTypeType(dictType.ValueType) == TypeType.Enum)
                                        type = type.Replace(dictType.ValueType, Base.Int);

                                    string fullType = CodeWriter.GetFullNamespace(Values.ConfigRootNode, dictType.KeyType);
                                    type = type.Replace(dictType.KeyType, fullType);
                                    fullType = CodeWriter.GetFullNamespace(Values.ConfigRootNode, dictType.ValueType);
                                    type = type.Replace(dictType.ValueType, fullType);

                                    string initValue = string.Format("new {0}()", type);
                                    CodeWriter.Comments(builder, field.Des);
                                    CodeWriter.FieldInit(builder, CodeWriter.Public, CodeWriter.Readonly, type, field.Name, initValue);
                                    break;
                                }
                            case TypeType.None:
                            default:
                                break;
                        }
                    }

                    builder.AppendLine();
                    if (classInfo.Inherit == null)
                        CodeWriter.Constructor(builder, CodeWriter.Public, classInfo.Name, new string[] { CLASS_DATA_STREAM }, new string[] { ARG_DATASTREAM });
                    else
                    {
                        string funcName = string.Format("{0} : base({1})", classInfo.Name, ARG_DATASTREAM);
                        string[] args = new string[] { ARG_DATASTREAM };
                        CodeWriter.Constructor(builder, CodeWriter.Public, classInfo.Name, new string[] { CLASS_DATA_STREAM }, args, args);
                    }
                    for (int j = 0; j < classInfo.Fields.Count; j++)
                    {
                        FieldInfo field = classInfo.Fields[j];
                        InitVariable(builder, field.BaseInfo, field.Type, field.Name, ARG_DATASTREAM);
                    }
                }
                else
                {
                    //CodeWriter.ClassBase(builder, CodeWriter.Public, CodeWriter.Sealed, baseType.Name);
                    //EnumTypeInfo enumType = baseType as EnumTypeInfo;
                    //foreach (ConstInfo item in enumType.Enums)
                    //{
                    //    if (!string.IsNullOrWhiteSpace(item.Des))
                    //        CodeWriter.Comments(builder, item.Des);
                    //    CodeWriter.FieldInit(builder, CodeWriter.Public, CodeWriter.Const, Base.Int, item.Name, item.Value);
                    //}

                    EnumTypeInfo enumType = baseType as EnumTypeInfo;
                    CodeWriter.Enum(builder, CodeWriter.Public, enumType.Name);
                    foreach (ConstInfo item in enumType.Enums)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Des))
                            CodeWriter.Comments(builder, item.Des);
                        CodeWriter.EnumField(builder, item.Name, item.Value);
                    }
                }

                CodeWriter.EndAll(builder);

                string path = Path.Combine(Values.ExportCode, baseType.NamespaceName, baseType.Name + ".cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
        private static void InitVariable(StringBuilder builder, BaseTypeInfo typeInfo, string type, string varName, string argName)
        {
            CodeWriter.IntervalLevel(builder);
            TypeType typeType = typeInfo.EType;
            switch (typeType)
            {
                case TypeType.Base:
                    string baseFunc = string.Format("{0}.Get{1}()", argName, Util.FirstCharUpper(type));
                    builder.AppendFormat("this.{0} = {1};\n", varName, baseFunc);
                    break;
                case TypeType.Enum:
                    builder.AppendFormat("this.{0} = {1}.GetInt();\n", varName, argName);
                    break;
                case TypeType.Class:
                    builder.AppendFormat("this.{0} = {1};\n", varName, GetClassVarValue(typeInfo, type, argName));
                    break;
                case TypeType.List:
                    builder.AppendFormat("for (int n = {0}.GetInt(); n-- > 0; )", argName);
                    CodeWriter.Start(builder);
                    CodeWriter.IntervalLevel(builder);
                    ListTypeInfo listType = typeInfo as ListTypeInfo;
                    BaseTypeInfo itemTypeInfo = listType.ItemInfo.BaseInfo;
                    switch (itemTypeInfo.EType)
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
                    CodeWriter.End(builder);
                    break;
                case TypeType.Dict:
                    builder.AppendFormat("for (int n = {0}.GetInt(); n-- > 0;)", argName);
                    CodeWriter.Start(builder);
                    CodeWriter.IntervalLevel(builder);
                    DictTypeInfo dictType = typeInfo as DictTypeInfo;
                    if (TypeInfo.GetTypeType(dictType.KeyType) == TypeType.Base)
                        builder.AppendFormat("{0} k = {1}.Get{2}();\n", dictType.KeyType, argName, Util.FirstCharUpper(dictType.KeyType));
                    else if (TypeInfo.GetTypeType(dictType.KeyType) == TypeType.Enum)
                        builder.AppendFormat("{0} k = {1}.GetInt();\n", "int", argName);

                    CodeWriter.IntervalLevel(builder);
                    BaseTypeInfo vTypeInfo = dictType.ValueInfo.BaseInfo;
                    switch (vTypeInfo.EType)
                    {
                        case TypeType.Base:
                            builder.AppendFormat("this.{0}[k] = {1}.Get{2}();\n", varName, argName, Util.FirstCharUpper(dictType.ValueType));
                            break;
                        case TypeType.Class:
                            builder.AppendFormat("this.{0}[k] = {1};\n", varName, GetClassVarValue(vTypeInfo, dictType.ValueType, argName));
                            break;
                        case TypeType.Enum:
                            builder.AppendFormat("this.{0}[k] = ({1}){2}.GetInt();\n", varName, vTypeInfo.GetFullName(), argName);
                            break;
                        case TypeType.List:
                        case TypeType.Dict:
                        case TypeType.None:
                        default:
                            break;
                    }
                    CodeWriter.End(builder);
                    break;
                case TypeType.None:
                default:
                    break;
            }

        }
        private static string GetClassVarValue(BaseTypeInfo baseType, string type, string argName)
        {
            string value = "";
            ClassTypeInfo classType = baseType as ClassTypeInfo;
            if (classType.IsPolyClass)
                value = string.Format("({0}){1}.GetObject({1}.GetString())", type, argName);
            else
                value = string.Format("new {0}({1})", type, argName);
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
            string path = Path.Combine(Values.ExportCode, CLASS_DATA_STREAM + ".cs");
            CodeWriter.UsingNamespace(builder, NameSpaces);
            CodeWriter.NameSpace(builder, Values.ConfigRootNode);
            CodeWriter.ClassBase(builder, CodeWriter.Public, CLASS_DATA_STREAM);

            string fRIndex = "_rIndex";
            string fCIndex = "_cIndex";
            string fRows = "_rows";
            string fColumns = "_columns";
            string[] types = { Base.String, "Encoding" };
            string[] args = { "path", "encoding" };
            CodeWriter.Constructor(builder, CodeWriter.Public, CLASS_DATA_STREAM, types, args);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("{0} = File.ReadAllLines({1}, {2});\n", fRows, args[0], args[1]);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("{0} = {1} = 0;\n", fRIndex, fCIndex);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("if (_rows.Length > 0)\n");
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("{0} = {1}[{2}].Split(\"{3}\".ToCharArray());\n",
                fColumns, fRows, fRIndex, Values.CsvSplitFlag);
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("public int Count {{ get {{ return {0}.Length; }} }}\n", fRows);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, Base.Int, "GetInt");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("int result;");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("int.TryParse(Next(), out result);");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("return result;");
            CodeWriter.End(builder);

            CodeWriter.Function(builder, CodeWriter.Public, Base.Long, "GetLong");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("long result;");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("long.TryParse(Next(), out result);");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("return result;");
            CodeWriter.End(builder);

            CodeWriter.Function(builder, CodeWriter.Public, Base.Float, "GetFloat");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("float result;");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("float.TryParse(Next(), out result);");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("return result;");
            CodeWriter.End(builder);

            CodeWriter.Function(builder, CodeWriter.Public, Base.Bool, "GetBool");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("string v = Next();");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("if (string.IsNullOrEmpty(v)) return false;");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("return !v.Equals(\"0\");");
            CodeWriter.End(builder);

            CodeWriter.Function(builder, CodeWriter.Public, Base.String, "GetString");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("return Next();");
            CodeWriter.End(builder);

            types = new string[] { "string" };
            args = new string[] { "fullName" };
            CodeWriter.Comments(builder, "支持多态,直接反射类型");
            CodeWriter.Function(builder, CodeWriter.Public, CLASS_CFG_OBJECT, "GetObject", types, args);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("Type type = Type.GetType({0});\n",  args[0]);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("if (type == null)");
            CodeWriter.Start(builder);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("UnityEngine.Debug.LogErrorFormat(\"DataStream 解析{{0}}类型失败!\", {0});\n", args[0]);
            CodeWriter.End(builder);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("return ({0})Activator.CreateInstance(type, new object[] {{ this }});\n", CLASS_CFG_OBJECT);
            CodeWriter.End(builder);
            builder.AppendLine();
            builder.AppendLine();

            CodeWriter.Field(builder, CodeWriter.Private, Base.Int, fRIndex);
            CodeWriter.Field(builder, CodeWriter.Private, Base.Int, fCIndex);
            CodeWriter.Field(builder, CodeWriter.Private, "string[]", fRows);
            CodeWriter.Field(builder, CodeWriter.Private, "string[]", fColumns);
            builder.AppendLine();
            CodeWriter.Function(builder, CodeWriter.Private, Base.Void, "NextRow");
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("if ({0} >= {1}.Length) return;\n", fRIndex, fRows);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("{0}++;\n", fRIndex);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("{0} = 0;\n", fCIndex);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("{0} = {1}[{2}].Split(\"{3}\".ToCharArray());\n",
                fColumns, fRows, fRIndex, Values.CsvSplitFlag);
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Private, Base.String, "Next");
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("if ({0} >= {1}.Length) NextRow();\n", fCIndex, fColumns);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("return {0}[{1}++];\n", fColumns, fCIndex, Values.CsvSplitFlag);
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.EndAll(builder);
            Util.SaveFile(path, builder.ToString());
            builder.Clear();
        }
        /// <summary>
        /// 配置管理类
        /// </summary>
        private static void DefineCfgManager()
        {
            StringBuilder builder = new StringBuilder();
            List<string> NameSpaces = new List<string>() { "System", "System.IO", "System.Text", "System.Collections.Generic" };

            //构建Csv数据解析类DataStream.cs
            string path = Path.Combine(Values.ExportCode, CLASS_CFG_MANAGER + ".cs");
            CodeWriter.UsingNamespace(builder, NameSpaces);
            CodeWriter.NameSpace(builder, Values.ConfigRootNode);
            CodeWriter.ClassBase(builder, CodeWriter.Public, CLASS_CFG_MANAGER);

            CodeWriter.Comments(builder, "配置文件文件夹路径");
            CodeWriter.Field(builder, CodeWriter.Public, CodeWriter.Static, Base.String, FIELD_CONFIG_DIR);
            builder.AppendLine();

            List<BaseTypeInfo> typeInfos = new List<BaseTypeInfo>();
            typeInfos.AddRange(TypeInfo.Instance.ClassInfos);
            List<string> loadAll = new List<string>();
            StringBuilder clear = new StringBuilder();
            for (int i = 0; i < typeInfos.Count; i++)
            {
                BaseTypeInfo baseType = typeInfos[i];
                if (baseType.EType != TypeType.Class)
                    continue;
                ClassTypeInfo classType = baseType as ClassTypeInfo;
                if (classType.IndexField == null
                    || string.IsNullOrWhiteSpace(classType.DataPath)
                    || string.IsNullOrWhiteSpace(classType.IndexField.Type))
                    continue;

                CodeWriter.IntervalLevel(builder);
                TypeType keyType = TypeInfo.GetTypeType(classType.IndexField.Type);
                if (keyType == TypeType.Enum)
                    builder.AppendFormat("public static readonly Dictionary<{0}, {1}> {2} = new Dictionary<{0}, {1}>();\n",
                       Base.Int, classType.GetFullName(), classType.Name);
                else
                {
                    string fullType = CodeWriter.GetFullNamespace(Values.ConfigRootNode, classType.GetFullName());
                    builder.AppendFormat("public static readonly Dictionary<{0}, {1}> {2} = new Dictionary<{0}, {1}>();\n",
                    classType.IndexField.Type, fullType, classType.Name);
                }

                //加载所有配置-块内语句
                string rel = classType.GetFullName().Replace(".", "/") + Values.CsvFileExt;
                loadAll.Add(string.Format("path = {0} + \"{1}\";\n", FIELD_CONFIG_DIR, rel));
                loadAll.Add(string.Format("var {0}s = Load(path, (d) => new {1}(d));\n",
                    classType.Name.ToLower(), classType.GetFullName()));
                loadAll.Add(string.Format("{0}s.ForEach(v => {1}.Add(v.{2}, v));\n",
                    classType.Name.ToLower(), classType.Name, classType.IndexField.Name));

                //清除所有配置-块内语句
                CodeWriter.IntervalLevel(clear);
                clear.AppendFormat("\t{0}.Clear();\n", classType.Name);
            }
            builder.AppendLine();

            //加载单个配置
            string[] types = { "string", "Func<DataStream, T>" };
            string[] args = { "path", "constructor" };
            CodeWriter.Field(builder, CodeWriter.Private, CodeWriter.Static, Base.Int, "_row");
            CodeWriter.Comments(builder, "constructor参数为指定类型的构造函数");
            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, "List<T>", "Load<T>", types, args);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("if (!File.Exists(path))");
            CodeWriter.Start(builder);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("UnityEngine.Debug.LogError(path + \"配置路径不存在\");\n");
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("return new List<T>();\n");
            CodeWriter.End(builder);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("{0} data = new {0}({1}, Encoding.UTF8);\n", CLASS_DATA_STREAM, args[0]);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("List<T> list = new List<T>();\n");
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("for (int i = 0; i < data.Count; i++)");
            CodeWriter.Start(builder);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("_row = i;\n");
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("list.Add({0}(data));\n", args[1]);
            CodeWriter.End(builder);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("return list;\n");
            CodeWriter.End(builder);
            builder.AppendLine();

            //加载所有配置
            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Void, "LoadAll");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("string path = \"Data Path Empty\";");
            CodeWriter.IntervalLevel(builder);
            builder.Append("try");
            CodeWriter.Start(builder);
            for (int i = 0; i < loadAll.Count; i++)
            {
                CodeWriter.IntervalLevel(builder);
                builder.Append(loadAll[i]);
            }
            CodeWriter.End(builder);
            CodeWriter.IntervalLevel(builder);
            builder.Append("catch (Exception e)");
            CodeWriter.Start(builder);
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("UnityEngine.Debug.LogErrorFormat(\"{0}[r{3}]\\n{1}\\n{2}\", path, e.Message, e.StackTrace, _row);");
            CodeWriter.End(builder);
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Void, "Clear");
            builder.Append(clear);
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.EndAll(builder);
            Util.SaveFile(path, builder.ToString());
            builder.Clear();
        }
    }
}
