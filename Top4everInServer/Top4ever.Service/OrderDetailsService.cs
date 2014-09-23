using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;
using Newtonsoft.Json;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface.OrderRelated;
using Top4ever.Interface;
using Top4ever.Utils;

namespace Top4ever.Service
{
    public class OrderDetailsService
    {
        #region Private Fields

        private static readonly OrderDetailsService _instance = new OrderDetailsService();
        private readonly IDaoManager _daoManager;
        private readonly IOrderDetailsDao _orderDetailsDao;
        private readonly IDailyStatementDao _dailyStatementDao;

        #endregion

        #region Constructor

        private OrderDetailsService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderDetailsDao = _daoManager.GetDao(typeof(IOrderDetailsDao)) as IOrderDetailsDao;
            _dailyStatementDao = _daoManager.GetDao(typeof(IDailyStatementDao)) as IDailyStatementDao;
        }

        #endregion

        #region Public methods

        public static OrderDetailsService GetInstance()
        {
            return _instance;
        }

        public void CreateOrderDetails(OrderDetails orderDetails)
        {
            try
            {
                _daoManager.OpenConnection();
                _orderDetailsDao.CreateOrderDetails(orderDetails);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[CreateOrderDetails]参数：orderDetails_{0}", JsonConvert.SerializeObject(orderDetails)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
        }

        public bool UpdateOrderDetails(OrderDetails orderDetails)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                result = _orderDetailsDao.UpdateOrderDetails(orderDetails);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[UpdateOrderDetails]参数：orderDetails_{0}", JsonConvert.SerializeObject(orderDetails)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public bool UpdateOrderDetailsDiscount(OrderDetails orderDetails)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                result = _orderDetailsDao.UpdateOrderDetailsDiscount(orderDetails);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[UpdateOrderDetailsDiscount]参数：orderDetails_{0}", JsonConvert.SerializeObject(orderDetails)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public IList<OrderDetails> GetOrderDetails(Guid orderId)
        {
            IList<OrderDetails> orderDetailsList = null;
            try
            {
                _daoManager.OpenConnection();
                orderDetailsList = _orderDetailsDao.GetOrderDetailsList(orderId);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetOrderDetails]参数：orderId_" + orderId, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return orderDetailsList;
        }

        public bool LadeOrderDetails(OrderDetails orderDetails)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                result = _orderDetailsDao.LadeOrderDetails(orderDetails);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[LadeOrderDetails]参数：orderDetails_{0}", JsonConvert.SerializeObject(orderDetails)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public bool SubtractSalesSplitOrder(OrderDetails orderDetails)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                result = _orderDetailsDao.SubtractSalesSplitOrder(orderDetails);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[SubtractSalesSplitOrder]参数：orderDetails_{0}", JsonConvert.SerializeObject(orderDetails)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public DeletedAllItems GetAllDeletedItems(DateTime beginDate, DateTime endDate, int dateType)
        {
            DeletedAllItems deletedItems = new DeletedAllItems();
            try
            {
                _daoManager.OpenConnection();
                deletedItems.DeletedOrderItemList = _orderDetailsDao.GetDeletedOrderItemList(beginDate, endDate, dateType);
                deletedItems.DeletedGoodsItemList = _orderDetailsDao.GetDeletedGoodsItemList(beginDate, endDate, dateType);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetAllDeletedItems]参数：beginDate_{0},endDate_{1},dateType_{2}", beginDate, endDate, dateType), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return deletedItems;
        }

        public decimal GetLastCustomPrice(Guid goodsId)
        {
            decimal price = 0M;
            try
            {
                _daoManager.OpenConnection();
                //日结号
                string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                if (!string.IsNullOrEmpty(dailyStatementNo))
                {
                    price = _orderDetailsDao.GetLastCustomPrice(dailyStatementNo, goodsId);
                }
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetLastCustomPrice]参数：goodsId_{0}", goodsId), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return price;
        }

        #endregion
    }
}
