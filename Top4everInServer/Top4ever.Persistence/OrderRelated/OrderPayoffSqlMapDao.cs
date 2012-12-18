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

        public IList<OrderPayoffSum> GetOrderPayoffSumList(string dailyStatementNo)
        {
            return ExecuteQueryForList<OrderPayoffSum>("GetOrderPayoffSum", dailyStatementNo);
        }

        public void CreateOrderPayoff(OrderPayoff orderPayoff)
        {
            ExecuteInsert("InsertOrderPayoff", orderPayoff);
        }

        #endregion
    }
}
