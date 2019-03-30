using ConfigGen.Description;
using System.Collections.Generic;

namespace ConfigGen.TypeInfo
{
    public class FieldInfo
    {
        public static bool IsRawOrEnumOrClass(string type)
        {
            return Values.RawTypes.Contains(type)
                || EnumInfo.IsEnum(type) || ClassInfo.IsDynamic(type);
        }


        public ClassInfo Host { get { return _host; } }
        public string Name { get { return _des.Name; } }
        /// <summary>
        /// 完整类型
        /// </summary>
        public string FullType { get { return _des.Type; } }
        public string Desc { get { return _des.Desc; } }
        /// <summary>
        /// 初始类型,即去除泛型后的类型
        /// </summary>
        public string OriginalType { get { return _types[0]; } }
        public bool IsRaw { get { return Values.RawTypes.Contains(OriginalType); } }
        public bool IsContainer { get { return Values.ContainerTypes.Contains(OriginalType); } }
        public bool IsClass { get { return ClassInfo.IsClass(OriginalType); } }
        public bool IsDynamic { get { return ClassInfo.IsDynamic(OriginalType); } }
        public bool IsEnum { get { return EnumInfo.IsEnum(OriginalType); } }

        private FieldDes _des;
        private readonly ClassInfo _host;
        /// <summary>
        /// 嵌套类型,例dict:int:string,则0=dict,1=int,0=string
        /// 任意类型用_types[0]描述
        /// </summary>
        private readonly List<string> _types;
        /// <summary>
        /// 优先Class.Group,其次才是Field.Group
        /// </summary>
        private readonly HashSet<string> _groups;

        /// <summary>
        /// 泛型类型TKV字段
        /// </summary>
        public FieldInfo(ClassInfo host, FieldDes des, List<string> ts, HashSet<string> gs)
        {
            _des = des;
            _host = host;
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
            _types = new List<string>(Util.Split(des.Type));
            string[] gs = Util.Split(des.Group);
            _groups = new HashSet<string>(gs ?? new string[0]);

            if (_groups.Count == 0 && !des.Group.IsEmpty())
            {
                var groups = des.Group.Split(Values.ArgsSplitFlag);
                for (int i = 0; i < groups.Length; i++)
                {
                    if (!_groups.Contains(groups[i]))
                        _groups.Add(groups[i]);
                }
            }
        }

        /// <summary>
        /// list 项定义
        /// </summary>
        public FieldInfo GetItemDefine()
        {
            return new FieldInfo(_des, _host, new List<string>() { _types[1] }, _groups);
        }
        /// <summary>
        /// dict key 定义
        /// </summary>
        public FieldInfo GetKeyDefine()
        {
            return new FieldInfo(_des, _host, new List<string>() { _types[1] }, _groups);
        }
        /// <summary>
        /// dict value 定义
        /// </summary>
        public FieldInfo GetValueDefine()
        {
            return new FieldInfo(_des, _host, new List<string>() { _types[2] }, _groups);
        }

        public void VerifyDefine()
        {
            CheckType(1);
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

                        ConfigInfo config = ConfigInfo.Get(itemType);
                        if (config != null && !config.Index.IsEmpty())
                        {
                             
                        }
                    }
                    else if (type == "dict")
                    {
                        CheckType(3);
                        string key = _types[1];
                        if (IsRawOrEnumOrClass(key))
                            Error("非法的dict key类型:" + key);
                        string value = _types[2];
                        if (IsRawOrEnumOrClass(value))
                            Error("非法的dict value类型:" + value);
                    }
                }
                else
                    Error("未知类型:" + type);
            }

            var git = _groups.GetEnumerator();
            while (git.MoveNext())
                if (!GroupInfo.IsGroup(git.Current))
                    Error("未知 Group:" + git.Current);
        }
        void CheckType(int size)
        {
            if (_types.Count < size)
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
