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

        private string _width;
        [XmlAttribute("Width")]
        public string Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Left Center Right;
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
