using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using IBatisNet.DataAccess;
using Newtonsoft.Json;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Interface.GoodsRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Utils;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for GoodsService.
    /// </summary>
    public class GoodsService
    {
        #region Private Fields

        private static readonly GoodsService _instance = new GoodsService();
        private readonly IDaoManager _daoManager;
        private readonly IGoodsDao _goodsDao;

        #endregion

        #region Constructor

        private GoodsService()
        {
            _daoManager = ServiceConfig.GetInstance().DaoManager;
            _goodsDao = _daoManager.GetDao(typeof(IGoodsDao)) as IGoodsDao;
        }

        #endregion

        #region Public methods

        public static GoodsService GetInstance()
        {
            return _instance;
        }

        public IList<GoodsCheckStock> GetGoodsCheckStock()
        {
            IList<GoodsCheckStock> goodsCheckStockList = null;
            try
            {
                _daoManager.OpenConnection();
                goodsCheckStockList = _goodsDao.GetGoodsCheckStock();
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetGoodsCheckStock]", exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            return goodsCheckStockList;
        }

        public string UpdateReducedGoodsQty(IList<GoodsCheckStock> goodsCheckStockList)
        {
            string goodsName = string.Empty;
            _daoManager.BeginTransaction();
            try
            {
                foreach (GoodsCheckStock item in goodsCheckStockList)
                {
                    int result = _goodsDao.UpdateReducedGoodsQty(item.GoodsID, item.ReducedQuantity);
                    if (result != 1)
                    {
                        goodsName = item.GoodsName;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(goodsName))
                {
                    _daoManager.CommitTransaction();
                }
                else
                {
                    _daoManager.RollBackTransaction();
                }
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error(string.Format("[UpdateReducedGoodsQty]参数：goodsCheckStockList_{0}", JsonConvert.SerializeObject(goodsCheckStockList)), exception);
                _daoManager.RollBackTransaction();
            }
            return goodsName;
        }

        public IList<GoodsImage> GetGoodsImageListInGroup(Guid goodsGroupId)
        {
            IList<Goods> goodsList = null;
            try
            {
                _daoManager.OpenConnection();
                goodsList = _goodsDao.GetGoodsListInGroup(goodsGroupId);
            }
            catch (Exception exception)
            {
                LogHelper.GetInstance().Error("[GetGoodsImageListInGroup]参数：goodsGroupId_" + goodsGroupId, exception);
            }
            finally
            {
                _daoManager.CloseConnection();
            }
            IList<GoodsImage> goodsImageList = null;
            //获取图片逻辑
            if (goodsList != null && goodsList.Count > 0) 
            {
                goodsImageList = new List<GoodsImage>();
                string imageDirPath = GetImageDirPath();
                foreach (Goods goods in goodsList)
                {
                    string saveImagePath = string.Format("{0}\\{1}.jpg", imageDirPath, goods.GoodsID);
                    string saveThumbnailPath = string.Format("{0}\\{1}_s.jpg", imageDirPath, goods.GoodsID);
                    GoodsImage goodsImage = new GoodsImage
                    {
                        GoodsID = goods.GoodsID, 
                        OriginalImage = ImageToByteArray(saveImagePath), 
                        GoodsThumb = ImageToByteArray(saveThumbnailPath)
                    };
                    goodsImageList.Add(goodsImage);
                }
            }
            return goodsImageList;
        }

        /// <summary>
        /// 获取品项图片
        /// </summary>
        public GoodsImage GetGoodsImage(Guid goodsId)
        {
            string imageDirPath = GetImageDirPath();
            string saveImagePath = string.Format("{0}\\{1}.jpg", imageDirPath, goodsId);
            string saveThumbnailPath = string.Format("{0}\\{1}_s.jpg", imageDirPath, goodsId);
            return new GoodsImage
            {
                GoodsID = goodsId,
                OriginalImage = ImageToByteArray(saveImagePath),
                GoodsThumb = ImageToByteArray(saveThumbnailPath)
            };
        }

        /// <summary>
        /// 图片转为Byte字节数组
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <returns>字节数组</returns>
        private byte[] ImageToByteArray(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Image imageIn = Image.FromFile(filePath))
                    {
                        using (Bitmap bmp = new Bitmap(imageIn))
                        {
                            bmp.Save(ms, imageIn.RawFormat);
                        }
                    }
                    return ms.ToArray();
                }
            }
            return null;
        }

        private string GetImageDirPath()
        {
            string localDriver = "C";
            if (Directory.Exists("D:\\"))
            {
                localDriver = "D";
            }
            return localDriver + ":\\VechImage";
        }

        #endregion
    }
}
