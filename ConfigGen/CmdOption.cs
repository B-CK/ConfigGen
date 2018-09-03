using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace ConfigGen
{
    class CmdOption
    {
        public const string CONFIG_DIR = "-configDir";
        public const string OPTION_MODE = "-optMode";
        public const string EXPORT_CSV = "-dataDir";
        public const string EXPORT_CSHARP = "-codeDir";
        public const string EXPORT_CS_LSON = "-xmlCodeDir";
        public const string EXPORT_LUA = "-luaDir";
        public const string GROUP = "-group";
        public const string EXPORT_INFO = "-export";
        public const string HELP = "-help";
        //public const string REPLACE = "-replace";
        //public const string FIND = "-find";

        void Usage()
        {
            Console.WriteLine("-configDir [path] 数据文件根目录路径");
            Console.WriteLine("-optMode [all|part] 导出模式,全部导出或者只导出被修改文件");
            Console.WriteLine("-dataDir [path] 导出数据到指定目录路径");
            Console.WriteLine("-codeDir [path] 导出结构到指定目录路径");
            Console.WriteLine("-xmlCodeDir [path] 导出xml类到指定目录路径");
            Console.WriteLine("-luaDir [path] 导出lua脚本到指定目录路径");
            Console.WriteLine("-group [client|editor|server] 按分组导出数据,分组可自定义");
            Console.WriteLine("-export [path] 按配置文件导出数据或者结构类,配置可自定义");
            Console.WriteLine("--help 打印指令说明");
        }


        public static CmdOption Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CmdOption();
                return _instance;
            }
        }
        private static CmdOption _instance;

        private CmdOption() { }

        public Dictionary<string, List<string>> CmdArgs { get; private set; }
        public bool Init(string[] args)
        {
            CmdArgs = new Dictionary<string, List<string>>();
            string currentArg = "";
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.StartsWith("-"))
                {
                    currentArg = arg;
                    if (!CmdArgs.ContainsKey(arg))
                        CmdArgs.Add(arg, new List<string>());
                    else
                        Util.LogWarning(string.Format("忽略重复命令{0},以第一个为主.", arg));
                }
                else if (CmdArgs.ContainsKey(currentArg))
                    CmdArgs[currentArg].Add(arg);
                else
                    Util.LogWarning(string.Format("异常参数{0},未正常读取.", arg));
            }

            try
            {
                Values.IsOptPart = true;
                foreach (var cmd in CmdArgs)
                {
                    string cmdName = cmd.Key;
                    switch (cmdName)
                    {
                        case CONFIG_DIR:
                            if (!CheckArgList(cmdName, cmd.Value)) return false;
                            Values.ConfigDir = Util.NormalizePath(string.Format(@"{0}\{1}\", Values.ApplicationDir, cmd.Value[0]));
                            if (!Directory.Exists(Values.ConfigDir))
                            {
                                Util.LogErrorFormat("[{0}]Config文件夹不在此路径{1}", cmdName, Values.ConfigDir);
                                Values.ConfigDir = null;
                                return false;
                            }
                            break;
                        case HELP:
                            Usage();
                            break;
                        case OPTION_MODE:
                            if (!CheckArgList(cmdName, cmd.Value)) return false;
                            bool isNormal = "part".Equals(cmd.Value[0]) || "all".Equals(cmd.Value[0]);
                            if (!isNormal)
                            {
                                Util.LogErrorFormat("-optMode 参数异常!可选参数为all,part.错误:{0}", cmd.Value[0]);
                                return false;
                            }
                            Values.IsOptPart = "part".Equals(cmd.Value[0]);
                            break;
                        case EXPORT_CSV://null不导出csv
                            if (!CheckArgList(cmdName, cmd.Value)) return false;
                            Values.ExportCsv = Util.NormalizePath(cmd.Value[0]);
                            if (string.IsNullOrWhiteSpace(Values.ExportCsv))
                                Values.ExportCsv = null;
                            break;
                        case EXPORT_CSHARP://null不做语言类导出
                            if (!CheckArgList(cmdName, cmd.Value)) return false;
                            Values.ExportCode = Util.NormalizePath(cmd.Value[0]);
                            if (string.IsNullOrWhiteSpace(Values.ExportCode))
                                Values.ExportCode = null;
                            break;
                        case EXPORT_CS_LSON://null不做语言类导出
                            if (!CheckArgList(cmdName, cmd.Value)) return false;
                            Values.ExportXmlCode = Util.NormalizePath(cmd.Value[0]);
                            if (string.IsNullOrWhiteSpace(Values.ExportXmlCode))
                                Values.ExportXmlCode = null;
                            break;
                        case EXPORT_LUA:
                            if (!CheckArgList(cmdName, cmd.Value)) return false;
                            Values.ExportLua = Util.NormalizePath(cmd.Value[0]);
                            if (string.IsNullOrWhiteSpace(Values.ExportLua))
                                Values.ExportLua = null;
                            break;
                        case GROUP://默认导出所有分组
                            string[] groups = cmd.Value[0].Split(Values.ItemSplitFlag.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < groups.Length; i++)
                                groups[i] = groups[i].ToLower();
                            Values.ExportGroup = new HashSet<string>(groups);
                            Values.ExportGroup.Add(Values.DefualtGroup);
                            break;
                        case EXPORT_INFO:
                            if (!CheckArgList(cmdName, cmd.Value)) return false;
                            Values.ExportFilter = Util.NormalizePath((string.Format(@"{0}\{1}", Values.ApplicationDir, cmd.Value[0])));
                            if (!File.Exists(Values.ExportFilter))
                            {
                                Util.LogErrorFormat("[{0}]导出定义文件不在此路径{1}", cmdName, Values.ExportFilter);
                                Values.ExportFilter = null;
                                return false;
                            }
                            break;
                        //case REPLACE:
                        //    if (!CheckArgList(cmdName, cmd.Value)) return false;
                        //    break;
                        //case FIND:
                        //    if (!CheckArgList(cmdName, cmd.Value)) return false;
                        //    break;
                        default:
                            Util.LogErrorFormat("应用无法执行此命令{0}.", cmdName);
                            return false;
                    }
                }
            }
            catch (Exception e)
            {
                Util.LogError("初始化命令参数失败!");
                Util.LogErrorFormat("{0}\r\n{1}", e.Message, e.StackTrace);
                return false;
            }
            return true;
        }

        private bool CheckArgList(string cmdName, List<string> list)
        {
            if (list.Count == 0)
            {
                Util.LogErrorFormat("[{0}]命令参数未配置!", cmdName);
                return false;
            }
            return true;
        }
    }
}
