using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.OrderRelated;

namespace Top4ever.Service
{
    public class SalesOrderService
    {

        #region Private Fields

        private static SalesOrderService _instance = new SalesOrderService();
        private IDaoManager _daoManager = null;
        private IOrderDao _orderDao = null;
        private IOrderDetailsDao _orderDetailsDao = null;
        private IOrderDiscountDao _orderDiscountDao = null;
        private IOrderPayoffDao _orderPayoffDao = null;
        private IDailyStatementDao _dailyStatementDao = null;
        private ISystemConfigDao _sysConfigDao = null;
        private ISystemDictionaryDao _sysDictionary = null;
        private IDeskDao _deskDao = null;
        private IPrintTaskDao _printTaskDao = null;

        #endregion

        #region Constructor

        private SalesOrderService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _orderDao = _daoManager.GetDao(typeof(IOrderDao)) as IOrderDao;
            _orderDetailsDao = _daoManager.GetDao(typeof(IOrderDetailsDao)) as IOrderDetailsDao;
            _orderDiscountDao = _daoManager.GetDao(typeof(IOrderDiscountDao)) as IOrderDiscountDao;
            _orderPayoffDao = _daoManager.GetDao(typeof(IOrderPayoffDao)) as IOrderPayoffDao;
            _dailyStatementDao = _daoManager.GetDao(typeof(IDailyStatementDao)) as IDailyStatementDao;
            _sysConfigDao = _daoManager.GetDao(typeof(ISystemConfigDao)) as ISystemConfigDao;
            _sysDictionary = _daoManager.GetDao(typeof(ISystemDictionaryDao)) as ISystemDictionaryDao;
            _deskDao = _daoManager.GetDao(typeof(IDeskDao)) as IDeskDao;
            _printTaskDao = _daoManager.GetDao(typeof(IPrintTaskDao)) as IPrintTaskDao;
        }

        #endregion

        #region Public methods

        public static SalesOrderService GetInstance()
        {
            return _instance;
        }

        public Int32 CreateSalesOrder(SalesOrder salesOrder)
        {
            Int32 tranSequence = 0;
            _daoManager.BeginTransaction();
            try
            {
                //日结号
                string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                if (!string.IsNullOrEmpty(dailyStatementNo))
                {
                    Order order = salesOrder.order;
                    order.DailyStatementNo = dailyStatementNo;
                    //分单号
                    Int32 curSubOrderNo = _orderDao.GetCurrentSubOrderNo(order.DeskName);
                    if (curSubOrderNo > 0)
                    {
                        curSubOrderNo++;
                    }
                    else
                    {
                        curSubOrderNo = 1;
                    }
                    order.SubOrderNo = curSubOrderNo;
                    //流水号
                    order.TranSequence = tranSequence = _sysDictionary.GetCurrentTranSequence();
                    string orderNo = _orderDao.CreateOrder(order);
                    order.OrderNo = orderNo;
                    //菜单品项序号
                    int seqNumber = _orderDetailsDao.GetSequenceNum(order.OrderID);
                    foreach (OrderDetails item in salesOrder.orderDetailsList)
                    {
                        item.DailyStatementNo = dailyStatementNo;
                        item.OrderBy = seqNumber;
                        _orderDetailsDao.CreateOrderDetails(item);
                        seqNumber++;
                    }
                    //折扣信息
                    if (salesOrder.orderDiscountList != null && salesOrder.orderDiscountList.Count > 0)
                    {
                        foreach (OrderDiscount item in salesOrder.orderDiscountList)
                        {
                            item.DailyStatementNo = dailyStatementNo;
                            _orderDiscountDao.CreateOrderDiscount(item);
                        }
                    }
                    //添加打印任务
                    SystemConfig systemConfig = _sysConfigDao.GetSystemConfigInfo();
                    if (systemConfig.IncludeKitchenPrint)
                    {
                        IList<PrintTask> printTaskList = PrintTaskService.GetInstance().GetPrintTaskList(salesOrder, systemConfig.PrintStyle, systemConfig.FollowStyle, systemConfig.PrintType, 1, string.Empty);
                        foreach (PrintTask printTask in printTaskList)
                        {
                            _printTaskDao.InsertPrintTask(printTask);
                        }
                    }
                }
                _daoManager.CommitTransaction();
            }
            catch
            {
                tranSequence = 0;
                _daoManager.RollBackTransaction();
            }
            return tranSequence;
        }

