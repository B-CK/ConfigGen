using System;
using System.IO;
using System.Text;
using ConfigGen.Description;
using System.Collections.Generic;
using ConfigGen.TypeInfo;

namespace ConfigGen.Export
{
    public class ExportCSharp
    {
        private const string CLASS_CFG_OBJECT = "CfgObject";
        private const string CLASS_DATA_STREAM = "DataStream";
        private const string CLASS_CFG_MANAGER = "CfgManager";
        private const string FIELD_CONFIG_DIR = "ConfigDir";
        private const string ARG_DATASTREAM = "data";
        private static readonly List<string> CsvNameSpaces = new List<string>() { "System", "System.Collections.Generic", Consts.ConfigRootNode };

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
        public static void Export()
        {
            StringBuilder builder = new StringBuilder();

            //构建Csv存储类基础类CfgObject.cs
            string path = Path.Combine(Consts.CSDir, CLASS_CFG_OBJECT + ".cs");
            CodeWriter.UsingNamespace(builder, new List<string>(CsvNameSpaces));
            CodeWriter.NameSpace(builder, Consts.ConfigRootNode);
            CodeWriter.DefineClass(builder, string.Format("{0} {1}", CodeWriter.Public, CodeWriter.Abstract), CLASS_CFG_OBJECT);
            CodeWriter.EndAll(builder);
            Util.SaveFile(path, builder.ToString());
            builder.Clear();

            DefineDataStream();
            DefineCfgManager();

            GenClasses();
            GenEnums();
        }

        /// <summary>
        /// 数据流解析类,各种数据类型解析
        /// </summary>
        private static void DefineDataStream()
        {
            StringBuilder builder = new StringBuilder();
            List<string> NameSpaces = new List<string>() { "System", "System.Text", "System.IO", "System.Reflection" };

            //构建Csv数据解析类DataStream.cs
            string path = Path.Combine(Consts.CSDir, CLASS_DATA_STREAM + ".cs");
            CodeWriter.UsingNamespace(builder, NameSpaces);
            CodeWriter.NameSpace(builder, Consts.ConfigRootNode);
            CodeWriter.Comments(builder, "数据解析类");
            CodeWriter.DefineClass(builder, CodeWriter.Public, CLASS_DATA_STREAM);

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
                fColumns, fRows, fRIndex, Consts.CsvSplitFlag);
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
            builder.AppendFormat("Type type = Type.GetType({0});\n", args[0]);
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

