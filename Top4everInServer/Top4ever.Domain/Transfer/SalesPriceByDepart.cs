using System;
using System.Collections.Generic;

namespace Top4ever.Domain.Transfer
{
    public class SalesPriceByDepart
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }
        /// <summary>
        /// 品项总数量
        /// </summary>
        public decimal TotalItemsNum { get; set; }
        /// <summary>
        /// 部门总金额
        /// </summary>
        public decimal TotalDepartPrice { get; set; }
        /// <summary>
        /// 品项价格列表
        /// </summary>
        public IList<ItemsPrice> ItemsPriceList { get; set; }
    }
}
