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
        bool GetEmployee(string login, string password, out Employee employee);

        bool GetEmployee(string attendanceCard, out Employee employee);

        IList<String> GetRightsCodeList(string userName, string password);
    }
}
