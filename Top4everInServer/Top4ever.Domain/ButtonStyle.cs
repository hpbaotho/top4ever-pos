using System;
using System.Collections.Generic;

namespace Top4ever.Domain
{
    public class ButtonStyle
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ButtonStyleID { get; set; }
        /// <summary>
        /// 背景色
        /// </summary>
        public int BackColor { get; set; }
        /// <summary>
        /// 点击后的颜色
        /// </summary>
        public int ClickedBackColor { get; set; }
        /// <summary>
        /// Hover的颜色
        /// </summary>
        public int HoverBackColor { get; set; }
        /// <summary>
        /// 字体名称
        /// </summary>
        public string FontName { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize { get; set; }
        /// <summary>
        /// 字体类别
        /// </summary>
        public string FontStyle { get; set; }
        /// <summary>
        /// 字体颜色
        /// </summary>
        public int ForeColor { get; set; }
    }
}
