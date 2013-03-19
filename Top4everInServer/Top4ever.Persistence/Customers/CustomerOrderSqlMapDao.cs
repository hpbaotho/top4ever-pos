using System;
using System.Collections.Generic;

using Top4ever.Domain.Customers;
using Top4ever.Interface.Customers;

namespace Top4ever.Persistence.Customers
{
    /// <summary>
    /// Summary description for CustomerOrderSqlMapDao
    /// </summary>
    public class CustomerOrderSqlMapDao : BaseSqlMapDao, ICustomerOrderDao
    {
        #region ICustomerOrderDao Members

        public void CreateCustomerOrder(CustomerOrder customerOrder)
        {
            ExecuteInsert("InsertCustomerOrder", customerOrder);
        }

        #endregion
    }
}
