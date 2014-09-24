using System;
using System.Collections.Generic;
using System.Linq;
using IBatisNet.DataAccess;
using Newtonsoft.Json;
using Top4ever.Domain;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.GoodsRelated;
using Top4ever.Interface.OrderRelated;
using Top4ever.Utils;

namespace Top4ever.Service
{
    public class SalesOrderService
    {
        #region Private Fields

        private static readonly SalesOrderService _instance = new SalesOrderService();
        private readonly IDaoManager _daoManager;
        private readonly IOrderDao _orderDao;
        private readonly IOrderDetailsDao _orderDetailsDao;
        private readonly IOrderDiscountDao _orderDiscountDao;
        private readonly IOrderPayoffDao _orderPayoffDao;
        private readonly IDailyStatementDao _dailyStatementDao;
        private readonly ISystemConfigDao _sysConfigDao;
        private readonly ISystemDictionaryDao _sysDictionary;
        private readonly IDeskDao _deskDao;
        private readonly IPrintTaskDao _printTaskDao;
        private readonly IGoodsDao _goodsDao;

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
            _goodsDao = _daoManager.GetDao(typeof(IGoodsDao)) as IGoodsDao;
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
            catch(Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[CreateSalesOrder]参数：salesOrder_{0}", JsonConvert.SerializeObject(salesOrder)), exception);
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
            catch(Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[UpdateSalesOrder]参数：salesOrder_{0}", JsonConvert.SerializeObject(salesOrder)), exception);
                result = 0;
                _daoManager.RollBackTransaction();
            }
            return result;
        }

        public SalesOrder GetSalesOrder(Guid orderId)
        {
            SalesOrder salesOrder = null;
            try
            {
                _daoManager.OpenConnection();
                Order order = _orderDao.GetOrder(orderId);
                if (order != null)
                {
                    IList<OrderDetails> orderDetailsList = _orderDetailsDao.GetOrderDetailsList(orderId);
                    salesOrder = new SalesOrder
                    {
                        order = order, 
                        orderDetailsList = orderDetailsList
                    };
                }
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetSalesOrder]参数：orderId_{0}", orderId), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
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
            catch(Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[SplitSalesOrder]参数：salesSplitOrder_{0}", JsonConvert.SerializeObject(salesSplitOrder)), exception);
                _daoManager.RollBackTransaction();
                returnValue = false;
            }
            return returnValue;
        }

