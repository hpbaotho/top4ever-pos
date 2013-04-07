﻿using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.BLL.Enum;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Service;
using Newtonsoft.Json;

namespace Top4ever.BLL
{
    public class GoodsBLL
    {
        public static byte[] GetGoodsCheckStock()
        {
            byte[] objRet = null;
            IList<GoodsCheckStock> goodsCheckStockList = GoodsService.GetInstance().GetGoodsCheckStock();
            if (goodsCheckStockList != null && goodsCheckStockList.Count > 0)
            {
                //获取成功
                string json = JsonConvert.SerializeObject(goodsCheckStockList);
                byte[] jsonByte = Encoding.UTF8.GetBytes(json);

                int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + jsonByte.Length;
                objRet = new byte[transCount];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                Array.Copy(jsonByte, 0, objRet, 2 * BasicTypeLength.INT32, jsonByte.Length);
            }
            else
            {
                //数据库操作失败
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.ERROR_DB), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(ParamFieldLength.PACKAGE_HEAD), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            }
            return objRet;
        }

        public static byte[] UpdateReducedGoodsQty(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string strReceive = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, itemBuffer.Length - ParamFieldLength.PACKAGE_HEAD).Trim('\0');
            IList<GoodsCheckStock> goodsCheckStockList = JsonConvert.DeserializeObject<IList<GoodsCheckStock>>(strReceive);

            //返回数量不足的品项名称
            string goodsName = GoodsService.GetInstance().UpdateReducedGoodsQty(goodsCheckStockList);
            byte[] returnByte = Encoding.UTF8.GetBytes(goodsName);

            int transCount = BasicTypeLength.INT32 + BasicTypeLength.INT32 + ParamFieldLength.GOODS_NAME;
            objRet = new byte[transCount];
            Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
            Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
            Array.Copy(returnByte, 0, objRet, 2 * BasicTypeLength.INT32, returnByte.Length);
            return objRet;
        }
    }
}
