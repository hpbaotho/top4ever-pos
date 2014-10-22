using System;
using System.Collections.Generic;

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

        /// <summary>
        /// 得到属性值
        /// </summary>
        /// <param name="fieldName">属性字符</param>
        /// <returns></returns>
        public string GetValue(string fieldName)
        {
            string result = string.Empty;
            switch (fieldName.ToLower())
            { 
                case "shopname":
                    result = this.ShopName;
                    break;
                case "deskname":
                    result = this.DeskName;
                    break;
                case "personnum":
                    result = this.PersonNum;
                    break;
                case "printtime":
                    result = this.PrintTime;
                    break;
                case "transequence":
                    result = this.TranSequence;
                    break;
                case "employeeno":
                    result = this.EmployeeNo;
                    break;
                case "shopaddress":
                    result = this.ShopAddress;
                    break;
                case "telephone":
                    result = this.Telephone;
                    break;
                case "receivablemoney":
                    if (!string.IsNullOrEmpty(this.ReceivableMoney))
                    {
                        result = this.ReceivableMoney.Length < 7 ? this.ReceivableMoney.PadLeft(7) : this.ReceivableMoney;
                    }
                    break;
                case "servicefee":
                    if (!string.IsNullOrEmpty(this.ServiceFee))
                    {
                        result = this.ServiceFee.Length < 7 ? this.ServiceFee.PadLeft(7) : this.ServiceFee;
                    }
                    break;
                case "totalamount":
                    if (!string.IsNullOrEmpty(this.TotalAmount))
                    {
                        result = this.TotalAmount.Length < 7 ? this.TotalAmount.PadLeft(7) : this.TotalAmount;
                    }
                    break;
                case "paidinmoney":
                    if (!string.IsNullOrEmpty(this.PaidInMoney))
                    {
                        result = this.PaidInMoney.Length < 7 ? this.PaidInMoney.PadLeft(7) : this.PaidInMoney;
                    }
                    break;
                case "needchangepay":
                    if (!string.IsNullOrEmpty(this.NeedChangePay))
                    {
                        result = this.NeedChangePay.Length < 7 ? this.NeedChangePay.PadLeft(7) : this.NeedChangePay;
                    }
                    break;
                case "customerphone":
                    result = this.CustomerPhone;
                    break;
                case "customername":
                    result = this.CustomerName;
                    break;
                case "deliveryaddress":
                    result = this.DeliveryAddress;
                    break;
                case "remark":
                    result = this.Remark;
                    break;
                case "deliveryemployeename":
                    result = this.DeliveryEmployeeName;
                    break;
                default :
                    result = string.Empty;
                    break;
            }
            return result;
        }
    }
}
