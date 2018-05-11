using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConfigGen.LocalInfo;
using ConfigGen.CmdUsage;


namespace ConfigGen
{
    class Program
    {
        //测试内容
        //1.表中数据是int还是string类型;HashSet中int=1和string=1是两种不同值
        //

        //局部流程
        //1.解析完类定义
        //2.检查类定义
        //3.解析数据定义

        static void Main(string[] args)
        {
            //命令行参数解析
            if (!CmdOption.Instance.Init(args)) return;

            //构建本地数据库
            var infoTypes = new List<LocalInfoType>() { LocalInfoType.FileInfo };
            if (CmdOption.Instance.CmdArgs.ContainsKey("-language"))
                infoTypes.Add(LocalInfoType.TypeInfo);
            if (CmdOption.Instance.CmdArgs.ContainsKey("-replace")
                || CmdOption.Instance.CmdArgs.ContainsKey("-find"))
                infoTypes.Add(LocalInfoType.FindInfo);
            LocalInfoManager.Instance.Init(infoTypes);
            LocalInfoManager.Instance.Update();


            //重新存储文件信息及类型信息
            //刷新数据库
            //TODO
            //查找内容/替换内容/导出类文件/导出csv
            Console.ReadKey();
        }
    }
}
