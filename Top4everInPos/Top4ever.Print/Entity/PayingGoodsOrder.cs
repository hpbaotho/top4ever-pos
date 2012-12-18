using System;
using System.Collections.Generic;
using System.Text;

namespace Top4ever.Print.Entity
{
    public class PayingGoodsOrder
    {
        /// <summary>
        /// 支付名称
        /// </summary>
        public string PayoffName { get; set; }

        /// <summary>
        /// 1,现金; 2,礼券或代金券; 3,信用卡; 4,会员卡或储值卡
        /// </summary>
        public int PayoffType { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public string PayoffMoney { get; set; }

        /// <summary>
        /// 找零
        /// </summary>
        public string NeedChangePay { get; set; }

        /// <summary>
        /// 记录支付方式涉及的卡号
        /// </summary>
        public string CardNo { get; set; }
    }
}
