using System;
using System.IO;
using System.Text;
using ConfigGen.Description;
using System.Collections.Generic;
using ConfigGen.TypeInfo;

namespace ConfigGen.Export
{
    public class CSharp
    {
        public const string Int = "int";
        public const string Long = "long";
        public const string Float = "float";
        public const string Bool = "bool";
        public const string String = "string";
        public const string Void = "void";
        public const string Object = "object";
    }

    public class ExportCSharp
    {
        private const string CLASS_CFG_OBJECT = "CfgObject";
        private const string CLASS_DATA_STREAM = "DataStream";
        private const string CLASS_CFG_MANAGER = "CfgManager";
        private const string FIELD_CONFIG_DIR = "ConfigDir";
        private const string ARG_DATASTREAM = "data";
        private static readonly List<string> CsvNameSpaces = new List<string>() { "System", "System.Collections.Generic", Setting.ConfigRootNode };

        /// <summary>
        /// 导出CSharp类型Csv存储类
        /// </summary>
        public static void Export()
        {
            //构建Csv存储类基础类CfgObject.cs
            string path = Path.Combine(Setting.CSDir, CLASS_CFG_OBJECT + ".cs");
            CodeWriter cfgCode = new CodeWriter();
            cfgCode.UsingNamespace(new List<string>(CsvNameSpaces));
            cfgCode.NameSpace(Setting.ConfigRootNode);
            cfgCode.DefineClass(string.Format("{0} {1}", CodeWriter.Public, CodeWriter.Abstract), CLASS_CFG_OBJECT);
            cfgCode.EndAll();
            Util.SaveFile(path, cfgCode.ToString());

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
            CodeWriter builder = new CodeWriter();
            List<string> NameSpaces = new List<string>() { "System", "System.Text", "System.IO", "System.Reflection" };

            //构建Csv数据解析类DataStream.cs
            string path = Path.Combine(Setting.CSDir, CLASS_DATA_STREAM + ".cs");
            builder.UsingNamespace(NameSpaces);
            builder.NameSpace(Setting.ConfigRootNode);
            builder.Comments("数据解析类");
            builder.DefineClass(CodeWriter.Public, CLASS_DATA_STREAM);

            string fRIndex = "_rIndex";
            string fCIndex = "_cIndex";
            string fRows = "_rows";
            string fColumns = "_columns";
            string[] types = { CSharp.String, "Encoding" };
            string[] args = { "path", "encoding" };
            builder.Constructor(CodeWriter.Public, CLASS_DATA_STREAM, types, args);
            builder.AppendFormat("{0} = File.ReadAllLines({1}, {2});\n", fRows, args[0], args[1]);
            builder.AppendFormat("{0} = {1} = 0;\n", fRIndex, fCIndex);
            builder.AppendFormat("if (_rows.Length > 0)\n");
            builder.AppendFormat("{0} = {1}[{2}].Split(\"\\n\".ToCharArray());\n", fColumns, fRows, fRIndex);//CValues.CsvSplitFlag
            builder.End();
            builder.AppendLine();

            builder.AppendFormat("public int Count {{ get {{ return {0}.Length; }} }}\n", fRows);
            builder.AppendLine();

            builder.Function(CodeWriter.Public, CSharp.Int, "GetInt");
            builder.AppendLine("int result;");
            builder.AppendLine("int.TryParse(Next(), out result);");
            builder.AppendLine("return result;");
            builder.End();

            builder.Function(CodeWriter.Public, CSharp.Long, "GetLong");
            builder.AppendLine("long result;");
            builder.AppendLine("long.TryParse(Next(), out result);");
            builder.AppendLine("return result;");
            builder.End();

            builder.Function(CodeWriter.Public, CSharp.Float, "GetFloat");
            builder.AppendLine("float result;");
            builder.AppendLine("float.TryParse(Next(), out result);");
            builder.AppendLine("return result;");
            builder.End();

            builder.Function(CodeWriter.Public, CSharp.Bool, "GetBool");
            builder.AppendLine("string v = Next();");
            builder.AppendLine("if (string.IsNullOrEmpty(v)) return false;");
            builder.AppendLine("return !v.Equals(\"0\");");
            builder.End();

            builder.Function(CodeWriter.Public, CSharp.String, "GetString");
            builder.AppendLine("return Next();");
            builder.End();

            types = new string[] { "string" };
            args = new string[] { "fullName" };
            builder.Comments("支持多态,直接反射类型");
            builder.Function(CodeWriter.Public, CLASS_CFG_OBJECT, "GetObject", types, args);
            builder.AppendFormat("Type type = Type.GetType({0});\n", args[0]);
            builder.AppendFormat("if (type == null)");
            builder.Start();
            builder.AppendFormat("UnityEngine.Debug.LogErrorFormat(\"DataStream 解析{{0}}类型失败!\", {0});\n", args[0]);
            builder.End();
            builder.AppendFormat("return ({0})Activator.CreateInstance(type, new object[] {{ this }});\n", CLASS_CFG_OBJECT);
            builder.End();
            builder.AppendLine();
            builder.AppendLine();

            builder.DefineField(CodeWriter.Private, CSharp.Int, fRIndex);
            builder.DefineField(CodeWriter.Private, CSharp.Int, fCIndex);
            builder.DefineField(CodeWriter.Private, "string[]", fRows);
            builder.DefineField(CodeWriter.Private, "string[]", fColumns);
            builder.AppendLine();
            builder.Function(CodeWriter.Private, CSharp.Void, "NextRow");
            builder.AppendFormat("if ({0} >= {1}.Length) return;\n", fRIndex, fRows);
            builder.AppendFormat("{0}++;\n", fRIndex);
            builder.AppendFormat("{0} = 0;\n", fCIndex);
            builder.AppendFormat("{0} = {1}[{2}].Split(\"\\n\".ToCharArray());\n", fColumns, fRows, fRIndex);//CValues.CsvSplitFlag
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Private, CSharp.String, "Next");
            builder.AppendFormat("if ({0} >= {1}.Length) NextRow();\n", fCIndex, fColumns);
            builder.AppendFormat("return {0}[{1}++];\n", fColumns, fCIndex, Setting.CsvSplitFlag);
            builder.End();
            builder.AppendLine();

