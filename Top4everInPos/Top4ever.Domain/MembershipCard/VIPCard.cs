using System;

namespace Top4ever.Domain.MembershipCard
{
    public class VIPCard
    {
        /// <summary>
        /// 会员卡主键
        /// </summary>
        public Guid VIPCardID { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 折扣率
        /// </summary>
        public decimal DiscountRate { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 最后消费时间
        /// </summary>
        public DateTime? LastConsumeTime { get; set; }
        /// <summary>
        /// 开卡时间
        /// </summary>
        public DateTime OpenCardTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
