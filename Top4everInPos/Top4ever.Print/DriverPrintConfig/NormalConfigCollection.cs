using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Print.DriverPrintConfig
{
    [XmlRoot("NormalConfigCollection")]
    public class NormalConfigCollection
    {
        private List<NormalConfig> m_NormalConfigList;

        [XmlElement("NormalConfig")]
        public List<NormalConfig> NormalConfigList
        {
            get { return m_NormalConfigList; }
            set { m_NormalConfigList = value; }
        }
    }
}
