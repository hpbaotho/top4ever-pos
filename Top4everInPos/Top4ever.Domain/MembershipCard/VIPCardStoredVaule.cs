using System;

namespace Top4ever.Domain.MembershipCard
{
    public class VIPCardStoredVaule
    {
        /// <summary>
        /// 储值金额
        /// </summary>
        public decimal StoredVauleAmount { get; set; }
        /// <summary>
        /// 赠送固定金额
        /// </summary>
        public decimal FixedAmount { get; set; }
        /// <summary>
        /// 赠送金额比率
        /// </summary>
        public decimal PresentAmountRate { get; set; }
        /// <summary>
        /// 赠送固定积分
        /// </summary>
        public int FixedIntegral { get; set; }
        /// <summary>
        /// 赠送积分比率
        /// </summary>
        public decimal PresentIntegralRate { get; set; }
        /// <summary>
        /// 是否倍数
        /// </summary>
        public bool IsMultiple { get; set; }
    }
}