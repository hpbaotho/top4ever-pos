using System;
using System.Collections.Generic;

using Top4ever.Domain.GoodsRelated;

namespace Top4ever.Entity
{
    public class GoodsUtil
    {
        public static bool IsGoodsInGroup(Guid goodsID, Guid goodsGroupID)
        {
            bool result = false;
            foreach (GoodsGroup goodsGroup in ConstantValuePool.GoodsGroupList)
            {
                foreach (Goods goods in goodsGroup.GoodsList)
                {
                    if (goods.GoodsID == goodsID)
                    {
                        if (goodsGroup.GoodsGroupID == goodsGroupID)
                        {
                            result = true;
                        }
                        break;
                    }
                }
                if (result)
                {
                    break;
                }
            }
            return result;
        }
    }
}
