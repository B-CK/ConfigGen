using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    [XmlInclude(typeof(ClassXml))]
    public class ClassXml : TypeXml
    {
        [XmlAttribute]
        public string Inherit;
        [XmlAttribute]
        public string Index;
        [XmlAttribute]
        public string DataPath;

        [XmlElement("Field")]
        public List<FieldXml> Fields;
    }
}
