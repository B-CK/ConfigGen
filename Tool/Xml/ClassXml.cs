using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    [XmlInclude(typeof(ClassXml))]
    public class ClassXml
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Inherit;
        [XmlAttribute]
        public string Index;
        [XmlAttribute]
        public string DataPath;
        [XmlAttribute]
        public string Group;
        [XmlAttribute]
        public string Desc;
        [XmlAttribute]
        public string Attribute;

        [XmlElement("Field")]
        public List<FieldXml> Fields;
        [XmlElement("Const")]
        public List<ConstXml> Consts;
    }
}
