using System;
using System.Collections.Generic;

namespace Top4ever.Domain.Transfer
{
    public class HourOrderSales
    {
        /// <summary>
        /// 账单数
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string OrderDate { get; set; }
        /// <summary>
        /// 小时
        /// </summary>
        public string OrderHour { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal OrderPrice { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNum { get; set; }
    }
}
