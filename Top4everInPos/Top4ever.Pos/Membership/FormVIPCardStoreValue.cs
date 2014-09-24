using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Common;
using Top4ever.CustomControl;
using Top4ever.Domain.OrderRelated;
using Top4ever.Entity;
using Top4ever.Domain;
using Top4ever.Domain.Transfer;

namespace Top4ever.Pos.Membership
{
    public partial class FormVIPCardStoreValue : Form
    {
        private PayoffWay _curPayoffWay;
        private CrystalButton _curButton;

        public FormVIPCardStoreValue()
        {
            InitializeComponent();
        }

        private void FormVIPCardStoreValue_Load(object sender, EventArgs e)
        {
            BindPayoffWay();
        }

        private void BindPayoffWay()
        {
            this.pnlPayoffWay.Controls.Clear();
            //support six buttons 
            int space = 5;
            int px = 0, py = 0;
            int width = (pnlPayoffWay.Width - 2 * space) / 3;
            int height = (pnlPayoffWay.Height - space) / 2;
            List<PayoffWay> payoffWayList = new List<PayoffWay>();
            foreach (PayoffWay item in ConstantValuePool.PayoffWayList)
            {
                if (item.AsVIPCardPayWay)
                {
                    payoffWayList.Add(item);
                }
            }
            int pageSize = 0;
            if (payoffWayList.Count > 6)
            {
                pageSize = 6;
            }
            else
            {
                pageSize = payoffWayList.Count;
            }
            for (int index = 0; index < pageSize; index++)
            {
                PayoffWay payoff = payoffWayList[index];
                CrystalButton btn = new CrystalButton();
                btn.Name = payoff.PayoffID.ToString();
                btn.Text = payoff.PayoffName;
                btn.Width = width;
                btn.Height = height;
                btn.BackColor = btn.DisplayColor = Color.Blue;
                btn.Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular);
                btn.ForeColor = Color.White;
                btn.Location = new Point(px, py);
                foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                {
                    if (payoff.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                    {
                        float emSize = (float)btnStyle.FontSize;
                        FontStyle style = FontStyle.Regular;
                        btn.Font = new Font(btnStyle.FontName, emSize, style);
                        btn.ForeColor = ColorConvert.RGB(btnStyle.ForeColor);
                        btn.BackColor = btn.DisplayColor = ColorConvert.RGB(btnStyle.BackColor);
                        break;
                    }
                }
                btn.Tag = payoff;
                btn.Click += new System.EventHandler(this.btnPayoff_Click);
                pnlPayoffWay.Controls.Add(btn);
                px += width + space;
                if ((index + 1) % 3 == 0)
                {
                    px = 0;
                    py += height + space;
                }
            }
        }

        private void btnNumeric_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (this.txtStoreAmout.Text.Trim() == ".")
            {
                this.txtStoreAmout.Text = btn.Text;
            }
            else if (this.txtStoreAmout.Text.Trim() == "0" && btn.Text == ".")
            {
                this.txtStoreAmout.Text += btn.Text;
            }
            else if (this.txtStoreAmout.Text.Trim() == "0" && btn.Text != ".")
            {
                this.txtStoreAmout.Text = btn.Text;
            }
            else
            {
                if (this.txtStoreAmout.Text.IndexOf('.') > 0 && btn.Text == ".")
                {
                    //do nothing
                }
                else
                {
                    this.txtStoreAmout.Text += btn.Text;
                }
            }
            this.txtStoreAmout.Focus();
            this.txtStoreAmout.Select(this.txtStoreAmout.Text.Length, 0);//光标移动到文本的末尾
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            string text = this.txtStoreAmout.Text;
            if (text.Length > 0)
            {
                this.txtStoreAmout.Text = text.Substring(0, text.Length - 1);
            }
        }

        private void btnAddBigNumber_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string bigNumber = btn.Text.Substring(1);
            decimal storeAmout = 0M;
            if (this.txtStoreAmout.Text.EndsWith("."))
            {
                storeAmout = decimal.Parse(this.txtStoreAmout.Text.Substring(0, this.txtStoreAmout.Text.Length - 1));
            }
            else
            {
                if (!string.IsNullOrEmpty(this.txtStoreAmout.Text))
                {
                    storeAmout = decimal.Parse(this.txtStoreAmout.Text);
                }
            }
            storeAmout += decimal.Parse(bigNumber);
            this.txtStoreAmout.Text = storeAmout.ToString("f2");
        }

        private void btnPayoff_Click(object sender, EventArgs e)
        {
            if (_curButton != null)
            {
                _curButton.BackColor = _curButton.DisplayColor;
            }
            CrystalButton btn = sender as CrystalButton;
            btn.BackColor = ConstantValuePool.PressedColor;
            _curButton = btn;
            _curPayoffWay = btn.Tag as PayoffWay;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string cardNo = txtCardNo.Text.Trim();
            if (string.IsNullOrEmpty(cardNo))
            {
                MessageBox.Show("请输入您的会员卡号！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(txtStoreAmout.Text.Trim()))
            {
                MessageBox.Show("请输入充值金额！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            decimal storeAmout = 0;
            if (!decimal.TryParse(txtStoreAmout.Text.Trim(), out storeAmout))
            {
                MessageBox.Show("请输入正确的充值金额格式！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_curPayoffWay == null)
            {
                MessageBox.Show("请输入充值付款方式！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            VIPCardAddMoney cardAddMoney = new VIPCardAddMoney();
            cardAddMoney.CardNo = cardNo;
            cardAddMoney.StoreMoney = storeAmout;
            cardAddMoney.EmployeeNo = ConstantValuePool.CurrentEmployee.EmployeeNo;
            cardAddMoney.DeviceNo = ConstantValuePool.BizSettingConfig.DeviceNo;
            cardAddMoney.PayoffID = _curPayoffWay.PayoffID;
            cardAddMoney.PayoffName = _curPayoffWay.PayoffName;
            string tradePayNo = string.Empty;
            int result = VIPCardTradeService.GetInstance().AddVIPCardStoredValue(cardAddMoney, out tradePayNo);
            if (result == 1)
            {
                MessageBox.Show("会员充值成功！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else if (result == 2)
            {
                MessageBox.Show("卡号不存在，请确认输入的卡号是否正确！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (result == 3)
            {
                MessageBox.Show("该卡未开通，请先开卡！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (result == 4)
            {
                MessageBox.Show("该卡已挂失，不能充值！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (result == 5)
            {
                MessageBox.Show("该卡已锁卡，不能充值！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (result == 6)
            {
                MessageBox.Show("该卡已作废，不能充值！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (result == 7)
            {
                MessageBox.Show("该卡所属会员组没有储值功能！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("服务器出现错误，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
