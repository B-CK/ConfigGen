using System.Collections.Generic;
using System.Xml.Serialization;

namespace Desc.Xml
{
    [XmlRoot("Module")]
    public class ModuleXml
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Groups;
        [XmlElement("Import")]
        public List<string> Imports;
    }
}
