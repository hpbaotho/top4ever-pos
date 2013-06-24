using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.Entity;

namespace Top4ever.Pos.Membership
{
    public partial class FormVIPCardStatus : Form
    {
        public FormVIPCardStatus()
        {
            InitializeComponent();
        }

        private void FormVIPCardStatus_Load(object sender, EventArgs e)
        {
            cmbCardStatus.Items.Clear();
            cmbCardStatus.Items.Add("开卡");
            if (RightsItemCode.FindRights(RightsItemCode.REPORTLOSS))
            {
                cmbCardStatus.Items.Add("挂失");
            }
            if (RightsItemCode.FindRights(RightsItemCode.LOCKCARD))
            {
                cmbCardStatus.Items.Add("锁卡");
            }
            if (RightsItemCode.FindRights(RightsItemCode.CANCELCARD))
            {
                cmbCardStatus.Items.Add("作废");
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
