using System;
using System.Collections.Generic;

using Top4ever.Domain;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for IPrintTaskDao.
    /// </summary>
    public interface IPrintTaskDao
    {
        void InsertPrintTask(PrintTask printTask);
    }
}
