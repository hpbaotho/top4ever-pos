using System;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;

namespace Top4ever.Domain.Transfer
{
    [Serializable]
    public class SalesOrder
    {
        /// <summary>
        /// 账单信息
        /// </summary>
        public Order order { get; set; }

        /// <summary>
        /// 账单列表
        /// </summary>
        public IList<OrderDetails> orderDetailsList { get; set; }

        /// <summary>
        /// 账单折扣列表
        /// </summary>
        public IList<OrderDiscount> orderDiscountList { get; set; }
    }
}
