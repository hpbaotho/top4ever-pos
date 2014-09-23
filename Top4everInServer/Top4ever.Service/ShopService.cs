using System;
using System.Collections;

using IBatisNet.DataAccess;
using Newtonsoft.Json;
using Top4ever.Domain;
using Top4ever.Interface;
using Top4ever.Utils;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for ShopService.
    /// </summary>
    public class ShopService
    {
        #region Private Fields

        private static readonly ShopService _instance = new ShopService();
        private readonly IDaoManager _daoManager;
        private readonly IShopDao _shopDao;

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
            try
            {
                _daoManager.OpenConnection();
                shop = _shopDao.GetSingleShop();
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetSingleShop]", exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return shop;
        }

        #endregion
    }
}
