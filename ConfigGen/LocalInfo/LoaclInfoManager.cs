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
        void Save();
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
        /// key:表文件路径
        /// value:数据表信息
        /// </summary>
        public Dictionary<string, TableDataInfo> DataInfoDict { get; private set; }
        /// <summary>
        /// key:表文件路径
        /// value:定义表信息
        /// </summary>
        public Dictionary<string, TableDefineInfo> DefineInfoDict { get; private set; }

        private List<LocalInfoType> _infoTypes = new List<LocalInfoType>();
        private List<string> _diffRelPath = new List<string>();
        private const string _ext = "lfi";
        private bool _isInit = false;
        public void Init(List<LocalInfoType> infos)
        {
            if (_isInit) return;
            _isInit = true;
            _infoTypes = infos;
            for (int i = 0; i < infos.Count; i++)
            {
                LocalInfoType type = infos[i];
                string path = GetInfoPath(type);
                switch (type)
                {
                    case LocalInfoType.FileInfo:
                        {
                            if (File.Exists(path))
                            {
                                string txt = File.ReadAllText(path);
                                if (string.IsNullOrWhiteSpace(txt))
                                    File.Delete(path);
                            }
                            else
                            {
                                FileInfoLib = new FileInfo();
                                FileInfoLib.Save();
                            }
                            FileInfoLib = Util.Deserialize(path, typeof(FileInfo)) as FileInfo;
                            FileInfoLib.Init();
                            break;
                        }
                    case LocalInfoType.TypeInfo:
                        {
                            if (File.Exists(path))
                            {
                                string txt = File.ReadAllText(path);
                                if (string.IsNullOrWhiteSpace(txt))
                                    File.Delete(path);
                            }
                            else
                            {
                                TypeInfoLib = new TypeInfo();
                                TypeInfoLib.Save();
                            }
                            TypeInfoLib = Util.Deserialize(path, typeof(TypeInfo)) as TypeInfo;
                            TypeInfoLib.Init();
                            DefineInfoDict = new Dictionary<string, TableDefineInfo>();
                            DataInfoDict = new Dictionary<string, TableDataInfo>();
                            break;
                        }
                    case LocalInfoType.FindInfo:
                        {
                            if (File.Exists(path))
                            {
                                string txt = File.ReadAllText(path);
                                if (string.IsNullOrWhiteSpace(txt))
                                    File.Delete(path);
                            }
                            else
                            {
                                FindInfoLib = new FindInfo();
                                FindInfoLib.Save();
                            }
                            FindInfoLib = Util.Deserialize(path, typeof(FindInfo)) as FindInfo;
                            FindInfoLib.Init();
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        public void Update()
        {
            for (int i = 0; i < _infoTypes.Count; i++)
            {
                LocalInfoType type = _infoTypes[i];
                switch (type)
                {
                    case LocalInfoType.FileInfo:
                        List<string> diffRelPath = new List<string>();
                        string[] files = Directory.GetFiles(Values.ConfigDir, "*", SearchOption.AllDirectories);
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
                        break;
                    case LocalInfoType.TypeInfo:
                        UpdateTypeInfo();
                        break;
                    case LocalInfoType.FindInfo:
                        UpdateFindInfo();
                        break;
                    default:
                        break;
                }
            }
        }
        private void UpdateTypeInfo()
        {
            if (TypeInfoLib == null) return;

            Util.Start();
            Dictionary<string, DataTable> _dataTableDict = new Dictionary<string, DataTable>();
            //填充基本信息
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
                Util.Log("==>>解析定义");
                foreach (var define in DefineInfoDict)
                    define.Value.Analyze();
                Util.Log("");
                //检查类定义并且修正集合类型中的泛型
                Util.Log("==>>检查类定义并且修正集合类型中的泛型");
                var infoDict = new Dictionary<string, BaseTypeInfo>(TypeInfoLib.TypeInfoDict);
                foreach (var item in infoDict)
                {
                    BaseTypeInfo typeInfo = item.Value;
                    if (typeInfo.TypeType == TypeType.Class
                        || typeInfo.TypeType == TypeType.Enum)
                    {
                        string error = TableChecker.CheckType(item.Key);
                        if (!string.IsNullOrWhiteSpace(error))
                        {
                            Util.LogErrorFormat("{0}类型不存在,错误位置{1}", item.Key, item.Value.RelPath);
                            continue;
                        }

                        ClassTypeInfo classType = typeInfo as ClassTypeInfo;
                        for (int i = 0; classType != null && i < classType.Fields.Count; i++)
                        {
                            FieldInfo fieldInfo = classType.Fields[i];
                            TypeType typeType = TypeInfo.GetTypeType(fieldInfo.Type);
                            if (typeType == TypeType.None)
                            {
                                string combine = Util.Combine(classType.NamespaceName, fieldInfo.Type);
                                typeType = TypeInfo.GetTypeType(combine);
                            }
                            BaseTypeInfo baseType = null;
                            switch (typeType)
                            {
                                case TypeType.Class:
                                case TypeType.Enum:
                                    fieldInfo.Type = CorrectType(fieldInfo.Type, classType, fieldInfo.Name);
                                    continue;
                                case TypeType.List:
                                    ListTypeInfo listType = new ListTypeInfo();
                                    listType.TypeType = typeType;
                                    string type = fieldInfo.Type.Replace("list<", "").Replace(">", "");
                                    listType.ItemType = CorrectType(type, classType, fieldInfo.Name);
                                    fieldInfo.Type = string.Format("list<{0}>", listType.ItemType);
                                    listType.Name = fieldInfo.Type;
                                    baseType = listType;
                                    break;
                                case TypeType.Dict:
                                    DictTypeInfo dictType = new DictTypeInfo();
                                    dictType.TypeType = typeType;
                                    string[] kv = fieldInfo.Type.Replace("dict<", "").Replace(">", "").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    if (kv.Length != 2)
                                    {
                                        Util.LogErrorFormat("{0}类中字段{1}的dict类型格式错误,错误位置{2}",
                                            classType.GetClassName(), fieldInfo.Name, classType.RelPath);
                                        continue;
                                    }
                                    string key = CorrectType(kv[0], classType, fieldInfo.Name);
                                    error = TableChecker.CheckDictKey(key);
                                    if (!string.IsNullOrWhiteSpace(error))
                                    {
                                        Util.LogErrorFormat("{0}类中字段{1}定义dict key类型错误,错误位置{2}",
                                            classType.GetClassName(), fieldInfo.Name, classType.RelPath);
                                        continue;
                                    }
                                    dictType.KeyType = key;
                                    dictType.ValueType = CorrectType(kv[1], classType, fieldInfo.Name);
                                    fieldInfo.Type = string.Format("dict<{0}, {1}>", dictType.KeyType, dictType.ValueType);
                                    dictType.Name = fieldInfo.Type;
                                    baseType = dictType;
                                    break;
                                default:
                                    continue;
                            }
                            TypeInfoLib.Add(baseType);
                        }
                    }
                }
                TypeInfoLib.TypeInfoDict = infoDict;
                Util.Log("");
            }
            catch (Exception e)
            {
                Util.LogError(e.StackTrace);
            }

            //解析数据定义和检查规则
            Util.Log("==>>解析数据定义和检查规则");
            foreach (var define in TypeInfoLib.ClassInfoDict)
            {
                Util.Start();
                ClassTypeInfo classType = define.Value;
                if (string.IsNullOrWhiteSpace(classType.DataTable))
                    continue;
                string type = classType.GetClassName();
                string dataTablePath = null;
                string combine = string.Format("{0}\\{1}", classType.NamespaceName, classType.DataTable);
                if (!_dataTableDict.ContainsKey(combine))
                {
                    if (!_dataTableDict.ContainsKey(classType.DataTable))
                    {
                        Util.LogWarningFormat("{0}类型的数据表表不存在,错误位置:{1}",
                            type, classType.DataTable);
                        continue;
                    }
                    else
                        dataTablePath = classType.DataTable;
                }
                else
                    dataTablePath = combine;
                TableDataInfo data = new TableDataInfo(classType.DataTable, _dataTableDict[dataTablePath], classType);
                data.Analyze();
                if (!DataInfoDict.ContainsKey(type))
                    DataInfoDict.Add(type, data);
                Util.Stop(string.Format("解析数据:{0}", data.RelPath));
            }

            TypeInfoLib.Save();
            Util.Stop("==>>更新类型/数据信息完毕");
        }
        private string CorrectType(string type, BaseTypeInfo baseType, string name)
        {
            string newType = null;
            string combine = Util.Combine(baseType.NamespaceName, type);
            string error = TableChecker.CheckType(combine);
            if (!string.IsNullOrWhiteSpace(error))
            {
                error = TableChecker.CheckType(type);
                if (!string.IsNullOrWhiteSpace(error))
                    newType = null;
                else
                    newType = type;
            }
            else
                newType = combine;

            if (newType != null)
            {
                var newInfo = TypeInfo.GetTypeInfo(newType);
                var info = TypeInfo.GetTypeInfo(type);
                TypeInfoLib.Remove(info);
                info = TypeInfo.GetTypeInfo(combine);
                TypeInfoLib.Remove(info);
                TypeInfoLib.Add(newInfo);
            }
            else
            {
                Util.LogErrorFormat("{0}类字段{1}类型{2}不存在,错误位置{3}",
                   baseType.GetClassName(), name, type, baseType.RelPath);
                throw new Exception();
            }
            return newType;
        }
        private void UpdateFindInfo()
        {
            if (FindInfoLib == null) return;


        }

        public static string GetInfoPath(LocalInfoType type)
        {
            return string.Format(@"{0}\{1}.{2}", Values.ApplicationDir, type.ToString(), _ext);
        }
    }
}
