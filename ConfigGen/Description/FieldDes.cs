using System.Xml.Serialization;

namespace ConfigGen.Description
{
    [XmlInclude(typeof(FieldDes))]
    public class FieldDes
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Type;
        [XmlAttribute]
        public string Desc;
        [XmlAttribute]
        public string Group;
        [XmlAttribute]
        public string Check;

        //[XmlAttribute]
        //public string Split;
    }

    [XmlInclude(typeof(ConstDes))]
    public class ConstDes : FieldDes
    {
        [XmlAttribute]
        public string Value;
        [XmlAttribute]
        public string Alias;
    }
}
