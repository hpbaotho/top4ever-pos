using System;
using System.Collections;
using System.Collections.Generic;

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
        public int GetEmployee(string userName, string password, out Employee employee)
        {
            int result = 0;

            _daoManager.OpenConnection();
            if (_employeeDao.GetEmployee(userName, password, out employee))
            {
                if (employee == null)
                {
                    result = 2;
                }
                else
                {
                    employee.RightsCodeList = _employeeDao.GetRightsCodeList(userName, password);
                    result = 1;
                }
            }
            _daoManager.CloseConnection();

            return result;
        }

        public IList<String> GetRightsCodeList(string userName, string password)
        {
            _daoManager.OpenConnection();
            IList<String> rightsCodeList = _employeeDao.GetRightsCodeList(userName, password);
            _daoManager.CloseConnection();

            return rightsCodeList;
        }

        #endregion
    }
}
