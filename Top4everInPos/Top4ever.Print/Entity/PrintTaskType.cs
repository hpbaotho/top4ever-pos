using System;
using System.Collections.Generic;
using System.Text;

namespace Top4ever.Print.Entity
{
    public enum PrintTaskType
    {
        /// <summary>
        /// 加单
        /// </summary>
        AddBill = 1,
        /// <summary>
        /// 删单
        /// </summary>
        DeleteBill = 2,
        /// <summary>
        /// 催单
        /// </summary>
        Reminder = 3,
        /// <summary>
        /// 转台
        /// </summary>
        TurnTable = 4
    }
}
