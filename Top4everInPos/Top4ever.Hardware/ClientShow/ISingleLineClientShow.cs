using System;

namespace Top4ever.Hardware.ClientShow
{
    public interface ISingleLineClientShow
    {
        /// <summary>
        /// 清屏
        /// </summary>
        void ClientShowCLS();
        /// <summary>
        /// 单价
        /// </summary>
        void ShowUnitPrice(string balance);
        /// <summary>
        /// 总计
        /// </summary>
        void ShowTotalPrice(string balance);
        /// <summary>
        /// 收款
        /// </summary>
        void ShowReceipt(string balance);
        /// <summary>
        /// 找零
        /// </summary>
        void ShowChange(string balance);
    }
}
