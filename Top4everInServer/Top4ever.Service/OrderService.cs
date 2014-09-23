using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;
using Newtonsoft.Json;
using Top4ever.Domain;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.OrderRelated;
using Top4ever.Utils;

namespace Top4ever.Service
{
    public class OrderService
    {
        #region Private Fields

        private static readonly OrderService _instance = new OrderService();
        private readonly IDaoManager _daoManager;
        private readonly IOrderDao _orderDao;
        private readonly IPrintTaskDao _printTaskDao;
        private readonly IDailyStatementDao _dailyStatementDao;
        private readonly ISystemConfigDao _sysConfigDao;

        #endregion

        #region Constructor

        private OrderService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderDao = _daoManager.GetDao(typeof(IOrderDao)) as IOrderDao;
            _printTaskDao = _daoManager.GetDao(typeof(IPrintTaskDao)) as IPrintTaskDao;
            _dailyStatementDao = _daoManager.GetDao(typeof(IDailyStatementDao)) as IDailyStatementDao;
            _sysConfigDao = _daoManager.GetDao(typeof(ISystemConfigDao)) as ISystemConfigDao;
        }

        #endregion

        #region Public methods

        public static OrderService GetInstance()
        {
            return _instance;
        }

        public void CreateOrder(Order order)
        {
            try
            {
                _daoManager.OpenConnection();
                _orderDao.CreateOrder(order);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[CreateOrder]参数：order_{0}", JsonConvert.SerializeObject(order)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
        }

        public bool UpdateOrder(Order order)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                result = _orderDao.UpdateOrder(order);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[UpdateOrder]参数：order_{0}", JsonConvert.SerializeObject(order)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public IList<Order> GetOrderList(string deskName)
        {
            IList<Order> orderList = null;
            try
            {
                _daoManager.OpenConnection();
                orderList = _orderDao.GetOrderList(deskName);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetOrderList]参数：deskName_{0}", deskName), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return orderList;
        }

        public IList<Order> GetOrderListBySearch(string strWhere, string orderBy, int pageIndex, int pageSize)
        {
            IList<Order> orderList = null;
            try
            {
                _daoManager.OpenConnection();
                orderList = _orderDao.GetOrderListBySearch(strWhere, orderBy, pageIndex, pageSize);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetOrderListBySearch]参数：Param_{0}_{1}_{2}_{3}", strWhere, orderBy, pageIndex, pageSize), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return orderList;
        }

        public Order GetOrder(Guid orderId)
        {
            Order order = null;
            try
            {
                _daoManager.OpenConnection();
                order = _orderDao.GetOrder(orderId);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetOrder]参数：orderId_{0}", orderId), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return order;
        }

        public bool OrderDeskOperate(DeskChange deskChange)
        {
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                SystemConfig systemConfig = _sysConfigDao.GetSystemConfigInfo();
                if (systemConfig.IncludeKitchenPrint)
                {
                    //添加打印任务
                    _printTaskDao.InsertDeskOperatePrint(deskChange);
                }
                // 转台
                if (deskChange.OrderID1st != Guid.Empty && deskChange.OrderID2nd == Guid.Empty)
                {
                    //分单号
                    Int32 curSubOrderNo = _orderDao.GetCurrentSubOrderNo(deskChange.DeskName);
                    if (curSubOrderNo > 0)
                    {
                        curSubOrderNo++;
                    }
                    else
                    {
                        curSubOrderNo = 1;
                    }
                    Order order = new Order();
                    order.OrderID = deskChange.OrderID1st;
                    order.DeskName = deskChange.DeskName;
                    order.SubOrderNo = curSubOrderNo;
                    returnValue = _orderDao.UpdateOrderDeskName(order);
                }
                // 合并
                if (deskChange.OrderID1st != Guid.Empty && deskChange.OrderID2nd != Guid.Empty)
                {
                    returnValue = _orderDao.MergeSalesOrder(deskChange);
                }
                _daoManager.CommitTransaction();
            }
            catch(Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[OrderDeskOperate]参数：deskChange_{0}", JsonConvert.SerializeObject(deskChange)), exception);
                _daoManager.RollBackTransaction();
                returnValue = false;
            }
            return returnValue;
        }

        public Int32 GetCurrentSubOrderNo(string deskName)
        {
            Int32 result = 0;
            try
            {
                _daoManager.OpenConnection();
                result = _orderDao.GetCurrentSubOrderNo(deskName);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetCurrentSubOrderNo]参数：deskName_{0}", deskName), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public bool IsExistOrderInTimeInterval(DateTime beginTime, DateTime endTime)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                result = _orderDao.IsExistOrderInTimeInterval(beginTime, endTime);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[IsExistOrderInTimeInterval]参数：beginTime_{0},endTime_{1}", beginTime, endTime), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public bool UpdateOrderStatus(Guid orderId, int status)
        {
            bool result = false;
            try
            {
                _daoManager.OpenConnection();
                result = _orderDao.UpdateOrderStatus(orderId, status);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[UpdateOrderStatus]参数：orderId_{0},status_{1}", orderId, status), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public IList<DeliveryOrder> GetDeliveryOrderList()
        {
            IList<DeliveryOrder> deliveryOrderList = null;
            try
            {
                _daoManager.OpenConnection();
                //日结号
                string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                if (!string.IsNullOrEmpty(dailyStatementNo))
                {
                    deliveryOrderList = _orderDao.GetDeliveryOrderList(dailyStatementNo);
                }
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetDeliveryOrderList]", exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return deliveryOrderList;
        }

        public IList<HourOrderSales> GetHourSalesReport(DateTime beginTime, DateTime endTime)
        {
            IList<HourOrderSales> hourSalesList = null;
            try
            {
                _daoManager.OpenConnection();
                if (beginTime == DateTime.MinValue && endTime == DateTime.MinValue)
                {
                    //日结号
                    string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                    if (!string.IsNullOrEmpty(dailyStatementNo))
                    {
                        hourSalesList = _orderDao.GetHourSalesReport(dailyStatementNo);
                    }
                }
                else
                {
                    hourSalesList = _orderDao.GetHourSalesReportByTime(beginTime, endTime);
                }
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetHourSalesReport]参数：beginTime_{0},endTime_{1}", beginTime, endTime), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return hourSalesList;
        }

        #endregion
    }
}
