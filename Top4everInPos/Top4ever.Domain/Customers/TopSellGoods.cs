using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Top4ever.Domain.Customers
{
    public class TopSellGoods
    {
        /// <summary>
        /// 次数
        /// </summary>
        public int Times { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public Guid GoodsID { get; set; }

        /// <summary>
        /// 菜品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 单位售价
        /// </summary>
        public decimal SellPrice { get; set; }

        /// <summary>
        /// 能否打折
        /// </summary>
        public bool CanDiscount { get; set; }
    }
}
