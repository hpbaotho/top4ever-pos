using System;

namespace Top4ever.LocalService.Entity
{
    public class CardRefundPay
    {
        /// <summary>
        /// 自增
        /// </summary>
        public int StoreValueID { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShopID { get; set; }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string TradePayNo { get; set; }
        /// <summary>
        /// 日结号
        /// </summary>
        public string DailyStatementNo { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 员工
        /// </summary>
        public string EmployeeNo { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
