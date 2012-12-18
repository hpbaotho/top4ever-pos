using System;
using System.Collections.Generic;

namespace Top4ever.Domain.Transfer
{
    [Serializable]
    public class HandoverInfo
    {
        /// <summary>
        /// 交班记录
        /// </summary>
        public HandoverRecord handoverRecord { get; set; }
        /// <summary>
        /// 交班金额列表
        /// </summary>
        public IList<HandoverTurnover> handoverTurnoverList { get; set; }
        /// <summary>
        /// 支付方式合计
        /// </summary>
        public IList<OrderPayoffSum> orderPayoffSumList { get; set; }
        /// <summary>
        /// 折扣合计
        /// </summary>
        public IList<OrderDiscountSum> orderDiscountSumList { get; set; }
    }
}
