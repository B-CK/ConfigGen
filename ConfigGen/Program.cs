using System;
using ConfigGen.Export;
using ConfigGen.CmdUsage;
using ConfigGen.LocalInfo;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.ComponentModel;


namespace ConfigGen
{
    /// 所有的路径均以应用为相对路径生成
    /// 尽可能少配置Excel文件,文件读取非常耗时
    class Program
    {
        //未完成内容
        //5.查找和替换功能?
        //3.导出lua 数据,类

        //说明文档按功能来整理

        static void Main(string[] args)
        {
            Util.Start();

            //命令行参数解析
            if (!CmdOption.Instance.Init(args))
            {
                Console.ReadKey();
                return;
            }

            //构建本地数据库
            Local.Instance.FileInfoLib = LocalInfo.FileInfo.Create();
            Local.Instance.UpdateFileInfo();
            if (CmdOption.Instance.CmdArgs.ContainsKey(CmdOption.EXPORT_CSHARP)
                || CmdOption.Instance.CmdArgs.ContainsKey(CmdOption.EXPORT_CSV))
            {
                Local.Instance.TypeInfoLib = TypeInfo.Create();
                Local.Instance.DefineInfoDict = new Dictionary<string, TableDefineInfo>();
                Local.Instance.DataInfoDict = new Dictionary<string, TableDataInfo>();
                Local.Instance.UpdateTypeInfo();
            }        
            if (CmdOption.Instance.CmdArgs.ContainsKey(CmdOption.REPLACE)
                || CmdOption.Instance.CmdArgs.ContainsKey(CmdOption.FIND))
            {
                Local.Instance.FindInfoLib = FindInfo.Create();
                Local.Instance.UpdateFindInfo();
            }

            //数据分组
            Local.Instance.DoGrouping();

            //导出数据
            if (!string.IsNullOrWhiteSpace(Values.ExportCsv))
            {
                Util.Start();
                ExportCsv.Export();
                Util.Stopln("==>> Csv数据导出完毕");
            }
            if (!string.IsNullOrWhiteSpace(Values.ExportCSharp))
            {
                Util.Start();
                ExportCSharp.Export_CsvOp();
                Util.Stopln("==>> CS Csv操作类导出完毕");
            }
            if (!string.IsNullOrWhiteSpace(Values.ExportCsLson))
            {
                Util.Start();
                ExportCSharp.Export_LsonOp();
                Util.Stopln("==>> CS Lson操作类导出完毕");
            }

            Util.Log("\n\n\n");
            Util.Stop("=================>> 总共");
            Console.ReadKey();
        }

        //static void Ex()
        //{
        //    int c = 10000;
        //    List<object> ins = new List<object>();
        //    for (int i = 0; i < c; i++)
        //        ins.Add(i.ToString());

        //    Util.Start();
        //    HashSet<object> vs = new HashSet<object>(ins);
        //    int result = 0;
        //    for (int i = 0; i < c; i++)
        //    {
        //        if (vs.Contains(i.ToString()))
        //        {
        //            result++;
        //        }
        //    }
        //    Util.Stopln(result.ToString() + "  Hash Time : ");

        //    Util.Start();
        //    result = 0;
        //    for (int i = 0; i < c; i++)
        //    {
        //        if (ins.Contains(i.ToString()))
        //        {
        //            result++;
        //        }
        //    }
        //    Util.Stopln(result.ToString() + "  List Time : ");



        //}

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

        //string path = @"E:\C#Project\ConfigGen\Csv\AllType\Lson数据\2.json";
        //string json = File.ReadAllText(path);
        //JObject jObject = JObject.Parse(json);
        //foreach (var item in jObject)
        //{
        //    object value = null;
        //    switch (item.Value.Type)
        //    {
        //        case JTokenType.Undefined:
        //        case JTokenType.Guid:
        //        case JTokenType.TimeSpan:
        //        case JTokenType.Uri:
        //        case JTokenType.Bytes:
        //        case JTokenType.Raw:
        //        case JTokenType.Date:
        //        case JTokenType.Null:
        //        case JTokenType.None:
        //        case JTokenType.Constructor:
        //        case JTokenType.Property:
        //        case JTokenType.Comment:
        //            value = item.Value.Type;
        //            break;
        //        case JTokenType.Array:
        //            string s = "";
        //            foreach (var v in item.Value.Values())
        //                s += string.Format("{0},", (object)v);
        //            value = s;
        //            JArray jAry = item.Value as JArray;
        //            break;
        //        case JTokenType.Object:
        //            value = item.Value.Type;
        //            JObject jObj = item.Value as JObject;
        //            if (item.Key.Contains("Dict"))
        //            {
        //                var properties = jObj.GetEnumerator();
        //                while (properties.MoveNext())
        //                {
        //                    Console.WriteLine(string.Format("---------->{0} - {1}", properties.Current.Key, properties.Current.Value));
        //                }
        //            }
        //            break;
        //        case JTokenType.Integer:
        //            //value = (long)item.Value;
        //            //break;
        //        case JTokenType.Float:
        //            //value = item.Value.Value<float>();
        //            //break;
        //        case JTokenType.String:
        //            //value = item.Value.Value<string>();
        //            //break;
        //        case JTokenType.Boolean:
        //            //value = item.Value.Value<bool>();
        //            value = item.Value;
        //            break;
        //        default:
        //            break;
        //    }
        //    Console.WriteLine(string.Format("{0} - {1} - {2}", item.Key, value, (object)item.Value));
        //}

        //Console.ReadKey();
        //return;
    }
}
