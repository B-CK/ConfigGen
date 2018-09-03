using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigGen.LocalInfo
{
    public class FileStateInfo
    {
         const string LIB_NAME = "FileStateInfo.lfi";

        public List<FileState> FileStates { get; set; }
        public List<string> DiffRelPath { get; private set; }
        public Dictionary<string, FileState> FileDict { get; private set; }

        public static void Init()
        {
            FileStateInfo fileInfo = new FileStateInfo();
            fileInfo.FileDict = new Dictionary<string, FileState>();
            fileInfo.DiffRelPath = new List<string>();
            string path = string.Format(@"{0}\{1}", Values.ApplicationDir, LIB_NAME);
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
                fileInfo.FileDict.Add(state.RelPath, state);
            }
            fileInfo.DoFileState();
        }

        private void Add(object info)
        {
            FileState fileState = info as FileState;
            if (!FileDict.ContainsKey(fileState.RelPath))
                FileDict.Add(fileState.RelPath, fileState);
        }
        private void Remove(object info)
        {
            FileState fileState = info as FileState;
            if (FileDict.ContainsKey(fileState.RelPath))
                FileDict.Remove(fileState.RelPath);
        }
        private void Save()
        {
            UpdateList();
            string path = string.Format(@"{0}\{1}", Values.ApplicationDir, LIB_NAME);
            StringBuilder builder = new StringBuilder();
            foreach (var file in FileDict)
                builder.AppendFormat("{0}|{1}\r\n", file.Key, file.Value.MD5Hash);
            Util.SaveFile(path, builder.ToString());
        }
        private void DoFileState()
        {
            for (int i = 0; i < FileStates.Count; i++)
            {
                string path = Util.GetConfigAbsPath(FileStates[i].RelPath);
                if (!File.Exists(path)) continue;
                if (!FileDict.ContainsKey(FileStates[i].RelPath))
                    FileDict.Add(FileStates[i].RelPath, FileStates[i]);
            }

            List<string> diffRelPath = new List<string>();
            string[] files = Directory.GetFiles(Values.ConfigDir, "*.xls", SearchOption.AllDirectories);
            for (int j = 0; j < files.Length; j++)
            {
                string relPath = Util.GetConfigRelPath(files[j]);
                string md5 = Util.GetMD5HashFromFile(files[j]);
                if (string.IsNullOrWhiteSpace(md5)) return;
                if (FileDict.ContainsKey(relPath))
                {
                    if (FileDict[relPath].MD5Hash != md5)
                    {
                        FileDict[relPath].MD5Hash = md5;
                        diffRelPath.Add(relPath);
                    }
                }
                else
                {
                    FileState fileState = new FileState();
                    fileState.RelPath = relPath;
                    fileState.MD5Hash = md5;
                    Add(fileState);
                    diffRelPath.Add(relPath);
                }
            }
            if (diffRelPath.Count > 0)
            {
                Save();
                for (int k = 0; k < diffRelPath.Count; k++)
                {
                    Util.LogFormat(">修改文件:{0}", diffRelPath[k]);
                }
            }

            Util.LogFormat("\r\n==>>修改文件数{0}个\n", diffRelPath.Count.ToString());
            if (Values.IsOptPart)
                DiffRelPath.AddRange(diffRelPath);
            else
                DiffRelPath.AddRange(this.FileDict.Keys);
            DiffRelPath = DiffRelPath.FindAll(f => Path.GetExtension(f).Contains(".xls"));

        }
        private void UpdateList()
        {
            var ls = new List<FileState>();
            foreach (var f in FileDict)
            {
                string path = Util.GetConfigAbsPath(f.Key);
                if (File.Exists(path))
                    ls.Add(f.Value);
            }
            FileStates = ls;
        }
    }
    public class FileState
    {
        public string RelPath { get; set; }
        public string MD5Hash { get; set; }
    }
}
