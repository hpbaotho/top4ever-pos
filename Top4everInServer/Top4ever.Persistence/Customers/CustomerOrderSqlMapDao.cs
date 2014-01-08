using System;
using System.Collections;
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

        public void CreateOrUpdateCustomerOrder(CustomerOrder customerOrder)
        {
            Hashtable htParam = new Hashtable();
            htParam["OrderID"] = customerOrder.OrderID;
            htParam["Telephone"] = customerOrder.Telephone;
            htParam["CustomerName"] = customerOrder.CustomerName;
            htParam["Address"] = customerOrder.Address;
            htParam["Remark"] = customerOrder.Remark ?? string.Empty;
            htParam["DeliveryEmployeeNo"] = customerOrder.DeliveryEmployeeNo ?? string.Empty;
            ExecuteInsert("InsertCustomerOrder", htParam);
        }

        public CustomerOrder GetCustomerOrder(Guid orderID)
        {
            return (ExecuteQueryForObject("GetCustomerOrder", orderID) as CustomerOrder);
        }

        #endregion
    }
}
