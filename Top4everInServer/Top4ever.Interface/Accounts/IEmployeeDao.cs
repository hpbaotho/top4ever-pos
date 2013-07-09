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

        Employee GetEmployee(string attendanceCard);

        Employee GetEmployeeByNo(string employeeNo);

        IList<String> GetRightsCodeList(string userName, string password);

        Int32 UpdateEmployeePassword(string employeeNo, string password, string newPassword);
    }
}
