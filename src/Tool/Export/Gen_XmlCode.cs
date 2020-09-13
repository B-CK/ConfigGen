using System.IO;
using System.Text;
using Tool.Wrap;
using System.Collections.Generic;

namespace Tool.Export
{
    //可以考虑使用反射完成Cfg数据导出到Xml[序列化类]
    //编辑器模块名称
    public class Gen_XmlCode
    {
        const string CLASS_XML_OBJECT = "XmlObject";
        const string FEILD_MODIFIERS = "public";
        const string CONST_MODIFIERS = "public const";

        const int TYPE_LEVEL = 1;
        const int MEM_LEVEL = 2;
        const int SEM_LEVEL = 3;

        static StringBuilder builder = new StringBuilder();
        static List<string> namespaces = new List<string>()
        {
            "using System;",
            "using System.IO;",
            "using System.Xml;",
            "using System.Collections.Generic;",
        };

        public static void Gen()
        {
            GenClass();
            GenEnum();
            GenXmlObject();
        }

        static void Start(int n)
        {
            builder.IntervalLevel(n);
            builder.AppendLine("{");
        }
        static void End(int n)
        {
            builder.IntervalLevel(n);
            builder.AppendLine("}");
        }
        static void Comment(string comment, int n)
        {
            builder.IntervalLevel(n);
            builder.AppendLine("/// <summary>");
            builder.IntervalLevel(n);
            builder.AppendLine($"/// {comment}");
            builder.IntervalLevel(n);
            builder.AppendLine("/// <summary>");
        }

