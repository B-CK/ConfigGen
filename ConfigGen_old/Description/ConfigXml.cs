using System.Collections.Generic;
using System.Xml.Serialization;

namespace Description.Xml
{
    [XmlRoot("Config")]
    public class ConfigXml
    {
        [XmlAttribute]
        public string Root;
        [XmlElement("Group")]
        public string Group;
        [XmlElement("Import")]
        public List<string> Import;
    }
}
