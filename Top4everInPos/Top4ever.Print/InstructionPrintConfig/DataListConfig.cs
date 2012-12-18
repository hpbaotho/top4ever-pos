using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Print.InstructionPrintConfig
{
    public class DataListConfig
    {
        private string m_ClassName;
        [XmlAttribute("ClassName")]
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }

        private PrintColumnHeadCollection m_ColumnHeads;
        [XmlElement("ColumnHeadCollection")]
        public PrintColumnHeadCollection PrintColumnHeads
        {
            get { return m_ColumnHeads; }
            set { m_ColumnHeads = value; }
        }

        private PrintColumnCollection m_PrintColumns;
        [XmlElement("ColumnCollection")]
        public PrintColumnCollection PrintColumns
        {
            get { return m_PrintColumns; }
            set { m_PrintColumns = value; }
        }
    }
}
