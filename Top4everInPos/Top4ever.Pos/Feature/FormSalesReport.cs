using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;

namespace Top4ever.Pos.Feature
{
    public partial class FormSalesReport : Form
    {
        /// <summary>
        /// 1:交班 2:日结
        /// </summary>
        private int m_ModelType;
        private BusinessReport bizReport = null;

        private bool m_HandleSuccess = false;
        public bool HandleSuccess
        {
            get { return m_HandleSuccess; }
        }

        public FormSalesReport(int modelType)
        {
            InitializeComponent();
            this.dgvSalesReport.BackgroundColor = SystemColors.ButtonFace;
            m_ModelType = modelType;
            if (modelType == 1)
            {
                this.lbWeather.Visible = false;
                this.comboBox1.Visible = false;
                btnSalesReport.Text = "交班";
                this.Text = "交班报表";
            }
            else if (modelType == 2)
            {
                btnSalesReport.Text = "日结";
                this.Text = "日结报表";
            }
        }

        private void FormSalesReport_Load(object sender, EventArgs e)
        {
            BusinessReportService bizReportService = new BusinessReportService();
            if (m_ModelType == 1)
            {
                bizReport = bizReportService.GetReportDataByHandover(ConstantValuePool.BizSettingConfig.DeviceNo);
            }
            else if (m_ModelType == 2)
            {
                bizReport = bizReportService.GetReportDataByDailyStatement();
            }
            if (bizReport != null)
            {
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
                str = "现金核数";
                InsertNewDataGridViewItem(str);
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
                decimal moreOrLess = bizReport.ActualTotalIncome + bizReport.TotalServiceFee - totalSellPrice;
                str = GetDataType3("金额过多(-)/不足(+)：", string.Empty, moreOrLess.ToString("f2"));
                InsertNewDataGridViewItem(str);

                str = string.Empty;
                InsertNewDataGridViewItem(str);
                str = "折扣详细资料";
                InsertNewDataGridViewItem(str);
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

                str = string.Empty;
                InsertNewDataGridViewItem(str);
                str = "部门营业报表";
                InsertNewDataGridViewItem(str);
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

        private void InsertNewDataGridViewItem(string strText)
        {
            int index = this.dgvSalesReport.Rows.Add();
            this.dgvSalesReport.Rows[index].Cells[0].Value = strText;
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
            if (bizReport != null)
            {
                if (m_ModelType == 1)
                {
                    Guid handoverRecordID = Guid.NewGuid();
                    HandoverRecord handoverRecord = new HandoverRecord();
                    handoverRecord.HandoverRecordID = handoverRecordID;
                    handoverRecord.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
                    handoverRecord.EmployeeID = ConstantValuePool.CurrentEmployee.EmployeeID;
                    IList<HandoverTurnover> handoverTurnoverList = new List<HandoverTurnover>();
                    foreach (OrderPayoffSum item in bizReport.orderPayoffSumList)
                    {
                        HandoverTurnover handoverTurnover = new HandoverTurnover();
                        handoverTurnover.HandoverRecordID = handoverRecordID;
                        handoverTurnover.PayoffID = item.PayoffID;
                        handoverTurnover.SalesTurnover = item.PayoffMoney;
                        handoverTurnoverList.Add(handoverTurnover);
                    }

                    HandoverInfo handover = new HandoverInfo();
                    handover.handoverRecord = handoverRecord;
                    handover.handoverTurnoverList = handoverTurnoverList;
                    HandoverService handoverService = new HandoverService();
                    bool result = handoverService.CreateHandover(handover);
                    if (result)
                    {
                        MessageBox.Show("交班成功！");
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
                    FormChooseDate form = new FormChooseDate(bizReport.LastDailyStatementTime);
                    form.ShowDialog();
                    if (form.DailyStatementDate != null)
                    {
                        string weather = this.comboBox1.Text;
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
                        DailyBalanceService dailyBalanceService = new DailyBalanceService();
                        int result = dailyBalanceService.CreateDailyBalance(dailyBalance);
                        if (result == 1)
                        {
                            MessageBox.Show("日结成功！");
                            m_HandleSuccess = true;
                        }
                        else if (result == 2)
                        {
                            MessageBox.Show("存在未结账单据，请先结完账！");
                        }
                        else if (result == 3)
                        {
                            MessageBox.Show("存在未交班的POS，请先交班！");
                        }
                        else
                        {
                            MessageBox.Show("出现异常错误，请重新日结！");
                        }
                    }
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
