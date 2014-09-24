using System;
using System.Collections.Generic;
using System.Linq;

namespace Top4ever.Domain.Transfer
{
    public class GoodsImage
    {
        /// <summary>
        /// 菜品ID
        /// </summary>
        public Guid GoodsID { get; set; }

        /// <summary>
        /// 菜品图片
        /// </summary>
        public byte[] OriginalImage { get; set; }

        /// <summary>
        /// 菜品缩略图
        /// </summary>
        public byte[] GoodsThumb { get; set; }
    }
}
