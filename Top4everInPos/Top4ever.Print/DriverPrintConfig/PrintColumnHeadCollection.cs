using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Print.DriverPrintConfig
{
    [XmlRoot("ColumnHeadCollection")]
    public class PrintColumnHeadCollection
    {
        private List<PrintColumnHead> m_ColumnHeadList;

        [XmlElement("ColumnHead")]
        public List<PrintColumnHead> ColumnHeadList
        {
            get { return m_ColumnHeadList; }
            set { m_ColumnHeadList = value; }
        }
    }
}
