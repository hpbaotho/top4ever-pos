using System;
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
        public IList<Details> GetDetailsListInGroup(Guid detailsGroupId)
        {
            return ExecuteQueryForList<Details>("GetDetailsListByGroup", detailsGroupId);
        }

        public IList<Details> GetAllDetails()
        {
            return ExecuteQueryForList<Details>("GetAllDetails", null);
        }

        public IList<Guid> GetDetailsGroupIDListInDetails(Guid detailsId)
        {
            return ExecuteQueryForList<Guid>("GetDetailsGroupIDListByDetailsID", detailsId);
        }

        public IList<DetailsDetailsGroup> GetDetailsGroupIdsInDetails()
        {
            return ExecuteQueryForList<DetailsDetailsGroup>("GetDetailsGroupIdsInDetails", null);
        }
    }
}
