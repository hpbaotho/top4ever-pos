using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain.Transfer;
using Top4ever.Domain.MembershipCard;
using Top4ever.Interface;
using Top4ever.Interface.MembershipCard;
using Top4ever.Utils;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for BusinessReportService.
    /// </summary>
    public class BusinessReportService
    {
        #region Private Fields

        private static readonly BusinessReportService _instance = new BusinessReportService();
        private readonly IDaoManager _daoManager;
        private readonly IBusinessReportDao _businessReportDao;
        private readonly IDailyStatementDao _dailyStatementDao;
        private readonly IVIPCardTradeDao _VIPCardTradeDao;

        #endregion

        #region Constructor

        private BusinessReportService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _businessReportDao = _daoManager.GetDao(typeof(IBusinessReportDao)) as IBusinessReportDao;
            _dailyStatementDao = _daoManager.GetDao(typeof(IDailyStatementDao)) as IDailyStatementDao;
            _VIPCardTradeDao = _daoManager.GetDao(typeof(IVIPCardTradeDao)) as IVIPCardTradeDao;
        }

        #endregion

        #region Public methods

        public static BusinessReportService GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// 获取营业额统计
        /// </summary>
        /// <param name="deviceNo">设备号</param>
        /// <returns></returns>
        public BusinessReport GetReportDataByHandover(string deviceNo)
        {
            BusinessReport reportData = null;
            try
            {
                _daoManager.OpenConnection();
                //日结
                string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                if (!string.IsNullOrEmpty(dailyStatementNo))
                {
                    BusinessReport businessReport = _businessReportDao.GetTurnoverByHandover(dailyStatementNo, deviceNo);
                    int workSequence = businessReport.WorkSequence;
                    IList<OrderDiscountSum> orderDiscountSumList = _businessReportDao.GetOrderDiscountSumByHandover(dailyStatementNo, workSequence);
                    IList<OrderPayoffSum> orderPayoffSumList = _businessReportDao.GetOrderPayoffSumByHandover(dailyStatementNo, workSequence);
                    IList<ItemsPrice> itemsPriceList = _businessReportDao.GetItemsPriceByHandover(dailyStatementNo, workSequence);

                    IList<SalesPriceByDepart> priceByDepartList = new List<SalesPriceByDepart>();
                    if (itemsPriceList.Count > 0)
                    {
                        string curDepartName = itemsPriceList[0].DepartName;
                        decimal totalDepartPrice = 0;
                        decimal totalItemsNum = 0;
                        IList<ItemsPrice> tempItemsPriceList = new List<ItemsPrice>();
                        foreach (ItemsPrice item in itemsPriceList)
                        {
                            if (item.DepartName != curDepartName)
                            {
                                SalesPriceByDepart salesPrice = new SalesPriceByDepart();
                                salesPrice.DepartName = curDepartName;
                                salesPrice.TotalDepartPrice = totalDepartPrice;
                                salesPrice.TotalItemsNum = totalItemsNum;
                                salesPrice.ItemsPriceList = tempItemsPriceList;
                                priceByDepartList.Add(salesPrice);

                                curDepartName = item.DepartName;
                                totalDepartPrice = item.ItemsTotalPrice;
                                totalItemsNum = item.ItemsTotalQty;
                                tempItemsPriceList = new List<ItemsPrice>();
                            }
                            else
                            {
                                totalDepartPrice += item.ItemsTotalPrice;
                                totalItemsNum += item.ItemsTotalQty;
                            }
                            tempItemsPriceList.Add(item);
                        }
                        SalesPriceByDepart tempSalesPrice = new SalesPriceByDepart
                        {
                            DepartName = curDepartName,
                            TotalDepartPrice = totalDepartPrice,
                            TotalItemsNum = totalItemsNum,
                            ItemsPriceList = tempItemsPriceList
                        };
                        priceByDepartList.Add(tempSalesPrice);
                    }
                    reportData = new BusinessReport
                    {
                        WorkSequence = businessReport.WorkSequence,
                        LastHandoverTime = businessReport.LastHandoverTime,
                        TotalRevenue = businessReport.TotalRevenue,
                        CutOffTotalPrice = businessReport.CutOffTotalPrice,
                        DiscountTotalPrice = businessReport.DiscountTotalPrice,
                        ActualTotalIncome = businessReport.ActualTotalIncome,
                        TotalServiceFee = businessReport.TotalServiceFee,
                        BillTotalQty = businessReport.BillTotalQty,
                        PeopleTotalNum = businessReport.PeopleTotalNum,
                        orderDiscountSumList = orderDiscountSumList,
                        orderPayoffSumList = orderPayoffSumList,
                        salesPriceByDepartList = priceByDepartList
                    };
                }
            }
            catch (Exception exception)
            {
                reportData = null;
                LogHelper.GetInstance().Error("[GetReportDataByHandover]参数：deviceNo_" + deviceNo, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return reportData;
        }

        /// <summary>
        /// 获取营业额统计
        /// </summary>
        /// <param name="handoverRecordId">交班Id</param>
        /// <returns></returns>
        public BusinessReport GetReportDataByHandoverRecordId(Guid handoverRecordId)
        {
            BusinessReport businessReport = null;
            IList<OrderDiscountSum> orderDiscountSumList = null;
            IList<OrderPayoffSum> orderPayoffSumList = null;
            IList<ItemsPrice> itemsPriceList = null;
            try
            {
                _daoManager.OpenConnection();
                businessReport = _businessReportDao.GetTurnoverByHandoverRecordID(handoverRecordId);
                string dailyStatementNo = businessReport.DailyStatementNo;
                int workSequence = businessReport.WorkSequence;
                orderDiscountSumList = _businessReportDao.GetOrderDiscountSumByHandover(dailyStatementNo, workSequence);
                orderPayoffSumList = _businessReportDao.GetOrderPayoffSumByHandover(dailyStatementNo, workSequence);
                itemsPriceList = _businessReportDao.GetItemsPriceByHandover(dailyStatementNo, workSequence);
            }
            catch (Exception exception)
            {
                businessReport = null;
                LogHelper.GetInstance().Error("[GetReportDataByHandoverRecordID]参数：handoverRecordId_" + handoverRecordId, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            if (itemsPriceList == null || itemsPriceList.Count <= 0 || businessReport == null) return null;
            IList<SalesPriceByDepart> priceByDepartList = new List<SalesPriceByDepart>();
            if (itemsPriceList.Count > 0)
            {
                string curDepartName = itemsPriceList[0].DepartName;
                decimal totalDepartPrice = 0;
                decimal totalItemsNum = 0;
                IList<ItemsPrice> tempItemsPriceList = new List<ItemsPrice>();
                foreach (ItemsPrice item in itemsPriceList)
                {
                    if (item.DepartName != curDepartName)
                    {
                        SalesPriceByDepart salesPrice = new SalesPriceByDepart();
                        salesPrice.DepartName = curDepartName;
                        salesPrice.TotalDepartPrice = totalDepartPrice;
                        salesPrice.TotalItemsNum = totalItemsNum;
                        salesPrice.ItemsPriceList = tempItemsPriceList;
                        priceByDepartList.Add(salesPrice);

                        curDepartName = item.DepartName;
                        totalDepartPrice = item.ItemsTotalPrice;
                        totalItemsNum = item.ItemsTotalQty;
                        tempItemsPriceList = new List<ItemsPrice>();
                    }
                    else
                    {
                        totalDepartPrice += item.ItemsTotalPrice;
                        totalItemsNum += item.ItemsTotalQty;
                    }
                    tempItemsPriceList.Add(item);
                }
                SalesPriceByDepart tempSalesPrice = new SalesPriceByDepart();
                tempSalesPrice.DepartName = curDepartName;
                tempSalesPrice.TotalDepartPrice = totalDepartPrice;
                tempSalesPrice.TotalItemsNum = totalItemsNum;
                tempSalesPrice.ItemsPriceList = tempItemsPriceList;
                priceByDepartList.Add(tempSalesPrice);
            }
            BusinessReport reportData = new BusinessReport
            {
                WorkSequence = businessReport.WorkSequence,
                LastHandoverTime = businessReport.LastHandoverTime,
                TotalRevenue = businessReport.TotalRevenue,
                CutOffTotalPrice = businessReport.CutOffTotalPrice,
                DiscountTotalPrice = businessReport.DiscountTotalPrice,
                ActualTotalIncome = businessReport.ActualTotalIncome,
                TotalServiceFee = businessReport.TotalServiceFee,
                BillTotalQty = businessReport.BillTotalQty,
                PeopleTotalNum = businessReport.PeopleTotalNum,
                orderDiscountSumList = orderDiscountSumList,
                orderPayoffSumList = orderPayoffSumList,
                salesPriceByDepartList = priceByDepartList
            };
            return reportData;
        }

        /// <summary>
        /// 获取日结营业额统计
        /// </summary>
        /// <param name="dailyStatementNo">日结号</param>
        /// <returns></returns>
        public BusinessReport GetReportDataByDailyStatement(string dailyStatementNo)
        {
            BusinessReport reportData = null;
            try
            {
                _daoManager.OpenConnection();
                //日结
                if (string.IsNullOrEmpty(dailyStatementNo))
                {
                    dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                }
                if (!string.IsNullOrEmpty(dailyStatementNo))
                {
                    BusinessReport businessReport = _businessReportDao.GetTurnoverByDailyStatement(dailyStatementNo);
                    IList<OrderDiscountSum> orderDiscountSumList = _businessReportDao.GetOrderDiscountSumByDailyStatement(dailyStatementNo);
                    IList<VIPCardTrade> cardStoredValueList = _VIPCardTradeDao.GetAllStoredAmount(dailyStatementNo);
                    IList<OrderPayoffSum> orderPayoffSumList = _businessReportDao.GetOrderPayoffSumByDailyStatement(dailyStatementNo);
                    IList<ItemsPrice> itemsPriceList = _businessReportDao.GetItemsPriceByDailyStatement(dailyStatementNo);

                    IList<SalesPriceByDepart> priceByDepartList = new List<SalesPriceByDepart>();
                    if (itemsPriceList.Count > 0)
                    {
                        string curDepartName = itemsPriceList[0].DepartName;
                        decimal totalDepartPrice = 0;
                        decimal totalItemsNum = 0;
                        IList<ItemsPrice> tempItemsPriceList = new List<ItemsPrice>();
                        foreach (ItemsPrice item in itemsPriceList)
                        {
                            if (item.DepartName != curDepartName)
                            {
                                SalesPriceByDepart salesPrice = new SalesPriceByDepart();
                                salesPrice.DepartName = curDepartName;
                                salesPrice.TotalDepartPrice = totalDepartPrice;
                                salesPrice.TotalItemsNum = totalItemsNum;
                                salesPrice.ItemsPriceList = tempItemsPriceList;
                                priceByDepartList.Add(salesPrice);

                                curDepartName = item.DepartName;
                                totalDepartPrice = item.ItemsTotalPrice;
                                totalItemsNum = item.ItemsTotalQty;
                                tempItemsPriceList = new List<ItemsPrice>();
                            }
                            else
                            {
                                totalDepartPrice += item.ItemsTotalPrice;
                                totalItemsNum += item.ItemsTotalQty;
                            }
                            tempItemsPriceList.Add(item);
                        }
                        SalesPriceByDepart tempSalesPrice = new SalesPriceByDepart
                        {
                            DepartName = curDepartName, 
                            TotalDepartPrice = totalDepartPrice, 
                            TotalItemsNum = totalItemsNum, 
                            ItemsPriceList = tempItemsPriceList
                        };
                        priceByDepartList.Add(tempSalesPrice);
                    }
                    reportData = new BusinessReport
                    {
                        DailyStatementNo = dailyStatementNo,
                        LastDailyStatementTime = businessReport.LastDailyStatementTime,
                        TotalRevenue = businessReport.TotalRevenue,
                        CutOffTotalPrice = businessReport.CutOffTotalPrice,
                        DiscountTotalPrice = businessReport.DiscountTotalPrice,
                        ActualTotalIncome = businessReport.ActualTotalIncome,
                        TotalServiceFee = businessReport.TotalServiceFee,
                        BillTotalQty = businessReport.BillTotalQty,
                        PeopleTotalNum = businessReport.PeopleTotalNum,
                        orderDiscountSumList = orderDiscountSumList,
                        cardStoredValueList = cardStoredValueList,
                        orderPayoffSumList = orderPayoffSumList,
                        salesPriceByDepartList = priceByDepartList
                    };
                }
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetReportDataByDailyStatement]参数：dailyStatementNo_" + dailyStatementNo, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return reportData;
        }

        public IList<GroupPrice> GetItemPriceListByGroup(DateTime beginDate, DateTime endDate)
        {
            IList<GroupPrice> groupPriceList = null;
            try
            {
                _daoManager.OpenConnection();
                groupPriceList = _businessReportDao.GetItemsPriceByGroup(beginDate, endDate);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetItemPriceListByGroup]参数：beginDate_{0},endDate_{1}", beginDate, endDate), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return groupPriceList;
        }

        #endregion
    }
}
