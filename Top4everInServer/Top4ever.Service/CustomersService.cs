using System;
using System.Collections.Generic;
using IBatisNet.DataAccess;
using Newtonsoft.Json;
using Top4ever.Domain.Customers;
using Top4ever.Interface.Customers;
using Top4ever.Interface.OrderRelated;
using Top4ever.Utils;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for CustomerService.
    /// </summary>
    public class CustomersService
    {
        #region Private Fields

        private static readonly CustomersService _instance = new CustomersService();
        private readonly IDaoManager _daoManager;
        private readonly ICustomerInfoDao _customerInfoDao;
        private readonly ICustomerOrderDao _customerOrderDao;
        private readonly IOrderDao _orderDao;

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
            }
            catch (Exception exception)
            {
                result = 0;
                LogHelper.GetInstance().Error(string.Format("[CreateCustomerInfo]参数：customerInfo_{0}", JsonConvert.SerializeObject(customerInfo)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
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
            }
            catch(Exception exception)
            {
                result = false;
                LogHelper.GetInstance().Error(string.Format("[UpdateCustomerInfo]参数：customerInfo_{0}", JsonConvert.SerializeObject(customerInfo)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
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
            }
            catch (Exception exception)
            {
                customerInfo = null;
                LogHelper.GetInstance().Error(string.Format("[GetCustomerInfoByPhone]参数：telephone_{0}", telephone), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
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
            }
            catch (Exception exception)
            {
                customerInfoList = null;
                LogHelper.GetInstance().Error("[GetAllCustomerInfo]", exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return customerInfoList;
        }

        public CustomerOrder GetCustomerOrder(Guid orderId)
        {
            CustomerOrder customerOrder = null;
            try
            {
                _daoManager.OpenConnection();
                customerOrder = _customerOrderDao.GetCustomerOrder(orderId);
            }
            catch (Exception exception)
            {
                customerOrder = null;
                LogHelper.GetInstance().Error(string.Format("[GetCustomerOrder]参数：orderId_{0}", orderId), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
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
                    _customerOrderDao.CreateOrUpdateCustomerOrder(customerOrder);
                }
                _orderDao.DeliveryTakeoutOrder(customerOrder.OrderID, customerOrder.DeliveryEmployeeNo);
                returnValue = true;
                _daoManager.CommitTransaction();
            }
            catch(Exception exception)
            {
                _daoManager.RollBackTransaction();
                returnValue = false;
                LogHelper.GetInstance().Error(string.Format("[UpdateTakeoutOrderStatus]参数：customerOrder_{0}", JsonConvert.SerializeObject(customerOrder)), exception);
            }
            return returnValue;
        }

        public bool CreateOrUpdateCustomerOrder(CustomerOrder customerOrder)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                _customerOrderDao.CreateOrUpdateCustomerOrder(customerOrder);
                result = true;
            }
            catch (Exception exception)
            {
                result = false;
                LogHelper.GetInstance().Error(string.Format("[CreateOrUpdateCustomerOrder]参数：customerOrder_{0}", JsonConvert.SerializeObject(customerOrder)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        /// <summary>
        /// 创建或者更新电话记录
        /// </summary>
        /// <param name="callRecord">电话记录</param>
        /// <returns></returns>
        public bool CreateOrUpdateCallRecord(CallRecord callRecord)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                _customerOrderDao.CreateOrUpdateCallRecord(callRecord);
                result = true;
            }
            catch (Exception exception)
            {
                result = false;
                LogHelper.GetInstance().Error(string.Format("[CreateOrUpdateCallRecord]参数：callRecord_{0}", JsonConvert.SerializeObject(callRecord)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        /// <summary>
        /// 获取最近一个月特定状态通话记录
        /// </summary>
        /// <returns></returns>
        public IList<CallRecord> GetCallRecordByStatus(int status)
        {
            IList<CallRecord> callRecordList = null;
            try
            {
                _daoManager.OpenConnection();
                callRecordList = status == -1 ? _customerOrderDao.GetCallRecordList() : _customerOrderDao.GetCallRecordByStatus(status);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetCallRecordByStatus]参数：status_" + status, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return callRecordList;
        }

        /// <summary>
        /// 获取热销产品列表
        /// </summary>
        /// <param name="telephone">电话号码</param>
        /// <returns></returns>
        public IList<TopSellGoods> GetTopSellGoods(string telephone)
        {
            IList<TopSellGoods> topSellGoodsList = null;
            try
            {
                _daoManager.OpenConnection();
                topSellGoodsList = _customerOrderDao.GetTopSellGoods(telephone);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetTopSellGoods]参数：telephone_" + telephone, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return topSellGoodsList;
        }

        /// <summary>
        ///  获取一段时间内热销产品列表
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public IList<TopSellGoods> GetTopSellGoodsByTime(DateTime beginDate, DateTime endDate)
        {
            IList<TopSellGoods> topSellGoodsList = null;
            try
            {
                _daoManager.OpenConnection();
                topSellGoodsList = _customerOrderDao.GetTopSellGoodsByTime(beginDate, endDate);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetTopSellGoodsByTime]参数：beginDate_{0}, endDate_{1}", beginDate, endDate), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return topSellGoodsList;
        }

        #endregion
    }
}
