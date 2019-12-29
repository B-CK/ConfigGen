using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace Description
{
    class CmdOption
    {
        public const string CONFIG_XML = "-configXml";
        public const string DATA_DIR = "-data";
        public const string CS_DIR = "-csharp";
        //public const string JAVA_DIR = "-java";
        public const string LUA_DIR = "-lua";
        public const string XML_CODE_DIR = "-xmlCode";
        public const string GROUP = "-group";
        public const string CHECK = "-check";
        public const string HELP = "-help";

        void Usage()
        {
            Console.WriteLine("语法:ConfigGen.exe [option]");
            Console.WriteLine("     -configXml [path] 数据导出文件路径");
            Console.WriteLine("     -data [path] 导出数据到指定目录路径");
            Console.WriteLine("     -csharp [path] 导出结构到指定目录路径");
            Console.WriteLine("     -lua [path] 导出结构到指定目录路径");
            Console.WriteLine("     -xmlCode [path] 导出xml类到指定目录路径");
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

        private bool _result;
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
        private string CheckDirArg(string flag, string arg)
        {
            if (!CheckArg(flag, arg))
            {
                _result &= false;
                return null;
            }
            return NormalizePath(arg);
        }
        public Dictionary<string, List<string>> CmdArgs { get; private set; }
        public static string NormalizePath(string patth)
        {
            return patth.Replace("/", @"\");
        }
        public void Parse(string[] args)
        {
            _result = true;
            CmdArgs = new Dictionary<string, List<string>>();
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case CONFIG_XML:
                        if (!CheckArg(CONFIG_XML, args[++i]))
                        {
                            _result &= false;
                            break;
                        }
                        Setting.ConfigXml = Path.Combine(Setting.ApplicationDir, NormalizePath(args[i]));
                        if (!File.Exists(Setting.ConfigXml))
                        {
                            Util.LogErrorFormat("[{0}]confgxml配置文件{1}不存在!", CONFIG_XML, Setting.ConfigXml);
                            _result &= false;
                        }
                        Setting.ConfigDir = Path.GetDirectoryName(Setting.ConfigXml);
                        break;
                    case DATA_DIR:
                        Setting.DataDir = CheckDirArg(DATA_DIR, args[++i]);
                        break;
                    case CS_DIR:
                        Setting.CSDir = CheckDirArg(CS_DIR, args[++i]);
                        break;
                    case LUA_DIR:
                        Setting.LuaDir = CheckDirArg(LUA_DIR, args[++i]);
                        break;
                    case XML_CODE_DIR:
                        Setting.XmlCodeDir = CheckDirArg(XML_CODE_DIR, args[++i]);
                        break;
                    case GROUP:
                        if (!CheckArg(GROUP, args[++i]))
                        {
                            _result &= false;
                            break;
                        }
                        string[] groups = args[i].Split(Setting.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
                        for (int g = 0; g < groups.Length; g++)
                            groups[g] = groups[g].ToLower();
                        Setting.ExportGroup = new HashSet<string>(groups);
                        Setting.ExportGroup.Add(Setting.DefualtGroup);
                        break;
                    case CHECK:
                        Setting.Check = true;
                        break;
                    case HELP:
                        Usage();
                        break;
                    default:
                        Util.LogWarning("未知命令" + args[i]);
                        Usage();
                        break;
                }
            }

            if (!Setting.XmlCodeDir.IsEmpty() && Setting.ExportGroup.Count == 0)
            {
                Util.LogError("导出XmlCode编辑器代码时,必须指定Group参数");
                _result &= false;
            }
            if (Setting.CSDir.IsEmpty() && Setting.JavaDir.IsEmpty() && Setting.XmlCodeDir.IsEmpty()
                && Setting.LuaDir.IsEmpty() && Setting.DataDir.IsEmpty() && !Setting.Check)
            {
                Util.LogError("检查或者导出代码,数据三种操作至少有一个存在");
                _result &= false;
            }
        }

        public bool CheckResult()
        {
            return _result;
        }
    }
}

