using System;
using System.Collections.Generic;

namespace Top4ever.Domain.GoodsRelated
{
    [Serializable]
    public class GoodsGroup
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid GoodsGroupID { get; set; }

        /// <summary>
        /// 菜品组编号
        /// </summary>
        public string GoodsGroupNo { get; set; }

        /// <summary>
        /// 菜品组名称
        /// </summary>
        public string GoodsGroupName { get; set; }

        /// <summary>
        /// 菜品组名称第二语言
        /// </summary>
        public string GoodsGroupName2nd { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public Guid ButtonStyleID { get; set; }

        /// <summary>
        /// 菜品组下面所有的菜品
        /// </summary>
        public IList<Goods> GoodsList { get; set; }
    }
}
