using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigGen
{
    public class Consts
    {
        #region 基础配置
        /// <summary>
        /// 工具所在目录
        /// </summary>
        public static readonly string ApplicationDir = Directory.GetCurrentDirectory();

        /// <summary>
        /// 数据表数据占位符,仅用于基础类型;不填写数据时,使用null占位.
        /// 默认值int,long,float="";string=""[无"null"字符串];bool=false;
        /// </summary>
        public const string Null = "null";
        public const string BOOL = "bool";
        public const string INT = "int";
        public const string FLOAT = "float";
        public const string LONG = "long";
        public const string STRING = "string";
        public const string LIST = "list";
        public const string DICT = "dict";

        public static readonly HashSet<string> RawTypes = new HashSet<string>() { BOOL, INT, FLOAT, LONG, STRING };
        public static readonly HashSet<string> ContainerTypes = new HashSet<string>() { LIST, DICT };
        /// <summary>
        /// 多参数分隔符,适用检查规则,分组,键值对
        /// </summary>
        public static readonly char[] ArgsSplitFlag = new char[1] { ':' };
        /// <summary>
        /// 集合元素分隔符
        /// </summary>
        public static readonly char[] SplitFlag = new char[1] { ',' };
        /// <summary>
        /// 导出所有分组
        /// </summary>
        public const string DefualtGroup = "defualt";
        #endregion

        #region 命令配置
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
        public static string CSDir { get; set; }
        /// <summary>
        /// 导出Java类型
        /// </summary>
        public static string JavaDir { get; set; }
        /// <summary>
        /// 导出Lua类型
        /// </summary>
        public static string LuaDir { get; set; }
        /// <summary>
        /// 导出CSharp类型Xml操作类
        /// </summary>
        public static string XmlCodeDir { get; set; }

        /// <summary>
        /// 导出csv存储路径
        /// </summary>
        public static string DataDir { get; set; }
        /// <summary>
        /// 数据导出分组
        /// </summary>
        public static HashSet<string> ExportGroup = new HashSet<string>();
        /// <summary>
        /// 导出Csv数据读写类命名空间根节点
        /// </summary>
        public static string ConfigRootNode { get; set; }
        /// <summary>
        /// 导出Xml数据读写类命名空间根节点
        /// </summary>
        public static string XmlRootNode { get { return "Xml"; } }
        public static bool Check { get; set; }
        #endregion

        #region 文件相关常量
        /// <summary>
        /// Csv数据存储分隔符
        /// </summary>
        public const string CsvSplitFlag = "\n";
        /// <summary>
        /// Csv数据文件扩展名
        /// </summary>
        public const string CsvFileExt = ".data";
        /// <summary>
        /// Excel数据行结束符
        /// </summary>
        public const string RowEndFlag = "##";
        /// <summary>
        /// 数据集合结束符,可用在数据/子字段上.
        /// </summary>
        public const string SetEndFlag = "]]";
        /// <summary>
        /// 列表元素命名
        /// </summary>
        public const string ITEM = "Item";
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