        #region 类
        static void GenClass()
        {
            List<ClassWrap> classes = ClassWrap.GetExports();
            for (int i = 0; i < classes.Count; i++)
            {
                ClassWrap cls = classes[i];
                //命名空间
                builder.AppendLine(string.Join("\r\n", namespaces));
                builder.AppendLine($"namespace {Setting.ModuleName}.{cls.Namespace}");
                Start(0);
                {
                    //类
                    Comment(cls.Desc, TYPE_LEVEL);
                    if (cls.Inherit.IsEmpty())
                        builder.IntervalLevel(TYPE_LEVEL).AppendLine($"public partial class {cls.Name} : {CLASS_XML_OBJECT}");
                    else
                        builder.IntervalLevel(TYPE_LEVEL).AppendLine($"public partial class {cls.Name} : {Util.CorrectFullType(cls.Inherit)}");
                    Start(TYPE_LEVEL);
                    {

                        StringBuilder writer = new StringBuilder();
                        StringBuilder reader = new StringBuilder();
                        for (int j = 0; j < cls.Consts.Count; j++)
                        {
                            ConstWrap constant = cls.Consts[j];
                            if (!Util.MatchGroups(constant.Group)) continue;
                            Const(constant);//常量成员
                        }
                        for (int j = 0; j < cls.Fields.Count; j++)
                        {
                            FieldWrap field = cls.Fields[j];
                            if (!Util.MatchGroups(field.Group)) continue;

                            Field(field);//字段成员
                            WriteFunc(writer, field);//写xml数据
                            ReadFunc(reader, field);//读xml数据 
                        }

                        //写函数
                        builder.IntervalLevel(MEM_LEVEL);
                        builder.AppendLine($"public override void Write(TextWriter _1)");
                        Start(MEM_LEVEL);
                        if (!cls.Inherit.IsEmpty())
                            builder.IntervalLevel(SEM_LEVEL).AppendLine("base.Write(_1);");
                        builder.Append(writer.ToString());
                        End(MEM_LEVEL);
                        //读函数
                        builder.IntervalLevel(MEM_LEVEL);
                        builder.AppendLine($"public override void Read(XmlNode _1)");
                        Start(MEM_LEVEL);
                        {
                            if (!cls.Inherit.IsEmpty())
                                builder.IntervalLevel(SEM_LEVEL).AppendLine("base.Read(_1);");
                            builder.IntervalLevel(SEM_LEVEL).AppendLine($"foreach (System.Xml.XmlNode _2 in GetChilds (_1))");
                            builder.IntervalLevel(SEM_LEVEL).AppendLine($"switch (_2.Name)");
                            Start(SEM_LEVEL);
                            builder.Append(reader.ToString());
                            End(SEM_LEVEL);
                        }
                        End(MEM_LEVEL);
                    }
                    End(TYPE_LEVEL);
                }
                End(0);
                string path = Path.Combine(Setting.XmlCodeDir, $"{Setting.ModuleName}.{cls.FullName}.cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
        static void Const(ConstWrap constant)
        {
            Comment(constant.Desc, MEM_LEVEL);
            builder.IntervalLevel(MEM_LEVEL);
            builder.AppendLine($"{CONST_MODIFIERS} {Util.CorrectFullType(constant.FullType)} {constant.Name} = {Util.CorrectConst(constant.FullType, constant.Value)};");
        }
        static void Field(FieldWrap field)
        {
            Comment(field.Desc, MEM_LEVEL);
            builder.IntervalLevel(MEM_LEVEL);
            if (field.IsRaw || field.IsEnum || field.IsClass)
                builder.AppendLine($"{FEILD_MODIFIERS} {Util.CorrectFullType(field.FullName)} {field.Name};");
            else if (field.OriginalType == Setting.LIST)
                builder.AppendLine($"{FEILD_MODIFIERS} List<{Util.CorrectFullType(field.Types[1])}> {field.Name} = new List<{Util.CorrectFullType(field.Types[1])}>();");
            else if (field.OriginalType == Setting.DICT)
                builder.AppendLine($"{FEILD_MODIFIERS} Dictionary<{field.Types[1]}, {Util.CorrectFullType(field.Types[2])}> {field.Name} = new Dictionary<{field.Types[1]}, {Util.CorrectFullType(field.Types[2])}>();");
        }
        static void WriteFunc(StringBuilder writer, FieldWrap field)
        {
            writer.IntervalLevel(SEM_LEVEL).AppendLine($"Write(_1, \"{field.Name}\", {field.Name});");
        }
        static void ReadFunc(StringBuilder reader, FieldWrap field)
        {
            int level = SEM_LEVEL + 1;
            string type = Util.CorrectFullType(field.FullName);
            if (field.IsRaw)
                reader.IntervalLevel(level).AppendLine($"case \"{field.Name}\": {field.Name} = Read{type.FirstCharUpper()}(_2); break;");
            else if (field.IsEnum)
                reader.IntervalLevel(level).AppendLine($"case \"{field.Name}\": {field.Name} = ({type})ReadInt(_2); break;");
            else if (field.IsClass)
            {
                if (field.IsDynamic)
                {
                    string ns = type.Substring(0, type.LastIndexOf('.'));
                    reader.IntervalLevel(level).AppendLine($"case \"{field.Name}\": {field.Name} = ReadDynamicObject<{type}>(_2, \"{ns}\"); break;");
                }
                else
                    reader.IntervalLevel(level).AppendLine($"case \"{field.Name}\": {field.Name} = ReadObject<{type}>(_2, \"{type}\"); break;");
            }
            else if (field.IsContainer)
            {
                var level0 = level + 1;
                reader.IntervalLevel(level).AppendLine($"case \"{field.Name}\":");
                if (field.OriginalType == Setting.LIST)
                {
                    var item = field.GetItemDefine();
                    reader.IntervalLevel(level0).AppendLine($"var {field.Name}s = GetChilds(_2);");
                    reader.IntervalLevel(level0).AppendLine($"for (int i = 0; i < {field.Name}s.Count; i++)");
                    reader.IntervalLevel(level0).AppendLine($"{{");
                    reader.IntervalLevel(level0 + 1).AppendLine($"var _3 = {field.Name}s[i];");
                    reader.IntervalLevel(level0 + 1).AppendLine($"{field.Name}.Add({ReadList(item)});");
                    reader.IntervalLevel(level0).AppendLine($"}}");
                    reader.IntervalLevel(level0).AppendLine($"break;");
                }
                else if (field.OriginalType == Setting.DICT)
                {
                    var key = field.GetKeyDefine();
                    var value = field.GetValueDefine();
                    reader.IntervalLevel(level0).AppendLine($"var {field.Name}s = GetChilds(_2);");
                    reader.IntervalLevel(level0).AppendLine($"for (int i = 0; i < {field.Name}s.Count; i++)");
                    reader.IntervalLevel(level0).AppendLine($"{{");
                    reader.IntervalLevel(level0 + 1).AppendLine($"var _3 = {field.Name}s[i];");
                    reader.IntervalLevel(level0 + 1).AppendLine($"var key = {ReadDict(key, Setting.KEY)};");
                    reader.IntervalLevel(level0 + 1).AppendLine($"var value = {ReadDict(value, Setting.VALUE)};");
                    reader.IntervalLevel(level0 + 1).AppendLine($"{field.Name}.Add(key, value);");
                    reader.IntervalLevel(level0).AppendLine($"}}");
                    reader.IntervalLevel(level0).AppendLine($"break;");
                }
            }
        }
        static string ReadList(FieldWrap field)
        {
            var type = Util.CorrectFullType(field.FullName);
            if (field.IsRaw)
                return $"Read{type.FirstCharUpper()}(_3)";
            else if (field.IsEnum)
                return $"({type})ReadInt(_3)";
            else if (field.IsClass)
                return $"ReadObject<{type}>(_3, \"{type}\")";
            else
            {
                Util.LogError($"集合中类型仅支持基础类型,Enum,Class!");
                return "<集合中类型仅支持基础类型,Enum,Class!>";
            }
        }
        static string ReadDict(FieldWrap field, string node)
        {
            var type = Util.CorrectFullType(field.FullName);
            if (field.IsRaw)
                return $"Read{type.FirstCharUpper()}(GetOnlyChild(_3, \"{node}\"))";
            else if (field.IsEnum)
                return $"({type})ReadInt(GetOnlyChild(_3, \"{node}\"))";
            else if (field.IsClass)
                return $"ReadObject<{type}>(GetOnlyChild(_3, \"{node}\"), \"{type}\")";
            else
            {
                Util.LogError($"集合中类型仅支持基础类型,Enum,Class!");
                return "<集合中类型仅支持基础类型,Enum,Class!>";
            }
        }
        #endregion

        #region 枚举
        static void GenEnum()
        {
            List<EnumWrap> enms = EnumWrap.GetExports();
            for (int i = 0; i < enms.Count; i++)
            {
                EnumWrap en = enms[i];

                //命名空间
                builder.AppendLine(string.Join("\r\n", namespaces));
                builder.AppendLine($"namespace {Setting.ModuleName}.{en.Namespace}");
                Start(0);
                {
                    //枚举
                    Comment(en.Desc, TYPE_LEVEL);
                    if (en.IsFlags)
                        builder.IntervalLevel(TYPE_LEVEL).AppendLine($"[System.Flags]");
                    builder.IntervalLevel(TYPE_LEVEL).AppendLine($"public enum {en.Name}");
                    Start(TYPE_LEVEL);
                    {
                        foreach (var item in en.Values)
                        {
                            Comment(en.Items[item.Key].Desc, MEM_LEVEL);
                            builder.IntervalLevel(MEM_LEVEL);
                            builder.AppendLine($"{item.Key} = {item.Value},");
                        }
                    }
                    End(TYPE_LEVEL);
                }
                End(0);
                string path = Path.Combine(Setting.XmlCodeDir, $"{Setting.ModuleName}.{en.FullName}.cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
        #endregion

        #region XmlObject:基类
        static void GenXmlObject()
        {
            int level1 = TYPE_LEVEL + 1;
            int level2 = TYPE_LEVEL + 2;
            int level3 = TYPE_LEVEL + 3;
            int level4 = TYPE_LEVEL + 4;
            int level5 = TYPE_LEVEL + 5;

            //命名空间
            builder.AppendLine(string.Join("\r\n", namespaces));
            builder.AppendLine("using System.Linq;");
            builder.AppendLine($"namespace {Setting.ModuleName}");
            Start(0);
            {
                //注释
                Comment("Xml序列化类", TYPE_LEVEL);
                builder.IntervalLevel(TYPE_LEVEL).AppendLine($"public abstract class {CLASS_XML_OBJECT}");
                Start(TYPE_LEVEL);
                {
                    builder.IntervalLevel(level1).AppendLine("public abstract void Write(TextWriter os);");
                    builder.IntervalLevel(level1).AppendLine("public abstract void Read(XmlNode os);");
                    builder.AppendLine();

                    //ReadAttribute
                    builder.IntervalLevel(level1).AppendLine("public static string ReadAttribute(XmlNode node, string attribute)");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("try");
                        Start(level2);
                        {
                            builder.IntervalLevel(level3).AppendLine("if (node != null && node.Attributes != null && node.Attributes[attribute] != null)");
                            builder.IntervalLevel(level4).AppendLine("return node.Attributes[attribute].Value;");
                        }
                        End(level2);
                        builder.IntervalLevel(level2).AppendLine("catch (Exception ex)");
                        Start(level2);
                        {
                            builder.IntervalLevel(level3).AppendLine("throw new Exception(string.Format(\"attribute:{0} not exist\", attribute), ex);");
                        }
                        End(level2);
                        builder.IntervalLevel(level2).AppendLine("return \"\";");
                    }
                    End(level1);

                    //GetOnlyChild
                    builder.IntervalLevel(level1).AppendLine("public static XmlNode GetOnlyChild(XmlNode parent, string name)");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("XmlNode child = null;");
                        builder.IntervalLevel(level2).AppendLine("foreach (XmlNode sub in parent.ChildNodes)");
                        Start(level2);
                        {
                            builder.IntervalLevel(level2).AppendLine("if (sub.NodeType == XmlNodeType.Element && sub.Name == name)");
                            Start(level3);
                            {
                                builder.IntervalLevel(level4).AppendLine("if (child != null)");
                                builder.IntervalLevel(level5).AppendLine("throw new Exception(string.Format(\"child:{0} duplicate\", name));");
                                builder.IntervalLevel(level4).AppendLine("child = sub;");
                            }
                            End(level3);
                        }
                        End(level2);
                        builder.IntervalLevel(level2).AppendLine("return child;");
                    }
                    End(level1);

                    //GetChilds
                    builder.IntervalLevel(level1).AppendLine("public static List<XmlNode> GetChilds(XmlNode parent)");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("var childs = new List<XmlNode>();");
                        builder.IntervalLevel(level2).AppendLine("if (parent != null)");
                        builder.IntervalLevel(level3).AppendLine("childs.AddRange(parent.ChildNodes.Cast<XmlNode>().Where(sub => sub.NodeType == XmlNodeType.Element));");
                        builder.IntervalLevel(level2).AppendLine("return childs;");
                    }
                    End(level1);

                    //ReadBool
                    builder.IntervalLevel(level1).AppendLine("public static bool ReadBool(XmlNode node)");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("string str = node.InnerText.ToLower();");
                        builder.IntervalLevel(level2).AppendLine("if (str == \"true\")");
                        builder.IntervalLevel(level3).AppendLine("return true;");
                        builder.IntervalLevel(level2).AppendLine("else if (str == \"false\")");
                        builder.IntervalLevel(level3).AppendLine("return false;");
                        builder.IntervalLevel(level2).AppendLine("throw new Exception(string.Format(\"\'{0}\' is not valid bool\", str));");
                    }
                    End(level1);

                    //ReadInt/ReadLong/ReadFloat
                    List<string> readNumList = new List<string>() { "Int", "Long", "Float" };
                    for (int i = 0; i < readNumList.Count; i++)
                    {
                        string type = readNumList[i];
                        builder.IntervalLevel(level1).AppendLine($"public static {type.ToLower()} Read{type}(XmlNode node)");
                        Start(level1);
                        {
                            builder.IntervalLevel(level2).AppendLine($"return {type.ToLower()}.Parse(node.InnerText);");
                        }
                        End(level1);
                    }

                    //ReadString
                    builder.IntervalLevel(level1).AppendLine("public static string ReadString(XmlNode node)");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("return node.InnerText;");
                    }
                    End(level1);

                    //ReadObject
                    builder.IntervalLevel(level1).AppendLine("public static T ReadObject<T>(XmlNode node, string fullTypeName) where T :XmlObject");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("var obj = (T)Create(node, fullTypeName);");
                        builder.IntervalLevel(level2).AppendLine("obj.Read(node);");
                        builder.IntervalLevel(level2).AppendLine("return obj;");
                    }
                    End(level1);

                    //ReadDynamicObject
                    builder.IntervalLevel(level1).AppendLine("public static T ReadDynamicObject<T>(XmlNode node, string ns) where T :XmlObject");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("var fullTypeName = ns + \".\" + ReadAttribute(node, \"Type\");");
                        builder.IntervalLevel(level2).AppendLine("return ReadObject<T>(node, fullTypeName);");
                    }
                    End(level1);

                    //Create
                    builder.IntervalLevel(level1).AppendLine("public static object Create(XmlNode node, string type)");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("try");
                        Start(level2);
                        {
                            builder.IntervalLevel(level3).AppendLine("var t = Type.GetType(type);");
                            builder.IntervalLevel(level3).AppendLine("return Activator.CreateInstance(t);");
                        }
                        End(level2);
                        builder.IntervalLevel(level2).AppendLine("catch (Exception e)");
                        Start(level2);
                        {
                            builder.IntervalLevel(level3).AppendLine("throw new Exception(string.Format(\"type:{0} create fail!\", type), e);");
                        }
                        End(level2);
                    }
                    End(level1);

                    //Write:int,long,string,bool,float
                    List<string> writeBaseList = new List<string>() { "int", "long", "float", "string", "bool" };
                    for (int i = 0; i < writeBaseList.Count; i++)
                    {
                        string type = writeBaseList[i];
                        builder.IntervalLevel(level1).AppendLine($"public static void Write(TextWriter os, string name, {type} x)");
                        Start(level1);
                        {
                            builder.IntervalLevel(level2).AppendLine("os.WriteLine(\"<{0}>{1}</{0}>\", name, x);");
                        }
                        End(level1);
                    }

                    //Write:XmlObject
                    builder.IntervalLevel(level1).AppendLine("public static void Write(TextWriter os, string name, XmlObject x)");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("os.WriteLine(\"<{0} Type =\\\"{1}\\\">\", name, x.GetType().Name);");
                        builder.IntervalLevel(level2).AppendLine("x.Write(os);");
                        builder.IntervalLevel(level2).AppendLine("os.WriteLine(\"</{0}>\", name);");
                    }
                    End(level1);

