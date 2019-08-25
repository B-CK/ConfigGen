using System.Collections.Generic;
using System.Xml.Serialization;

namespace Description.Xml
{
    [XmlRoot("Module")]
    public class ModuleXml
    {
        [XmlAttribute]
        public string Name;
        [XmlElement("Groups")]
        public string Groups;
        [XmlElement("Import")]
        public List<string> Imports;
    }
}
