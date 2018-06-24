using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace ConfigGen.CmdUsage
{
    class CmdOption
    {
        public const string CONFIG_DIR = "-configDir";
        public const string HELP = "-help";
        public const string OPTION_MODE = "-optMode";
        public const string EXPORT_CSV = "-exportCsv";
        public const string EXPORT_CSHARP = "-exportCSharp";
        public const string EXPORT_CS_LSON = "-exportCsLson";
        public const string EXPORT_LUA = "-exportLua";
        public const string GROUP = "-group";
        public const string REPLACE = "-replace";
        public const string FIND = "-find";


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

        private Dictionary<string, List<string>> _cmdArgs = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> CmdArgs { get { return _cmdArgs; } }
        public bool Init(string[] args)
        {
            string currentArg = "";
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.StartsWith("-"))
                {
                    currentArg = arg;
                    if (!_cmdArgs.ContainsKey(arg))
                        _cmdArgs.Add(arg, new List<string>());
                    else
                        Util.LogWarning(string.Format("忽略重复命令{0},以第一个为主.", arg));
                }
                else if (_cmdArgs.ContainsKey(currentArg))
                    _cmdArgs[currentArg].Add(arg);
                else
                    Util.LogWarning(string.Format("异常参数{0},未正常读取.", arg));
            }

            //导出用法到Xml
            CmdHelp.CreateUsage();

            try
            {
                Values.IsOptPart = true;
                foreach (var cmd in _cmdArgs)
                {
                    string cmdName = cmd.Key;
                    switch (cmdName)
                    {
                        case CONFIG_DIR:
                            if (!CheckArgList(cmdName, cmd.Value)) return false;
                            Values.ConfigDir = string.Format(@"{0}\{1}\", Values.ApplicationDir, cmd.Value[0]);
                            if (!Directory.Exists(Values.ConfigDir))
                            {
                                Util.LogErrorFormat("[{0}]Config文件夹不在此路径{1}", cmdName, Values.ConfigDir);
                                Values.ConfigDir = null;
                                return false;
                            }
                            break;
                        case HELP:
                            Help();
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
                            Values.ExportCsv = cmd.Value[0];
                            if (string.IsNullOrWhiteSpace(Values.ExportCsv))
                                Values.ExportCsv = null;
                            break;
                        case EXPORT_CSHARP://null不做语言类导出
                            if (!CheckArgList(cmdName, cmd.Value)) return false;
                            Values.ExportCSharp = cmd.Value[0];
                            if (string.IsNullOrWhiteSpace(Values.ExportCSharp))
                                Values.ExportCSharp = null;
                            break;
                        case EXPORT_CS_LSON://null不做语言类导出
                            if (!CheckArgList(cmdName, cmd.Value)) return false;
                            Values.ExportCsLson = cmd.Value[0];
                            if (string.IsNullOrWhiteSpace(Values.ExportCsLson))
                                Values.ExportCsLson = null;
                            break;
                        case EXPORT_LUA:
                            if (!CheckArgList(cmdName, cmd.Value)) return false;
                            Values.ExportLua = cmd.Value[0];
                            if (string.IsNullOrWhiteSpace(Values.ExportLua))
                                Values.ExportLua = null;
                            break;
                        case GROUP://默认导出所有分组
                            string[] groups = cmd.Value[0].Split(Values.ArgsSplitFlag.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            Values.ExportGroup = new HashSet<string>(groups);
                            if (Values.ExportGroup.Count == 0)
                                Values.ExportGroup.Add(Values.AllGroup);
                            break;
                        case REPLACE:
                            if (!CheckArgList(cmdName, cmd.Value)) return false;
                            break;
                        case FIND:
                            if (!CheckArgList(cmdName, cmd.Value)) return false;
                            break;
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
        private void Help()
        {
            CmdUsage usage = CmdHelp.Usage;
            int maxLength = 0;
            for (int i = 0; i < usage.CmdUsageList.Count; i++)
            {
                CmdLine cmdline = usage.CmdUsageList[i];
                if (cmdline.Cmd.Length > maxLength)
                    maxLength = cmdline.Cmd.Length;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("用法:");
            const int lineLength = 50;
            for (int i = 0; i < usage.CmdUsageList.Count; i++)
            {
                CmdLine cmdline = usage.CmdUsageList[i];
                int delta = maxLength - cmdline.Cmd.Length;
                int lines = cmdline.CmdDes.Length / lineLength;
                int remain = cmdline.CmdDes.Length % lineLength;
                sb.AppendFormat("{0}{1}\r\n{2}\r\n", GetSpace(3), cmdline.Cmd, cmdline.CmdDes);

                for (int k = 0; cmdline.CmdArgs != null && k < cmdline.CmdArgs.Count; k++)
                {
                    string arg = cmdline.CmdArgs[k].Arg;
                    string des = cmdline.CmdArgs[k].ArgDes;
                    sb.AppendFormat("{0}{1}{2}{3}\r\n", GetSpace(3 + maxLength), arg, GetSpace(14 - arg.Length), des);
                }
                sb.AppendLine();
            }
            Util.Log(sb.ToString());
        }
        private string GetSpace(int num)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < num; j++)
                sb.Append(" ");
            return sb.ToString();
        }
    }
}
