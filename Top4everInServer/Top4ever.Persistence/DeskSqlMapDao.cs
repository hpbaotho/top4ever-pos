using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for DeskSqlMapDao
    /// </summary>
    public class DeskSqlMapDao : BaseSqlMapDao, IDeskDao
    {
        #region IDeskDao Members

        public IList<BizDesk> GetAllBizDeskByRegion(Guid regionID)
        {
            return ExecuteQueryForList<BizDesk>("GetAllDeskInRegion", regionID);
        }

        public BizDesk GetBizDeskByName(string deskName)
        {
            return (ExecuteQueryForObject("GetDeskByName", deskName) as BizDesk);
        }

        public bool UpdateBizDeskStatus(BizDesk desk)
        {
            int result = 0;
            result = ExecuteUpdate("UpdateDeskStatus", desk);
            return result > 0;
        }

        public IList<DeskRealTimeInfo> GetDeskRealTimeInfo(Guid regionID)
        {
            return ExecuteQueryForList<DeskRealTimeInfo>("GetDeskInfo", new Hashtable { { "RegionID", regionID } });
        }

        #endregion
    }
}
