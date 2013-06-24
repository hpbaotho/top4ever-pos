using System;
using System.Collections.Generic;

namespace Top4ever.Domain.OrderRelated
{
    [Serializable]
    public class OrderDetails
    {
        /// <summary>
        /// 单子详情ID
        /// </summary>
        public Guid OrderDetailsID { get; set; }

        /// <summary>
        /// 单子ID
        /// </summary>
        public Guid OrderID { get; set; }

        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceNo { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public int ItemType { get; set; }

        /// <summary>
        /// 主键(包括DetailID)
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
        /// 单位名称
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 能否打折
        /// </summary>
        public bool CanDiscount { get; set; }

        /// <summary>
        /// 细项的层次
        /// </summary>
        public int ItemLevel { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal ItemQty { get; set; }

        /// <summary>
        /// 单位售价
        /// </summary>
        public decimal SellPrice { get; set; }

        /// <summary>
        /// 售价总计
        /// </summary>
        public decimal TotalSellPrice { get; set; }

        /// <summary>
        /// 折扣总计
        /// </summary>
        public decimal TotalDiscount { get; set; }

        /// <summary>
        /// 挂单
        /// </summary>
        public int Wait { get; set; }

        /// <summary>
        /// 日结号
        /// </summary>
        public string DailyStatementNo { get; set; }

        /// <summary>
        /// 厨房打印机
        /// </summary>
        public string PrintSolutionName { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public Guid DepartID { get; set; }

        /// <summary>
        /// 员工主键
        /// </summary>
        public Guid EmployeeID { get; set; }
    }
}
