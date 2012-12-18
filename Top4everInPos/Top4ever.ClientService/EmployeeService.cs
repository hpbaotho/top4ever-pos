using System;
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
        public EmployeeService()
        { }

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

            Int32 operCode = 0;
            employee = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                socket.Connect();
                Byte[] receiveData = null;
                operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    employee = JsonConvert.DeserializeObject<Employee>(strReceive);
                }
                socket.Disconnect();
            }
            return operCode;
        }
    }
}
