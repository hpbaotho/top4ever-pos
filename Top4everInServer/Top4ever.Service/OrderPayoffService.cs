using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;
using Newtonsoft.Json;
using Top4ever.Domain.OrderRelated;
using Top4ever.Interface.OrderRelated;
using Top4ever.Utils;

namespace Top4ever.Service
{
    public class OrderPayoffService
    {
        #region Private Fields

        private static readonly OrderPayoffService _instance = new OrderPayoffService();
        private readonly IDaoManager _daoManager;
        private readonly IOrderPayoffDao _orderPayoffDao;

        #endregion
        
        #region Constructor

        private OrderPayoffService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderPayoffDao = _daoManager.GetDao(typeof(IOrderPayoffDao)) as IOrderPayoffDao;
        }

        #endregion

        #region Public methods

        public static OrderPayoffService GetInstance()
        {
            return _instance;
        }

        public void CreateOrderPayoff(OrderPayoff orderPayoff)
        {
            try
            {
                _daoManager.OpenConnection();
                _orderPayoffDao.CreateOrderPayoff(orderPayoff);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[CreateOrderPayoff]参数：orderPayoff_{0}", JsonConvert.SerializeObject(orderPayoff)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
        }

        public bool DeleteOrderPayoff(Guid orderId)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                result = _orderPayoffDao.DeleteOrderPayoff(orderId);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[DeleteOrderPayoff]参数：orderId_{0}", orderId), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public IList<OrderPayoff> GetOrderPayoffList(Guid orderId)
        {
            IList<OrderPayoff> orderPayoffList = null;
            try
            {
                _daoManager.OpenConnection();
                orderPayoffList = _orderPayoffDao.GetOrderPayoffList(orderId);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetOrderPayoffList]参数：orderId_{0}", orderId), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return orderPayoffList;
        }

        #endregion
    }
}
