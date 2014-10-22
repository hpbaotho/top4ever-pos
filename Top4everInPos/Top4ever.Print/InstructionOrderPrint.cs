using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;

using Top4ever.Common;
using Top4ever.Print.InstructionPrintConfig;
using Top4ever.Print.Entity;

namespace Top4ever.Print
{
    public class InstructionOrderPrint
    {
        private readonly PrintHelper _printHelper;
        private String _printLayoutPath;
        private PrintData _printData = null;

        private static PrintConfig _curPrintConfig = null;
        private static String _printOrderLayoutPath;
        private static String _printPrePayOrderLayoutPath;
        private static String _deliveryOrderLayoutPath;
        private static String _printPaidOrderLayoutPath;

        public InstructionOrderPrint(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, string paperType)
        {
            _printHelper = new PrintHelper(portName, baudRate, parity, dataBits, stopBits);
            InitPrintSetting(paperType);
        }

        public InstructionOrderPrint(string usbVid, string usbPid, string endpointId, string paperType)
        {
            _printHelper = new PrintHelper(usbVid, usbPid, endpointId);
            InitPrintSetting(paperType);
        }

        public InstructionOrderPrint(string ip, int port, string paperType)
        {
            _printHelper = new PrintHelper(ip, port);
            InitPrintSetting(paperType);
        }

        private void InitPrintSetting(string paperType)
        {
            if (_curPrintConfig == null)
            {
                string printConfigPath = AppDomain.CurrentDomain.BaseDirectory + "PrintConfig\\InstructionPrintOrderSetting.config";
                if (!File.Exists(printConfigPath))
                {
                    throw new ArgumentNullException("Print config file does not exist.");
                }
                InstructionOrderPrintSetting printSetting = XmlUtil.Deserialize<InstructionOrderPrintSetting>(printConfigPath);
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
                    _printHelper.Open();
                    _printHelper.SetPrinterInit();
                    DrawPaper();
                    _printHelper.CutPaper();
                }
                catch (Exception exception)
                {
                    LogHelper.GetInstance().Error("InstructionOrderPrint.DoPrintOrder error.", exception);
                }
                finally
                {
                    _printHelper.Close();
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
                    _printHelper.Open();
                    _printHelper.SetPrinterInit();
                    DrawPaper();
                    _printHelper.CutPaper();
                }
                catch (Exception exception)
                {
                    LogHelper.GetInstance().Error("InstructionOrderPrint.DoPrintPrePayOrder error.", exception);
                }
                finally
                {
                    _printHelper.Close();
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
                    _printHelper.Open();
                    _printHelper.SetPrinterInit();
                    DrawPaper();
                    _printHelper.CutPaper();
                }
                catch (Exception exception)
                {
                    LogHelper.GetInstance().Error("InstructionOrderPrint.DoPrintDeliveryOrder error.", exception);
                }
                finally
                {
                    _printHelper.Close();
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
                    _printHelper.Open();
                    _printHelper.SetPrinterInit();
                    DrawPaper();
                    _printHelper.CutPaper();
                }
                catch (Exception exception)
                {
                    LogHelper.GetInstance().Error("InstructionOrderPrint.DoPrintPaidOrder error.", exception);
                }
                finally
                {
                    _printHelper.Close();
                }
            }
        }

