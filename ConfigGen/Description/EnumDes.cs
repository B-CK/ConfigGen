using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConfigGen.Description
{
    [XmlInclude(typeof(EnumDes))]
    public class EnumDes
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Group;

        [XmlElement("Const")]
        public List<ConstDes> Enums;
    }
}
