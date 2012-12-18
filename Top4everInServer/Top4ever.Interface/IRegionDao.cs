using System;
using System.Collections.Generic;

using Top4ever.Domain;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for IRegionDao.
    /// </summary>
    public interface IRegionDao
    {
        IList<BizRegion> GetAllBizRegion();
    }
}
