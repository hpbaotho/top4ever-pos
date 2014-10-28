using System;

namespace Top4ever.Domain.Transfer
{
    public class VIPCardAddMoney
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string CardPassword { get; set; }
        /// <summary>
        /// 储值金额
        /// </summary>
        public decimal StoreMoney { get; set; }
        /// <summary>
        /// 员工号
        /// </summary>
        public string EmployeeNo { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceNo { get; set; }
        /// <summary>
        /// 支付ID
        /// </summary>
        public Guid PayoffID { get; set; }
        /// <summary>
        /// 支付名称
        /// </summary>
        public string PayoffName { get; set; }
    }
}
