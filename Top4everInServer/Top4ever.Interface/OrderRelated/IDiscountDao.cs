using System;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;

namespace Top4ever.Interface.OrderRelated
{
    /// <summary>
    /// Summary description for IDiscountDao.
    /// </summary>
    public interface IDiscountDao
    {
        IList<Discount> GetAllDiscount();
    }
}
