using System;
using System.IO;
using System.Text;
using System.Linq;
using ConfigGen.Export;
using ConfigGen.CmdUsage;
using ConfigGen.LocalInfo;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConfigGen
{
    class Program
    {
        //未完成内容
        //4.CSharp Xml写
        //3.Xml读
        //5.查找和替换功能
        //1.字段检查,自定义检查规则
        //2.导出lua 数据,类

        //static Stopwatch stopwatch = new Stopwatch();

        static void Main(string[] args)
        {
            Util.Start();
            //stopwatch.Start();

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
                Util.Stop("=================>> Csv数据导出完毕");
                Util.Log("");
            }
            if (!string.IsNullOrWhiteSpace(Values.ExportCSharp))
            {
                Util.Start();
                ExportCSharp.Export_CsvOp();
                Util.Stop("=================>> CSharp类导出完毕");
                Util.Log("");
            }
            if (!string.IsNullOrWhiteSpace(Values.ExportCsLson))
            {
                Util.Start();
                ExportCSharp.Export_LsonOp();
                Util.Stop("==>>CSharp类导出完毕");
                Util.Log("");
            }

            Util.Log("\n\n\n");
            Util.Stop("=================>> 总共");
            //Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.ReadKey();
        }

        //static void TEST()
        //{
        //    var subTypeQuery = from t in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
        //                       where IsSubClassOf(t, Type.GetType("Base"))
        //                       select t;

        //    foreach (var type in subTypeQuery)
        //    {
        //        Console.WriteLine(type);
        //    }
        //}

        //static bool IsSubClassOf(Type type, Type baseType)
        //{
        //    var b = type.BaseType;
        //    while (b != null)
        //    {
        //        if (b.Equals(baseType))
        //        {
        //            return true;
        //        }
        //        b = b.BaseType;
        //    }
        //    return false;
        //}
    }
}