        /// <summary>
        /// 更新单据
        /// </summary>
        /// <returns>0:更新失败 1:更新成功 2:单据被其他设备占用</returns>
        public Int32 UpdateSalesOrder(SalesOrder salesOrder)
        {
            int result = 0;
            _daoManager.BeginTransaction();
            try
            {
                Order order = salesOrder.order;
                BizDesk desk = _deskDao.GetBizDeskByName(order.DeskName);
                if (desk == null || desk.DeviceNo == order.DeviceNo)
                {
                    if (_orderDao.UpdateOrder(order))
                    {
                        //日结号
                        string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                        //菜单品项序号
                        int seqNumber = _orderDetailsDao.GetSequenceNum(order.OrderID);
                        foreach (OrderDetails item in salesOrder.orderDetailsList)
                        {
                            item.OrderBy = seqNumber;
                            item.DailyStatementNo = dailyStatementNo;
                            _orderDetailsDao.UpdateOrderDetails(item);
                            seqNumber++;
                        }
                        //折扣信息
                        if (salesOrder.orderDiscountList != null && salesOrder.orderDiscountList.Count > 0)
                        {
                            foreach (OrderDiscount item in salesOrder.orderDiscountList)
                            {
                                _orderDiscountDao.UpdateOrderDiscount(item);
                            }
                        }
                        result = 1;
                    }
                }
                else
                {
                    result = 2;
                }
                _daoManager.CommitTransaction();
            }
            catch
            {
                result = 0;
                _daoManager.RollBackTransaction();
            }
            return result;
        }

        public SalesOrder GetSalesOrder(Guid orderID)
        {
            SalesOrder salesOrder = null;
            _daoManager.OpenConnection();

            Order order = _orderDao.GetOrder(orderID);
            if (order != null)
            {
                IList<OrderDetails> orderDetailsList = _orderDetailsDao.GetOrderDetailsList(orderID);
                salesOrder = new SalesOrder();
                salesOrder.order = order;
                salesOrder.orderDetailsList = orderDetailsList;
            }

            _daoManager.CloseConnection();
            return salesOrder;
        }

        public bool SplitSalesOrder(SalesSplitOrder salesSplitOrder)
        {
            bool returnValue = false;
            _daoManager.BeginTransaction();
            try
            {
                //更新账单表头价格
                if (_orderDao.UpdateSplitOrderPrice(salesSplitOrder.OriginalOrder))
                {
                    //减去分单出去的主体以及折扣
                    foreach (OrderDetails item in salesSplitOrder.SubOrderDetailsList)
                    {
                        _orderDetailsDao.SubtractSalesSplitOrder(item);
                    }
                    //新增账单
                    //日结号
                    string serialNumber = _dailyStatementDao.GetCurrentDailyStatementNo();
                    Order order = salesSplitOrder.NewOrder;
                    order.DailyStatementNo = serialNumber;
                    //分单号
                    Int32 curSubOrderNo = _orderDao.GetCurrentSubOrderNo(order.DeskName);
                    if (curSubOrderNo > 0)
                    {
                        curSubOrderNo++;
                    }
                    else
                    {
                        curSubOrderNo = 1;
                    }
                    order.SubOrderNo = curSubOrderNo;
                    //流水号
                    order.TranSequence = _sysDictionary.GetCurrentTranSequence();
                    _orderDao.CreateOrder(order);
                    //菜单品项序号
                    int seqNumber = _orderDetailsDao.GetSequenceNum(order.OrderID);
                    foreach (OrderDetails item in salesSplitOrder.NewOrderDetailsList)
                    {
                        item.OrderBy = seqNumber;
                        _orderDetailsDao.CreateOrderDetails(item);
                        seqNumber++;
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

        public SalesOrder GetSalesOrderByBillSearch(Guid orderID)
        {
            SalesOrder salesOrder = null;
            _daoManager.OpenConnection();

            Order order = _orderDao.GetOrder(orderID);
            if (order != null)
            {
                salesOrder = new SalesOrder();
                salesOrder.order = order;
                if (order.Status != 4)
                {
                    IList<OrderDetails> orderDetailsList = null;
                    IList<OrderDiscount> orderDiscountList = null;
                    if (order.Status == 0 || order.Status == 1 || order.Status == 3)
                    {
                        orderDetailsList = _orderDetailsDao.GetOrderDetailsList(orderID);
                        orderDiscountList = _orderDiscountDao.GetOrderDiscountList(orderID);
                    }
                    else if (order.Status == 2)
                    {
                        orderDetailsList = _orderDetailsDao.GetDeletedOrderDetailsList(orderID);
                        orderDiscountList = _orderDiscountDao.GetDeletedOrderDiscountList(orderID);
                    }
                    IList<OrderPayoff> orderPayoffList = null;
                    if (order.Status == 1)
                    {
                        orderPayoffList = _orderPayoffDao.GetOrderPayoffList(orderID);
                    }
                    else if (order.Status == 2)
                    {
                        orderPayoffList = _orderPayoffDao.GetDeletedOrderPayoffList(orderID);
                    }
                    salesOrder.orderDetailsList = orderDetailsList;
                    salesOrder.orderDiscountList = orderDiscountList;
                    salesOrder.orderPayoffList = orderPayoffList;
                }
            }

            _daoManager.CloseConnection();
            return salesOrder;
        }

        #endregion
    }
}
