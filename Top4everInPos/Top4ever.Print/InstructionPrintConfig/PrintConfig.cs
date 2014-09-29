using System.Xml.Serialization;

namespace Top4ever.Print.InstructionPrintConfig
{
    public class PrintConfig
    {
        [XmlAttribute("PaperType")]
        public string PaperType { get; set; }

        [XmlAttribute("TotalCharNum")]
        public int TotalCharNum { get; set; }

        [XmlElement("LayoutCollection")]
        public PrintLayoutCollection PrintLayouts { get; set; }

        [XmlElement("LineCollection")]
        public PrintLineCollection PrintLines { get; set; }

        [XmlElement("NormalConfigCollection")]
        public NormalConfigCollection NormalConfigs { get; set; }

        [XmlElement("DataListConfigCollection")]
        public DataListConfigCollection DataListConfigs { get; set; }
    }
}
