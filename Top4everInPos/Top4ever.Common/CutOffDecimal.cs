using System;
using System.Collections.Generic;
using System.Text;

namespace Top4ever.Common
{
    public enum CutOffType
    {
        /// <summary>
        /// 四舍五入
        /// </summary>
        ROUND_OFF = 0,
        /// <summary>
        /// 无条件进位
        /// </summary>
        UNCONDITIONALLY_CARRY = 1,
        /// <summary>
        /// 无条件舍去
        /// </summary>
        UNCONDITIONALLY_ELIMINATED = 2
    }

    public class CutOffDecimal
    {
        /// <summary>
        /// 去尾处理
        /// </summary>
        /// <param name="originalNumber">原始数值</param>
        /// <param name="cutOffType">去尾类型</param>
        /// <param name="cutOffDigit">去尾位数</param>
        /// <returns>返回去尾后的数值</returns>
        public static Decimal HandleCutOff(decimal originalNumber, CutOffType cutOffType, int cutOffDigit)
        {
            //去尾处理后返回的数
            decimal cutOffNumber = originalNumber;
            if (cutOffType == CutOffType.ROUND_OFF)
            {
                string strDigit = string.Empty;
                if (cutOffDigit == 0)
                {
                    strDigit = "f0";
                }
                else if (cutOffDigit == 1)
                {
                    strDigit = "f1";
                }
                else if (cutOffDigit == 2)
                {
                    strDigit = "f2";
                }
                if (string.IsNullOrEmpty(strDigit))
                {
                    cutOffNumber = Math.Round(originalNumber, cutOffDigit);
                }
                else
                {
                    cutOffNumber = Convert.ToDecimal(originalNumber.ToString(strDigit));
                }
            }
            else if (cutOffType == CutOffType.UNCONDITIONALLY_CARRY)
            {
                if (cutOffDigit == 0)
                {
                    originalNumber = Round(originalNumber, 0);
                }
                else if (cutOffDigit == 1)
                {
                    originalNumber = Round(originalNumber, 1);
                }
                else if (cutOffDigit == 2)
                {
                    originalNumber = Round(originalNumber, 2);
                }
            }
            else if (cutOffType == CutOffType.UNCONDITIONALLY_ELIMINATED)
            {
                if (cutOffDigit == 0)
                {
                    originalNumber = Convert.ToDecimal((int)originalNumber);
                }
                else if (cutOffDigit == 1)
                {
                    string numeric = originalNumber.ToString("f2");
                    numeric = numeric.Substring(0, numeric.Length - 1);
                    originalNumber = decimal.Parse(numeric);
                }
                else if (cutOffDigit == 2)
                {
                    string numeric = originalNumber.ToString("f3");
                    numeric = numeric.Substring(0, numeric.Length - 1);
                    originalNumber = decimal.Parse(numeric);
                }
            }
            return cutOffNumber;
        }

        public static decimal Round(decimal value, int digit)
        {
            decimal vt = (decimal)Math.Pow(10, digit + 1);
            return Math.Round(value + 5 / vt, digit);
        }
    }
}

