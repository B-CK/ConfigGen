using Description.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Description.Wrap
{
    /// <summary>
    /// 结构类型
    /// </summary>
    public abstract class TypeWrap : BaseWrap
    {
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
                if (_xml.Desc.IsEmpty())
                    return Name;
                else
                    return Util.Format("{0}:{1}", Name, _xml.Desc);
            }
        }
        public override string FullName { get { return _namespace == null ? null : Util.Format("{0}.{1}", _namespace.Name, Name); } }
        public NamespaceWrap Namespace { get { return _namespace; } set { _namespace = value; } }
        protected NamespaceWrap _namespace;
        protected TypeXml _xml;
        protected TypeWrap(TypeXml xml, NamespaceWrap ns)
        {
            Init(xml, ns);
        }
        protected virtual void Init(TypeXml xml, NamespaceWrap ns)
        {
            base.Init(xml.Name);
            _xml = xml;
            _namespace = ns;
        }
        public void BreakParent()
        {
            Namespace.RemoveTypeWrap(this);
        }
    }
}
