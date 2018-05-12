﻿using System;
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
        /// key:数据表类型
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

            //填充基本信息
            for (int i = 0; i < _diffRelPath.Count; i++)
            {
                string filePath = Util.GetConfigAbsPath(_diffRelPath[i]);
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
                        Util.LogWarningFormat("{0}文件中{1}表定义异常", _diffRelPath[i], defines[j]);
                        continue;
                    }
                    if (defineInfo == null)
                        defineInfo = new TableDefineInfo(_diffRelPath[i], dt);
                    else
                        defineInfo.TableDataSet.Merge(dt);

                    if (!DefineInfoDict.ContainsKey(_diffRelPath[i]))
                        DefineInfoDict.Add(_diffRelPath[i], defineInfo);
                }
                List<string> datas = dict[SheetType.Data];
                TableDataInfo dataInfo = null;
                for (int j = 0; j < datas.Count; j++)
                {
                    DataTable dt = ds.Tables[datas[j]];
                    if (dt.Rows.Count < 4)
                    {
                        Util.LogWarningFormat("{0}文件中{1}表定义异常", _diffRelPath[i], datas[j]);
                        continue;
                    }
                    string name = defineInfo == null ? null : defineInfo.GetFirstName();
                    if (dataInfo == null)
                        dataInfo = new TableDataInfo(_diffRelPath[i], dt, name);
                    else
                    {
                        for (int k = Values.DataSheetDataStartIndex; k < dt.Rows.Count; k++)
                            dataInfo.TableDataSet.Rows.Add(dt.Rows[k]);
                    }
                }
                if (dataInfo != null && !DataInfoDict.ContainsKey(dataInfo.ClassName))
                    DataInfoDict.Add(dataInfo.ClassName, dataInfo);
            }
            //解析类定义
            foreach (var define in DefineInfoDict)
                define.Value.Analyze();
            //检查类定义
            foreach (var item in TypeInfoLib.TypeInfoDict)
            {
                BaseTypeInfo baseType = item.Value;
                TableChecker.CheckTypeByTypeType(item.Key, baseType.TypeType);
            }
            //解析数据定义和检查规则
            foreach (var data in DataInfoDict)
            {
                Util.Start();
                data.Value.Analyze();
                Util.Stop(string.Format("加载配置:{0}", data.Value.RelPath));
            }

            TypeInfoLib.Save();
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
