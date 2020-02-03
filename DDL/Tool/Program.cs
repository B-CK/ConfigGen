using Xml;
using System;
using System.IO;
using Tool.Wrap;
using Tool.Config;
using System.Collections.Generic;
using System.Reflection;

//-module .\Module\Editor.xml -xmlCode XmlCode/ -group all
//-module .\Module\Example.xml -data ./Data  -csharp CSharp/ -group all


namespace Tool
{
    /// 所有的路径均以应用为相对路径生成
    /// 尽可能少配置Excel文件,文件读取非常耗时
    class Program
    {
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
            Export("CS", Setting.CSDir);
            Export("Java", Setting.JavaDir);
            Export("Lua", Setting.LuaDir);
            //Export("XmlCode", Setting.XmlCodeDir);

            if (!Setting.DataDir.IsEmpty() || Setting.Check)
            {
                try
                {
                    LoadData();
                    VerifyData();
                    Export("Data", Setting.ModuleName);
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
            Dictionary<string, NamespaceXml> allNs = new Dictionary<string, NamespaceXml>();
            string namespacePath = "无法解析Xml.NamespaceXml";
            try
            {
                var moduleXml = Util.Deserialize(Setting.Module, typeof(ModuleXml)) as ModuleXml;
                if (moduleXml.Name.IsEmpty())
                    throw new Exception("数据结构导出时必须指定命名空间根节点<Module Name =\"**\">");
                GroupWrap.LoadGroup(moduleXml.Groups);
                Setting.ModuleName = moduleXml.Name;
                string moduleDir = Path.GetDirectoryName(Setting.Module);
                List<string> imports = moduleXml.Imports;
                for (int i = 0; i < imports.Count; i++)
                {
                    ///命名空间目录与模块目录并列
                    namespacePath = Path.Combine(moduleDir, imports[i]);
                    var nsx = Util.Deserialize(namespacePath, typeof(NamespaceXml)) as NamespaceXml;
                    allNs.Add(nsx.Name, nsx);

                    string space = nsx.Name;
                    var cls = nsx.Classes;
                    for (int k = 0; k < cls.Count; k++)
                    {
                        ClassXml xml = cls[k];
                        var info = new ClassWrap(xml, space);
                        if (info.IsConfig())
                            new ConfigWrap(xml, space, Path.GetDirectoryName(namespacePath));
                    }
                    var ens = nsx.Enums;
                    for (int k = 0; k < ens.Count; k++)
                        new EnumWrap(ens[k], space);
                }
            }
            catch (Exception e)
            {
                Util.LogErrorFormat("路径:{0} 错误:{1}\n{2}", namespacePath, e.Message, e.StackTrace);
                return;
            }
        }

        static void VerifyDefine()
        {
            var es = EnumWrap.Enums;
            foreach (var item in ClassWrap.Classes)
                item.Value.VerifyDefine();
            foreach (var item in ConfigWrap.Configs)
                item.Value.VerifyDefine();
        }

        static void LoadData()
        {
            foreach (var item in ConfigWrap.Configs)
            {
                ConfigWrap cfg = item.Value;
                long start = DateTime.Now.Ticks;
                cfg.LoadData();
                long end = DateTime.Now.Ticks;
                if ((end - start) >= TimeSpan.TicksPerSecond)
                    Util.LogFormat("{0} 耗时 {1:F3}s\n", cfg.FullType, (end - start) * 1f / TimeSpan.TicksPerSecond);
            }
        }

        static void VerifyData()
        {
            var cit = ConfigWrap.Configs.GetEnumerator();
            while (cit.MoveNext())
                cit.Current.Value.VerifyData();
        }

        static void Export(string code, string path)
        {
            if (path.IsEmpty()) return;

            Util.TryDeleteDirectory(path);
            Type type = Type.GetType($"Tool.Export.Gen_{code}");
            var export = type.GetMethod("Gen", BindingFlags.Public | BindingFlags.Static);
            export.Invoke(null, null);
        }
    }
}
