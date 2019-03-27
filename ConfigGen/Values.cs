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
        /// 当前支持的可导出语言
        /// </summary>
        public static readonly HashSet<string> ExportLanguage = new HashSet<string>() { "c#", "java", "lua" };
        public static readonly HashSet<string> RawTypes = new HashSet<string>() { "bool", "int", "float", "long", "string" };
        public static readonly HashSet<string> ContainerTypes = new HashSet<string>() { "list", "dict" };
        /// <summary>
        /// 多参数分隔符,适用检查规则,分组
        /// </summary>
        public static readonly char[] ArgsSplitFlag = new char[1] { ':' };
        /// <summary>
        /// 字符串空
        /// </summary>
        public const string NULL_STR = "null";


        /// <summary>
        /// 数据表文件夹路径
        /// </summary>
        public static string ConfigDir { get; set; }
        /// <summary>
        /// 需要导出的数据和结构文件的描述文件,例cfg.xml
        /// </summary>
        public static string ConfigXml { get; set; }
        /// <summary>
        /// 导出CSharp类型
        /// </summary>
        public static string ExportCode { get; set; }
        /// <summary>
        /// 导出CSharp类型Xml操作类
        /// </summary>
        public static string ExportXmlCode { get; set; }

        /// <summary>
        /// 导出csv存储路径
        /// </summary>
        public static string ExportCsv { get; set; }
        /// <summary>
        /// 数据导出分组
        /// </summary>
        public static HashSet<string> ExportGroup { get; set; }
        /// <summary>
        /// 导出Csv数据读写类命名空间根节点
        /// </summary>
        public static string ConfigRootNode { get; set; }
        /// <summary>
        /// 导出Xml数据读写类命名空间根节点
        /// </summary>
        public static string XmlRootNode { get { return "Xml" + ConfigRootNode; } }
        public static bool OnlyCheck { get; set; }

        //--弃用

        /// <summary>
        /// 导出Lua类型类
        /// </summary>
        public static string ExportLua { get; set; }
        //--

        /// <summary>
        /// 存储运行时打印的所有信息，在程序运行完毕后输出为txt文件，从而解决如果输出内容过多控制台无法显示全部信息的问题
        /// </summary>
        public static StringBuilder LogContent = new StringBuilder();

        /// <summary>
        /// 数据表数据占位符,仅用于基础类型;不填写数据时,使用null占位.
        /// 默认值int,long,float="";string=""[无"null"字符串];bool=false;
        /// </summary>
        public const string Null = "null";
        /// <summary>
        /// 表格每行最大数据个数
        /// </summary>
        public const int SheetLineDataNum = 1024;
        /// <summary>
        /// 导出所有分组
        /// </summary>
        public const string DefualtGroup = "defualt";
        /// <summary>
        /// 工具所在目录
        /// </summary>
        public static readonly string ApplicationDir = Directory.GetCurrentDirectory();
        /// <summary>
        /// 数据文件扩展名
        /// </summary>
        public const string DataFileFlag = "data";
        /// <summary>
        /// 数据表格Sheet名标识
        /// </summary>
        public const string ExcelSheetDataFlag = "#";
        ///// <summary>
        ///// List元素默认分隔符,元素类型仅支持基础类型
        ///// </summary>
        //public const string SetSplitFlag = ",";
        /// <summary>
        /// 数据填写扩展文件后缀名
        /// </summary>
        public const string TableExtFileExt = ".xml";
        /// <summary>
        /// 数据表前三行用于描述,分别声明字段检查规则,字段描述,字段名,字段数据类型.
        /// </summary>
        //public const int DataSheetFieldIndex = 0;
        public const int DataSheetDataStartIndex = 3;
        /// <summary>
        /// 数据集合结束符,可用在数据/子字段上.
        /// </summary>
        public const string DataSetEndFlag = "]]";




        /// <summary>
        /// 多态类型属性标识符?????
        /// </summary>
        public const string PolymorphismFlag = "Type";

        #region 文件相关常量
        /// <summary>
        /// Csv数据存储分隔符
        /// </summary>
        public const string CsvSplitFlag = "▃";
        /// <summary>
        /// Csv数据文件扩展名
        /// </summary>
        public const string CsvFileExt = ".data";
        /// <summary>
        /// 列表元素命名
        /// </summary>
        public const string ELEMENT = "Element";
        /// <summary>
        /// 字典Key命名
        /// </summary>
        public const string KEY = "Key";
        /// <summary>
        /// 字典Value命名
        /// </summary>
        public const string VALUE = "Value";
        #endregion
    }
}
