using System;
using System.Collections.Generic;

namespace Top4ever.Domain.OrderRelated
{
    public class PayoffWay
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid PayoffID { get; set; }
        /// <summary>
        /// 支付名称
        /// </summary>
        public string PayoffName { get; set; }
        /// <summary>
        /// 支付名称第二语言
        /// </summary>
        public string PayoffName2nd { get; set; }
        /// <summary>
        /// 1,现金; 2,礼券或代金券; 3,信用卡; 4,会员卡或储值卡
        /// </summary>
        public int PayoffType { get; set; }
        /// <summary>
        /// 单位价值
        /// </summary>
        public decimal AsPay { get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public Guid ButtonStyleID { get; set; }
        /// <summary>
        /// 作为会员卡的储值支付方式
        /// </summary>
        public bool AsVIPCardPayWay { get; set; }
    }
}
