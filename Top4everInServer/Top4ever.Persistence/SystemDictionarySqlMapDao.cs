using System;
using System.Collections;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Interface;

namespace Top4ever.Persistence
{
    /// <summary>
    /// Summary description for SystemDictionarySqlMapDao
    /// </summary>
    public class SystemDictionarySqlMapDao : BaseSqlMapDao, ISystemDictionaryDao
    {
        #region ISystemDictionary Members

        public int GetCurrentTranSequence()
        {
            int i = -1;
            int curTranSeq = 0;
            Hashtable htParam = new Hashtable();
            htParam["ReturnValue"] = i;
            object result = ExecuteQueryForObject("SelectCurrentTranSequence", htParam);
            i = (int)htParam["ReturnValue"];    //返回值
            if (i == 1)
            {
                curTranSeq = Convert.ToInt32(result);
            }
            return curTranSeq;
        }

        #endregion
    }
}
