using System;
using System.Collections.Generic;

using Top4ever.Domain.Customers;

namespace Top4ever.Interface.Customers
{
    /// <summary>
    /// Summary description for ICustomerOrderDao.
    /// </summary>
    public interface ICustomerOrderDao
    {
        void CreateCustomerOrder(CustomerOrder customerOrder);

        CustomerOrder GetCustomerOrder(Guid orderID);

        bool UpdateCustomerOrder(CustomerOrder customerOrder);
    }
}
