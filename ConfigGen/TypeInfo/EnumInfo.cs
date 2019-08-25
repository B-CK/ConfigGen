using Description.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Description.TypeInfo
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
        public static List<EnumInfo> GetExports()
        {
            return new List<EnumInfo>(Enums.Values);
        }
        static void Add(EnumInfo info)
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
        public Dictionary<string, string> Values { get { return _values; } }
        public Dictionary<string, string> Aliases { get { return _aliases; } }

        public string GetEnumValue(string name)
        {
            if (_values.ContainsKey(name))
                return _values[name];
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
        private Dictionary<string, string> _values = new Dictionary<string, string>();
        /// <summary>
        /// key-枚举别名;
        /// value-枚举名
        /// </summary>
        private Dictionary<string, string> _aliases = new Dictionary<string, string>();

        public EnumInfo(EnumXml des, string namespace0)
        {
            _des = des;
            _namespace = namespace0;
            _fullName = string.Format("{0}.{1}", namespace0, des.Name);

            if (!Util.MatchIdentifier(Name))
                Error("命名不合法:" + Name);

            var consts = des.Enums;
            for (int i = 0; i < consts.Count; i++)
            {
                var cst = consts[i];
                int value = 0;
                if (!cst.Value.IsEmpty() && !int.TryParse(cst.Value, out value))
                    Error("值无法转换成数字:" + cst.Value);
                if (!_values.ContainsKey(cst.Name))
                    _values.Add(cst.Name, cst.Value);
                else
                    Error("Name重复定义:" + cst.Name);
                if (!cst.Alias.IsEmpty())
                {
                    if (!_aliases.ContainsKey(cst.Alias))
                        _aliases.Add(cst.Alias, cst.Name);
                    else
                        Error("Alias重复定义:" + cst.Alias);
                }
            }

            Add(this);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Enum - FullName:{0}\tDataPath{1}\tGroup:{2}\n", FullName, _des.Group);
            var ait = _aliases.GetEnumerator();
            while (ait.MoveNext())
            {
                var item = ait.Current;
                builder.AppendFormat("\t{0}({1}) = {2}\n", item.Value, item.Key, _values[item.Value]);
            }
            return builder.ToString();
        }
        private void Error(string msg)
        {
            string error = string.Format("Enum:{0} 错误:{1}", FullName, msg);
            throw new Exception(error);
        }
    }
}
