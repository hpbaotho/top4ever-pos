using System;

namespace Top4ever.Domain
{
    public class HandoverTurnover
    {
        /// <summary>
        /// 交班ID
        /// </summary>
        public Guid HandoverRecordID { get; set; }
        /// <summary>
        /// 日结号
        /// </summary>
        public string DailyStatementNo { get; set; }
        /// <summary>
        /// 支付方式ID
        /// </summary>
        public Guid PayoffID { get; set; }
        /// <summary>
        /// 交班金额
        /// </summary>
        public decimal SalesTurnover { get; set; }
    }
}
