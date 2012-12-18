using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Top4ever.Print.InstructionPrintConfig;

namespace Top4ever.Print
{
    [XmlRoot("PrintSetting")]
    public class InstructionOrderPrintSetting
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
