using System;
using System.Collections;
using System.Collections.Generic;

using IBatisNet.DataAccess;
using Newtonsoft.Json;
using Top4ever.Domain.MembershipCard;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.MembershipCard;
using Top4ever.Utils;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for VIPCardTradeService.
    /// </summary>
    public class VIPCardTradeService
    {
        #region Private Fields

        private static readonly VIPCardTradeService _instance = new VIPCardTradeService();
        private readonly IDaoManager _daoManager;
        private readonly IVIPCardDao _VIPCardDao;
        private readonly IVIPCardTradeDao _VIPCardTradeDao;
        private readonly IDailyStatementDao _dailyStatementDao;

        #endregion

        #region Constructor

        private VIPCardTradeService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _VIPCardDao = _daoManager.GetDao(typeof(IVIPCardDao)) as IVIPCardDao;
            _VIPCardTradeDao = _daoManager.GetDao(typeof(IVIPCardTradeDao)) as IVIPCardTradeDao;
            _dailyStatementDao = _daoManager.GetDao(typeof(IDailyStatementDao)) as IDailyStatementDao;
        }

        #endregion

        #region Public methods

        public static VIPCardTradeService GetInstance()
        {
            return _instance;
        }

        public Int32 GetVIPCardTradeList(string cardNo, DateTime beginDate, DateTime endDate, out VIPCardTradeRecord cardTradeRecord)
        {
            cardTradeRecord = new VIPCardTradeRecord();
            VIPCard card = null;
            try
            {
                _daoManager.OpenConnection();
                card = _VIPCardDao.GetVIPCard(cardNo);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[GetVIPCardTradeList]参数：cardNo_{0},beginDate_{1},endDate_{2}", cardNo, beginDate, endDate), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            int result = 0;
            if (card != null)
            {
                result = card.Status;
            }
            if (result == 1)
            {
                if (card != null)
                {
                    cardTradeRecord.Balance = card.Balance;
                    cardTradeRecord.Integral = card.Integral;
                    cardTradeRecord.DiscountRate = card.DiscountRate;
                }
                cardTradeRecord.VIPCardTradeList = _VIPCardTradeDao.GetVIPCardTradeList(cardNo, beginDate, endDate);
            }
            return result;
        }

        public Int32 AddVIPCardStoredValue(VIPCardAddMoney cardMoney, out string tradePayNo)
        {
            int result = 0;
            tradePayNo = string.Empty;
            try
            {
                _daoManager.OpenConnection();
                //日结号
                string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                VIPCardStoredVaule cardStoredVaule = _VIPCardTradeDao.GetVIPCardStoredVaule(cardMoney.CardNo, cardMoney.StoreMoney);
                decimal giftAmount = 0M;
                int giftIntegral = 0;
                if (cardStoredVaule != null)
                {
                    decimal multiple = 1M;
                    if (cardStoredVaule.IsMultiple)
                    {
                        multiple = cardMoney.StoreMoney/cardStoredVaule.StoredVauleAmount;
                    }
                    if (cardStoredVaule.FixedAmount > 0)
                    {
                        giftAmount = cardStoredVaule.FixedAmount*multiple;
                    }
                    else
                    {
                        giftAmount = cardMoney.StoreMoney*cardStoredVaule.PresentAmountRate*multiple;
                    }
                    if (cardStoredVaule.FixedIntegral > 0)
                    {
                        giftIntegral = Convert.ToInt32(cardStoredVaule.FixedIntegral*multiple);
                    }
                    else
                    {
                        giftIntegral = Convert.ToInt32(cardMoney.StoreMoney*cardStoredVaule.PresentIntegralRate*multiple);
                    }
                }
                result = _VIPCardTradeDao.AddVIPCardStoredValue(cardMoney.CardNo, cardMoney.StoreMoney, giftAmount, giftIntegral, cardMoney.EmployeeNo, cardMoney.DeviceNo, dailyStatementNo, cardMoney.PayoffID, cardMoney.PayoffName, out tradePayNo);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[AddVIPCardStoredValue]参数：cardMoney_{0}", JsonConvert.SerializeObject(cardMoney)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public Int32 AddVIPCardPayment(VIPCardPayment cardPayment, out string tradePayNo)
        {
            int result = 0;
            tradePayNo = string.Empty;
            try
            {
                _daoManager.OpenConnection();
                //日结号
                string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
                result = _VIPCardTradeDao.AddVIPCardPayment(cardPayment.cardNo, cardPayment.payAmount, cardPayment.payIntegral, cardPayment.orderNo, cardPayment.employeeNo, cardPayment.deviceNo, dailyStatementNo, out tradePayNo);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[AddVIPCardPayment]参数：cardPayment_{0}", JsonConvert.SerializeObject(cardPayment)), exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        public Int32 RefundVIPCardPayment(string cardNo, string tradePayNo)
        {
            int result = 0;
            try
            {
                _daoManager.OpenConnection();
                result = _VIPCardTradeDao.RefundVIPCardPayment(cardNo, tradePayNo);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[RefundVIPCardPayment]参数：cardNo_{0},tradePayNo_{1}", cardNo, tradePayNo), exception);
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
