using System;
using Description.Xml;
using System.Collections.Generic;

namespace Description.Wrap
{
    public class EnumWrap : TypeWrap, IDisposable
    {
        public static EnumWrap[] Enums
        {
            get
            {
                if (_enums.Length != _enumDict.Count)
                {
                    var ls = new List<EnumWrap>(_enumDict.Values);
                    ls.Sort((a, b) => Comparer<string>.Default.Compare(a.DisplayName, b.DisplayName));
                    _enums = ls.ToArray();
                }
                return _enums;
            }
        }
        public static Dictionary<string, EnumWrap> EnumDict { get { return _enumDict; } }
        static Dictionary<string, EnumWrap> _enumDict = new Dictionary<string, EnumWrap>();
        static EnumWrap[] _enums = new EnumWrap[] { };

        public static EnumWrap Create(string name, NamespaceWrap ns)
        {
            EnumXml xml = new EnumXml() { Name = name };
            return Create(xml, ns);
        }
        public static EnumWrap Create(EnumXml xml, NamespaceWrap ns)
        {
            var wrap = PoolManager.Ins.Pop<EnumWrap>();
            if (wrap == null)
                wrap = new EnumWrap(xml, ns);
            else
                wrap.Init(xml, ns);
            return wrap ?? new EnumWrap(xml, ns);
        }

        //public override string FullName { get { return Util.Format("{0}.{1}", _namespace.Name, Name); } }
        //public NamespaceWrap Namespace { get { return _namespace; } set { _namespace = value; } }

        public string Inhert { get { return _xml.Inherit; } set { _xml.Inherit = value; } }
        public string Desc { get { return _xml.Desc; } set { _xml.Desc = value; } }
        public List<EnumItemWrap> Items { get { return _items; } }

        public override string DisplayName
        {
            get
            {
                if (Desc.IsEmpty())
                    return Name;
                else
                    return Util.Format("{0}:{1}", Name, Desc);
            }
        }


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
            _enumDict.Add(FullName, this);
        }

        public bool AddEItem(EnumItemWrap wrap)
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
        public override void Dispose()
        {
            base.Dispose();
            for (int i = 0; i < _items.Count; i++)
                _items[i].Dispose();
            _items.Clear();
            _enumDict.Remove(FullName);
            PoolManager.Ins.Push(this);
        }

        public static implicit operator EnumXml(EnumWrap wrap)
        {
            return wrap._xml;
        }
    }
}
