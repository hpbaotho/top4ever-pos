using System;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Domain.Transfer;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for IDeskDao.
    /// </summary>
    public interface IDeskDao
    {
        IList<BizDesk> GetAllBizDeskByRegion(Guid regionId);

        IList<BizDesk> GetAllBizDesks();

        IList<string> GetAllDeskName();

        BizDesk GetBizDeskByName(string deskName);

        bool UpdateBizDeskStatus(BizDesk desk);

        IList<DeskRealTimeInfo> GetDeskRealTimeInfo(Guid regionId);

        IList<DeskInfo> GetDeskList();
    }
}
