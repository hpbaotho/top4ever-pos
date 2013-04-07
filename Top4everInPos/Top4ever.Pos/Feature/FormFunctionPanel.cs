using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.CustomControl;
using Top4ever.Entity;

namespace Top4ever.Pos.Feature
{
    public partial class FormFunctionPanel : Form
    {
        private List<CrystalButton> btnList = new List<CrystalButton>();
        private bool needExist = false;
        
        public bool IsNeedExist
        {
            get { return needExist; }
        }

        public FormFunctionPanel()
        {
            InitializeComponent();
        }

        private void FormFunctionPanel_Load(object sender, EventArgs e)
        {
            CrystalButton btnBillManage = new CrystalButton();
            btnBillManage.Name = "btnBillManage";
            btnBillManage.Text = "账单管理";
            btnBillManage.BackColor = Color.Teal;
            btnBillManage.ForeColor = Color.White;
            btnBillManage.Click += new EventHandler(this.btnBillManage_Click);
            btnList.Add(btnBillManage);
            CrystalButton btnHandover = new CrystalButton();
            btnHandover.Name = "btnHandover";
            btnHandover.Text = "交班";
            btnHandover.BackColor = Color.Teal;
            btnHandover.ForeColor = Color.White;
            btnHandover.Click += new EventHandler(this.btnHandover_Click);
            btnList.Add(btnHandover);
            //加载权限按钮
            InitializeManagerButton();
            CrystalButton btnMemberStored = new CrystalButton();
            btnMemberStored.Name = "btnMemberStored";
            btnMemberStored.Text = "会员储值";
            btnMemberStored.BackColor = Color.Teal;
            btnMemberStored.ForeColor = Color.White;
            btnMemberStored.Click += new EventHandler(this.btnMemberStored_Click);
            btnList.Add(btnMemberStored);
            CrystalButton btnMemberSearch = new CrystalButton();
            btnMemberSearch.Name = "btnMemberSearch";
            btnMemberSearch.Text = "会员查询";
            btnMemberSearch.BackColor = Color.Teal;
            btnMemberSearch.ForeColor = Color.White;
            btnMemberSearch.Click += new EventHandler(this.btnMemberSearch_Click);
            btnList.Add(btnMemberSearch);
            CrystalButton btnMemberStatus = new CrystalButton();
            btnMemberStatus.Name = "btnMemberStatus";
            btnMemberStatus.Text = "会员状态管理";
            btnMemberStatus.BackColor = Color.Teal;
            btnMemberStatus.ForeColor = Color.White;
            btnMemberStatus.Click += new EventHandler(this.btnMemberStatus_Click);
            btnList.Add(btnMemberStatus);
            CrystalButton btnUploadBill = new CrystalButton();
            btnUploadBill.Name = "btnUploadBill";
            btnUploadBill.Text = "上传单据";
            btnUploadBill.BackColor = Color.Teal;
            btnUploadBill.ForeColor = Color.White;
            btnUploadBill.Click += new EventHandler(this.btnUploadBill_Click);
            btnList.Add(btnUploadBill);
            CrystalButton btnDownloadData = new CrystalButton();
            btnDownloadData.Name = "btnDownloadData";
            btnDownloadData.Text = "下载基础数据";
            btnDownloadData.BackColor = Color.Teal;
            btnDownloadData.ForeColor = Color.White;
            btnDownloadData.Click += new EventHandler(this.btnDownloadData_Click);
            btnList.Add(btnDownloadData);
            CrystalButton btnModifyPassword = new CrystalButton();
            btnModifyPassword.Name = "btnModifyPassword";
            btnModifyPassword.Text = "修改密码";
            btnModifyPassword.BackColor = Color.Teal;
            btnModifyPassword.ForeColor = Color.White;
            btnModifyPassword.Click += new EventHandler(this.btnModifyPassword_Click);
            btnList.Add(btnModifyPassword);
            CrystalButton btnCheckStock = new CrystalButton();
            btnCheckStock.Name = "btnCheckStock";
            btnCheckStock.Text = "沽清功能";
            btnCheckStock.BackColor = Color.Teal;
            btnCheckStock.ForeColor = Color.White;
            btnCheckStock.Click += new EventHandler(this.btnCheckStock_Click);
            btnList.Add(btnCheckStock);
            CrystalButton btnPunchIn = new CrystalButton();
            btnPunchIn.Name = "btnPunchIn";
            btnPunchIn.Text = "上班打卡";
            btnPunchIn.BackColor = Color.Teal;
            btnPunchIn.ForeColor = Color.White;
            btnPunchIn.Click += new EventHandler(this.btnPunchIn_Click);
            btnList.Add(btnPunchIn);
            CrystalButton btnSetting = new CrystalButton();
            btnSetting.Name = "btnSetting";
            btnSetting.Text = "系统设置";
            btnSetting.BackColor = Color.Teal;
            btnSetting.ForeColor = Color.White;
            btnSetting.Click += new EventHandler(this.btnSetting_Click);
            btnList.Add(btnSetting);
            CrystalButton btnCancel = new CrystalButton();
            btnCancel.Name = "btnCancel";
            btnCancel.Text = "取消";
            btnCancel.BackColor = Color.Red;
            btnCancel.ForeColor = Color.White;
            btnCancel.Click += new EventHandler(this.btnCancel_Click);
            btnList.Add(btnCancel);

            //判断每行显示多少列
            int count = btnList.Count;
            int maxColumn = Convert.ToInt32(Math.Ceiling((Math.Sqrt(Convert.ToDouble(count)))));
            //显示多少行
            int perLine = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(count) / maxColumn));
            //计算button长宽
            int space = 6;
            int width = (this.pnlFunction.Width - space * (maxColumn + 1)) / maxColumn;
            int height = (this.pnlFunction.Height - space * (perLine + 1)) / perLine;
            int index = 1;
            int px = space, py = space;
            foreach (CrystalButton btn in btnList)
            {
                btn.Width = width;
                btn.Height = height;
                btn.Location = new Point(px, py);
                btn.Font = new Font("Microsoft YaHei", 10.5F, FontStyle.Bold);
                this.pnlFunction.Controls.Add(btn);

                index++;
                if (index > maxColumn)
                {
                    px = space;
                    py += height + space;
                    index = 1;
                }
                else
                {
                    px += width + space;
                }
            }
        }

        private void InitializeManagerButton()
        {
            if (RightsItemCode.FindRights(RightsItemCode.DAILYSTATEMENT))
            {
                CrystalButton btnDailyStatement = new CrystalButton();
                btnDailyStatement.Name = "btnDailyStatement";
                btnDailyStatement.Text = "日结";
                btnDailyStatement.BackColor = Color.Teal;
                btnDailyStatement.ForeColor = Color.White;
                btnDailyStatement.Click += new EventHandler(this.btnDailyStatement_Click);
                btnList.Add(btnDailyStatement);
            }
            if (RightsItemCode.FindRights(RightsItemCode.PETTYCASHMODIFY))
            {
                CrystalButton btnPettyCashModify = new CrystalButton();
                btnPettyCashModify.Name = "btnPettyCashModify";
                btnPettyCashModify.Text = "零用金修改";
                btnPettyCashModify.BackColor = Color.Teal;
                btnPettyCashModify.ForeColor = Color.White;
                btnPettyCashModify.Click += new EventHandler(this.btnPettyCashModify_Click);
                btnList.Add(btnPettyCashModify);
            }
            if (RightsItemCode.FindRights(RightsItemCode.HOURSTURNOVER))
            {
                CrystalButton btnHoursTurnover = new CrystalButton();
                btnHoursTurnover.Name = "btnHoursTurnover";
                btnHoursTurnover.Text = "小时营业额";
                btnHoursTurnover.BackColor = Color.Teal;
                btnHoursTurnover.ForeColor = Color.White;
                btnHoursTurnover.Click += new EventHandler(this.btnHoursTurnover_Click);
                btnList.Add(btnHoursTurnover);
            }
            if (RightsItemCode.FindRights(RightsItemCode.HISTORYSEARCH))
            {
                CrystalButton btnHistorySearch = new CrystalButton();
                btnHistorySearch.Name = "btnHistorySearch";
                btnHistorySearch.Text = "历史日结/交班查询";
                btnHistorySearch.BackColor = Color.Teal;
                btnHistorySearch.ForeColor = Color.White;
                btnHistorySearch.Click += new EventHandler(this.btnHistorySearch_Click);
                btnList.Add(btnHistorySearch);
            }
            if (RightsItemCode.FindRights(RightsItemCode.SINGLEGOODSSTATISTICS))
            {
                CrystalButton btnSingleGoodsStatistics = new CrystalButton();
                btnSingleGoodsStatistics.Name = "btnSingleGoodsStatistics";
                btnSingleGoodsStatistics.Text = "单品销量统计";
                btnSingleGoodsStatistics.BackColor = Color.Teal;
                btnSingleGoodsStatistics.ForeColor = Color.White;
                btnSingleGoodsStatistics.Click += new EventHandler(this.btnSingleGoodsStatistics_Click);
                btnList.Add(btnSingleGoodsStatistics);
            }
            if (RightsItemCode.FindRights(RightsItemCode.DELETEDGOODSREPORT))
            {
                CrystalButton btnDeletedGoodsReport = new CrystalButton();
                btnDeletedGoodsReport.Name = "btnDeletedGoodsReport";
                btnDeletedGoodsReport.Text = "删除品项报表";
                btnDeletedGoodsReport.BackColor = Color.Teal;
                btnDeletedGoodsReport.ForeColor = Color.White;
                btnDeletedGoodsReport.Click += new EventHandler(this.btnDeletedGoodsReport_Click);
                btnList.Add(btnDeletedGoodsReport);
            }
            if (RightsItemCode.FindRights(RightsItemCode.PETTYCASHREPORT))
            {
                CrystalButton btnPettyCashReport = new CrystalButton();
                btnPettyCashReport.Name = "btnPettyCashReport";
                btnPettyCashReport.Text = "零用金报表";
                btnPettyCashReport.BackColor = Color.Teal;
                btnPettyCashReport.ForeColor = Color.White;
                btnPettyCashReport.Click += new EventHandler(this.btnPettyCashReport_Click);
                btnList.Add(btnPettyCashReport);
            }
            if (RightsItemCode.FindRights(RightsItemCode.HISTORYDAILYREPORT))
            {
                CrystalButton btnHistoryDailyReport = new CrystalButton();
                btnHistoryDailyReport.Name = "btnHistoryDailyReport";
                btnHistoryDailyReport.Text = "历史日结汇总表";
                btnHistoryDailyReport.BackColor = Color.Teal;
                btnHistoryDailyReport.ForeColor = Color.White;
                btnHistoryDailyReport.Click += new EventHandler(this.btnHistoryDailyReport_Click);
                btnList.Add(btnHistoryDailyReport);
            }
            if (RightsItemCode.FindRights(RightsItemCode.PUNCHINREPORT))
            {
                CrystalButton btnPunchInReport = new CrystalButton();
                btnPunchInReport.Name = "btnPunchInReport";
                btnPunchInReport.Text = "打卡报表";
                btnPunchInReport.BackColor = Color.Teal;
                btnPunchInReport.ForeColor = Color.White;
                btnPunchInReport.Click += new EventHandler(this.btnPunchInReport_Click);
                btnList.Add(btnPunchInReport);
            }
            if (RightsItemCode.FindRights(RightsItemCode.LOGSEARCH))
            {
                CrystalButton btnLogSearch = new CrystalButton();
                btnLogSearch.Name = "btnLogSearch";
                btnLogSearch.Text = "日志查询";
                btnLogSearch.BackColor = Color.Teal;
                btnLogSearch.ForeColor = Color.White;
                btnLogSearch.Click += new EventHandler(this.btnLogSearch_Click);
                btnList.Add(btnLogSearch);
            }
        }

        private void btnBillManage_Click(object sender, EventArgs e)
        {
            FormBillManagement form = new FormBillManagement();
            form.ShowDialog();
        }

        private void btnHandover_Click(object sender, EventArgs e)
        {
            int modelType = 1;
            FormSalesReport formReport = new FormSalesReport(modelType);
            formReport.ShowDialog();
            if (formReport.HandleSuccess)
            {
                needExist = true;
                this.Close();
            }
        }

        private void btnMemberStored_Click(object sender, EventArgs e)
        {

        }

        private void btnMemberSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnMemberStatus_Click(object sender, EventArgs e)
        {

        }

        private void btnUploadBill_Click(object sender, EventArgs e)
        {

        }

        private void btnDownloadData_Click(object sender, EventArgs e)
        {

        }

        private void btnModifyPassword_Click(object sender, EventArgs e)
        {

        }

        private void btnCheckStock_Click(object sender, EventArgs e)
        {

        }

        private void btnPunchIn_Click(object sender, EventArgs e)
        {

        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            FormConfig form = new FormConfig();
            form.ShowDialog();
        }

        private void btnDailyStatement_Click(object sender, EventArgs e)
        {
            int modelType = 2;
            FormSalesReport formReport = new FormSalesReport(modelType);
            formReport.ShowDialog();
            if (formReport.HandleSuccess)
            {
                needExist = true;
                this.Close();
            }
        }

        private void btnPettyCashModify_Click(object sender, EventArgs e)
        {

        }

        private void btnHoursTurnover_Click(object sender, EventArgs e)
        {

        }

        private void btnHistorySearch_Click(object sender, EventArgs e)
        {

        }

        private void btnSingleGoodsStatistics_Click(object sender, EventArgs e)
        {

        }
        private void btnDeletedGoodsReport_Click(object sender, EventArgs e)
        {

        }

        private void btnPettyCashReport_Click(object sender, EventArgs e)
        {

        }

        private void btnHistoryDailyReport_Click(object sender, EventArgs e)
        {

        }

        private void btnPunchInReport_Click(object sender, EventArgs e)
        {

        }

        private void btnLogSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
