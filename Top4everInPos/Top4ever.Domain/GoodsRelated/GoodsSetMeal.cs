using System;
using System.Collections.Generic;

namespace Top4ever.Domain.GoodsRelated
{
    public class GoodsSetMeal
    {
        /// <summary>
        /// 套餐主项
        /// </summary>
        public Guid ParentGoodsID { get; set; }

        /// <summary>
        /// 组号
        /// </summary>
        public int GroupNo { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 是否必选
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 限定数量
        /// </summary>
        public int LimitedQty { get; set; }

        /// <summary>
        /// 套餐明细的数量
        /// </summary>
        public decimal ItemQty { get; set; }

        /// <summary>
        /// 套餐明细的折扣类型
        /// </summary>
        public int DiscountType { get; set; }

        /// <summary>
        /// 套餐明细的折扣率
        /// </summary>
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// 套餐明细的折固定金额
        /// </summary>
        public decimal OffFixPay { get; set; }

        /// <summary>
        /// GoodsID
        /// </summary>
        public Guid GoodsID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string GoodsNo { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 名称第二语言
        /// </summary>
        public string GoodsName2nd { get; set; }

        /// <summary>
        /// 单位售价
        /// </summary>
        public decimal SellPrice { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Unit { get; set; }

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
