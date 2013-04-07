using System;
using System.Collections.Generic;
using System.Text;

namespace Top4ever.Print.Entity
{
    public class PrintData
    {
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 桌号
        /// </summary>
        public string DeskName { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public string PersonNum { get; set; }
        /// <summary>
        /// 打印时间
        /// </summary>
        public string PrintTime { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string TranSequence { get; set; }
        /// <summary>
        /// 员工号
        /// </summary>
        public string EmployeeNo { get; set; }
        /// <summary>
        /// 店铺地址
        /// </summary>
        public string ShopAddress { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public string ReceivableMoney { get; set; }
        /// <summary>
        /// 服务费
        /// </summary>
        public string ServiceFee { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public string TotalAmount { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public string PaidInMoney { get; set; }
        /// <summary>
        /// 找零金额
        /// </summary>
        public string NeedChangePay { get; set; }
        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<GoodsOrder> GoodsOrderList { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public List<PayingGoodsOrder> PayingOrderList { get; set; }
        /// <summary>
        /// 客户联系电话
        /// </summary>
        public string CustomerPhone { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 外送地址
        /// </summary>
        public string DeliveryAddress { get; set; }
        /// <summary>
        /// 外送备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 外送员工姓名
        /// </summary>
        public string DeliveryEmployeeName { get; set; }
    }
}
