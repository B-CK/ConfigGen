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

namespace ConfigGen
{
    public enum FileState
    {
        Inexist,     // 不存在
        IsOpen,      // 已被打开
        Available,   // 当前可用
    }

    public static class Util
    {
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
        public static DataSet ReadXlsxFile(string filePath, out string errorString)
        {
            // 检查文件是否存在且没被打开
            FileState fileState = GetFileState(filePath);
            if (fileState == FileState.Inexist)
            {
                errorString = string.Format("{0}文件不存在", filePath);
                return null;
            }
            else if (fileState == FileState.IsOpen)
            {
                errorString = string.Format("{0}文件正在被其他软件打开，请关闭后重新运行本工具", filePath);
                return null;
            }

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
                    //var index = sheetName.IndexOf(Values.ExcelSheetDataFlag);
                    if (sheetName.IndexOf(Values.ExcelSheetDataFlag) != 1) continue;

                    da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand(String.Format("Select * FROM [{0}]", sheetName), conn);
                    da.Fill(ds, sheetName);
                    da.Dispose();
                }
            }
            catch (Exception e)
            {
                errorString = string.Format("{0}\n错误：连接Excel失败，你可能尚未安装Office数据连接组件\n{1}", filePath, e.Message);
                return null;
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

            errorString = null;
            return ds;
        }
        /// <summary>
        /// 将Excel中的列编号转为列名称（第1列为A，第28列为AB）
        /// </summary>
        public static string GetColumnName(int columnNumber)
        {
            string result = string.Empty;
            int temp = columnNumber;
            int quotient;
            int remainder;
            do
            {
                quotient = temp / 26;
                remainder = temp % 26;
                if (remainder == 0)
                {
                    remainder = 26;
                    --quotient;
                }

                result = (char)(remainder - 1 + 'A') + result;
                temp = quotient;
            }
            while (quotient > 0);

            return result;
        }
        public static string GetErrorSite(string relPath, int c, int r)
        {
            return string.Format("错误位置:{0}[{1}{2}]", relPath, GetColumnName(c), r);
        }
        /// <summary>
        /// Xml反序列化
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
        /// Xml序列化
        /// </summary>
        public static void Serialize(string filePath, object sourceObj)
        {
            if (!string.IsNullOrEmpty(filePath) && filePath.Trim().Length != 0 && sourceObj != null)
            {
                File.Delete(filePath);
                Type type = sourceObj.GetType();
                using (StreamWriter streamWriter = new StreamWriter(filePath))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(type);
                    xmlSerializer.Serialize(streamWriter, sourceObj);
                }
            }
        }
        /// <summary>
        /// 获取文件MD5
        /// </summary>
        public static string GetMD5HashFromFile(string filePath)
        {
            try
            {
                FileState fileState = GetFileState(filePath);
                if (fileState == FileState.Inexist)
                {
                    LogErrorFormat("{0}文件不存在", filePath);
                    return null;
                }
                else if (fileState == FileState.IsOpen)
                {
                    LogErrorFormat("{0}文件正在被其他软件打开，请关闭后重新运行本工具", filePath);
                    return null;
                }

                FileStream file = new FileStream(filePath, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception)
            {
                LogErrorFormat("文件{0} MD5生成失败.", filePath);
                return null;
            }
        }
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
        public static string GetConfigRelPath(string path)
        {
            return path.Replace(Values.ConfigDir, "");
        }
        public static string GetConfigAbsPath(string relPath)
        {
            return string.Format(@"{0}{1}", Values.ConfigDir, relPath);
        }
        public static string Combine(string nameSpace, string name)
        {
            return string.Format("{0}.{1}", nameSpace, name);
        }
        /// <summary>
        /// 获取完整命名空间名,不包含类名
        /// </summary>
        public static string GetFullNamespace(string root, string _namespace)
        {
            if (string.IsNullOrWhiteSpace(_namespace))
                return root;
            else
                return string.Format("{0}.{1}", root, _namespace);
        }
        public static string FirstCharUpper(string name)
        {
            return Char.ToUpper(name[0]) + name.Substring(1);
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



        public static string ListStringSplit(object[] array, string split = ",")
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
                sb.AppendFormat("{0}{1}", array[i], i != array.Length - 1 ? split : "");
            return sb.ToString();
        }
        public static string ListStringSplit(List<string> list, string split = ",")
        {
            return ListStringSplit(list.ToArray(), split);
        }

        public static void Log(object logString, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(logString);
            Values.LogContent.AppendLine(logString.ToString());
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
            Values.LogContent.AppendLine(ListStringSplit(logString, "\r\n"));
        }
        public static void LogWarningFormat(string format, params object[] warningString)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(format, warningString);
            Values.LogContent.AppendLine(ListStringSplit(warningString, "\r\n"));
        }
        public static void LogErrorFormat(string format, params object[] errorString)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(format, errorString);
            Values.LogContent.AppendLine(ListStringSplit(errorString, "\r\n"));
        }

        static Stopwatch _sw = new Stopwatch();
        static Stack<long> _timeStack = new Stack<long>();
        public static void Start()
        {
            if (!_sw.IsRunning)
            {
                _sw.Reset();
                _sw.Start();
            }

            _timeStack.Push(_sw.ElapsedMilliseconds);
        }
        public static void Stop(string msg)
        {
            if (_timeStack.Count <= 0) return;

            long ms = _sw.ElapsedMilliseconds - _timeStack.Pop();
            string seconds = (ms / 1000f).ToString();
            LogFormat("{0} 耗时 {1:N3}s\n", msg, seconds);
        }



        #region 字符串扩展及操作
        /// <summary>
        /// 符合程序化命名规则,除首字母非"_"
        /// </summary>
        public static bool MatchName(string name)
        {
            return Regex.IsMatch(name, @"[a-zA-Z]\w");
        }
        public static string[] Split(this string type)
        {
            return type.IsEmpty() ? new string[0] : type.Split(Values.ArgsSplitFlag);
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

    }
}
