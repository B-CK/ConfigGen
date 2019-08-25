using System.Collections.Generic;
using System.Xml.Serialization;

namespace Description.Xml
{
    [XmlInclude(typeof(EnumXml))]
    public class EnumXml
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Group;
        [XmlAttribute]
        public string Desc;

        [XmlElement("Const")]
        public List<ConstXml> Enums;
    }
}
