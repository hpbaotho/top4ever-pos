using System;
using System.Collections.Generic;

using Top4ever.Domain;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for ISysConfigDao.
    /// </summary>
    public interface ISystemConfigDao
    {
        SystemConfig GetSystemConfigInfo();
    }
}
