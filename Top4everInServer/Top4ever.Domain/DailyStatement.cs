using System;
using System.Collections.Generic;

namespace Top4ever.Domain
{
    public class DailyStatement
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid DailyStatementID { get; set; }
        /// <summary>
        /// 日结号
        /// </summary>
        public string DailyStatementNo { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceNo { get; set; }
        /// <summary>
        /// 日结所属日期
        /// </summary>
        public DateTime BelongToDate { get; set; }
        /// <summary>
        /// 天气
        /// </summary>
        public string Weather { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid EmployeeID { get; set; }
    }
}
