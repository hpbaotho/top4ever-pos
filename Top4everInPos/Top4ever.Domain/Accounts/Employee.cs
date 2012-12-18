using System;
using System.Collections.Generic;

namespace Top4ever.Domain.Accounts
{
    public class Employee
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid EmployeeID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 最高限额
        /// </summary>
        public decimal LimitMoney { get; set; }

        /// <summary>
        /// 最低折扣
        /// </summary>
        public decimal MinDiscount { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IndentifyNo { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? EntryDate { get; set; }

        /// <summary>
        /// 离职时间
        /// </summary>
        public DateTime? LeaveDate { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public IList<String> RightsCodeList { get; set; }
    }
}
