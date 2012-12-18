using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain.GoodsRelated;
using Top4ever.Interface.GoodsRelated;

namespace Top4ever.Persistence.GoodsRelated
{
    // <summary>
    /// Summary description for DetailsGroupSqlMapDao
    /// </summary>
    public class DetailsGroupSqlMapDao : BaseSqlMapDao, IDetailsGroupDao
    {
        public IList<DetailsGroup> GetAllDetailsGroup()
        {
            return ExecuteQueryForList<DetailsGroup>("GetDetailsGroupList", null);
        }
    }
}
