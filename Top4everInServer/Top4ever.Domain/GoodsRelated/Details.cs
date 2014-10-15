using System;
using System.Collections.Generic;

namespace Top4ever.Domain.GoodsRelated
{
    public class Details
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid DetailsID { get; set; }

        /// <summary>
        /// 细项编号
        /// </summary>
        public string DetailsNo { get; set; }

        /// <summary>
        /// 细项名称
        /// </summary>
        public string DetailsName { get; set; }

        /// <summary>
        /// 细项名称第二语言
        /// </summary>
        public string DetailsName2nd { get; set; }

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
        /// 简码
        /// </summary>
        public string BrevityCode { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinyinCode { get; set; }

        /// <summary>
        /// 厨房打印机
        /// </summary>
        public string PrintSolutionName { get; set; }

        /// <summary>
        /// 所属细项组Id
        /// </summary>
        public Guid DetailsGroupID { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public Guid DepartID { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public Guid ButtonStyleID { get; set; }

        /// <summary>
        /// 细项具有的细项组ID
        /// </summary>
        public virtual IList<Guid> DetailsGroupIDList { get; set; }
    }
}
