using System;
using System.Collections.Generic;

namespace Top4ever.Domain
{
    public class Shop
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ShopID { get; set; }
        /// <summary>
        /// 店铺编号
        /// </summary>
        public string ShopNo { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 公司别名
        /// </summary>
        public string CompanyAlias { get; set; }
        /// <summary>
        /// 经理
        /// </summary>
        public string Manager { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 店铺注册地址
        /// </summary>
        public string RegAddress { get; set; }
        /// <summary>
        /// 店铺经营地址
        /// </summary>
        public string RunAddress { get; set; }
        /// <summary>
        /// 经营面积
        /// </summary>
        public decimal Area { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
