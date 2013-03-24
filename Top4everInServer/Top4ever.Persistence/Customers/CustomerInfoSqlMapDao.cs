using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.Customers;
using Top4ever.Interface.Customers;

namespace Top4ever.Persistence.Customers
{
    /// <summary>
    /// Summary description for CustomerInfoSqlMapDao
    /// </summary>
    public class CustomerInfoSqlMapDao : BaseSqlMapDao, ICustomerInfoDao
    {
        public int CreateCustomerInfo(CustomerInfo customerInfo)
        {
            Hashtable htParam = new Hashtable();
            htParam["Telephone"] = customerInfo.Telephone;
            htParam["CustomerName"] = customerInfo.CustomerName;
            htParam["DeliveryAddress1"] = customerInfo.DeliveryAddress1;
            htParam["DeliveryAddress2"] = customerInfo.DeliveryAddress2;
            htParam["DeliveryAddress3"] = customerInfo.DeliveryAddress3;
            htParam["ActiveIndex"] = customerInfo.ActiveIndex;
            htParam["LastModifiedEmployeeID"] = customerInfo.LastModifiedEmployeeID;
            htParam["ReturnValue"] = 0;

            ExecuteInsert("InsertCustomerInfo", htParam);
            return (int)htParam["ReturnValue"];
        }

        public bool UpdateCustomerInfo(CustomerInfo customerInfo)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateCustomerInfo", customerInfo);
            return result > 0;
        }

        public CustomerInfo GetCustomerInfoByPhone(string telephone)
        {
            return (ExecuteQueryForObject("GetCustomerInfoByPhone", telephone) as CustomerInfo);
        }

        public IList<CustomerInfo> GetAllCustomerInfo()
        {
            return ExecuteQueryForList<CustomerInfo>("GetAllCustomerInfo", null);
        }
    }
}
