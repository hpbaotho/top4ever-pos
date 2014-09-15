using System;

namespace Top4ever.Domain.Transfer
{
    public class GoodsGroupInfo
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
    }
}
