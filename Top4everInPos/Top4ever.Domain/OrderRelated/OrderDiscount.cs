using System;
using System.Collections.Generic;

namespace Top4ever.Domain.OrderRelated
{
    [Serializable]
    public class OrderDiscount
    {
        /// <summary>
        /// 单子折扣ID
        /// </summary>
        public Guid OrderDiscountID { get; set; }

        /// <summary>
        /// 单子ID
        /// </summary>
        public Guid OrderID { get; set; }

        /// <summary>
        /// 单子详情ID
        /// </summary>
        public Guid OrderDetailsID { get; set; }

        /// <summary>
        /// 折扣ID
        /// </summary>
        public Guid DiscountID { get; set; }

        /// <summary>
        /// 折扣名称
        /// </summary>
        public string DiscountName { get; set; }

        /// <summary>
        /// 折扣类别 1:折扣率, 2:固定折扣
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
        /// 折扣金额
        /// </summary>
        public decimal OffPay { get; set; }

        /// <summary>
        /// 日结号
        /// </summary>
        public string DailyStatementNo { get; set; }

        /// <summary>
        /// 员工主键
        /// </summary>
        public Guid EmployeeID { get; set; }
    }
}