                    //Write<V>
                    builder.IntervalLevel(level1).AppendLine("public static void Write<V>(TextWriter os, string name, List<V> x)");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("os.WriteLine(\"<{0}>\", name);");
                        builder.IntervalLevel(level2).AppendLine("x.ForEach(v => Write(os, \"Item\", v));");
                        builder.IntervalLevel(level2).AppendLine("os.WriteLine(\"</{0}>\", name);");
                    }
                    End(level1);

                    //Write<K,V>
                    builder.IntervalLevel(level1).AppendLine("public static void Write<K, V>(TextWriter os, string name, Dictionary<K, V> x)");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("os.WriteLine(\"<{0}>\", name);");
                        builder.IntervalLevel(level2).AppendLine("foreach (var e in x)");
                        Start(level2);
                        {
                            builder.IntervalLevel(level3).AppendLine("os.WriteLine(\"<Pair>\");");
                            builder.IntervalLevel(level3).AppendLine("Write(os, \"Key\", e.Key);");
                            builder.IntervalLevel(level3).AppendLine("Write(os, \"Value\", e.Value);");
                            builder.IntervalLevel(level3).AppendLine("os.WriteLine(\"</Pair>\");");
                        }
                        End(level2);
                        builder.IntervalLevel(level2).AppendLine("os.WriteLine(\"</{0}>\", name);");
                    }
                    End(level1);

                    //Write:object
                    builder.IntervalLevel(level1).AppendLine("public static void Write(TextWriter os, string name, object x)");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("if (x is bool)");
                        builder.IntervalLevel(level3).AppendLine("Write(os, name, (bool)x);");
                        builder.IntervalLevel(level2).AppendLine("else if (x is int || x is Enum)");
                        builder.IntervalLevel(level3).AppendLine("Write(os, name, (int)x);");
                        builder.IntervalLevel(level2).AppendLine("else if (x is long)");
                        builder.IntervalLevel(level3).AppendLine("Write(os, name, (long)x);");
                        builder.IntervalLevel(level2).AppendLine("else if (x is float)");
                        builder.IntervalLevel(level3).AppendLine("Write(os, name, (float)x);");
                        builder.IntervalLevel(level2).AppendLine("else if (x is string)");
                        builder.IntervalLevel(level3).AppendLine("Write(os, name, (string)x);");
                        builder.IntervalLevel(level2).AppendLine("else if (x is XmlObject)");
                        builder.IntervalLevel(level3).AppendLine("Write(os, name, (XmlObject)x);");
                        builder.IntervalLevel(level2).AppendLine("else");
                        builder.IntervalLevel(level3).AppendLine("throw new Exception(\"unknown Lson type; \" + x.GetType());");
                    }
                    End(level1);

                    //LoadAConfig
                    builder.IntervalLevel(level1).AppendLine("public void LoadAConfig(string file)");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("var doc = new XmlDocument();");
                        builder.IntervalLevel(level2).AppendLine("doc.Load(file);");
                        builder.IntervalLevel(level2).AppendLine("Read(doc.DocumentElement);");
                    }
                    End(level1);

                    //SaveAConfig
                    builder.IntervalLevel(level1).AppendLine("public void SaveConfig(string file)");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("var os = new StringWriter();");
                        builder.IntervalLevel(level2).AppendLine("Write(os, \"Root\", this);");
                        builder.IntervalLevel(level2).AppendLine("File.WriteAllText(file, os.ToString());");
                    }
                    End(level1);

                    //LoadConfig<T>
                    builder.IntervalLevel(level1).AppendLine("public static void LoadConfig<T>(List<T> x, string file) where T : XmlObject");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("var doc = new XmlDocument();");
                        builder.IntervalLevel(level2).AppendLine("doc.Load(file);");
                        builder.IntervalLevel(level2).AppendLine("x.AddRange(GetChilds(doc.DocumentElement).Select(_ => ReadDynamicObject<T>(_, typeof(T).Namespace)));");
                    }
                    End(level1);

                    //SaveConfig<T>
                    builder.IntervalLevel(level1).AppendLine("public static void SaveConfig<T>(List<T> x, string file) where T : XmlObject");
                    Start(level1);
                    {
                        builder.IntervalLevel(level2).AppendLine("var os = new StringWriter();");
                        builder.IntervalLevel(level2).AppendLine("Write(os, \"Root\", x);");
                        builder.IntervalLevel(level2).AppendLine("File.WriteAllText(file, os.ToString());");
                    }
                    End(level1);
                }
                End(TYPE_LEVEL);
            }
            End(0);

            string path = Path.Combine(Setting.XmlCodeDir, $"{CLASS_XML_OBJECT}.cs");
            Util.SaveFile(path, builder.ToString());
            builder.Clear();
        }
        #endregion
    }
}
