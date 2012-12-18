using System;
using System.Collections.Generic;

namespace Top4ever.Domain.GoodsRelated
{
    public class GoodsSetMeal
    {
        /// <summary>
        /// 套餐的GoodsID
        /// </summary>
        public Guid GoodsSetMealID { get; set; }

        /// <summary>
        /// 套餐细项的GoodsID
        /// </summary>
        public Guid GoodsID { get; set; }

        /// <summary>
        /// 套餐细项的父级
        /// </summary>
        public Guid ParentGoodsID { get; set; }

        /// <summary>
        /// 套餐细项的数量
        /// </summary>
        public decimal ItemQty { get; set; }

        /// <summary>
        /// 套餐细项的折扣率
        /// </summary>
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// 套餐细项的折扣额
        /// </summary>
        public decimal OffFixPay { get; set; }
    }
}
