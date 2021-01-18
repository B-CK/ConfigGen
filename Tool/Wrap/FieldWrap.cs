using System.Collections.Generic;
using Xml;
using Tool.Check;

namespace Tool.Wrap
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
        public string FullName { get { return _fullName; } }
        public string Desc { get { return _desc; } }
        public string Attribute { get { return _attribute; } }
        /// <summary>
        /// 原始类型,例泛型list:int返回list,其他直接返回类型
        /// </summary>
        public string OriginalType { get { return _types[0]; } }
        public string[] Types { get { return _types; } }
        public HashSet<string> Group { get { return _groups; } }
        public List<Checker> Checkers { get { return _checkers; } }
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
                return parent.HasChild(_fullName);
            }
        }

        private ClassWrap _host;
        private string _name;
        private string _fullName;
        private string _desc;
        private string _attribute;
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
        private List<Checker> _checkers;

        /// <summary>
        /// Class 字段
        /// </summary>
        public FieldWrap(ClassWrap host, string name, string fullName, string group, string desc, string attribute, HashSet<string> parentGroups)
            : this(host, name, fullName, Util.Split(fullName), parentGroups)
        {
            _group = group == null ? "" : group.ToLower();
            if (_groups == null)
            {
                _groups = new HashSet<string>(Util.Split(_group));
                if (_groups.Count == 0)
                    _groups.Add(Setting.DefualtGroup);
            }
            _desc = desc;
            _attribute = attribute;
        }
        /// <summary>
        /// config作为字段定义
        /// </summary>
        public FieldWrap(ClassWrap host, string name, string fullName, string[] types, HashSet<string> gs)
        {
            _host = host;
            _name = name;
            _fullName = fullName;
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
                _fullName = Util.ToString(_types, ":");
            }
            else if (_fullName.IndexOfAny(Setting.DotSplit) < 0)
            {
                _fullName = string.Format("{0}.{1}", _host.Namespace, _fullName);
                _types[0] = _fullName;
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
        public void CreateChecker(FieldXml field)
        {
            if (OriginalType == Setting.BOOL) return;

            if (_checkers == null)
                _checkers = new List<Checker>();
            if (!field.Ref.IsEmpty())
                _checkers.Add(new RefChecker(this, field.Ref));
            if (!field.File.IsEmpty())
                _checkers.Add(new FileChecker(this, field.File));
            if (field.Unique != null)
                _checkers.Add(new UniqueChecker(this, field.Unique));
            if (field.NotEmpty != null)
                _checkers.Add(new NotEmptyChecker(this, field.NotEmpty));
            if (!field.Range.IsEmpty())
                _checkers.Add(new RangeChecker(this, field.Range));
        }
        public void CreateKeyChecker()
        {
            if (_checkers == null)
                _checkers = new List<Checker>();
            _checkers.Add(new UniqueChecker(this, ""));
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
                            Error("list的 item类型不存在或者不是raw,enum,class类型:" + itemType);
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

            //验证检查规则
            if (_checkers != null)
            {
                var errorCheckers = new List<Checker>();
                var checkers = _checkers;
                for (int k = 0; k < checkers.Count; k++)
                {
                    if (!checkers[k].VerifyRule())
                        errorCheckers.Add(checkers[k]);
                }
                for (int i = 0; i < errorCheckers.Count; i++)
                    _checkers.Remove(errorCheckers[i]);
            }
        }
        void CheckType(int size)
        {
            if (_types.Length < size)
                Error("定义非法Type");
        }
        public void Error(string msg)
        {
            throw new System.Exception($"字段信息异常!\n{_host}Field:{Name}({FullName}) {msg}");
        }
        public override string ToString()
        {
            return string.Format("Field - Name:{0}\tType:{1}\tGroup:{2}", Name, FullName, _group);
        }

    }
}
