using System;
using System.Collections.Generic;

namespace Top4ever.Domain.Transfer
{
    public class DeskInfo
    {
        /// <summary>
        /// 桌号
        /// </summary>
        public string DeskName { get; set; }
        /// <summary>
        /// 最低消费
        /// </summary>
        public decimal MinConsumption { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
