using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Top4ever.Domain.Transfer
{
    public class GoodsInfo
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
        /// 每个菜品所拥有的细项组ID
        /// </summary>
        public virtual IList<Guid> DetailsGroupIds { get; set; }
    }
}
