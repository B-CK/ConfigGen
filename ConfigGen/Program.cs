using System;
using ConfigGen.Export;
using ConfigGen.Description;
using System.Collections.Generic;
using System.IO;
using ConfigGen.TypeInfo;

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
                CmdOption.Ins.Parse(args);
                //解析命令
                if (!CmdOption.Ins.CheckResult())
                {
                    Util.LogError("命令参数解析失败!");
                    Console.ReadKey();
                    return;
                }

                LoadDefine();
                VerifyDefine();


                //构建本地数据库               
                Description.TypeInfo.Init();
                DataFileInfo.Init();

                bool canExportCsv = !string.IsNullOrWhiteSpace(Values.DataDir);
                bool canExportCode = !string.IsNullOrWhiteSpace(Values.CSDir);
                bool canExportXmlCode = !string.IsNullOrWhiteSpace(Values.XmlCodeDir);
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
                    Util.TryDeleteDirectory(Values.DataDir);
                    ExportCsv.Export();
                    Util.Stop("==>> Csv 数据导出完毕");
                }
                if (canExportCode)
                {
                    Util.Start();
                    Util.TryDeleteDirectory(Values.CSDir);
                    ExportCSharp.Export_CsvOp();
                    Util.Stop("==>> Code 类导出完毕");
                }
                if (canExportXmlCode)
                {
                    Util.Start();
                    Util.TryDeleteDirectory(Values.XmlCodeDir);
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

        static void LoadDefine()
        {
            //解析类型定义
            Dictionary<string, NamespaceDes> pairs = new Dictionary<string, NamespaceDes>();
            var configXml = Util.Deserialize(Values.ConfigXml, typeof(ConfigXml)) as ConfigXml;
            if (configXml.Root.IsEmpty())
                throw new Exception("数据结构导出时必须指定命名空间根节点<Config Root=\"**\">");
            Values.ConfigRootNode = configXml.Root;
            List<string> include = configXml.Include;
            string path = "无法解析Xml.NamespaceDes";
            try
            {
                for (int i = 0; i < include.Count; i++)
                {
                    path = Util.GetAbsPath(include[i]);
                    var des = Util.Deserialize(path, typeof(NamespaceDes)) as NamespaceDes;
                    if (pairs.ContainsKey(des.Name))
                    {
                        pairs[des.Name].Classes.AddRange(des.Classes);
                        pairs[des.Name].Enums.AddRange(des.Enums);
                    }
                    else
                    {
                        des.XmlDirPath = Path.GetDirectoryName(path);
                        pairs.Add(des.Name, des);
                    }
                }
            }
            catch (Exception e)
            {
                Util.LogErrorFormat("路径:{0} Error:{1}\n{2}", path, e.Message, e.StackTrace);
                return;
            }
            GroupInfo.LoadGroup(configXml.Group);
            HashSet<string> fullHash = new HashSet<string>();
            foreach (var item in pairs)
            {
                string namespace0 = item.Key;
                for (int i = 0; i < item.Value.Classes.Count; i++)
                    new ClassInfo(item.Value.Classes[i], namespace0);
                for (int i = 0; i < item.Value.Enums.Count; i++)
                    new EnumInfo(item.Value.Enums[i], namespace0);
            }
        }

        static void VerifyDefine()
        {

        }

        static void LoadData()
        {

        }

        static void VerifyData()
        {

        }
    }
}
