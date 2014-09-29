using System.Collections.Generic;
using System.Xml.Serialization;

namespace Top4ever.Print.DriverPrintConfig
{
    [XmlRoot("LayoutCollection")]
    public class PrintLayoutCollection
    {
        [XmlElement("LayoutConfig")]
        public List<PrintLayout> LayoutList { get; set; }
    }
}
