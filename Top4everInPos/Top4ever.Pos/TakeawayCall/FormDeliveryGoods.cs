using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.Domain.Transfer;
using Top4ever.CustomControl;

namespace Top4ever.Pos.TakeawayCall
{
    public partial class FormDeliveryGoods : Form
    {
        private bool m_HandwritingEnabled = false;
        private SalesOrder _salesOrder = null;
        private string _telephone;
        private string _customerName;
        private string _address;

        public FormDeliveryGoods(SalesOrder salesOrder, string telephone, string customerName, string address)
        {
            _salesOrder = salesOrder;
            _telephone = telephone;
            _customerName = customerName;
            _address = address;
            InitializeComponent();
        }

        private void FormDeliveryGoods_Load(object sender, EventArgs e)
        {
            ShowHandwriting();
            if (_salesOrder != null)
            {
                this.txtTranSeq.Text = _salesOrder.order.TranSequence.ToString();
                this.txtTotalAmount.Text = (_salesOrder.order.ActualSellPrice + _salesOrder.order.ServiceFee).ToString("f2");
                this.txtTelephone.Text = _telephone;
                this.txtName.Text = _customerName;
                this.txtAddress.Text = _address;
            }
        }

        private void btnHandwriting_Click(object sender, EventArgs e)
        {
            m_HandwritingEnabled = !m_HandwritingEnabled;
            ShowHandwriting();
        }

        private void ShowHandwriting()
        {
            if (m_HandwritingEnabled)
            {
                this.Height = 578;
                this.handwritingPad1.Visible = true;
                this.btnHandwriting.Text = "手写输入∧";
            }
            else
            {
                this.Height = 350;
                this.handwritingPad1.Visible = false;
                this.btnHandwriting.Text = "手写输入∨";
            }
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {

        }

        private void handwritingPad1_UserHandWriting(object sender, InkWritingEventArgs e)
        {
            this.txtRemark.Text += e.InkPadValue;
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            string inputString = this.txtRemark.Text;
            if (!string.IsNullOrEmpty(inputString))
            {
                this.txtRemark.TabIndex = 0;
                this.txtRemark.Focus();
                SendKeys.Send("{BACKSPACE}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