            CodeWriter.DefineField(builder, CodeWriter.Private, Base.Int, fRIndex);
            CodeWriter.DefineField(builder, CodeWriter.Private, Base.Int, fCIndex);
            CodeWriter.DefineField(builder, CodeWriter.Private, "string[]", fRows);
            CodeWriter.DefineField(builder, CodeWriter.Private, "string[]", fColumns);
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
                fColumns, fRows, fRIndex, Consts.CsvSplitFlag);
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Private, Base.String, "Next");
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("if ({0} >= {1}.Length) NextRow();\n", fCIndex, fColumns);
            CodeWriter.IntervalLevel(builder);
            builder.AppendFormat("return {0}[{1}++];\n", fColumns, fCIndex, Consts.CsvSplitFlag);
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

            string path = Path.Combine(Consts.CSDir, CLASS_CFG_MANAGER + ".cs");
            CodeWriter.UsingNamespace(builder, NameSpaces);
            CodeWriter.NameSpace(builder, Consts.ConfigRootNode);
            CodeWriter.Comments(builder, "数据管理类");
            CodeWriter.DefineClass(builder, CodeWriter.Public, CLASS_CFG_MANAGER);

            CodeWriter.Comments(builder, "配置文件文件夹路径");
            CodeWriter.DefineField(builder, CodeWriter.Public, CodeWriter.Static, Base.String, FIELD_CONFIG_DIR);
            builder.AppendLine();

            List<ClassInfo> exports = ClassInfo.GetExports();
            List<string> loadAll = new List<string>();
            StringBuilder clear = new StringBuilder();
            for (int i = 0; i < exports.Count; i++)
            {
                ClassInfo cls = exports[i];
                ConfigInfo cfg = ConfigInfo.Get(cls.FullName);
                CodeWriter.IntervalLevel(builder);
                builder.AppendFormat("public static readonly Dictionary<{0}, {1}> {2} = new Dictionary<{0}, {1}>();\n", cfg.Index.FullType, cls.Name, cls.FullName);

                //加载所有配置-块内语句
                string rel = cls.FullName.Replace(".", "/") + Consts.CsvFileExt;
                loadAll.Add(string.Format("path = {0} + \"{1}\";\n", FIELD_CONFIG_DIR, rel));
                loadAll.Add(string.Format("var {0}s = Load(path, (d) => new {1}(d));\n", cls.Name.ToLower(), cls.FullName));
                loadAll.Add(string.Format("{0}s.ForEach(v => {1}.Add(v.{2}, v));\n", cls.Name.ToLower(), cls.Name, cfg.Index.Name));

                //清除所有配置-块内语句
                CodeWriter.IntervalLevel(clear);
                clear.AppendFormat("\t{0}.Clear();\n", cls.Name);
            }
            builder.AppendLine();

            //加载单个配置
            string[] types = { "string", "Func<DataStream, T>" };
            string[] args = { "path", "constructor" };
            CodeWriter.DefineField(builder, CodeWriter.Private, CodeWriter.Static, Base.Int, "_row");
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


        /// <summary>
        /// 构造自定义类型
        /// </summary>
        private static void GenClasses()
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            List<ClassInfo> exports = ClassInfo.GetExports();
            for (int i = 0; i < exports.Count; i++)
            {
                ClassInfo cls = exports[i];
                CodeWriter.UsingNamespace(builder, CsvNameSpaces);
                builder.AppendLine();
                CodeWriter.NameSpace(builder, cls.Namespace);
                CodeWriter.Comments(builder, cls.Desc);
                string inherit = cls.Inherit.IsEmpty() ? CLASS_CFG_OBJECT : cls.Inherit;
                CodeWriter.DefineClass(builder, CodeWriter.Public, cls.Name, CLASS_CFG_OBJECT);

                //常量字段
                for (int j = 0; j < cls.Consts.Count; j++)
                {
                    ConstInfo field = cls.Consts[j];
                    CodeWriter.Comments(builder, field.Desc);
                    string type = field.FullType;
                    string value = CheckConst(field.OriginalType, field.Value);
                    switch (field.OriginalType)
                    {
                        case Consts.LIST:
                            type = string.Format("List<{0}>", field.Types[1]);
                            string[] list = field.Value.Split(Consts.SplitFlag);
                            for (int k = 0; k < list.Length; k++)
                                list[k] = CheckConst(field.Types[1], list[k]);
                            value = string.Format("new {0}(){{ {1} }}", type, Util.List2String(list));
                            break;
                        case Consts.DICT:
                            type = string.Format("Dictionary<{0}, {1}>", field.Types[1], field.Types[2]);
                            string[] dict = field.Value.Split(Consts.SplitFlag);
                            for (int k = 0; k < dict.Length; k++)
                            {
                                string[] nodes = dict[k].Split(Consts.ArgsSplitFlag);
                                nodes[0] = CheckConst(field.Types[1], nodes[0]);
                                nodes[1] = CheckConst(field.Types[2], nodes[1]);
                                dict[k] = string.Format("{{{0}, {1}}},", nodes[0], nodes[1]);
                            }
                            value = string.Format("new {0}(){{ {1} }}", type, Util.List2String(dict));
                            break;
                    }
                    CodeWriter.DefineConst(builder, type, field.Name, value);
                }

                for (int j = 0; j < cls.Fields.Count; j++)
                {
                    FieldInfo field = cls.Fields[j];
                    if (!Util.MatchGroups(field.Group)) continue;

                    //普通字段
                    CodeWriter.Comments(builder, field.Desc);
                    string modifier = string.Format("{0} {1}", CodeWriter.Public, CodeWriter.Readonly);
                    if (field.IsRaw || field.IsEnum || field.IsClass)
                        CodeWriter.DefineField(builder, modifier, field.FullType, field.Name);
                    else if (field.IsContainer)
                    {
                        string init = "-";
                        if (field.OriginalType == Consts.LIST)
                            init = string.Format("new List<{0}>", field.Types[1]);
                        else if (field.OriginalType == Consts.DICT)
                            init = string.Format("new Dictionary<{0}, {1}>", field.Types[1], field.Types[2]);
                        CodeWriter.DefineField(builder, modifier, field.FullType, field.Name, init);
                    }

                    //构造函数
                    if (field.IsRaw || field.IsEnum || field.IsClass)
                        CodeWriter.SetField(sb, field.Name, ReadType(field));
                    else if (field.IsContainer)
                    {
                        if (field.OriginalType == Consts.LIST)
                        {
                            sb.AppendFormat("for (int n = {0}.GetInt(); n-- > 0;)", ARG_DATASTREAM);
                            CodeWriter.Start(sb);
                            CodeWriter.IntervalLevel(sb);
                            FieldInfo item = field.GetItemDefine();
                            CodeWriter.DefineField(sb, null, item.FullType, "v", ReadType(item));
                            sb.AppendFormat("{0}.Add(v);\n", field.Name);
                            CodeWriter.IntervalLevel(sb);
                            CodeWriter.End(sb);
                        }
                        else if (field.OriginalType == Consts.DICT)
                        {
                            sb.AppendFormat("for (int n = {0}.GetInt(); n-- > 0;)", ARG_DATASTREAM);
                            CodeWriter.Start(sb);
                            CodeWriter.IntervalLevel(sb);
                            FieldInfo key = field.GetKeyDefine();
                            FieldInfo value = field.GetValueDefine();
                            CodeWriter.DefineField(sb, null, key.FullType, "k", ReadType(key));
                            CodeWriter.DefineField(sb, null, value.FullType, "v", ReadType(value));
                            sb.AppendFormat("{0}[k] = v;\n", field.Name);
                            CodeWriter.End(sb);
                        }
                    }
                }
                builder.AppendLine();

                string[] types = new string[] { CLASS_DATA_STREAM };
                string[] args = new string[] { CLASS_DATA_STREAM };
                CodeWriter.Constructor(builder, CodeWriter.Public, cls.Name, types, args, cls.Inherit.IsEmpty() ? null : args);
                builder.AppendLine(sb.ToString());

                CodeWriter.EndAll(builder);
                string path = Path.Combine(Consts.CSDir, cls.Namespace, cls.Name + ".cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
        private static string CheckConst(string type, string value)
        {
            switch (type)
            {
                case Consts.FLOAT:
                    value = string.Format("{0}f", value);
                    break;
                case Consts.STRING:
                    value = string.Format("@\"{0}\"", value);
                    break;
            }
            return value;
        }
        private static string ReadType(FieldInfo field)
        {
            if (field.IsRaw)
                return string.Format("{0}.Get{1}()", ARG_DATASTREAM, Util.FirstCharUpper(field.FullType));
            else if (field.IsEnum)
                return string.Format("({1}){0}.GetInt()", ARG_DATASTREAM, field.FullType);
            else if (field.IsClass)
            {
                ClassInfo info = ClassInfo.Get(field.FullType);
                string fmt = info.IsDynamic() ? "({0}){1}.GetObject({1}.GetString())" : "new ({0}){1}";
                return string.Format(fmt, field.FullType, ARG_DATASTREAM);
            }
            Util.Error("不支持集合嵌套类型:" + field.FullType);
            return null;
        }
        /// <summary>
        /// 生成枚举类
        /// </summary>
        private static void GenEnums()
        {

            StringBuilder builder = new StringBuilder();
            List<EnumInfo> exports = EnumInfo.GetExports();
            for (int i = 0; i < exports.Count; i++)
            {
                EnumInfo en = exports[i];
                CodeWriter.Comments(builder, en.Desc);
                CodeWriter.Enum(builder, CodeWriter.Public, en.Name);
                var eit = en.Values.GetEnumerator();
                while (eit.MoveNext())
                {
                    var item = eit.Current;
                    CodeWriter.DefineEnum(builder, item.Key, item.Value);
                }

                CodeWriter.EndAll(builder);
                string path = Path.Combine(Consts.CSDir, en.Namespace, en.Name + ".cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
    }
}
