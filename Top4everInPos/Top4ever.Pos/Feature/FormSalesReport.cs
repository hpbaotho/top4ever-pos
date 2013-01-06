using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;

namespace Top4ever.Pos.Feature
{
    public partial class FormSalesReport : Form
    {
        public FormSalesReport()
        {
            InitializeComponent();
        }

        private void FormSalesReport_Load(object sender, EventArgs e)
        {
            this.lbShopName.Text = ConstantValuePool.CurrentShop.ShopName;
            this.lbShopNo.Text = ConstantValuePool.CurrentShop.ShopNo;
            this.lbBizDay.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.lbEmployeeNo.Text = ConstantValuePool.CurrentEmployee.EmployeeNo;

            BusinessReportService bizReportService = new BusinessReportService();
            BusinessReport bizReport = bizReportService.GetReportDataByDailyStatement();

            int index = this.dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells[0].Value = "营业总额：";
            dataGridView1.Rows[index].Cells[1].Value = bizReport.TotalRevenue.ToString("f2");
            index = this.dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells[0].Value = "去尾折扣：";
            dataGridView1.Rows[index].Cells[1].Value = bizReport.CutOffTotalPrice.ToString("f2");
            index = this.dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells[0].Value = "折扣金额：";
            dataGridView1.Rows[index].Cells[1].Value = bizReport.DiscountTotalPrice.ToString("f2");
            index = this.dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells[0].Value = "营收净额：";
            dataGridView1.Rows[index].Cells[1].Value = (bizReport.ActualTotalIncome + bizReport.TotalServiceFee).ToString("f2");
            int itemNum = 4;
            if (bizReport.TotalServiceFee > 0)
            {
                itemNum++;
                index = this.dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = "(包含服务费：";
                dataGridView1.Rows[index].Cells[1].Value = bizReport.TotalServiceFee.ToString("f2") + ")";
            }
            this.dataGridView1.Height = this.dataGridView1.RowTemplate.Height * itemNum + 1;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.ClearSelection();
            this.dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;
            //现金核数
            int space = 12;
            int py = 141 + this.dataGridView1.Height + space;
            int px = lbMoneyCheck.Location.X;
            lbMoneyCheck.Location = new Point(px, py);
            py += lbMoneyCheck.Height + 1;
            dataGridView2.Location = new Point(0, py);
            decimal totalSellPrice = 0;
            foreach (OrderPayoffSum item in bizReport.orderPayoffSumList)
            {
                totalSellPrice += item.PayoffMoney;
            }
            decimal moreOrLess = bizReport.ActualTotalIncome + bizReport.TotalServiceFee - totalSellPrice;
            foreach (OrderPayoffSum item in bizReport.orderPayoffSumList)
            { 
                index = this.dataGridView2.Rows.Add();
                dataGridView2.Rows[index].Cells[0].Value = item.PayoffName;
                dataGridView2.Rows[index].Cells[1].Value = item.Times;
                dataGridView2.Rows[index].Cells[2].Value = item.PayoffMoney.ToString("f2");
            }
            index = this.dataGridView2.Rows.Add();
            dataGridView2.Rows[index].Cells[0].Value = "营业净额：";
            dataGridView2.Rows[index].Cells[2].Value = (bizReport.ActualTotalIncome + bizReport.TotalServiceFee).ToString("f2");
            index = this.dataGridView2.Rows.Add();
            dataGridView2.Rows[index].Cells[0].Value = "金额过多(-)/不足(+)";
            dataGridView2.Rows[index].Cells[2].Value = moreOrLess.ToString("f2");
            itemNum = bizReport.orderPayoffSumList.Count + 2;
            this.dataGridView2.Height = this.dataGridView2.ColumnHeadersHeight + this.dataGridView2.RowTemplate.Height * itemNum;
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.ClearSelection();
            py += this.dataGridView2.Height + space;
            px = lbDiscountDetail.Location.X;
            lbDiscountDetail.Location = new Point(px, py);
            py += lbDiscountDetail.Height + 1;
            dataGridView3.Location = new Point(0, py);
            //折扣详细资料
            decimal totalDiscountPrice = 0;
            foreach (OrderDiscountSum item in bizReport.orderDiscountSumList)
            {
                totalDiscountPrice += item.DiscountMoney;
                index = this.dataGridView3.Rows.Add();
                dataGridView3.Rows[index].Cells[0].Value = item.DiscountName;
                dataGridView3.Rows[index].Cells[1].Value = item.DiscountMoney.ToString("f2");
            }
            index = this.dataGridView3.Rows.Add();
            dataGridView3.Rows[index].Cells[0].Value = "折扣总金额";
            dataGridView3.Rows[index].Cells[1].Value = totalDiscountPrice.ToString("f2");
            itemNum = bizReport.orderDiscountSumList.Count + 1;
            this.dataGridView3.Height = this.dataGridView3.ColumnHeadersHeight + this.dataGridView3.RowTemplate.Height * itemNum;
            this.dataGridView3.ReadOnly = true;
            this.dataGridView3.ClearSelection();
            py += this.dataGridView3.Height + space;
        }
    }
}