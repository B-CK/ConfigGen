using System;
using System.IO;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace ConfigGen.LocalInfo
{
    enum LocalInfoType
    {
        FileInfo,
        TypeInfo,
        FindInfo,
    }

    interface BaseInfo
    {
        void Init();
        void Add(object info);
        void Remove(object info);
    }

    class LocalInfoManager
    {
        public static LocalInfoManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LocalInfoManager();
                return _instance;
            }
        }
        private static LocalInfoManager _instance;
        private LocalInfoManager() { }

        /// <summary>
        /// 文件信息
        /// </summary>
        public FileInfo FileInfoLib { get; private set; }
        /// <summary>
        /// 数据类型信息
        /// </summary>
        public TypeInfo TypeInfoLib { get; private set; }
        /// <summary>
        /// 查询操作信息
        /// </summary>
        public FindInfo FindInfoLib { get; private set; }
        /// <summary>
        /// 差异表信息
        /// </summary>
        public List<TableInfo> DiffTableInfos { get; private set; }

        private const string _ext = "lfi";
        private bool _isInit = false;
        public void InitInfo(List<LocalInfoType> infos)
        {
            if (_isInit) return;
            _isInit = true;
            for (int i = 0; i < infos.Count; i++)
            {
                LocalInfoType type = infos[i];
                string path = GetInfoPath(type);
                if (!File.Exists(path)) return;
                switch (type)
                {
                    case LocalInfoType.FileInfo:
                        FileInfoLib = Util.Deserialize(path, typeof(FileInfo)) as FileInfo;
                        FileInfoLib.Init();
                        break;
                    case LocalInfoType.TypeInfo:
                        TypeInfoLib = Util.Deserialize(path, typeof(TypeInfo)) as TypeInfo;
                        TypeInfoLib.Init();
                        break;
                    case LocalInfoType.FindInfo:
                        FindInfoLib = Util.Deserialize(path, typeof(FindInfo)) as FindInfo;
                        FindInfoLib.Init();
                        break;
                    default:
                        break;
                }
            }
        }
        //public void RefreshInfo(List<LocalInfoType> types, List<object> infos)
        //{
        //    if (types.Count != infos.Count)
        //    {
        //        Util.LogError("刷新数据类型的种类比实际数据量多.");
        //        return;
        //    }
        //    for (int i = 0; i < types.Count; i++)
        //    {
        //        LocalInfoType type = types[i];
        //        object info = infos[i];
        //        if (info == null)
        //        {
        //            Util.LogWarningFormat("[{0}]刷新信息失败,数据为null!", type.ToString());
        //            return;
        //        }
        //        string path = string.Format(@"{0}/{1}.{2}", Values.ApplicationDir, type.ToString(), _ext);
        //        switch (type)
        //        {
        //            case LocalInfoType.FileInfo:
        //                if (FileInfoLib == null)
        //                    FileInfoLib = new FileInfo();
        //                Util.Serialize(path, FileInfoLib);
        //                break;
        //            case LocalInfoType.TypeInfo:
        //                break;
        //            case LocalInfoType.FindInfo:
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
        public void UpdateFileInfo()
        {
            List<string> diffRelPath = new List<string>();
            string[] files = Directory.GetFiles(Values.ConfigDir);
            var fileDict = FileInfoLib.FileDict;
            for (int i = 0; i < files.Length; i++)
            {
                string relPath = Util.GetConfigRelPath(files[i]);
                string md5 = Util.GetMD5HashFromFile(files[i]);
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
            Util.Serialize(GetInfoPath(LocalInfoType.FileInfo), FileInfoLib);

            for (int i = 0; i < diffRelPath.Count; i++)
            {
                string filePath = Util.GetConfigAbsPath(diffRelPath[i]);
                string error = null;
                Dictionary<SheetType, List<string>> dict;
                DataSet ds = Util.ReadXlsxFile(filePath, out dict, out error);
                if (!string.IsNullOrEmpty(error))
                {
                    Util.LogErrorFormat("[UpdateFileInfo]读取Xlsx失败!错误:{0}", error);
                    return;
                }
                foreach (var item in dict)
                {
                    for (int j = 0; j < item.Value.Count; j++)
                    {
                        DataTable dt = ds.Tables[item.Value[j]];
                        TableInfo tableInfo = null;
                        switch (item.Key)
                        {
                            case SheetType.Data:
                                tableInfo = new TableDataInfo(item.Key, Util.GetConfigRelPath(filePath), dt);
                                break;
                            case SheetType.Define:
                                tableInfo = new TableDefineInfo(item.Key, Util.GetConfigRelPath(filePath), dt);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        public void UpdateTypeInfo()
        {
            if (TypeInfoLib == null) return;

            //for (int i = 0; i < _diffFile.Count; i++)
            //{
            //    //string file = _diffFile[i];
            //    //命名空间:相对路径确定命名空间
            //    //类字段
            //    //  1.类型
            //    //  2.字段名
            //}
        }
        public void UpdateFindInfo()
        {
            if (FindInfoLib == null) return;


        }

        private string GetInfoPath(LocalInfoType type)
        {
            return string.Format(@"{0}/{1}.{2}", Values.ApplicationDir, type.ToString(), _ext);
        }
    }
}
