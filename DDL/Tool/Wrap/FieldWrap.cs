using Description;
using System.Collections.Generic;

namespace Wrap
{
    public class FieldWrap
    {
        public static bool IsRawOrEnumOrClass(string type)
        {
            return Setting.RawTypes.Contains(type)
                || EnumWrap.IsEnum(type) || ClassWrap.IsClass(type);
        }


        public ClassWrap Host { get { return _host; } }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get { return _name; } }
        /// <summary>
        /// 完整类型,实际类型;当为动态类型时,_fullType != _types[0]
        /// </summary>
        public string FullType { get { return _fullType; } }
        public string Desc { get { return _desc; } }
        /// <summary>
        /// 原始类型,例泛型list:int返回list,其他直接返回类型
        /// </summary>
        public string OriginalType { get { return _types[0]; } }
        public string[] Types { get { return _types; } }
        public HashSet<string> Group { get { return _groups; } }
        public bool IsRaw { get { return Setting.RawTypes.Contains(OriginalType); } }
        public bool IsContainer { get { return Setting.ContainerTypes.Contains(OriginalType); } }
        public bool IsClass { get { return ClassWrap.IsClass(OriginalType); } }
        public bool IsDynamic { get { return ClassWrap.IsDynamic(OriginalType); } }
        public bool IsEnum { get { return EnumWrap.IsEnum(OriginalType); } }
        public bool IsInherit
        {
            get
            {
                ClassWrap parent = ClassWrap.Get(OriginalType);
                return parent.HasChild(_fullType);
            }
        }
        public string[] Refs { get { return _refs; } }
        public string[] RefPaths { get { return _refPaths; } }

        private ClassWrap _host;
        private string _name;
        private string _fullType;
        private string _desc;
        private string _group;
        /// <summary>
        /// 原始类型信息
        /// </summary>
        private string[] _types;
        /// <summary>
        /// 嵌套类型,例dict:int:string,则0=dict,1=int,0=string
        /// 任意类型用_types[0]描述
        /// </summary>
        /// <summary>
        /// 优先Class.Group,其次才是Field.Group
        /// </summary>
        private HashSet<string> _groups;
        private string[] _refs;
        private string[] _refPaths;

        public void InitCheck(string refs, string refPaths)
        {
            _refs = Util.Split(refs);
            _refPaths = Util.Split(refPaths);
        }
        /// <summary>
        /// Class 字段
        /// </summary>
        public FieldWrap(ClassWrap host, string name, string type, string group, string desc, HashSet<string> gs)
            : this(host, name, type, Util.Split(type), gs)
        {
            _group = group;
            if (_groups == null)
            {
                _groups = new HashSet<string>(Util.Split(group));
                if (_groups.Count == 0)
                    _groups.Add(Setting.DefualtGroup);
            }
            _desc = desc;
        }
        /// <summary>
        /// config作为字段定义
        /// </summary>
        public FieldWrap(ClassWrap host, string name, string type, string[] types, HashSet<string> gs)
        {
            _host = host;
            _name = name;
            _fullType = type;
            _types = types;
            _groups = gs;

            if (IsRaw || _host == null) return;
            if (IsContainer)
            {
                if (OriginalType == Setting.LIST && _types[1].IndexOfAny(Setting.DotSplit) < 0
                    && !Setting.RawTypes.Contains(_types[1]))
                    _types[1] = string.Format("{0}.{1}", _host.Namespace, _types[1]);
                else if (OriginalType == Setting.DICT && _types[2].IndexOfAny(Setting.DotSplit) < 0
                    && !Setting.RawTypes.Contains(_types[2]))
                    _types[2] = string.Format("{0}.{1}", _host.Namespace, _types[2]);
                _fullType = Util.ToString(_types, ":");
            }
            else if (_fullType.IndexOfAny(Setting.DotSplit) < 0)
            {
                _fullType = string.Format("{0}.{1}", _host.Namespace, _fullType);
                _types[0] = _fullType;
            }
        }

        /// <summary>
        /// list 项定义
        /// </summary>
        public FieldWrap GetItemDefine()
        {
            return new FieldWrap(_host, _name, _types[1], new string[] { _types[1] }, _groups);
        }
        /// <summary>
        /// dict key 定义
        /// </summary>
        public FieldWrap GetKeyDefine()
        {
            return new FieldWrap(_host, _name, _types[1], new string[] { _types[1] }, _groups);
        }
        /// <summary>
        /// dict value 定义
        /// </summary>
        public FieldWrap GetValueDefine()
        {
            return new FieldWrap(_host, _name, _types[2], new string[] { _types[2] }, _groups);
        }

        public void VerifyDefine()
        {
            CheckType(1);
            if (!Util.MatchIdentifier(Name))
                Error("命名不合法:" + Name);
            string type = OriginalType;
            if (!IsRaw && !IsClass && !IsEnum)
            {
                if (IsContainer)
                {
                    if (type == "list")
                    {
                        CheckType(2);
                        string itemType = _types[1];
                        if (!IsRawOrEnumOrClass(itemType))
                            Error("非法的list item类型:" + itemType);
                    }
                    else if (type == "dict")
                    {
                        CheckType(3);
                        string key = _types[1];
                        if (!Setting.RawTypes.Contains(key) && !EnumWrap.IsEnum(key))
                            Error("非法的dict key类型:" + key);
                        string value = _types[2];
                        if (!IsRawOrEnumOrClass(value))
                            Error("非法的dict value类型:" + value);
                    }
                }
                else
                {
                    Error("未知类型:" + type);
                }
            }

            var git = _groups.GetEnumerator();
            while (git.MoveNext())
                if (!GroupWrap.IsGroup(git.Current))
                    Error("未知 Group:" + git.Current);
        }
        void CheckType(int size)
        {
            if (_types.Length < size)
                Error("定义非法Type");
        }
        void Error(string msg)
        {
            string error = string.Format("字段信息异常!\n{0}Field:{1} {2}", _host, Name, msg);
            throw new System.Exception(error);
        }
        public override string ToString()
        {
            return string.Format("Field - Name:{0}\tType:{1}\tGroup:{2}", Name, FullType, _group);
        }

    }
}
