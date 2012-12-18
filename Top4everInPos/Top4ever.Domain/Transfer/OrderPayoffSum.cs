using System;

namespace Top4ever.Domain.Transfer
{
    public class OrderPayoffSum
    {
        /// <summary>
        /// 支付方式ID
        /// </summary>
        public Guid PayoffID { get; set; }

        /// <summary>
        /// 支付名称
        /// </summary>
        public string PayoffName { get; set; }

        /// <summary>
        /// 支付的次数
        /// </summary>
        public int Times { get; set; }

        /// <summary>
        /// 支付金额合计
        /// </summary>
        public decimal PayoffMoney { get; set; }
    }
}
