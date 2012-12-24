using System;
using System.Collections.Generic;

namespace Top4ever.Domain.Transfer
{
    public class ItemsPrice
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }
        /// <summary>
        /// 品项名称
        /// </summary>
        public string ItemsName { get; set; }
        /// <summary>
        /// 品项总数量
        /// </summary>
        public decimal ItemsTotalQty { get; set; }
        /// <summary>
        /// 品项总价格
        /// </summary>
        public decimal ItemsTotalPrice { get; set; }
    }
}
