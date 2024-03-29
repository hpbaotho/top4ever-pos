﻿using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Persistence.OrderRelated
{
    /// <summary>
    /// Summary description for OrderDiscountSqlMapDao
    /// </summary>
    public class OrderDiscountSqlMapDao : BaseSqlMapDao, IOrderDiscountDao
    {
        #region IOrderDiscountDao Members

        public IList<OrderDiscountSum> GetOrderDiscountSumList(string dailyStatementNo)
        {
            return ExecuteQueryForList<OrderDiscountSum>("GetOrderDiscountSum", dailyStatementNo);
        }

        public void CreateOrderDiscount(OrderDiscount orderDiscount)
        {
            ExecuteInsert("InsertOrderDiscount", orderDiscount);
        }

        public bool UpdateOrderDiscount(OrderDiscount orderDiscount)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateOrderDiscount", orderDiscount);
            return result > 0;
        }

        public bool DeleteOrderDiscount(Guid orderID)
        {
            int result = 0;
            result = ExecuteUpdate("DeleteOrderDiscount", orderID);
            return result > 0;
        }

        public bool DeleteOrderSingleDiscount(Guid orderDetailsID)
        {
            int result = 0;
            result = ExecuteUpdate("DeleteOrderSingleDiscount", orderDetailsID);
            return result > 0;
        }

        public IList<OrderDiscount> GetOrderDiscountList(Guid orderID)
        {
            return ExecuteQueryForList<OrderDiscount>("GetOrderDiscount", orderID);
        }

        public IList<OrderDiscount> GetDeletedOrderDiscountList(Guid orderID)
        {
            return ExecuteQueryForList<OrderDiscount>("GetDeletedOrderDiscount", orderID);
        }

        #endregion
    }
}
