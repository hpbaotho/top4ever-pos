using System;
using System.Collections.Generic;

using Top4ever.Domain;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for INoticeDao.
    /// </summary>
    public interface INoticeDao
    {
        IList<Notice> GetAllNotice();
    }
}
