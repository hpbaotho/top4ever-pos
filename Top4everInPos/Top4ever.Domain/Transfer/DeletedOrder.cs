using System;
using System.Collections.Generic;

namespace Top4ever.Domain.Transfer
{
    public class DeletedOrder
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid OrderID { get; set; }
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
