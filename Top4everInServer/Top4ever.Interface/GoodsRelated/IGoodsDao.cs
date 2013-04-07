using System;
using System.Collections.Generic;

using Top4ever.Domain.GoodsRelated;

namespace Top4ever.Interface.GoodsRelated
{
    /// <summary>
    /// Summary description for IGoodsDao.
    /// </summary>
    public interface IGoodsDao
    {
        IList<Goods> GetGoodsListInGroup(Guid goodsGroupID);

        IList<Guid> GetDetailsGroupIDListInGoods(Guid goodsID);

        IList<GoodsCheckStock> GetGoodsCheckStock();

        Int32 UpdateReducedGoodsQty(Guid goodsID, decimal reducedQty);
    }
}
