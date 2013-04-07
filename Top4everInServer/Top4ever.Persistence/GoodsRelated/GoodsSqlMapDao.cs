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

        public IList<Goods> GetGoodsListInGroup(Guid goodsGroupID)
        {
            return ExecuteQueryForList<Goods>("GetGoodsListByGroup", goodsGroupID);
        }

        public IList<Guid> GetDetailsGroupIDListInGoods(Guid goodsID)
        {
            return ExecuteQueryForList<Guid>("GetDetailsGroupIDListByGoodsID", goodsID);
        }

        public IList<GoodsCheckStock> GetGoodsCheckStock()
        {
            return ExecuteQueryForList<GoodsCheckStock>("GetGoodsCheckStock", null);
        }

        public Int32 UpdateReducedGoodsQty(Guid goodsID, decimal reducedQty)
        {
            Hashtable htParam = new Hashtable();
            htParam["GoodsID"] = goodsID;
            htParam["ReducedQty"] = reducedQty;
            htParam["ReturnValue"] = 0;
            ExecuteQueryForObject("UpdateReducedGoodsQty", htParam);
            int i = (int)htParam["ReturnValue"];    //返回值
            return i;
        }
        #endregion
    }
}
