using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Top4ever.Entity.Enum;

namespace Top4ever.Entity.Config
{
    public class PrintConfig
    {
        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }

        [XmlElement("PortType")]
        public PortType PrinterPort { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("VID")]
        public string VID { get; set; }

        [XmlElement("PID")]
        public string PID { get; set; }

        [XmlElement("Copies")]
        public int Copies { get; set; }

        [XmlElement("PaperWidth")]
        public string PaperWidth { get; set; }
    }
}
