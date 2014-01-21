using System;

namespace Top4ever.Domain.Transfer
{
    [Serializable]
    public class DailyBalanceTime
    {
        /// <summary>
        /// 日结号
        /// </summary>
        public string DailyStatementNo { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime MinTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime MaxTime { get; set; }
    }
}
