using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VechsoftPos.Register
{
    public partial class FormRegister : Form
    {
        private string systemID;
        private string shopName;
        private string shopNo;
        private string productVersion;

        private bool _result = false;
        public bool Result
        {
            get { return _result; }
        }

        public FormRegister(string systemID, string shopName, string shopNo,string productVersion)
        {
            this.systemID = systemID;
            this.shopName = shopName;
            this.shopNo = shopNo;
            this.productVersion = productVersion;
            InitializeComponent();
        }

        private void Registration_Load(object sender, EventArgs e)
        {
            txtSystemID.Text = this.systemID;
            txtShopName.Text = this.shopName;
            txtShopNo.Text = this.shopNo;
            txtVersion.Text = this.productVersion;
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            if (txtSerialNumber.Text.Trim().Length != 24 || !txtSerialNumber.Text.Trim().Contains("-"))
            {
                MessageBox.Show("Wrong serial number format!");
                _result = false;
                return;
            }
            int nValue = 0;
            if (nValue == 401)
            {
                MessageBox.Show("Can not connect to server!");
                _result = false;
            }
            if (nValue == 0 || nValue == -1)
            {
                MessageBox.Show("Registration failed!");
                _result = false;
            }
            if (nValue == 1 || nValue == 2)
            {
                MessageBox.Show("Registration sucess!");
                _result = true;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _result = false;
            this.Close();
        }
    }
}