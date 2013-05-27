using System;
using System.Collections.Generic;

namespace Top4ever.Domain
{
    public class PrintTask
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int PrintTaskID { get; set; }
        /// <summary>
        /// 账单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNum { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid EmployeeID { get; set; }
        /// <summary>
        /// 员工号
        /// </summary>
        public string EmployeeNo { get; set; }
        /// <summary>
        /// 序列号
        /// </summary>
        public int TranSequence { get; set; }
        /// <summary>
        /// 业务类型 (店用,外带,外送)
        /// </summary>
        public int EatType { get; set; }
        /// <summary>
        /// 台号
        /// </summary>
        public string DeskName { get; set; }
        /// <summary>
        /// 子单号
        /// </summary>
        public int SubOrderNo { get; set; }
        /// <summary>
        /// 任务类别
        /// </summary>
        public int TaskType { get; set; }
        /// <summary>
        /// 打印时间
        /// </summary>
        public DateTime PrintTime { get; set; }
        /// <summary>
        /// 原台号
        /// </summary>
        public string SrcDeskName { get; set; }
        /// <summary>
        /// 原因名称
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 是否已经打印
        /// </summary>
        public bool IsPrinted { get; set; }
        /// <summary>
        /// 厨打解决方案名称
        /// </summary>
        public string PrintSolutionName { get; set; }
        /// <summary>
        /// 打印类型（0 一单一切, 1 一菜一切）
        /// </summary>
        public int PrintType { get; set; }
        /// <summary>
        /// 品项名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 套餐品项名称
        /// </summary>
        public string SubGoodsName { get; set; }
        /// <summary>
        /// 细项名称
        /// </summary>
        public string DetailsName { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal ItemQty { get; set; }
        /// <summary>
        /// 外卖模式细项名称列表
        /// </summary>
        public string TotalDetailsName { get; set; }
    }
}
