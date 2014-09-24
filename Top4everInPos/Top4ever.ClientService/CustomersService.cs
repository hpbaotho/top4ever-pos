using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.ClientService.Enum;
using Top4ever.Domain.Customers;
using Top4ever.Entity;
using Newtonsoft.Json;

namespace Top4ever.ClientService
{
    public class CustomersService
    {
        private static readonly CustomersService Instance = new CustomersService();

        private CustomersService()
        { }

        public static CustomersService GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// 创建客户信息 0:数据库操作失败, 1:成功, 2:手机号已存在
        /// </summary>
        public int CreateCustomerInfo(CustomerInfo customerInfo)
        {
            string json = JsonConvert.SerializeObject(customerInfo);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int cByte = ParamFieldLength.PACKAGE_HEAD + jsonByte.Length;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_CREATE_CUSTOMERINFO), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            Array.Copy(jsonByte, 0, sendByte, byteOffset, jsonByte.Length);
            byteOffset += jsonByte.Length;

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

        public bool UpdateCustomerInfo(CustomerInfo customerInfo)
        {
            string json = JsonConvert.SerializeObject(customerInfo);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int cByte = ParamFieldLength.PACKAGE_HEAD + jsonByte.Length;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_UPDATE_CUSTOMERINFO), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            Array.Copy(jsonByte, 0, sendByte, byteOffset, jsonByte.Length);
            byteOffset += jsonByte.Length;

            bool result = false;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = true;
                }
                socket.Close();
            }
            return result;
        }

        public CustomerInfo GetCustomerInfoByPhone(string telephone)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.TELEPHONE;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_CUSTOMERINFOBYPHONE), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            //telephone
            byte[] tempByte = Encoding.UTF8.GetBytes(telephone);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.TELEPHONE;

            Int32 operCode = 0;
            CustomerInfo customerInfo = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
                    customerInfo = JsonConvert.DeserializeObject<CustomerInfo>(strReceive);
                }
                socket.Close();
            }
            return customerInfo;
        }

        public IList<CustomerInfo> GetAllCustomerInfo()
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_ALLCUSTOMERINFO), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            Int32 operCode = 0;
            IList<CustomerInfo> customerInfoList = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
                    customerInfoList = JsonConvert.DeserializeObject<IList<CustomerInfo>>(strReceive);
                }
                socket.Close();
            }
            return customerInfoList;
        }

        public CustomerOrder GetCustomerOrder(Guid orderID)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.ORDER_ID;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_CUSTOMERORDER), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            //orderID
            byte[] tempByte = Encoding.UTF8.GetBytes(orderID.ToString());
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.ORDER_ID;

            Int32 operCode = 0;
            CustomerOrder customerOrder = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
                    customerOrder = JsonConvert.DeserializeObject<CustomerOrder>(strReceive);
                }
                socket.Close();
            }
            return customerOrder;
        }

        public bool UpdateTakeoutOrderStatus(CustomerOrder customerOrder)
        {
            string json = JsonConvert.SerializeObject(customerOrder);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int cByte = ParamFieldLength.PACKAGE_HEAD + jsonByte.Length;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_UPDATE_TAKEOUTORDERSTATUS), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            Array.Copy(jsonByte, 0, sendByte, byteOffset, jsonByte.Length);
            byteOffset += jsonByte.Length;

            bool result = false;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = true;
                }
                socket.Close();
            }
            return result;
        }

        public bool CreateOrUpdateCustomerOrder(CustomerOrder customerOrder)
        {
            string json = JsonConvert.SerializeObject(customerOrder);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int cByte = ParamFieldLength.PACKAGE_HEAD + jsonByte.Length;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_CREATE_CUSTOMERORDER), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            Array.Copy(jsonByte, 0, sendByte, byteOffset, jsonByte.Length);
            byteOffset += jsonByte.Length;

            bool result = false;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = true;
                }
                socket.Close();
            }
            return result;
        }

        /// <summary>
        /// 创建或者更新电话记录
        /// </summary>
        /// <param name="callRecord">电话记录</param>
        /// <returns></returns>
        public bool CreateOrUpdateCallRecord(CallRecord callRecord)
        {
            string json = JsonConvert.SerializeObject(callRecord);
            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            int cByte = ParamFieldLength.PACKAGE_HEAD + jsonByte.Length;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_CREATE_CALLRECORD), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;
            Array.Copy(jsonByte, 0, sendByte, byteOffset, jsonByte.Length);
            byteOffset += jsonByte.Length;

            bool result = false;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = true;
                }
                socket.Close();
            }
            return result;
        }

        /// <summary>
        /// 获取最近一个月特定状态通话记录
        /// </summary>
        /// <returns></returns>
        public IList<CallRecord> GetCallRecordByStatus(int status)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_CALLRECORD), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            //status
            Array.Copy(BitConverter.GetBytes(status), 0, sendByte, byteOffset, BasicTypeLength.INT32);

            IList<CallRecord> callRecordList = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    callRecordList = JsonConvert.DeserializeObject<IList<CallRecord>>(strReceive);
                }
                socket.Close();
            }
            return callRecordList;
        }

        /// <summary>
        /// 获取热销产品列表
        /// </summary>
        /// <param name="telephone">电话号码</param>
        /// <returns></returns>
        public IList<TopSellGoods> GetTopSellGoods(string telephone)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.TELEPHONE;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_TOPSELLGOODS), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            //telephone
            byte[] tempByte = Encoding.UTF8.GetBytes(telephone);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.TELEPHONE;

            IList<TopSellGoods> topSellGoodsList = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD);
                    topSellGoodsList = JsonConvert.DeserializeObject<IList<TopSellGoods>>(strReceive);
                }
                socket.Close();
            }
            return topSellGoodsList;
        }
    }
}
