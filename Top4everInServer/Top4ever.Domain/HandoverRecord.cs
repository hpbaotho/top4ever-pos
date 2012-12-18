using System;

namespace Top4ever.Domain
{
    public class HandoverRecord
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
        /// 设备号
        /// </summary>
        public string DeviceNo { get; set; }
        /// <summary>
        /// 班次
        /// </summary>
        public int WorkSequence { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid EmployeeID { get; set; }
    }
}
