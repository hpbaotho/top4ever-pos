using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;
using Newtonsoft.Json;
using Top4ever.Domain.OrderRelated;
using Top4ever.Interface.OrderRelated;
using Top4ever.Utils;

namespace Top4ever.Service
{
    public class OrderDiscountService
    {
        #region Private Fields

        private static readonly OrderDiscountService _instance = new OrderDiscountService();
        private readonly IDaoManager _daoManager;
        private readonly IOrderDiscountDao _orderDiscountDao;

        #endregion

        #region Constructor

        private OrderDiscountService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderDiscountDao = _daoManager.GetDao(typeof(IOrderDiscountDao)) as IOrderDiscountDao;
        }

        #endregion

        #region Public methods

        public static OrderDiscountService GetInstance()
        {
            return _instance;
        }

        public void CreateOrderDiscount(OrderDiscount orderDiscount)
        {
            try
            {
                _daoManager.OpenConnection();
                _orderDiscountDao.CreateOrderDiscount(orderDiscount);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[CreateOrderDiscount]参数：orderDiscount_{0}", JsonConvert.SerializeObject(orderDiscount)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
        }

        public bool UpdateOrderDiscount(OrderDiscount orderDiscount)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                result = _orderDiscountDao.UpdateOrderDiscount(orderDiscount);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[UpdateOrderDiscount]参数：orderDiscount_{0}", JsonConvert.SerializeObject(orderDiscount)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }
        #endregion
    }
}
