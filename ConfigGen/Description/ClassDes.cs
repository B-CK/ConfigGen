using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConfigGen.Description
{
    [XmlInclude(typeof(ClassDes))]
    public class ClassDes
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

        [XmlElement("Field")]
        public List<FieldDes> Fields;
        [XmlElement("Const")]
        public List<ConstDes> Consts;
    }
}
