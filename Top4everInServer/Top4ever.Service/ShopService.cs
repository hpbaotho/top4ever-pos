using System;
using System.Collections;

using IBatisNet.DataAccess;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for ShopService.
    /// </summary>
    public class ShopService
    {
        #region Private Fields

        private static ShopService _instance = new ShopService();
        private IDaoManager _daoManager = null;
        private IShopDao _shopDao = null;

        #endregion

        #region Constructor

        private ShopService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _shopDao = _daoManager.GetDao(typeof(IShopDao)) as IShopDao;
        }

        #endregion

        #region Public methods

        public static ShopService GetInstance()
        {
            return _instance;
        }

        public Shop GetSingleShop()
        {
            Shop shop = null;

            _daoManager.OpenConnection();
            shop = _shopDao.GetSingleShop();
            _daoManager.CloseConnection();

            return shop;
        }

        #endregion
    }
}
