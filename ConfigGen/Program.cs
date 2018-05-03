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
            LocalInfoManager.Instance.InitInfo(infoTypes);
            //筛选Config文件,依据文件MD5判断是否有修改;
            LocalInfoManager.Instance.UpdateFileInfo();
            //更新Config数据类型信息和查询信息文件
            LocalInfoManager.Instance.UpdateTypeInfo();
            LocalInfoManager.Instance.UpdateFindInfo();

            //执行所有命令
            if (!CmdOption.Instance.Excute()) return;
            //重新存储文件信息及类型信息
            //刷新数据库
            //TODO
            //查找内容/替换内容/导出类文件/导出csv
            Console.Read();
        }
    }
}
