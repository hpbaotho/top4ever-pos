using System.Xml.Serialization;

namespace Top4ever.Print.InstructionPrintConfig
{
    [XmlRoot("LayoutConfig")]
    public class PrintLayout
    {
        [XmlElement("Key")]
        public string Key { get; set; }

        [XmlElement("Value")]
        public string Value { get; set; }
    }
}
