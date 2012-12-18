using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
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
        private static Dictionary<string, InstructionOrderPrintSetting> dicPrintSetting = new Dictionary<string, InstructionOrderPrintSetting>();
        private String m_PaperType;
        private PrintHelper m_PrintHelper;
        private PrintData printData = null;
        private String printLayoutPath = null;
        private PrintConfig curPrintConfig = null;

        public InstructionOrderPrint(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, string paperType)
        {
            m_PaperType = paperType;
            m_PrintHelper = new PrintHelper(portName, baudRate, parity, dataBits, stopBits);
        }

        public InstructionOrderPrint(string usbVID, string usbPID, string paperType)
        {
            m_PaperType = paperType;
            m_PrintHelper = new PrintHelper(usbVID, usbPID);
        }

        public InstructionOrderPrint(string ip, int port, string paperType)
        {
            m_PaperType = paperType;
            m_PrintHelper = new PrintHelper(ip, port);
        }

        public void DoPrint(PrintData printData, string printLayoutPath, string printConfigPath)
        {
            if (printData != null && File.Exists(printLayoutPath) && File.Exists(printConfigPath))
            {
                this.printData = printData;
                this.printLayoutPath = printLayoutPath;
                InstructionOrderPrintSetting printSetting = null;
                if (dicPrintSetting.ContainsKey(printConfigPath))
                {
                    printSetting = dicPrintSetting[printConfigPath];
                }
                else
                {
                    printSetting = XmlUtil.Deserialize<InstructionOrderPrintSetting>(printConfigPath);
                    dicPrintSetting.Add(printConfigPath, printSetting);
                }
                foreach (PrintConfig printConfig in printSetting.PrintConfigs)
                {
                    if (printConfig.PaperType == m_PaperType)
                    {
                        curPrintConfig = printConfig;
                        break;
                    }
                }
                m_PrintHelper.Open();
                m_PrintHelper.SetPrinterInit();
                DrawPaper();
                m_PrintHelper.Close();
            }
        }

        private void DrawPaper()
        {
            string[] repeatedConfigArr = null;
            int braceCount = -1;    //{}个数计数
            bool inCirculation = false; //是否在循环内部
            using (StreamReader stream = new StreamReader(printLayoutPath))
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
                    foreach (PrintLine line in curPrintConfig.PrintLines.LineList)
                    {
                        if (line.LineName == str)
                        {
                            int startPX = 0, endPX = 0;
                            if (line.StartPX.IndexOf('%') > 0)
                            {
                                string strAlign = line.StartPX;
                                string number = strAlign.Substring(0, strAlign.Length - 1);
                                startPX = Convert.ToInt32(float.Parse(number) / 100f * curPrintConfig.TotalCharNum);
                            }
                            else
                            {
                                startPX = int.Parse(line.StartPX.Trim());
                            }
                            if (line.EndPX.IndexOf('%') > 0)
                            {
                                string strAlign = line.EndPX;
                                string number = strAlign.Substring(0, strAlign.Length - 1);
                                endPX = Convert.ToInt32(float.Parse(number) / 100f * curPrintConfig.TotalCharNum);
                            }
                            else
                            {
                                endPX = int.Parse(line.EndPX.Trim());
                            }
                            string text = ArrangeLinePosition(line.LineText, startPX, endPX);
                            //打印设置
                            m_PrintHelper.SetFontSize(0, 0);
                            m_PrintHelper.SetCharSpacing(0);
                            m_PrintHelper.Write(text);
                            break;
                        }
                    }
                }
                else
                {
                    foreach (NormalConfig item in curPrintConfig.NormalConfigs.NormalConfigList)
                    {
                        if (item.Name == str)
                        {
                            string itemName = item.ValuePrefix + GetPropertyValue(printData, item.Name).ToString();
                            int width = 0;
                            if (item.Width.IndexOf('%') > 0)
                            {
                                string strWidth = item.Width;
                                string number = strWidth.Substring(0, strWidth.Length - 1);
                                width = Convert.ToInt32(float.Parse(number) / 100f * curPrintConfig.TotalCharNum);
                            }
                            else
                            {
                                width = int.Parse(item.Width.Trim());
                            }
                            TextAlign align;
                            if (item.Align == "Left")
                            {
                                align = TextAlign.Left;
                            }
                            else if (item.Align == "Center")
                            {
                                align = TextAlign.Center;
                            }
                            else if (item.Align == "Right")
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
                            m_PrintHelper.SetFontSize(widthTimes, heightTimes);
                            m_PrintHelper.SetCharSpacing(0);
                            m_PrintHelper.Write(itemName, align, width);
                            break;
                        }
                    }
                }
            }
            m_PrintHelper.PrintAndFeedLines(1);
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
            if (className == "GoodsOrder")
            {
                foreach (DataListConfig item in curPrintConfig.DataListConfigs.DataListConfigList)
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
                                            width = Convert.ToInt32(float.Parse(number) / 100f * curPrintConfig.TotalCharNum);
                                        }
                                        else
                                        {
                                            width = int.Parse(columnHead.Width.Trim());
                                        }
                                        TextAlign align;
                                        if (columnHead.Align == "Left")
                                        {
                                            align = TextAlign.Left;
                                        }
                                        else if (columnHead.Align == "Center")
                                        {
                                            align = TextAlign.Center;
                                        }
                                        else if (columnHead.Align == "Right")
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
                                        m_PrintHelper.SetFontSize(widthTimes, heightTimes);
                                        m_PrintHelper.SetCharSpacing(0);
                                        m_PrintHelper.Write(columnHead.Text, align, width);
                                        break;
                                    }
                                }
                            }
                        }
                        m_PrintHelper.PrintAndFeedLines(1);
                        //column
                        foreach (GoodsOrder goodsItem in printData.GoodsOrderList)
                        {
                            foreach (List<string> propertyList in propertyArrList)
                            {
                                foreach (string property in propertyList)
                                {
                                    string itemValue = GetPropertyValue(goodsItem, property).ToString();
                                    foreach (PrintColumn column in item.PrintColumns.PrintColumnList)
                                    {
                                        if (property == column.Name)
                                        {
                                            int width = 0;
                                            if (column.Width.IndexOf('%') > 0)
                                            {
                                                string strWidth = column.Width;
                                                string number = strWidth.Substring(0, strWidth.Length - 1);
                                                width = Convert.ToInt32(float.Parse(number) / 100f * curPrintConfig.TotalCharNum);
                                            }
                                            else
                                            {
                                                width = int.Parse(column.Width.Trim());
                                            }
                                            TextAlign align;
                                            if (column.Align == "Left")
                                            {
                                                align = TextAlign.Left;
                                            }
                                            else if (column.Align == "Center")
                                            {
                                                align = TextAlign.Center;
                                            }
                                            else if (column.Align == "Right")
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
                                            m_PrintHelper.SetFontSize(widthTimes, heightTimes);
                                            m_PrintHelper.SetCharSpacing(0);
                                            m_PrintHelper.Write(itemValue, align, width);
                                            break;
                                        }
                                    }
                                }
                                m_PrintHelper.PrintAndFeedLines(1);
                            }
                        }
                        break;
                    }
                }
            }
        }

        private object GetPropertyValue(object obj, string fieldName)
        {
            Type type = obj.GetType();
            return type.GetProperty(fieldName).GetValue(obj, null);
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
