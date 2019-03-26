using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConfigGen.Description
{
    [XmlRoot("Namespace")]
    public class NamespaceDes
    {
        [XmlAttribute]
        public string Name;
        [XmlElement("Class")]
        public List<ClassDes> Classes;
        [XmlElement("Enum")]
        public List<EnumDes> Enums;

        [XmlIgnore]
        public string XmlDirPath;
    }
}
