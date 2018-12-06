using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConfigGen.LocalInfo
{
    [XmlRoot("Define")]
    public class TypeDescription
    {
        [XmlAttribute]
        public string Namespace;
        [XmlElement("Class")]
        public List<ClassDes> Classes;
        [XmlElement("Enum")]
        public List<EnumDes> Enums;

        [XmlIgnore]
        public string XmlDirPath;
    }
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
    [XmlInclude(typeof(FieldDes))]
    public class FieldDes
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Type;
        [XmlAttribute]
        public string Check;
        [XmlAttribute]
        public string Des;
        [XmlAttribute]
        public string Group;
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

    [XmlRoot("Config")]
    public class ConfigXml
    {
        [XmlAttribute]
        public string Root;
        [XmlElement("Include")]
        public List<string> Include;
        [XmlElement("NoStruct")]
        public List<string> NoStruct;
    }
}
