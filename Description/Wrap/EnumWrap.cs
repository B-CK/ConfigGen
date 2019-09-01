using System;
using Description.Xml;
using System.Collections.Generic;

namespace Description.Wrap
{
    public class EnumWrap : BaseWrap, IDisposable
    {
        public static EnumWrap Create(string name, NamespaceWrap ns)
        {
            EnumXml xml = new EnumXml() { Name = name };
            return Create(xml, ns);
        }
        public static EnumWrap Create(EnumXml xml, NamespaceWrap ns)
        {
            var wrap = PoolManager.Ins.Pop<EnumWrap>();
            return wrap ?? new EnumWrap(xml, ns);
        }

        public override string FullName { get { return Util.Format("{0}.{1}", _namespace.Name, Name); } }
        public NamespaceWrap Parent { get { return _namespace; } set { _namespace = value; } }
        public string Namespace { get { return _namespace.Name; } }
        public string Desc { get { return _xml.Desc; } set { _xml.Desc = value; } }
        public string Group { get { return _xml.Group; } set { _xml.Group = value; } }

        private EnumXml _xml;
        private NamespaceWrap _namespace;
        private List<EnumItemWrap> _items;
        protected EnumWrap(EnumXml xml, NamespaceWrap ns) : base(xml.Name)
        {
            _xml = xml;
            _namespace = ns;
            _items = new List<EnumItemWrap>();
            _xml.Items = _xml.Items ?? new List<EnumItemXml>();
            var xitems = _xml.Items;
            for (int i = 0; i < xitems.Count; i++)
            {
                var xitem = xitems[i];
                var item = EnumItemWrap.Create(xitem, this);
                _items.Add(item);
            }
        }

        public bool AddEItem(EnumItemWrap wrap)
        {
            if (Contains(wrap.Name))
            {
                Util.MsgError("错误", "枚举{0}已经包含{1}.", Name, wrap.Name);
                return false;
            }

            Add(wrap.Name);
            _items.Add(wrap);
            _items.Sort((a, b) => a.Value - b.Value);
            _xml.Items.Add(wrap);
            return true;
        }
        public void RemoveItem(EnumItemWrap wrap)
        {
            if (!Contains(wrap.Name)) return;

            Remove(wrap.Name);
            _items.Remove(wrap);
            _xml.Items.Remove(wrap);
        }
        public override void Dispose()
        {
            base.Dispose();
            for (int i = 0; i < _items.Count; i++)
                _items[i].Dispose();
            _items.Clear();
        }
        public static implicit operator EnumXml(EnumWrap wrap)
        {
            return wrap._xml;
        }
    }
}
