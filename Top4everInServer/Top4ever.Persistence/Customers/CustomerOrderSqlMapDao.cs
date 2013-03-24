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

        public CustomerOrder GetCustomerOrder(Guid orderID)
        {
            return (ExecuteQueryForObject("GetCustomerOrder", orderID) as CustomerOrder);
        }

        public bool UpdateCustomerOrder(CustomerOrder customerOrder)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateCustomerOrder", customerOrder);
            return result > 0;
        }

        #endregion
    }
}
