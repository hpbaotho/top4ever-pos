using System;

namespace Top4ever.Domain.GoodsRelated
{
    public class GoodsCronTrigger
    {
        /// <summary>
        /// 组或者品项的主键
        /// </summary>
        public Guid ItemID { get; set; }
        /// <summary>
        /// 品项类别
        /// </summary>
        public int ItemType { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 周
        /// </summary>
        public string Week { get; set; }
        /// <summary>
        /// 日
        /// </summary>
        public string Day { get; set; }
        /// <summary>
        /// 小时
        /// </summary>
        public string Hour { get; set; }
        /// <summary>
        /// 分钟
        /// </summary>
        public string Minute { get; set; }
    }
}
