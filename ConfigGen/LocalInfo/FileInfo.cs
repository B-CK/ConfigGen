using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ConfigGen.LocalInfo
{
    [XmlRoot("FileInfo")]
    class FileInfo : BaseInfo
    {
        [XmlIgnore]
        private List<FileState> _fileStates;
        public List<FileState> FileStates
        {
            get
            {
                if (_fileStates == null)
                    _fileStates = new List<FileState>();
                UpdateList();
                return _fileStates;
            }
            set
            {
                _fileStates = value;
                UpdateList();
            }
        }
        private void UpdateList()
        {
            var ls = new List<FileState>();
            foreach (var f in _fileStates)
            {
                string path = Util.GetConfigAbsPath(f.RelPath);
                if (File.Exists(path))
                    ls.Add(f);
            }
            _fileStates = ls;
        }

        [XmlIgnore]
        public Dictionary<string, FileState> FileDict { get { return _fileDict; } }
        private Dictionary<string, FileState> _fileDict = new Dictionary<string, FileState>();
        public void Init()
        {
            if (FileStates == null || FileStates.Count == 0)
            {
                FileStates = new List<FileState>();
                return;
            }

            for (int i = 0; i < FileStates.Count; i++)
            {
                string relPath = FileStates[i].RelPath;
                if (!File.Exists(relPath)) continue;
                if (!_fileDict.ContainsKey(relPath))
                    _fileDict.Add(relPath, FileStates[i]);
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
    }
    [XmlInclude(typeof(FileState))]
    class FileState
    {
        public string RelPath { get; set; }
        public string MD5Hash { get; set; }
    }
}
