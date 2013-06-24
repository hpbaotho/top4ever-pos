using System;

namespace Top4ever.Domain.MembershipCard
{
    public class VIPCardTrade
    {
        public string CardNo { get; set; }

        public string TradePayNo { get; set; }

        public int TradeType { get; set; }

        public decimal TradeAmount { get; set; }

        public int TradeIntegral { get; set; }

        public DateTime TradeTime { get; set; }

        public string PayoffName { get; set; }

        public string DeviceNo { get; set; }

        public bool IsVoided { get; set; }
    }
}
