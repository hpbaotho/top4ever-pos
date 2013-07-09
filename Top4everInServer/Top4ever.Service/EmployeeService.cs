using System;
using System.Collections;
using System.Collections.Generic;

using IBatisNet.Common.Logging;
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
        private static readonly ILog logger = LogManager.GetLogger(typeof(EmployeeService));

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
            try
            {
                _daoManager.OpenConnection();
                employee = _employeeDao.GetEmployee(userName, password);
                if (employee == null)
                {
                    result = 2;
                }
                else
                {
                    employee.RightsCodeList = _employeeDao.GetRightsCodeList(userName, password);
                    result = 1;
                }
                _daoManager.CloseConnection();
            }
            catch (Exception ex)
            {
                employee = null;
                result = 0;
                logger.Error("Database operation failed !", ex);
            }
            return result;
        }

        /// <summary>
        /// 获取用户信息 0:数据库操作失败, 1:成功, 2:账号或者密码错误
        /// </summary>
        public int GetEmployee(string attendanceCard, out Employee employee)
        {
            int result = 0;
            try
            {
                _daoManager.OpenConnection();
                employee = _employeeDao.GetEmployee(attendanceCard);
                if (employee == null)
                {
                    result = 2;
                }
                else
                {
                    employee.RightsCodeList = _employeeDao.GetRightsCodeList(employee.EmployeeNo, employee.Password);
                    result = 1;
                }
                _daoManager.CloseConnection();
            }
            catch (Exception ex)
            {
                employee = null;
                result = 0;
                logger.Error("Database operation failed !", ex);
            }
            return result;
        }

        /// <summary>
        /// 获取用户信息 0:数据库操作失败, 1:成功, 2:账号不存在
        /// </summary>
        public int GetEmployeeByNo(string employeeNo, out Employee employee)
        {
            int result = 0;
            try
            {
                _daoManager.OpenConnection();
                employee = _employeeDao.GetEmployeeByNo(employeeNo);
                if (employee == null)
                {
                    result = 2;
                }
                else
                {
                    result = 1;
                }
                _daoManager.CloseConnection();
            }
            catch (Exception ex)
            {
                employee = null;
                result = 0;
                logger.Error("Through employee No. to get employee operation failed !", ex);
            }
            return result;
        }

        public IList<String> GetRightsCodeList(string userName, string password)
        {
            IList<String> rightsCodeList = null;
            try
            {
                _daoManager.OpenConnection();
                rightsCodeList = _employeeDao.GetRightsCodeList(userName, password);
                _daoManager.CloseConnection();
            }
            catch (Exception ex)
            {
                rightsCodeList = null;
                logger.Error("Database operation failed !", ex);
            }
            return rightsCodeList;
        }

        public Int32 UpdateEmployeePassword(string employeeNo, string password, string newPassword)
        {
            int result = 0;
            _daoManager.OpenConnection();
            result = _employeeDao.UpdateEmployeePassword(employeeNo, password, newPassword);
            _daoManager.CloseConnection();
            return result;
        }

        #endregion
    }
}
