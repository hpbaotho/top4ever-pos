using System;
using System.Collections.Generic;

using Top4ever.Domain.OrderRelated;

namespace Top4ever.Interface.OrderRelated
{
    /// <summary>
    /// Summary description for IReasonDao.
    /// </summary>
    public interface IReasonDao
    {
        IList<Reason> GetAllReason();

        Reason GetReason(Guid reasonID);
    }
}
