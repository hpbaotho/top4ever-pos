using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top4ever.Domain.GoodsRelated;

namespace Top4ever.Domain.Transfer
{
    public class AndroidBasicData
    {
        /// <summary>
        /// 店铺信息
        /// </summary>
        public Shop CurrentShop { get; set; }
        /// <summary>
        /// 桌号列表
        /// </summary>
        public IList<string> DeskNameList { get; set; }
        /// <summary>
        /// 菜品组信息
        /// </summary>
        public IList<GoodsGroup> GoodsGroupList { get; set; }
        /// <summary>
        /// 菜品信息
        /// </summary>
        public IList<DetailsGroup> DetailsGroupList { get; set; }
    }
}
