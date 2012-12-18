using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

using Top4ever.Print.DriverPrintConfig;

namespace Top4ever.Print
{
    [XmlRoot("PrintSetting")]
    public class DriverPrintSetting
    {
        private List<PrintConfig> _printConfigs;
        [XmlElement("PrintConfig")]
        public List<PrintConfig> PrintConfigs
        {
            get { return _printConfigs; }
            set { _printConfigs = value; }
        }
    }
}
