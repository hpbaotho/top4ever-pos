using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Top4ever.Common;
using Top4ever.Print.DriverPrintConfig;
using Top4ever.Print.Entity;

namespace Top4ever.Print
{
    public class DriverCardPrint
    {
        private static readonly object Obj = new object();
        private static readonly Dictionary<string, DriverCardPrint> DicInstance = new Dictionary<string, DriverCardPrint>();

        private readonly PrintDocument _printDocument;
        private readonly PrintConfig _curPrintConfig;

        private String _printLayoutPath;
        private readonly String _printCardConsumeLayoutPath;
        private readonly String _printCardStoreLayoutPath;

        private PrintMemberCard _printData = null;
        private Graphics m_Graphics;
        private Rectangle m_PageBounds;
        private float MarginLeft;
        private float PY;

        private DriverCardPrint(string printerName, string paperName, string paperType)
        {
            string printConfigPath = AppDomain.CurrentDomain.BaseDirectory + "PrintConfig\\PrintCardSetting.config";
            if (!File.Exists(printConfigPath))
            {
                throw new ArgumentNullException("Print config file does not exist.");
            }
            DriverPrintSetting printSetting = XmlUtil.Deserialize<DriverPrintSetting>(printConfigPath);
            if (printSetting != null && printSetting.PrintConfigs != null && printSetting.PrintConfigs.Count > 0)
            {
                foreach (PrintConfig printConfig in printSetting.PrintConfigs)
                {
                    if (printConfig.PaperType == paperType)
                    {
                        _curPrintConfig = printConfig;
                        break;
                    }
                }
            }
            if (_curPrintConfig != null)
            {
                if (_curPrintConfig.PrintLayouts != null && _curPrintConfig.PrintLayouts.LayoutList != null && _curPrintConfig.PrintLayouts.LayoutList.Count > 0)
                {
                    foreach (var printLayout in _curPrintConfig.PrintLayouts.LayoutList)
                    {
                        if (printLayout.Key.Equals("PrintCardConsume", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _printCardConsumeLayoutPath = string.Format("{0}PrintConfig\\{1}", AppDomain.CurrentDomain.BaseDirectory, printLayout.Value);
                        }
                        else if (printLayout.Key.Equals("PrintCardStore", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _printCardStoreLayoutPath = string.Format("{0}PrintConfig\\{1}", AppDomain.CurrentDomain.BaseDirectory, printLayout.Value);
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException(string.Format("Can not find the {0} paper type.", paperType));
            }
            int width = 283, height = 1869;
            if (paperType.Equals("58mm", StringComparison.CurrentCultureIgnoreCase))
            {
                width = 203;
            }
            else if (paperType.Equals("76mm", StringComparison.CurrentCultureIgnoreCase))
            {
                width = 268;
            }
            else if (paperType.Equals("80mm", StringComparison.CurrentCultureIgnoreCase))
            {
                width = 283;
            }
            _printDocument = new PrintDocument();
            _printDocument.PrinterSettings.PrinterName = printerName;
            PaperSize paperSize = new PaperSize(paperName, width, height);//页面大小;
            foreach (PaperSize ps in _printDocument.PrinterSettings.PaperSizes)
            {
                if (ps.PaperName.Equals(paperName))
                {
                    paperSize = ps;
                }
            }
            _printDocument.DefaultPageSettings.PaperSize = paperSize;
            _printDocument.DefaultPageSettings.Landscape = false;//横向打印
            _printDocument.PrintPage += new PrintPageEventHandler(pDoc_PrintPage);
        }

        /// <summary>
        /// 获取DriverOrderPrint单例
        /// </summary>
        /// <returns></returns>
        public static DriverCardPrint GetInstance(string printerName, string paperName, string paperType)
        {
            if (string.IsNullOrEmpty(printerName))
            {
                throw new ArgumentNullException("Printer name is null.");
            }
            if (string.IsNullOrEmpty(paperName))
            {
                throw new ArgumentNullException("Paper name is null.");
            }
            if (string.IsNullOrEmpty(paperType))
            {
                throw new ArgumentNullException("Paper type is null.");
            }
            lock (Obj)
            {
                string key = printerName + "_" + paperName + "_" + paperType;
                DriverCardPrint instance;
                if (DicInstance.ContainsKey(key))
                {
                    instance = DicInstance[key];
                }
                else
                {
                    instance = new DriverCardPrint(printerName, paperName, paperType);
                    DicInstance.Add(key, instance);
                }
                return instance;
            }
        }

        /// <summary>
        /// 卡消费
        /// </summary>
        /// <param name="printData"></param>
        public void DoPrintCardConsume(PrintMemberCard printData)
        {
            if (printData != null)
            {
                _printLayoutPath = _printCardConsumeLayoutPath;
                _printData = printData;
                try
                {
                    _printDocument.Print();
                }
                catch (Exception exception)
                {
                    LogHelper.GetInstance().Error("DriverOrderPrint.DoPrintOrder error.", exception);
                }
            }
        }

        /// <summary>
        /// 卡储值
        /// </summary>
        /// <param name="printData"></param>
        public void DoPrintCardStore(PrintMemberCard printData)
        {
            if (printData != null)
            {
                _printLayoutPath = _printCardStoreLayoutPath;
                _printData = printData;
                try
                {
                    _printDocument.Print();
                }
                catch (Exception exception)
                {
                    LogHelper.GetInstance().Error("DriverOrderPrint.DoPrintPrePayOrder error.", exception);
                }
            }
        }

        private void pDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            m_Graphics = e.Graphics;
            m_PageBounds = e.PageBounds;
            DrawPaper();
        }

        private void DrawPaper()
        {
            string paperMargin = _curPrintConfig.PaperMargin;
            if (paperMargin.IndexOf(',') > 0)
            {
                string[] marginArr = paperMargin.Split(',');
                MarginLeft = float.Parse(marginArr[0]);
                PY = float.Parse(marginArr[1]);
            }
            else
            {
                MarginLeft = PY = float.Parse(paperMargin);
            }
            using (StreamReader stream = new StreamReader(_printLayoutPath))
            {
                stream.BaseStream.Seek(0, SeekOrigin.Begin);
                string strLine = stream.ReadLine();
                while (strLine != null)
                {
                    HandlerNormalConfig(strLine);
                    strLine = stream.ReadLine();
                }
            }
        }

        private void HandlerNormalConfig(string normalConfig)
        {
            List<string> itemArr = new List<string>();
            MatchCollection mc = Regex.Matches(normalConfig, @"\[([^\]]*)\]");
            foreach (Match m in mc)
            {
                itemArr.Add(m.Groups[1].Value);
            }
            //处理每一项
            float maxHeight = 0f;
            float px = MarginLeft;
            foreach (string str in itemArr)
            {
                if (str.Contains("Line"))
                {
                    foreach (PrintLine line in _curPrintConfig.PrintLines.LineList)
                    {
                        if (line.LineName == str)
                        {
                            Font font = new Font(line.Font.FontName, line.Font.FontSize, line.Font.FontStyle);
                            Color color = Color.FromName(line.Font.ForeColor);
                            Brush brush = new SolidBrush(color);
                            SizeF size = m_Graphics.MeasureString(line.LineText, font);//测量字体的大小
                            float startPX = 0f, endPX = 0f;
                            if (line.StartPX.IndexOf('%') > 0)
                            {
                                string strAlign = line.StartPX;
                                string number = strAlign.Substring(0, strAlign.Length - 1);
                                startPX = float.Parse(number) / 100 * (m_PageBounds.Width - 2 * MarginLeft);
                            }
                            else
                            {
                                startPX = float.Parse(line.StartPX.Trim());
                            }
                            if (line.EndPX.IndexOf('%') > 0)
                            {
                                string strAlign = line.EndPX;
                                string number = strAlign.Substring(0, strAlign.Length - 1);
                                endPX = float.Parse(number) / 100 * (m_PageBounds.Width - 2 * MarginLeft);
                            }
                            else
                            {
                                endPX = float.Parse(line.EndPX.Trim());
                            }
                            //计算分割线重复的数量
                            int count = Convert.ToInt32((endPX - startPX) / size.Width);
                            StringBuilder sbLine = new StringBuilder(2 * count);
                            for (int i = 0; i < count; i++)
                            {
                                sbLine.Append(line.LineText);
                            }
                            //判断分割线是否符合长度
                            do
                            {
                                sbLine.Append(line.LineText);
                                size = m_Graphics.MeasureString(sbLine.ToString(), font);//测量分割线的大小
                            } while (size.Width < endPX - startPX);
                            m_Graphics.DrawString(sbLine.ToString(), font, brush, MarginLeft + startPX, PY);
                            maxHeight = size.Height;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (NormalConfig item in _curPrintConfig.NormalConfigs.NormalConfigList)
                    {
                        if (item.Name == str)
                        {
                            Font font = new Font(item.Font.FontName, item.Font.FontSize, item.Font.FontStyle);
                            Color color = Color.FromName(item.Font.ForeColor);
                            Brush brush = new SolidBrush(color);
                            string itemName = item.ValuePrefix + _printData.GetValue(item.Name);
                            SizeF size = m_Graphics.MeasureString(itemName, font);//测量字体的大小
                            float itemWidth = 0f;
                            if (item.Width.IndexOf('%') > 0)
                            {
                                string strWidth = item.Width.Trim();
                                string number = strWidth.Substring(0, strWidth.Length - 1);
                                itemWidth = float.Parse(number) / 100 * (m_PageBounds.Width - 2 * MarginLeft);
                            }
                            else
                            {
                                itemWidth = float.Parse(item.Width.Trim());
                            }
                            float itemHeight = 0f;
                            if (size.Width > itemWidth)
                            {
                                itemHeight = (float)Math.Ceiling(Convert.ToDecimal(size.Width) / Convert.ToDecimal(itemWidth)) * size.Height;
                            }
                            else
                            {
                                itemHeight = size.Height;
                            }
                            if (itemHeight > maxHeight)
                            {
                                maxHeight = itemHeight;
                            }
                            RectangleF drawRect = new RectangleF(px, PY, itemWidth, itemHeight);
                             StringFormat sf = new StringFormat();
                             sf.LineAlignment = StringAlignment.Center;
                            if (item.Align.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
                            {
                                sf.Alignment = StringAlignment.Near;
                            }
                            else if (item.Align.Equals("Center", StringComparison.InvariantCultureIgnoreCase))
                            {
                                sf.Alignment = StringAlignment.Center;
                            }
                            else if (item.Align.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
                            {
                                sf.Alignment = StringAlignment.Far;
                            }
                            m_Graphics.DrawString(itemName, font, brush, drawRect, sf);
                            px += itemWidth;
                            break;
                        }
                    }
                }
            }
            PY += maxHeight;
        }
    }
}
