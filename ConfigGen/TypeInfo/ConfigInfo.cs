using ConfigGen.Data;
using ConfigGen.Description;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConfigGen.TypeInfo
{
    public class ConfigInfo
    {
        private static Dictionary<string, ConfigInfo> _configs = new Dictionary<string, ConfigInfo>();
        public static Dictionary<string, ConfigInfo> Configs { get { return _configs; } }
        public static ConfigInfo Get(string fullName)
        {
            return IsConfig(fullName) ? _configs[fullName] : null;
        }
        public static bool IsConfig(string fullName)
        {
            return _configs.ContainsKey(fullName);
        }
        static void Add(ConfigInfo info)
        {
            if (_configs.ContainsKey(info._fullName))
                Util.LogWarningFormat("{0} 重复定义!", info._fullName);
            else
                _configs.Add(info._fullName, info);
        }


        public string FullName { get { return _fullName; } }
        public string Namespace { get { return _namespace; } }
        public string Name { get { return _des.Name; } }
        public string Index { get { return _des.Index; } }
        public HashSet<string> Groups { get { return _groups; } }
        public FList Data { get { return _data; } }
        public string[] InputFiles { get { return _inputFiles; } }
        public string OutputFile { get { return _outputFile; } }


        private ClassDes _des;
        private string _fullName;
        private string _xmlDir;
        private string _namespace;
        private string[] _inputFiles;
        private string _outputFile;
        private FList _data;
        private readonly HashSet<string> _groups;

        public ConfigInfo(ClassDes des, string namespace0)
        {
            _des = des;
            _namespace = namespace0;
            _fullName = string.Format("{0}.{1}", namespace0, des.Name);
            _groups = new HashSet<string>(Util.Split(des.Group));
            if (File.Exists(_des.DataPath))
                _inputFiles = new string[] { _des.DataPath };
            else
                _inputFiles = Directory.GetFiles(_des.DataPath);
            string relPath = _fullName.Replace('.', '\\');
            _outputFile = string.Format("{0}{1}", Values.DataDir, relPath);

            Add(this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Config - FullName:{0}\tGroup:{1}\tDataPath:{2}\n", FullName, _des.Group, _des.DataPath);
            return builder.ToString();
        }
        public void Error(string msg)
        {
            string error = string.Format("Config:{0} 错误:{1}", FullName, msg);
            throw new Exception(error);
        }
    }
}
