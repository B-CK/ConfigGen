using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ConfigGen.LocalInfo
{
    [XmlRoot("FileInfo")]
    public class FileInfo : BaseInfo
    {
        [XmlElement("FileState")]
        public List<FileState> FileStates { get; set; }
        private void UpdateList()
        {
            var ls = new List<FileState>();
            foreach (var f in _fileDict)
            {
                string path = Util.GetConfigAbsPath(f.Key);
                if (File.Exists(path))
                    ls.Add(f.Value);
            }
            FileStates = ls;
        }

        [XmlIgnore]
        public Dictionary<string, FileState> FileDict { get { return _fileDict; } }
        private Dictionary<string, FileState> _fileDict = new Dictionary<string, FileState>();
        public void Init()
        {
            for (int i = 0; i < FileStates.Count; i++)
            {
                string path = Util.GetConfigAbsPath(FileStates[i].RelPath);
                if (!File.Exists(path)) continue;
                if (!_fileDict.ContainsKey(FileStates[i].RelPath))
                    _fileDict.Add(FileStates[i].RelPath, FileStates[i]);
            }
        }
        public void Add(object info)
        {
            FileState fileState = info as FileState;
            if (!_fileDict.ContainsKey(fileState.RelPath))
                _fileDict.Add(fileState.RelPath, fileState);
        }
        public void Remove(object info)
        {
            FileState fileState = info as FileState;
            if (_fileDict.ContainsKey(fileState.RelPath))
                _fileDict.Remove(fileState.RelPath);
        }

        public void Save()
        {
            UpdateList();
            string path = LocalInfoManager.GetInfoPath(LocalInfoType.FileInfo);
            Util.Serialize(path, this);
        }
    }
    [XmlInclude(typeof(FileState))]
    public class FileState
    {
        [XmlAttribute]
        public string RelPath { get; set; }
        [XmlAttribute]
        public string MD5Hash { get; set; }
    }
}
