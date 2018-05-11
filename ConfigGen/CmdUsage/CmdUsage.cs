using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ConfigGen.CmdUsage
{
    public class CmdHelp
    {
        /// <summary>
        /// 命令行描述
        /// </summary>
        public static CmdUsage Usage { get; private set; }
        public static void CreateUsage()
        {
            Usage = new CmdUsage();
            Usage.CmdUsageList = new List<CmdLine>();
            string cmdDes = "";         
           
            CmdLine cmdLine = new CmdLine();
            cmdLine.Cmd = "-configDir";
            cmdLine.CmdDes = "[必填]数据表所在文件夹路径.";
            Usage.CmdUsageList.Add(cmdLine);

            cmdLine = new CmdLine();
            cmdLine.Cmd = "-help";
            cmdLine.CmdDes = "[选填]显示应用命令行帮助文档";
            Usage.CmdUsageList.Add(cmdLine);

            cmdLine = new CmdLine();
            cmdLine.Cmd = "-optMode";
            cmdLine.CmdDes = "[选填]数据表操作模式,全部或者部分,默认只对部分文件进行操作";
            cmdLine.CmdArgs = new List<CmdArg>()
            {
                new CmdArg("part", "总是对修改文件进行操作"),
                new CmdArg("all", "无论文件是否修改,均进行操作"),
            };
            Usage.CmdUsageList.Add(cmdLine);

            //数据及类型生成操作
            cmdLine = new CmdLine();
            cmdLine.Cmd = "-csvDir";
            cmdLine.CmdDes = "[选填]数据表导出数据存储路径,静态语言必填.";
            Usage.CmdUsageList.Add(cmdLine);

            cmdLine = new CmdLine();
            cmdLine.Cmd = "-language";
            cmdDes = Util.ListStringSplit(Values.Languages, " ");
            cmdLine.CmdDes = string.Format("[选填]数据表导出语言.动态语言无需数据路径,静态语言需要数据存储路径.可选[{0}]", cmdDes);
            cmdLine.CmdArgs = new List<CmdArg>()
            {
                new CmdArg("|符号", "多种语言同时使用时,用|符号分隔."),
            };
            Usage.CmdUsageList.Add(cmdLine);

            cmdLine = new CmdLine();
            cmdLine.Cmd = "-group";
            cmdDes = Util.ListStringSplit(Values.Groups, " ");
            cmdLine.CmdDes = string.Format("[选填]数据分组导出.默认导出所有.可选[{0}].", cmdDes);
            cmdLine.CmdArgs = new List<CmdArg>()
            {
                new CmdArg("xx", "自定义组名"),
                new CmdArg("|符号", "多个组同时使用时,用|符号分隔."),
            };
            Usage.CmdUsageList.Add(cmdLine);

            //外部工具操作指令
            cmdLine = new CmdLine();
            cmdLine.Cmd = "-replace";
            cmdLine.CmdDes = "[选填]替换指定内容,格式old,new[,isCmd].例:vip,vipLevel,true,将vip替换成vipLevel,在命令行显示.";
            cmdLine.CmdArgs = new List<CmdArg>()
            {
                new CmdArg("old", "旧内容"),
                new CmdArg("new", "新内容"),
                new CmdArg("isCmd", "是否在命令行显示,默认为true,其他均为false"),
            };
            Usage.CmdUsageList.Add(cmdLine);

            cmdLine = new CmdLine();
            cmdLine.Cmd = "-find";
            cmdLine.CmdDes = "[选填]查找指定内容,格式content[,isCmd].例:vip,false,查找vip字符串,返回结果不在命令行显示.";
            cmdLine.CmdArgs = new List<CmdArg>()
            {
                new CmdArg("content", "需要查询的内容"),
                new CmdArg("isCmd", "是否在命令行显示,默认为true,其他均为false"),
            };
            Usage.CmdUsageList.Add(cmdLine);

            Util.Serialize(string.Format("{0}/Usage.xml", Values.ApplicationDir), Usage);
        }
    }

    [XmlRoot("Usage")]
    public class CmdUsage
    {
        [XmlArrayItem(typeof(CmdLine))]
        [XmlArray(ElementName = "UsageList")]
        public List<CmdLine> CmdUsageList;
    }
    [XmlInclude(typeof(CmdLine))]
    public class CmdLine
    {
        public string Cmd;
        public string CmdDes;
        public List<CmdArg> CmdArgs;
    }
    [XmlInclude(typeof(CmdArg))]
    public class CmdArg
    {
        public string Arg;
        public string ArgDes;
        public CmdArg() { }
        public CmdArg(string arg, string argDes)
        {
            Arg = arg;
            ArgDes = argDes;
        }
    }
}
