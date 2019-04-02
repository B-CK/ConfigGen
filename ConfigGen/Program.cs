using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using ConfigGen.Description;
using ConfigGen.TypeInfo;
using ConfigGen.Config;

namespace ConfigGen
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
            Util.Start();

            CmdOption.Ins.Parse(args);
            //解析命令
            if (!CmdOption.Ins.CheckResult())
            {
                Util.LogError("命令参数解析失败!");
                Console.ReadKey();
                return;
            }

            LoadDefine();
            VerifyDefine();

            if (!Consts.DataDir.IsEmpty() || Consts.Check)
            {
                try
                {
                    LoadData();
                    //VerifyData();
                }
                catch (Exception e)
                {
                    Util.Log("\n-------------最后一条数据-------------\n");
                    Util.Log(lastData);
                    Util.Log("\n--------------------------------------\n");
                    Util.LogErrorFormat("{0}\n{1}\n", e.Message, e.StackTrace);
                }
            }

            Export("CSharp", Consts.CSDir);
            Export("Java", Consts.JavaDir);
            Export("Lua", Consts.LuaDir);
            Export("XmlCode", Consts.XmlCodeDir);
            Export("Csv", Consts.DataDir);

            Util.Log("\n\n");
            Util.Stop("=================>> 总共");
        }

        static void LoadDefine()
        {
            //解析类型定义
            Dictionary<string, NamespaceDes> pairs = new Dictionary<string, NamespaceDes>();
            string path = "无法解析Xml.NamespaceDes";
            try
            {
                var configXml = Util.Deserialize(Consts.ConfigXml, typeof(ConfigXml)) as ConfigXml;
                if (configXml.Root.IsEmpty())
                    throw new Exception("数据结构导出时必须指定命名空间根节点<Config Root=\"**\">");
                Consts.ConfigRootNode = configXml.Root;
                List<string> include = configXml.Include;
                for (int i = 0; i < include.Count; i++)
                {
                    path = Util.GetAbsPath(include[i]);
                    var des = Util.Deserialize(path, typeof(NamespaceDes)) as NamespaceDes;
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
                    ClassDes classDes = item.Value.Classes[i];
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
                Util.Log('.');
                long start = DateTime.Now.Ticks;
                cfg.LoadData();
                long end = DateTime.Now.Ticks;
                if ((end - start) >= 1000)
                    Util.LogFormat("{0} 耗时 {1}ms\n", cfg.FullName, end - start);
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
