using System;
using System.Collections.Generic;

using IBatisNet.Common.Logging;

using Top4ever.Domain.Accounts;
using Top4ever.Interface.Accounts;

namespace Top4ever.Persistence.Accounts
{
    /// <summary>
    /// Summary description for EmployeeSqlMapDao
    /// </summary>
    public class EmployeeSqlMapDao : BaseSqlMapDao, IEmployeeDao
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(EmployeeSqlMapDao));

        #region IEmployeeDao Members

        public bool GetEmployee(string login, string password, out Employee employee)
        {
            bool result = false;
            Employee emp = new Employee();
            emp.EmployeeNo = login;
            emp.Password = password;
            try
            {
                employee = ExecuteQueryForObject("GetEmployeeByLoginAndPassword", emp) as Employee;
                result = true;
            }
            catch(Exception ex)
            {
                result = false;
                employee = null;
                logger.Error("Database operation failed !", ex);
            }
            return result;
        }

        public IList<String> GetRightsCodeList(Guid employeeID)
        {
            return ExecuteQueryForList<String>("GetRightsCodeListByEmployee", employeeID);
        }

        #endregion
    }
}
