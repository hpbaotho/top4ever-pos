using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Print.DriverPrintConfig
{
    public class NormalConfig
    {
        private string _name;
        [XmlAttribute("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _prefix;
        [XmlAttribute("Prefix")]
        public string ValuePrefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        /// <summary>
        /// Left Middle Right; x%; xx
        /// </summary>
        private string _align;
        [XmlAttribute("Align")]
        public string Align
        {
            get { return _align; }
            set { _align = value; }
        }

        private PrintFont font;
        [XmlElement("Font")]
        public PrintFont Font
        {
            get { return font; }
            set { font = value; }
        }
    }
}
