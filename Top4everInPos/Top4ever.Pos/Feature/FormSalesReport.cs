using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain.Transfer;

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
            int index = this.dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells[0].Value = "营业总额：";
            dataGridView1.Rows[index].Cells[1].Value = "2327.00";
            index = this.dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells[0].Value = "去尾折扣：";
            dataGridView1.Rows[index].Cells[1].Value = "-0.70";
            index = this.dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells[0].Value = "折扣金额：";
            dataGridView1.Rows[index].Cells[1].Value = "121.70";
            index = this.dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells[0].Value = "营收净额：";
            dataGridView1.Rows[index].Cells[1].Value = "2206.00";
            this.dataGridView1.Height = this.dataGridView1.RowTemplate.Height * 4 + 1;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.ClearSelection();
            this.dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = SystemColors.Control;

            BusinessReportService bizReportService = new BusinessReportService();
            BusinessReport bizReport = bizReportService.GetReportDataByHandover("P0");
        }
    }
}