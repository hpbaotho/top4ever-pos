using System;

namespace Top4ever.Domain.GoodsRelated
{
    public class GoodsCheckStock
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid GoodsID { get; set; }

        /// <summary>
        /// 品项名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 品项名称第二语言
        /// </summary>
        public string GoodsName2nd { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQuantity { get; set; }

        /// <summary>
        /// 减少的数量
        /// </summary>
        public decimal ReducedQuantity { get; set; }
    }
}
