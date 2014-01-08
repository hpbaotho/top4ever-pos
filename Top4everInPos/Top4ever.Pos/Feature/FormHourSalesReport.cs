using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Top4ever.ClientService;
using Top4ever.Domain.Transfer;

namespace Top4ever.Pos.Feature
{
    public partial class FormHourSalesReport : Form
    {
        public FormHourSalesReport()
        {
            InitializeComponent();
        }

        private void FormHourSalesReport_Load(object sender, EventArgs e)
        {
            OrderService orderService = new OrderService();
            IList<HourOrderSales> hourSalesList = orderService.GetHourSalesReport(DateTime.MinValue, DateTime.MinValue);
            BindSalesReportData(hourSalesList);
        }

        private void BindSalesReportData(IList<HourOrderSales> hourSalesList)
        {
            dataGirdViewExt1.Rows.Clear();
            if (hourSalesList != null && hourSalesList.Count > 0)
            {
                decimal totalAmount = 0M;
                int totalQty = 0;
                foreach (HourOrderSales item in hourSalesList)
                {
                    totalAmount += item.OrderPrice;
                    totalQty += item.OrderCount;
                }
                decimal pricePercent = 0M;
                foreach (HourOrderSales item in hourSalesList)
                {
                    int index = dataGirdViewExt1.Rows.Add();
                    dataGirdViewExt1.Rows[index].Cells["colOrderDate"].Value = item.OrderDate;
                    dataGirdViewExt1.Rows[index].Cells["colOrderHour"].Value = item.OrderHour;
                    dataGirdViewExt1.Rows[index].Cells["colOrderCount"].Value = item.OrderCount;
                    dataGirdViewExt1.Rows[index].Cells["colPeopleNum"].Value = item.PeopleNum;
                    dataGirdViewExt1.Rows[index].Cells["colOrderPrice"].Value = item.OrderPrice.ToString("f2");
                    string tempPercent = (item.OrderPrice * 100 / totalAmount).ToString("f1");
                    pricePercent += decimal.Parse(tempPercent);
                    dataGirdViewExt1.Rows[index].Cells["colPercent"].Value = tempPercent + "%";
                }
                if (pricePercent != 100M)
                {
                    string strPercent = dataGirdViewExt1.Rows[dataGirdViewExt1.Rows.Count - 1].Cells["colPercent"].Value.ToString();
                    decimal percent = decimal.Parse(strPercent.Substring(0, strPercent.Length - 1));
                    dataGirdViewExt1.Rows[dataGirdViewExt1.Rows.Count - 1].Cells["colPercent"].Value = percent + 100M - pricePercent;
                }
                txtTotalQty.Text = totalQty.ToString();
                txtTotalAmount.Text = totalAmount.ToString("f2");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string beginDate = dateTimePicker1.Value.ToString("yyyy-MM-dd 00:00:00");
            string endDate = dateTimePicker2.Value.ToString("yyyy-MM-dd 23:59:59");
            OrderService orderService = new OrderService();
            IList<HourOrderSales> hourSalesList = orderService.GetHourSalesReport(DateTime.Parse(beginDate), DateTime.Parse(endDate));
            BindSalesReportData(hourSalesList);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }
    }
}
