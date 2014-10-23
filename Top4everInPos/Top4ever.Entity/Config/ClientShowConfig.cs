using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Entity.Config
{
    public class ClientShowConfig
    {
        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }

        [XmlElement("IsUsbPort")]
        public bool IsUsbPort { get; set; }

        [XmlElement("Port")]
        public string Port { get; set; }

        [XmlElement("VID")]
        public string VID { get; set; }

        [XmlElement("PID")]
        public string PID { get; set; }

        [XmlElement("EndpointID")]
        public string EndpointID { get; set; }

        [XmlElement("ClientShowModel")]
        public string ClientShowModel { get; set; }

        [XmlElement("ClientShowType")]
        public string ClientShowType { get; set; }
    }
}
