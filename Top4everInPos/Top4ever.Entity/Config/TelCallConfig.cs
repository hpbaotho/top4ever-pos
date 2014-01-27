using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Entity.Config
{
    public class TelCallConfig
    {
        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// 0:纽曼NM-LD-U新版 1:纽曼NM-LD-U
        /// </summary>
        [XmlElement("Model")]
        public int Model { get; set; }
    }
}
