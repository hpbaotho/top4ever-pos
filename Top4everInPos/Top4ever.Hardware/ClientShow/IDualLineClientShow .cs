using System;
using System.Collections.Generic;
using System.Text;

namespace Top4ever.Hardware.ClientShow
{
    public interface IDualLineClientShow
    {
        /// <summary>
        /// ����
        /// </summary>
        void ClientShowCLS();
        /// <summary>
        /// ��ʾ���ۺ��ܼ�
        /// </summary>
        void ShowUnitAndTotalPrice(string unitPrice, string totalPrice);
        /// <summary>
        /// ��ʾ�տ������
        /// </summary>
        void ShowReceiptAndChange(string receipt, string change);
    }
}
