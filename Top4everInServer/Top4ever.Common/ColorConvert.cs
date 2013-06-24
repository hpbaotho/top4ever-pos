using System;
using System.Drawing;

namespace Top4ever.Common
{
    public class ColorConvert
    {
        /// <summary>
        /// 将Color对象转化为Int型
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static int ParseRGB(Color color)
        {
            return (int)(((uint)color.B << 16) | (ushort)(((ushort)color.G << 8) | color.R));
        }

        /// <summary>
        /// 将Int型转化为Color类型
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color RGB(int color)
        {
            int r = 0xFF & color;
            int g = 0xFF00 & color;
            g >>= 8;
            int b = 0xFF0000 & color;
            b >>= 16;
            return Color.FromArgb(r, g, b);
        }
    }
}
