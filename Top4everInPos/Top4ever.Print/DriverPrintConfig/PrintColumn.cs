using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Print.DriverPrintConfig
{
    [XmlRoot("Column")]
    public class PrintColumn
    {
        private string _name;
        [XmlAttribute("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
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

        /// <summary>
        /// Text align is right or not
        /// </summary>
        private bool _IsRight;
        [XmlAttribute("IsRight")]
        public bool IsRight
        {
            get { return _IsRight; }
            set { _IsRight = value; }
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
