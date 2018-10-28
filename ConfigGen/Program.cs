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
                TypeInfo.Init();
                DataFileInfo.Init();

                bool canExportCsv = !string.IsNullOrWhiteSpace(Values.ExportCsv);
                bool canExportCode = !string.IsNullOrWhiteSpace(Values.ExportCode);
                bool canExportXmlCode = !string.IsNullOrWhiteSpace(Values.ExportXmlCode);
                bool canExportLua = !string.IsNullOrWhiteSpace(Values.ExportLua);

                if (canExportCsv)
                {
                    TableInfo.Init();
                    //类型,数据,检查操作
                    Util.Start();
                    TableChecker.CheckAllData();
                    TableChecker.CheckFullTable();
                    Util.Stop("==>> 检查数据耗时");

                    Util.Start();
                    Util.TryDeleteDirectory(Values.ExportCsv);
                    ExportCsv.Export();
                    Util.Stop("==>> Csv 数据导出完毕");
                }
                if (canExportCode)
                {
                    Util.Start();
                    Util.TryDeleteDirectory(Values.ExportCode);
                    ExportCSharp.Export_CsvOp();
                    Util.Stop("==>> Code 类导出完毕");
                }
                if (canExportXmlCode)
                {
                    Util.Start();
                    Util.TryDeleteDirectory(Values.ExportXmlCode);
                    ExportCSharp.Export_XmlOp();
                    Util.Stop("==>> XmlCode 类导出完毕");
                }
                if (canExportLua)
                {
                    Util.Start();
                    Util.TryDeleteDirectory(Values.ExportLua);
                    ExportLua.Export();
                    Util.Stop("==>> Lua 脚本导出完毕");
                }

            }
            catch (Exception e)
            {
                Util.LogErrorFormat("{0}\n{1}", e.Message, e.StackTrace);
            }

            Util.Log("\n\n\n");
            Util.Stop("=================>> 总共");
            Console.ReadKey();
        }
    }
}
