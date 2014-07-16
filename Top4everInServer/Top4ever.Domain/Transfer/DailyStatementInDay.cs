using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Top4ever.Domain.Transfer
{
    public class DailyStatementInDay
    {
        /// <summary>
        /// 日结所属日期
        /// </summary>
        public DateTime BelongToDate { get; set; }

        /// <summary>
        /// 营收净额
        /// </summary>
        public decimal ActualTotalIncome { get; set; }
    }
}
