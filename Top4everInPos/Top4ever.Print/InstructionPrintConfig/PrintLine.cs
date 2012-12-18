using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Print.InstructionPrintConfig
{
    [XmlRoot("Line")]
    public class PrintLine
    {
        private string lineName;
        [XmlAttribute("Name")]
        public string LineName
        {
            get { return lineName; }
            set { lineName = value; }
        }

        private string lineText;
        [XmlAttribute("Text")]
        public string LineText
        {
            get { return lineText; }
            set { lineText = value; }
        }

        private string startPosition;
        [XmlAttribute("StartPX")]
        public string StartPX
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        private string endPosition;
        [XmlAttribute("EndPX")]
        public string EndPX
        {
            get { return endPosition; }
            set { endPosition = value; }
        }
    }
}
