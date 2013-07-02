using System;

namespace Top4ever.Domain.Transfer
{
    public class VIPCardPayment
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string cardNo { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal payAmount { get; set; }
        /// <summary>
        /// 支付积分
        /// </summary>
        public int payIntegral { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 员工号
        /// </summary>
        public string employeeNo { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string deviceNo { get; set; }
    }
}