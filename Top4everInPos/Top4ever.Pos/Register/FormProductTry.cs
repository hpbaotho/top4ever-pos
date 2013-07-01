using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Web;
using System.Diagnostics;

namespace Top4ever.Pos.Register
{
    public partial class FormProductTry : Form
    {
        private int remainDays;
        private string shopName;
        private string shopNo;
        private string productVersion;

        private bool _result = false;
        public bool Result
        {
            get { return _result; }
        }

        public FormProductTry(int remainDays, string shopName, string shopNo, string productVersion)
        {
            this.remainDays = remainDays;
            this.shopName = shopName;
            this.shopNo = shopNo;
            this.productVersion = productVersion;
            InitializeComponent();
        }

        private void FormProductTry_Load(object sender, EventArgs e)
        {
            lbSystemID.Text = GetMACID();
            lbRemainDays.Text = remainDays.ToString();
            if (remainDays <= 0)
            {
                btnTry.Enabled = false;
            }
            lbVersion.Text = productVersion;
        }

        public static string GetMACID()
        {
            //获取网卡硬件地址
            string macAdress = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                    macAdress= mo["MacAddress"].ToString();
                mo.Dispose();
            }
            return macAdress;
        }

        private void btnTry_Click(object sender, EventArgs e)
        {
            _result = true;
            this.Close();
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            FormRegister formRegister = new FormRegister(lbSystemID.Text, shopName, shopNo, productVersion);
            formRegister.ShowDialog();
            _result = formRegister.Result;
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            _result = false;
            this.Close();
        }
    }
}