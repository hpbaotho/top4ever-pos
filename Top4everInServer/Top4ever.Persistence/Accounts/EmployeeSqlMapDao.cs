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
            Employee employee = new Employee();
            employee.EmployeeNo = login;
            employee.Password = password;

            Employee returnVal = null;
            try
            {
                returnVal = ExecuteQueryForObject("GetEmployeeByLoginAndPassword", employee) as Employee;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }

        public IList<String> GetRightsCodeList(Guid employeeID)
        {
            return ExecuteQueryForList<String>("GetRightsCodeListByEmployee", employeeID);
        }

        #endregion
    }
}