        public SalesOrder GetSalesOrderByBillSearch(Guid orderId)
        {
            SalesOrder salesOrder = null;
            try
            {
                _daoManager.OpenConnection();
                Order order = _orderDao.GetOrder(orderId);
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
                            orderDetailsList = _orderDetailsDao.GetOrderDetailsList(orderId);
                            orderDiscountList = _orderDiscountDao.GetOrderDiscountList(orderId);
                        }
                        else if (order.Status == 2)
                        {
                            orderDetailsList = _orderDetailsDao.GetDeletedOrderDetailsList(orderId);
                            orderDiscountList = _orderDiscountDao.GetDeletedOrderDiscountList(orderId);
                        }
                        IList<OrderPayoff> orderPayoffList = null;
                        if (order.Status == 1)
                        {
                            orderPayoffList = _orderPayoffDao.GetOrderPayoffList(orderId);
                        }
                        else if (order.Status == 2)
                        {
                            orderPayoffList = _orderPayoffDao.GetDeletedOrderPayoffList(orderId);
                        }
                        salesOrder.orderDetailsList = orderDetailsList;
                        salesOrder.orderDiscountList = orderDiscountList;
                        salesOrder.orderPayoffList = orderPayoffList;
                    }
                }
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetSalesOrderByBillSearch]参数：orderId_{0}", orderId), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return salesOrder;
        }

        public bool CreateOrderInAndroid(string deskName, int peopleNum, Guid employeeId, string employeeNo, IList<OrderDetail> orderDetailList)
        {
            bool isSuccess = false;
            if (orderDetailList != null && orderDetailList.Count > 0)
            {
                _daoManager.BeginTransaction();
                try
                {
                    //日结号
                    string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                    if (!string.IsNullOrEmpty(dailyStatementNo))
                    {
                        decimal totalPrice = 0, actualPayMoney = 0, totalDiscount = 0;
                        foreach (var orderDetail in orderDetailList)
                        {
                            totalPrice += orderDetail.SellPrice*orderDetail.GoodsQty;
                            actualPayMoney += orderDetail.SellPrice*orderDetail.GoodsQty - orderDetail.TotalDiscount;
                            totalDiscount += orderDetail.TotalDiscount;
                        }
                        const string deviceNo = "AD";
                        //批量获取品项
                        List<Guid> goodsIdList = orderDetailList.Select(orderDetail => orderDetail.GoodsId).ToList();
                        IList<Goods> goodsList = _goodsDao.GetGoodsList(goodsIdList);
                        if (goodsList != null && goodsList.Count > 0)
                        {
                            //订单头部
                            Order order = new Order
                            {
                                OrderID = Guid.NewGuid(),
                                TotalSellPrice = totalPrice,
                                ActualSellPrice = actualPayMoney,
                                DiscountPrice = totalDiscount,
                                CutOffPrice = 0,
                                ServiceFee = 0,
                                DeviceNo = deviceNo,
                                DeskName = deskName,
                                EatType = (int) EatWayType.DineIn,
                                Status = 0,
                                PeopleNum = peopleNum,
                                EmployeeID = employeeId,
                                EmployeeNo = employeeNo,
                                DailyStatementNo = dailyStatementNo
                            };
                            //分单号
                            Int32 curSubOrderNo = _orderDao.GetCurrentSubOrderNo(deskName);
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
                            string orderNo = _orderDao.CreateOrder(order);
                            order.OrderNo = orderNo;
                            //菜单品项序号
                            IList<OrderDetails> orderDetailsList = new List<OrderDetails>();
                            int seqNumber = _orderDetailsDao.GetSequenceNum(order.OrderID);
                            foreach (OrderDetail item in orderDetailList)
                            {
                                Goods goods = goodsList.FirstOrDefault(g => g.GoodsID.Equals(item.GoodsId));
                                if (goods != null)
                                {
                                    OrderDetails orderDetails = new OrderDetails
                                    {
                                        OrderDetailsID = Guid.NewGuid(),
                                        OrderID = order.OrderID,
                                        DeviceNo = deviceNo,
                                        TotalSellPrice = item.SellPrice*item.GoodsQty,
                                        TotalDiscount = item.TotalDiscount,
                                        ItemQty = item.GoodsQty,
                                        EmployeeID = employeeId,
                                        ItemType = (int) OrderItemType.Goods,
                                        GoodsID = item.GoodsId,
                                        GoodsNo = goods.GoodsNo,
                                        GoodsName = item.GoodsName,
                                        Unit = goods.Unit,
                                        CanDiscount = goods.CanDiscount,
                                        SellPrice = item.SellPrice,
                                        PrintSolutionName = goods.PrintSolutionName,
                                        DepartID = goods.DepartID,
                                        DailyStatementNo = dailyStatementNo,
                                        OrderBy = seqNumber
                                    };
                                    orderDetailsList.Add(orderDetails);
                                    _orderDetailsDao.CreateOrderDetails(orderDetails);
                                    seqNumber++;
                                    if (!string.IsNullOrEmpty(item.Remark))
                                    {
                                        //自定义口味
                                        orderDetails = new OrderDetails
                                        {
                                            OrderDetailsID = Guid.NewGuid(),
                                            OrderID = order.OrderID,
                                            DeviceNo = deviceNo,
                                            TotalSellPrice = 0,
                                            TotalDiscount = 0,
                                            ItemLevel = 1,
                                            ItemQty = item.GoodsQty,
                                            EmployeeID = employeeId,
                                            ItemType = (int) OrderItemType.Details,
                                            GoodsID = new Guid("77777777-7777-7777-7777-777777777777"),
                                            GoodsNo = "7777",
                                            GoodsName = item.Remark,
                                            Unit = "",
                                            CanDiscount = false,
                                            SellPrice = 0,
                                            PrintSolutionName = goods.PrintSolutionName,
                                            DepartID = goods.DepartID,
                                            DailyStatementNo = dailyStatementNo,
                                            OrderBy = seqNumber
                                        };
                                        orderDetailsList.Add(orderDetails);
                                        _orderDetailsDao.CreateOrderDetails(orderDetails);
                                        seqNumber++;
                                    }
                                }
                            }
                            //折扣信息
                            //if (salesOrder.orderDiscountList != null && salesOrder.orderDiscountList.Count > 0)
                            //{
                            //    foreach (OrderDiscount item in salesOrder.orderDiscountList)
                            //    {
                            //        item.DailyStatementNo = dailyStatementNo;
                            //        _orderDiscountDao.CreateOrderDiscount(item);
                            //    }
                            //}
                            SalesOrder salesOrder = new SalesOrder
                            {
                                order = order,
                                orderDetailsList = orderDetailsList
                            };
                            //salesOrder.orderDiscountList = newOrderDiscountList;
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
                    }
                    _daoManager.CommitTransaction();
                    isSuccess = true;
                }
                catch(Exception exception)
                {
                    LogHelper.GetInstance().Error(string.Format("[CreateOrderInAndroid]参数：deskName_{0},peopleNum_{1},employeeId_{2},employeeNo_{3},orderDetailList_{4}", deskName, peopleNum, employeeId, employeeNo, JsonConvert.SerializeObject(orderDetailList)), exception);
                    isSuccess = false;
                    _daoManager.RollBackTransaction();
                }
            }
            return isSuccess;
        }

        private enum EatWayType
        {
            /// <summary>
            /// 堂食
            /// </summary>
            DineIn = 1,
            /// <summary>
            /// 外带
            /// </summary>
            Takeout = 2,
            /// <summary>
            /// 外送
            /// </summary>
            OutsideOrder = 3
        }

        private enum OrderItemType
        {
            /// <summary>
            /// 菜品
            /// </summary>
            Goods = 1,
            /// <summary>
            /// 细项
            /// </summary>
            Details = 2,
            /// <summary>
            /// 套餐
            /// </summary>
            SetMeal = 3
        }

        #endregion
    }
}
