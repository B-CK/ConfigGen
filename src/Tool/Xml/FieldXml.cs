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
        
        //---检查规则---
        [XmlAttribute(DataType = "string")]
        public string Ref;//引用字段
        [XmlAttribute]
        public string File;//引用资源相对路径
        [XmlAttribute]
        public string Unique;//数据唯一性
        [XmlAttribute]
        public string NotEmpty;//字符串非空检查
        [XmlAttribute]
        public string Range;//数值范围检查
    }
    [XmlInclude(typeof(EnumItemXml))]
    public class EnumItemXml : MemberXml
    {
        [XmlAttribute]
        public int Value;
        [XmlAttribute]
        public string Alias;
    }
    [XmlInclude(typeof(ConstXml))]
    public class ConstXml : MemberXml
    {
        [XmlAttribute]
        public string Type;//支持int,float,string,bool,long
        [XmlAttribute]
        public string Value;
    }
}
