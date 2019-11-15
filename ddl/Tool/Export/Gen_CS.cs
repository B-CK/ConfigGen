using Wrap;
using System.IO;
using Description;
using System.Text;
using System.Collections.Generic;

namespace Tool.Export
{
    public class Gen_CS
    {
        const string CLASS_CFG_OBJECT = "CfgObject";
        const string CLASS_DATA_STREAM = "DataStream";
        const string ARG_DATASTREAM = "data";
        const string FEILD_MODIFIERS = "public readonly";

        const int TYPE_LEVEL = 1;
        const int MEM_LEVEL = 2;
        const int SEM_LEVEL = 3;

        static StringBuilder builder = new StringBuilder();
        static List<string> namespaces = new List<string>()
        {
            $"using {Setting.ModuleName};",
            "using System;",
            "using System.Text;",
            "using System.Linq;",
            "using System.Collections.Generic;",
        };

        public static void Gen()
        {
            GenClass();
            GenEnum();
            ConfigComponent();
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
                        builder.IntervalLevel(TYPE_LEVEL).AppendLine($"public class {cls.Name} : {CLASS_CFG_OBJECT}");
                    else
                        builder.IntervalLevel(TYPE_LEVEL).AppendLine($"public class {cls.Name} : {Util.CorrectFullType(cls.Inherit)}");
                    Start(TYPE_LEVEL);
                    {
                        StringBuilder initer = new StringBuilder();
                        for (int j = 0; j < cls.Fields.Count; j++)
                        {
                            FieldWrap field = cls.Fields[j];
                            if (!Util.MatchGroups(field.Group)) continue;

                            Field(field);//字段成员
                            InitField(initer, field);//初始化字段语句
                        }

                        //构造函数
                        builder.IntervalLevel(MEM_LEVEL);
                        if (cls.Inherit.IsEmpty())
                            builder.AppendLine($"public {cls.Name}({CLASS_DATA_STREAM} {ARG_DATASTREAM})");
                        else
                            builder.AppendLine($"public {cls.Name}({CLASS_DATA_STREAM} {ARG_DATASTREAM}) : base({ARG_DATASTREAM})");
                        Start(MEM_LEVEL);
                        builder.Append(initer.ToString());
                        End(MEM_LEVEL);

                        //配置加载函数
                        if (cls.IsConfig())
                            LoadFunc(ConfigWrap.Configs[cls.FullType]);
                    }
                    End(TYPE_LEVEL);
                }
                End(0);
                string path = Path.Combine(Setting.CSDir, cls.Namespace.Replace(Setting.DotSplit[0], '/'), $"{cls.Name}.cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
        static string ReadValue(FieldWrap field)
        {
            if (field.IsRaw)
                return $"{ARG_DATASTREAM}.Get{field.FullType.FirstCharUpper()}()";
            else if (field.IsEnum)
                return $"({Util.CorrectFullType(field.FullType)}){ARG_DATASTREAM}.GetInt()";
            else if (field.IsClass)
            {
                ClassWrap info = ClassWrap.Get(field.FullType);
                string fullType = Util.CorrectFullType(field.FullType);
                if (info.IsDynamic())
                    return $"({fullType}){ARG_DATASTREAM}.GetObject({ARG_DATASTREAM}.GetString())";
                else
                    return $"new {fullType}({ARG_DATASTREAM})";
            }
            else
                Util.Error("不支持集合嵌套类型:" + field.FullType);
            return null;
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
        static void InitField(StringBuilder initer, FieldWrap field)
        {
            int level = SEM_LEVEL + 1;
            if (field.IsRaw || field.IsEnum || field.IsClass)
            {
                initer.IntervalLevel(SEM_LEVEL);
                initer.AppendLine($"{field.Name} = {ReadValue(field)};");
            }
            else if (field.OriginalType == Setting.LIST)
            {
                initer.IntervalLevel(SEM_LEVEL).AppendLine($"for (int n = {ARG_DATASTREAM}.GetInt(); n-- > 0;)");
                initer.IntervalLevel(SEM_LEVEL).AppendLine("{");
                {
                    FieldWrap item = field.GetItemDefine();
                    initer.IntervalLevel(level).AppendLine($"var v = {ReadValue(item)};");
                    initer.IntervalLevel(level).AppendLine($"{field.Name}.Add(v);");
                }
                initer.IntervalLevel(SEM_LEVEL).AppendLine("}");
            }
            else if (field.OriginalType == Setting.DICT)
            {
                initer.IntervalLevel(SEM_LEVEL).AppendLine($"for (int n = {ARG_DATASTREAM}.GetInt(); n-- > 0;)");
                initer.IntervalLevel(SEM_LEVEL).AppendLine("{");
                {
                    FieldWrap key = field.GetKeyDefine();
                    FieldWrap value = field.GetValueDefine();
                    initer.IntervalLevel(level).AppendLine($"var k = {ReadValue(key)};");
                    initer.IntervalLevel(level).AppendLine($"{field.Name}[k] = {ReadValue(value)};");
                }
                initer.IntervalLevel(SEM_LEVEL).AppendLine("}");
            }
        }
        static void LoadFunc(ConfigWrap cfg)
        {
            int level1 = SEM_LEVEL + 1;
            int level2 = SEM_LEVEL + 2;
            var keyType = Util.CorrectFullType(cfg.Index.FullType);
            var varType = Util.CorrectFullType(cfg.FullType);
            builder.IntervalLevel(MEM_LEVEL).AppendLine($"public static Dictionary<{cfg.Index.FullType}, {varType}> Load()");
            builder.IntervalLevel(MEM_LEVEL).AppendLine("{");
            {
                builder.IntervalLevel(SEM_LEVEL).AppendLine($"var dict = new Dictionary<{cfg.Index.FullType}, {varType}>();");
                builder.IntervalLevel(SEM_LEVEL).AppendLine($"var path = \"{cfg.FullType.Replace(Setting.DotSplit[0], '/')}{Setting.DataFileExt}\";");
                builder.IntervalLevel(SEM_LEVEL).AppendLine($"try");
                builder.IntervalLevel(SEM_LEVEL).AppendLine($"{{");
                {
                    builder.IntervalLevel(level1).AppendLine($"var data = new DataStream(path, Encoding.UTF8);");
                    builder.IntervalLevel(level1).AppendLine($"int length = data.GetInt();");
                    builder.IntervalLevel(level1).AppendLine($"for (int i = 0; i < length; i++)");
                    builder.IntervalLevel(level1).AppendLine($"{{");
                    {
                        builder.IntervalLevel(level2).AppendLine($"var v = new {varType}(data);");
                        builder.IntervalLevel(level2).AppendLine($"dict.Add(v.{cfg.Index.Name}, v);");
                    }
                    builder.IntervalLevel(level1).AppendLine($"}}");
                }
                builder.IntervalLevel(SEM_LEVEL).AppendLine($"}}");
                builder.IntervalLevel(SEM_LEVEL).AppendLine($"catch (Exception e)");
                builder.IntervalLevel(SEM_LEVEL).AppendLine($"{{");
                {
                    builder.IntervalLevel(level1).AppendLine($"UnityEngine.Debug.LogError($\"{{path}}解析异常~\\n{{e.Message}}\");");
                    builder.AppendLine($"#if UNITY_EDITOR");
                    builder.IntervalLevel(level1).AppendLine($"UnityEngine.Debug.LogError($\"最后一条数据Key:{{dict.Last().Key}}.\");");
                    builder.AppendLine($"#endif");
                }
                builder.IntervalLevel(SEM_LEVEL).AppendLine($"}}");
                builder.IntervalLevel(SEM_LEVEL).AppendLine($"return dict;");
            }
            builder.IntervalLevel(MEM_LEVEL).AppendLine("}");
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
                string path = Path.Combine(Setting.CSDir, en.Namespace.Replace(Setting.DotSplit[0], '/'), $"{en.Name}.cs");
                Util.SaveFile(path, builder.ToString());
                builder.Clear();
            }
        }
        #endregion

        #region 配置管理类
        static void ConfigComponent()
        {
            builder.AppendLine(string.Join("\r\n", namespaces));
            builder.AppendLine($"namespace {Setting.ModuleName}");
            Start(0);
            {
                builder.IntervalLevel(TYPE_LEVEL).AppendLine($"public partial class ConfigComponent");
                Start(TYPE_LEVEL);
                {
                    StringBuilder load = new StringBuilder();
                    foreach (var cfg in ConfigWrap.Configs)
                    {
                        var key = cfg.Value.Index.FullType;
                        var value = cfg.Value.FullType;
                        var property = $"{Util.FirstCharUpper(cfg.Value.Name)}s";
                        var field = $"_{cfg.Value.Name.ToLower()}s";
                        builder.IntervalLevel(MEM_LEVEL).AppendLine($"public Dictionary<{key}, {value}> {property} => {field};");
                        builder.IntervalLevel(MEM_LEVEL).AppendLine($"private Dictionary<{key}, {value}> {field} = new Dictionary<{key}, {value}>();");

                        load.IntervalLevel(SEM_LEVEL).AppendLine($"{field} = {cfg.Value.FullType}.Load();");
                    }

                    builder.IntervalLevel(MEM_LEVEL).AppendLine("public void Load()");
                    builder.IntervalLevel(MEM_LEVEL).AppendLine("{");
                    builder.Append(load.ToString());
                    builder.IntervalLevel(MEM_LEVEL).AppendLine("}");
                }
                End(TYPE_LEVEL);
            }
            End(0);

            string path = Path.Combine(Setting.CSDir, "ConfigComponent.cs");
            Util.SaveFile(path, builder.ToString());
            builder.Clear();
        }
        #endregion
    }
}

//#region 配置加载类
//static void CfgLoader()
//{
//    builder.AppendLine(string.Join("\r\n", namespaces));
//    builder.AppendLine($"namespace {Setting.ModuleName}");
//    Start(0);
//    {
//        int level1 = SEM_LEVEL + 1;
//        int level2 = SEM_LEVEL + 2;
//        var configs = ConfigWrap.GetExports();
//        for (int i = 0; i < configs.Count; i++)
//        {
//            var cfg = configs[i];
//            builder.AppendLine($"public partial class {cfg.Name}Category");
//            Start(TYPE_LEVEL);
//            {
//                var keyType = Util.CorrectFullType(cfg.Index.FullType);
//                var varType = Util.CorrectFullType(cfg.FullType);
//                builder.IntervalLevel(MEM_LEVEL).AppendLine($"public static Dictionry<{cfg.Index.FullType}, {varType}> Load()");
//                builder.IntervalLevel(MEM_LEVEL).AppendLine("{");
//                {
//                    builder.IntervalLevel(SEM_LEVEL).AppendLine($"var dict = new Dictionry<{cfg.Index.FullType}, {varType}>();");
//                    builder.IntervalLevel(SEM_LEVEL).AppendLine($"try");
//                    builder.IntervalLevel(SEM_LEVEL).AppendLine($"{{");
//                    {
//                        builder.IntervalLevel(level1).AppendLine($"var path = \"{cfg.FullType.Replace(Setting.DotSplit[0], '/')}{Setting.DataFileExt}\";");
//                        builder.IntervalLevel(level1).AppendLine($"var data = new DataStream(path, Encoding.UTF8);");
//                        builder.IntervalLevel(level1).AppendLine($"int length = data.GetInt();");
//                        builder.IntervalLevel(level1).AppendLine($"for (int i = 0; i < length; i++)");
//                        builder.IntervalLevel(level1).AppendLine($"{{");
//                        {
//                            builder.IntervalLevel(level2).AppendLine($"var v = new {varType}(data);");
//                            builder.IntervalLevel(level2).AppendLine($"dict.Add(v.{cfg.Index.Name}, v);");
//                        }
//                        builder.IntervalLevel(level1).AppendLine($"}}");
//                    }
//                    builder.IntervalLevel(SEM_LEVEL).AppendLine($"}}");
//                    builder.IntervalLevel(SEM_LEVEL).AppendLine($"catch (Exception e)");
//                    builder.IntervalLevel(SEM_LEVEL).AppendLine($"{{");
//                    {
//                        builder.IntervalLevel(level1).AppendLine($"UnityEngine.Debug.LogError($\"{{path}}解析异常~\\n{{e.Message}}\");");
//                        builder.AppendLine($"#if UNITY_EDITOR");
//                        builder.IntervalLevel(level1).AppendLine($"UnityEngine.Debug.LogError($\"最后一条数据Key:{{dict.Last().Key}}.\");");
//                        builder.AppendLine($"#endif");
//                    }
//                    builder.IntervalLevel(SEM_LEVEL).AppendLine($"}}");
//                    builder.IntervalLevel(SEM_LEVEL).AppendLine($"return dict;");
//                }
//                builder.IntervalLevel(MEM_LEVEL).AppendLine("}");
//            }
//            End(TYPE_LEVEL);
//        }
//    }
//    End(0);

//    string path = Path.Combine(Setting.CSDir, "CfgLoader.cs");
//    Util.SaveFile(path, builder.ToString());
//    builder.Clear();
//}
//#endregion
