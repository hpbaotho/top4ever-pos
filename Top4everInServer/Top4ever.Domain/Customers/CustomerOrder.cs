using System;
using System.Collections.Generic;
using System.Text;

namespace Top4ever.Domain.Customers
{
    public class CustomerOrder
    {
        /// <summary>
        /// 账单OrderID
        /// </summary>
        public Guid OrderID { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 外送地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 外送人员
        /// </summary>
        public string DeliveryEmployeeName { get; set; }
    }
}
