using Xml;
using Wrap;
using System;
using DataSet;
using System.IO;
using System.Collections.Generic;

namespace Tool
{
    /// <summary>
    /// 生成,检查工具
    /// </summary>
    public class Program
    {
        #region 操作类型
        /// <summary>
        /// 依据模块生成所有脚本
        /// </summary>
        const string module = "module";
        /// <summary>
        /// 依据命名空间生成脚本
        /// </summary>
        const string ns = "namespace";
        /// <summary>
        /// 生成数据
        /// </summary>
        const string data = "data";
        /// <summary>
        /// 操作类型
        /// </summary>
        static readonly HashSet<string> OptHash = new HashSet<string>() { module, ns, data };
        #endregion
        #region 语言类型
        const string csharp = "cs";
        static readonly HashSet<string> LanHash = new HashSet<string>() { csharp };
        #endregion

        const string OPERATE = "-opt";
        //模块路径
        const string MODULE = "-m";
        //输入路径
        const string INPUT = "-i";
        //输出路径
        const string OUTPUT = "-o";
        //生成语言类型,目前支持cs
        const string LAN = "-lan";
        const string GROUP = "-gourp";
        const string HELP = "-help";

        static void Usage()
        {
            Console.WriteLine("语法:DDL.exe [命令参数] ");
            Console.WriteLine($"\t\t{OPERATE}:[操作]\t执行操作,当前支持类型:module[依据模块导出脚本],namespace[依据命名空间导出脚本],data[导出数据]");
            Console.WriteLine($"\t\t{MODULE}:[模块路径]\t按照模块描述生成内容");
            Console.WriteLine($"\t\t{INPUT}:[命名空间路径]\t按照命名空间描述生成内容");
            Console.WriteLine($"\t\t{LAN}:[语言]\t生成指定语言代码,目前支持:cs");
            Console.WriteLine($"\t\t{OUTPUT}:[目录路径]\t用于保存生成文件的目录");
            Console.WriteLine($"\t\t{GROUP}:[组1|组2]\t仅导出指定组的脚本/数据,默认全部导出");
            Console.WriteLine($"\t\t{HELP}\t使用说明");

            Environment.Exit(0);
        }

        //最后一条数据记录
        static Data lastData;
        public static void RecordLastData(Data data)
        {
            lastData = data;
        }

