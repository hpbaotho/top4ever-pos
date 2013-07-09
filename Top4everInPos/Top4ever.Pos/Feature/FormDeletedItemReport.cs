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
    public partial class FormDeletedItemReport : Form
    {
        public FormDeletedItemReport()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string beginDate = dateTimePicker1.Value.ToString("yyyy-MM-dd 00:00:00");
            string endDate = dateTimePicker2.Value.ToString("yyyy-MM-dd 23:59:59");
            int dateType = 1;
            if(cmbDateType.Text == "账务日")
            {
                dateType = 1;
            }
            if(cmbDateType.Text == "营业日")
            {
                dateType = 2;
            }
            OrderDetailsService orderDetailsService = new OrderDetailsService();
            DeletedAllItems deletedAllItems = orderDetailsService.GetAllDeletedItems(beginDate, endDate, dateType);
            //绑定单品删除品项
            if (deletedAllItems.DeletedGoodsItemList != null && deletedAllItems.DeletedGoodsItemList.Count > 0)
            {
                decimal totalAmount = 0M;
                foreach (DeletedItem item in deletedAllItems.DeletedGoodsItemList)
                {
                    totalAmount += item.TotalSellPrice;
                    int index = dataGirdViewExt1.Rows.Add();
                    dataGirdViewExt1.Rows[index].Cells["colTranSequence"].Value = item.TranSequence;
                    dataGirdViewExt1.Rows[index].Cells["colDeskName"].Value = item.DeskName;
                    dataGirdViewExt1.Rows[index].Cells["colOrderNo"].Value = item.OrderNo;
                    dataGirdViewExt1.Rows[index].Cells["colGoodsName"].Value = item.GoodsName;
                    dataGirdViewExt1.Rows[index].Cells["colItemQty"].Value = item.ItemQty;
                    dataGirdViewExt1.Rows[index].Cells["colCancelReasonName"].Value = item.CancelReasonName;
                    dataGirdViewExt1.Rows[index].Cells["colCancelEmployeeNo"].Value = item.CancelEmployeeNo;
                    dataGirdViewExt1.Rows[index].Cells["colLastModifiedTime"].Value = item.LastModifiedTime;
                    string type = string.Empty;
                    if (item.PayTime == null)
                    {
                        type = "退菜";
                    }
                    else
                    {
                        type = "退单";
                    }
                    dataGirdViewExt1.Rows[index].Cells["colType"].Value = type;
                }
                txtTotalQty.Text = deletedAllItems.DeletedGoodsItemList.Count.ToString();
                txtTotalAmount.Text = totalAmount.ToString("f2");
            }
            else
            {
                dataGirdViewExt1.Rows.Clear();
            }
            //绑定整单删除品项
            if (deletedAllItems.DeletedOrderItemList != null && deletedAllItems.DeletedOrderItemList.Count > 0)
            {
                decimal totalMoney = 0M;
                foreach (DeletedItem item in deletedAllItems.DeletedOrderItemList)
                {
                    totalMoney += item.TotalSellPrice;
                    int index = dataGirdViewExt2.Rows.Add();
                    dataGirdViewExt2.Rows[index].Cells["colTranSequence1"].Value = item.TranSequence;
                    dataGirdViewExt2.Rows[index].Cells["colDeskName1"].Value = item.DeskName;
                    dataGirdViewExt2.Rows[index].Cells["colOrderNo1"].Value = item.OrderNo;
                    dataGirdViewExt2.Rows[index].Cells["colGoodsName1"].Value = item.GoodsName;
                    dataGirdViewExt2.Rows[index].Cells["colItemQty1"].Value = item.ItemQty;
                    dataGirdViewExt2.Rows[index].Cells["colCancelReasonName1"].Value = item.CancelReasonName;
                    dataGirdViewExt2.Rows[index].Cells["colCancelEmployeeNo1"].Value = item.CancelEmployeeNo;
                    dataGirdViewExt2.Rows[index].Cells["colLastModifiedTime1"].Value = item.LastModifiedTime;
                    string type = string.Empty;
                    if (item.PayTime == null)
                    {
                        type = "退菜";
                    }
                    else
                    {
                        type = "退单";
                    }
                    dataGirdViewExt2.Rows[index].Cells["colType1"].Value = type;
                }
                txtTotalNum.Text = deletedAllItems.DeletedOrderItemList.Count.ToString();
                txtTotalMoney.Text = totalMoney.ToString("f2");
            }
            else
            {
                dataGirdViewExt1.Rows.Clear();
            }
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
