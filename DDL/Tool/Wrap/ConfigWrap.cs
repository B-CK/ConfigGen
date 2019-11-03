using System;
using System.IO;
using System.Text;
using Description.Wrap;
using Description.Import;
using Xml;
using System.Collections.Generic;
using System.Xml;
using Description;

namespace Wrap
{
    /// <summary>
    /// 与数据关联的结构
    /// </summary>
    public class ConfigWrap
    {
        private static Dictionary<string, ConfigWrap> _configs = new Dictionary<string, ConfigWrap>();
        public static Dictionary<string, ConfigWrap> Configs { get { return _configs; } }
        public static ConfigWrap Get(string fullName)
        {
            return IsConfig(fullName) ? _configs[fullName] : null;
        }
        public static bool IsConfig(string fullName)
        {
            return _configs.ContainsKey(fullName);
        }
        public static List<ConfigWrap> GetExports()
        {
            var exports = new List<ConfigWrap>();
            var cit = _configs.GetEnumerator();
            while (cit.MoveNext())
            {
                var cfg = cit.Current.Value;
                if (Util.MatchGroups(cfg._groups))
                    exports.Add(cfg);
            }
            return exports;
        }
        static void Add(ConfigWrap info)
        {
            if (_configs.ContainsKey(info._fullType))
                Util.LogWarningFormat("{0} 重复定义!", info._fullType);
            else
                _configs.Add(info._fullType, info);
        }


        public string FullType { get { return _fullType; } }
        public string Namespace { get { return _namespace; } }
        public string Name { get { return _des.Name; } }
        public FieldWrap Index { get { return _index; } }
        public HashSet<string> Groups { get { return _groups; } }
        public FList Data { get { return _data; } }
        public string[] InputFiles { get { return _inputFiles; } }
        public string OutputFile { get { return _outputFile; } }


        private ClassXml _des;
        private string _fullType;
        private string _namespace;
        private string[] _inputFiles;
        private string _outputFile;
        private FieldWrap _index;
        private FList _data;
        private readonly HashSet<string> _groups;

        public ConfigWrap(ClassXml des, string namespace0, string xmlDir)
        {
            _des = des;
            _namespace = namespace0;
            _fullType = string.Format("{0}.{1}", namespace0, des.Name);
            _groups = new HashSet<string>(Util.Split(des.Group));
            if (_groups.Count == 0)
                _groups.Add(Setting.DefualtGroup);

            if (des.Index.IsEmpty())
                Error("索引(Index)未填写");
            ClassWrap cls = ClassWrap.Get(_fullType);
            _index = cls.Fields.Find(f => f.Name == des.Index);

            string path = Path.Combine(xmlDir, _des.DataPath);
            if (File.Exists(path))
                _inputFiles = new string[] { path };
            else if (Directory.Exists(path))
                _inputFiles = Directory.GetFiles(path);
            else
                Error("数据路径不存在:" + path);
            _outputFile = _fullType.Replace('.', '/').Substring(Setting.ModuleName.Length + 1).ToLower();

            Add(this);
        }

        public void VerifyDefine()
        {
            if (!_fullType.IsEmpty())
            {
                if (_index == null)
                    Error(string.Format("Index:{0} 不是Class:{1}的字段", _des.Index, _fullType));
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
                    string fullType = "list:" + _fullType;
                    FieldWrap field = new FieldWrap(null, Name, fullType, Util.Split(fullType), _groups);
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
            _data.VerifyData();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Config - FullName:{0}\tGroup:{1}\tDataPath:{2}\n", FullType, _des.Group, _des.DataPath);
            return builder.ToString();
        }
        private void Error(string msg)
        {
            string error = string.Format("Config:{0} 错误:{1}", FullType, msg);
            throw new Exception(error);
        }
    }
}
