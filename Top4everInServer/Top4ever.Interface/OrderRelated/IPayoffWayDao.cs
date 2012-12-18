using System;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;

namespace Top4ever.Interface.OrderRelated
{
    /// <summary>
    /// Summary description for IPayoffWayDao.
    /// </summary>
    public interface IPayoffWayDao
    {
        IList<PayoffWay> GetAllPayoffWay();
    }
}
