using System;
using System.Collections.Generic;

namespace Top4ever.Domain.GoodsRelated
{
    public class GoodsSetMeal
    {
        /// <summary>
        /// 套餐项的父级
        /// </summary>
        public Guid ParentGoodsID { get; set; }

        /// <summary>
        /// 套餐项的数量
        /// </summary>
        public decimal ItemQty { get; set; }

        /// <summary>
        /// 套餐项的折扣率
        /// </summary>
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// 套餐项的折扣额
        /// </summary>
        public decimal OffFixPay { get; set; }

        /// <summary>
        /// 套餐项的ID
        /// </summary>
        public Guid ItemID { get; set; }

        /// <summary>
        /// 套餐项编号
        /// </summary>
        public string ItemNo { get; set; }

        /// <summary>
        /// 套餐项名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 套餐项名称第二语言
        /// </summary>
        public string ItemName2nd { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 单位售价
        /// </summary>
        public decimal SellPrice { get; set; }

        /// <summary>
        /// 能否打折
        /// </summary>
        public bool CanDiscount { get; set; }

        /// <summary>
        /// 是否自动显示细项
        /// </summary>
        public bool AutoShowDetails { get; set; }

        /// <summary>
        /// 厨房打印机
        /// </summary>
        public string PrintSolutionName { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public Guid DepartID { get; set; }
    }
}
