using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Top4ever.Domain.Customers
{
    public class CallRecord
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid CallRecordID { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 拨打时间
        /// </summary>
        public DateTime CallTime { get; set; }
        /// <summary>
        /// 0:未接 1:已接 2:忽略
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
    }
}
