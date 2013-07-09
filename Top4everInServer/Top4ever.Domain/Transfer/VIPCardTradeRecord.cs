using System;
using System.Collections.Generic;

using Top4ever.Domain.MembershipCard;

namespace Top4ever.Domain.Transfer
{
    public class VIPCardTradeRecord
    {
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 折扣率
        /// </summary>
        public decimal DiscountRate { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 交易记录
        /// </summary>
        public IList<VIPCardTrade> VIPCardTradeList { get; set; }
    }
}