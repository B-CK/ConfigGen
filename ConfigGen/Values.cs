using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConfigGen
{
    class Values
    {
        /// <summary>
        /// 数据表文件夹路径
        /// </summary>
        public static string ConfigDir { get; set; }
        /// <summary>
        /// 导出CSharp类型
        /// </summary>
        public static string ExportCSharp { get; set; }
        /// <summary>
        /// 导出csv存储路径
        /// </summary>
        public static string ExportCsv { get; set; }
        /// <summary>
        /// 数据导出分组
        /// </summary>
        public static string ExportGroup { get; set; }
        /// <summary>
        /// 生成本地数据类型信息
        /// </summary>
        public static string GenTypeDB { get; set; }
        /// <summary>
        /// 是否只对已修改文件进行操作
        /// </summary>
        public static bool IsOptPart { get; set; }
        ///// <summary>
        ///// 替换内容参数
        ///// </summary>
        //public static string ReplaceArgs { get; set; }
        ///// <summary>
        ///// 查找内容参数
        ///// </summary>
        //public static string FindArgs { get; set; }


        /// <summary>
        /// 存储运行时打印的所有信息，在程序运行完毕后输出为txt文件，从而解决如果输出内容过多控制台无法显示全部信息的问题
        /// </summary>
        public static StringBuilder LogContent = new StringBuilder();

        #region 常量
        /// <summary>
        /// excel类数据库文件
        /// 路径与应用路径相同
        /// </summary>
        public const string ExcelInfoDB = "excelInfo.ei";
        public static readonly string ApplicationDir = Directory.GetCurrentDirectory();
        /// <summary>
        /// 导出所有分组
        /// </summary>
        public const string AllGroup = "all";
        /// <summary>
        /// 数据表格Sheet名定义格式
        /// </summary>
        public const string DataSheetPrefix = "data";
        /// <summary>
        /// 数据类型表格Sheet名定义格式
        /// </summary>
        public const string DefineSheetPrefix = "define";
        /// <summary>
        /// 数据表前四行分别声明字段检查规则,字段描述,字段名,字段数据类型.
        /// </summary>
        public const int DataSheetFieldIndex = 0;
        public const int DataSheetCheckIndex = 1;
        //public const int DataSheetDesIndex = 2;
        //public const int DataSheetTypeIndex = 3;
        public const int DataSheetDataStartIndex = 4;
        /// <summary>
        /// 数据类型定义起始符
        /// </summary>
        public const string DefineTypeFlag = "##";
        /// <summary>
        /// 数据集合结束符,可用在数据/子字段上.
        /// </summary>
        public const string DataSetEndFlag = "##";
        /// <summary>
        /// 多个数据字段检查规则组合分隔符
        /// </summary>
        public const string CheckRuleSplitFlag = "|";
        /// <summary>
        /// 单个检查功能内存在多个参数时,用:符号分离
        /// </summary>
        public const string CheckRunleArgsSplitFlag = ":";
        #endregion
    }

    class Group
    {
        
    }
}
