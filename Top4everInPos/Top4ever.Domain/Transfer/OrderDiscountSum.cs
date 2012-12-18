using System;

namespace Top4ever.Domain.Transfer
{
    public class OrderDiscountSum
    {
        /// <summary>
        /// 折扣ID
        /// </summary>
        public Guid DiscountID { get; set; }

        /// <summary>
        /// 折扣名称
        /// </summary>
        public string DiscountName { get; set; }

        /// <summary>
        /// 折扣金额
        /// </summary>
        public decimal DiscountMoney { get; set; }
    }
}
