using System;

namespace Top4ever.Domain.GoodsRelated
{
    public class GoodsCronTrigger
    {
        /// <summary>
        /// 组或者品项的主键
        /// </summary>
        public Guid CronTriggerID { get; set; }
        /// <summary>
        /// 年
        /// </summary>
        public string Year { get; set; }
        /// <summary>
        /// 周
        /// </summary>
        public string Week { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public string Month { get; set; }
        /// <summary>
        /// 日
        /// </summary>
        public string Day { get; set; }
        /// <summary>
        /// 时间段
        /// </summary>
        public string SmallTime { get; set; }
    }
}
