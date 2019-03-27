using ConfigGen.Description;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigGen.TypeInfo
{
    public class EnumInfo
    {
        public const int NULL = int.MinValue;
        private static readonly Dictionary<string, EnumInfo> _enums = new Dictionary<string, EnumInfo>();
        public static Dictionary<string, EnumInfo> Enums { get { return _enums; } }
        public static bool IsEnum(string fullName)
        {
            return _enums.ContainsKey(fullName);
        }

        public string XmlPath { get { return _xmlDir; } }
        public string FullName { get { return _fullName; } }
        public string Namespace { get { return _namespace; } }
        public string Name { get { return _des.Name; } }
        public string Desc { get { return _des.Desc; } }
        public Dictionary<string, int> Values { get { return _values; } }
        public Dictionary<string, string> Aliases { get { return _aliases; } }

        public int GetEnumValue(string name)
        {
            if (_values.ContainsKey(name))
                return _values[name];
            return NULL;
        }
        public string GetEnumName(string alias)
        {
            if (_aliases.ContainsKey(alias))
                return _aliases[alias];
            return "";
        }

        private EnumDes _des;
        private string _xmlDir;
        private string _namespace;
        private string _fullName;
        /// <summary>
        /// key-枚举名;
        /// value-枚举值
        /// </summary>
        private Dictionary<string, int> _values = new Dictionary<string, int>();
        /// <summary>
        /// key-枚举别名;
        /// value-枚举名
        /// </summary>
        private Dictionary<string, string> _aliases = new Dictionary<string, string>();

        public EnumInfo(EnumDes des)
        {
            _des = des;
        }
    }
}
