using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using IBatisNet.DataAccess;

using Top4ever.Domain.GoodsRelated;
using Top4ever.Interface.GoodsRelated;
using Top4ever.Domain.Transfer;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for GoodsService.
    /// </summary>
    public class GoodsService
    {
        #region Private Fields

        private static GoodsService _instance = new GoodsService();
        private IDaoManager _daoManager = null;
        private IGoodsDao _goodsDao = null;

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

            _daoManager.OpenConnection();
            goodsCheckStockList = _goodsDao.GetGoodsCheckStock();
            _daoManager.CloseConnection();

            return goodsCheckStockList;
        }

        public string UpdateReducedGoodsQty(IList<GoodsCheckStock> goodsCheckStockList)
        {
            string goodsName = string.Empty;
            _daoManager.BeginTransaction();
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
            return goodsName;
        }

        public IList<GoodsImage> GetGoodsImageListInGroup(Guid goodsGroupId)
        {
            IList<GoodsImage> goodsImageList = null;

            IList<Goods> goodsList = null;
            _daoManager.OpenConnection();
            goodsList = _goodsDao.GetGoodsListInGroup(goodsGroupId);
            _daoManager.CloseConnection();

            //获取图片逻辑
            if (goodsList != null && goodsList.Count > 0) 
            {
                goodsImageList = new List<GoodsImage>();
                string imageDirPath = GetImageDirPath();
                foreach (Goods goods in goodsList)
                {
                    string saveImagePath = string.Format("{0}\\{1}.jpg", imageDirPath, goods.GoodsID);
                    string saveThumbnailPath = string.Format("{0}\\{1}_s.jpg", imageDirPath, goods.GoodsID);
                    GoodsImage goodsImage = new GoodsImage();
                    goodsImage.GoodsID = goods.GoodsID;
                    goodsImage.OriginalImage = ImageToByteArray(saveImagePath);
                    goodsImage.GoodsThumb = ImageToByteArray(saveThumbnailPath);
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
