using System;
using System.Collections.Generic;

using Top4ever.Domain.GoodsRelated;

namespace Top4ever.Interface.GoodsRelated
{
    /// <summary>
    /// Summary description for IDetailsDao.
    /// </summary>
    public interface IDetailsDao
    {
        IList<Details> GetDetailsListInGroup(Guid detailsGroupId);

        IList<Details> GetAllDetails();

        IList<Guid> GetDetailsGroupIDListInDetails(Guid detailsId);

        IList<DetailsDetailsGroup> GetDetailsGroupIdsInDetails();
    }
}
