﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    [XmlInclude(typeof(EnumXml))]
    public class EnumXml : TypeXml
    {
        //[XmlAttribute]
        //public string Inherit;
        [XmlElement("Item")]
        public List<EnumItemXml> Items;
    }
}
