using System;
using System.Collections.Generic;

using Top4ever.Domain.GoodsRelated;

namespace Top4ever.Interface.GoodsRelated
{
    /// <summary>
    /// Summary description for IGoodsGroupDao.
    /// </summary>
    public interface IGoodsGroupDao
    {
        IList<GoodsGroup> GetAllGoodsGroup();
    }
}
