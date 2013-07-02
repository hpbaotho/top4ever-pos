using System;
using System.Collections;
using System.Collections.Generic;

using IBatisNet.Common.Logging;
using IBatisNet.DataAccess;

using Top4ever.Domain.MembershipCard;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;
using Top4ever.Interface.MembershipCard;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for VIPCardTradeService.
    /// </summary>
    public class VIPCardTradeService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(VIPCardTradeService));

        #region Private Fields

        private static VIPCardTradeService _instance = new VIPCardTradeService();
        private IDaoManager _daoManager = null;
        private IVIPCardTradeDao _VIPCardTradeDao = null;
        private IDailyStatementDao _dailyStatementDao = null;

        #endregion

        #region Constructor

        private VIPCardTradeService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _VIPCardTradeDao = _daoManager.GetDao(typeof(IVIPCardTradeDao)) as IVIPCardTradeDao;
            _dailyStatementDao = _daoManager.GetDao(typeof(IDailyStatementDao)) as IDailyStatementDao;
        }

        #endregion

        #region Public methods

        public static VIPCardTradeService GetInstance()
        {
            return _instance;
        }

        public IList<VIPCardTrade> GetVIPCardTradeList(string cardNo, string beginDate, string endDate)
        {
            IList<VIPCardTrade> cardTradeList = null;

            _daoManager.OpenConnection();
            cardTradeList = _VIPCardTradeDao.GetVIPCardTradeList(cardNo, beginDate, endDate);
            _daoManager.CloseConnection();

            return cardTradeList;
        }

        public Int32 AddVIPCardStoredValue(VIPCardAddMoney cardMoney, out string tradePayNo)
        {
            int result = 0;
            tradePayNo = string.Empty;
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
                    multiple = cardMoney.StoreMoney / cardStoredVaule.StoredVauleAmount;
                }
                if (cardStoredVaule.FixedAmount > 0)
                {
                    giftAmount = cardStoredVaule.FixedAmount * multiple;
                }
                else
                {
                    giftAmount = cardMoney.StoreMoney * cardStoredVaule.PresentAmountRate * multiple;
                }
                if (cardStoredVaule.FixedIntegral > 0)
                {
                    giftIntegral = Convert.ToInt32(cardStoredVaule.FixedIntegral * multiple);
                }
                else
                {
                    giftIntegral = Convert.ToInt32(cardMoney.StoreMoney * cardStoredVaule.PresentIntegralRate * multiple);
                }
            }
            result = _VIPCardTradeDao.AddVIPCardStoredValue(cardMoney.CardNo, cardMoney.StoreMoney, giftAmount, giftIntegral, cardMoney.EmployeeNo, cardMoney.DeviceNo, dailyStatementNo, cardMoney.PayoffID, cardMoney.PayoffName, out tradePayNo);
            _daoManager.CloseConnection();
            return result;
        }

        public Int32 AddVIPCardPayment(VIPCardPayment cardPayment, out string tradePayNo)
        {
            int result = 0;
            tradePayNo = string.Empty;
            _daoManager.OpenConnection();
            //日结号
            string dailyStatementNo = _dailyStatementDao.GetCurrentDailyStatementNo();
            result = _VIPCardTradeDao.AddVIPCardPayment(cardPayment.cardNo, cardPayment.payAmount, cardPayment.payIntegral, cardPayment.orderNo, cardPayment.employeeNo, cardPayment.deviceNo, dailyStatementNo, out tradePayNo);
            _daoManager.CloseConnection();
            return result;
        }
        #endregion
    }
}
