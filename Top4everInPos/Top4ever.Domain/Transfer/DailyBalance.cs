using System;
using System.Collections.Generic;

namespace Top4ever.Domain.Transfer
{
    [Serializable]
    public class DailyBalance
    {
        /// <summary>
        /// 日结信息
        /// </summary>
        public DailyStatement dailyStatement { get; set; }
        /// <summary>
        /// 日结金额
        /// </summary>
        public DailyTurnover dailyTurnover { get; set; }
    }
}
