﻿using System;
using System.Collections.Generic;

using Top4ever.Domain.Transfer;
using Top4ever.Domain.OrderRelated;

namespace Top4ever.Interface.OrderRelated
{
    /// <summary>
    /// Summary description for IOrderDetailsDao.
    /// </summary>
    public interface IOrderDetailsDao
    {
        void CreateOrderDetails(OrderDetails orderDetails);

        bool UpdateOrderDetails(OrderDetails orderDetails);

        bool UpdateOrderDetailsDiscount(OrderDetails orderDetails);

        IList<OrderDetails> GetOrderDetailsList(Guid orderId);

        IList<OrderDetails> GetDeletedOrderDetailsList(Guid orderId);

        OrderDetails GetOrderDetails(Guid orderDetailsId);

        IList<OrderDetails> GetOrderDetailsList(IList<Guid> orderDetailsIds);

        Int32 GetSequenceNum(Guid orderId);

        bool DeleteWholeOrderDetails(DeletedOrder deletedOrder);

        bool DeleteSingleOrderDetails(DeletedOrderDetails deletedOrderDetails);

        bool LadeOrderDetails(OrderDetails orderDetails);

        bool SubtractSalesSplitOrder(OrderDetails orderDetails);

        IList<DeletedItem> GetDeletedOrderItemList(DateTime beginDate, DateTime endDate, int dateType);

        IList<DeletedItem> GetDeletedGoodsItemList(DateTime beginDate, DateTime endDate, int dateType);

        decimal GetLastCustomPrice(string dailyStatementNo, Guid goodsId);
    }
}
