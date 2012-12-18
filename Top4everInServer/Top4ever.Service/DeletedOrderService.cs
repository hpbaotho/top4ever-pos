using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.OrderRelated;
namespace Top4ever.Service
{
    public class DeletedOrderService
    {
        #region Private Fields

        private static DeletedOrderService _instance = new DeletedOrderService();
        private IDaoManager _daoManager = null;
        private IOrderDao _orderDao = null;
        private IOrderDetailsDao _orderDetailsDao = null;
        private IOrderDiscountDao _orderDiscountDao = null;

        #endregion

        #region Constructor

        private DeletedOrderService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderDao = _daoManager.GetDao(typeof(IOrderDao)) as IOrderDao;
            _orderDetailsDao = _daoManager.GetDao(typeof(IOrderDetailsDao)) as IOrderDetailsDao;
            _orderDiscountDao = _daoManager.GetDao(typeof(IOrderDiscountDao)) as IOrderDiscountDao;
        }

        #endregion

        #region Public methods

        public static DeletedOrderService GetInstance()
        {
            return _instance;
        }

        public bool DeleteWholeOrder(DeletedOrder deletedOrder)
        {
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                if (_orderDao.DeleteWholeOrder(deletedOrder))
                {
                    if (_orderDetailsDao.DeleteWholeOrderDetails(deletedOrder))
                    {
                        //该订单可能不包含折扣
                        _orderDiscountDao.DeleteOrderDiscount(deletedOrder.OrderID);
                        returnValue = true;
                    }
                }
                _daoManager.CommitTransaction();
            }
            catch
            {
                _daoManager.RollBackTransaction();
                returnValue = false;
            }
            return returnValue;
        }

        public bool DeleteSingleOrder(DeletedSingleOrder deletedSingleOrder)
        {
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                Order order = new Order();
                order.OrderID = deletedSingleOrder.OrderID;
                order.TotalSellPrice = deletedSingleOrder.TotalSellPrice;
                order.ActualSellPrice = deletedSingleOrder.ActualSellPrice;
                order.DiscountPrice = deletedSingleOrder.DiscountPrice;
                order.CutOffPrice = deletedSingleOrder.CutOffPrice;
                if (_orderDao.UpdateOrderPrice(order))
                {
                    foreach (DeletedOrderDetails item in deletedSingleOrder.deletedOrderDetailsList)
                    {
                        _orderDetailsDao.DeleteSingleOrderDetails(item);
                    }
                    returnValue = true;
                }
                _daoManager.CommitTransaction();
            }
            catch
            {
                _daoManager.RollBackTransaction();
                returnValue = false;
            }
            return returnValue;
        }
        #endregion
    }
}
