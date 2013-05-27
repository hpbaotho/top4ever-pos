using System;

namespace Top4ever.Domain.Promotions
{
    public class PromotionCondition
    {
        /// <summary>
        /// 活动号
        /// </summary>
        public string ActivityNo { get; set; }
        /// <summary>
        /// 组编号
        /// </summary>
        public Guid? GoodsGroupID { get; set; }
        /// <summary>
        /// 品项编号
        /// </summary>
        public Guid? GoodsID { get; set; }
        /// <summary>
        /// 组或者品项
        /// </summary>
        public bool GroupOrItem { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 单品金额
        /// </summary>
        public decimal SellPrice { get; set; }
        /// <summary>
        /// 金额大小 1:以上, 2:等于, 3:以下
        /// </summary>
        public int MoreOrLess { get; set; }
        /// <summary>
        /// 单品折扣率 以上
        /// </summary>
        public decimal LeastDiscountRate { get; set; }
    }
}
