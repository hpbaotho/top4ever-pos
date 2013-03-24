using System;

namespace Top4ever.Domain.Transfer
{
    public class DeliveryOrder
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid OrderID { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public int TranSequence { get; set; }
        /// <summary>
        /// 业务类型 (店用,外带,外送)
        /// </summary>
        public int EatType { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime? PayTime{ get; set; }
        /// <summary>
        /// 外送时间
        /// </summary>
        public DateTime? DeliveryTime { get; set; }
    }
}
