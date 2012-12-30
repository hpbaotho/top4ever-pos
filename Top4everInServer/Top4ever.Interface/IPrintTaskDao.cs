using System;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Domain.Transfer;

namespace Top4ever.Interface
{
    /// <summary>
    /// Summary description for IPrintTaskDao.
    /// </summary>
    public interface IPrintTaskDao
    {
        void InsertPrintTask(PrintTask printTask);

        void InsertDeskOperatePrint(DeskChange deskChange);
    }
}
