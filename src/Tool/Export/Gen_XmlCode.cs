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

        const int TYPE_LEVEL = 1;
        const int MEM_LEVEL = 2;
        const int SEM_LEVEL = 3;

        static StringBuilder builder = new StringBuilder();
        static List<string> namespaces = new List<string>()
        {
            "using System;",
            $"using XmlEditor;",
            "using System.IO;",
            "using System.Xml;",
            "using System.Collections.Generic;",
        };

        public static void Gen()
        {
            GenClass();
            GenEnum();
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
                        builder.IntervalLevel(TYPE_LEVEL).AppendLine($"public class {cls.Name} : {CLASS_XML_OBJECT}");
                    else
                        builder.IntervalLevel(TYPE_LEVEL).AppendLine($"public class {cls.Name} : {Util.CorrectFullType(cls.Inherit)}");
                    Start(TYPE_LEVEL);
                    {

                        StringBuilder writer = new StringBuilder();
                        StringBuilder reader = new StringBuilder();
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
                string path = Path.Combine(Setting.XmlCodeDir, cls.FullType + ".cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
        static void Field(FieldWrap field)
        {
            Comment(field.Desc, MEM_LEVEL);
            builder.IntervalLevel(MEM_LEVEL);
            if (field.IsRaw || field.IsEnum || field.IsClass)
                builder.AppendLine($"{FEILD_MODIFIERS} {Util.CorrectFullType(field.FullType)} {field.Name};");
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
            string type = Util.CorrectFullType(field.FullType);
            if (field.IsRaw)
                reader.IntervalLevel(level).AppendLine($"case \"{field.Name}\": {field.Name} = Read{type.FirstCharUpper()}(_2); break;");
            else if (field.IsEnum)
                reader.IntervalLevel(level).AppendLine($"case \"{field.Name}\": {field.Name} = ({type})ReadString(_2); break;");
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
            var type = Util.CorrectFullType(field.FullType);
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
            var type = Util.CorrectFullType(field.FullType);
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
                    builder.IntervalLevel(TYPE_LEVEL).AppendLine($"public enum {en.Name}");
                    Start(TYPE_LEVEL);
                    {
                        foreach (var item in en.Values)
                        {
                            var alias = en.Items[item.Key].Alias;
                            if (!alias.IsEmpty())
                                Comment(alias, MEM_LEVEL);
                            builder.IntervalLevel(MEM_LEVEL);
                            builder.AppendLine($"{item.Key} = {item.Value},");
                        }
                    }
                    End(TYPE_LEVEL);
                }
                End(0);
                string path = Path.Combine(Setting.XmlCodeDir, en.FullName + ".cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
        #endregion
    }
}
