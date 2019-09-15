using System;
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
            if (wrap == null)
                wrap = new EnumItemWrap(xml, enum0);
            else
                wrap.Init(xml, enum0);
            return wrap;
        }



        public int Value { get { return _xml.Value; } set { _xml.Value = value; } }
        public string Alias { get { return _xml.Alias; } set { _xml.Alias = value; } }

        public override string DisplayName => Util.Format("{0}({1}) = {2}", Name, Alias, Value);

        private EnumWrap _enum;
        private EnumItemXml _xml;
        protected EnumItemWrap(EnumItemXml xml, EnumWrap enum0) : base(xml.Name)
        {
            Init(xml, enum0);
        }
        private void Init(EnumItemXml xml, EnumWrap enum0)
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
