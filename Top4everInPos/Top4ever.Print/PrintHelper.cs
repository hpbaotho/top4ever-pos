using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;

using Top4ever.Hardware;

namespace Top4ever.Print
{
    public enum TextAlign { Left, Center, Right }

    public class PrintHelper : IDisposable
    {
        private const byte HT = 0x9;
        private const byte LF = 0x0A;
        private const byte CR = 0x0D;
        private const byte ESC = 0x1B;
        private const byte DLE = 0x10;
        private const byte GS = 0x1D;
        private const byte FS = 0x1C;
        private const byte STX = 0x02;
        private const byte US = 0x1F;
        private const byte CAN = 0x18;
        private const byte CLR = 0x0C;

        private PortUtil m_PortUtil;

        public PrintHelper(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            m_PortUtil = new PortUtil(portName, baudRate, parity, dataBits, stopBits); 
        }

        public PrintHelper(string usbVID, string usbPID)
        {
            m_PortUtil = new PortUtil(usbVID, usbPID);
        }

        public PrintHelper(string ip, int port)
        {
            m_PortUtil = new PortUtil(ip, port);
        }

        public void Open()
        {
            m_PortUtil.Open();
        }

        /// <summary>
        /// 向打印机中发送数据
        /// </summary>
        public void Write(byte[] data)
        {
            m_PortUtil.Write(data);
        }

        /// <summary>
        /// 向打印机中发送数据
        /// </summary>
        public void Write(string data)
        {
            m_PortUtil.Write(data);
        }

        public void Write(string data, TextAlign textAlign, int pagerWidth)
        {
            int len = Encoding.Default.GetBytes(data).Length;
            if (len >= pagerWidth)
            {
                data = CutString(data, pagerWidth);
                Write(data);
            }
            else
            {
                StringBuilder result = new StringBuilder();
                switch (textAlign)
                {
                    case TextAlign.Left:
                        result.Append(data);
                        for (int i = 0; i < pagerWidth - len; i++)
                        {
                            result.Append(" ");
                        }
                        break;
                    case TextAlign.Center:
                        int cutLength = (pagerWidth - len) / 2;
                        for (int i = 0; i < cutLength; i++)
                        {
                            result.Append(" ");
                        }
                        result.Append(data);
                        for (int i = 0; i < pagerWidth - len - cutLength; i++)
                        {
                            result.Append(" ");
                        }
                        break;
                    case TextAlign.Right:
                        for (int i = 0; i < pagerWidth - len; i++)
                        {
                            result.Append(" ");
                        }
                        result.Append(data);
                        break;
                    default:
                        break;
                }
                Write(result.ToString());
            }
        }

        public void CutPaper()
        {
            byte[] cutPaper = new byte[] { GS, (byte)'V', 0x42, 0x00 };
            Write(cutPaper);
        }

        public void SetCutPaperMode(int n)
        {
            n = n % 256;
            int m = 66;
            byte[] cutPaper = new byte[] { GS, (byte)'V', (byte)m, (byte)n };
            Write(cutPaper);
        }

        public void SetFontSize(int widthMulti, int heightMulti)
        {
            int val = widthMulti * 16 + heightMulti;
            byte[] fontSize = new byte[] { GS, (byte)'!', (byte)val };
            Write(fontSize);
        }

        public void SetMultiByteCharMode(bool doubleWidth, bool doubleHeight, bool underLine)
        {
            int n = 0;
            if (doubleWidth)
            {
                n |= 1 << 2;
            }
            if (doubleHeight)
            {
                n |= 1 << 3;
            }
            if (underLine)
            {
                n |= 1 << 7;
            }
            byte[] multiByteChar = new byte[] { FS, (byte)'!', (byte)n };
            Write(multiByteChar);
        }

        public void SetPrintMode(bool fontB, bool both, bool doubleWidth, bool doubleHeight, bool underLine)
        {
            int n = 0;
            if (fontB)
            {
                n |= 1;
            }
            if (both)
            {
                n |= 1 << 3;
            }
            if (doubleHeight)
            {
                n |= 1 << 4;
            }
            if (doubleWidth)
            {
                n |= 1 << 5;
            }
            if (underLine)
            {
                n |= 1 << 7;
            }
            byte[] printMode = new byte[] { ESC, (byte)'!', (byte)n };
            Write(printMode);
        }

        public void SetDefaultColor()
        {
            byte[] color = new byte[] { ESC, (byte)'r', 0x00 };
            Write(color);
        }

        public void SetRedColor()
        {
            byte[] color = new byte[] { ESC, (byte)'r', 0x01 };
            Write(color);
        }

        /// <summary>
        /// 初始化打印机
        /// </summary>
        public void SetPrinterInit()
        {
            byte[] printerInit = new byte[] { ESC, (byte)'@' };
            Write(printerInit);
        }

        /// <summary>
        /// 左对齐
        /// </summary>
        public void SetAlignLeft()
        {
            byte[] alignLeft = new byte[] { ESC, (byte)'a', 0x00 };
            Write(alignLeft);
        }

        /// <summary>
        /// 居中
        /// </summary>
        public void SetAlignCenter()
        {
            byte[] alignCenter = new byte[] { ESC, (byte)'a', 0x01 };
            Write(alignCenter);
        }

        /// <summary>
        /// 右对齐
        /// </summary>
        public void SetAlignRight()
        {
            byte[] alignRight = new byte[] { ESC, (byte)'a', 0x02 };
            Write(alignRight);
        }

        /// <summary>
        /// 设置字符右间距
        /// </summary>
        /// <param name="n">0 ≤ n ≤ 255 </param>
        /// <returns></returns>
        public void SetCharSpacing(int n)
        {
            n = (n > -1 || n < 256 ? n : 0);
            byte[] charSpacing = new byte[] { ESC, (byte)' ', (byte)n };
            Write(charSpacing);
        }

        /// <summary>
        /// 行间距离
        /// </summary>
        /// <param name="n">默认0x00</param>
        /// <returns></returns>
        public void SetLineSpacing(int n)
        {
            n = (n > -1 || n < 256 ? n : 24);
            byte[] lineSpacing = new byte[] { ESC, (byte)'3', (byte)n };
            Write(lineSpacing);
        }

        /// <summary>
        /// 打印并走行
        /// </summary>
        /// <param name="n">行数</param>
        /// <returns></returns>
        public void PrintAndFeedLines(int n)
        {
            n = (n > 255 ? 255 : n);
            n = (n < 0 ? 0 : n);
            byte[] feedLines = new byte[] { ESC, (byte)'d', (byte)n };
            Write(feedLines);
        }

        private string CutString(string strInput, int strLen)
        {
            strInput = strInput.Trim();
            byte[] myByte = Encoding.Default.GetBytes(strInput);
            if (myByte.Length > strLen)
            {
                string resultStr = string.Empty;
                for (int i = 0; i < strInput.Length; i++)
                {
                    byte[] tempByte = Encoding.Default.GetBytes(resultStr);
                    if (tempByte.Length < strLen)
                    {
                        resultStr += strInput.Substring(i, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                return resultStr;
            }
            else
            {
                return strInput;
            }
        }

        public void Close()
        {
            m_PortUtil.Close();
        }

        public void Dispose()
        {
            m_PortUtil.Dispose();
        }
    }
}
