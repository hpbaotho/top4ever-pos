using System;
using System.Collections.Generic;
using System.Text;

namespace Top4ever.Print.Entity
{
    public class GoodsOrder
    {
        /// <summary>
        /// 菜品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string GoodsNum { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 单位售价
        /// </summary>
        public string SellPrice { get; set; }

        /// <summary>
        /// 售价总计
        /// </summary>
        public string TotalSellPrice { get; set; }

        /// <summary>
        /// 折扣总计
        /// </summary>
        public string TotalDiscount { get; set; }
    }
}
