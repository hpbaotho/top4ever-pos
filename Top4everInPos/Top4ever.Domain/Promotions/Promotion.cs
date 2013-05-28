using System;

namespace Top4ever.Domain.Promotions
{
    public class Promotion
    {
        /// <summary>
        /// 活动号
        /// </summary>
        public string ActivityNo { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        public string PromotionName { get; set; }
        /// <summary>
        /// 1:一般 2:累赠 3:单选 4:复选
        /// </summary>
        public int PresentType { get; set; }
        /// <summary>
        /// 是否与其他促销并用
        /// </summary>
        public bool WithOtherPromotion { get; set; }
        /// <summary>
        /// 基于会员
        /// </summary>
        public int? CustomerBase { get; set; }
		/// <summary>
		/// 生日限制
		/// </summary>
		public bool MustBirthday { get; set; }
		/// <summary>
		/// 次数限制
		/// </summary>
		public int? TimeLimit { get; set; }
        /// <summary>
        /// 包含/排除
        /// </summary>
        public bool IsIncluded { get; set; }
        /// <summary>
        /// And/or
        /// </summary>
        public bool AndOr { get; set; }
    }
}
