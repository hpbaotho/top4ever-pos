using System;

namespace Top4ever.Domain.MembershipCard
{
    public class VIPCardTrade
    {
        /// <summary>
        /// 
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string TradePayNo { get; set; }
        /// <summary>
        /// 1:储值 2:储值赠送 3:消费
        /// </summary>
        public int TradeType { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal TradeAmount { get; set; }
        /// <summary>
        /// 交易积分
        /// </summary>
        public int TradeIntegral { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime TradeTime { get; set; }
        /// <summary>
        /// 支付方式名称
        /// </summary>
        public string PayoffName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeviceNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsVoided { get; set; }
    }
}
