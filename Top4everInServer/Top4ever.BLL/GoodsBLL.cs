using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.BLL.Enum;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Domain.Transfer;
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

        public static byte[] GetGoodsImage(byte[] itemBuffer)
        {
            byte[] objRet = null;
            string goodsId = Encoding.UTF8.GetString(itemBuffer, ParamFieldLength.PACKAGE_HEAD, ParamFieldLength.GOODSORGROUP_ID).Trim('\0');
            var goodsImage = GoodsService.GetInstance().GetGoodsImage(new Guid(goodsId));
            if (goodsImage.OriginalImage != null || goodsImage.GoodsThumb != null)
            {
                //构造返回值
                int transCount = ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32 * 4;
                if (goodsImage.OriginalImage != null)
                {
                    transCount += goodsImage.OriginalImage.Length;
                }
                if (goodsImage.GoodsThumb != null)
                {
                    transCount += goodsImage.GoodsThumb.Length;
                }
                objRet = new byte[transCount];
                int byteOffset = 0;
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                byteOffset += BasicTypeLength.INT32;
                Array.Copy(BitConverter.GetBytes(transCount), 0, objRet, byteOffset, BasicTypeLength.INT32);
                byteOffset += BasicTypeLength.INT32;

                int imageOffset = ParamFieldLength.PACKAGE_HEAD + BasicTypeLength.INT32 * 4;
                //ImageOffset
                Array.Copy(BitConverter.GetBytes(imageOffset), 0, objRet, byteOffset, BasicTypeLength.INT32);
                byteOffset += BasicTypeLength.INT32;
                if (goodsImage.OriginalImage != null)
                {
                    //ImageLength
                    Array.Copy(BitConverter.GetBytes(goodsImage.OriginalImage.Length), 0, objRet, byteOffset, BasicTypeLength.INT32);
                    byteOffset += BasicTypeLength.INT32;
                    //OriginalImage
                    Array.Copy(goodsImage.OriginalImage, 0, objRet, imageOffset, goodsImage.OriginalImage.Length);
                    imageOffset += goodsImage.OriginalImage.Length;
                }
                else
                {
                    //ImageLength
                    Array.Copy(BitConverter.GetBytes(0), 0, objRet, byteOffset, BasicTypeLength.INT32);
                    byteOffset += BasicTypeLength.INT32;
                }
                //ThumbOffset
                Array.Copy(BitConverter.GetBytes(imageOffset), 0, objRet, byteOffset, BasicTypeLength.INT32);
                byteOffset += BasicTypeLength.INT32;
                if (goodsImage.GoodsThumb != null)
                {
                    //ThumbLength
                    Array.Copy(BitConverter.GetBytes(goodsImage.GoodsThumb.Length), 0, objRet, byteOffset, BasicTypeLength.INT32);
                    byteOffset += BasicTypeLength.INT32;
                    //GoodsThumb
                    Array.Copy(goodsImage.GoodsThumb, 0, objRet, imageOffset, goodsImage.GoodsThumb.Length);
                    imageOffset += goodsImage.GoodsThumb.Length;
                }
                else
                {
                    //ThumbLength
                    Array.Copy(BitConverter.GetBytes(0), 0, objRet, byteOffset, BasicTypeLength.INT32);
                    byteOffset += BasicTypeLength.INT32;
                }
                return objRet;
            }
            else
            {
                objRet = new byte[ParamFieldLength.PACKAGE_HEAD];
                Array.Copy(BitConverter.GetBytes((int)RET_VALUE.SUCCEEDED), 0, objRet, 0, BasicTypeLength.INT32);
                Array.Copy(BitConverter.GetBytes(0), 0, objRet, BasicTypeLength.INT32, BasicTypeLength.INT32);
                return objRet;
            }
        }
    }
}
