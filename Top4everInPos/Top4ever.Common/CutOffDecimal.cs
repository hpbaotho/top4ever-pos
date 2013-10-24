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
        /// <param name="IsCutTail">是否去尾</param>
        /// <param name="cutTailType">去尾类别</param>
        /// <param name="cutTailDigit">去尾保留位数</param>
        /// <returns>返回去尾后的数值</returns>
        public static Decimal HandleCutOff(decimal originalNumber, bool IsCutTail, int cutTailType, int cutTailDigit)
        {
            //去尾处理后返回的数
            if (IsCutTail)
            {
                CutOffType cutOffType = (CutOffType)cutTailType;
                if (cutOffType == CutOffType.ROUND_OFF)
                {
                    string strDigit = string.Empty;
                    if (cutTailDigit == 0)
                    {
                        strDigit = "f0";
                    }
                    else if (cutTailDigit == 1)
                    {
                        strDigit = "f1";
                    }
                    else if (cutTailDigit == 2)
                    {
                        strDigit = "f2";
                    }
                    if (string.IsNullOrEmpty(strDigit))
                    {
                        originalNumber = Math.Round(originalNumber, cutTailDigit);
                    }
                    else
                    {
                        originalNumber = Convert.ToDecimal(originalNumber.ToString(strDigit));
                    }
                }
                else if (cutOffType == CutOffType.UNCONDITIONALLY_CARRY)
                {
                    if (cutTailDigit == 0)
                    {
                        originalNumber = Round(originalNumber, 0);
                    }
                    else if (cutTailDigit == 1)
                    {
                        originalNumber = Round(originalNumber, 1);
                    }
                    else if (cutTailDigit == 2)
                    {
                        originalNumber = Round(originalNumber, 2);
                    }
                }
                else if (cutOffType == CutOffType.UNCONDITIONALLY_ELIMINATED)
                {
                    if (cutTailDigit == 0)
                    {
                        originalNumber = Convert.ToDecimal((int)originalNumber);
                    }
                    else if (cutTailDigit == 1)
                    {
                        string numeric = originalNumber.ToString("f2");
                        numeric = numeric.Substring(0, numeric.Length - 1);
                        originalNumber = decimal.Parse(numeric);
                    }
                    else if (cutTailDigit == 2)
                    {
                        string numeric = originalNumber.ToString("f3");
                        numeric = numeric.Substring(0, numeric.Length - 1);
                        originalNumber = decimal.Parse(numeric);
                    }
                }
            }
            return originalNumber;
        }

        private static decimal Round(decimal value, int digit)
        {
            decimal vt = (decimal)Math.Pow(10, digit + 1);
            return Math.Round(value + 5 / vt, digit);
        }
    }
}

