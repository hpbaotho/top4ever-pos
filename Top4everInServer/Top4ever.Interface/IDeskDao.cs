using System;
using System.Collections.Generic;

using Top4ever.Domain;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for IDeskDao.
    /// </summary>
    public interface IDeskDao
    {
        IList<BizDesk> GetAllBizDeskByRegion(Guid regionID);

        BizDesk GetBizDeskByName(string deskName);

        bool UpdateBizDeskStatus(BizDesk desk);

        IList<DeskRealTimeInfo> GetDeskRealTimeInfo(Guid regionID);
    }
}
