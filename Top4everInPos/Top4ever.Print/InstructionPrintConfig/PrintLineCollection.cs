using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Print.InstructionPrintConfig
{
    [XmlRoot("LineCollection")]
    public class PrintLineCollection
    {
        private List<PrintLine> m_LineList;

        [XmlElement("Line")]
        public List<PrintLine> LineList
        {
            get { return m_LineList; }
            set { m_LineList = value; }
        }
    }
}
