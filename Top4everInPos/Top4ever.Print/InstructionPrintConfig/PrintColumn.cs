using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Print.InstructionPrintConfig
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

        private string _width;
        [XmlAttribute("Width")]
        public string Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private string _fontSize;
        [XmlAttribute("Font")]
        public string FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }
    }
}
