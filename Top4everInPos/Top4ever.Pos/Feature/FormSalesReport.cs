using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Enum;
using Top4ever.Print;
using Top4ever.Domain.MembershipCard;
using Top4ever.LocalService;
using Top4ever.LocalService.Entity;
using VechsoftPos.Membership;

namespace VechsoftPos.Feature
{
    public partial class FormSalesReport : Form
    {
        /// <summary>
        /// 1:交班 2:日结
        /// </summary>
        private readonly int m_ModelType;
        private readonly BusinessReport bizReport = null;

        private bool m_HandleSuccess = false;
        public bool HandleSuccess
        {
            get { return m_HandleSuccess; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelType">1:交班 2:日结</param>
        public FormSalesReport(int modelType)
        {
            InitializeComponent();
            this.dgvSalesReport.BackgroundColor = SystemColors.ButtonFace;
            m_ModelType = modelType;
            if (modelType == 1)
            {
                this.lbWeather.Visible = false;
                this.cmbWeather.Visible = false;
                btnSalesReport.Text = "交班";
                this.Text = "交班报表";
                bizReport = BusinessReportService.GetInstance().GetReportDataByHandover(ConstantValuePool.BizSettingConfig.DeviceNo);
            }
            else if (modelType == 2)
            {
                this.cmbWeather.SelectedIndex = 0;
                btnSalesReport.Text = "日结";
                this.Text = "日结报表";
                bizReport = BusinessReportService.GetInstance().GetReportDataByDailyStatement(string.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelType">1:交班 2:日结</param>
        /// <param name="recordId"></param>
        public FormSalesReport(int modelType, object recordId)
        {
            InitializeComponent();
            this.lbWeather.Visible = false;
            this.cmbWeather.Visible = false;
            this.dgvSalesReport.BackgroundColor = SystemColors.ButtonFace;
            m_ModelType = modelType;
            if (modelType == 1)
            {
                btnSalesReport.Text = "交班";
                btnSalesReport.Enabled = false;
                btnSalesReport.BackColor = ConstantValuePool.DisabledColor;
                this.Text = "交班报表";
                Guid handoverRecordId = recordId == null ? Guid.Empty : (Guid)recordId;
                bizReport = BusinessReportService.GetInstance().GetReportDataByHandoverRecordId(handoverRecordId);
            }
            else if (modelType == 2)
            {
                btnSalesReport.Text = "日结";
                btnSalesReport.Enabled = false;
                btnSalesReport.BackColor = ConstantValuePool.DisabledColor;
                this.Text = "日结报表";
                string dailyStatementNo = recordId.ToString();
                bizReport = BusinessReportService.GetInstance().GetReportDataByDailyStatement(dailyStatementNo);
            }
        }

        private void FormSalesReport_Load(object sender, EventArgs e)
        {
            if (bizReport != null && bizReport.BillTotalQty > 0)
            {
                if (m_ModelType == 1)
                {
                    InsertNewDataGridViewItem("---交班报表---");
                }
                if (m_ModelType == 2)
                {
                    InsertNewDataGridViewItem("---日结报表---");
                }
                string str = string.Empty;
                InsertNewDataGridViewItem(str);
                str = GetDataType2("店铺名称：", ConstantValuePool.CurrentShop.ShopName);
                InsertNewDataGridViewItem(str);
                str = GetDataType2("店铺编号：", ConstantValuePool.CurrentShop.ShopNo);
                InsertNewDataGridViewItem(str);
                str = GetDataType2("营业日：", DateTime.Now.ToString("yyyy-MM-dd"));
                InsertNewDataGridViewItem(str);
                str = GetDataType2("员工号：", ConstantValuePool.CurrentEmployee.EmployeeNo);
                InsertNewDataGridViewItem(str);
                if (m_ModelType == 1)
                {
                    int workSequence = 1;
                    if (bizReport.WorkSequence > 0)
                    {
                        workSequence = bizReport.WorkSequence;
                    }
                    str = GetDataType2("班次号：", workSequence.ToString());
                    InsertNewDataGridViewItem(str);
                    str = GetDataType2("设备号：", ConstantValuePool.BizSettingConfig.DeviceNo);
                    InsertNewDataGridViewItem(str);
                }

                str = string.Empty;
                InsertNewDataGridViewItem(str);
                str = "营业收入统计";
                InsertNewDataGridViewItem(str);
                str = GetDataType2Ex("营业总额：", bizReport.TotalRevenue.ToString("f2"));
                InsertNewDataGridViewItem(str);
                str = GetDataType2Ex("- 去尾折扣：", (-bizReport.CutOffTotalPrice).ToString("f2"));
                InsertNewDataGridViewItem(str);
                str = GetDataType2Ex("- 折扣金额：", bizReport.DiscountTotalPrice.ToString("f2"));
                InsertNewDataGridViewItem(str);
                str = GetDataType2Ex("= 营收净额：", (bizReport.ActualTotalIncome + bizReport.TotalServiceFee).ToString("f2"));
                InsertNewDataGridViewItem(str);
                if (bizReport.TotalServiceFee > 0)
                {
                    str = GetDataType2Ex("(包含服务费：", bizReport.TotalServiceFee.ToString("f2") + ")");
                    InsertNewDataGridViewItem(str);
                }

                str = string.Empty;
                InsertNewDataGridViewItem(str);
                InsertNewDataGridViewItem("现金核数");
                decimal totalSellPrice = 0;
                foreach (OrderPayoffSum item in bizReport.orderPayoffSumList)
                {
                    totalSellPrice += item.PayoffMoney;
                }
                str = GetDataType3("营收净额：", string.Empty, (bizReport.ActualTotalIncome + bizReport.TotalServiceFee).ToString("f2"));
                InsertNewDataGridViewItem(str);
                foreach (OrderPayoffSum item in bizReport.orderPayoffSumList)
                {
                    str = GetDataType3(item.PayoffName, item.Times.ToString(), item.PayoffMoney.ToString("f2"));
                    InsertNewDataGridViewItem(str);
                }
                decimal moreOrLess = totalSellPrice - (bizReport.ActualTotalIncome + bizReport.TotalServiceFee);
                str = GetDataType3("金额过多(+)/不足(-)：", string.Empty, moreOrLess.ToString("f2"));
                if (moreOrLess < 0)
                {
                    InsertNewDataGridViewItem(str, Color.Red);
                }
                else if (moreOrLess > 0)
                {
                    InsertNewDataGridViewItem(str, Color.Green);
                }
                else
                {
                    InsertNewDataGridViewItem(str);
                }

                str = string.Empty;
                InsertNewDataGridViewItem(str);
                InsertNewDataGridViewItem("折扣详细资料");
                decimal totalDiscountPrice = 0;
                foreach (OrderDiscountSum item in bizReport.orderDiscountSumList)
                {
                    totalDiscountPrice += item.DiscountMoney;
                    str = GetDataType2Ex(item.DiscountName, item.DiscountMoney.ToString("f2"));
                    InsertNewDataGridViewItem(str);
                }
                str = GetDataType2Ex("= 总数", totalDiscountPrice.ToString("f2"));
                InsertNewDataGridViewItem(str);

                str = string.Empty;
                InsertNewDataGridViewItem(str);
                str = GetDataType3("核数", "数量", "金额");
                InsertNewDataGridViewItem(str);
                str = GetDataType3("账单数目", bizReport.BillTotalQty.ToString(), totalSellPrice.ToString("f2"));
                InsertNewDataGridViewItem(str);
                str = GetDataType3("账单平均", string.Empty, (totalSellPrice / bizReport.BillTotalQty).ToString("f2"));
                InsertNewDataGridViewItem(str);
                str = GetDataType3("人数/平均", bizReport.PeopleTotalNum.ToString(), (totalSellPrice / bizReport.PeopleTotalNum).ToString("f2"));
                InsertNewDataGridViewItem(str);

                if (m_ModelType == 2)
                {
                    str = string.Empty;
                    InsertNewDataGridViewItem(str);
                    InsertNewDataGridViewItem("会员卡预存金额");
                    str = GetDataType4Ext("时间", "卡号", "金额", "支付方式");
                    InsertNewDataGridViewItem(str);
                    foreach (VIPCardTrade item in bizReport.cardStoredValueList)
                    {
                        str = GetDataType4Ext(item.TradeTime.ToString("MM-dd HH:mm"), item.CardNo, item.TradeAmount.ToString("f2"), item.PayoffName);
                        InsertNewDataGridViewItem(str);
                    }
                    str = string.Empty;
                    InsertNewDataGridViewItem(str);
                    InsertNewDataGridViewItem("支付方式合计（包含会员卡预存金额）");
                    Dictionary<string, decimal> dicPayoffWay = new Dictionary<string, decimal>();
                    foreach (OrderPayoffSum item in bizReport.orderPayoffSumList)
                    {
                        if (dicPayoffWay.ContainsKey(item.PayoffName))
                        {
                            dicPayoffWay[item.PayoffName] += item.PayoffMoney;
                        }
                        else
                        {
                            dicPayoffWay.Add(item.PayoffName, item.PayoffMoney);
                        }
                    }
                    foreach (VIPCardTrade item in bizReport.cardStoredValueList)
                    {
                        if (dicPayoffWay.ContainsKey(item.PayoffName))
                        {
                            dicPayoffWay[item.PayoffName] += item.TradeAmount;
                        }
                        else
                        {
                            dicPayoffWay.Add(item.PayoffName, item.TradeAmount);
                        }
                    }
                    foreach (KeyValuePair<string, decimal> item in dicPayoffWay)
                    {
                        str = GetDataType2Ex(item.Key, item.Value.ToString("f2"));
                        InsertNewDataGridViewItem(str);
                    }
                }

                str = string.Empty;
                InsertNewDataGridViewItem(str);
                InsertNewDataGridViewItem("部门营业报表");
                decimal totalDepartPriceSum = 0;
                foreach (SalesPriceByDepart item in bizReport.salesPriceByDepartList)
                {
                    totalDepartPriceSum += item.TotalDepartPrice;
                }
                foreach (SalesPriceByDepart item in bizReport.salesPriceByDepartList)
                {
                    decimal percent = item.TotalDepartPrice / totalDepartPriceSum * 100;
                    str = GetDataType3("  " + item.DepartName, item.TotalDepartPrice.ToString("f2"), percent.ToString("f2") + "%");
                    InsertNewDataGridViewItem(str);
                }
                str = GetDataType3("部门合计：", totalDepartPriceSum.ToString("f2"), "100.00%");
                InsertNewDataGridViewItem(str);

                str = string.Empty;
                InsertNewDataGridViewItem(str);
                str = "销售报表";
                InsertNewDataGridViewItem(str);
                str = GetDataType4("  品名", "数量", "金额", "百分比");
                InsertNewDataGridViewItem(str);
                foreach (SalesPriceByDepart item in bizReport.salesPriceByDepartList)
                {
                    str = "[" + item.DepartName + "]";
                    InsertNewDataGridViewItem(str);
                    foreach (ItemsPrice itemsPrice in item.ItemsPriceList)
                    {
                        decimal itemPercent = itemsPrice.ItemsTotalPrice / item.TotalDepartPrice * 100;
                        str = GetDataType4("  " + itemsPrice.ItemsName, itemsPrice.ItemsTotalQty.ToString(), itemsPrice.ItemsTotalPrice.ToString("f2"), itemPercent.ToString("f2") + "%");
                        InsertNewDataGridViewItem(str);
                    }
                    decimal percent = item.TotalDepartPrice / totalDepartPriceSum * 100;
                    str = GetDataType4("  合计", item.TotalItemsNum.ToString(), item.TotalDepartPrice.ToString("f2"), percent.ToString("f2") + "%");
                    InsertNewDataGridViewItem(str);
                    str = string.Empty;
                    InsertNewDataGridViewItem(str);
                }

                if (m_ModelType == 1)
                {
                    string lastHandoverTime = string.Empty;
                    if (bizReport.LastHandoverTime != null)
                    {
                        lastHandoverTime = ((DateTime)bizReport.LastHandoverTime).ToString("yyyy-MM-dd HH:mm");
                    }
                    str = GetDataType2("上次交班时间：", lastHandoverTime);
                    InsertNewDataGridViewItem(str);
                    str = GetDataType2("本次交班时间：", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    InsertNewDataGridViewItem(str);
                }
                else if (m_ModelType == 2)
                {
                    string lastDailyStatementTime = string.Empty;
                    if (bizReport.LastDailyStatementTime != null)
                    {
                        lastDailyStatementTime = ((DateTime)bizReport.LastDailyStatementTime).ToString("yyyy-MM-dd HH:mm");
                    }
                    str = GetDataType2("上次日结时间：", lastDailyStatementTime);
                    InsertNewDataGridViewItem(str);
                    str = GetDataType2("本次日结时间：", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    InsertNewDataGridViewItem(str);
                }

                str = string.Empty;
                InsertNewDataGridViewItem(str);
                str = "---报表完结---";
                InsertNewDataGridViewItem(str);
            }
        }

        private string GetDataType2(string strText, string strValue)
        {
            string strNull = string.Empty;
            int len1st = CheckTextLength(strText);
            for (int i = 0; i < (22 - len1st); i++)
                strNull += " ";
            return strText + strNull + strValue;
        }

        private string GetDataType2Ex(string strText, string strValue)
        {
            string strNull = string.Empty;
            int len1st = CheckTextLength(strText);
            for (int i = 0; i < (38 - len1st); i++)
                strNull += " ";
            return strText + strNull + strValue;
        }

        private string GetDataType3(string strText, string strValue1st, string strValue2nd)
        {
            string result = string.Empty;

            string strNull = string.Empty;
            int len1st = CheckTextLength(strText);
            for (int i = 0; i < (22 - len1st); i++)
                strNull += " ";
            result += strText + strNull;

            strNull = string.Empty;
            int len2nd = CheckTextLength(strValue1st);
            for (int i = 0; i < (16 - len2nd); i++)
                strNull += " ";
            result += strValue1st + strNull + strValue2nd;

            return result;
        }

        private string GetDataType4(string strText, string strValue1st, string strValue2nd, string strValue3rd)
        {
            string result = string.Empty;

            string strNull = string.Empty;
            int len1st = CheckTextLength(strText);
            for (int i = 0; i < (22 - len1st); i++)
                strNull += " ";
            result += strText + strNull;

            strNull = string.Empty;
            int len2nd = CheckTextLength(strValue1st);
            for (int i = 0; i < (8 - len2nd); i++)
                strNull += " ";
            result += strValue1st + strNull;

            strNull = string.Empty;
            int len3rd = CheckTextLength(strValue2nd);
            for (int i = 0; i < (8 - len3rd); i++)
                strNull += " ";
            result += strValue2nd + strNull + strValue3rd;

            return result;
        }

        private string GetDataType4Ext(string strText, string strValue1st, string strValue2nd, string strValue3rd)
        {
            string result = string.Empty;

            string strNull = string.Empty;
            int len1st = CheckTextLength(strText);
            for (int i = 0; i < (14 - len1st); i++)
                strNull += " ";
            result += strText + strNull;

            strNull = string.Empty;
            int len2nd = CheckTextLength(strValue1st);
            for (int i = 0; i < (14 - len2nd); i++)
                strNull += " ";
            result += strValue1st + strNull;

            strNull = string.Empty;
            int len3rd = CheckTextLength(strValue2nd);
            for (int i = 0; i < (10 - len3rd); i++)
                strNull += " ";
            result += strValue2nd + strNull + strValue3rd;

            return result;
        }

        private void InsertNewDataGridViewItem(string strText)
        {
            int index = this.dgvSalesReport.Rows.Add();
            this.dgvSalesReport.Rows[index].Cells[0].Value = strText;
        }

        private void InsertNewDataGridViewItem(string strText, Color foreColor)
        {
            int index = this.dgvSalesReport.Rows.Add();
            this.dgvSalesReport.Rows[index].Cells[0].Value = strText;
            this.dgvSalesReport.Rows[index].Cells[0].Style.ForeColor = foreColor;
        }

        private int CheckTextLength(string strText)
        {
            int len = 0;
            for (int i = 0; i < strText.Length; i++)
            {
                byte[] byte_len = Encoding.Default.GetBytes(strText.Substring(i, 1));
                if (byte_len.Length > 1)
                    len += 2; //如果长度大于1，是中文，占两个字节，+2
                else
                    len += 1;  //如果长度等于1，是英文，占一个字节，+1
            }
            return len;
        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            if (bizReport != null && bizReport.BillTotalQty > 0)
            {
                if (m_ModelType == 1)
                {
                    //判断是否存在退款失败的账单
                    CardRefundPayService refundPayService = new CardRefundPayService();
                    List<CardRefundPay> cardRefundPayList = refundPayService.GetCardRefundPayList();
                    if (cardRefundPayList != null && cardRefundPayList.Count > 0)
                    {
                        FormVIPCardRefundPay refundPayForm = new FormVIPCardRefundPay(cardRefundPayList);
                        refundPayForm.ShowDialog();
                    }
                    Guid handoverRecordId = Guid.NewGuid();
                    HandoverRecord handoverRecord = new HandoverRecord();
                    handoverRecord.HandoverRecordID = handoverRecordId;
                    handoverRecord.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                    handoverRecord.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                    IList<HandoverTurnover> handoverTurnoverList = new List<HandoverTurnover>();
                    foreach (OrderPayoffSum item in bizReport.orderPayoffSumList)
                    {
                        HandoverTurnover handoverTurnover = new HandoverTurnover();
                        handoverTurnover.HandoverRecordID = handoverRecordId;
                        handoverTurnover.PayoffID = item.PayoffID;
                        handoverTurnover.SalesTurnover = item.PayoffMoney;
                        handoverTurnoverList.Add(handoverTurnover);
                    }

                    HandoverInfo handover = new HandoverInfo();
                    handover.handoverRecord = handoverRecord;
                    handover.handoverTurnoverList = handoverTurnoverList;
                    bool result = HandoverService.GetInstance().CreateHandover(handover);
                    if (result)
                    {
                        MessageBox.Show("交班成功！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        m_HandleSuccess = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("出现异常错误，请重新交班！");
                    }
                }
                else if (m_ModelType == 2)
                {
                    //判断是否存在退款失败的账单
                    CardRefundPayService refundPayService = new CardRefundPayService();
                    List<CardRefundPay> cardRefundPayList = refundPayService.GetCardRefundPayList();
                    if (cardRefundPayList != null && cardRefundPayList.Count > 0)
                    {
                        FormVIPCardRefundPay refundPayForm = new FormVIPCardRefundPay(cardRefundPayList);
                        refundPayForm.ShowDialog();
                    }
                    FormChooseDate form = new FormChooseDate(bizReport.LastDailyStatementTime);
                    form.ShowDialog();
                    if (form.DailyStatementDate != null)
                    {
                        string weather = this.cmbWeather.Text;
                        if (string.IsNullOrEmpty(weather))
                        {
                            MessageBox.Show("请先选择当天天气情况！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        DateTime belongToDate = (DateTime)form.DailyStatementDate;
                        DailyStatement dailyStatement = new DailyStatement();
                        dailyStatement.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                        dailyStatement.BelongToDate = belongToDate;
                        dailyStatement.Weather = weather;
                        dailyStatement.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                        DailyTurnover dailyTurnover = new DailyTurnover();
                        dailyTurnover.TotalRevenue = bizReport.TotalRevenue;
                        dailyTurnover.CutOffTotalPrice = bizReport.CutOffTotalPrice;
                        dailyTurnover.DiscountTotalPrice = bizReport.DiscountTotalPrice;
                        dailyTurnover.ActualTotalIncome = bizReport.ActualTotalIncome;
                        dailyTurnover.TotalServiceFee = bizReport.TotalServiceFee;
                        dailyTurnover.StoredTotalPrice = 0;

                        DailyBalance dailyBalance = new DailyBalance();
                        dailyBalance.dailyStatement = dailyStatement;
                        dailyBalance.dailyTurnover = dailyTurnover;
                        string unCheckDeviceNo;  //未结账的设备号
                        int result = DailyBalanceService.GetInstance().CreateDailyBalance(dailyBalance, out unCheckDeviceNo);
                        if (result == 1)
                        {
                            MessageBox.Show("日结成功！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            m_HandleSuccess = true; 
                            this.Close();
                        }
                        else if (result == 2)
                        {
                            MessageBox.Show("存在未结账单据，请先结完账！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (result == 3)
                        {
                            if (string.IsNullOrEmpty(unCheckDeviceNo))
                            {
                                MessageBox.Show("存在未交班的POS，请先交班！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                if (unCheckDeviceNo.IndexOf(',') == -1 && unCheckDeviceNo.IndexOf(ConstantValuePool.BizSettingConfig.DeviceNo) >= 0)
                                {
                                    MessageBox.Show("当前设备未交班，请交班后进行日结！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    MessageBox.Show(string.Format("设备【{0}】未交班，请先交班！",unCheckDeviceNo), "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("出现异常错误，请重新日结！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvSalesReport.Rows.Count > 0)
            {
                List<String> printData = new List<String>();
                foreach (DataGridViewRow dgr in dgvSalesReport.Rows)
                {
                    if (dgr.Cells[0].Value != null && !string.IsNullOrEmpty(dgr.Cells[0].Value.ToString().Trim()))
                    {
                        printData.Add(dgr.Cells[0].Value.ToString());
                    }
                    else
                    {
                        printData.Add("  ");
                    }
                }
                if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.DRIVER)
                {
                    string printerName = ConstantValuePool.BizSettingConfig.printConfig.Name;
                    string paperName = ConstantValuePool.BizSettingConfig.printConfig.PaperName;
                    DriverSinglePrint driverPrint = new DriverSinglePrint(printerName, paperName);
                    driverPrint.DoPrint(printData);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
