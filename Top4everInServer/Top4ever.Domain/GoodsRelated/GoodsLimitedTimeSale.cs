using System;

namespace Top4ever.Domain.GoodsRelated
{
    public class GoodsLimitedTimeSale
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
        /// 折扣类型
        /// </summary>
        public int DiscountType { get; set; }
        /// <summary>
        /// 折扣率
        /// </summary>
        public decimal DiscountRate { get; set; }
        /// <summary>
        /// 固定折扣
        /// </summary>
        public decimal OffFixPay { get; set; }
        /// <summary>
        /// 折扣到
        /// </summary>
        public decimal OffSaleTo { get; set; }
    }
}
