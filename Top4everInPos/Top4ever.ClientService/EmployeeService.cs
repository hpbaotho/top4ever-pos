﻿using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.ClientService.Enum;
using Top4ever.Domain.Accounts;
using Top4ever.Entity;
using Newtonsoft.Json;

namespace Top4ever.ClientService
{
    public class EmployeeService
    {
        private static readonly EmployeeService Instance = new EmployeeService();

        private EmployeeService()
        { }

        public static EmployeeService GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// 获取用户信息 0:数据库操作失败, 1:成功, 2:账号或者密码错误
        /// </summary>
        public int EmployeeLogin(string userName, string userPassword, ref Employee employee)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.USER_NO + ParamFieldLength.USER_PWD;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_USERLOGIN), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            byte[] tempByte = null;
            //UserName
            tempByte = Encoding.UTF8.GetBytes(userName);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.USER_NO;
            //UserPassword
            tempByte = Encoding.UTF8.GetBytes(userPassword);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.USER_PWD;

            int result = 0;
            employee = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    employee = JsonConvert.DeserializeObject<Employee>(strReceive);
                    result = 1;
                }
                else if (operCode == (int)RET_VALUE.ERROR_AUTHENTICATION)
                {
                    result = 2;
                }
                else
                {
                    result = 0;
                }
                socket.Close();
            }
            return result;
        }

        /// <summary>
        /// 获取用户信息 0:数据库操作失败, 1:成功, 2:账号或者密码错误
        /// </summary>
        public int EmployeeLogin(string attendanceCard, ref Employee employee)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.ATTENDANCE_CARD;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_SWIPINGCARDTOLOGIN), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            byte[] tempByte = null;
            //AttendanceCard
            tempByte = Encoding.UTF8.GetBytes(attendanceCard);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.ATTENDANCE_CARD;

            int result = 0;
            employee = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    employee = JsonConvert.DeserializeObject<Employee>(strReceive);
                    result = 1;
                }
                else if (operCode == (int)RET_VALUE.ERROR_AUTHENTICATION)
                {
                    result = 2;
                }
                else
                {
                    result = 0;
                }
                socket.Close();
            }
            return result;
        }

        /// <summary>
        /// 获取用户信息 0:数据库操作失败, 1:成功, 2:用户名不存在
        /// </summary>
        public int GetEmployeeByNo(string employeeNo, ref Employee employee)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.USER_NO;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_EMPLOYEEBYNO), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            byte[] tempByte = null;
            //employeeNo
            tempByte = Encoding.UTF8.GetBytes(employeeNo);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.USER_NO;

            int result = 0;
            employee = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    employee = JsonConvert.DeserializeObject<Employee>(strReceive);
                    result = 1;
                }
                else if (operCode == (int)RET_VALUE.ERROR_AUTHENTICATION)
                {
                    result = 2;
                }
                else
                {
                    result = 0;
                }
                socket.Close();
            }
            return result;
        }

        public IList<String> GetRightsCodeList(string userName, string userPassword)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.USER_NO + ParamFieldLength.USER_PWD;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_RIGHTSCODELIST), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            byte[] tempByte = null;
            //UserName
            tempByte = Encoding.UTF8.GetBytes(userName);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.USER_NO;
            //UserPassword
            tempByte = Encoding.UTF8.GetBytes(userPassword);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.USER_PWD;

            IList<String> rightsCodeList = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    rightsCodeList = JsonConvert.DeserializeObject<IList<String>>(strReceive);
                }
                socket.Close();
            }
            return rightsCodeList;
        }

        public int UpdateEmployeePassword(string employeeNo, string currentPassword, string newPassword)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.EMPLOYEE_NO + ParamFieldLength.EMPLOYEE_PASSWORD + ParamFieldLength.EMPLOYEE_PASSWORD;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_UPDATE_EMPLOYEEPASSWORD), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            byte[] tempByte = null;
            //employeeNo
            tempByte = Encoding.UTF8.GetBytes(employeeNo);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.EMPLOYEE_NO;
            //currentPassword
            tempByte = Encoding.UTF8.GetBytes(currentPassword);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.EMPLOYEE_PASSWORD;
            //newPassword
            tempByte = Encoding.UTF8.GetBytes(newPassword);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.EMPLOYEE_PASSWORD;

            int result = 0;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = BitConverter.ToInt32(receiveData, ParamFieldLength.PACKAGE_HEAD);
                }
                socket.Close();
            }
            return result;
        }
    }
}
