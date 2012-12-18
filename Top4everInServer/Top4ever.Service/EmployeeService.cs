using System;
using System.Collections;

using IBatisNet.DataAccess;

using Top4ever.Domain.Accounts;
using Top4ever.Interface.Accounts;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for EmployeeService.
    /// </summary>
    public class EmployeeService
    {
        #region Private Fields

        private static EmployeeService _instance = new EmployeeService();
        private IDaoManager _daoManager = null;
        private IEmployeeDao _employeeDao = null;

        #endregion

        #region Constructor

        private EmployeeService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _employeeDao = _daoManager.GetDao(typeof(IEmployeeDao)) as IEmployeeDao;
        }

        #endregion

        #region Public methods

        public static EmployeeService GetInstance()
        {
            return _instance;
        }

        public Employee GetEmployee(string login, string password)
        {
            Employee employee = null;

            _daoManager.OpenConnection();
            employee = _employeeDao.GetEmployee(login, password);
            if (employee != null)
            {
                employee.RightsCodeList = _employeeDao.GetRightsCodeList(employee.EmployeeID);
            }
            _daoManager.CloseConnection();

            return employee;
        }

        #endregion
    }
}
