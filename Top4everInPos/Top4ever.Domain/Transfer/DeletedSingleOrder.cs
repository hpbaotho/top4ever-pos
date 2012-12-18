using System;
using System.Collections.Generic;

namespace Top4ever.Domain.Transfer
{
    public class DeletedSingleOrder
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid OrderID { get; set; }
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
        /// 删除的订单详情列表
        /// </summary>
        public IList<DeletedOrderDetails> deletedOrderDetailsList { get; set; }
    }
}
