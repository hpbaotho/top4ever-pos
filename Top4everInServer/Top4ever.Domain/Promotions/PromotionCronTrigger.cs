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
        /// 开始日期
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 周
        /// </summary>
        public string Week { get; set; }
        /// <summary>
        /// 日
        /// </summary>
        public string Day { get; set; }
        /// <summary>
		/// 小时
		/// </summary>
		public string Hour { get; set; }
		/// <summary>
		/// 分钟
		/// </summary>
		public string Minute { get; set; }
    }
}
