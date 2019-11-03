using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("Namespace")]
    public class NamespaceXml
    {
        [XmlAttribute]
        public string Name;
        [XmlElement("Class")]
        public List<ClassXml> Classes;
        [XmlElement("Enum")]
        public List<EnumXml> Enums;

        [XmlIgnore]
        public string XmlDir;
    }
}
