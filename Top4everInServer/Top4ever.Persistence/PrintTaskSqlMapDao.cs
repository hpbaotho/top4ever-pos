using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for PrintTaskSqlMapDao
    /// </summary>
    public class PrintTaskSqlMapDao : BaseSqlMapDao, IPrintTaskDao
    {
        #region IPrintTaskDao Members

        public void InsertPrintTask(PrintTask printTask)
        {
            ExecuteInsert("InsertPrintTask", printTask);
        }

        public void InsertDeskOperatePrint(DeskChange deskChange)
        {
            Hashtable htParam = new Hashtable();
            htParam["SrcOrderID"] = deskChange.OrderID1st;
            htParam["DeskName"] = deskChange.DeskName;

            ExecuteInsert("InsertDeskPrint", htParam);
        }
        #endregion
    }
}
