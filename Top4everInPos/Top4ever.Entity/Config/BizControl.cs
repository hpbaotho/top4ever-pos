using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Top4ever.Entity
{
    public class BizControl
    {
        /// <summary>
        /// 控制所属名称
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }
        /// <summary>
        /// 横向个数
        /// </summary>
        [XmlAttribute("ColumnsCount")]
        public int ColumnsCount { get; set; }
        /// <summary>
        /// 纵向个数
        /// </summary>
        [XmlAttribute("RowsCount")]
        public int RowsCount { get; set; }
    }
}
