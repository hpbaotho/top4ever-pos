using System;
using System.Collections.Generic;

namespace Top4ever.Print.Entity
{
    public class KitchenPrintData
    {
        /// <summary>
        /// 厨打单标题
        /// </summary>
        public string BillTitle { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 业务类型 (店用,外带,外送)
        /// </summary>
        public int EatType { get; set; }
        /// <summary>
        /// 业务类型名称
        /// </summary>
        public string EatTypeName { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public int TaskType { get; set; }
        /// <summary>
        /// 任务类型名称
        /// </summary>
        public string TaskTypeName { get; set; }
        /// <summary>
        /// 打印方案名称
        /// </summary>
        public string PrintSolutionName { get; set; }
        /// <summary>
        /// 打印时间
        /// </summary>
        public string PrintTime { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public string PersonNum { get; set; }
        /// <summary>
        /// 员工号
        /// </summary>
        public string EmployeeNo { get; set; }
        /// <summary>
        /// 桌号
        /// </summary>
        public string DeskName { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string TranSequence { get; set; }
        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<GoodsOrder> GoodsOrderList { get; set; }
    }
}
