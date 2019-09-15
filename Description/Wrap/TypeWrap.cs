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
        public override string FullName { get { return Util.Format("{0}.{1}", _namespace.Name, Name); } }
        public NamespaceWrap Namespace { get { return _namespace; } set { _namespace = value; } }
        protected NamespaceWrap _namespace;
        protected TypeWrap(TypeXml xml, NamespaceWrap ns) : base(xml.Name) {
            Init(xml, ns);
        }
        protected virtual void Init(TypeXml xml, NamespaceWrap ns)
        {
            _namespace = ns;
        }
    }
}
