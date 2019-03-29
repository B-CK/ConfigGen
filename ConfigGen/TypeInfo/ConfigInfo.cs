using ConfigGen.Data;
using ConfigGen.Description;
using System;
using System.Collections.Generic;
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
            return _configs.ContainsKey(fullName);// && _configs[fullName].
        }

        private ClassDes _des;
        private string _namespace;
        private string _fullName;
        private string _xmlDir;
        private FList _data;
        private string[] _inputFiles;
        private string _outputFile;
        private readonly HashSet<string> _groups;

    }
}
