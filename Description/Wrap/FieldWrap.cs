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
            return wrap ?? new FieldWrap(xml, cls);
        }

        public string Type { get { return _xml.Type; } set { _xml.Type = value; } }
        public bool IsPublic { get { return _xml.IsPublic; } set { _xml.IsPublic = value; } }
        public bool IsStatic { get { return _xml.IsStatic; } set { _xml.IsStatic = value; } }
        public bool IsReadonly { get { return _xml.IsReadonly; } set { _xml.IsReadonly = value; } }
        /// <summary>
        /// 只有常量才有值
        /// </summary>
        public string Value { get { return _xml.Value; } set { _xml.Value = value; } }
        public string Checker { get { return _xml.Checker; } set { _xml.Checker = value; } }


        private ClassWrap _cls;
        private FieldXml _xml;
        protected FieldWrap(FieldXml xml, ClassWrap cls) : base(xml.Name)
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
        public override string ToString()
        {
            if (Value.IsEmpty())
                return Util.Format("{0}:{1}", _name, Type);
            else
                return Util.Format("{0}:{1}={2}", _name, Type, Value);
        }
    }
}