        private void DrawPaper()
        {
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
            foreach (string str in itemArr)
            {
                if (str.Contains("Line"))
                {
                    foreach (PrintLine line in _curPrintConfig.PrintLines.LineList)
                    {
                        if (line.LineName == str)
                        {
                            int startPX = 0, endPX = 0;
                            if (line.StartPX.IndexOf('%') > 0)
                            {
                                string strAlign = line.StartPX;
                                string number = strAlign.Substring(0, strAlign.Length - 1);
                                startPX = Convert.ToInt32(float.Parse(number) / 100f * _curPrintConfig.TotalCharNum);
                            }
                            else
                            {
                                startPX = int.Parse(line.StartPX.Trim());
                            }
                            if (line.EndPX.IndexOf('%') > 0)
                            {
                                string strAlign = line.EndPX;
                                string number = strAlign.Substring(0, strAlign.Length - 1);
                                endPX = Convert.ToInt32(float.Parse(number) / 100f * _curPrintConfig.TotalCharNum);
                            }
                            else
                            {
                                endPX = int.Parse(line.EndPX.Trim());
                            }
                            string text = ArrangeLinePosition(line.LineText, startPX, endPX);
                            //打印设置
                            _printHelper.SetFontSize(0, 0);
                            //_printHelper.SetCharSpacing(0);
                            _printHelper.Write(text);
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
                            string itemName = item.ValuePrefix + _printData.GetValue(item.Name);
                            int width = 0;
                            if (item.Width.IndexOf('%') > 0)
                            {
                                string strWidth = item.Width;
                                string number = strWidth.Substring(0, strWidth.Length - 1);
                                width = Convert.ToInt32(float.Parse(number) / 100f * _curPrintConfig.TotalCharNum);
                            }
                            else
                            {
                                width = int.Parse(item.Width.Trim());
                            }
                            TextAlign align;
                            if (item.Align.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
                            {
                                align = TextAlign.Left;
                            }
                            else if (item.Align.Equals("Center", StringComparison.InvariantCultureIgnoreCase))
                            {
                                align = TextAlign.Center;
                            }
                            else if (item.Align.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
                            {
                                align = TextAlign.Right;
                            }
                            else
                            {
                                align = TextAlign.Left;
                            }
                            string[] times = item.FontSize.Split(',');
                            int widthTimes = int.Parse(times[0]);
                            int heightTimes = int.Parse(times[1]);
                            //打印设置
                            _printHelper.SetFontSize(widthTimes, heightTimes);
                            //_printHelper.SetCharSpacing(0);
                            _printHelper.Write(itemName, align, width);
                            break;
                        }
                    }
                }
            }
            _printHelper.PrintAndFeedLines(1);
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
                        //columnHead
                        foreach (List<string> propertyList in propertyArrList)
                        {
                            foreach (string property in propertyList)
                            {
                                foreach (PrintColumnHead columnHead in item.PrintColumnHeads.ColumnHeadList)
                                {
                                    if (property == columnHead.Name)
                                    {
                                        int width = 0;
                                        if (columnHead.Width.IndexOf('%') > 0)
                                        {
                                            string strWidth = columnHead.Width;
                                            string number = strWidth.Substring(0, strWidth.Length - 1);
                                            width = Convert.ToInt32(float.Parse(number) / 100f * _curPrintConfig.TotalCharNum);
                                        }
                                        else
                                        {
                                            width = int.Parse(columnHead.Width.Trim());
                                        }
                                        TextAlign align;
                                        if (columnHead.Align.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            align = TextAlign.Left;
                                        }
                                        else if (columnHead.Align.Equals("Center", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            align = TextAlign.Center;
                                        }
                                        else if (columnHead.Align.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            align = TextAlign.Right;
                                        }
                                        else
                                        {
                                            align = TextAlign.Left;
                                        }
                                        string[] times = columnHead.FontSize.Split(',');
                                        int widthTimes = int.Parse(times[0]);
                                        int heightTimes = int.Parse(times[1]);
                                        //打印设置
                                        _printHelper.SetFontSize(widthTimes, heightTimes);
                                        //_printHelper.SetCharSpacing(0);
                                        _printHelper.Write(columnHead.Text, align, width);
                                        break;
                                    }
                                }
                            }
                        }
                        _printHelper.PrintAndFeedLines(1);
                        //column
                        foreach (GoodsOrder goodsItem in _printData.GoodsOrderList)
                        {
                            foreach (List<string> propertyList in propertyArrList)
                            {
                                foreach (string property in propertyList)
                                {
                                    string itemValue = goodsItem.GetValue(property);
                                    foreach (PrintColumn column in item.PrintColumns.PrintColumnList)
                                    {
                                        if (property == column.Name)
                                        {
                                            int width = 0;
                                            if (column.Width.IndexOf('%') > 0)
                                            {
                                                string strWidth = column.Width;
                                                string number = strWidth.Substring(0, strWidth.Length - 1);
                                                width = Convert.ToInt32(float.Parse(number) / 100f * _curPrintConfig.TotalCharNum);
                                            }
                                            else
                                            {
                                                width = int.Parse(column.Width.Trim());
                                            }
                                            TextAlign align;
                                            if (column.Align.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                align = TextAlign.Left;
                                            }
                                            else if (column.Align.Equals("Center", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                align = TextAlign.Center;
                                            }
                                            else if (column.Align.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                align = TextAlign.Right;
                                            }
                                            else
                                            {
                                                align = TextAlign.Left;
                                            }
                                            string[] times = column.FontSize.Split(',');
                                            int widthTimes = int.Parse(times[0]);
                                            int heightTimes = int.Parse(times[1]);
                                            //打印设置
                                            _printHelper.SetFontSize(widthTimes, heightTimes);
                                            //_printHelper.SetCharSpacing(0);
                                            _printHelper.Write(itemValue, align, width);
                                            break;
                                        }
                                    }
                                }
                                _printHelper.PrintAndFeedLines(1);
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
                        //columnHead
                        foreach (List<string> propertyList in propertyArrList)
                        {
                            foreach (string property in propertyList)
                            {
                                foreach (PrintColumnHead columnHead in item.PrintColumnHeads.ColumnHeadList)
                                {
                                    if (property == columnHead.Name)
                                    {
                                        int width = 0;
                                        if (columnHead.Width.IndexOf('%') > 0)
                                        {
                                            string strWidth = columnHead.Width;
                                            string number = strWidth.Substring(0, strWidth.Length - 1);
                                            width = Convert.ToInt32(float.Parse(number) / 100f * _curPrintConfig.TotalCharNum);
                                        }
                                        else
                                        {
                                            width = int.Parse(columnHead.Width.Trim());
                                        }
                                        TextAlign align;
                                        if (columnHead.Align.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            align = TextAlign.Left;
                                        }
                                        else if (columnHead.Align.Equals("Center", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            align = TextAlign.Center;
                                        }
                                        else if (columnHead.Align.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            align = TextAlign.Right;
                                        }
                                        else
                                        {
                                            align = TextAlign.Left;
                                        }
                                        string[] times = columnHead.FontSize.Split(',');
                                        int widthTimes = int.Parse(times[0]);
                                        int heightTimes = int.Parse(times[1]);
                                        //打印设置
                                        _printHelper.SetFontSize(widthTimes, heightTimes);
                                        //_printHelper.SetCharSpacing(0);
                                        _printHelper.Write(columnHead.Text, align, width);
                                        break;
                                    }
                                }
                            }
                        }
                        _printHelper.PrintAndFeedLines(1);
                        //column
                        foreach (PayingGoodsOrder payingOrder in _printData.PayingOrderList)
                        {
                            foreach (List<string> propertyList in propertyArrList)
                            {
                                foreach (string property in propertyList)
                                {
                                    string itemValue = payingOrder.GetValue(property);
                                    foreach (PrintColumn column in item.PrintColumns.PrintColumnList)
                                    {
                                        if (property == column.Name)
                                        {
                                            int width = 0;
                                            if (column.Width.IndexOf('%') > 0)
                                            {
                                                string strWidth = column.Width;
                                                string number = strWidth.Substring(0, strWidth.Length - 1);
                                                width = Convert.ToInt32(float.Parse(number) / 100f * _curPrintConfig.TotalCharNum);
                                            }
                                            else
                                            {
                                                width = int.Parse(column.Width.Trim());
                                            }
                                            TextAlign align;
                                            if (column.Align.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                align = TextAlign.Left;
                                            }
                                            else if (column.Align.Equals("Center", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                align = TextAlign.Center;
                                            }
                                            else if (column.Align.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                align = TextAlign.Right;
                                            }
                                            else
                                            {
                                                align = TextAlign.Left;
                                            }
                                            string[] times = column.FontSize.Split(',');
                                            int widthTimes = int.Parse(times[0]);
                                            int heightTimes = int.Parse(times[1]);
                                            //打印设置
                                            _printHelper.SetFontSize(widthTimes, heightTimes);
                                            //_printHelper.SetCharSpacing(0);
                                            _printHelper.Write(itemValue, align, width);
                                            break;
                                        }
                                    }
                                }
                                _printHelper.PrintAndFeedLines(1);
                            }
                        }
                        break;
                    }
                }
            }
        }

        private string ArrangeLinePosition(string signChar, int startNum, int endNum)
        {
            StringBuilder result = new StringBuilder();
            if (startNum > 0)
            {
                for (int i = 0; i < startNum; i++)
                {
                    result.Append(" ");
                }
            }
            for (int i = startNum; i <= endNum; i++)
            {
                result.Append(signChar);
            }
            return result.ToString();
        }
    }
}
