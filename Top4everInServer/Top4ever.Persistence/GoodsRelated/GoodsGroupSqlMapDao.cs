﻿using System.Collections.Generic;

using Top4ever.Domain.GoodsRelated;
using Top4ever.Interface.GoodsRelated;

namespace Top4ever.Persistence.GoodsRelated
{
    /// <summary>
    /// Summary description for GoodsGroupSqlMapDao
    /// </summary>
    public class GoodsGroupSqlMapDao : BaseSqlMapDao, IGoodsGroupDao
    {
        public IList<GoodsGroup> GetAllGoodsGroup()
        {
            return ExecuteQueryForList<GoodsGroup>("GetGoodsGroupList", null);
        }

        public IList<GoodsCronTrigger> GetAllGoodsCronTrigger()
        {
            return ExecuteQueryForList<GoodsCronTrigger>("GetGoodsCronTriggerList", null);
        }


        public IList<GoodsLimitedTimeSale> GetAllGoodsLimitedTimeSale()
        {
            return ExecuteQueryForList<GoodsLimitedTimeSale>("GetGoodsLimitedTimeSaleList", null);
        }

        public IList<GoodsCombinedSale> GetAllGoodsCombinedSale()
        {
            return ExecuteQueryForList<GoodsCombinedSale>("GetGoodsCombinedSaleList", null);
        }
    }
}
