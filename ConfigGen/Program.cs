using System;
using ConfigGen.Export;
using ConfigGen.LocalInfo;

namespace ConfigGen
{
    /// 所有的路径均以应用为相对路径生成
    /// 尽可能少配置Excel文件,文件读取非常耗时
    class Program
    {
        //未完成内容
        //1.说明文档按功能来整理

        static void Main(string[] args)
        {
            Util.Start();

            try
            {
                //解析命令
                if (!CmdOption.Instance.Init(args))
                {
                    Console.ReadKey();
                    return;
                }

                //构建本地数据库
                FileInfo.Init();
                TypeInfo.Init();
                TableInfo.Init();

                //类型,数据,检查操作
                Util.Start();
                TableChecker.CheckAllData();
                TableChecker.CheckFullTable();
                Util.Stop("==>> 检查数据耗时");

                //导出数据
                if (!string.IsNullOrWhiteSpace(Values.ExportCsv))
                {
                    Util.Start();
                    ExportCsv.Export();
                    Util.Stop("==>> Csv数据导出完毕");
                }
                if (!string.IsNullOrWhiteSpace(Values.ExportCSharp))
                {
                    Util.Start();
                    ExportCSharp.Export_CsvOp();
                    Util.Stop("==>> CS Csv操作类导出完毕");
                }
                if (!string.IsNullOrWhiteSpace(Values.ExportCsLson))
                {
                    Util.Start();
                    ExportCSharp.Export_LsonOp();
                    Util.Stop("==>> CS Lson操作类导出完毕");
                }
                if (!string.IsNullOrWhiteSpace(Values.ExportLua))
                {
                    Util.Start();
                    ExportLua.Export();
                    Util.Stop("==>> Lua操作类导出完毕");
                }

            }
            catch (Exception e)
            {
                Util.LogErrorFormat("数据导出异常!!!\n{0}\n{1}", e.Message, e.StackTrace);
            }

            Util.Log("\n\n\n");
            Util.Stop("=================>> 总共");
            Console.ReadKey();
        }
    }
}
