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
        public static Data LastData => lastData;
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
                Util.LogError(">命令参数解析失败!");
#if DEBUG
                Console.ReadKey();
#endif
                return;
            }

            try
            {
                LoadDefine();
                VerifyDefine();
            }
            catch (Exception e)
            {
                Util.LogErrorFormat("加载定义异常!\n{0}\n{1}\n", e.Message, e.StackTrace);
#if DEBUG
                Console.ReadKey();
#endif
                return;
            }

            try
            {
                Export("CS", Setting.CSDir);
                Export("Java", Setting.JavaDir);
                Export("Lua", Setting.LuaDir);
                Export("XmlCode", Setting.XmlCodeDir);
            }
            catch (Exception e)
            {
                Util.LogErrorFormat("导出多语言文件异常!\n{0}\n{1}\n", e.Message, e.StackTrace);
#if DEBUG
                Console.ReadKey();
#endif
                return;
            }
           
            //Export("XmlCode", Setting.XmlCodeDir);

            if (!Setting.DataDir.IsEmpty()
               || !Setting.BinaryDir.IsEmpty()
               || Setting.Check)
            {
                try
                {
                    LoadData();
                    VerifyData();
                    Export("Data", Setting.DataDir);
                    Export("Binary", Setting.BinaryDir);
                }
                catch (Exception e)
                {
                    Util.Log("\n-------------最后一条数据-------------\n");
                    Util.Log(lastData?.ExportData());
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
            long start = DateTime.Now.Ticks;
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
            long end = DateTime.Now.Ticks;
            float second = (end - start) * 1f / TimeSpan.TicksPerSecond;
            Util.LogFormat("#{0,-40} 耗时 {1:F3}s", "加载数据结构定义", second);
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
            Util.LogFormat("# 共需加载配置文件 {0} 个", ConfigWrap.Configs.Count);
            List<string> warnings = new List<string>();
            foreach (var item in ConfigWrap.Configs)
            {
                ConfigWrap cfg = item.Value;
                long start = DateTime.Now.Ticks;
                cfg.LoadData();
                long end = DateTime.Now.Ticks;
                float second = (end - start) * 1f / TimeSpan.TicksPerSecond;
                string output = string.Format("{0,-40} 耗时 {1:F3}s", $"\t>{cfg.FullName}", second);
                Util.Log(output);

                if ((end - start) >= TimeSpan.TicksPerSecond)
                    warnings.Add(output);
            }
            Util.Log("\n");

            Util.LogWarningFormat("# 加载耗时严重的配置文件 {0} 个", warnings.Count);
            for (int i = 0; i < warnings.Count; i++)
                Util.LogWarning(warnings[i]);
            Util.Log("\n");
        }

        static void VerifyData()
        {
            Util.LogFormat("#{0,-38}\n", " 开始检查配置表");
            long start = DateTime.Now.Ticks;
            var cit = ConfigWrap.Configs.GetEnumerator();
            while (cit.MoveNext())
                cit.Current.Value.VerifyData();
            long end = DateTime.Now.Ticks;
            float second = (end - start) * 1f / TimeSpan.TicksPerSecond;
            Util.LogFormat("#{0,-39} 耗时 {1:F3}s\n", " 检查配置表", second);
        }

        static void Export(string code, string path)
        {
            if (path.IsEmpty()) return;

            long start = DateTime.Now.Ticks;
            Util.TryDeleteDirectory(path);
            Type type = Type.GetType($"Tool.Export.Gen_{code}");
            var export = type.GetMethod("Gen", BindingFlags.Public | BindingFlags.Static);
            export.Invoke(null, null);
            long end = DateTime.Now.Ticks;
            Util.LogFormat("#{0,-40} 耗时 {1:F3}s\n", $" 导出{code}总共", (end - start) * 1f / TimeSpan.TicksPerSecond);
        }
    }
}
