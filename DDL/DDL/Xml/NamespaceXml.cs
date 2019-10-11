using System.Collections.Generic;
using System.Xml.Serialization;

namespace DDL.Xml
{
    [XmlRoot("Namespace")]
    public class NamespaceXml
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Desc;
        [XmlElement("Class")]
        public List<ClassXml> Classes;
        [XmlElement("Enum")]
        public List<EnumXml> Enums;     
    }
}
