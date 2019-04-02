using ConfigGen.Description;
using System.Collections.Generic;

namespace ConfigGen.TypeInfo
{
    public class FieldInfo
    {
        public static bool IsRawOrEnumOrClass(string type)
        {
            return Consts.RawTypes.Contains(type)
                || EnumInfo.IsEnum(type) || ClassInfo.IsDynamic(type);
        }


        public ClassInfo Host { get { return _host; } }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get { return _des.Name; } }
        /// <summary>
        /// 完整类型
        /// </summary>
        public string FullType { get { return _fullType; } }
        public string Desc { get { return _des.Desc; } }
        /// <summary>
        /// 原始类型,例泛型list:int返回list,其他直接返回类型
        /// </summary>
        public string OriginalType { get { return _types[0]; } }
        public string[] Types { get { return _types; } }
        public HashSet<string> Group { get { return _groups; } }
        public bool IsRaw { get { return Consts.RawTypes.Contains(OriginalType); } }
        public bool IsContainer { get { return Consts.ContainerTypes.Contains(OriginalType); } }
        public bool IsClass { get { return ClassInfo.IsClass(OriginalType); } }
        public bool IsDynamic { get { return ClassInfo.IsDynamic(OriginalType); } }
        public bool IsEnum { get { return EnumInfo.IsEnum(OriginalType); } }

        private FieldDes _des;
        private string _fullType;
        private ClassInfo _host;
        /// <summary>
        /// 嵌套类型,例dict:int:string,则0=dict,1=int,0=string
        /// 任意类型用_types[0]描述
        /// </summary>
        private string[] _types;
        /// <summary>
        /// 优先Class.Group,其次才是Field.Group
        /// </summary>
        private HashSet<string> _groups;

        /// <summary>
        /// config作为字段定义
        /// </summary>
        public FieldInfo(string fullType, HashSet<string> gs)
        {
            _fullType = fullType;
            _types = Util.Split(fullType);
            _groups = gs;
        }
        /// <summary>
        /// 泛型类型TKV字段
        /// </summary>
        public FieldInfo(ClassInfo host, FieldDes des, string[] ts, HashSet<string> gs)
        {
            _des = des;
            _host = host;
            _fullType = des.Type;
            _types = ts;
            _groups = gs;
        }
        /// <summary>
        /// 类字段成员
        /// </summary>
        public FieldInfo(ClassInfo host, FieldDes des)
        {
            _des = des;
            _host = host;
            _fullType = des.Type;
            _types = Util.Split(_fullType);

            _groups = new HashSet<string>(Util.Split(des.Group));
            if (_groups.Count == 0)
                _groups.Add(Consts.DefualtGroup);
        }

        /// <summary>
        /// list 项定义
        /// </summary>
        public FieldInfo GetItemDefine()
        {
            return new FieldInfo(_host, _des, new string[1] { _types[1] }, _groups);
        }
        /// <summary>
        /// dict key 定义
        /// </summary>
        public FieldInfo GetKeyDefine()
        {
            return new FieldInfo(_host, _des, new string[1] { _types[1] }, _groups);
        }
        /// <summary>
        /// dict value 定义
        /// </summary>
        public FieldInfo GetValueDefine()
        {
            return new FieldInfo(_host, _des, new string[1] { _types[2] }, _groups);
        }

        public void VerifyDefine()
        {
            CheckType(1);
            if (!Util.MatchName(Name))
                Error("命名不合法:" + Name);
            string type = OriginalType;
            if (!IsRaw)
            {
                if (IsContainer)
                {
                    if (type == "list")
                    {
                        CheckType(2);
                        _types[1] = VerifyType(_types[1]);
                        string itemType = _types[1];
                        if (!IsRawOrEnumOrClass(itemType))
                            Error("非法的list item类型:" + itemType);
                    }
                    else if (type == "dict")
                    {
                        CheckType(3);
                        _types[1] = VerifyType(_types[1]);
                        _types[2] = VerifyType(_types[2]);
                        string key = _types[1];
                        if (!Consts.RawTypes.Contains(key) && !EnumInfo.IsEnum(key))
                            Error("非法的dict key类型:" + key);
                        string value = _types[2];
                        if (!IsRawOrEnumOrClass(value))
                            Error("非法的dict value类型:" + value);
                    }
                }
                else
                {
                    _fullType = _types[0] = VerifyType(_types[0]);
                    if (!IsClass && !IsEnum)
                        Error("未知类型:" + type);
                }
            }

            var git = _groups.GetEnumerator();
            while (git.MoveNext())
                if (!GroupInfo.IsGroup(git.Current))
                    Error("未知 Group:" + git.Current);
        }
        string VerifyType(string type)
        {
            if (_host != null && type.IndexOf(Consts.ConfigRootNode) < 0)
                type = string.Format("{0}.{1}", _host.Namespace, type);
            return type;
        }
        void CheckType(int size)
        {
            if (_types.Length < size)
                Error("定义非法Type");
        }
        void Error(string msg)
        {
            string error = string.Format("{0}\nField:{1} {2}", _host, Name, msg);
            throw new System.Exception(error);
        }
        public override string ToString()
        {
            return string.Format("Field - Name:{0}\tType:{1}\tGroup:{2}", Name, FullType, _des.Group);
        }

    }
}
