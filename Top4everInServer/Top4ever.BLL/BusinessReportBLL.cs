using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.BLL.Enum;
using Top4ever.Domain.Transfer;
using Top4ever.Service;
using Newtonsoft.Json;

namespace Top4ever.BLL
{
    public class BusinessReportBLL
    {
        public static byte[] GetReportDataByHandover(byte[] itemBuffer)
        {
            byte[] objRet = null;
             string deviceNo = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.DEVICE_NO).Trim('\0');

             BusinessReport bizReport = BusinessReportService.GetInstance().GetReportDataByHandover(deviceNo);
             if (bizReport == null)
            {
                //数据获取失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                string json = JsonConvert.SerializeObject(bizReport);
                byte[] jsonByte = Encoding.UTF8.GetBytes(json);

                int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
                objRet = new byte[transCount];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                Array.Copy(jsonByte, 0, objRet, 2 * BasicTypeLength.INT32, jsonByte.Length);
            }
            return objRet;
        }

        public static byte[] GetReportDataByDailyStatement(byte[] itemBuffer)
        {
            byte[] objRet = null;

            BusinessReport bizReport = BusinessReportService.GetInstance().GetReportDataByDailyStatement();
            if (bizReport == null)
            {
                //数据获取失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                string json = JsonConvert.SerializeObject(bizReport);
                byte[] jsonByte = Encoding.UTF8.GetBytes(json);

                int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
                objRet = new byte[transCount];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                Array.Copy(jsonByte, 0, objRet, 2 * BasicTypeLength.INT32, jsonByte.Length);
            }
            return objRet;
        }

        public static byte[] GetItemPriceListByGroup(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string beginDate = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.BEGINDATE).Trim('\0');
            string endDate = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD + ParamFieldLength.BEGINDATE, ParamFieldLength.ENDDATE).Trim('\0');

            IList<GroupPrice> groupPriceList = BusinessReportService.GetInstance().GetItemPriceListByGroup(DateTime.Parse(beginDate), DateTime.Parse(endDate));
            if (groupPriceList == null)
            {
                //数据获取失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            else
            {
                string json = JsonConvert.SerializeObject(groupPriceList);
                byte[] jsonByte = Encoding.UTF8.GetBytes(json);

                int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
                objRet = new byte[transCount];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                Array.Copy(jsonByte, 0, objRet, 2 * BasicTypeLength.INT32, jsonByte.Length);
            }
            return objRet;
        }
    }
}
