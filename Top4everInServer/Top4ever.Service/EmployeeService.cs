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

        /// <summary>
        /// 获取用户信息 0:数据库操作失败, 1:成功, 2:账号或者密码错误
        /// </summary>
        public int GetEmployee(string login, string password, out Employee employee)
        {
            int result = 0;

            _daoManager.OpenConnection();
            if (_employeeDao.GetEmployee(login, password, out employee))
            {
                if (employee == null)
                {
                    result = 2;
                }
                else
                {
                    employee.RightsCodeList = _employeeDao.GetRightsCodeList(employee.EmployeeID);
                    result = 1;
                }
            }
            _daoManager.CloseConnection();

            return result;
        }

        #endregion
    }
}
