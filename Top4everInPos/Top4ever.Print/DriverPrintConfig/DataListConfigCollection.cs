using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Print.DriverPrintConfig
{
    [XmlRoot("DataListConfigCollection")]
    public class DataListConfigCollection
    {
        private List<DataListConfig> m_DataListConfigs;

        [XmlElement("DataListConfig")]
        public List<DataListConfig> DataListConfigList
        {
            get { return m_DataListConfigs; }
            set { m_DataListConfigs = value; }
        }
    }
}
