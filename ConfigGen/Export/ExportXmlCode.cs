using System;
using System.Text;
using ConfigGen.Description;
using System.Collections.Generic;
using System.IO;
using ConfigGen.TypeInfo;

namespace ConfigGen.Export
{
    public class ExportXmlCode
    {
        private const string XML_STREAM = "XmlManager";
        private const string CLASS_XML_OBJECT = "XmlObject";
        private static readonly List<string> XmlNameSpaces = new List<string>() { "System", "System.IO", "System.Linq", "System.Xml", "System.Collections.Generic" };


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
            string path = Path.Combine(Consts.XmlCodeDir, CLASS_XML_OBJECT + ".cs");
            builder.UsingNamespace(XmlNameSpaces);
            builder.AppendLine();
            builder.NameSpace(Consts.XmlRootNode);

            builder.DefineClass(CodeWriter.Public, CodeWriter.Abstract, CLASS_XML_OBJECT);
            builder.IntervalLevel();
            builder.AppendLine("public abstract void Write(TextWriter os);");
            builder.IntervalLevel();
            builder.AppendLine("public abstract void Read(XmlNode os);\n\n");

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.String, "ReadAttribute",
                new string[] { "XmlNode", CSharp.String }, new string[] { "node", "attribute" });
            {
                builder.IntervalLevel();
                builder.Append("try");
                builder.Start();
                {
                    builder.IntervalLevel();
                    builder.AppendLine("if (node != null && node.Attributes != null && node.Attributes[attribute] != null)");
                    builder.IntervalLevel();
                    builder.AppendLine("\treturn node.Attributes[attribute].Value;");
                }
                builder.End();
                builder.IntervalLevel();
                builder.Append("catch (Exception ex)");
                builder.Start();
                {
                    builder.IntervalLevel();
                    builder.AppendLine("throw new Exception(string.Format(\"attribute:{0} not exist\", attribute), ex);");
                }
                builder.End();
                builder.IntervalLevel();
                builder.AppendLine("return \"\";");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, "XmlNode", "GetOnlyChild",
                new string[] { "XmlNode", CSharp.String }, new string[] { "parent", "name" });
            {
                builder.IntervalLevel();
                builder.AppendLine("XmlNode child = null;");
                builder.IntervalLevel();
                builder.Append("foreach (XmlNode sub in parent.ChildNodes)");
                builder.Start();
                {
                    builder.IntervalLevel();
                    builder.Append("if (sub.NodeType == XmlNodeType.Element && sub.Name == name)");
                    builder.Start();
                    {
                        builder.IntervalLevel();
                        builder.AppendLine("if (child != null)");
                        builder.IntervalLevel();
                        builder.AppendLine("\tthrow new Exception(string.Format(\"child:{0} duplicate\", name));");
                        builder.IntervalLevel();
                        builder.AppendLine("child = sub;");
                    }
                    builder.End();

                }
                builder.End();
                builder.IntervalLevel();
                builder.AppendLine("return child;");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, "List<XmlNode>", "GetChilds",
                new string[] { "XmlNode" }, new string[] { "parent" });
            {
                builder.IntervalLevel();
                builder.AppendLine("var childs = new List<XmlNode>();");
                builder.IntervalLevel();
                builder.AppendLine("if (parent != null)");
                builder.IntervalLevel();
                builder.AppendLine("\tchilds.AddRange(parent.ChildNodes.Cast<XmlNode>().Where(sub => sub.NodeType == XmlNodeType.Element));");
                builder.IntervalLevel();
                builder.AppendLine("return childs;");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Bool, "ReadBool",
                new string[] { "XmlNode" }, new string[] { "node" });
            {
                builder.IntervalLevel();
                builder.AppendLine("string str = node.InnerText.ToLower();");
                builder.IntervalLevel();
                builder.AppendLine("if (str == \"true\")");
                builder.IntervalLevel();
                builder.AppendLine("\treturn true;");
                builder.IntervalLevel();
                builder.AppendLine("else if (str == \"false\")");
                builder.IntervalLevel();
                builder.AppendLine("\treturn false;");
                builder.IntervalLevel();
                builder.AppendLine("throw new Exception(string.Format(\"'{0}' is not valid bool\", str));");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Int, "ReadInt",
                new string[] { "XmlNode" }, new string[] { "node" });
            {
                builder.IntervalLevel();
                builder.AppendLine("return int.Parse(node.InnerText);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Long, "ReadLong",
                new string[] { "XmlNode" }, new string[] { "node" });
            {
                builder.IntervalLevel();
                builder.AppendLine("return long.Parse(node.InnerText);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Float, "ReadFloat",
                new string[] { "XmlNode" }, new string[] { "node" });
            {
                builder.IntervalLevel();
                builder.AppendLine("return float.Parse(node.InnerText);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.String, "ReadString",
                new string[] { "XmlNode" }, new string[] { "node" });
            {
                builder.IntervalLevel();
                builder.AppendLine("return node.InnerText;");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, "T", "ReadObject<T>",
                new string[] { "XmlNode", CSharp.String }, new string[] { "node", "fullTypeName" }, CLASS_XML_OBJECT);
            {
                builder.IntervalLevel();
                builder.AppendLine("var obj = (T)Create(node, fullTypeName);");
                builder.IntervalLevel();
                builder.AppendLine("obj.Read(node);");
                builder.IntervalLevel();
                builder.AppendLine("return obj;");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, "T", "ReadDynamicObject<T>",
                new string[] { "XmlNode", CSharp.String }, new string[] { "node", "ns" }, CLASS_XML_OBJECT);
            {
                builder.IntervalLevel();
                builder.AppendLine("var fullTypeName = ns + \".\" + ReadAttribute(node, \"Type\");");
                builder.IntervalLevel();
                builder.AppendLine("return ReadObject<T>(node, fullTypeName);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Object, "Create",
                new string[] { "XmlNode", CSharp.String }, new string[] { "node", "type" });
            {
                builder.IntervalLevel();
                builder.Append("try");
                builder.Start();
                {
                    builder.IntervalLevel();
                    builder.AppendLine("var t = Type.GetType(type);");
                    builder.IntervalLevel();
                    builder.AppendLine("return Activator.CreateInstance(t);");
                }
                builder.End();
                builder.IntervalLevel();
                builder.Append("catch (Exception e)");
                builder.Start();
                {
                    builder.IntervalLevel();
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
                    builder.IntervalLevel();
                    builder.AppendLine("os.WriteLine(\"<{0}>{1}</{0}>\", name, x);");
                }
                builder.End();
                builder.AppendLine();
            }

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "Write",
                new string[] { "TextWriter", CSharp.String, CLASS_XML_OBJECT }, new string[] { "os", "name", "x" });
            {
                builder.IntervalLevel();
                builder.AppendLine("os.WriteLine(\"<{0} Type =\\\"{1}\\\">\", name, x.GetType().Name);");
                builder.IntervalLevel();
                builder.AppendLine("x.Write(os);");
                builder.IntervalLevel();
                builder.AppendLine("os.WriteLine(\"</{0}>\", name);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "Write<V>",
                new string[] { "TextWriter", CSharp.String, "List<V>" }, new string[] { "os", "name", "x" });
            {
                builder.IntervalLevel();
                builder.AppendLine("os.WriteLine(\"<{0}>\", name);");
                builder.IntervalLevel();
                builder.AppendLine("x.ForEach(v => Write(os, \"Item\", v));");
                builder.IntervalLevel();
                builder.AppendLine("os.WriteLine(\"</{0}>\", name);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "Write<K, V>",
                new string[] { "TextWriter", CSharp.String, "Dictionary<K, V>" }, new string[] { "os", "name", "x" });
            {
                builder.IntervalLevel();
                builder.AppendLine("os.WriteLine(\"<{0}>\", name);");
                builder.IntervalLevel();
                builder.Append("foreach (var e in x)");
                builder.Start();
                {
                    builder.IntervalLevel();
                    builder.AppendLine("os.WriteLine(\"<Pair>\");");
                    builder.IntervalLevel();
                    builder.AppendLine("Write(os, \"Key\", e.Key);");
                    builder.IntervalLevel();
                    builder.AppendLine("Write(os, \"Value\", e.Value);");
                    builder.IntervalLevel();
                    builder.AppendLine("os.WriteLine(\"</Pair>\");");
                }
                builder.End();
                builder.IntervalLevel();
                builder.AppendLine("os.WriteLine(\"</{0}>\", name);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "Write",
                new string[] { "TextWriter", CSharp.String, CSharp.Object }, new string[] { "os", "name", "x" });
            {
                builder.IntervalLevel();
                builder.AppendLine("if (x is bool)");
                builder.IntervalLevel();
                builder.AppendLine("\tWrite(os, name, (bool)x);");
                builder.IntervalLevel();
                builder.AppendLine("else if (x is int)");
                builder.IntervalLevel();
                builder.AppendLine("\tWrite(os, name, (int)x);");
                builder.IntervalLevel();
                builder.AppendLine("else if (x is long)");
                builder.IntervalLevel();
                builder.AppendLine("\tWrite(os, name, (long)x);");
                builder.IntervalLevel();
                builder.AppendLine("else if (x is float)");
                builder.IntervalLevel();
                builder.AppendLine("\tWrite(os, name, (float)x);");
                builder.IntervalLevel();
                builder.AppendLine("else if (x is string)");
                builder.IntervalLevel();
                builder.AppendLine("\tWrite(os, name, (string)x);");
                builder.IntervalLevel();
                builder.AppendFormat("else if (x is {0})\n", CLASS_XML_OBJECT);
                builder.IntervalLevel();
                builder.AppendFormat("\tWrite(os, name, ({0})x);\n", CLASS_XML_OBJECT);
                builder.IntervalLevel();
                builder.AppendLine("else");
                builder.IntervalLevel();
                builder.AppendLine("\tthrow new Exception(\"unknown Lson type; \" + x.GetType());");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public, CSharp.Void, "LoadAConfig",
                new string[] { CSharp.String }, new string[] { "file" });
            {
                builder.IntervalLevel();
                builder.AppendLine("var doc = new XmlDocument();");
                builder.IntervalLevel();
                builder.AppendLine("doc.Load(file);");
                builder.IntervalLevel();
                builder.AppendLine("Read(doc.DocumentElement);");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public, CSharp.Void, "SaveAConfig",
                new string[] { CSharp.String }, new string[] { "file" });
            {
                builder.IntervalLevel();
                builder.AppendLine("var os = new StringWriter();");
                builder.IntervalLevel();
                builder.AppendLine("Write(os, \"Root\", this);");
                builder.IntervalLevel();
                builder.AppendLine("File.WriteAllText(file, os.ToString());");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "LoadConfig<T>",
                new string[] { "List<T>", CSharp.String }, new string[] { "x", "file" }, CLASS_XML_OBJECT);
            {
                builder.IntervalLevel();
                builder.AppendLine("var doc = new XmlDocument();");
                builder.IntervalLevel();
                builder.AppendLine("doc.Load(file);");
                builder.IntervalLevel();
                builder.AppendLine("x.AddRange(GetChilds(doc.DocumentElement).Select(_ => ReadDynamicObject<T>(_, typeof(T).Namespace)));");
            }
            builder.End();
            builder.AppendLine();

            builder.Function(CodeWriter.Public + " " + CodeWriter.Static, CSharp.Void, "SaveConfig<T>",
                new string[] { "List<T>", CSharp.String }, new string[] { "x", "file" }, CLASS_XML_OBJECT);
            {
                builder.IntervalLevel();
                builder.AppendLine("var os = new StringWriter();");
                builder.IntervalLevel();
                builder.AppendLine("Write(os, \"Root\", x);");
                builder.IntervalLevel();
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
            var cit = ClassInfo.Classes.GetEnumerator();
            while (cit.MoveNext())
            {
                var cls = cit.Current.Value;
                builder.UsingNamespace(XmlNameSpaces);
                builder.AppendLine();
                builder.NameSpace(cls.FullName);

                //常量字段
                for (int j = 0; j < cls.Consts.Count; j++)
                {
                    ConstInfo field = cls.Consts[j];
                    builder.Comments(field.Desc);
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
                    builder.DefineConst(type, field.Name, value);
                }

                //普通字段
                for (int i = 0; i < cls.Fields.Count; i++)
                {
                    var field = cls.Fields[i];
                    if (!Util.MatchGroups(field.Group)) continue;

                    builder.Comments(field.Desc);
                    string modifier = string.Format("{0} {1}", CodeWriter.Public, CodeWriter.Readonly);
                    if (field.IsRaw || field.IsEnum || field.IsClass)
                        builder.DefineField(modifier, field.FullType, field.Name);
                    else if (field.IsContainer)
                    {
                        string init = "-";
                        if (field.OriginalType == Consts.LIST)
                            init = string.Format("new List<{0}>", field.Types[1]);
                        else if (field.OriginalType == Consts.DICT)
                            init = string.Format("new Dictionary<{0}, {1}>", field.Types[1], field.Types[2]);
                        builder.DefineField(modifier, field.FullType, field.Name, init);
                    }

                    writer.AppendLine(WriteField(field));

                    string r0 = string.Format("case \"{0}\": {0} = {1};", field.Name, ReadField(field, 2));
                    string r1 = string.Format("case \"{0}\": {1};", field.Name, ReadField(field, 2));
                    reader.AppendLine(field.IsContainer ? r0 : r1);
                }

                builder.Function(CodeWriter.Public + " " + CodeWriter.Override, CSharp.Void, "Write", new string[] { "TextWriter" }, new string[] { "_1" });
                builder.AppendLine(writer.ToString());
                builder.End();
                builder.Function(CodeWriter.Public + " " + CodeWriter.Override, CSharp.Void, "Read", new string[] { "XmlNode" }, new string[] { "_1" });
                builder.AppendLine("foreach (System.Xml.XmlNode _2 in GetChilds (_1))");
                builder.AppendLine("switch (_2.Name)");
                builder.AddLevel();
                builder.AppendLine(reader.ToString());
                builder.EndAll();

                string file = string.Format("{0}{1}.cs", Consts.XmlRootNode, cls.FullName);
                string path = Path.Combine(Consts.XmlCodeDir, file);
                Util.SaveFile(path, builder.ToString());
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
        private static string WriteField(FieldInfo field)
        {
            return string.Format("Write(_1, \"{0}\", this.{0});", field.Name);
        }
        private static string ReadField(FieldInfo field, int arg)
        {
            if (field.IsRaw)
                return string.Format("Read{0}(_{1})", field.FullType, arg);
            else if (field.IsEnum)
                return string.Format("({0})ReadInt(_{1})", field.FullType, arg);
            else if (field.IsClass)
                return string.Format("ReadObject<{0}>(_{1}, {0})", field.FullType, arg);
            else if (field.IsContainer)
            {
                if (field.OriginalType == Consts.LIST)
                {
                    var item = field.GetItemDefine();
                    string value = ReadField(item, arg + 1);
                    return string.Format("GetChilds(_{0}).ForEach (_{1} => {2}.Add({3}))", arg, arg + 1, field.Name, value);
                }
                else if (field.OriginalType == Consts.DICT)
                {
                    var key = field.GetKeyDefine();
                    var value = field.GetValueDefine();
                    int arg1 = arg + 1;
                    string skey = ReadField(key, arg1).Replace(arg1.ToString(), "GetOnlyChild(_3, \"Key\")");
                    string svalue = ReadField(value, arg1).Replace(arg1.ToString(), "GetOnlyChild(_3, \"Value\")");
                    return string.Format("GetChilds(_{0}).ForEach (_{1} => {2}.Add({3}, {4}))", arg, arg1, field.Name, skey, svalue);
                }
            }

            Util.Error("未知类型:" + field.FullType);
            return null;
        }
        private static void GenXmlEnums()
        {
            CodeWriter builder = new CodeWriter();
            List<EnumInfo> exports = EnumInfo.GetExports();
            for (int i = 0; i < exports.Count; i++)
            {
                EnumInfo en = exports[i];
                builder.Comments(en.Desc);
                builder.Enum(CodeWriter.Public, en.Name);
                var eit = en.Values.GetEnumerator();
                while (eit.MoveNext())
                {
                    var item = eit.Current;
                    builder.DefineEnum(item.Key, item.Value);
                }

                builder.EndAll();
                string path = Path.Combine(Consts.CSDir, en.Namespace, en.Name + ".cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
    }
}
