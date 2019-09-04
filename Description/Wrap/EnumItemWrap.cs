using Description.Xml;

namespace Description.Wrap
{
    public class EnumItemWrap : BaseWrap
    {
        public static EnumItemWrap Create(string name, EnumWrap enum0)
        {
            EnumItemXml xml = new EnumItemXml() { Name = name };
            return Create(xml, enum0);
        }
        public static EnumItemWrap Create(EnumItemXml xml, EnumWrap enum0)
        {
            var wrap = PoolManager.Ins.Pop<EnumItemWrap>();
            return wrap ?? new EnumItemWrap(xml, enum0);
        }

        public int Value { get { return _xml.Value; } set { _xml.Value = value; } }
        public string Alias { get { return _xml.Alias; } set { _xml.Alias = value; } }

        private EnumWrap _enum;
        private EnumItemXml _xml;
        protected EnumItemWrap(EnumItemXml xml, EnumWrap enum0) : base(xml.Name)
        {
            _xml = xml;
            _enum = enum0;
        }

        public static implicit operator EnumItemXml(EnumItemWrap wrap)
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
