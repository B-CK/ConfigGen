using Description.Xml;
using System;

namespace Description.Wrap
{
    public class FieldWrap : BaseWrap
    {
        public static FieldWrap Create(string name, ClassWrap cls)
        {
            FieldXml xml = new FieldXml() { Name = name };
            return Create(xml, cls);
        }
        public static FieldWrap Create(FieldXml xml, ClassWrap cls)
        {
            var wrap = PoolManager.Ins.Pop<FieldWrap>();
            if (wrap == null)
                wrap = new FieldWrap(xml, cls);
            else
                wrap.Init(xml, cls);
            return wrap;
        }
        public static implicit operator FieldXml(FieldWrap wrap)
        {
            return wrap._xml;
        }

        public virtual string DisplayFullName
        {
            get
            {
                if (_xml.Desc.IsEmpty())
                    return FullName;
                else
                    return Util.Format("{0}:{1}", FullName, _xml.Desc);
            }
        }
        public override string DisplayName
        {
            get
            {
                if (Value.IsEmpty())
                    return Util.Format("{0}:{1}", _name, Type);
                else
                    return Util.Format("{0}:{1}={2}", _name, Type, Value);
            }
        }
        public override string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                _xml.Name = value;
            }
        }
        public string Type { get { return _xml.Type; } set { _xml.Type = value; } }
        public bool IsConst { get { return _xml.IsConst; } set { _xml.IsConst = value; } }
        /// <summary>
        /// 既可作常量值,也可作默认值
        /// </summary>
        public string Value { get { return _xml.Value; } set { _xml.Value = value; } }
        public string Group { get { return _xml.Group; } set { _xml.Group = value; } }
        public string Desc { get { return _xml.Desc; } set { _xml.Desc = value; } }
        public string Checker { get { return _xml.Checker; } set { _xml.Checker = value; } }



        private ClassWrap _cls;
        private FieldXml _xml;
        protected FieldWrap(FieldXml xml, ClassWrap cls)
        {
            Init(xml, cls);
        }
        private void Init(FieldXml xml, ClassWrap cls)
        {
            base.Init(xml.Name);

            _xml = xml;
            _cls = cls;
        }
        public override bool Check()
        {
            bool r = base.Check();
            string[] nodes = Type.Split(Util.ArgsSplitFlag, StringSplitOptions.RemoveEmptyEntries);
            switch (nodes.Length)
            {
                case 1:
                    r &= CheckType(nodes[0]);
                    break;
                case 2:
                    r &= CheckType(nodes[0]);
                    r &= CheckType(nodes[1], true);
                    break;
                case 3:
                    r &= CheckType(nodes[0]);
                    r &= CheckType(nodes[1], true);
                    r &= CheckType(nodes[2], true);
                    break;
                case 0:
                default:
                    r = false;
                    Debug.LogErrorFormat("[Class]类型{0}中字段{1}的类型异常[{2}]", _cls.FullName, _name, Type);
                    break;
            }
            return r;
        }
        private bool CheckType(string type, bool isSubType = false)
        {
            bool isOK = Util.HasType(type);
            if (isOK == false)
                Debug.LogErrorFormat("[Class]类型{0}中字段{1}的类型异常[{2}]![{3}]类型不存在.", _cls.FullName, _name, Type, type);
            if (EnumWrap.Dict.ContainsKey(type) && !isSubType)
            {
                var enm = EnumWrap.Dict[type];
                var c = enm.Contains(Value);
                if (c == false)
                    Debug.LogErrorFormat("[Class]类型{0}中字段{1}的枚举值{2}.[{3}]不存在!.", _cls.FullName, _name, Type, Value);
                isOK &= c;
            }
            if (!Util.BaseHash.Contains(type))
            {
                bool d = ModuleWrap.Current.CheckType(type);
                isOK &= d;
                if (d == false)
                    Debug.LogErrorFormat("[Class]{0}模块中不包含{1}类型{2}字段的类型{3}!",
                        ModuleWrap.Current.Name, _cls.FullName, _name, type);
            }
            return isOK;
        }
        public override void Dispose()
        {
            base.Dispose();
            PoolManager.Ins.Push(this);
        }
    }
}
