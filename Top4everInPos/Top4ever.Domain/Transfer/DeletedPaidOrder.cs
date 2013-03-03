using System;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;

namespace Top4ever.Domain.Transfer
{
    public class DeletedPaidOrder
    {
        /// <summary>
        /// 账单信息
        /// </summary>
        public Order order { get; set; }

        /// <summary>
        /// 删除的订单详情列表
        /// </summary>
        public IList<DeletedOrderDetails> deletedOrderDetailsList { get; set; }

        /// <summary>
        /// 账单支付方式列表
        /// </summary>
        public IList<OrderPayoff> orderPayoffList { get; set; }
    }
}
