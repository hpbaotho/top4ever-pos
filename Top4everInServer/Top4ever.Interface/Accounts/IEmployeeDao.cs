using System;
using System.Collections.Generic;

using Top4ever.Domain.Accounts;

namespace Top4ever.Interface.Accounts
{
    /// <summary>
    /// Summary description for IEmployeeDao.
    /// </summary>
    public interface IEmployeeDao
    {
        Employee GetEmployee(string login, string password);

        IList<String> GetRightsCodeList(Guid employeeID);
    }
}
