using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Print.DriverPrintConfig
{
    public class PrintConfig
    {
        private string m_PaperType;
        private string m_Margin;
        private PrintLineCollection m_PrintLines;
        private NormalConfigCollection m_NormalConfigs;
        private DataListConfigCollection m_DataListConfigs;
        
        [XmlAttribute("PaperType")]
        public string PaperType
        {
            get { return m_PaperType; }
            set { m_PaperType = value; }
        }

        [XmlAttribute("Margin")]
        public string PaperMargin
        {
            get { return m_Margin; }
            set { m_Margin = value; }
        }

        [XmlElement("LineCollection")]
        public PrintLineCollection PrintLines
        {
            get { return m_PrintLines; }
            set { m_PrintLines = value; }
        }

        [XmlElement("NormalConfigCollection")]
        public NormalConfigCollection NormalConfigs
        {
            get { return m_NormalConfigs; }
            set { m_NormalConfigs = value; }
        }

        [XmlElement("DataListConfigCollection")]
        public DataListConfigCollection DataListConfigs
        {
            get { return m_DataListConfigs; }
            set { m_DataListConfigs = value; }
        }
    }
}
