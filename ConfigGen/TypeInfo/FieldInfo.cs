using ConfigGen.Description;
using System.Collections.Generic;

namespace ConfigGen.TypeInfo
{
    public class FieldInfo
    {
        public ClassInfo Host { get { return _host; } }
        public string Name { get { return _des.Name; } }
        public string FullType { get { return _des.Type; } }
        public string Desc { get { return _des.Desc; } }
        /// <summary>
        /// 初始类型,即去除泛型后的类型
        /// </summary>
        public string OriginalType { get { return _types[0]; } }
        public bool IsRaw { get { return Values.RawTypes.Contains(OriginalType); } }
        /// <summary>
        /// 泛型的类型,例list:int,_tkv = int
        /// </summary>
        public bool IsGeneric { get { return !_tkv.IsEmpty(); } }
        public bool IsContainer { get { return Values.ContainerTypes.Contains(OriginalType); } }
        public bool IsClass { get { return ClassInfo.IsClass(OriginalType); } }
        public bool IsDynamic { get { return ClassInfo.IsDynamic(OriginalType); } }
        public bool IsEnum { get { return EnumInfo.IsEnum(OriginalType); } }

        private FieldDes _des;
        private readonly ClassInfo _host;
        /// <summary>
        /// 嵌套类型,例dict:int:string,则0=dict,1=int,0=string
        /// </summary>
        private readonly List<string> _types;
        /// <summary>
        /// 优先Class.Group,其次才是Field.Group
        /// </summary>
        private readonly HashSet<string> _groups;
        private string _tkv;

        /// <summary>
        /// 字段成员
        /// </summary>
        public FieldInfo(FieldDes des, string[] gps) : this(des, null, gps) { }
        /// <summary>
        /// 类字段成员
        /// </summary>
        public FieldInfo(FieldDes des, ClassInfo host, string[] gps)
        {
            _des = des;
            _host = host;
            _groups = new HashSet<string>(gps ?? new string[0]);

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
            ClassInfo info = ClassInfo.Get(_types[1]);
            return new FieldInfo(;
        }
        /// <summary>
        /// dict key 定义
        /// </summary>
        public FieldInfo GetKeyDefine()
        {
            return null;
        }
        /// <summary>
        /// dict value 定义
        /// </summary>
        public FieldInfo GetValueDefine()
        {
            return null;
        }
    }
}
