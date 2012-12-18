using System;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;

namespace Top4ever.Domain.Transfer
{
    [Serializable]
    public class SalesSplitOrder
    {
        /// <summary>
        /// 原始账单信息
        /// </summary>
        public Order OriginalOrder { get; set; }

        /// <summary>
        /// 需要减去的账单列表
        /// </summary>
        public List<OrderDetails> SubOrderDetailsList { get; set; }

        /// <summary>
        /// 新增账单信息
        /// </summary>
        public Order NewOrder { get; set; }

        /// <summary>
        /// 新增账单列表
        /// </summary>
        public List<OrderDetails> NewOrderDetailsList { get; set; }
    }
}