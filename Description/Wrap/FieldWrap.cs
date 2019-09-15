using Description.Xml;

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

        public string Type { get { return _xml.Type; } set { _xml.Type = value; } }
        public bool IsConst { get { return _xml.IsConst; } set { _xml.IsConst = value; } }
        /// <summary>
        /// 只有常量才有值
        /// </summary>
        public string Value { get { return _xml.Value; } set { _xml.Value = value; } }
        public string Group { get { return _xml.Group; } set { _xml.Group = value; } }
        public string Desc { get { return _xml.Desc; } set { _xml.Desc = value; } }
        public string Checker { get { return _xml.Checker; } set { _xml.Checker = value; } }

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

        private ClassWrap _cls;
        private FieldXml _xml;
        protected FieldWrap(FieldXml xml, ClassWrap cls) : base(xml.Name)
        {
            Init(xml, cls);
        }
        private void Init(FieldXml xml, ClassWrap cls)
        {
            _xml = xml;
            _cls = cls;
        }

        public static implicit operator FieldXml(FieldWrap wrap)
        {
            return wrap._xml;
        }
        public override void Dispose()
        {
            base.Dispose();
            PoolManager.Ins.Push(this);
        }
    }
}
