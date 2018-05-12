using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.OleDb;
using ConfigGen.LocalInfo;
using System.Diagnostics;
using System.Timers;


namespace ConfigGen
{
    public enum FileState
    {
        Inexist,     // 不存在
        IsOpen,      // 已被打开
        Available,   // 当前可用
    }

    class Util
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);
        private const int OF_READWRITE = 2;
        private const int OF_SHARE_DENY_NONE = 0x40;
        private static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

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
        public static DataSet ReadXlsxFile(string filePath, out Dictionary<SheetType, List<string>> sheetDict, out string errorString)
        {
            // 检查文件是否存在且没被打开
            FileState fileState = GetFileState(filePath);
            if (fileState == FileState.Inexist)
            {
                sheetDict = null;
                errorString = string.Format("{0}文件不存在", filePath);
                return null;
            }
            else if (fileState == FileState.IsOpen)
            {
                sheetDict = null;
                errorString = string.Format("{0}文件正在被其他软件打开，请关闭后重新运行本工具", filePath);
                return null;
            }

            OleDbConnection conn = null;
            OleDbDataAdapter da = null;
            DataSet ds = new DataSet();
            ds.DataSetName = filePath;
            sheetDict = new Dictionary<SheetType, List<string>>();
            sheetDict.Add(SheetType.Data, new List<string>());
            sheetDict.Add(SheetType.Define, new List<string>());

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
                    if (!sheetName.StartsWith(Values.DataSheetPrefix)
                        && !sheetName.StartsWith(Values.DefineSheetPrefix)) continue;

                    if (sheetName.StartsWith(Values.DataSheetPrefix))
                        sheetDict[SheetType.Data].Add(sheetName);
                    else if (sheetName.StartsWith(Values.DefineSheetPrefix))
                        sheetDict[SheetType.Define].Add(sheetName);

                    da = new OleDbDataAdapter();
                    da.SelectCommand = new OleDbCommand(String.Format("Select * FROM [{0}]", sheetName), conn);
                    da.Fill(ds, sheetName);
                    da.Dispose();
                }
            }
            catch (Exception e)
            {
                errorString = "错误：连接Excel失败，你可能尚未安装Office数据连接组件\n" + e.Message;
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
                    new XmlSerializer(type);
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
            catch (Exception ex)
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

            File.WriteAllText(filePath, content, Encoding.UTF8);
        }
        public static string GetConfigRelPath(string path)
        {
            return path.Replace(Values.ConfigDir, "");
        }
        public static string GetConfigAbsPath(string relPath)
        {
            return string.Format(@"{0}{1}", Values.ConfigDir, relPath);
        }
        public static string Combine(string nameSpace, string className)
        {
            return string.Format("{0}.{1}", nameSpace, className);
        }

        public static string ListStringSplit(string[] array, string split = ",")
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

        public static void Log(string logString, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(logString);
            Values.LogContent.AppendLine(logString);
        }
        public static void LogWarning(string warningString)
        {
            Log(warningString, ConsoleColor.Yellow);
        }
        public static void LogError(string errorString)
        {
            Log(errorString, ConsoleColor.Red);
        }
        public static void LogFormat(string format, params string[] logString)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(format, logString);
            Values.LogContent.AppendLine(ListStringSplit(logString, "\r\n"));
        }
        public static void LogWarningFormat(string format, params string[] warningString)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(format, warningString);
            Values.LogContent.AppendLine(ListStringSplit(warningString, "\r\n"));
        }
        public static void LogErrorFormat(string format, params string[] errorString)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(format, errorString);
            Values.LogContent.AppendLine(ListStringSplit(errorString, "\r\n"));
        }

        static Stopwatch _sw = new Stopwatch();

        public static void Start()
        {
            _sw.Reset();
            _sw.Start();
        }
        public static void Stop(string msg)
        {
            string seconds = (_sw.ElapsedMilliseconds / 1000).ToString();
            Util.LogFormat("{0} 耗时 {1}s", msg, seconds);
        }

    }
}
