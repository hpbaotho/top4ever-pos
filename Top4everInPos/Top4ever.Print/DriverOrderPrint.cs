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
    public class DriverOrderPrint
    {
        private static readonly object Obj = new object();
        private static readonly Dictionary<string, DriverOrderPrint> DicInstance = new Dictionary<string, DriverOrderPrint>();

        private readonly PrintDocument _printDocument;
        private readonly PrintConfig _curPrintConfig;

        private String _printLayoutPath;
        private readonly String _printOrderLayoutPath;
        private readonly String _printPrePayOrderLayoutPath;
        private readonly String _deliveryOrderLayoutPath;
        private readonly String _printPaidOrderLayoutPath;

        private PrintData _printData = null;
        private Graphics m_Graphics;
        private Rectangle m_PageBounds;
        private float MarginLeft;
        private float PY;

        private DriverOrderPrint(string printerName, string paperName, string paperType)
        {
            string printConfigPath = AppDomain.CurrentDomain.BaseDirectory + "PrintConfig\\PrintOrderSetting.config";
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
                        if (printLayout.Key.Equals("PrintOrder", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _printOrderLayoutPath = string.Format("{0}PrintConfig\\{1}", AppDomain.CurrentDomain.BaseDirectory, printLayout.Value);
                        }
                        else if (printLayout.Key.Equals("PrintPrePayOrder", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _printPrePayOrderLayoutPath = string.Format("{0}PrintConfig\\{1}", AppDomain.CurrentDomain.BaseDirectory, printLayout.Value);
                        }
                        else if (printLayout.Key.Equals("DeliveryOrder", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _deliveryOrderLayoutPath = string.Format("{0}PrintConfig\\{1}", AppDomain.CurrentDomain.BaseDirectory, printLayout.Value);
                        }
                        else if (printLayout.Key.Equals("PrintPaidOrder", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _printPaidOrderLayoutPath = string.Format("{0}PrintConfig\\{1}", AppDomain.CurrentDomain.BaseDirectory, printLayout.Value);
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
        public static DriverOrderPrint GetInstance(string printerName, string paperName, string paperType)
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
                DriverOrderPrint instance;
                if (DicInstance.ContainsKey(key))
                {
                    instance = DicInstance[key];
                }
                else
                {
                    instance = new DriverOrderPrint(printerName, paperName, paperType);
                    DicInstance.Add(key, instance);
                }
                return instance;
            }
        }

        public void DoPrintOrder(PrintData printData)
        {
            if (printData != null)
            {
                _printLayoutPath = _printOrderLayoutPath;
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

        public void DoPrintPrePayOrder(PrintData printData)
        {
            if (printData != null)
            {
                _printLayoutPath = _printPrePayOrderLayoutPath;
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

        public void DoPrintDeliveryOrder(PrintData printData)
        {
            if (printData != null)
            {
                _printLayoutPath = _deliveryOrderLayoutPath;
                _printData = printData;
                try
                {
                    _printDocument.Print();
                }
                catch (Exception exception)
                {
                    LogHelper.GetInstance().Error("DriverOrderPrint.DoPrintDeliveryOrder error.", exception);
                }
            }
        }

        public void DoPrintPaidOrder(PrintData printData)
        {
            if (printData != null)
            {
                _printLayoutPath = _printPaidOrderLayoutPath;
                _printData = printData;
                try
                {
                    _printDocument.Print();
                }
                catch (Exception exception)
                {
                    LogHelper.GetInstance().Error("DriverOrderPrint.DoPrintPaidOrder error.", exception);
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

            string[] repeatedConfigArr = null;
            int braceCount = -1;    //{}个数计数
            bool inCirculation = false; //是否在循环内部
            using (StreamReader stream = new StreamReader(_printLayoutPath))
            {
                string strPrintConfig = stream.ReadToEnd();
                List<string> repeatedConfigList = new List<string>();
                MatchCollection mc = Regex.Matches(strPrintConfig, @"(?<={)[^{}]+(?=})");
                foreach (Match m in mc)
                {
                    repeatedConfigList.Add(m.Groups[0].Value);
                }
                repeatedConfigArr = repeatedConfigList.ToArray();
                stream.BaseStream.Seek(0, SeekOrigin.Begin);
                string strLine = stream.ReadLine();
                while (strLine != null)
                {
                    //逻辑
                    if (!inCirculation)
                    {
                        if (strLine.IndexOf('{') > -1)  //meet '{'
                        {
                            braceCount++;
                            strLine = strLine.Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ').Trim();
                            //'{'符号另起一行
                            if (strLine.IndexOf('{') == 0)
                            {
                                HandlerCirculationConfig(repeatedConfigArr[braceCount]);
                            }
                            else
                            {
                                int index = strLine.IndexOf('{');
                                HandlerNormalConfig(strLine.Substring(0, index));
                                HandlerCirculationConfig(repeatedConfigArr[braceCount]);
                            }
                            if (strLine.IndexOf('}') > -1)  //meet '}'
                            {
                                inCirculation = false;
                            }
                            else
                            {
                                inCirculation = true;
                            }
                        }
                        else
                        {
                            HandlerNormalConfig(strLine);
                        }
                    }
                    else
                    {
                        if (strLine.IndexOf('}') > -1)  //meet '}'
                        {
                            inCirculation = false;
                        }
                    }
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

        private void HandlerCirculationConfig(string repeatedConfig)
        {
            string[] strArr = repeatedConfig.Split('$');
            string className = strArr[0].Trim();
            string strProperty = strArr[1].Trim(new char[] { '\r', '\n' });
            //满足属性多行的情况
            List<List<string>> propertyArrList = new List<List<string>>();
            string[] propertyArr = strProperty.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string propertyItem in propertyArr)
            {
                List<string> propertyList = new List<string>();
                MatchCollection mc = Regex.Matches(propertyItem, @"\[([^\]]*)\]");
                foreach (Match m in mc)
                {
                    propertyList.Add(m.Groups[1].Value);
                }
                propertyArrList.Add(propertyList);
            }
            if (className.Equals("GoodsOrder", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (DataListConfig item in _curPrintConfig.DataListConfigs.DataListConfigList)
                {
                    if (item.ClassName == className)
                    {
                        float maxHeight = 0f;
                        //columnHead
                        foreach (List<string> propertyList in propertyArrList)
                        {
                            float px = MarginLeft;
                            foreach (string property in propertyList)
                            {
                                foreach (PrintColumnHead columnHead in item.PrintColumnHeads.ColumnHeadList)
                                {
                                    if (property == columnHead.Name)
                                    {
                                        Font font = new Font(columnHead.Font.FontName, columnHead.Font.FontSize, columnHead.Font.FontStyle);
                                        Color color = Color.FromName(columnHead.Font.ForeColor);
                                        Brush brush = new SolidBrush(color);
                                        string itemName = columnHead.Text;
                                        SizeF size = m_Graphics.MeasureString(itemName, font);//测量字体的大小
                                        float itemWidth = 0f;
                                        if (columnHead.Width.IndexOf('%') > 0)
                                        {
                                            string strWidth = columnHead.Width.Trim();
                                            string number = strWidth.Substring(0, strWidth.Length - 1);
                                            itemWidth = float.Parse(number) / 100 * (m_PageBounds.Width - 2 * MarginLeft);
                                        }
                                        else
                                        {
                                            itemWidth = float.Parse(columnHead.Width.Trim());
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
                                        if (columnHead.Align.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            sf.Alignment = StringAlignment.Near;
                                        }
                                        else if (columnHead.Align.Equals("Center", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            sf.Alignment = StringAlignment.Center;
                                        }
                                        else if (columnHead.Align.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
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
                        //column
                        foreach (GoodsOrder goodsItem in _printData.GoodsOrderList)
                        {
                            maxHeight = 0f;
                            foreach (List<string> propertyList in propertyArrList)
                            {
                                float px = MarginLeft;
                                foreach (string property in propertyList)
                                {
                                    string itemValue = goodsItem.GetValue(property);
                                    foreach (PrintColumn column in item.PrintColumns.PrintColumnList)
                                    {
                                        if (property == column.Name)
                                        {
                                            Color color = Color.FromName(column.Font.ForeColor);
                                            Brush brush = new SolidBrush(color);
                                            Font font = new Font(column.Font.FontName, column.Font.FontSize, column.Font.FontStyle);
                                            SizeF size = m_Graphics.MeasureString(itemValue, font);//测量字体的大小
                                            float itemWidth = 0f;
                                            if (column.Width.IndexOf('%') > 0)
                                            {
                                                string strWidth = column.Width.Trim();
                                                string number = strWidth.Substring(0, strWidth.Length - 1);
                                                itemWidth = float.Parse(number) / 100 * (m_PageBounds.Width - 2 * MarginLeft);
                                            }
                                            else
                                            {
                                                itemWidth = float.Parse(column.Width.Trim());
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
                                            if (column.Align.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                sf.Alignment = StringAlignment.Near;
                                            }
                                            else if (column.Align.Equals("Center", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                sf.Alignment = StringAlignment.Center;
                                            }
                                            else if (column.Align.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                sf.Alignment = StringAlignment.Far;
                                            }
                                            m_Graphics.DrawString(itemValue, font, brush, drawRect, sf);
                                            px += itemWidth;
                                            break;
                                        }
                                    }
                                }
                                PY += maxHeight;
                            }
                        }
                        break;
                    }
                }
            }
            else if (className.Equals("PayingGoodsOrder", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (DataListConfig item in _curPrintConfig.DataListConfigs.DataListConfigList)
                {
                    if (item.ClassName == className)
                    {
                        float maxHeight = 0f;
                        //columnHead
                        foreach (List<string> propertyList in propertyArrList)
                        {
                            float px = MarginLeft;
                            foreach (string property in propertyList)
                            {
                                foreach (PrintColumnHead columnHead in item.PrintColumnHeads.ColumnHeadList)
                                {
                                    if (property == columnHead.Name)
                                    {
                                        Font font = new Font(columnHead.Font.FontName, columnHead.Font.FontSize, columnHead.Font.FontStyle);
                                        Color color = Color.FromName(columnHead.Font.ForeColor);
                                        Brush brush = new SolidBrush(color);
                                        string itemName = columnHead.Text;
                                        SizeF size = m_Graphics.MeasureString(itemName, font);//测量字体的大小
                                        float itemWidth = 0f;
                                        if (columnHead.Width.IndexOf('%') > 0)
                                        {
                                            string strWidth = columnHead.Width.Trim();
                                            string number = strWidth.Substring(0, strWidth.Length - 1);
                                            itemWidth = float.Parse(number) / 100 * (m_PageBounds.Width - 2 * MarginLeft);
                                        }
                                        else
                                        {
                                            itemWidth = float.Parse(columnHead.Width.Trim());
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
                                        if (columnHead.Align.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            sf.Alignment = StringAlignment.Near;
                                        }
                                        else if (columnHead.Align.Equals("Center", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            sf.Alignment = StringAlignment.Center;
                                        }
                                        else if (columnHead.Align.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            sf.Alignment = StringAlignment.Far;
                                        }
                                        m_Graphics.DrawString(itemName, font, brush, drawRect, sf);
                                        px += itemWidth;
                                        break;
                                    }
                                }
                            }
                            PY += maxHeight;
                        }
                        //column
                        foreach (PayingGoodsOrder payingOrder in _printData.PayingOrderList)
                        {
                            maxHeight = 0f;
                            foreach (List<string> propertyList in propertyArrList)
                            {
                                float px = MarginLeft;
                                foreach (string property in propertyList)
                                {
                                    string itemValue = payingOrder.GetValue(property);
                                    foreach (PrintColumn column in item.PrintColumns.PrintColumnList)
                                    {
                                        if (property == column.Name)
                                        {
                                            Color color = Color.FromName(column.Font.ForeColor);
                                            Brush brush = new SolidBrush(color);
                                            Font font = new Font(column.Font.FontName, column.Font.FontSize, column.Font.FontStyle);
                                            SizeF size = m_Graphics.MeasureString(itemValue, font);//测量字体的大小
                                            float itemWidth = 0f;
                                            if (column.Width.IndexOf('%') > 0)
                                            {
                                                string strWidth = column.Width.Trim();
                                                string number = strWidth.Substring(0, strWidth.Length - 1);
                                                itemWidth = float.Parse(number) / 100 * (m_PageBounds.Width - 2 * MarginLeft);
                                            }
                                            else
                                            {
                                                itemWidth = float.Parse(column.Width.Trim());
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
                                            if (column.Align.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                sf.Alignment = StringAlignment.Near;
                                            }
                                            else if (column.Align.Equals("Center", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                sf.Alignment = StringAlignment.Center;
                                            }
                                            else if (column.Align.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                sf.Alignment = StringAlignment.Far;
                                            }
                                            m_Graphics.DrawString(itemValue, font, brush, drawRect, sf);
                                            px += itemWidth;
                                            break;
                                        }
                                    }
                                }
                                PY += maxHeight;
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}
