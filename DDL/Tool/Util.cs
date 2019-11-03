using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;

namespace Description
{
    public enum FileState
    {
        Inexist,     // 不存在
        IsOpen,      // 已被打开
        Available,   // 当前可用
    }

    public static class Util
    {
        #region Excel 文件操作
        [DllImport("kernel32.dll")]
        private static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);
        private const int OF_READWRITE = 2;
        private const int OF_SHARE_DENY_NONE = 0x40;
        private static readonly IntPtr HFILE_ERROR = new IntPtr(-1);
        private static readonly UTF8Encoding UTF8 = new UTF8Encoding(false);

        public static FileState GetFileState(string filePath)
        {
            if (File.Exists(filePath))
            {
                IntPtr vHandle = _lopen(filePath, OF_READWRITE | OF_SHARE_DENY_NONE);
                if (vHandle == HFILE_ERROR)
                    return FileState.IsOpen;

                CloseHandle(vHandle);
                return FileState.Available;
            }
            else
                return FileState.Inexist;
        }

        /// <summary>
        /// 将指定Excel文件的内容读取到DataTable中
        /// </summary>
        /// <param name="filePath">excel文件路径</param>
        /// <param name="sheetDict">sheet表类型及名字列表</param>
        /// <returns>excel中多个sheet数据集</returns>
        public static DataSet ReadXlsxFile(string filePath)
        {
            // 检查文件是否存在且没被打开
            FileState fileState = GetFileState(filePath);
            if (fileState == FileState.Inexist)
                new Exception(string.Format("{0}文件不存在", filePath));
            else if (fileState == FileState.IsOpen)
                new Exception(string.Format("{0}文件正在被其他软件打开，请关闭后重新运行本工具", filePath));

            OleDbConnection conn = null;
            OleDbDataAdapter da = null;
            DataSet ds = new DataSet();
            ds.DataSetName = filePath;

            try
            {
                string connectionString = "";
                string ext = Path.GetExtension(filePath);
                if (ext.Equals(".xls"))
                    connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\"";
                else
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=1\"";

                conn = new OleDbConnection(connectionString);
                conn.Open();

                // 获取数据源的表定义元数据                       
                DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                for (int i = 0; i < dtSheet.Rows.Count; ++i)
                {
                    string sheetName = dtSheet.Rows[i]["TABLE_NAME"].ToString();
                    da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand(String.Format("Select * FROM [{0}]", sheetName), conn);
                    da.Fill(ds, sheetName);
                    da.Dispose();
                }
            }
            catch (Exception e)
            {
                new Exception(string.Format("{0}\n错误：连接Excel失败，你可能尚未安装Office数据连接组件\n{1}", filePath, e.Message));
            }
            finally
            {
                // 关闭连接
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    // 由于C#运行机制，即便因为表格中没有Sheet名为data的工作簿而return null，也会继续执行finally，而此时da为空，故需要进行判断处理
                    if (da != null)
                        da.Dispose();
                    conn.Dispose();
                }
            }

            return ds;
        }
        #endregion


        /// <summary>
        /// Xml文件反序列化
        /// </summary>
        public static object Deserialize(string filePath, Type type)
        {
            object result = null;
            if (File.Exists(filePath))
            {
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    result = new XmlSerializer(type).Deserialize(streamReader);
                }
            }
            return result;
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
        /// <summary>
        /// 匹配组,有交集则匹配
        /// </summary>
        public static bool MatchGroups(HashSet<string> gs)
        {
            if (Setting.ExportGroup.Contains(Setting.DefualtGroup))
                return true;
            if (gs.Contains(Setting.DefualtGroup))
                return true;
            return Setting.ExportGroup.Overlaps(gs);
        }
        public static void Error(string fmt, params object[] msg)
        {
            new Exception(string.Format(fmt, msg));
        }
        public static void Log(object logString, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(logString);
        }
        public static void LogWarning(object warningString)
        {
            Log(warningString, ConsoleColor.Yellow);
        }
        public static void LogError(object errorString)
        {
            Log(errorString, ConsoleColor.Red);
        }
        public static void LogFormat(string format, params object[] logString)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(format, logString);
        }
        public static void LogWarningFormat(string format, params object[] warningString)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(format, warningString);
        }
        public static void LogErrorFormat(string format, params object[] errorString)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(format, errorString);
        }
        public static string GetRelPath(string path)
        {
            return path.Replace(Setting.ExcelDir, "");
        }
        public static string GetAbsPath(string relPath)
        {
            return Path.Combine(Setting.ExcelDir, relPath);
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
        /// 符合程序化命名规则,除首字母非"_"
        /// </summary>
        public static bool MatchIdentifier(string name)
        {
            return Regex.IsMatch(name, @"^[a-zA-Z_]\w*$");
        }
        public static string[] Split(this string type)
        {
            return type.IsEmpty() ? new string[0] : type.Split(Setting.ArgsSplitFlag);
        }

        /// <summary>
        /// 各种空
        /// </summary>
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
        public static string ToLowerExt(this string self)
        {
            if (self != null)
                return self.ToLower();
            return null;
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

            File.WriteAllText(filePath, content, UTF8);
        }
        public static string FirstCharUpper(this string name)
        {
            return Char.ToUpper(name[0]) + name.Substring(1);
        }
        public static string ToString(object[] array, string split = ",")
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
                sb.AppendFormat("{0}{1}", array[i], i != array.Length - 1 ? split : "");
            return sb.ToString();
        }
        public static string ToString(List<string> list, string split = ",")
        {
            return ToString(list.ToArray(), split);
        }

        public static string GetRelativePath(string filePath, string folder)
        {
            if (!File.Exists(filePath) || !Directory.Exists(folder))
                return null;

            const string directorySeparatorChar = "\\";
            Uri pathUri = new Uri(filePath);

            if (!folder.EndsWith(directorySeparatorChar))
            {
                folder += directorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace("/", directorySeparatorChar));
        }
    }
}
