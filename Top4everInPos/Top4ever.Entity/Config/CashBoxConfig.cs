using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Entity.Config
{
    public class CashBoxConfig
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
    }
}
