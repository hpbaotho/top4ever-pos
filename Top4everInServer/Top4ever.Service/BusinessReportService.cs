using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain.Transfer;
using Top4ever.Domain.MembershipCard;
using Top4ever.Interface;
using Top4ever.Interface.MembershipCard;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for BusinessReportService.
    /// </summary>
    public class BusinessReportService
    {
        #region Private Fields

        private static BusinessReportService _instance = new BusinessReportService();
        private IDaoManager _daoManager = null;
        private IBusinessReportDao _businessReportDao = null;
        private IDailyStatementDao _dailyStatementDao = null;
        private IVIPCardTradeDao _VIPCardTradeDao = null;

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

        public BusinessReport GetReportDataByHandover(string deviceNo)
        {
            BusinessReport reportData = null;
            _daoManager.BeginTransaction();
            try
            {
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
                        SalesPriceByDepart tempSalesPrice = new SalesPriceByDepart();
                        tempSalesPrice.DepartName = curDepartName;
                        tempSalesPrice.TotalDepartPrice = totalDepartPrice;
                        tempSalesPrice.TotalItemsNum = totalItemsNum;
                        tempSalesPrice.ItemsPriceList = tempItemsPriceList;
                        priceByDepartList.Add(tempSalesPrice);
                    }

                    reportData = new BusinessReport();
                    reportData.WorkSequence = businessReport.WorkSequence;
                    reportData.LastHandoverTime = businessReport.LastHandoverTime;
                    reportData.TotalRevenue = businessReport.TotalRevenue;
                    reportData.CutOffTotalPrice = businessReport.CutOffTotalPrice;
                    reportData.DiscountTotalPrice = businessReport.DiscountTotalPrice;
                    reportData.ActualTotalIncome = businessReport.ActualTotalIncome;
                    reportData.TotalServiceFee = businessReport.TotalServiceFee;
                    reportData.BillTotalQty = businessReport.BillTotalQty;
                    reportData.PeopleTotalNum = businessReport.PeopleTotalNum;
                    reportData.orderDiscountSumList = orderDiscountSumList;
                    reportData.orderPayoffSumList = orderPayoffSumList;
                    reportData.salesPriceByDepartList = priceByDepartList;
                }
                _daoManager.CommitTransaction();
            }
            catch
            {
                _daoManager.RollBackTransaction();
                reportData = null;
            }
            return reportData;
        }

        public BusinessReport GetReportDataByDailyStatement()
        {
            BusinessReport reportData = null;
            _daoManager.BeginTransaction();
            try
            {
                //日结
                string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
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
                        SalesPriceByDepart tempSalesPrice = new SalesPriceByDepart();
                        tempSalesPrice.DepartName = curDepartName;
                        tempSalesPrice.TotalDepartPrice = totalDepartPrice;
                        tempSalesPrice.TotalItemsNum = totalItemsNum;
                        tempSalesPrice.ItemsPriceList = tempItemsPriceList;
                        priceByDepartList.Add(tempSalesPrice);
                    }

                    reportData = new BusinessReport();
                    reportData.LastDailyStatementTime = businessReport.LastDailyStatementTime;
                    reportData.TotalRevenue = businessReport.TotalRevenue;
                    reportData.CutOffTotalPrice = businessReport.CutOffTotalPrice;
                    reportData.DiscountTotalPrice = businessReport.DiscountTotalPrice;
                    reportData.ActualTotalIncome = businessReport.ActualTotalIncome;
                    reportData.TotalServiceFee = businessReport.TotalServiceFee;
                    reportData.BillTotalQty = businessReport.BillTotalQty;
                    reportData.PeopleTotalNum = businessReport.PeopleTotalNum;
                    reportData.orderDiscountSumList = orderDiscountSumList;
                    reportData.cardStoredValueList = cardStoredValueList;
                    reportData.orderPayoffSumList = orderPayoffSumList;
                    reportData.salesPriceByDepartList = priceByDepartList;
                }
                _daoManager.CommitTransaction();
            }
            catch
            {
                _daoManager.RollBackTransaction();
                reportData = null;
            }
            return reportData;
        }

        public IList<GroupPrice> GetItemPriceListByGroup(DateTime beginDate, DateTime endDate)
        {
            IList<GroupPrice> groupPriceList = null;
            _daoManager.OpenConnection();
            groupPriceList = _businessReportDao.GetItemsPriceByGroup(beginDate, endDate);
            _daoManager.CloseConnection();
            return groupPriceList;
        }

        #endregion
    }
}
