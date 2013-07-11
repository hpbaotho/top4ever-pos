using System;

namespace Top4ever.Domain.GoodsRelated
{
    public class GoodsCombinedSale
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 周
        /// </summary>
        public string Week { get; set; }
        /// <summary>
        /// 日
        /// </summary>
        public string Day { get; set; }
        /// <summary>
        /// 小时
        /// </summary>
        public string Hour { get; set; }
        /// <summary>
        /// 分钟
        /// </summary>
        public string Minute { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ItemID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ItemType { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 售价
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
        /// <summary>
        /// 优惠区间 第二件半价 第三件免费
        /// </summary>
        public int PreferentialInterval { get; set; }
        /// <summary>
        /// 倍数
        /// </summary>
        public bool IsMultiple { get; set; }
        /// <summary>
        /// 折扣类型2
        /// </summary>
        public int DiscountType2 { get; set; }
        /// <summary>
        /// 折扣率2
        /// </summary>
        public decimal DiscountRate2 { get; set; }
        /// <summary>
        /// 固定折扣2
        /// </summary>
        public decimal OffFixPay2 { get; set; }
        /// <summary>
        /// 折扣到2
        /// </summary>
        public decimal OffSaleTo2 { get; set; }
        /// <summary>
        /// 折扣类型3
        /// </summary>
        public int DiscountType3 { get; set; }
        /// <summary>
        /// 折扣率3
        /// </summary>
        public decimal DiscountRate3 { get; set; }
        /// <summary>
        /// 固定折扣3
        /// </summary>
        public decimal OffFixPay3 { get; set; }
        /// <summary>
        /// 折扣到3
        /// </summary>
        public decimal OffSaleTo3 { get; set; }
    }
}