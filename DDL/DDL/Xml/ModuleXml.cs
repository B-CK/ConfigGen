using System.Collections.Generic;
using System.Xml.Serialization;

namespace DDL.Xml
{
    [XmlRoot("Module")]
    public class ModuleXml
    {
        [XmlAttribute]
        public string Name;
        [XmlElement("Import")]
        public List<string> Imports;
    }
}
