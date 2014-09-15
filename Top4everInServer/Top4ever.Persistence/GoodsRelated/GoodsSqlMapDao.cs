using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.GoodsRelated;
using Top4ever.Interface.GoodsRelated;

namespace Top4ever.Persistence.GoodsRelated
{
    /// <summary>
    /// Summary description for GoodsSqlMapDao
    /// </summary>
    public class GoodsSqlMapDao : BaseSqlMapDao, IGoodsDao
    {
        #region IGoodsDao Members

        public IList<Goods> GetGoodsListInGroup(Guid goodsGroupId)
        {
            return ExecuteQueryForList<Goods>("GetGoodsListByGroup", goodsGroupId);
        }

        public IList<Goods> GetGoodsList(IList<Guid> goodsIdList)
        {
            return ExecuteQueryForList<Goods>("GetGoodsList", goodsIdList);
        }

        public IList<Guid> GetDetailsGroupIDListInGoods(Guid goodsId)
        {
            return ExecuteQueryForList<Guid>("GetDetailsGroupIDListByGoodsID", goodsId);
        }

        public IList<GoodsCheckStock> GetGoodsCheckStock()
        {
            return ExecuteQueryForList<GoodsCheckStock>("GetGoodsCheckStock", null);
        }

        public Int32 UpdateReducedGoodsQty(Guid goodsId, decimal reducedQty)
        {
            Hashtable htParam = new Hashtable();
            htParam["GoodsID"] = goodsId;
            htParam["ReducedQty"] = reducedQty;
            htParam["ReturnValue"] = 0;
            ExecuteQueryForObject("UpdateReducedGoodsQty", htParam);
            int i = (int)htParam["ReturnValue"];    //返回值
            return i;
        }

        public IList<Guid> GetTopSaleGoodsId()
        {
            DateTime date = DateTime.Today.AddMonths(-1);
            return ExecuteQueryForList<Guid>("GetTopSaleGoodsId", date);
        }

        #endregion
    }
}
