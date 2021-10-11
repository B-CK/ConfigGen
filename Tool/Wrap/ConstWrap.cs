using Xml;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Tool.Wrap
{
    public class ConstWrap
    {
        public static bool IsConst(string type)
        {
            return Setting.ConstTypes.Contains(type) || EnumWrap.IsEnum(type);
        }

        public ClassWrap Host { get { return _host; } }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get { return _name; } }
        /// <summary>
        /// 完整类型,实际类型;当为动态类型时,_fullType != _types[0]
        /// </summary>
        public string FullName { get { return _fullName; } }
        public string Desc { get { return _desc; } }
        public string Value { get { return _value; } }
        public HashSet<string> Group { get { return _groups; } }


        private ClassWrap _host;
        private string _fullName;
        private string _name;
        private string _type;
        private string _value;
        private string _group;
        private HashSet<string> _groups;
        private string _desc;

        public ConstWrap(ClassWrap host, string name, string type, string value, string group, string desc, HashSet<string> gs)
        {
            _host = host;
            _fullName = type;
            _name = name;
            _value = value;
            _desc = desc;
            _groups = gs;

            _group = group == null ? "" : group.ToLower();
            if (_groups == null)
            {
                _groups = new HashSet<string>(Util.Split(_group));
                if (_groups.Count == 0)
                    _groups.Add(Setting.DefualtGroup);
            }
        }

        public void VerifyDefine()
        {
            if (!Util.MatchIdentifier(Name))
                Error("命名不合法:" + Name);
            if (!IsConst(_fullName))
                Error("常量仅支持基础类型(bool,int,long,float,string,enum):" + Name);
            switch (FullName)
            {
                case Setting.BOOL:
                    if (!bool.TryParse(_value, out bool b))
                        Error("值与类型不匹配:" + Name);
                    break;
                case Setting.INT:
                    if (!int.TryParse(_value, out int i))
                        Error("值与类型不匹配:" + Name);
                    break;
                case Setting.LONG:
                    if (!long.TryParse(_value, out long l))
                        Error("值与类型不匹配:" + Name);
                    break;
                case Setting.FLOAT:
                    if (!float.TryParse(_value, out float f))
                        Error("值与类型不匹配:" + Name);
                    break;
                default:
                    if (EnumWrap.IsEnum(_fullName) && !EnumWrap.Enums[_fullName].ContainItem(Value))
                        Error("值与类型不匹配:" + Name);
                    break;
            }
            var git = _groups.GetEnumerator();
            while (git.MoveNext())
                if (!GroupWrap.IsGroup(git.Current))
                    Error("未知 Group:" + git.Current);
        }
        public override string ToString()
        {
            return string.Format("Const - Name:{0}\tType:{1}\tValue:{2}\tGroup:{3}", Name, FullName, Value, _group);
        }
        void Error(string msg)
        {
            string error = string.Format("常量字段信息异常!\n{0}Const:{1}({2})  {3}", _host, Name, FullName, msg);
            throw new System.Exception(error);
        }
    }
}
