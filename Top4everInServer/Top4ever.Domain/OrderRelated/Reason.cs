using System;
using System.Collections.Generic;

namespace Top4ever.Domain.OrderRelated
{
    public class Reason
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ReasonID { get; set; }
        /// <summary>
        /// 具体原因
        /// </summary>
        public string ReasonName { get; set; }
        /// <summary>
        /// 具体原因
        /// </summary>
        public string ReasonName2nd { get; set; }
        /// <summary>
        /// 原因类别
        /// </summary>
        public int ReasonType { get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public Guid ButtonStyleID { get; set; }
    }
}
