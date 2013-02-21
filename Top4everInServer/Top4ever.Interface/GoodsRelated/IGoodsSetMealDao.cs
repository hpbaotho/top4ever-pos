using System;
using System.Collections.Generic;

using Top4ever.Domain.GoodsRelated;

namespace Top4ever.Interface.GoodsRelated
{
    /// <summary>
    /// Summary description for IGoodsSetMealDao.
    /// </summary>
    public interface IGoodsSetMealDao
    {
        IList<GoodsSetMeal> GetAllGoodsSetMeal();

        IList<GoodsSetMeal> GetAllDetailsSetMeal();
    }
}
