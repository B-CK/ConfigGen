using System;
using System.IO;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace ConfigGen.LocalInfo
{
    public enum LocalInfoType
    {
        FileInfo,
        TypeInfo,
        FindInfo,
    }

    interface BaseInfo
    {
        void Add(object info);
        void Remove(object info);
        void Save();
    }

    public class Local
    {
        public static Local Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Local();
                return _instance;
            }
        }
        private static Local _instance;
        private Local() { }

        /// <summary>
        /// 文件信息
        /// </summary>
        public FileInfo FileInfoLib { get; set; }
        /// <summary>
        /// 数据类型信息
        /// </summary>
        public TypeInfo TypeInfoLib { get; set; }
        /// <summary>
        /// 查询操作信息
        /// </summary>
        public FindInfo FindInfoLib { get; set; }

        /// <summary>
        /// key:表文件路径
        /// value:数据表信息
        /// </summary>
        public Dictionary<string, TableDataInfo> DataInfoDict { get; set; }
        /// <summary>
        /// key:表文件路径
        /// value:定义表信息
        /// </summary>
        public Dictionary<string, TableDefineInfo> DefineInfoDict { get; set; }

        private List<string> _diffRelPath = new List<string>();
        private const string _ext = "lfi";

        public void UpdateFileInfo()
        {
            List<string> diffRelPath = new List<string>();
            string[] files = Directory.GetFiles(Values.ConfigDir, "*.xls", SearchOption.AllDirectories);
            var fileDict = FileInfoLib.FileDict;
            for (int j = 0; j < files.Length; j++)
            {
                string relPath = Util.GetConfigRelPath(files[j]);
                string md5 = Util.GetMD5HashFromFile(files[j]);
                if (string.IsNullOrWhiteSpace(md5)) return;
                if (fileDict.ContainsKey(relPath))
                {
                    if (fileDict[relPath].MD5Hash != md5)
                    {
                        fileDict[relPath].MD5Hash = md5;
                        diffRelPath.Add(relPath);
                    }
                }
                else
                {
                    FileState fileState = new FileState();
                    fileState.RelPath = relPath;
                    fileState.MD5Hash = md5;
                    FileInfoLib.Add(fileState);
                    diffRelPath.Add(relPath);
                }
            }
            if (diffRelPath.Count > 0)
            {
                FileInfoLib.Save();
                for (int k = 0; k < diffRelPath.Count; k++)
                {
                    Util.LogFormat(">修改文件:{0}", diffRelPath[k]);
                }
            }

            Util.LogFormat("\r\n==>>修改文件数{0}个\n", diffRelPath.Count.ToString());
            if (Values.IsOptPart)
                _diffRelPath.AddRange(diffRelPath);
            else
                _diffRelPath.AddRange(FileInfoLib.FileDict.Keys);
            _diffRelPath = _diffRelPath.FindAll(f => Path.GetExtension(f).Contains(".xls"));
        }
        public void UpdateTypeInfo()
        {
            Dictionary<string, DataTable> _dataTableDict = new Dictionary<string, DataTable>();
            //填充基本信息,均为xlsx文件
            for (int i = 0; i < _diffRelPath.Count; i++)
            {
                string relPath = _diffRelPath[i];
                string filePath = Util.GetConfigAbsPath(relPath);
                string error = null;
                Dictionary<SheetType, List<string>> dict;
                DataSet ds = Util.ReadXlsxFile(filePath, out dict, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    Util.LogErrorFormat("读取Xlsx失败!{0}", error);
                    return;
                }

                List<string> defines = dict[SheetType.Define];
                TableDefineInfo defineInfo = null;
                for (int j = 0; j < defines.Count; j++)
                {
                    DataTable dt = ds.Tables[defines[j]];
                    if (dt.Rows.Count < 2)
                    {
                        Util.LogWarningFormat("{0}文件中{1}表定义异常", relPath, defines[j]);
                        continue;
                    }
                    if (defineInfo == null)
                        defineInfo = new TableDefineInfo(relPath, dt);
                    else
                        defineInfo.TableDataSet.Merge(dt);

                    if (!DefineInfoDict.ContainsKey(relPath))
                        DefineInfoDict.Add(relPath, defineInfo);
                }
                List<string> datas = dict[SheetType.Data];
                DataTable dataInfo = null;
                for (int j = 0; j < datas.Count; j++)
                {
                    DataTable dt = ds.Tables[datas[j]];
                    if (dt.Rows.Count < 4)
                    {
                        Util.LogWarningFormat("{0}文件中{1}表定义异常", relPath, datas[j]);
                        continue;
                    }

                    if (dataInfo == null)
                        dataInfo = dt;
                    else
                    {
                        for (int k = Values.DataSheetDataStartIndex; k < dt.Rows.Count; k++)
                            dataInfo.Rows.Add(dt.Rows[k]);
                    }
                }
                if (dataInfo != null && !_dataTableDict.ContainsKey(relPath))
                    _dataTableDict.Add(relPath, dataInfo);
            }

            try
            {
                //解析类定义                
                foreach (var define in DefineInfoDict)
                    define.Value.Analyze();
                //Util.Log("==>>解析定义完毕\n");
                //检查类定义并且修正集合类型中的泛型
                TypeInfoLib.CorrectClassInfo();
                TypeInfoLib.Save();
                //Util.Log("==>>类型修正完毕\n");

                //解析数据定义和检查规则
                foreach (var define in TypeInfoLib.ClassInfoDict)
                {
                    Util.Start();
                    ClassTypeInfo classType = define.Value;
                    if (string.IsNullOrWhiteSpace(classType.DataTable))
                    {
                        Util.PopTime();
                        continue;
                    }
                    string type = classType.GetClassName();
                    string excel = null, xml = null;
                    string combine = string.Format("{0}\\{1}", classType.NamespaceName, classType.DataTable);
                    if (_dataTableDict.ContainsKey(combine))
                        excel = combine;
                    else if (_dataTableDict.ContainsKey(classType.DataTable))
                        excel = classType.DataTable;
                    else
                    {
                        string absDir1 = string.Format("{0}{1}", Values.ConfigDir, combine);
                        string absDir2 = string.Format("{0}{1}", Values.ConfigDir, classType.DataTable);
                        if (Directory.Exists(absDir1))
                            xml = absDir1;
                        else if (Directory.Exists(absDir2))
                            xml = absDir2;
                    }

                    TableDataInfo data = null;
                    if (!string.IsNullOrWhiteSpace(excel))
                        data = new TableDataInfo(excel, _dataTableDict[excel], classType);
                    else if (!string.IsNullOrWhiteSpace(xml))
                        data = new TableLsonInfo(xml, classType);
                    else
                    {
                        Util.LogErrorFormat("{0}类型的数据文件不存在,错误位置:{1}",
                            type, classType.DataTable);
                        Util.PopTime();
                        continue;
                    }
                    data.Analyze();
                    if (!DataInfoDict.ContainsKey(type))
                        DataInfoDict.Add(type, data);
                    Util.Stop(string.Format("解析数据:{0}", classType.DataTable));
                }
            }
            catch (Exception e)
            {
                Util.LogErrorFormat("Excel,Xml解析异常.\n{0}\n{1}", e.Message, e.StackTrace);
            }
        }
        public void UpdateFindInfo()
        {
            if (FindInfoLib == null) return;


        }

        public static string GetInfoPath(LocalInfoType type)
        {
            return string.Format(@"{0}\{1}.{2}", Values.ApplicationDir, type.ToString(), _ext);
        }
    }
}
