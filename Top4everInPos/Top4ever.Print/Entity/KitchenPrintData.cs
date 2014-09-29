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
        /// 转台
        /// </summary>
        public string DeskOperateText { get; set; }
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
        /// 子桌号
        /// </summary>
        public int SubOrderNo { get; set; }
        /// <summary>
        /// 原桌号
        /// </summary>
        public string SrcDeskName { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string TranSequence { get; set; }
        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<GoodsOrder> GoodsOrderList { get; set; }

        /// <summary>
        /// 得到属性值
        /// </summary>
        /// <param name="fieldName">属性字符</param>
        /// <returns></returns>
        public string GetValue(string fieldName)
        {
            string result;
            switch (fieldName.ToLower())
            {
                case "billtitle":
                    result = this.BillTitle;
                    break;
                case "orderno":
                    result = this.OrderNo;
                    break;
                case "eattypename":
                    result = this.EatTypeName;
                    break;
                case "tasktypename":
                    result = this.TaskTypeName;
                    break;
                case "deskoperatetext":
                    result = this.DeskOperateText;
                    break;
                case "printsolutionname":
                    result = this.PrintSolutionName;
                    break;
                case "srcdeskname":
                    result = this.SrcDeskName;
                    break;
                case "reason":
                    result = this.Reason;
                    break;
                case "deskname":
                    result = this.DeskName;
                    break;
                case "personnum":
                    result = this.PersonNum;
                    break;
                case "printtime":
                    result = this.PrintTime;
                    break;
                case "transequence":
                    result = this.TranSequence;
                    break;
                case "employeeno":
                    result = this.EmployeeNo;
                    break;
                default:
                    result = string.Empty;
                    break;
            }
            return result;
        }
    }
}
