using System;

namespace Top4ever.Domain.Promotions
{
    public class PromotionCronTrigger
    {
        /// <summary>
        /// 活动号
        /// </summary>
        public string ActivityNo { get; set; }
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
