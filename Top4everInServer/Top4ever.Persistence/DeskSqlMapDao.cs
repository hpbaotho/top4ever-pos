using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for DeskSqlMapDao
    /// </summary>
    public class DeskSqlMapDao : BaseSqlMapDao, IDeskDao
    {
        #region IDeskDao Members

        public IList<BizDesk> GetAllBizDeskByRegion(Guid regionId)
        {
            return ExecuteQueryForList<BizDesk>("GetAllDeskInRegion", regionId);
        }

        public IList<BizDesk> GetAllBizDesks()
        {
            return ExecuteQueryForList<BizDesk>("GetAllDesks", null);
        }

        public IList<string> GetAllDeskName()
        {
            return ExecuteQueryForList<string>("GetAllDeskName", null);
        }

        public BizDesk GetBizDeskByName(string deskName)
        {
            return (ExecuteQueryForObject("GetDeskByName", deskName) as BizDesk);
        }

        public bool UpdateBizDeskStatus(BizDesk desk)
        {
            int result = ExecuteUpdate("UpdateDeskStatus", desk);
            return result > 0;
        }

        public IList<DeskRealTimeInfo> GetDeskRealTimeInfo(Guid regionId)
        {
            return ExecuteQueryForList<DeskRealTimeInfo>("GetDeskInfo", new Hashtable { { "RegionID", regionId } });
        }

        public IList<DeskInfo> GetDeskList()
        {
            return ExecuteQueryForList<DeskInfo>("GetDeskList", null);
        }

        #endregion
    }
}
