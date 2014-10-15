using System;
using System.Collections.Generic;

using IBatisNet.DataAccess;

using Top4ever.Domain.Accounts;
using Top4ever.Interface.Accounts;
using Top4ever.Utils;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for EmployeeService.
    /// </summary>
    public class EmployeeService
    {
        #region Private Fields

        private static readonly EmployeeService _instance = new EmployeeService();
        private readonly IDaoManager _daoManager;
        private readonly IEmployeeDao _employeeDao;

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
            }
            catch (Exception exception)
            {
                employee = null;
                LogHelper.GetInstance().Error("[GetEmployee]参数：UserName_" + userName, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
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
            }
            catch (Exception exception)
            {
                employee = null;
                LogHelper.GetInstance().Error("[GetEmployee]参数：AttendanceCard_" + attendanceCard, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
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
                result = employee == null ? 2 : 1;
            }
            catch (Exception exception)
            {
                employee = null;
                LogHelper.GetInstance().Error("[GetEmployeeByNo]参数：EmployeeNo_" + employeeNo, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
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
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetRightsCodeList]参数：UserName_" + userName, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return rightsCodeList;
        }

        public Int32 UpdateEmployeePassword(string employeeNo, string password, string newPassword)
        {
            int result = 0;
            try
            {
                _daoManager.OpenConnection();
                result = _employeeDao.UpdateEmployeePassword(employeeNo, password, newPassword);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[UpdateEmployeePassword]参数：EmployeeNo_" + employeeNo, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return result;
        }

        #endregion
    }
}
