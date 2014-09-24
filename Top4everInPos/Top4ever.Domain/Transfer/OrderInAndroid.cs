using System;
using System.Collections.Generic;

namespace Top4ever.Domain.Transfer
{
    public class OrderInAndroid
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 桌号
        /// </summary>
        public string DeskName { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNum { get; set; }

        /// <summary>
        /// 订单详情
        /// </summary>
        public IList<OrderDetail> OrderDetailList { get; set; }
    }

    public class OrderDetail
    {
        /// <summary>
        /// 品项Id
        /// </summary>
        public Guid GoodsId { get; set; }

        /// <summary>
        /// 菜品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int GoodsQty { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal SellPrice { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal TotalDiscount { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}