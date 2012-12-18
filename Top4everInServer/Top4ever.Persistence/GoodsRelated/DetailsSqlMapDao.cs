using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.GoodsRelated;
using Top4ever.Interface.GoodsRelated;

namespace Top4ever.Persistence.GoodsRelated
{
    /// <summary>
    /// Summary description for DetailsSqlMapDao
    /// </summary>
    public class DetailsSqlMapDao : BaseSqlMapDao, IDetailsDao
    {
        public IList<Details> GetDetailsListInGroup(Guid detailsGroupID)
        {
            return ExecuteQueryForList<Details>("GetDetailsListByGroup", detailsGroupID);
        }

        public IList<Guid> GetDetailsGroupIDListInDetails(Guid detailsID)
        {
            return ExecuteQueryForList<Guid>("GetDetailsGroupIDListByDetailsID", detailsID);
        }
    }
}
