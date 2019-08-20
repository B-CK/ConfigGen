using System.Xml.Serialization;

namespace ConfigGen.Description
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


        //[XmlAttribute]
        //public string Split;

        //public void ToLower()
        //{
        //    Name = Name.ToLowerExt();
        //    Type = Type.ToLowerExt();
        //    Check = Check.ToLowerExt();
        //}
    }

    [XmlInclude(typeof(ConstDes))]
    public class ConstDes : FieldXml
    {
        [XmlAttribute]
        public string Value;
        [XmlAttribute]
        public string Alias;
    }
}
