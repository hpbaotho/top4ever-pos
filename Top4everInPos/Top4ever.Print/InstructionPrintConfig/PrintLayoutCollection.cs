using System.Collections.Generic;
using System.Xml.Serialization;

namespace Top4ever.Print.InstructionPrintConfig
{
    [XmlRoot("LayoutCollection")]
    public class PrintLayoutCollection
    {
        [XmlElement("LayoutConfig")]
        public List<PrintLayout> LayoutList { get; set; }
    }
}
