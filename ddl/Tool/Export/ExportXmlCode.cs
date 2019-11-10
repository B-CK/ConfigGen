using System;
using System.Text;
using Xml;
using Wrap;
using System.IO;
using Description;
using System.Collections.Generic;

namespace Export
{
    public class ExportXmlCode
    {
        private const string XML_STREAM = "XmlManager";
        private const string CLASS_XML_OBJECT = "XmlObject";
        private static readonly List<string> XmlNameSpaces = new List<string>() { "System", "System.Linq", "System.IO", Setting.EditorName + Setting.ModuleName, "System.Xml", "System.Collections.Generic" };


        /// <summary>
        /// 编辑模式下的Xml读写操作
        /// </summary>
        public static void Export()
        {
            DefineXmlObject();
            GenXmlClass();
            GenXmlEnums();
        }
        private static void DefineXmlObject()
        {
            CodeWriter builder = new CodeWriter();
            string path = Path.Combine(Setting.XmlCodeDir, CLASS_XML_OBJECT + ".cs");
            builder.UsingNamespace(XmlNameSpaces);
            builder.AppendLine();
            builder.NameSpace(Setting.EditorName + Setting.ModuleName);

            builder.DefineClass(CodeWriter.Public + " " + CodeWriter.Abstract, CLASS_XML_OBJECT);
            builder.AppendLine("public abstract void Write(TextWriter os);");
            builder.AppendLine("public abstract void Read(XmlNode os);\n\n");
            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.String, "ReadAttribute",
                new string[] { "XmlNode", CSharp.String }, new string[] { "node", "attribute" });
            {

                builder.Append("try");
                builder.Start();
                {
                    builder.AppendLine("if (node != null && node.Attributes != null && node.Attributes[attribute] != null)");
                    builder.AppendLine("\treturn node.Attributes[attribute].Value;");
                }
                builder.End();

                builder.Append("catch (Exception ex)");
                builder.Start();
                {
                    builder.AppendLine("throw new Exception(string.Format(\"attribute:{0} not exist\", attribute), ex);");
                }
                builder.End();
                builder.AppendLine("return \"\";");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, "XmlNode", "GetOnlyChild",
                new string[] { "XmlNode", CSharp.String }, new string[] { "parent", "name" });
            {

                builder.AppendLine("XmlNode child = null;");
                builder.Append("foreach (XmlNode sub in parent.ChildNodes)");
                builder.Start();
                {
                    builder.Append("if (sub.NodeType == XmlNodeType.Element && sub.Name == name)");
                    builder.Start();
                    {
                        builder.AppendLine("if (child != null)");
                        builder.AppendLine("\tthrow new Exception(string.Format(\"child:{0} duplicate\", name));");
                        builder.AppendLine("child = sub;");
                    }
                    builder.End();
                }
                builder.End();
                builder.AppendLine("return child;");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, "List<XmlNode>", "GetChilds",
                new string[] { "XmlNode" }, new string[] { "parent" });
            {
                builder.AppendLine("var childs = new List<XmlNode>();");
                builder.AppendLine("if (parent != null)");
                builder.AppendLine("\tchilds.AddRange(parent.ChildNodes.Cast<XmlNode>().Where(sub => sub.NodeType == XmlNodeType.Element));");
                builder.AppendLine("return childs;");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Bool, "ReadBool",
                new string[] { "XmlNode" }, new string[] { "node" });
            {

                builder.AppendLine("string str = node.InnerText.ToLower();");

                builder.AppendLine("if (str == \"true\")");

                builder.AppendLine("\treturn true;");

                builder.AppendLine("else if (str == \"false\")");

                builder.AppendLine("\treturn false;");

                builder.AppendLine("throw new Exception(string.Format(\"'{0}' is not valid bool\", str));");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Int, "ReadInt",
                new string[] { "XmlNode" }, new string[] { "node" });
            {

                builder.AppendLine("return int.Parse(node.InnerText);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Long, "ReadLong",
                new string[] { "XmlNode" }, new string[] { "node" });
            {

                builder.AppendLine("return long.Parse(node.InnerText);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Float, "ReadFloat",
                new string[] { "XmlNode" }, new string[] { "node" });
            {

                builder.AppendLine("return float.Parse(node.InnerText);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.String, "ReadString",
                new string[] { "XmlNode" }, new string[] { "node" });
            {

                builder.AppendLine("return node.InnerText;");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, "T", "ReadObject<T>",
                new string[] { "XmlNode", CSharp.String }, new string[] { "node", "fullTypeName" }, CLASS_XML_OBJECT);
            {

                builder.AppendLine("var obj = (T)Create(node, fullTypeName);");

                builder.AppendLine("obj.Read(node);");

                builder.AppendLine("return obj;");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, "T", "ReadDynamicObject<T>",
                new string[] { "XmlNode", CSharp.String }, new string[] { "node", "ns" }, CLASS_XML_OBJECT);
            {

                builder.AppendLine("var fullTypeName = ns + \".\" + ReadAttribute(node, \"Type\");");

                builder.AppendLine("return ReadObject<T>(node, fullTypeName);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Object, "Create",
                new string[] { "XmlNode", CSharp.String }, new string[] { "node", "type" });
            {

                builder.Append("try");
                builder.Start();
                {

                    builder.AppendLine("var t = Type.GetType(type);");

                    builder.AppendLine("return Activator.CreateInstance(t);");
                }
                builder.End();

                builder.Append("catch (Exception e)");
                builder.Start();
                {

                    builder.AppendLine("throw new Exception(string.Format(\"type:{0} create fail!\", type), e);");
                }
                builder.End();
            }
            builder.End();
            builder.AppendLine();

            List<string> ls = new List<string>() { CSharp.Bool, CSharp.Int, CSharp.Long, CSharp.Float, CSharp.String };
            foreach (var item in ls)
            {
                builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "Write",
                    new string[] { "TextWriter", CSharp.String, item }, new string[] { "os", "name", "x" });
                {

                    builder.AppendLine("os.WriteLine(\"<{0}>{1}</{0}>\", name, x);");
                }
                builder.End();
                builder.AppendLine();
            }

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "Write",
                new string[] { "TextWriter", CSharp.String, CLASS_XML_OBJECT }, new string[] { "os", "name", "x" });
            {

                builder.AppendLine("os.WriteLine(\"<{0} Type =\\\"{1}\\\">\", name, x.GetType().Name);");

                builder.AppendLine("x.Write(os);");

                builder.AppendLine("os.WriteLine(\"</{0}>\", name);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "Write<V>",
                new string[] { "TextWriter", CSharp.String, "List<V>" }, new string[] { "os", "name", "x" });
            {

                builder.AppendLine("os.WriteLine(\"<{0}>\", name);");

                builder.AppendLine("x.ForEach(v => Write(os, \"Item\", v));");

                builder.AppendLine("os.WriteLine(\"</{0}>\", name);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "Write<K, V>",
                new string[] { "TextWriter", CSharp.String, "Dictionary<K, V>" }, new string[] { "os", "name", "x" });
            {

                builder.AppendLine("os.WriteLine(\"<{0}>\", name);");

                builder.Append("foreach (var e in x)");
                builder.Start();
                {

                    builder.AppendLine("os.WriteLine(\"<Pair>\");");

                    builder.AppendLine("Write(os, \"Key\", e.Key);");

                    builder.AppendLine("Write(os, \"Value\", e.Value);");

                    builder.AppendLine("os.WriteLine(\"</Pair>\");");
                }
                builder.End();

                builder.AppendLine("os.WriteLine(\"</{0}>\", name);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "Write",
                new string[] { "TextWriter", CSharp.String, CSharp.Object }, new string[] { "os", "name", "x" });
            {

                builder.AppendLine("if (x is bool)");

                builder.AppendLine("\tWrite(os, name, (bool)x);");

                builder.AppendLine("else if (x is int)");

                builder.AppendLine("\tWrite(os, name, (int)x);");

                builder.AppendLine("else if (x is long)");

                builder.AppendLine("\tWrite(os, name, (long)x);");

                builder.AppendLine("else if (x is float)");

                builder.AppendLine("\tWrite(os, name, (float)x);");

                builder.AppendLine("else if (x is string)");

                builder.AppendLine("\tWrite(os, name, (string)x);");

                builder.AppendFormat("else if (x is {0})\n", CLASS_XML_OBJECT);

                builder.AppendFormat("\tWrite(os, name, ({0})x);\n", CLASS_XML_OBJECT);

                builder.AppendLine("else");

                builder.AppendLine("\tthrow new Exception(\"unknown Lson type; \" + x.GetType());");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public, CSharp.Void, "LoadAConfig",
                new string[] { CSharp.String }, new string[] { "file" });
            {

                builder.AppendLine("var doc = new XmlDocument();");

                builder.AppendLine("doc.Load(file);");

                builder.AppendLine("Read(doc.DocumentElement);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public, CSharp.Void, "SaveAConfig",
                new string[] { CSharp.String }, new string[] { "file" });
            {

                builder.AppendLine("var os = new StringWriter();");

                builder.AppendLine("Write(os, \"Root\", this);");

                builder.AppendLine("File.WriteAllText(file, os.ToString());");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "LoadConfig<T>",
                new string[] { "List<T>", CSharp.String }, new string[] { "x", "file" }, CLASS_XML_OBJECT);
            {

                builder.AppendLine("var doc = new XmlDocument();");

                builder.AppendLine("doc.Load(file);");

                builder.AppendLine("x.AddRange(GetChilds(doc.DocumentElement).Select(_ => ReadDynamicObject<T>(_, typeof(T).Namespace)));");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "SaveConfig<T>",
                new string[] { "List<T>", CSharp.String }, new string[] { "x", "file" }, CLASS_XML_OBJECT);
            {

                builder.AppendLine("var os = new StringWriter();");

                builder.AppendLine("Write(os, \"Root\", x);");

                builder.AppendLine("File.WriteAllText(file, os.ToString());");
            }
            builder.End();
            builder.AppendLine();

            builder.EndAll();
            Util.SaveFile(path, builder.ToString());
            builder.Clear();
        }
        private static void GenXmlClass()
        {
            CodeWriter builder = new CodeWriter();
            CodeWriter reader = new CodeWriter();
            CodeWriter writer = new CodeWriter();
            writer.SetLevel(3);
            reader.SetLevel(4);
            var cit = ClassWrap.Classes.GetEnumerator();
            while (cit.MoveNext())
            {
                var cls = cit.Current.Value;
                builder.UsingNamespace(XmlNameSpaces);
                builder.AppendLine();
                builder.NameSpace(Setting.EditorName + cls.Namespace);
                builder.Comments(cls.Desc);
                string inherit = cls.Inherit.IsEmpty() ? CLASS_XML_OBJECT : cls.Inherit;
                builder.DefineClass(CodeWriter.Public, cls.Name, CLASS_XML_OBJECT);

                //普通字段
                for (int i = 0; i < cls.Fields.Count; i++)
                {
                    var field = cls.Fields[i];
                    if (!Util.MatchGroups(field.Group)) continue;

                    builder.Comments(field.Desc);
                    if (field.IsRaw || field.IsEnum || field.IsClass)
                        builder.DefineField(CodeWriter.Public, ToXmlType(field.FullType), field.Name);
                    else if (field.IsContainer)
                    {
                        string modifier = string.Format("{0} {1}", CodeWriter.Public, CodeWriter.Readonly);
                        if (field.OriginalType == Setting.LIST)
                        {
                            string type = ToXmlType(field.Types[1]);
                            string init = string.Format("new List<{0}>()", type);
                            string fullType = string.Format("List<{0}>", type);
                            builder.DefineField(modifier, fullType, field.Name, init);
                        }
                        else if (field.OriginalType == Setting.DICT)
                        {
                            string type = ToXmlType(field.Types[2]);
                            string init = string.Format("new Dictionary<{0}, {1}>()", field.Types[1], type);
                            string fullType = string.Format("Dictionary<{0}, {1}>", field.Types[1], type);
                            builder.DefineField(modifier, fullType, field.Name, init);
                        }
                    }

                    writer.AppendLine(WriteField(field));

                    string r0 = string.Format("case \"{0}\": {0} = {1}; break;", field.Name, ReadField(field, 2));
                    string r1 = string.Format("case \"{0}\": {1}; break;", field.Name, ReadField(field, 2));
                    reader.AppendLine(!field.IsContainer ? r0 : r1);
                }

                builder.AppendLine(false);
                builder.Function(CodeWriter.Public + " " + CodeWriter.Override, CSharp.Void, "Write", new string[] { "TextWriter" }, new string[] { "_1" });
                builder.Append(writer.ToString(), false);
                builder.End();
                builder.Function(CodeWriter.Public + " " + CodeWriter.Override, CSharp.Void, "Read", new string[] { "XmlNode" }, new string[] { "_1" });
                builder.AppendLine("foreach (System.Xml.XmlNode _2 in GetChilds (_1))");
                builder.Append("switch (_2.Name)");
                builder.Start();
                builder.Append(reader.ToString(), false);
                builder.End();
                builder.EndAll();

                string file = string.Format("{0}{1}.cs", Setting.EditorName, cls.FullType);
                string path = Path.Combine(Setting.XmlCodeDir, file);
                Util.SaveFile(path, builder.ToString());

                builder.Clear();
                writer.Clear();
                reader.Clear();
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
        private static string WriteField(FieldWrap field)
        {
            return string.Format("Write(_1, \"{0}\", this.{0});", field.Name);
        }
        private static string ReadField(FieldWrap field, int arg)
        {
            if (field.IsRaw)
                return string.Format("Read{0}(_{1})", field.FullType.FirstCharUpper(), arg);
            else if (field.IsEnum)
                return string.Format("({0})ReadInt(_{1})", ToXmlType(field.FullType), arg);
            else if (field.IsClass)
                return string.Format("ReadObject<{0}>(_{1}, \"{0}\")", ToXmlType(field.FullType), arg);
            else if (field.IsContainer)
            {
                if (field.OriginalType == Setting.LIST)
                {
                    var item = field.GetItemDefine();
                    string value = ReadField(item, arg + 1);
                    return string.Format("GetChilds(_{0}).ForEach (_{1} => {2}.Add({3}))", arg, arg + 1, field.Name, value);
                }
                else if (field.OriginalType == Setting.DICT)
                {
                    var key = field.GetKeyDefine();
                    var value = field.GetValueDefine();
                    int arg1 = arg + 1;
                    string skey = ReadField(key, arg1).Replace("_" + arg1.ToString(), "GetOnlyChild(_3, \"Key\")");
                    string svalue = ReadField(value, arg1).Replace("_" + arg1.ToString(), "GetOnlyChild(_3, \"Value\")");
                    return string.Format("GetChilds(_{0}).ForEach (_{1} => {2}.Add({3}, {4}))", arg, arg1, field.Name, skey, svalue);
                }
            }

            Util.Error("未知类型:" + field.FullType);
            return null;
        }
        public static string ToXmlType(string fullType)
        {
            if (fullType.StartsWith(Setting.ModuleName))
                return Setting.EditorName + fullType;
            return fullType;
        }
        private static void GenXmlEnums()
        {
            CodeWriter builder = new CodeWriter();
            List<EnumWrap> exports = EnumWrap.GetExports();
            for (int i = 0; i < exports.Count; i++)
            {
                EnumWrap en = exports[i];
                builder.NameSpace(Setting.EditorName + en.Namespace);
                builder.Comments(en.Desc);
                builder.Enum(CodeWriter.Public, en.Name);
                foreach (var item in en.Values)
                    builder.DefineEnum(item.Key, item.Value.ToString());

                builder.EndAll();
                string path = string.Format("{0}{1}{2}.{3}.cs", Setting.XmlCodeDir, Setting.EditorName, en.Namespace, en.Name);
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
    }
}
