using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.Common;

namespace Top4ever.Pos
{
    public partial class FormSecondScreen : Form
    {
        public FormSecondScreen()
        {
            InitializeComponent();
        }

        public void BindGoodsOrderInfo(DataGridView dgvGoodsOrder)
        {
            if (dgvGoodsOrder.Rows.Count > 0)
            {
                this.dgvItemOrder.Rows.Clear();
                foreach (DataGridViewRow dgr in dgvGoodsOrder.Rows)
                {
                    int index = this.dgvItemOrder.Rows.Add();
                    this.dgvItemOrder.Rows[index].Cells["GoodsNum"].Value = dgr.Cells["GoodsNum"].Value;
                    this.dgvItemOrder.Rows[index].Cells["GoodsName"].Value = dgr.Cells["GoodsName"].Value;
                    this.dgvItemOrder.Rows[index].Cells["GoodsPrice"].Value = dgr.Cells["GoodsPrice"].Value;
                }
                BindOrderInfoSum();
            }
        }

        private void BindOrderInfoSum()
        {
            decimal totalPrice = 0, totalDiscount = 0;
            for (int i = 0; i < dgvItemOrder.Rows.Count; i++)
            {
                totalPrice += Convert.ToDecimal(dgvItemOrder.Rows[i].Cells["GoodsPrice"].Value);
                totalDiscount += Convert.ToDecimal(dgvItemOrder.Rows[i].Cells["GoodsDiscount"].Value);
            }
            this.lbTotalPrice.Text = "总金额：" + totalPrice.ToString("f2");
            this.lbDiscount.Text = "折扣：" + totalDiscount.ToString("f2");
            decimal wholePayMoney = totalPrice + totalDiscount;
            decimal actualPayMoney = CutOffDecimal.HandleCutOff(wholePayMoney, CutOffType.ROUND_OFF, 0);
            decimal cutoff = wholePayMoney - actualPayMoney;
            this.lbNeedPayMoney.Text = "实际应付：" + actualPayMoney.ToString("f2");
            this.lbCutOff.Text = "去零：" + (-cutoff).ToString("f2");
        }
    }
}
