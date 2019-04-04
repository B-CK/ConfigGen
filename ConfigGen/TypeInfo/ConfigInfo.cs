using System;
using System.IO;
using System.Text;
using ConfigGen.Config;
using ConfigGen.Import;
using ConfigGen.Description;
using System.Collections.Generic;
using System.Xml;

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
        public static List<ConfigInfo> GetExports()
        {
            var exports = new List<ConfigInfo>();
            var cit = _configs.GetEnumerator();
            while (cit.MoveNext())
            {
                var cfg = cit.Current.Value;
                if (Util.MatchGroups(cfg._groups))
                    exports.Add(cfg);
            }
            return exports;
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

        public ConfigInfo(ClassDes des, string namespace0, string xmlDir)
        {
            _des = des;
            _namespace = namespace0;
            _fullName = string.Format("{0}.{1}", namespace0, des.Name);
            _groups = new HashSet<string>(Util.Split(des.Group));
            if (_groups.Count == 0)
                _groups.Add(Setting.DefualtGroup);

            if (des.Index.IsEmpty())
                Error("索引(Index)未填写");
            ClassInfo cls = ClassInfo.Get(_fullName);
            _index = cls.Fields.Find(f => f.Name == des.Index);

            string path = Path.Combine(xmlDir, _des.DataPath);
            if (File.Exists(path))
                _inputFiles = new string[] { path };
            else if (Directory.Exists(path))
                _inputFiles = Directory.GetFiles(path);
            else
                Error("数据路径不存在:" + path);
            _outputFile = _fullName.Replace('.', '/');

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

                for (int i = 0; i < _inputFiles.Length; i++)
                {
                    string path = _inputFiles[i];
                    if (!File.Exists(path) && !Directory.Exists(path))
                        Error("数据文件路径不存在:" + path);
                }
            }
        }
        public void LoadData()
        {
            for (int i = 0; i < _inputFiles.Length; i++)
            {
                string path = _inputFiles[i];
                try
                {
                    string ext = Path.GetExtension(path);
                    FieldInfo field = new FieldInfo(null, Name, "list:" + _fullName, _groups);
                    if (ext == "xml")
                    {
                        var xml = new ImportXml(path);
                        _data = new FList(null, field, xml.Data);
                    }
                    else
                    {
                        var excel = new ImportExcel(path);
                        _data = new FList(null, field, excel);
                    }
                }
                catch (Exception e)
                {
                    Util.LogErrorFormat("{0}\n{1}\n", e.Message, e.StackTrace);
                    Error("[加载文件失败]:" + path);
                }

            }
        }
        public void VerifyData()
        {
            for (int i = 0; i < _data.Values.Count; i++)
            {
                //var value = _data.Values[i] as ;
            }
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
