﻿using System;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;

namespace Top4ever.Interface.OrderRelated
{
    /// <summary>
    /// Summary description for IOrderDao.
    /// </summary>
    public interface IOrderDao
    {
        string CreateOrder(Order order);

        bool UpdateOrder(Order order);

        bool UpdatePayingOrder(Order order);

        IList<Order> GetOrderList(string deskName);

        IList<Order> GetOrderListBySearch(string strWhere, string orderBy, int pageIndex, int pageSize);

        Order GetOrder(Guid orderID);

        Int32 GetCurrentSubOrderNo(string deskName);

        bool DeleteWholeOrder(DeletedOrder deletedOrder);

        bool UpdateOrderPrice(Order order);

        bool UpdateSplitOrderPrice(Order order);

        bool UpdateOrderDeskName(Order order);

        bool MergeSalesOrder(DeskChange deskChange);

        bool IsExistOrderInTimeInterval(DateTime beginTime, DateTime endTime);

        bool UpdateOrderStatus(Guid orderID, int status);
    }
}
