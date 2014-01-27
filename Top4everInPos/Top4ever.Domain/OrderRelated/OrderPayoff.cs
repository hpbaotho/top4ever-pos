using System;
using System.Collections.Generic;

namespace Top4ever.Domain.OrderRelated
{
    [Serializable]
    public class OrderPayoff
    {
        /// <summary>
        /// 单子支付ID
        /// </summary>
        public Guid OrderPayoffID { get; set; }

        /// <summary>
        /// 单子ID
        /// </summary>
        public Guid OrderID { get; set; }

        /// <summary>
        /// 支付方式ID
        /// </summary>
        public Guid PayoffID { get; set; }

        /// <summary>
        /// 支付名称
        /// </summary>
        public string PayoffName { get; set; }

        /// <summary>
        /// 1,现金; 2,礼券或代金券; 3,信用卡; 4,会员卡或储值卡; 5,团购券
        /// </summary>
        public int PayoffType { get; set; }

        /// <summary>
        /// 单位价值
        /// </summary>
        public decimal AsPay { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 找零
        /// </summary>
        public decimal NeedChangePay { get; set; }

        /// <summary>
        /// 记录支付方式涉及的卡号
        /// </summary>
        public string CardNo { get; set; }

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
