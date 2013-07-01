using System;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for NoticeSqlMapDao
    /// </summary>
    public class NoticeSqlMapDao : BaseSqlMapDao, INoticeDao
    {
        #region INoticeDao Members

        public IList<Notice> GetAllNotice()
        {
            return ExecuteQueryForList<Notice>("GetNoticeList", null);
        }

        #endregion
    }
}
