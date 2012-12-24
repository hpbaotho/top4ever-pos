using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.ClientService.Enum;
using Top4ever.Common;
using Top4ever.CustomControl;
using Top4ever.Domain.Accounts;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Config;
using Top4ever.Print;

namespace Top4ever.Pos
{
    public partial class FormLogin : Form
    {
        private int loginCount = 0;
        private const int MAX_LOGIN_COUNT = 5;
        private TextBox m_ActiveTextBox;

        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.AcceptButton = this.btnLogin;
            this.CancelButton = this.btnExit;
            m_ActiveTextBox = this.txtName;
            int px = (this.Width - this.pnlSystemLogin.Width) / 2;
            int py = (this.Height - this.pnlSystemLogin.Height) / 2;
            this.pnlSystemLogin.Location = new Point(px, py);
        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            CrystalButton btn = sender as CrystalButton;
            if (string.IsNullOrEmpty(m_ActiveTextBox.Text))
            {
                m_ActiveTextBox.Text = btn.Text;
            }
            else
            {
                m_ActiveTextBox.Text += btn.Text;
            }
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            string inputString = m_ActiveTextBox.Text;
            if (!string.IsNullOrEmpty(inputString))
            {
                m_ActiveTextBox.Text = m_ActiveTextBox.Text.Substring(0, inputString.Length - 1);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            m_ActiveTextBox.Text = string.Empty;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //读取程序的配置文件
            string appSettingPath = "Config/AppSetting.config";
            if (File.Exists(appSettingPath))
            {
                AppSettingConfig appSettingConfig = XmlUtil.Deserialize<AppSettingConfig>(appSettingPath);
                ConstantValuePool.BizSettingConfig = appSettingConfig;
            }
            else
            {
                MessageBox.Show("找不到程序的配置文件！");
                this.Close();
            }

            Employee employee = null;
            EmployeeService employeeService = new EmployeeService();
            int result = employeeService.EmployeeLogin(this.txtName.Text.Trim(), this.txtPassword.Text.Trim(), ref employee);
            if (result == 1)
            {
                //保存静态池内
                ConstantValuePool.CurrentEmployee = employee;
                //获取基础数据
                SystemBasicData basicDataService = new SystemBasicData();
                SysBasicData sysBasicData = basicDataService.GetSysBasicData();
                if (sysBasicData != null)
                {
                    ConstantValuePool.CurrentShop = sysBasicData.CurrentShop;
                    ConstantValuePool.RegionList = sysBasicData.RegionList;
                    ConstantValuePool.DiscountList = sysBasicData.DiscountList;
                    ConstantValuePool.PayoffWayList = sysBasicData.PayoffWayList;
                    ConstantValuePool.ReasonList = sysBasicData.ReasonList;
                    ConstantValuePool.GoodsGroupList = sysBasicData.GoodsGroupList;
                    ConstantValuePool.DetailsGroupList = sysBasicData.DetailsGroupList;
                    ConstantValuePool.ButtonStyleList = sysBasicData.ButtonStyleList;

                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    loginCount++;
                    if (loginCount == MAX_LOGIN_COUNT)
                    {
                        MessageBox.Show("超出登录次数限制，请重新运行系统！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.Cancel;
                    }
                    else
                    {
                        MessageBox.Show("获取的数据为空，请重新操作！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            else if (result == 2)
            {
                loginCount++;
                if (loginCount == MAX_LOGIN_COUNT)
                {
                    MessageBox.Show("超出登录次数限制，请重新运行系统！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.Cancel;
                }
                else
                {
                    MessageBox.Show("您输入的用户名或者密码错误，请重新输入！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                loginCount++;
                if (loginCount == MAX_LOGIN_COUNT)
                {
                    MessageBox.Show("超出登录次数限制，请重新运行系统！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.Cancel;
                }
                else
                {
                    MessageBox.Show("数据库操作失败，请重新输入！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void txtName_MouseDown(object sender, MouseEventArgs e)
        {
            m_ActiveTextBox = this.ActiveControl as TextBox;
        }

        private void txtPassword_MouseDown(object sender, MouseEventArgs e)
        {
            m_ActiveTextBox = this.ActiveControl as TextBox;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
