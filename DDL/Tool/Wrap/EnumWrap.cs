using Description;
using Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wrap
{
    public class EnumWrap
    {
        public const int NULL = int.MinValue;
        private static readonly Dictionary<string, EnumWrap> _enums = new Dictionary<string, EnumWrap>();
        public static Dictionary<string, EnumWrap> Enums { get { return _enums; } }
        public static bool IsEnum(string fullName)
        {
            return _enums.ContainsKey(fullName);
        }
        public static List<EnumWrap> GetExports()
        {
            return new List<EnumWrap>(Enums.Values);
        }
        static void Add(EnumWrap info)
        {
            if (_enums.ContainsKey(info._fullName))
                Util.LogWarningFormat("{0} 重复定义!", info._fullName);
            else
                _enums.Add(info._fullName, info);
        }

        public string FullName { get { return _fullName; } }
        public string Namespace { get { return _namespace; } }
        public string Name { get { return _des.Name; } }
        public string Desc { get { return _des.Desc; } }
        public Dictionary<string, int> Values { get { return _values; } }
        public Dictionary<string, string> Aliases { get { return _aliases; } }
        public Dictionary<string, EnumItemXml> Items => _items;

        public string GetEnumValue(string name)
        {
            if (_values.ContainsKey(name))
                return _values[name].ToString();
            return Setting.Null;
        }
        public string GetEnumName(string alias)
        {
            if (_aliases.ContainsKey(alias))
                return _aliases[alias];
            return "";
        }

        private EnumXml _des;
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
        private Dictionary<string, EnumItemXml> _items = new Dictionary<string, EnumItemXml>();

        public EnumWrap(EnumXml des, string namespace0)
        {
            _des = des;
            _namespace = namespace0;
            _fullName = string.Format("{0}.{1}", namespace0, des.Name);

            if (!Util.MatchIdentifier(Name))
                Error("命名不合法:" + Name);

            var items = des.Items;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (!_values.ContainsKey(item.Name))
                    _values.Add(item.Name, item.Value);
                else
                    Error("Name重复定义:" + item.Name);
                if (!item.Alias.IsEmpty())
                {
                    if (!_aliases.ContainsKey(item.Alias))
                        _aliases.Add(item.Alias, item.Name);
                    else
                        Error("Alias重复定义:" + item.Alias);
                }
                _items.Add(item.Name, item);
            }

            Add(this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Enum - FullName:{0}\tGroup:{1}\n", FullName, _des.Group);
            foreach (var item in _aliases)
                builder.AppendFormat("\t{0}({1}) = {2}\n", item.Value, item.Key, _values[item.Value]);
            return builder.ToString();
        }
        private void Error(string msg)
        {
            string error = string.Format("Enum:{0} 错误:{1}", FullName, msg);
            throw new Exception(error);
        }
    }
}
