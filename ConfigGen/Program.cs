using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConfigGen.LocalInfo;
using ConfigGen.CmdUsage;
using ConfigGen.Export;


namespace ConfigGen
{
    class Program
    {
        //注:新增集合类型不会再后续生产中删除,只会越来越多,但不重复.

        //测试内容
        //1.表中数据是int还是string类型;HashSet中int=1和string=1是两种不同值
        //
        static void Main(string[] args)
        {
            //命令行参数解析
            if (!CmdOption.Instance.Init(args)) return;

            //构建本地数据库
            var infoTypes = new List<LocalInfoType>() { LocalInfoType.FileInfo };
            if (CmdOption.Instance.CmdArgs.ContainsKey("-exportCSharp")
                || CmdOption.Instance.CmdArgs.ContainsKey("-exportCsv"))
                infoTypes.Add(LocalInfoType.TypeInfo);
            if (CmdOption.Instance.CmdArgs.ContainsKey("-replace")
                || CmdOption.Instance.CmdArgs.ContainsKey("-find"))
                infoTypes.Add(LocalInfoType.FindInfo);
            LocalInfoManager.Instance.Init(infoTypes);
            LocalInfoManager.Instance.Update();

            //导出数据
            if (!string.IsNullOrWhiteSpace(Values.ExportCsv))
            {
                Util.Start();
                ExportCsv.Export();
                Util.Stop("==>>Csv数据导出完毕");
                Util.Log("");
            }
            if (!string.IsNullOrWhiteSpace(Values.ExportCSharp))
            {
                Util.Start();
                ExportCSharp.Export();
                Util.Stop("==>>CSharp类导出完毕");
                Util.Log("");
            }


            //重新存储文件信息及类型信息
            //刷新数据库
            //TODO
            //查找内容/替换内容/导出类文件/导出csv
            Console.ReadKey();
        }
    }
}
