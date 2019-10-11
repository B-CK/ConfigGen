using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDL
{
    /// <summary>
    /// 生成,检查工具
    /// </summary>
    class Program
    {
        //按模块生成
        const string MODULE = "-m";
        //按命名空间生成
        const string INPUT = "-i";
        //输出目录
        const string OUTPUT = "-o";
        //生成语言类型
        const string LAN = "-lan";
        //生成数据
        const string DATA = "-data";
        const string GROUP = "-gourp";
        const string CHECK = "-check";
        const string HELP = "-help";

        static Dictionary<string, string> CmdArgs = new Dictionary<string, string>();
        //应用程序路径
        static string ApplicationDir => Directory.GetCurrentDirectory();

        static bool CheckEmptyArg(string cmd, string arg)
        {
            if (arg.IsEmpty())
            {
                Util.LogErrorFormat("{0} 命令参数为空", cmd);
                return false;
            }
            return true;
        }
        static void AddCmdArg(string key, string value)
        {
            if (!CmdArgs.ContainsKey(key))
                CmdArgs.Add(key, value);
        }

        static void Main(string[] args)
        {
            bool isOk = true;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].IsEmpty()) continue;
                string[] nodes = args[i].Split(Util.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
                if (nodes.Length == 0)
                {
                    Util.LogErrorFormat("{0} 命令格式错误", args[i]);
                    continue;
                }

                string key = nodes[0];
                if (key != CHECK && key != HELP && !CheckEmptyArg(key, nodes[1]))
                {
                    isOk &= false;
                    continue;
                }

                switch (key)
                {
                    case MODULE:
                        string module = Path.Combine(ApplicationDir, Util.NormalizePath(nodes[1]));
                        if (!File.Exists(module))
                        {
                            Util.LogErrorFormat("{0}模块文件不存在!", module);
                            isOk &= false;
                            break;
                        }
                        AddCmdArg(key, module);
                        break;
                    case INPUT:
                        string ns = Path.Combine(ApplicationDir, Util.NormalizePath(nodes[1]));
                        if (!File.Exists(ns))
                        {
                            Util.LogErrorFormat("{0}命名空间文件不存在!", ns);
                            isOk &= false;
                            break;
                        }
                        AddCmdArg(key, ns);
                        break;
                    case OUTPUT:
                        string output = Path.Combine(ApplicationDir, Util.NormalizePath(nodes[1]));
                        AddCmdArg(key, output);
                        break;
                    case LAN:
                        AddCmdArg(key, nodes[1]);
                        break;
                    case DATA:
                        string dataDir = Path.Combine(ApplicationDir, Util.NormalizePath(nodes[1]));
                        AddCmdArg(key, dataDir);
                        break;
                    case GROUP:
                        AddCmdArg(key, nodes[1]);
                        break;
                    case CHECK:
                    case HELP:
                        AddCmdArg(key, "");
                        break;
                    default:
                        Util.LogWarning($"未知命令:{key}");
                        isOk &= false;
                        break;
                }
            }

            if (isOk == false)
            {
                Util.LogError("命令参数解析失败!");
                Usage();
#if DEBUG
                Console.ReadKey();
#endif
                return;
            }

            foreach (var cmd in CmdArgs)
            {
                switch (cmd.Key)
                {
                    case MODULE:
                        break;
                    case INPUT:
                        break;
                    case OUTPUT:
                        break;
                    case LAN:
                        break;
                    case DATA:
                        break;
                    case GROUP:
                        break;
                    case CHECK:
                        break;
                    case HELP:
                        break;
                }
            }

#if DEBUG
            Console.ReadKey();
#endif
        }

        static void Usage()
        {
            Console.WriteLine("语法:DDL.exe [命令参数] ");
            Console.WriteLine($"\t\t{MODULE}:[模块路径]\t按照模块描述生成内容");
            Console.WriteLine($"\t\t{INPUT}:[命名空间路径]\t按照命名空间描述生成内容");
            Console.WriteLine($"\t\t{LAN}:[语言]\t生成指定语言代码,目前支持C#");
            Console.WriteLine($"\t\t{OUTPUT}:[目录路径]\t用于保存生成文件的目录");
            Console.WriteLine($"\t\t{DATA}:[数据目录]\t用于保存数据文件的目录");
            Console.WriteLine($"\t\t{GROUP}:[组1|组2]\t仅导出指定组的脚本/数据,默认全部导出");
            Console.WriteLine($"\t\t{CHECK}\t检查所有数据,在非导出数据情况下也可以进行");
            Console.WriteLine($"\t\t{HELP}\t使用说明");

            Environment.Exit(0);
        }
    }
}
