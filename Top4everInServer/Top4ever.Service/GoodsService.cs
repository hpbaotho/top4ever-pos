using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain.GoodsRelated;
using Top4ever.Interface.GoodsRelated;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for GoodsService.
    /// </summary>
    public class GoodsService
    {
        #region Private Fields

        private static GoodsService _instance = new GoodsService();
        private IDaoManager _daoManager = null;
        private IGoodsDao _goodsDao = null;

        #endregion

        #region Constructor

        private GoodsService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _goodsDao = _daoManager.GetDao(typeof(IGoodsDao)) as IGoodsDao;
        }

        #endregion

        #region Public methods

        public static GoodsService GetInstance()
        {
            return _instance;
        }

        public IList<GoodsCheckStock> GetGoodsCheckStock()
        {
            IList<GoodsCheckStock> goodsCheckStockList = null;

            _daoManager.OpenConnection();
            goodsCheckStockList = _goodsDao.GetGoodsCheckStock();
            _daoManager.CloseConnection();

            return goodsCheckStockList;
        }

        public string UpdateReducedGoodsQty(IList<GoodsCheckStock> goodsCheckStockList)
        {
            string goodsName = string.Empty;
            _daoManager.BeginTransaction();
            foreach (GoodsCheckStock item in goodsCheckStockList)
            {
                int result = _goodsDao.UpdateReducedGoodsQty(item.GoodsID, item.ReducedQuantity);
                if (result != 1)
                {
                    goodsName = item.GoodsName;
                    break;
                }
            }
            if (string.IsNullOrEmpty(goodsName))
            {
                _daoManager.CommitTransaction();
            }
            else
            {
                _daoManager.RollBackTransaction();
            }
            return goodsName;
        }

        #endregion
    }
}
