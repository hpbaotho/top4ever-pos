using System;
using System.Collections.Generic;

using Top4ever.Domain.MembershipCard;

namespace Top4ever.Domain.Transfer
{
    [Serializable]
    public class BusinessReport
    {
        /// <summary>
        /// 班次
        /// </summary>
        public int WorkSequence { get; set; }
        /// <summary>
        /// 上次交班时间
        /// </summary>
        public DateTime? LastHandoverTime { get; set; }
        /// <summary>
        /// 上次日结时间
        /// </summary>
        public DateTime? LastDailyStatementTime { get; set; }
        /// <summary>
        /// 营收总额
        /// </summary>
        public decimal TotalRevenue { get; set; }
        /// <summary>
        /// 去尾折扣
        /// </summary>
        public decimal CutOffTotalPrice { get; set; }
        /// <summary>
        /// 折扣合计
        /// </summary>
        public decimal DiscountTotalPrice { get; set; }
        /// <summary>
        /// 营收净额
        /// </summary>
        public decimal ActualTotalIncome { get; set; }
        /// <summary>
        /// 服务费
        /// </summary>
        public decimal TotalServiceFee { get; set; }
        /// <summary>
        /// 单据总数量
        /// </summary>
        public int BillTotalQty { get; set; }
        /// <summary>
        /// 总人数
        /// </summary>
        public int PeopleTotalNum { get; set; }
        /// <summary>
        /// 折扣合计
        /// </summary>
        public IList<OrderDiscountSum> orderDiscountSumList { get; set; }
        /// <summary>
        /// 储值金额列表
        /// </summary>
        public IList<VIPCardTrade> cardStoredValueList { get; set; }
        /// <summary>
        /// 支付方式合计
        /// </summary>
        public IList<OrderPayoffSum> orderPayoffSumList { get; set; }
        /// <summary>
        /// 部门销售数据
        /// </summary>
        public IList<SalesPriceByDepart> salesPriceByDepartList { get; set; }
    }
}
