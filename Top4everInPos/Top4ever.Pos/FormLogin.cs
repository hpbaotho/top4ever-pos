using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.CustomControl;
using Top4ever.Domain.Accounts;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Enum;
using Top4ever.Domain.GoodsRelated;

namespace VechsoftPos
{
    public partial class FormLogin : Form
    {
        private int loginCount = 0;
        private const int MAX_LOGIN_COUNT = 5;
        private TextBox m_ActiveTextBox;

        public FormLogin()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(ConstantValuePool.BizSettingConfig.LoginImagePath) && File.Exists(ConstantValuePool.BizSettingConfig.LoginImagePath))
            {
                this.BackgroundImage = Image.FromFile(ConstantValuePool.BizSettingConfig.LoginImagePath);
                this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            m_ActiveTextBox = this.txtName;
            int px = (this.Width - this.pnlSystemLogin.Width) / 2;
            int py = (this.Height - this.pnlSystemLogin.Height) / 2;
            this.pnlSystemLogin.Location = new Point(px, py);
            //第二屏
            if (Screen.AllScreens.Length > 1 && ConstantValuePool.BizSettingConfig.SecondScreenEnabled)
            {
                ConstantValuePool.SecondScreenForm = new FormSecondScreen();
                ConstantValuePool.SecondScreenForm.Show();
            }
        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            CrystalButton btn = sender as CrystalButton;
            int index = m_ActiveTextBox.SelectionStart;
            string text = m_ActiveTextBox.Text;
            text = text.Insert(index, btn.Text);
            m_ActiveTextBox.Text = text;
            //光标移动到下一位
            m_ActiveTextBox.Select(index + 1, 0);
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            string inputString = m_ActiveTextBox.Text;
            if (!string.IsNullOrEmpty(inputString))
            {
                m_ActiveTextBox.Select();
                SendKeys.Send("{BACKSPACE}");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //判断程序的配置文件
            if (ConstantValuePool.BizSettingConfig == null)
            {
                MessageBox.Show("找不到程序的配置文件！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            if (ConstantValuePool.CurrentShop == null)
            {
                MessageBox.Show("获取不到店铺信息，请检查服务器IP配置或者网络是否正确！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            string employeeName = this.txtName.Text.Trim();
            string password = this.txtPassword.Text.Trim();
            if (string.IsNullOrEmpty(employeeName))
            {
                MessageBox.Show("用户名不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("密码不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Employee employee = null;
            int result = EmployeeService.GetInstance().EmployeeLogin(employeeName, password, ref employee);
            if (result == 1)
            {
                bool haveDailyClose = true;
                if (DateTime.Now.Hour > 5)
                {
                    int status = DailyBalanceService.GetInstance().CheckLastDailyStatement();
                    if (status == 0)
                    {
                        MessageBox.Show("获取最后日结时间出错，请重新登录！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (status == 2)    //未日结
                    {
                        haveDailyClose = false;
                        MessageBox.Show("警告，上次未日结，请联系店长处理！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        haveDailyClose = true;
                        if (status == 3)
                        {
                            MessageBox.Show("警告，营业日期出现异常，请联系店长处理！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                //保存静态池内
                ConstantValuePool.CurrentEmployee = employee;
                //获取基础数据
                SysBasicData sysBasicData = SystemBasicDataService.GetInstance().GetSysBasicData();
                if (sysBasicData != null)
                {
                    ConstantValuePool.NoticeList = sysBasicData.NoticeList;
                    ConstantValuePool.SysConfig = sysBasicData.SysConfig;
                    ConstantValuePool.RegionList = sysBasicData.RegionList;
                    ConstantValuePool.DiscountList = sysBasicData.DiscountList;
                    ConstantValuePool.PayoffWayList = sysBasicData.PayoffWayList;
                    ConstantValuePool.ReasonList = sysBasicData.ReasonList;
                    ConstantValuePool.GoodsGroupList = sysBasicData.GoodsGroupList;
                    ConstantValuePool.DetailsGroupList = sysBasicData.DetailsGroupList;
                    ConstantValuePool.GoodsSetMealList = sysBasicData.GoodsSetMealList;
                    ConstantValuePool.GoodsCronTriggerList = sysBasicData.GoodsCronTriggerList;
                    ConstantValuePool.ButtonStyleList = sysBasicData.ButtonStyleList;
                    ConstantValuePool.PromotionList = sysBasicData.PromotionList;
                    ConstantValuePool.PromotionConditionList = sysBasicData.PromotionConditionList;
                    ConstantValuePool.PromotionCronTriggerList = sysBasicData.PromotionCronTriggerList;
                    ConstantValuePool.PromotionPresentList = sysBasicData.PromotionPresentList;
                    
                    IList<GoodsLimitedTimeSale> groupLimitedTimeSaleList = new List<GoodsLimitedTimeSale>();
                    IList<GoodsLimitedTimeSale> goodsLimitedTimeSaleList = new List<GoodsLimitedTimeSale>();
                    if (sysBasicData.TotalLimitedTimeSaleList != null && sysBasicData.TotalLimitedTimeSaleList.Count > 0)
                    {
                        foreach (GoodsLimitedTimeSale item in sysBasicData.TotalLimitedTimeSaleList)
                        {
                            if (item.ItemType == 1) //Group
                            {
                                groupLimitedTimeSaleList.Add(item);
                            }
                            if (item.ItemType == 2) //Item
                            {
                                goodsLimitedTimeSaleList.Add(item);
                            }
                        }
                    }
                    ConstantValuePool.GroupLimitedTimeSaleList = groupLimitedTimeSaleList;
                    ConstantValuePool.GoodsLimitedTimeSaleList = goodsLimitedTimeSaleList;

                    IList<GoodsCombinedSale> groupCombinedSaleList = new List<GoodsCombinedSale>();
                    IList<GoodsCombinedSale> goodsCombinedSaleList = new List<GoodsCombinedSale>();
                    if (sysBasicData.TotalCombinedSaleList != null && sysBasicData.TotalCombinedSaleList.Count > 0)
                    {
                        foreach (GoodsCombinedSale item in sysBasicData.TotalCombinedSaleList)
                        {
                            if (item.ItemType == 1) //Group
                            {
                                groupCombinedSaleList.Add(item);
                            }
                            if (item.ItemType == 2) //Item
                            {
                                goodsCombinedSaleList.Add(item);
                            }
                        }
                    }
                    ConstantValuePool.GroupCombinedSaleList = groupCombinedSaleList;
                    ConstantValuePool.GoodsCombinedSaleList = goodsCombinedSaleList;

                    if (ConstantValuePool.BizSettingConfig.SaleType == ShopSaleType.Takeout)
                    {
                        FormTakeout formTakeout = new FormTakeout(haveDailyClose);
                        formTakeout.VisibleShow = true;
                        formTakeout.ShowDialog();
                    }
                    else
                    {
                        FormDesk deskForm = new FormDesk(haveDailyClose);
                        ConstantValuePool.DeskForm = deskForm;
                        txtPassword.Text = string.Empty;
                        txtPassword.Focus();
                        deskForm.ShowDialog();
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

        private void btnSwipeCard_Click(object sender, EventArgs e)
        {
            Feature.FormSwipeCard form = new Feature.FormSwipeCard();
            form.ShowDialog();
            if (string.IsNullOrEmpty(form.CardNumber))
            {
                return;
            }
            string attendanceCard = form.CardNumber;

            //判断程序的配置文件
            if (ConstantValuePool.BizSettingConfig == null)
            {
                MessageBox.Show("找不到程序的配置文件！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            if (ConstantValuePool.CurrentShop == null)
            {
                MessageBox.Show("获取不到店铺信息，请检查服务器IP配置或者网络是否正确！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

            Employee employee = null;
            int result = EmployeeService.GetInstance().EmployeeLogin(attendanceCard, ref employee);
            if (result == 1)
            {
                bool haveDailyClose = true;
                if (DateTime.Now.Hour > 5)
                {
                    int status = DailyBalanceService.GetInstance().CheckLastDailyStatement();
                    if (status == 0)
                    {
                        MessageBox.Show("获取最后日结时间出错，请重新登录！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (status == 2)    //未日结
                    {
                        haveDailyClose = false;
                        MessageBox.Show("警告，上次未日结，请联系店长处理！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        haveDailyClose = true;
                        if (status == 3)
                        {
                            MessageBox.Show("警告，营业日期出现异常，请联系店长处理！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                //保存静态池内
                ConstantValuePool.CurrentEmployee = employee;
                //获取基础数据
                SysBasicData sysBasicData = SystemBasicDataService.GetInstance().GetSysBasicData();
                if (sysBasicData != null)
                {
                    ConstantValuePool.NoticeList = sysBasicData.NoticeList;
                    ConstantValuePool.SysConfig = sysBasicData.SysConfig;
                    ConstantValuePool.RegionList = sysBasicData.RegionList;
                    ConstantValuePool.DiscountList = sysBasicData.DiscountList;
                    ConstantValuePool.PayoffWayList = sysBasicData.PayoffWayList;
                    ConstantValuePool.ReasonList = sysBasicData.ReasonList;
                    ConstantValuePool.GoodsGroupList = sysBasicData.GoodsGroupList;
                    ConstantValuePool.DetailsGroupList = sysBasicData.DetailsGroupList;
                    ConstantValuePool.GoodsSetMealList = sysBasicData.GoodsSetMealList;
                    ConstantValuePool.GoodsCronTriggerList = sysBasicData.GoodsCronTriggerList;
                    ConstantValuePool.ButtonStyleList = sysBasicData.ButtonStyleList;
                    ConstantValuePool.PromotionList = sysBasicData.PromotionList;
                    ConstantValuePool.PromotionConditionList = sysBasicData.PromotionConditionList;
                    ConstantValuePool.PromotionCronTriggerList = sysBasicData.PromotionCronTriggerList;
                    ConstantValuePool.PromotionPresentList = sysBasicData.PromotionPresentList;

                    IList<GoodsLimitedTimeSale> groupLimitedTimeSaleList = new List<GoodsLimitedTimeSale>();
                    IList<GoodsLimitedTimeSale> goodsLimitedTimeSaleList = new List<GoodsLimitedTimeSale>();
                    if (sysBasicData.TotalLimitedTimeSaleList != null && sysBasicData.TotalLimitedTimeSaleList.Count > 0)
                    {
                        foreach (GoodsLimitedTimeSale item in sysBasicData.TotalLimitedTimeSaleList)
                        {
                            if (item.ItemType == 1) //Group
                            {
                                groupLimitedTimeSaleList.Add(item);
                            }
                            if (item.ItemType == 2) //Item
                            {
                                goodsLimitedTimeSaleList.Add(item);
                            }
                        }
                    }
                    ConstantValuePool.GroupLimitedTimeSaleList = groupLimitedTimeSaleList;
                    ConstantValuePool.GoodsLimitedTimeSaleList = goodsLimitedTimeSaleList;

                    IList<GoodsCombinedSale> groupCombinedSaleList = new List<GoodsCombinedSale>();
                    IList<GoodsCombinedSale> goodsCombinedSaleList = new List<GoodsCombinedSale>();
                    if (sysBasicData.TotalCombinedSaleList != null && sysBasicData.TotalCombinedSaleList.Count > 0)
                    {
                        foreach (GoodsCombinedSale item in sysBasicData.TotalCombinedSaleList)
                        {
                            if (item.ItemType == 1) //Group
                            {
                                groupCombinedSaleList.Add(item);
                            }
                            if (item.ItemType == 2) //Item
                            {
                                goodsCombinedSaleList.Add(item);
                            }
                        }
                    }
                    ConstantValuePool.GroupCombinedSaleList = groupCombinedSaleList;
                    ConstantValuePool.GoodsCombinedSaleList = goodsCombinedSaleList;

                    if (ConstantValuePool.BizSettingConfig.SaleType == ShopSaleType.Takeout)
                    {
                        FormTakeout formTakeout = new FormTakeout(haveDailyClose);
                        formTakeout.VisibleShow = true;
                        formTakeout.ShowDialog();
                    }
                    else
                    {
                        FormDesk deskForm = new FormDesk(haveDailyClose);
                        ConstantValuePool.DeskForm = deskForm;
                        txtName.Text = employee.EmployeeNo;
                        txtPassword.Text = string.Empty;
                        txtPassword.Focus();
                        deskForm.ShowDialog();
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
                    MessageBox.Show("获取不到用户信息，请检查是否是本系统的卡号！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            this.Close();
        }
    }
}
