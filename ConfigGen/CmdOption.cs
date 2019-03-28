using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace ConfigGen
{
    class CmdOption
    {
        public const string CONFIG_XML = "-configXml";
        public const string EXPORT_CSV = "-dataDir";
        public const string EXPORT_CODE = "-codeDir";
        public const string EXPORT_XML_CODE = "-xmlCodedir";
        public const string GROUP = "-group";
        public const string CHECK = "-check";
        public const string HELP = "-help";

        public const string OPTION_MODE = "-optMode";
        public const string EXPORT_LUA = "-luaDir";
        void Usage()
        {
            Console.WriteLine("语法:ConfigGen.exe [option]");
            Console.WriteLine("     -configXml [path] 数据导出文件路径");
            Console.WriteLine("     -dataDir [path] 导出数据到指定目录路径");
            Console.WriteLine("     -codeDir [path] 导出结构到指定目录路径");
            Console.WriteLine("     -xmlCodeDir [path] 导出xml类到指定目录路径");
            Console.WriteLine("     -group [client|editor|server] 按分组导出数据,分组可自定义");
            Console.WriteLine("     -check 检查数据及结构");
            Console.WriteLine("     -help 打印指令说明");

            Environment.Exit(0);
        }

        public static CmdOption Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CmdOption();
                return _ins;
            }
        }
        private static CmdOption _ins;

        private CmdOption() { }
        private bool CheckArg(string cmd, string arg)
        {
            if (arg.IsEmpty())
            {
                Util.LogErrorFormat("{0} 命令参数为空", cmd);
                Usage();
                return false;
            }
            return true;
        }
        public Dictionary<string, List<string>> CmdArgs { get; private set; }
        public static string NormalizePath(string patth)
        {
            return patth.Replace("/", @"\");
        }
        public bool Init(string[] args)
        {
            CmdArgs = new Dictionary<string, List<string>>();
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                switch (args[i])
                {
                    case CONFIG_XML:
                        if (!CheckArg(CONFIG_XML, args[++i])) return false;
                        Values.ConfigXml = NormalizePath(string.Format(@"{0}\{1}", Values.ApplicationDir, args[i]));
                        Values.ConfigDir = Path.GetDirectoryName(Values.ConfigXml);
                        if (!File.Exists(Values.ConfigXml))
                        {
                            Util.LogErrorFormat("[{0}]导出文件{1}不存在!", CONFIG_XML, Values.ConfigXml);
                            return false;
                        }
                        break;
                    case EXPORT_CSV:
                        if (!CheckArg(EXPORT_CSV, args[++i])) return false;
                        Values.ExportCsv = NormalizePath(args[i]);
                        if (!Directory.Exists(Values.ExportCsv))
                        {
                            Util.LogErrorFormat("[{0}]导出数据目录{1}不存在!", EXPORT_CSV, Values.ExportCsv);
                            return false;
                        }
                        break;
                    case EXPORT_CODE:
                        if (!CheckArg(EXPORT_CODE, args[++i])) return false;
                        Values.ExportCode = NormalizePath(args[i]);
                        if (!Directory.Exists(Values.ExportCode))
                        {
                            Util.LogErrorFormat("[{0}]导出代码目录{1}不存在!", EXPORT_CODE, Values.ExportCode);
                            return false;
                        }
                        break;
                    case EXPORT_XML_CODE:
                        if (!CheckArg(EXPORT_XML_CODE, args[++i])) return false;
                        Values.ExportXmlCode = NormalizePath(args[i]);
                        if (!Directory.Exists(Values.ExportXmlCode))
                        {
                            Util.LogErrorFormat("[{0}]导出Xml代码目录{1}不存在!", EXPORT_XML_CODE, Values.ExportXmlCode);
                            return false;
                        }
                        break;
                    case GROUP:
                        if (!CheckArg(GROUP, args[++i])) return false;
                        string[] groups = args[i].Split(Values.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
                        for (int g = 0; g < groups.Length; g++)
                            groups[g] = groups[g].ToLower();
                        Values.ExportGroup = new HashSet<string>(groups);
                        Values.ExportGroup.Add(Values.DefualtGroup);
                        break;
                    case CHECK:
                        Values.OnlyCheck = true;
                        break;
                    case HELP:
                        Usage();
                        break;
                    default:
                        Util.LogError("未知命令" + arg);
                        Usage();
                        break;
                }
            }

            return true;
        }
    }
}

