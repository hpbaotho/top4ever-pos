using System;
using System.Collections.Generic;

namespace Top4ever.Domain.OrderRelated
{
    public class Discount
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid DiscountID { get; set; }
        /// <summary>
        /// 折扣名称
        /// </summary>
        public string DiscountName { get; set; }
        /// <summary>
        /// 折扣名称第二语言
        /// </summary>
        public string DiscountName2nd { get; set; }
        /// <summary>
        /// 折扣类别
        /// </summary>
        public int DiscountType { get; set; }
        /// <summary>
        /// 折扣率
        /// </summary>
        public decimal DiscountRate { get; set; }
        /// <summary>
        /// 满足金额
        /// </summary>
        public decimal MinQuotas { get; set; }
        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal OffFixPay { get; set; }
        /// <summary>
        /// 显示模式 0:全部显示 1:单品折扣 2:整单折扣
        /// </summary>
        public int DisplayModel { get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public Guid ButtonStyleID { get; set; }
    }
}
