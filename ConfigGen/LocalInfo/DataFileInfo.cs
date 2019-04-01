using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigGen.Description
{
    public class DataFileInfo
    {
        const string LIB_NAME = "DataFileInfo.lfi";

        private static DataFileInfo _instance;
        private Dictionary<string, string> _stateDict;
        private Dictionary<string, string> _newStateDict;
        public static void Init()
        {
            _instance = new DataFileInfo();
            _instance._stateDict = new Dictionary<string, string>();
            _instance._newStateDict = new Dictionary<string, string>();
            string path = string.Format(@"{0}\{1}", Consts.ApplicationDir, LIB_NAME);
            if (!File.Exists(path)) return;

            string content = File.ReadAllText(path);
            string[] lines = content.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] nodes = lines[i].Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                _instance._stateDict.Add(nodes[0], nodes[1]);
            }
        }

        public static bool HasChangeFile(string path)
        {
            bool result = false;
            var dict = _instance._stateDict;
            if (File.Exists(path))
            {
                string refPath = Util.GetRelPath(path);
                string md5 = Util.GetMD5HashFromFile(path);
                if (dict.ContainsKey(refPath))
                    result = !dict[refPath].Equals(md5);
                else
                    result = true;
                _instance._newStateDict.Add(refPath, md5);
            }
            else if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path, "*." + Consts.DataFileFlag, SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    string refPath = Util.GetRelPath(files[i]);
                    if (File.Exists(files[i]))
                    {
                        string md5 = Util.GetMD5HashFromFile(files[i]);
                        if (dict.ContainsKey(refPath))
                            result = !dict[refPath].Equals(md5);
                        else
                            result = true;
                        _instance._newStateDict.Add(refPath, md5);
                    }
                }
            }
            return result;
        }

        public static void Save()
        {
            string path = string.Format(@"{0}\{1}", Consts.ApplicationDir, LIB_NAME);
            StringBuilder builder = new StringBuilder();
            foreach (var file in _instance._newStateDict)
                builder.AppendFormat("{0}|{1}\r\n", file.Key, file.Value);
            Util.SaveFile(path, builder.ToString());
        }
    }
}
