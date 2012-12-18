using System;
using System.Collections.Generic;

namespace Top4ever.Domain.Transfer
{
    public class DeletedOrderDetails
    {
        /// <summary>
        /// OrderDetailsID
        /// </summary>
        public Guid OrderDetailsID { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQuantity { get; set; }
        /// <summary>
        /// 剩余数量的折扣额
        /// </summary>
        public decimal OffPay { get; set; }
        /// <summary>
        /// 授权的经理人ID
        /// </summary>
        public Guid AuthorisedManager { get; set; }
        /// <summary>
        /// 删除操作的员工
        /// </summary>
        public string CancelEmployeeNo { get; set; }
        /// <summary>
        /// 原因名称
        /// </summary>
        public string CancelReasonName { get; set; }
    }
}
