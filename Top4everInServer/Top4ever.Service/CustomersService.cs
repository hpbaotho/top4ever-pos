using System;
using System.Collections.Generic;

using IBatisNet.Common.Logging;
using IBatisNet.DataAccess;

using Top4ever.Domain.Customers;
using Top4ever.Interface.Customers;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for CustomerService.
    /// </summary>
    public class CustomersService
    {
        #region Private Fields

        private static CustomersService _instance = new CustomersService();
        private IDaoManager _daoManager = null;
        private ICustomerInfoDao _customerInfoDao = null;
        private ICustomerOrderDao _customerOrderDao = null;
        private IOrderDao _orderDao = null;

        #endregion

        #region Constructor

        private CustomersService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _customerInfoDao = _daoManager.GetDao(typeof(ICustomerInfoDao)) as ICustomerInfoDao;
            _customerOrderDao = _daoManager.GetDao(typeof(ICustomerOrderDao)) as ICustomerOrderDao;
            _orderDao = _daoManager.GetDao(typeof(IOrderDao)) as IOrderDao;
        }

        #endregion

        #region Public methods

        public static CustomersService GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// 创建客户信息 0:数据库操作失败, 1:成功, 2:手机号已存在
        /// </summary>
        public int CreateCustomerInfo(CustomerInfo customerInfo)
        {
            int result = 0;
            try
            {
                _daoManager.OpenConnection();
                result = _customerInfoDao.CreateCustomerInfo(customerInfo);
                _daoManager.CloseConnection();
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public bool UpdateCustomerInfo(CustomerInfo customerInfo)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                result = _customerInfoDao.UpdateCustomerInfo(customerInfo);
                _daoManager.CloseConnection();
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public CustomerInfo GetCustomerInfoByPhone(string telephone)
        {
            CustomerInfo customerInfo = null;
            try
            {
                _daoManager.OpenConnection();
                customerInfo = _customerInfoDao.GetCustomerInfoByPhone(telephone);
                _daoManager.CloseConnection();
            }
            catch
            {
                customerInfo = null;
            }
            return customerInfo;
        }

        public IList<CustomerInfo> GetAllCustomerInfo()
        {
            IList<CustomerInfo> customerInfoList = null;
            try
            {
                _daoManager.OpenConnection();
                customerInfoList = _customerInfoDao.GetAllCustomerInfo();
                _daoManager.CloseConnection();
            }
            catch
            {
                customerInfoList = null;
            }
            return customerInfoList;
        }

        public CustomerOrder GetCustomerOrder(Guid orderID)
        {
            CustomerOrder customerOrder = null;
            try
            {
                _daoManager.OpenConnection();
                customerOrder = _customerOrderDao.GetCustomerOrder(orderID);
                _daoManager.CloseConnection();
            }
            catch
            {
                customerOrder = null;
            }
            return customerOrder;
        }

        public bool UpdateTakeoutOrderStatus(CustomerOrder customerOrder)
        {
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                if (!string.IsNullOrEmpty(customerOrder.Telephone))
                {
                    //更新外送人员
                    _customerOrderDao.UpdateCustomerOrder(customerOrder);
                }
                _orderDao.DeliveryTakeoutOrder(customerOrder.OrderID, customerOrder.DeliveryEmployeeName);
                returnValue = true;
                _daoManager.CommitTransaction();
            }
            catch
            {
                _daoManager.RollBackTransaction();
                returnValue = false;
            }
            return returnValue;
        }

        public bool CreateCustomerOrder(CustomerOrder customerOrder)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                _customerOrderDao.CreateCustomerOrder(customerOrder);
                _daoManager.CloseConnection();
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        #endregion
    }
}