        static Dictionary<string, string> CmdDict = new Dictionary<string, string>();
        static void Main(string[] args)
        {
            long start = DateTime.Now.Ticks;
            if (PaserCmdArgs(args) == false)
            {
                Util.LogError("命令参数解析失败!");
                Usage();
                return;
            }
            if (CheckCmdArgs() == false)
            {
                Util.LogError("命令参数使用错误!");
                Usage();
                return;
            }

            switch (CmdDict[OPERATE])
            {
                //依据模块生成所有脚本
                case module: ModuleExportScripts(); break;
                //依据命名空间生成脚本
                case ns: NamespaceExportScripts(); break;
                //生成数据
                case data: ModuleExportData(); break;
            }

            Util.Log("\n\n");
            long end = DateTime.Now.Ticks;
            Util.LogFormat("=================>> 总共耗时 {0:F3}s", (end - start) * 1f / TimeSpan.TicksPerSecond);
        }
        #region 命令行参数解析/验证
        /// <summary>
        /// 解析命令行参数
        /// </summary>
        static bool PaserCmdArgs(string[] args)
        {
            bool isOk = true;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].IsEmpty()) continue;
                string[] nodes = args[i].Split(Setting.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
                if (nodes.Length == 0)
                {
                    Util.LogErrorFormat("{0} 命令格式错误", args[i]);
                    continue;
                }

                string key = nodes[0];
                if (key != HELP && !CheckEmptyArg(key, nodes))
                {
                    isOk &= false;
                    continue;
                }

                switch (key)
                {
                    case MODULE:
                    case INPUT:
                    case OUTPUT:
                        string path = Path.Combine(Setting.CurrentDir, Util.NormalizePath(nodes[1]));
                        if (File.Exists(path) || Directory.Exists(path))
                            AddCmdArg(key, nodes[1]);
                        else
                        {
                            Util.LogErrorFormat("{0}路径不存在!", module);
                            isOk &= false;
                            break;
                        }

                        break;
                    case OPERATE:
                    //AddCmdArg(key, nodes[1]);
                    //if (!OptHash.Contains(Setting.Operate))
                    //{
                    //    Util.LogErrorFormat("程序中未定{0}操作!", Setting.Operate);
                    //    isOk &= false;
                    //}
                    //break;
                    case LAN:
                    case GROUP:
                        AddCmdArg(key, nodes[1]);
                        break;
                    case HELP:
                        AddCmdArg(key, "");
                        break;
                    default:
                        Util.LogWarning($"未知命令:{key}");
                        isOk &= false;
                        break;
                }
            }
            return isOk;
        }
        /// <summary>
        /// 检查命令行参数
        /// </summary>
        /// <returns></returns>
        static bool CheckCmdArgs()
        {
            bool isOK = true;
            if (!CmdDict.ContainsKey(OPERATE))
            {
                isOK &= false;
                List<string> ls = new List<string>(OptHash);
                Util.LogErrorFormat("调用工具时,必须指定执行的操作类型!可选操作:{0}.", ls.ToString(","));
            }
            else
            {
                string opt = CmdDict[OPERATE];
                switch (opt)
                {
                    case module:
                        if (!CmdDict.ContainsKey(INPUT)
                            || !CmdDict.ContainsKey(OUTPUT)
                            || !CmdDict.ContainsKey(LAN))
                        {
                            isOK &= false;
                            Util.LogError("依据模块导出脚本时,必须同时包含-i,-o,-lan命令!");
                        }
                        break;
                    case ns:
                        if (!CmdDict.ContainsKey(INPUT)
                            || !CmdDict.ContainsKey(OUTPUT)
                            || !CmdDict.ContainsKey(LAN))
                        {
                            isOK &= false;
                            Util.LogError("依据命名空间导出脚本时,必须同时包含-i,-o,-lan命令!");
                        }
                        break;
                    case data:
                        if (!CmdDict.ContainsKey(MODULE)
                            || !CmdDict.ContainsKey(INPUT)
                            || !CmdDict.ContainsKey(OUTPUT)
                            || !CmdDict.ContainsKey(LAN))
                        {
                            isOK &= false;
                            Util.LogError("解析Excel数据时,必须同时包含-m,-i,-o,-lan命令!");
                        }
                        break;
                    default:
                        isOK &= false;
                        Util.LogErrorFormat("程序未定义该操作:{0}!", opt);
                        break;
                }
            }
            return isOK;
        }
        static bool CheckEmptyArg(string cmd, string[] args)
        {
            if (args.Length != 2 || args[1].IsEmpty())
            {
                Util.LogErrorFormat("{0} 命令参数为空", cmd);
                return false;
            }
            return true;
        }
        static void AddCmdArg(string key, string value)
        {
            if (!CmdDict.ContainsKey(key))
                CmdDict.Add(key, value);
        }
        #endregion

        #region 按模块导出脚本
        static void ModuleExportScripts()
        {
            Setting.Input = CmdDict[INPUT];
            Setting.Output = CmdDict[OUTPUT];
            Setting.Language = CmdDict[LAN];
            PaserGroups();

            LoadDefine();
        }
        #endregion

        #region 按命名空间导出脚本
        static void NamespaceExportScripts()
        {
            Setting.Input = CmdDict[INPUT];
            Setting.Output = CmdDict[OUTPUT];
            Setting.Language = CmdDict[LAN];
            if (CmdDict.ContainsKey(GROUP) && !CmdDict[GROUP].IsEmpty())
            {
                var groups = CmdDict[GROUP].Split(Setting.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
                Setting.ExportGroup = new HashSet<string>(groups);
            }

        }
        #endregion

        #region 按模块导出数据
        /// <summary>
        /// 数据导出丢给外部dll
        /// </summary>
        static void ModuleExportData()
        {
            Setting.ModuleXml = CmdDict[MODULE];
            Setting.Input = CmdDict[INPUT];

            PaserGroups();
            LoadDefine();

            try
            {
                if (CmdDict.ContainsKey(OUTPUT))
                {
                    Setting.Output = CmdDict[OUTPUT];
                    LoadData();
                }
                VerifyData();
            }
            catch (Exception e)
            {
                Util.Log("\n-------------最后一条数据-------------\n");
                Util.Log(lastData.ExportData());
                Util.Log("\n--------------------------------------\n");
                Util.LogErrorFormat("{0}\n{1}\n", e.Message, e.StackTrace);
            }
        }
        #endregion

        /// <summary>
        /// 初始化组参数
        /// </summary>
        static void PaserGroups()
        {
            if (CmdDict.ContainsKey(GROUP) && !CmdDict[GROUP].IsEmpty())
            {
                var groups = CmdDict[GROUP].ToLowerExt().Split(Setting.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
                Setting.ExportGroup = new HashSet<string>(groups);
            }
        }
        /// <summary>
        /// 加载定义
        /// </summary>
        static void LoadDefine()
        {
            //解析类型定义
            Dictionary<string, NamespaceXml> allNs = new Dictionary<string, NamespaceXml>();
            string path = "无法解析Xml.NamespaceDes";
            try
            {
                var moduleXml = Util.Deserialize(Setting.ModuleXml, typeof(ModuleXml)) as ModuleXml;
                if (moduleXml.Name.IsEmpty())
                    throw new Exception("数据结构导出时必须指定命名空间根节点<Module Name =\"**\">");
                GroupWrap.LoadGroup(moduleXml.Groups);
                Setting.ModuleName = moduleXml.Name;
                List<string> include = moduleXml.Imports;
                for (int i = 0; i < include.Count; i++)
                {
                    path = Util.GetAbsPath(include[i]);
                    var nsx = Util.Deserialize(path, typeof(NamespaceXml)) as NamespaceXml;
                    allNs.Add(nsx.Name, nsx);

                    string space = nsx.Name;
                    var cls = nsx.Classes;
                    for (int k = 0; k < cls.Count; k++)
                    {
                        ClassXml xml = cls[k];
                        var info = new ClassWrap(xml, space);
                        if (info.IsConfig())
                            new ConfigWrap(xml, space);
                    }
                    var ens = nsx.Enums;
                    for (int k = 0; k < ens.Count; k++)
                        new EnumWrap(ens[k], space);
                }
            }
            catch (Exception e)
            {
                Util.LogErrorFormat("路径:{0} 错误:{1}\n{2}", path, e.Message, e.StackTrace);
                return;
            }

            foreach (var item in ClassWrap.Classes)
                item.Value.VerifyDefine();
            foreach (var item in ConfigWrap.Configs)
                item.Value.VerifyDefine();
        }
        /// <summary>
        /// 加载数据,按结构中字段排列顺序读取
        /// 多态时,先顺序读取基类字段,再读取当前类型字段
        /// </summary>
        static void LoadData()
        {
            var cit = ConfigWrap.Configs.GetEnumerator();
            while (cit.MoveNext())
            {
                ConfigWrap cfg = cit.Current.Value;
                long start = DateTime.Now.Ticks;
                cfg.LoadData();
                long end = DateTime.Now.Ticks;
                if ((end - start) >= TimeSpan.TicksPerSecond)
                    Util.LogFormat("{0} 耗时 {1:F3}s\n", cfg.FullType, (end - start) * 1f / TimeSpan.TicksPerSecond);
            }
        }
        /// <summary>
        /// 检查数据
        /// </summary>
        static void VerifyData()
        {
            var cit = ConfigWrap.Configs.GetEnumerator();
            while (cit.MoveNext())
                cit.Current.Value.VerifyData();
        }
    }
}