            builder.EndAll();
            Util.SaveFile(path, builder.ToString());
            builder.Clear();
        }
        /// <summary>
        /// 配置管理类
        /// </summary>
        private static void DefineCfgManager()
        {
            CodeWriter builder = new CodeWriter();
            List<string> NameSpaces = new List<string>() { "System", "System.IO", "System.Text", "System.Collections.Generic" };

            string path = Path.Combine(Setting.CSDir, CLASS_CFG_MANAGER + ".cs");
            builder.UsingNamespace(NameSpaces);
            builder.NameSpace(Setting.ConfigRootNode);
            builder.Comments("数据管理类");
            builder.DefineClass(CodeWriter.Public, CLASS_CFG_MANAGER);

            builder.Comments("配置文件文件夹路径");
            builder.DefineField(CodeWriter.Public + " " + CodeWriter.Static, CSharp.String, FIELD_CONFIG_DIR);
            builder.AppendLine();

            List<ConfigInfo> exports = ConfigInfo.GetExports();
            List<string> loadAll = new List<string>();
            CodeWriter clear = new CodeWriter();
            for (int i = 0; i < exports.Count; i++)
            {
                ConfigInfo cfg = exports[i];
                ClassInfo cls = ClassInfo.Get(cfg.FullType);
                builder.AppendFormat("public static readonly Dictionary<{0}, {1}> {2} = new Dictionary<{0}, {1}>();\n", cfg.Index.FullType, cls.FullType, cls.Name);

                //加载所有配置-块内语句
                string rel = cfg.OutputFile + Setting.CsvFileExt;
                loadAll.Add(string.Format("path = {0} + \"{1}\";\n", FIELD_CONFIG_DIR, rel));
                loadAll.Add(string.Format("var {0}s = Load(path, (d) => new {1}(d));\n", cls.Name.ToLower(), cls.FullType));
                loadAll.Add(string.Format("{0}s.ForEach(v => {1}.Add(v.{2}, v));\n", cls.Name.ToLower(), cls.Name, cfg.Index.Name));

                //清除所有配置-块内语句
                clear.AppendFormat("{0}.Clear();\n", cls.Name);
            }
            builder.AppendLine();

            //加载单个配置
            string[] types = { "string", "Func<DataStream, T>" };
            string[] args = { "path", "constructor" };
            builder.DefineField(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Int, "_row");
            builder.Comments("constructor参数为指定类型的构造函数");
            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, "List<T>", "Load<T>", types, args);
            builder.AppendFormat("if (!File.Exists(path))");
            builder.Start();
            builder.AppendFormat("UnityEngine.Debug.LogError(path + \"配置路径不存在\");\n");
            builder.AppendFormat("return new List<T>();\n");
            builder.End();
            builder.AppendFormat("{0} data = new {0}({1}, Encoding.UTF8);\n", CLASS_DATA_STREAM, args[0]);
            builder.AppendFormat("List<T> list = new List<T>();\n");
            builder.AppendFormat("for (int i = 0; i < data.Count; i++)");
            builder.Start();
            builder.AppendFormat("_row = i;\n");
            builder.AppendFormat("list.Add({0}(data));\n", args[1]);
            builder.End();
            builder.AppendFormat("return list;\n");
            builder.End();
            builder.AppendLine();

