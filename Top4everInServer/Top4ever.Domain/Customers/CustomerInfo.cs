using System;
using System.Collections.Generic;
using System.Text;

namespace Top4ever.Domain.Customers
{
    public class CustomerInfo
    {
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 家庭地址
        /// </summary>
        public string DeliveryAddress1 { get; set; }

        /// <summary>
        /// 工作地址
        /// </summary>
        public string DeliveryAddress2 { get; set; }

        /// <summary>
        /// 其他地址
        /// </summary>
        public string DeliveryAddress3 { get; set; }

        /// <summary>
        /// 最近外送地址索引
        /// </summary>
        public int ActiveIndex { get; set; }

        /// <summary>
        /// 最后修改员工ID
        /// </summary>
        public Guid LastModifiedEmployeeID { get; set; }
    }
}
