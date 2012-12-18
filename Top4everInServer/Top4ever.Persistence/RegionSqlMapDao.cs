using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for RegionSqlMapDao
    /// </summary>
    public class RegionSqlMapDao : BaseSqlMapDao, IRegionDao
    {
        #region IRegionDao Members

        public IList<BizRegion> GetAllBizRegion()
        {
            return ExecuteQueryForList<BizRegion>("GetBizRegionList", null);
        }

        #endregion
    }
}
