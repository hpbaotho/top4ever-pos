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
        IList<Goods> GetGoodsListInGroup(Guid goodsGroupId);

        IList<Goods> GetGoodsList(IList<Guid> goodsIdList);

        IList<Guid> GetDetailsGroupIDListByGoods(Guid goodsId);

        IList<GoodsDetailsGroup> GetDetailsGroupIdsInGoods();

        IList<GoodsCheckStock> GetGoodsCheckStock();

        Int32 UpdateReducedGoodsQty(Guid goodsId, decimal reducedQty);

        IList<Guid> GetTopSaleGoodsId();
    }
}
