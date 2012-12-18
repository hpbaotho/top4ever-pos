using System;
using System.Collections.Generic;

namespace Top4ever.Domain.OrderRelated
{
    [Serializable]
    public class Order
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid OrderID { get; set; }
        /// <summary>
        /// 账单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 售价总计
        /// </summary>
        public decimal TotalSellPrice { get; set; }
        /// <summary>
        /// 实际售价
        /// </summary>
        public decimal ActualSellPrice { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal DiscountPrice { get; set; }
        /// <summary>
        /// 去零
        /// </summary>
        public decimal CutOffPrice { get; set; }
        /// <summary>
        /// 服务费
        /// </summary>
        public decimal ServiceFee { get; set; }
        /// <summary>
        /// 找零
        /// </summary>
        public decimal NeedChangePay { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceNo { get; set; }
        /// <summary>
        /// 桌子名称
        /// </summary>
        public string DeskName { get; set; }
        /// <summary>
        /// 班次
        /// </summary>
        public int WorkSequence { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public int TranSequence { get; set; }
        /// <summary>
        /// 业务类型 (店用,外带,外送)
        /// </summary>
        public int EatType { get; set; }
        /// <summary>
        /// 单据状态0 初始，1 结账，2作废
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 子桌号
        /// </summary>
        public int SubOrderNo { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNum { get; set; }
        /// <summary>
        /// 会员卡
        /// </summary>
        public string MembershipCard{get;set;}
        /// <summary>
        /// 会员折扣
        /// </summary>
	    public decimal MemberDiscount{get;set;}
        /// <summary>
        /// 点单员工主键
        /// </summary>
        public Guid EmployeeID { get; set; }
        /// <summary>
        /// 点单员工编号
        /// </summary>
        public string EmployeeNo { get; set; }
        /// <summary>
        /// 结账员工主键
        /// </summary>
        public Guid PayEmployeeID { get; set; }
        /// <summary>
        /// 结账员工编号
        /// </summary>
        public string PayEmployeeNo { get; set; }
        /// <summary>
        /// 点单持续时间
        /// </summary>
        public int OrderLastTime { get; set; }
        /// <summary>
        /// 结账持续时间
        /// </summary>
        public int PayLastTime { get; set; }
        /// <summary>
        /// 结账设备号
        /// </summary>
        public string CheckoutDeviceNo { get; set; }
        /// <summary>
        /// 日结号
        /// </summary>
        public string DailyStatementNo { get; set; }
    }
}
