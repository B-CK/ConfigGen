using System;
using System.Text;
using ConfigGen.LocalInfo;
using System.Collections.Generic;
using System.IO;

namespace ConfigGen.Export
{
    public partial class ExportCSharp
    {
        private const string XML_STREAM = "XmlManager";
        private const string CLASS_XML_OBJECT = "XmlObject";
        private static readonly List<string> XmlNameSpaces = new List<string>() { "System", "System.IO", "System.Xml", "System.Collections.Generic" };


        /// <summary>
        /// 编辑模式下的Xml读写操作
        /// </summary>
        public static void Export_LsonOp()
        {
            DefineLsonObject();
            GenLsonClassScripts();
        }
        private static void DefineLsonObject()
        {
            StringBuilder builder = new StringBuilder();
            string path = Path.Combine(Values.ExportXmlCode, CLASS_XML_OBJECT + ".cs");
            CodeWriter.UsingNamespace(builder, XmlNameSpaces);
            builder.AppendLine();
            CodeWriter.NameSpace(builder, Values.XmlRootNode);

            CodeWriter.ClassBase(builder, CodeWriter.Public, CodeWriter.Abstract, CLASS_XML_OBJECT);
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("public abstract void Write(TextWriter os);");
            CodeWriter.IntervalLevel(builder);
            builder.AppendLine("public abstract void Read(XmlNode os);\n\n");

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.String, "ReadAttribute",
                new string[] { "XmlNode", Base.String }, new string[] { "node", "attribute" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.Append("try");
                CodeWriter.Start(builder);
                {
                    CodeWriter.IntervalLevel(builder);
                    builder.AppendLine("if (node != null && node.Attributes != null && node.Attributes[attribute] != null)");
                    CodeWriter.IntervalLevel(builder);
                    builder.AppendLine("\treturn node.Attributes[attribute].Value;");
                }
                CodeWriter.End(builder);
                CodeWriter.IntervalLevel(builder);
                builder.Append("catch (Exception ex)");
                CodeWriter.Start(builder);
                {
                    CodeWriter.IntervalLevel(builder);
                    builder.AppendLine("throw new Exception(string.Format(\"attribute:{0} not exist\", attribute), ex);");
                }
                CodeWriter.End(builder);
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("return \"\";");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, "XmlNode", "GetOnlyChild",
                new string[] { "XmlNode", Base.String }, new string[] { "parent", "name" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("XmlNode child = null;");
                CodeWriter.IntervalLevel(builder);
                builder.Append("foreach (XmlNode sub in parent.ChildNodes)");
                CodeWriter.Start(builder);
                {
                    CodeWriter.IntervalLevel(builder);
                    builder.Append("if (sub.NodeType == XmlNodeType.Element && sub.Name == name)");
                    CodeWriter.Start(builder);
                    {
                        CodeWriter.IntervalLevel(builder);
                        builder.AppendLine("if (child != null)");
                        CodeWriter.IntervalLevel(builder);
                        builder.AppendLine("\tthrow new Exception(string.Format(\"child:{0} duplicate\", name));");
                        CodeWriter.IntervalLevel(builder);
                        builder.AppendLine("child = sub;");
                    }
                    CodeWriter.End(builder);

                }
                CodeWriter.End(builder);
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("return child;");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, "List<XmlNode>", "GetChilds",
                new string[] { "XmlNode" }, new string[] { "parent" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("var childs = new List<XmlNode>();");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("if (parent != null)");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("\tchilds.AddRange(parent.ChildNodes.Cast<XmlNode>().Where(sub => sub.NodeType == XmlNodeType.Element));");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("return childs;");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Bool, "ReadBool",
                new string[] { "XmlNode" }, new string[] { "node" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("string str = node.InnerText.ToLower();");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("if (str == \"true\")");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("\treturn true;");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("else if (str == \"false\")");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("\treturn false;");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("throw new Exception(string.Format(\"'{0}' is not valid bool\", str));");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Int, "ReadInt",
                new string[] { "XmlNode" }, new string[] { "node" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("return int.Parse(node.InnerText);");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Long, "ReadLong",
                new string[] { "XmlNode" }, new string[] { "node" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("return long.Parse(node.InnerText);");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Float, "ReadFloat",
                new string[] { "XmlNode" }, new string[] { "node" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("return float.Parse(node.InnerText);");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.String, "ReadString",
                new string[] { "XmlNode" }, new string[] { "node" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("return node.InnerText;");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, "T", "ReadObject<T>",
                new string[] { "XmlNode", Base.String }, new string[] { "node", "fullTypeName" }, CLASS_XML_OBJECT);
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("var obj = (T)Create(node, fullTypeName);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("obj.Read(node);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("return obj;");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, "T", "ReadDynamicObject<T>",
                new string[] { "XmlNode", Base.String }, new string[] { "node", "ns" }, CLASS_XML_OBJECT);
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("var fullTypeName = ns + \".\" + ReadAttribute(node, \"Type\");");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("return ReadObject<T>(node, fullTypeName);");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Object, "Create",
                new string[] { "XmlNode", Base.String }, new string[] { "node", "type" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.Append("try");
                CodeWriter.Start(builder);
                {
                    CodeWriter.IntervalLevel(builder);
                    builder.AppendLine("var t = Type.GetType(type);");
                    CodeWriter.IntervalLevel(builder);
                    builder.AppendLine("return Activator.CreateInstance(t);");
                }
                CodeWriter.End(builder);
                CodeWriter.IntervalLevel(builder);
                builder.Append("catch (Exception e)");
                CodeWriter.Start(builder);
                {
                    CodeWriter.IntervalLevel(builder);
                    builder.AppendLine("throw new Exception(string.Format(\"type:{0}create fail!\", type), e);");
                }
                CodeWriter.End(builder);
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            List<string> ls = new List<string>() { Base.Bool, Base.Int, Base.Long, Base.Float, Base.String };
            foreach (var item in ls)
            {
                CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Void, "Write",
                    new string[] { "TextWriter", Base.String, item }, new string[] { "os", "name", "x" });
                {
                    CodeWriter.IntervalLevel(builder);
                    builder.AppendLine("os.WriteLine(\"<{0}>{1}</{0}>\", name, x);");
                }
                CodeWriter.End(builder);
                builder.AppendLine();
            }

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Void, "Write",
                new string[] { "TextWriter", Base.String, CLASS_XML_OBJECT }, new string[] { "os", "name", "x" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("os.WriteLine(\"<{0} type =\\\"{1}\\\">\", name, x.GetType().Name);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("x.Write(os);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("os.WriteLine(\"</{0}>\", name);");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Void, "Write<V>",
                new string[] { "TextWriter", Base.String, "List<V>" }, new string[] { "os", "name", "x" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("os.WriteLine(\"<{0}>\", name);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("x.ForEach(v => Write(os, \"Item\", v));");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("os.WriteLine(\"</{0}>\", name);");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Void, "Write<K, V>",
                new string[] { "TextWriter", Base.String, "Dictionary<K, V>" }, new string[] { "os", "name", "x" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("os.WriteLine(\"<{0}>\", name);");
                CodeWriter.IntervalLevel(builder);
                builder.Append("foreach (var e in x)");
                CodeWriter.Start(builder);
                {
                    CodeWriter.IntervalLevel(builder);
                    builder.AppendLine("os.WriteLine(\"<Pair>\");");
                    CodeWriter.IntervalLevel(builder);
                    builder.AppendLine("Write(os, \"Key\", e.Key);");
                    CodeWriter.IntervalLevel(builder);
                    builder.AppendLine("Write(os, \"Value\", e.Value);");
                    CodeWriter.IntervalLevel(builder);
                    builder.AppendLine("os.WriteLine(\"</Pair>\");");
                }
                CodeWriter.End(builder);
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("os.WriteLine(\"</{0}>\", name);");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.Void, "Write",
                new string[] { "TextWriter", Base.String, Base.Object }, new string[] { "os", "name", "x" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("if (x is bool)");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("\tWrite(os, name, (bool)x);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("else if (x is int)");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("\tWrite(os, name, (int)x);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("else if (x is long)");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("\tWrite(os, name, (long)x);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("else if (x is float)");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("\tWrite(os, name, (float)x);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("else if (x is string)");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("\tWrite(os, name, (string)x);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendFormat("else if (x is {0})\n", CLASS_XML_OBJECT);
                CodeWriter.IntervalLevel(builder);
                builder.AppendFormat("\tWrite(os, name, ({0})x);\n", CLASS_XML_OBJECT);
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("else");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("\tthrow new Exception(\"unknown Lson type; \" + x.GetType());");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, Base.String, "LoadAConfig",
                new string[] { Base.String }, new string[] { "file" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("var doc = new XmlDocument();");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("doc.Load(file);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("Read(doc.DocumentElement);");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, Base.String, "SaveAConfig",
                new string[] { Base.String }, new string[] { "file" });
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("var os = new StringWriter();");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("Write(os, \"Root\", this);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("File.WriteAllText(file, os.ToString());");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.String, "LoadConfig<T>",
                new string[] { "List<T>", Base.String }, new string[] { "x", "file" }, CLASS_XML_OBJECT);
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("var doc = new XmlDocument();");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("doc.Load(file);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("x.AddRange(GetChilds(doc.DocumentElement).Select(_ => ReadDynamicObject<T>(_, typeof(T).Namespace)));");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Static, Base.String, "SaveConfig<T>",
                new string[] { "List<T>", Base.String }, new string[] { "x", "file" }, CLASS_XML_OBJECT);
            {
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("var os = new StringWriter();");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("Write(os, \"Root\", x);");
                CodeWriter.IntervalLevel(builder);
                builder.AppendLine("File.WriteAllText(file, os.ToString());");
            }
            CodeWriter.End(builder);
            builder.AppendLine();

            CodeWriter.EndAll(builder);
            Util.SaveFile(path, builder.ToString());
            builder.Clear();
        }
        private static void GenLsonClassScripts()
        {
            StringBuilder builder = new StringBuilder();
            List<string> writer = new List<string>();
            List<string> reader = new List<string>();
            List<BaseTypeInfo> typeInfos = new List<BaseTypeInfo>();
            typeInfos.AddRange(TypeInfo.Instance.ClassInfos);
            typeInfos.AddRange(TypeInfo.Instance.EnumInfos);
            foreach (BaseTypeInfo baseType in typeInfos)
            {
                CodeWriter.UsingNamespace(builder, XmlNameSpaces);
                builder.AppendLine();
                CodeWriter.NameSpace(builder, string.Format("{0}.{1}", Values.XmlRootNode, baseType.NamespaceName));

                bool isWrited = false;
                if (baseType.EType == TypeType.Class)
                {
                    ClassTypeInfo classType = baseType as ClassTypeInfo;
                    if (classType.Inherit == null)
                        CodeWriter.ClassChild(builder, CodeWriter.Public, classType.Name, CLASS_XML_OBJECT);
                    else
                    {
                        string fullType = CodeWriter.GetFullNamespace(Values.XmlRootNode, classType.Inherit.GetFullName());
                        CodeWriter.ClassChild(builder, CodeWriter.Public, classType.Name, fullType);
                    }

                    //常量字段
                    for (int j = 0; j < classType.Consts.Count; j++)
                    {
                        ConstInfo field = classType.Consts[j];
                        CodeWriter.Comments(builder, field.Des);
                        string value = field.Value;
                        switch (field.Type)
                        {
                            case TypeInfo.FLOAT:
                                value = string.Format("{0}f", field.Value);
                                break;
                            case TypeInfo.STRING:
                                value = string.Format("@\"{0}\"", field.Value);
                                break;
                        }
                        CodeWriter.FieldInit(builder, CodeWriter.Public, CodeWriter.Const, field.Type, field.Name, value);
                    }

                    writer.Clear();
                    reader.Clear();
                    isWrited = true;
                    for (int j = 0; j < classType.Fields.Count; j++)
                    {
                        FieldInfo field = classType.Fields[j];
                        if (field.BaseInfo.EType == TypeType.Enum)
                            writer.Add(string.Format("Write(_1, \"{0}\", (int)this.{0});", field.Name));
                        else
                            writer.Add(string.Format("Write(_1, \"{0}\", this.{0});", field.Name));
                        switch (field.BaseInfo.EType)
                        {
                            case TypeType.Base:
                            case TypeType.Enum:
                            case TypeType.Class:
                                {
                                    CodeWriter.Comments(builder, field.Des);
                                    string fullType = CodeWriter.GetFullNamespace(Values.XmlRootNode, field.Type);
                                    CodeWriter.Field(builder, CodeWriter.Public, fullType, field.Name);

                                    if (field.BaseInfo.EType == TypeType.Base)
                                        reader.Add(string.Format("case \"{0}\": this.{0} = Read{1}(_2); break;", field.Name, Util.FirstCharUpper(field.Type)));
                                    else if (field.BaseInfo.EType == TypeType.Enum)
                                        reader.Add(string.Format("case \"{0}\": this.{0} = ({1}.{2})ReadInt(_2); break;", field.Name, Values.XmlRootNode, field.BaseInfo.GetFullName()));
                                    else if (field.BaseInfo.EType == TypeType.Class)
                                    {
                                        var classInfo = field.BaseInfo as ClassTypeInfo;
                                        string classFullName = string.Format("{0}.{1}", Values.XmlRootNode, classInfo.GetFullName());
                                        if (classInfo.Inherit == null)
                                            reader.Add(string.Format("case \"{0}\": this.{0} = ReadObject<{1}>(_2, \"{1}\"); break;", field.Name, classFullName));
                                        else
                                            reader.Add(string.Format("case \"{0}\": this.{0} = ReadDynamicObject<{1}>(_2, \"{2}\"); break;", field.Name, classFullName, classInfo.NamespaceName));
                                    }
                                }
                                break;
                            case TypeType.List:
                                {
                                    ListTypeInfo listType = field.BaseInfo as ListTypeInfo;
                                    string type = string.Format("List<{0}>", listType.ItemType);
                                    string initValue = string.Format("new {0}()", type);
                                    CodeWriter.Comments(builder, field.Des);
                                    TypeType itemType = TypeInfo.GetTypeType(listType.ItemType);
                                    string fullType = CodeWriter.GetFullNamespace(Values.XmlRootNode, listType.ItemType);
                                    type = type.Replace(listType.ItemType, fullType);
                                    CodeWriter.Field(builder, CodeWriter.Public, type, field.Name);

                                    BaseTypeInfo item = listType.ItemInfo.BaseInfo;
                                    if (item.EType == TypeType.Base)
                                        reader.Add(string.Format("case \"{0}\": GetChilds(_2).ForEach (_3 => this.{0}.Add(Read{1}(_3))); break;", field.Name, Util.FirstCharUpper(listType.ItemType)));
                                    else if (item.EType == TypeType.Enum)
                                        reader.Add(string.Format("case \"{0}\": GetChilds(_2).ForEach (_3 => this.{0}.Add(({1}.{2})ReadInt(_3))); break;", field.Name, Values.XmlRootNode, field.BaseInfo.GetFullName()));
                                    else if (item.EType == TypeType.Class)
                                    {
                                        var classInfo = item as ClassTypeInfo;
                                        string classFullName = string.Format("{0}.{1}", Values.XmlRootNode, classInfo.GetFullName());
                                        if (classInfo.Inherit == null)
                                            reader.Add(string.Format("case \"{0}\": GetChilds(_2).ForEach (_3 => this.{0}.Add(ReadObject<{1}>(_3, \"{1}\"))); break;", field.Name, classFullName));
                                        else
                                            reader.Add(string.Format("case \"{0}\": GetChilds(_2).ForEach (_3 => this.{0}.Add(ReadDynamicObject<{1}>(_3, \"{2}\"))); break;",
                                                field.Name, classFullName, classInfo.NamespaceName));
                                    }
                                    break;
                                }
                            case TypeType.Dict:
                                {
                                    DictTypeInfo dictType = field.BaseInfo as DictTypeInfo;
                                    string type = string.Format("Dictionary<{0}, {1}>", dictType.KeyType, dictType.ValueType);
                                    string initValue = string.Format("new {0}()", type);
                                    CodeWriter.Comments(builder, field.Des);
                                    string fullType = CodeWriter.GetFullNamespace(Values.XmlRootNode, dictType.KeyType);
                                    type = type.Replace(dictType.KeyType, fullType);
                                    fullType = CodeWriter.GetFullNamespace(Values.XmlRootNode, dictType.ValueType);
                                    type = type.Replace(dictType.ValueType, fullType);
                                    CodeWriter.Field(builder, CodeWriter.Public, type, field.Name);

                                    string key = string.Format("Read{0}(GetOnlyChild(_3, \"Key\"))", Util.FirstCharUpper(dictType.KeyType));
                                    BaseTypeInfo valueInfo = dictType.ValueInfo.BaseInfo;
                                    if (valueInfo.EType == TypeType.Base)
                                    {
                                        string value = string.Format("Read{0}(GetOnlyChild(_3, \"Value\"))", Util.FirstCharUpper(dictType.ValueType));
                                        reader.Add(string.Format("case \"{0}\": GetChilds(_2).ForEach (_3 => this.{0}.Add({1}, {2}); break;", field.Name, key, value));
                                    }
                                    else if (valueInfo.EType == TypeType.Enum)
                                    {
                                        string value = string.Format("({0}.{1})ReadInt(GetOnlyChild(_3, \"Value\"))", Values.XmlRootNode, field.BaseInfo.GetFullName());
                                        reader.Add(string.Format("case \"{0}\": GetChilds(_2).ForEach (_3 => this.{0}.Add({1}, {2}); break;", field.Name, key, value));
                                    }
                                    else if (valueInfo.EType == TypeType.Class)
                                    {
                                        var classInfo = valueInfo as ClassTypeInfo;
                                        string value = null;
                                        string classFullName = string.Format("{0}.{1}", Values.XmlRootNode, classInfo.GetFullName());
                                        if (classInfo.Inherit == null)
                                            value = string.Format("ReadObject<{0}>(GetChilds(_3, \"Value\", \"{0}\")", classFullName);
                                        else
                                            value = string.Format("ReadDynamicObject<{0}>(GetChilds(_3, \"value\", \"{1}\")", classFullName, classInfo.NamespaceName);
                                        reader.Add(string.Format("case \"{0}\": GetChilds(_2).ForEach (_3 => this.{0}.Add({1}, {2}); break;", field.Name, key, value));
                                    }
                                    break;
                                }
                            case TypeType.None:
                            default:
                                break;
                        }
                    }

                    builder.AppendLine();
                    CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Override, Base.Void, "Write",
                        new string[] { "TextWriter" }, new string[] { "_1" });
                    for (int i = 0; i < writer.Count; i++)
                    {
                        CodeWriter.IntervalLevel(builder);
                        builder.AppendLine(writer[i]);
                    }
                    CodeWriter.End(builder);
                    builder.AppendLine();
                    CodeWriter.Function(builder, CodeWriter.Public, CodeWriter.Override, Base.Void, "Read",
                        new string[] { "XmlNode" }, new string[] { "_1" });
                    {
                        CodeWriter.IntervalLevel(builder);
                        builder.AppendLine("foreach (System.Xml.XmlNode _2 in GetChilds (_1))");
                        CodeWriter.IntervalLevel(builder);
                        builder.Append("switch (_2.Name)");
                        CodeWriter.Start(builder);
                        {
                            for (int i = 0; i < reader.Count; i++)
                            {
                                CodeWriter.IntervalLevel(builder);
                                builder.AppendLine(reader[i]);
                            }
                        }
                        CodeWriter.End(builder);
                    }
                    CodeWriter.End(builder);
                }
                else if (baseType.EType == TypeType.Enum)
                {
                    isWrited = true;
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

                if (isWrited)
                {
                    string file = string.Format("{0}.{1}.cs", Values.XmlRootNode, baseType.GetFullName());
                    string path = Path.Combine(Values.ExportXmlCode, file);
                    Util.SaveFile(path, builder.ToString());
                }

                builder.Clear();
                CodeWriter.Reset();
            }
        }
    }
}
