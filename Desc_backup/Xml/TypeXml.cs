using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Desc.Xml
{
    [XmlInclude(typeof(TypeXml))]
    public abstract class TypeXml
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Group;
        [XmlAttribute]
        public string Desc;
    }
}
