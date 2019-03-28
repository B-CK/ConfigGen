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
        [XmlAttribute]
        public string Desc;

        [XmlElement("Const")]
        public List<ConstDes> Enums;

        //public void ToLower()
        //{
        //    Name = Name.ToLowerExt();
        //    Group = Group.ToLowerExt();

        //    for (int i = 0; i < Enums.Count; i++)
        //        Enums[i].ToLower();
        //}
    }
}
