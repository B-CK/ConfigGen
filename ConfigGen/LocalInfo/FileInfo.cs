using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigGen.LocalInfo
{
    public class FileInfo : BaseInfo
    {
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

        public Dictionary<string, FileState> FileDict { get { return _fileDict; } }
        private Dictionary<string, FileState> _fileDict = new Dictionary<string, FileState>();

        public static FileInfo Create()
        {
            FileInfo fileInfo = new FileInfo();
            string path = Local.GetInfoPath(LocalInfoType.FileInfo);
            if (File.Exists(path))
            {
                string txt = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(txt))
                {
                    File.Delete(path);
                    fileInfo.Save();
                }
                fileInfo.UpdateList();
            }
            else
            {
                fileInfo.Save();
            }
            string content = File.ReadAllText(path);
            string[] lines = content.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] nodes = lines[i].Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (!File.Exists(nodes[0])) continue;
                  
                FileState state = new FileState();
                state.RelPath = nodes[0];
                state.MD5Hash = nodes[1];
                fileInfo._fileDict.Add(state.RelPath, state);
            }
            //fileInfo = Util.Deserialize(path, typeof(FileInfo)) as FileInfo;
            fileInfo.Init();
            return fileInfo;
        }
        private void Init()
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
            string path = Local.GetInfoPath(LocalInfoType.FileInfo);
            StringBuilder builder = new StringBuilder();
            foreach (var file in _fileDict)
                builder.AppendFormat("{0}|{1}\r\n", file.Key, file.Value.MD5Hash);
            Util.SaveFile(path, builder.ToString());
            //Util.Serialize(path, this);
        }
    }
    public class FileState
    {
        public string RelPath { get; set; }
        public string MD5Hash { get; set; }
    }
}
