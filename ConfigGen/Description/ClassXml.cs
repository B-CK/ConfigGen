using System.Collections.Generic;
using System.Xml.Serialization;

namespace Description.Xml
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

        [XmlElement("Field")]
        public List<FieldXml> Fields;
        [XmlElement("Const")]
        public List<ConstXml> Consts;

        //public void ToLower()
        //{
        //    Name = Name.ToLowerExt();
        //    Inherit = Inherit.ToLowerExt();
        //    Index = Index.ToLowerExt();
        //    Group = Group.ToLowerExt();

        //    for (int i = 0; i < Fields.Count; i++)
        //        Fields[i].ToLower();
        //    for (int i = 0; i < Consts.Count; i++)
        //        Consts[i].ToLower();
        //}
    }
}
