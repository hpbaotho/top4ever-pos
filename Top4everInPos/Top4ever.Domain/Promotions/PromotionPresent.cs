using System;

namespace Top4ever.Domain.Promotions
{
    public class PromotionPresent
    {
        /// <summary>
        /// 活动号
        /// </summary>
        public string ActivityNo { get; set; }
        /// <summary>
        /// 销售总金额
        /// </summary>
        public decimal TotalMoney { get; set; }
        /// <summary>
        /// 销售总数量
        /// </summary>
        public decimal TotalQuantity { get; set; }
        /// <summary>
        /// 类别 1:赠品, 2:折扣, 3:固定折扣
        /// </summary>
        public int Classification { get; set; }
        /// <summary>
        /// 赠品品项编号
        /// </summary>
        public Guid? GoodsID { get; set; }
        /// <summary>
        /// 赠品品项号
        /// </summary>
        public string GoodsNo { get; set; }
        /// <summary>
        /// 赠品售价
        /// </summary>
        public decimal? SellPrice { get; set; }
        /// <summary>
        /// 赠品数量
        /// </summary>
        public decimal? Quantity { get; set; }
        /// <summary>
        /// 倍数
        /// </summary>
        public bool IsMultiple { get; set; }
        /// <summary>
        /// 折扣类型
        /// </summary>
        public int DiscountType { get; set; }
        /// <summary>
        /// 折扣率
        /// </summary>
        public decimal? DiscountRate { get; set; }
        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal? OffFixPay { get; set; }
        /// <summary>
        /// 单次限定 针对折扣 一个品项折扣算一次，销售单不能超过本栏位值
        /// </summary>
        public int DiscountLimit { get; set; }
    }
}
