using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for ButtonStyleSqlMapDao
    /// </summary>
    public class ButtonStyleSqlMapDao : BaseSqlMapDao, IButtonStyleDao
    {
        #region IButtonStyleDao Members

        public IList<ButtonStyle> GetButtonStyleList()
        {
            return ExecuteQueryForList<ButtonStyle>("GetAllButtonStyle", null);
        }

        #endregion
    }
}
