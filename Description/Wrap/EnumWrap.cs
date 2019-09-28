using System;
using Description.Xml;
using System.Collections.Generic;

namespace Description.Wrap
{
    public class EnumWrap : TypeWrap, IDisposable
    {
        public static EnumWrap[] Array
        {
            get
            {
                if (_array.Length != _dict.Count)
                {
                    var ls = new List<EnumWrap>(_dict.Values);
                    ls.Sort((a, b) => Comparer<string>.Default.Compare(a.DisplayName, b.DisplayName));
                    _array = ls.ToArray();
                }
                return _array;
            }
        }
        public static Dictionary<string, EnumWrap> Dict { get { return _dict; } }
        static Dictionary<string, EnumWrap> _dict = new Dictionary<string, EnumWrap>();
        static EnumWrap[] _array = new EnumWrap[] { };

        public static EnumWrap Create(string name, NamespaceWrap nsw)
        {
            EnumXml xml = new EnumXml() { Name = name };
            return Create(xml, nsw);
        }
        public static EnumWrap Create(EnumXml xml, NamespaceWrap nsw)
        {
            var wrap = PoolManager.Ins.Pop<EnumWrap>();
            if (wrap == null)
                wrap = new EnumWrap(xml, nsw);
            else
                wrap.Init(xml, nsw);
            nsw.AddTypeWrap(wrap, false);
            return wrap;
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
        //public string Inherit { get { return _xml.Inherit; } set { _xml.Inherit = value; } }
        public string Group { get { return _xml.Group; } set { _xml.Group = value; } }
        public string Desc { get { return _xml.Desc; } set { _xml.Desc = value; } }
        public List<EnumItemWrap> Items { get { return _items; } }


        private EnumXml _xml;
        //private NamespaceWrap _namespace;
        private List<EnumItemWrap> _items;
        protected EnumWrap(EnumXml xml, NamespaceWrap ns) : base(xml, ns)
        {
            Init(xml, ns);
        }
        protected void Init(EnumXml xml, NamespaceWrap ns)
        {
            base.Init(xml, ns);
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
                Add(item.Name);
            }

            _items.Sort((a, b) => a.Value - b.Value);
            _dict.Add(FullName, this);
        }

        public bool AddItem(EnumItemWrap wrap)
        {
            if (Contains(wrap.Name))
            {
                Util.MsgWarning("枚举{0}已经包含{1}.", Name, wrap.Name);
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
        public void OverrideField(EnumItemWrap wrap)
        {
            EnumItemWrap old = null;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Name == wrap.Name)
                {
                    old = wrap;
                    break;
                }
            }
            if (old != null)
                RemoveItem(old);
            AddItem(wrap);
        }
        public override void Dispose()
        {
            base.Dispose();
            for (int i = 0; i < _items.Count; i++)
                _items[i].Dispose();
            _items.Clear();
            _dict.Remove(FullName);
            PoolManager.Ins.Push(this);
        }

        public static implicit operator EnumXml(EnumWrap wrap)
        {
            return wrap._xml;
        }
    }
}
