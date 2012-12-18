using System;
using System.Collections.Generic;

using Top4ever.Domain;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for IButtonStyleDao.
    /// </summary>
    public interface IButtonStyleDao
    {
        IList<ButtonStyle> GetButtonStyleList();
    }
}
