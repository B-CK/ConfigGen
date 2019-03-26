using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConfigGen.Description
{
    [XmlRoot("Config")]
    public class ConfigXml
    {
        [XmlAttribute]
        public string Root;
        [XmlElement("Include")]
        public List<string> Include;
        [XmlElement("Nonstreaming")]
        public List<string> Nonstreaming;
    }
}
