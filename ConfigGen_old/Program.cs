using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Description.Xml;
using Description.TypeInfo;
using Description.Wrap;

namespace Description
{
    /// 所有的路径均以应用为相对路径生成
    /// 尽可能少配置Excel文件,文件读取非常耗时
    class Program
    {
        //未完成内容
        //1.说明文档按功能来整理

        static Data lastData;

        public static void AddLastData(Data data)
        {
            lastData = data;
        }

        static void Main(string[] args)
        {
            long start = DateTime.Now.Ticks;
            CmdOption.Ins.Parse(args);
            //解析命令
            if (!CmdOption.Ins.CheckResult())
            {
                Util.LogError("命令参数解析失败!");
#if DEBUG
                Console.ReadKey();
#endif
                return;
            }

            LoadDefine();
            VerifyDefine();
            Export("CSharp", Setting.CSDir);
            Export("Java", Setting.JavaDir);
            Export("Lua", Setting.LuaDir);
            Export("XmlCode", Setting.XmlCodeDir);

            if (!Setting.DataDir.IsEmpty() || Setting.Check)
            {
                try
                {
                    LoadData();
                    VerifyData();
                    Export("Csv", Setting.ConfigRootNode);
                }
                catch (Exception e)
                {
                    Util.Log("\n-------------最后一条数据-------------\n");
                    Util.Log(lastData.ExportData());
                    Util.Log("\n--------------------------------------\n");
                    Util.LogErrorFormat("{0}\n{1}\n", e.Message, e.StackTrace);
                }
            }

            Util.Log("\n\n");
            long end = DateTime.Now.Ticks;
            Util.LogFormat("=================>> 总共耗时 {0:F3}s", (end - start) * 1f / TimeSpan.TicksPerSecond);
#if DEBUG
            Console.ReadKey();
#endif
        }

        static void LoadDefine()
        {
            //解析类型定义
            Dictionary<string, NamespaceXml> pairs = new Dictionary<string, NamespaceXml>();
            string path = "无法解析Xml.NamespaceDes";
            try
            {
                var configXml = Util.Deserialize(Setting.ConfigXml, typeof(ConfigXml)) as ConfigXml;
                if (configXml.Root.IsEmpty())
                    throw new Exception("数据结构导出时必须指定命名空间根节点<Config Root=\"**\">");
                Setting.ConfigRootNode = configXml.Root;
                List<string> include = configXml.Import;
                for (int i = 0; i < include.Count; i++)
                {
                    path = Util.GetAbsPath(include[i]);
                    var des = Util.Deserialize(path, typeof(NamespaceXml)) as NamespaceXml;
                    if (pairs.ContainsKey(des.Name))
                    {
                        pairs[des.Name].Classes.AddRange(des.Classes);
                        pairs[des.Name].Enums.AddRange(des.Enums);
                    }
                    else
                    {
                        des.XmlDir = Path.GetDirectoryName(path);
                        pairs.Add(des.Name, des);
                    }
                }
                GroupInfo.LoadGroup(configXml.Group);
            }
            catch (Exception e)
            {
                Util.LogErrorFormat("路径:{0} Error:{1}\n{2}", path, e.Message, e.StackTrace);
                return;
            }

            HashSet<string> fullHash = new HashSet<string>();
            var nit = pairs.GetEnumerator();
            while (nit.MoveNext())
            {
                var item = nit.Current;
                string namespace0 = item.Key;
                for (int i = 0; i < item.Value.Classes.Count; i++)
                {
                    ClassXml classDes = item.Value.Classes[i];
                    var cls = new ClassInfo(classDes, namespace0);
                    if (cls.IsConfig())
                        new ConfigInfo(classDes, namespace0, item.Value.XmlDir);
                }
                for (int i = 0; i < item.Value.Enums.Count; i++)
                    new EnumInfo(item.Value.Enums[i], namespace0);
            }
        }

        static void VerifyDefine()
        {
            var cls = ClassInfo.Classes.GetEnumerator();
            while (cls.MoveNext())
                cls.Current.Value.VerifyDefine();
            var cfgs = ConfigInfo.Configs.GetEnumerator();
            while (cfgs.MoveNext())
                cfgs.Current.Value.VerifyDefine();
        }

        static void LoadData()
        {
            var cit = ConfigInfo.Configs.GetEnumerator();
            while (cit.MoveNext())
            {
                ConfigInfo cfg = cit.Current.Value;
                long start = DateTime.Now.Ticks;
                cfg.LoadData();
                long end = DateTime.Now.Ticks;
                if ((end - start) >= TimeSpan.TicksPerSecond)
                    Util.LogFormat("{0} 耗时 {1:F3}s\n", cfg.FullType, (end - start) * 1f / TimeSpan.TicksPerSecond);
            }
        }

        static void VerifyData()
        {
            var cit = ConfigInfo.Configs.GetEnumerator();
            while (cit.MoveNext())
                cit.Current.Value.VerifyData();
        }

        static void Export(string code, string path)
        {
            if (path.IsEmpty()) return;

            Util.TryDeleteDirectory(path);
            Type type = Type.GetType("ConfigGen.Export.Export" + code);
            var export = type.GetMethod("Export", BindingFlags.Public | BindingFlags.Static);
            export.Invoke(null, null);
        }
    }
}
