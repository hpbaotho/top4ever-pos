using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Persistence.OrderRelated
{
    /// <summary>
    /// Summary description for OrderPayoffSqlMapDao
    /// </summary>
    public class OrderPayoffSqlMapDao : BaseSqlMapDao, IOrderPayoffDao
    {
        #region IOrderPayoffDao Members

        public void CreateOrderPayoff(OrderPayoff orderPayoff)
        {
            ExecuteInsert("InsertOrderPayoff", orderPayoff);
        }

        public bool DeleteOrderPayoff(Guid orderID)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateDeletedOrderPayoff", orderID);
            return result > 0;
        }

        public IList<OrderPayoff> GetOrderPayoffList(Guid orderID)
        {
            return ExecuteQueryForList<OrderPayoff>("GetOrderPayoffList", orderID);
        }

        #endregion
    }
}
