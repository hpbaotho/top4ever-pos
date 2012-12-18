using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Print.DriverPrintConfig
{
    [XmlRoot("ColumnCollection")]
    public class PrintColumnCollection
    {
        private List<PrintColumn> m_ColumnList;

        [XmlElement("Column")]
        public List<PrintColumn> PrintColumnList
        {
            get { return m_ColumnList; }
            set { m_ColumnList = value; }
        }
    }
}
