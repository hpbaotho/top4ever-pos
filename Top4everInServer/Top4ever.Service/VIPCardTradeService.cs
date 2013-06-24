using System;
using System.Collections;
using System.Collections.Generic;

using IBatisNet.Common.Logging;
using IBatisNet.DataAccess;

using Top4ever.Domain.MembershipCard;
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

        #endregion

        #region Constructor

        private VIPCardTradeService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _VIPCardTradeDao = _daoManager.GetDao(typeof(IVIPCardTradeDao)) as IVIPCardTradeDao;
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

        #endregion
    }
}
