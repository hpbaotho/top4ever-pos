using System;
using System.Collections.Generic;

using Top4ever.Domain.Accounts;
using Top4ever.Interface.Accounts;

namespace Top4ever.Persistence.Accounts
{
    /// <summary>
    /// Summary description for EmployeeSqlMapDao
    /// </summary>
    public class EmployeeSqlMapDao : BaseSqlMapDao, IEmployeeDao
    {
        #region IEmployeeDao Members

        public Employee GetEmployee(string login, string password)
        {
            Employee emp = new Employee();
            emp.EmployeeNo = login;
            emp.Password = password;
            return ExecuteQueryForObject("GetEmployeeByLoginAndPassword", emp) as Employee;
        }

        public Employee GetEmployee(string attendanceCard)
        {
            return ExecuteQueryForObject("GetEmployeeBySwipeCard", attendanceCard) as Employee;
        }

        public Employee GetEmployeeByNo(string employeeNo)
        {
            return ExecuteQueryForObject("GetEmployeeByNo", employeeNo) as Employee;
        }

        public IList<String> GetRightsCodeList(string userName, string password)
        {
            Employee emp = new Employee();
            emp.EmployeeNo = userName;
            emp.Password = password;
            return ExecuteQueryForList<String>("GetRightsCodeListByEmployee", emp); ;
        }

        #endregion
    }
}
