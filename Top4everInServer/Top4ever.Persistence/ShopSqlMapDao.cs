using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for ShopSqlMapDao
    /// </summary>
    public class ShopSqlMapDao : BaseSqlMapDao, IShopDao
    {
        #region IShopDao Members

        public Shop GetSingleShop()
        {
            return (ExecuteQueryForObject("GetShopInfo", null) as Shop);
        }

        #endregion
    }
}
