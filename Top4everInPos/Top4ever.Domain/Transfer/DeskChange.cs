using System;
using System.Collections.Generic;

namespace Top4ever.Domain.Transfer
{
    public class DeskChange
    {
        /// <summary>
        /// 转台后的桌子
        /// </summary>
        public string DeskName { get; set; }

        /// <summary>
        /// 第一账单ID
        /// </summary>
        public Guid OrderID1st { get; set; }

        /// <summary>
        /// 第二账单ID
        /// </summary>
        public Guid OrderID2nd { get; set; }
    }
}
