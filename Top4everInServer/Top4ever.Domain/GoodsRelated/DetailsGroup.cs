using System;
using System.Collections.Generic;

namespace Top4ever.Domain.GoodsRelated
{
    public class DetailsGroup
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid DetailsGroupID { get; set; }

        /// <summary>
        /// 细项组编号
        /// </summary>
        public string DetailsGroupNo { get; set; }

        /// <summary>
        /// 细项组名称
        /// </summary>
        public string DetailsGroupName { get; set; }

        /// <summary>
        /// 细项组名称第二语言
        /// </summary>
        public string DetailsGroupName2nd { get; set; }

        /// <summary>
        /// 同一细项组可选细项的限定数量
        /// </summary>
        public int LimitedNumbers { get; set; }

        /// <summary>
        /// 公共细项组
        /// </summary>
        public bool IsCommon { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public Guid ButtonStyleID { get; set; }

        /// <summary>
        /// 细项组对应的细项
        /// </summary>
        public IList<Details> DetailsList { get; set; }
    }
}
