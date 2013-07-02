using System;

namespace Top4ever.Domain.Transfer
{
    public class VIPCardPayment
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 支付积分
        /// </summary>
        public int PayIntegral { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 员工号
        /// </summary>
        public string EmployeeNo { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceNo { get; set; }
    }
}