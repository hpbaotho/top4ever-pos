using System;
using System.Collections.Generic;
using System.Text;

namespace Top4ever.Hardware.ClientShow
{
    public interface IDualLineClientShow
    {
        /// <summary>
        /// 清屏
        /// </summary>
        void ClientShowCLS();
        /// <summary>
        /// 显示单价和总价
        /// </summary>
        void ShowUnitAndTotalPrice(string unitPrice, string totalPrice);
        /// <summary>
        /// 显示收款和找零
        /// </summary>
        void ShowReceiptAndChange(string receipt, string change);
    }
}
