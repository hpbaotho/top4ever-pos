using System;
using System.Collections.Generic;

using Top4ever.Domain.Customers;

namespace Top4ever.Interface.Customers
{
    /// <summary>
    /// Summary description for ICustomerInfoDao.
    /// </summary>
    public interface ICustomerInfoDao
    {
        int CreateCustomerInfo(CustomerInfo customerInfo);

        bool UpdateCustomerInfo(CustomerInfo customerInfo);

        CustomerInfo GetCustomerInfoByPhone(string telephone);

        IList<CustomerInfo> GetAllCustomerInfo();
    }
}