            //加载所有配置
            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "LoadAll");
            builder.AppendLine("string path = \"Data Path Empty\";");
            builder.Append("try");
            builder.Start();
            for (int i = 0; i < loadAll.Count; i++)
            {

                builder.Append(loadAll[i]);
            }
            builder.End();

            builder.Append("catch (Exception e)");
            builder.Start();
            builder.AppendLine("UnityEngine.Debug.LogErrorFormat(\"{0}[r{3}]\\n{1}\\n{2}\", path, e.Message, e.StackTrace, _row);");
            builder.End();
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "Clear");
            builder.Append(clear.ToString());
            builder.End();
            builder.AppendLine();

            builder.EndAll();
            Util.SaveFile(path, builder.ToString());
            builder.Clear();
        }


        /// <summary>
        /// 构造自定义类型
        /// </summary>
        private static void GenClasses()
        {
            CodeWriter builder = new CodeWriter();
            CodeWriter sb = new CodeWriter();
            sb.AddLevel();
            sb.AddLevel();
            sb.AddLevel();
            List<ClassInfo> exports = ClassInfo.GetExports();
            for (int i = 0; i < exports.Count; i++)
            {
                ClassInfo cls = exports[i];
                builder.UsingNamespace(CsvNameSpaces);
                builder.AppendLine();
                builder.NameSpace(cls.Namespace);
                builder.Comments(cls.Desc);
                string inherit = cls.Inherit.IsEmpty() ? CLASS_CFG_OBJECT : cls.Inherit;
                builder.DefineClass(CodeWriter.Public, cls.Name, inherit);

                //常量字段
                for (int j = 0; j < cls.Consts.Count; j++)
                {
                    ConstInfo field = cls.Consts[j];
                    builder.Comments(field.Desc);
                    string type = field.FullType;
                    string value = CheckConst(field.OriginalType, field.Value);
                    switch (field.OriginalType)
                    {
                        case Setting.LIST:
                            type = string.Format("List<{0}>", field.Types[1]);
                            string[] list = field.Value.Split(Setting.SplitFlag);
                            for (int k = 0; k < list.Length; k++)
                                list[k] = CheckConst(field.Types[1], list[k]);
                            value = string.Format("new {0}(){{ {1} }}", type, Util.List2String(list));
                            break;
                        case Setting.DICT:
                            type = string.Format("Dictionary<{0}, {1}>", field.Types[1], field.Types[2]);
                            string[] dict = field.Value.Split(Setting.SplitFlag);
                            for (int k = 0; k < dict.Length; k++)
                            {
                                string[] nodes = dict[k].Split(Setting.ArgsSplitFlag);
                                nodes[0] = CheckConst(field.Types[1], nodes[0]);
                                nodes[1] = CheckConst(field.Types[2], nodes[1]);
                                dict[k] = string.Format("{{{0}, {1}}},", nodes[0], nodes[1]);
                            }
                            value = string.Format("new {0}(){{ {1} }}", type, Util.List2String(dict));
                            break;
                    }
                    builder.DefineConst(type, field.Name, value);
                }

                for (int j = 0; j < cls.Fields.Count; j++)
                {
                    FieldInfo field = cls.Fields[j];
                    if (!Util.MatchGroups(field.Group)) continue;

                    //普通字段
                    builder.Comments(field.Desc);
                    string modifier = string.Format("{0} {1}", CodeWriter.Public, CodeWriter.Readonly);
                    if (field.IsRaw || field.IsEnum || field.IsClass)
                        builder.DefineField(modifier, field.FullType, field.Name);
                    else if (field.IsContainer)
                    {
                        if (field.OriginalType == Setting.LIST)
                        {
                            string init = string.Format("new List<{0}>()", field.Types[1]);
                            string fullType = string.Format("List<{0}>", field.Types[1]);
                            builder.DefineField(modifier, fullType, field.Name, init);
                        }
                        else if (field.OriginalType == Setting.DICT)
                        {
                            string init = string.Format("new Dictionary<{0}, {1}>()", field.Types[1], field.Types[2]);
                            string fullType = string.Format("Dictionary<{0}, {1}>", field.Types[1], field.Types[2]);
                            builder.DefineField(modifier, fullType, field.Name, init);
                        }
                    }

                    //构造函数                   
                    if (field.IsRaw || field.IsEnum || field.IsClass)
                        sb.SetField(field.Name, ReadType(field));
                    else if (field.IsContainer)
                    {
                        if (field.OriginalType == Setting.LIST)
                        {
                            sb.AppendFormat("for (int n = {0}.GetInt(); n-- > 0;)", ARG_DATASTREAM);
                            sb.Start();
                            FieldInfo item = field.GetItemDefine();
                            sb.DefineField(null, item.FullType, "v", ReadType(item));
                            sb.AppendFormat("{0}.Add(v);\n", field.Name);
                            sb.End();
                        }
                        else if (field.OriginalType == Setting.DICT)
                        {
                            sb.AppendFormat("for (int n = {0}.GetInt(); n-- > 0;)", ARG_DATASTREAM);
                            sb.Start();
                            FieldInfo key = field.GetKeyDefine();
                            FieldInfo value = field.GetValueDefine();
                            sb.DefineField(null, key.FullType, "k", ReadType(key));
                            sb.DefineField(null, value.FullType, "v", ReadType(value));
                            sb.AppendFormat("{0}[k] = v;\n", field.Name);
                            sb.End();
                        }
                    }
                }
                builder.AppendLine();

                string[] types = new string[] { CLASS_DATA_STREAM };
                string[] args = new string[] { "data" };
                builder.Constructor(CodeWriter.Public, cls.Name, types, args, cls.Inherit.IsEmpty() ? null : args);
                builder.Append(sb.ToString(), false);

                builder.EndAll();
                string path = Path.Combine(Setting.CSDir, cls.Namespace.Replace(Setting.DOT[0], '\\'), cls.Name + ".cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
                sb.Clear();
            }
        }
        private static string CheckConst(string type, string value)
        {
            switch (type)
            {
                case Setting.FLOAT:
                    value = string.Format("{0}f", value);
                    break;
                case Setting.STRING:
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
            CodeWriter builder = new CodeWriter();
            List<EnumInfo> exports = EnumInfo.GetExports();
            for (int i = 0; i < exports.Count; i++)
            {
                EnumInfo en = exports[i];
                builder.NameSpace(en.Namespace);
                builder.Comments(en.Desc);
                builder.Enum(CodeWriter.Public, en.Name);
                var eit = en.Values.GetEnumerator();
                while (eit.MoveNext())
                {
                    var item = eit.Current;
                    builder.DefineEnum(item.Key, item.Value);
                }

                builder.EndAll();
                string path = Path.Combine(Setting.CSDir, en.Namespace.Replace(Setting.DOT[0], '\\'), en.Name + ".cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
    }
}
