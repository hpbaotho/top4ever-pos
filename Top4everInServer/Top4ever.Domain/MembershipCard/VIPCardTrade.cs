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
        /// 
        /// </summary>
        public decimal TradeAmount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TradeIntegral { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime TradeTime { get; set; }
        /// <summary>
        /// 
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
