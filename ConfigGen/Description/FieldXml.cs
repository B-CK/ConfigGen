using System.Xml.Serialization;

namespace Description.Xml
{
    [XmlInclude(typeof(FieldXml))]
    public class FieldXml
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
        public string Ref;//引用字段
        [XmlAttribute]
        public string RefPath;//引用资源相对路径
    }

    [XmlInclude(typeof(ConstXml))]
    public class ConstXml : FieldXml
    {
        [XmlAttribute]
        public string Value;
        [XmlAttribute]
        public string Alias;
    }
}
