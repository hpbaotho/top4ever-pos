using System;

namespace Top4ever.Domain.Transfer
{
    public class DeletedItem
    {
        /// <summary>
        /// 账单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public int TranSequence { get; set; }
        /// <summary>
        /// 桌子名称
        /// </summary>
        public string DeskName { get; set; }
        /// <summary>
        /// 结账时间
        /// </summary>
        public DateTime? PayTime { get; set; }
        /// <summary>
        /// 菜品编号
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal ItemQty { get; set; }
        /// <summary>
        /// 售价总计
        /// </summary>
        public decimal TotalSellPrice { get; set; }
        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReasonName { get; set; }
        /// <summary>
        /// 取消人员
        /// </summary>
        public string CancelEmployeeNo { get; set; }
        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        public DateTime LastModifiedTime { get; set; }
    }
}
