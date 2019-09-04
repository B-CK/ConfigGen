using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Windows.Forms;
using Description.Properties;
using System.Drawing;

namespace Description
{
    public static class Util
    {
        #region 基础配置
        /// <summary>
        /// 工具所在目录
        /// </summary>
        public static readonly string ApplicationDir = Directory.GetCurrentDirectory();
        public const string DefaultModuleName = "Default";
        /// <summary>
        /// 空命名空间符号
        /// </summary>
        public const string EmptyNamespace = "@";
        public static string DefaultModule
        {
            get
            {
                if (_defaultModule.IsEmpty())
                    _defaultModule = Path.GetFullPath(GetModuleAbsPath(DefaultModuleName + ".xml"));
                return _defaultModule;
            }
        }
        /// <summary>
        /// ModuleXml描述文件目录
        /// </summary>
        public static string ModuleDir
        {
            get
            {
                if (_moduleDir.IsEmpty())
                    _moduleDir = Path.Combine(ApplicationDir, "Module");
                return _moduleDir;
            }
            set
            {
                _moduleDir = Path.GetFullPath(Path.Combine(ApplicationDir, value));
            }
        }
        /// <summary>
        /// ModuleXml描述文件目录
        /// </summary>
        public static string NamespaceDir
        {
            get
            {
                if (_namespaceDir.IsEmpty())
                    _namespaceDir = Path.Combine(ApplicationDir, "Namespace");
                return _namespaceDir;
            }
            set
            {
                _namespaceDir = Path.GetFullPath(Path.Combine(ApplicationDir, value));
            }
        }
        /// <summary>
        /// 最近一次打开记录
        /// </summary>
        public static string LastRecord
        {
            get
            {
                return _lastRecord;
            }
            set
            {
                _lastRecord = value;
                Settings.Default.LastModule = value;
            }
        }
        /// <summary>
        /// Excel数据目录
        /// </summary>
        public static string DataDir { get { return Format("{0}.{1}", ApplicationDir, _dataDir); } }
        public static string LogErrorFile { get { return Path.Combine(ApplicationDir, "error.log"); } }

        static string _defaultModule;
        static string _moduleDir;
        static string _namespaceDir;
        static string _lastRecord;
        static string _dataDir;
        #endregion

        public static Color NormalField = Color.LightGray;
        public static Color ErrorField = Color.Red;


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
        public static readonly char[] ArgsSplitFlag = new char[] { ':' };
        /// <summary>
        /// 类型名称分隔符
        /// </summary>
        public static readonly char[] DotSplit = new char[] { '.' };

        private static readonly char[] PathSplit = new char[] { '\\' };
        private static readonly StringBuilder Builder = new StringBuilder();
        public static string Format(string fmt, params object[] args)
        {
            Builder.Clear();
            return Builder.AppendFormat(fmt, args).ToString();
        }


        /// <summary>
        /// Xml文件反序列化
        /// </summary>
        public static T Deserialize<T>(string filePath) where T : class
        {
            object result = null;
            if (File.Exists(filePath))
            {
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    result = new XmlSerializer(typeof(T)).Deserialize(streamReader);
                }
            }
            return result as T;
        }
        /// <summary>
        /// Xml文件序列化
        /// </summary>
        public static void Serialize(string filePath, object source)
        {
            if (!string.IsNullOrEmpty(filePath) && filePath.Trim().Length != 0 && source != null)
            {
                File.Delete(filePath);
                Type type = source.GetType();
                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(type);
                    xmlSerializer.Serialize(streamWriter, source);
                }
            }
        }
        public static void Serialize(StringBuilder builder, object source)
        {
            if (builder != null && source != null)
            {
                Type type = source.GetType();
                using (XmlWriter writer = XmlWriter.Create(builder))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(type);
                    xmlSerializer.Serialize(writer, source);
                }
            }
        }
        public static void MsgWarning(string title, string fmt, params object[] msg)
        {
            string warning = Format(fmt, msg);
            MessageBox.Show(warning, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            ConsoleDock.Ins.LogWarning(warning);
        }
        public static void MsgError(string title, string fmt, params object[] msg)
        {
            string error = Format(fmt, msg);
            MessageBox.Show(error, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            ConsoleDock.Ins.LogError(error);
        }
        public static string GetModuleRelPath(string path)
        {
            return path.Replace(ModuleDir, "");
        }
        public static string GetModuleAbsPath(string relPath)
        {
            return Path.Combine(ModuleDir, relPath);
        }
        public static string GetNamespaceRelPath(string path)
        {
            return path.Replace(NamespaceDir, "");
        }
        public static string GetNamespaceAbsPath(string relPath)
        {
            return Path.Combine(NamespaceDir, relPath);
        }
        public static void TryDeleteDirectory(string path)
        {
            if (!Directory.Exists(path)) return;

            var fs = Directory.GetFiles(path, "*.*");
            for (int i = 0; i < fs.Length; i++)
                File.Delete(fs[i]);
            var ds = Directory.GetDirectories(path, "*");
            for (int i = 0; i < ds.Length; i++)
                TryDeleteDirectory(ds[i]);

            Directory.Delete(path, true);
        }

        #region 扩展及操作
        /// <summary>
        /// 匹配标识符命名规则
        /// </summary>
        public static bool MatchIdentifier(string name)
        {
            return Regex.IsMatch(name, @"^[a-zA-Z_]\w*$");
        }
        /// <summary>
        /// 各种空
        /// </summary>
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
        public static void Clear(this StringBuilder builder)
        {
            builder.Remove(0, builder.Length);
        }
        #endregion

        /// <summary>
        /// 创建文本
        /// </summary>
        public static void SaveFile(string filePath, string content)
        {
            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            File.WriteAllText(filePath, content, Encoding.UTF8);
        }
        public static string FirstCharUpper(this string name)
        {
            return Char.ToUpper(name[0]) + name.Substring(1);
        }
        public static string List2String(object[] array, string split = ",")
        {
            return string.Join(split, array);
        }
        public static string List2String(List<string> list, string split = ",")
        {
            return List2String(list.ToArray(), split);
        }
    }
}
