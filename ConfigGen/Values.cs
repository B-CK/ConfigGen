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
        public static string ExportCode { get; set; }
        /// <summary>
        /// 导出CSharp类型Xml操作类
        /// </summary>
        public static string ExportXmlCode { get; set; }
        /// <summary>
        /// 导出Lua类型类
        /// </summary>
        public static string ExportLua { get; set; }
        /// <summary>
        /// 导出csv存储路径
        /// </summary>
        public static string ExportCsv { get; set; }
        /// <summary>
        /// 数据导出分组
        /// </summary>
        public static HashSet<string> ExportGroup { get; set; }
        /// <summary>
        /// 类结构导出文件路径
        /// </summary>
        public static string ExportFilter { get; set; }
        /// <summary>
        /// 是否只对已修改文件进行操作
        /// </summary>
        public static bool IsOptPart { get; set; }


        /// <summary>
        /// 存储运行时打印的所有信息，在程序运行完毕后输出为txt文件，从而解决如果输出内容过多控制台无法显示全部信息的问题
        /// </summary>
        public static StringBuilder LogContent = new StringBuilder();

        #region 常量
        /// <summary>
        /// 导出所有分组
        /// </summary>
        public const string DefualtGroup = "defualt";
        /// <summary>
        /// 工具所在目录
        /// </summary>
        public static readonly string ApplicationDir = Directory.GetCurrentDirectory();
        /// <summary>
        /// 数据表格Sheet名定义格式
        /// </summary>
        public const string DataFileFlag = "data";
        /// <summary>
        /// 数据类型表格Sheet名定义格式
        /// </summary>
        public const string ClassDesFileExt = ".xml";
        /// <summary>
        /// 数据填写扩展文件后缀名
        /// </summary>
        public const string TableExtFileExt = ".xsl";
        /// <summary>
        /// 数据表前四行分别声明字段检查规则,字段描述,字段名,字段数据类型.
        /// </summary>
        public const int DataSheetFieldIndex = 0;
        //public const int DataSheetCheckIndex = 1;
        //public const int DataSheetDesIndex = 2;
        //public const int DataSheetTypeIndex = 3;
        public const int DataSheetDataStartIndex = 3;
        /// <summary>
        /// 数据集合结束符,可用在数据/子字段上.
        /// </summary>
        public const string DataSetEndFlag = "##";
        /// <summary>
        /// 多参数分隔符,适用检查规则,分组
        /// </summary>
        public const string ItemSplitFlag = "|";
        /// <summary>
        /// 单个检查功能中存在多个参数时,用:符号分离;
        /// 集合元素类型分离
        /// </summary>
        public const string ArgsSplitFlag = ":";
        /// <summary>
        /// Csv数据存储分隔符
        /// </summary>
        public const string CsvSplitFlag = "▃";
        /// <summary>
        /// Csv数据文件扩展名
        /// </summary>
        public const string CsvFileExt = ".data";
        /// <summary>
        /// 多态类型属性标识符
        /// </summary>
        public const string PolymorphismFlag = "Type";
        /// <summary>
        /// 导出Lson数据读写类命名空间根节点
        /// </summary>
        public const string XmlRootNode = "XmlCode";
        /// <summary>
        /// 导出Csv数据读写类命名空间根节点
        /// </summary>
        public const string ConfigRootNode = "Csv";
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
