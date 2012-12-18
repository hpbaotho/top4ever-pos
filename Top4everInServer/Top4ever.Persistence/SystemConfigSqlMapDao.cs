using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for SysConfigSqlMapDao
    /// </summary>
    public class SystemConfigSqlMapDao : BaseSqlMapDao, ISystemConfigDao
    {
        #region ISysConfigDao Members

        public SystemConfig GetSystemConfigInfo()
        {
            return (ExecuteQueryForObject("GetSysConfigInfo", null) as SystemConfig);
        }

        #endregion
    }
}
