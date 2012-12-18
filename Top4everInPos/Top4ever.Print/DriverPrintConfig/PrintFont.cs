using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Print.DriverPrintConfig
{
    [XmlRoot("Font")]
    public class PrintFont
    {
        private string fontName;
        [XmlElement("FontName")]
        public string FontName
        {
            get { return fontName; }
            set { fontName = value; }
        }

        private float fontSize;
        [XmlElement("FontSize")]
        public float FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        private FontStyle fontStyle;
        [XmlElement("FontStyle")]
        public FontStyle FontStyle
        {
            get { return fontStyle; }
            set { fontStyle = value; }
        }

        private string foreColor;
        [XmlElement("ForeColor")]
        public string ForeColor
        {
            get { return foreColor; }
            set { foreColor = value; }
        }
    }
}
