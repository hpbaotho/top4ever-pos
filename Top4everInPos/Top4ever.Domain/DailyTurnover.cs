using System;
using System.Collections.Generic;

namespace Top4ever.Domain
{
    public class DailyTurnover
    {
        /// <summary>
        /// 日结号
        /// </summary>
        public string DailyStatementNo { get; set; }
        /// <summary>
        /// 营收总额
        /// </summary>
        public decimal TotalRevenue { get; set; }
        /// <summary>
        /// 去尾折扣
        /// </summary>
        public decimal CutOffTotalPrice { get; set; }
        /// <summary>
        /// 折扣合计
        /// </summary>
        public decimal DiscountTotalPrice { get; set; }
        /// <summary>
        /// 营收净额
        /// </summary>
        public decimal ActualTotalIncome { get; set; }
        /// <summary>
        /// 储值金额
        /// </summary>
        public decimal StoredTotalPrice { get; set; }
    }
}
