using System;
using System.Collections.Generic;

namespace Top4ever.Domain.GoodsRelated
{
    public class Goods
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid GoodsID { get; set; }

        /// <summary>
        /// 菜品编号
        /// </summary>
        public string GoodsNo { get; set; }

        /// <summary>
        /// 条形码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 菜品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 菜品名称第二语言
        /// </summary>
        public string GoodsName2nd { get; set; }

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

        /// <summary>
        /// 外键
        /// </summary>
        public Guid ButtonStyleID { get; set; }
        
        /// <summary>
        /// 每个菜品所拥有的细项组ID
        /// </summary>
        public virtual IList<Guid> DetailsGroupIDList { get; set; }
    }
}
