using System;

namespace Top4ever.Entity.Enum
{
    public enum ButtonOperateType
    {
        /// <summary>
        /// 默认
        /// </summary>
        NONE = 0,
        /// <summary>
        /// 点单
        /// </summary>
        ORDER = 1,
        /// <summary>
        /// 清空
        /// </summary>
        CLEAR = 2,
        /// <summary>
        /// 转台
        /// </summary>
        CHANGE_DESK = 3,
        /// <summary>
        /// 结账
        /// </summary>
        CHECKOUT = 4
    }
}
