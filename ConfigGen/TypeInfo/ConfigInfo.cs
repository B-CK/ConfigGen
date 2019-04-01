using ConfigGen.Config;
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
        public FieldInfo Index { get { return _index; } }
        public HashSet<string> Groups { get { return _groups; } }
        public FList Data { get { return _data; } }
        public string[] InputFiles { get { return _inputFiles; } }
        public string OutputFile { get { return _outputFile; } }


        private ClassDes _des;
        private string _fullName;
        private string _namespace;
        private string[] _inputFiles;
        private string _outputFile;
        private FieldInfo _index;
        private FList _data;
        private readonly HashSet<string> _groups;

        public ConfigInfo(ClassDes des, string namespace0)
        {
            _des = des;
            _namespace = namespace0;
            _fullName = string.Format("{0}.{1}", namespace0, des.Name);
            _groups = new HashSet<string>(Util.Split(des.Group));
            if (_groups.Count == 0)
                _groups.Add(Consts.DefualtGroup);

            if (des.Index.IsEmpty())
                Error("索引(Index)未填写");
            ClassInfo cls = ClassInfo.Get(_fullName);
            _index = cls.Fields.Find(f => f.Name == des.Index);

            if (File.Exists(_des.DataPath))
                _inputFiles = new string[] { _des.DataPath };
            else if (Directory.Exists(_des.DataPath))
                _inputFiles = Directory.GetFiles(_des.DataPath);
            else
                Error("数据路径不存在:" + _des.DataPath);
            string relPath = _fullName.Replace('.', '\\');
            _outputFile = string.Format("{0}{1}", Consts.DataDir, relPath);

            Add(this);
        }

        public void VerifyDefine()
        {
            if (!_fullName.IsEmpty())
            {
                if (_index == null)
                    Error(string.Format("Index:{0} 不是Class:{1}的字段", _des.Index, _fullName));
                if (_index.IsContainer && _index.IsClass)
                    Error("Index 不能是集合类型或者类类型");

                string path = _des.DataPath;
                if (!File.Exists(path) && !Directory.Exists(path))
                    Error("数据文件路径不存在:" + path);
            }
        }
        public void LoadData()
        {

        }
        public void VerifyData()
        {

        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Config - FullName:{0}\tGroup:{1}\tDataPath:{2}\n", FullName, _des.Group, _des.DataPath);
            return builder.ToString();
        }
        private void Error(string msg)
        {
            string error = string.Format("Config:{0} 错误:{1}", FullName, msg);
            throw new Exception(error);
        }
    }
}
