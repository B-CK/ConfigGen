using System.IO;
using Tool;
using System.Collections.Generic;
using Tool.Wrap;

namespace Export
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
        private static readonly List<string> CsvNameSpaces = new List<string>() { "System", "System.Collections.Generic", Setting.ModuleName };

        /// <summary>
        /// 导出CSharp类型Csv存储类
        /// </summary>
        public static void Export()
        {
            //构建Csv存储类基础类CfgObject.cs
            string path = Path.Combine(Setting.CSDir, CLASS_CFG_OBJECT + ".cs");
            CodeWriter cfgCode = new CodeWriter();
            cfgCode.UsingNamespace(new List<string>(CsvNameSpaces));
            cfgCode.NameSpace(Setting.ModuleName);
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
            builder.NameSpace(Setting.ModuleName);
            builder.Comments("数据解析类");
            builder.DefineClass(CodeWriter.Public, CLASS_DATA_STREAM);

            builder.DefineField(CodeWriter.Private + " " + CodeWriter.Readonly, CSharp.String + "[]", "_line");
            builder.DefineField(CodeWriter.Private, CSharp.Int, "_index");
            builder.Constructor(CodeWriter.Public, CLASS_DATA_STREAM, new string[] { CSharp.String, "Encoding" }, new string[] { "path", "encoding" });
            builder.SetField("_line", "File.ReadAllLines(path)");
            builder.SetField("_index", "0");
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public, CSharp.String, "GetNext");
            builder.AppendLine("return _index < _line.Length ? _line[_index++] : null;");
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Private, CSharp.Void, "Error", new string[] { CSharp.String }, new string[] { "err" });
            builder.AppendLine("throw new Exception(err);");
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Private, CSharp.String, "GetNextAndCheckNotEmpty");
            builder.DefineField(null, CSharp.String, "v", "GetNext()");
            builder.AppendLine("if (v == null) {");
            builder.AddLevel();
            builder.AppendLine("Error(\"read not enough\");");
            builder.End();
            builder.AppendLine("return v;");
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public, CSharp.String, "GetString");
            builder.AppendLine("return GetNextAndCheckNotEmpty();");
            builder.End();

            builder.Function(CodeWriter.Public, CSharp.Float, "GetFloat");
            builder.AppendLine("return float.Parse(GetNextAndCheckNotEmpty());");
            builder.End();

            builder.Function(CodeWriter.Public, CSharp.Int, "GetInt");
            builder.AppendLine("return int.Parse(GetNextAndCheckNotEmpty());");
            builder.End();

            builder.Function(CodeWriter.Public, CSharp.Long, "GetLong");
            builder.AppendLine("return long.Parse(GetNextAndCheckNotEmpty());");
            builder.End();

            builder.Function(CodeWriter.Public, CSharp.Bool, "GetBool");
            builder.AppendLine("string v = GetNextAndCheckNotEmpty();");
            builder.AppendLine("if (v == \"true\") {");
            builder.AddLevel();
            builder.AppendLine("return true;");
            builder.End();
            builder.AppendLine("if (v == \"false\") {");
            builder.AddLevel();
            builder.AppendLine("return false;");
            builder.End();
            builder.AppendLine("Error(v + \" isn't bool\");");
            builder.AppendLine("return false;");
            builder.End();

            string cfgobj = Setting.ModuleName + "." + CLASS_CFG_OBJECT;
            string cfgdata = Setting.ModuleName + "." + CLASS_DATA_STREAM;
            builder.Function(CodeWriter.Public, cfgobj, "GetObject", new string[] { CSharp.String }, new string[] { "name" });
            builder.AppendFormat("return ({0})Type.GetType(name).GetConstructor(new[] {{ typeof({1}) }}).Invoke(new object[] {{ this }});\n", cfgobj, cfgdata);
            builder.End();

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
            builder.NameSpace(Setting.ModuleName);
            builder.Comments("数据管理类");
            builder.DefineClass(CodeWriter.Public, CLASS_CFG_MANAGER);

            builder.Comments("配置文件文件夹路径");
            builder.DefineField(CodeWriter.Public + " " + CodeWriter.Static, CSharp.String, FIELD_CONFIG_DIR);
            builder.AppendLine();

            List<ConfigWrap> exports = ConfigWrap.GetExports();
            List<string> loadAll = new List<string>();
            CodeWriter clear = new CodeWriter();
            for (int i = 0; i < exports.Count; i++)
            {
                ConfigWrap cfg = exports[i];
                ClassWrap cls = ClassWrap.Get(cfg.FullType);
                builder.AppendFormat("public static readonly Dictionary<{0}, {1}> {2} = new Dictionary<{0}, {1}>();\n", cfg.Index.FullType, cls.FullType, cls.Name);

                //加载所有配置-块内语句
                string rel = cfg.OutputFile + Setting.DataFileExt;
                loadAll.Add(string.Format("path = {0} + @\"{1}\";\n", FIELD_CONFIG_DIR, rel));
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
            builder.AppendFormat("int length = data.GetInt();");
            builder.AppendFormat("for (int i = 0; i < length; i++)");
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
            List<ClassWrap> exports = ClassWrap.GetExports();
            for (int i = 0; i < exports.Count; i++)
            {
                ClassWrap cls = exports[i];
                builder.UsingNamespace(CsvNameSpaces);
                builder.AppendLine();
                builder.NameSpace(cls.Namespace);
                builder.Comments(cls.Desc);
                string inherit = cls.Inherit.IsEmpty() ? CLASS_CFG_OBJECT : cls.Inherit;
                builder.DefineClass(CodeWriter.Public, cls.Name, inherit);

                for (int j = 0; j < cls.Fields.Count; j++)
                {
                    FieldWrap field = cls.Fields[j];
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
                            FieldWrap item = field.GetItemDefine();
                            sb.DefineField(null, item.FullType, "v", ReadType(item));
                            sb.AppendFormat("{0}.Add(v);\n", field.Name);
                            sb.End();
                        }
                        else if (field.OriginalType == Setting.DICT)
                        {
                            sb.AppendFormat("for (int n = {0}.GetInt(); n-- > 0;)", ARG_DATASTREAM);
                            sb.Start();
                            FieldWrap key = field.GetKeyDefine();
                            FieldWrap value = field.GetValueDefine();
                            sb.DefineField(null, key.FullType, "k", ReadType(key));
                            sb.AppendFormat("{0}[k] = {1};\n", field.Name, ReadType(value));
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
                string path = Path.Combine(Setting.CSDir, cls.Namespace.Replace(Setting.DotSplit[0], '\\'), cls.Name + ".cs");
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
        private static string ReadType(FieldWrap field)
        {
            if (field.IsRaw)
                return string.Format("{0}.Get{1}()", ARG_DATASTREAM, field.FullType.FirstCharUpper());
            else if (field.IsEnum)
                return string.Format("({1}){0}.GetInt()", ARG_DATASTREAM, field.FullType);
            else if (field.IsClass)
            {
                ClassWrap info = ClassWrap.Get(field.FullType);
                string fmt = info.IsDynamic() ? "({0}){1}.GetObject({1}.GetString())" : "new {0}({1})";
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
            List<EnumWrap> exports = EnumWrap.GetExports();
            for (int i = 0; i < exports.Count; i++)
            {
                EnumWrap en = exports[i];
                builder.NameSpace(en.Namespace);
                builder.Comments(en.Desc);
                builder.Enum(CodeWriter.Public, en.Name);
                foreach (var item in en.Values)
                    builder.DefineEnum(item.Key, item.Value.ToString());

                builder.EndAll();
                string path = Path.Combine(Setting.CSDir, en.Namespace.Replace(Setting.DotSplit[0], '\\'), en.Name + ".cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
    }
}
