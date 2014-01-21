using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Top4ever.Domain.Transfer
{
    public class EmployeeHandoverRecord
    {
        /// <summary>
        /// 交班ID
        /// </summary>
        public Guid HandoverRecordID { get; set; }

        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceNo { get; set; }

        /// <summary>
        /// 班次
        /// </summary>
        public int WorkSequence { get; set; }

        /// <summary>
        /// 交班时间
        /// </summary>
        public DateTime HandoverTime { get; set; }

        /// <summary>
        /// 员工号
        /// </summary>
        public string EmployeeNo { get; set; }
    }
}
