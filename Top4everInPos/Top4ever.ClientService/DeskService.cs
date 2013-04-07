using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.ClientService.Enum;
using Top4ever.Domain;
using Top4ever.Entity;
using Newtonsoft.Json;

namespace Top4ever.ClientService
{
    public class DeskService
    {
        public DeskService()
        { }

        public BizDesk GetBizDeskByName(string deskName)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.DESK_NAME;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_DESKBYNAME), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            //deskName
             byte[] tempByte = Encoding.UTF8.GetBytes(deskName);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.DESK_NAME;

            BizDesk desk = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                socket.Connect();
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
                    desk = JsonConvert.DeserializeObject<BizDesk>(strReceive);
                }
                socket.Disconnect();
            }
            return desk;
        }

        public bool UpdateDeskStatus(string deskName, string deviceNo, int status)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.DESK_NAME + ParamFieldLength.DEVICE_NO + BasicTypeLength.INT32;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_UPDATE_DESKSTATUS), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            byte[] tempByte = null;
            //deskName
            tempByte = Encoding.UTF8.GetBytes(deskName);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.DESK_NAME;
            //deviceNo
            tempByte = Encoding.UTF8.GetBytes(deviceNo);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.DEVICE_NO;
            //status
            Array.Copy(BitConverter.GetBytes(status), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            bool result = false;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                socket.Connect();
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    result = true;
                }
                socket.Disconnect();
            }
            return result;
        }

        public IList<DeskRealTimeInfo> GetDeskRealTimeInfo(string regionID)
        {
            int cByte = ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.REGION_ID;
            byte[] sendByte = new byte[cByte];
            int byteOffset = 0;
            Array.Copy(BitConverter.GetBytes((int)Command.ID_GET_DESKREALTIMEINFO), sendByte, BasicTypeLength.INT32);
            byteOffset = BasicTypeLength.INT32;
            Array.Copy(BitConverter.GetBytes(cByte), 0, sendByte, byteOffset, BasicTypeLength.INT32);
            byteOffset += BasicTypeLength.INT32;

            //regionID
            byte[] tempByte = Encoding.UTF8.GetBytes(regionID);
            Array.Copy(tempByte, 0, sendByte, byteOffset, tempByte.Length);
            byteOffset += ParamFieldLength.REGION_ID;

            IList<DeskRealTimeInfo> deskInfoList = null;
            using (SocketClient socket = new SocketClient(ConstantValuePool.BizSettingConfig.IPAddress, ConstantValuePool.BizSettingConfig.Port))
            {
                socket.Connect();
                Byte[] receiveData = null;
                Int32 operCode = socket.SendReceive(sendByte, out receiveData);
                if (operCode == (int)RET_VALUE.SUCCEEDED)
                {
                    string strReceive = Encoding.UTF8.GetString(receiveData, ParamFieldLength.PACKAGE_HEAD, receiveData.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
                    deskInfoList = JsonConvert.DeserializeObject<IList<DeskRealTimeInfo>>(strReceive);
                }
                socket.Disconnect();
            }
            return deskInfoList;
        }
    }
}