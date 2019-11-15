using System.Xml.Serialization;

namespace Xml
{
    [XmlInclude(typeof(MemberXml))]
    public abstract class MemberXml
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Desc;
        [XmlAttribute]
        public string Group;
    }

    [XmlInclude(typeof(FieldXml))]
    public class FieldXml : MemberXml
    {
        [XmlAttribute]
        public string Type;
        [XmlAttribute]
        public string Ref;//引用字段
        [XmlAttribute]
        public string RefPath;//引用资源相对路径
    }
    [XmlInclude(typeof(EnumItemXml))]
    public class EnumItemXml : MemberXml
    {
        [XmlAttribute]
        public int Value;
        [XmlAttribute]
        public string Alias;
    }
}
