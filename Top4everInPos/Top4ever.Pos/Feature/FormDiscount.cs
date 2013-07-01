using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.CustomControl;
using Top4ever.Domain.OrderRelated;
using Top4ever.Entity;
using Top4ever.Entity.Enum;

namespace Top4ever.Pos.Feature
{
    public partial class FormDiscount : Form
    {
        private Discount m_DiscountItem;
        private Discount m_CurrentDiscount;
        private decimal m_ActualSellPrice;

        public Discount CurrentDiscount
        {
            get { return m_CurrentDiscount; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayModel">显示模式 1:单品折扣, 2:整单折扣</param>
        /// <param name="expenditure">未打折的消费金额</param>
        /// <param name="actualSellPrice">整单实际金额</param>
        public FormDiscount(DiscountDisplayModel displayModel, decimal expenditure, decimal actualSellPrice)
        {
            m_ActualSellPrice = actualSellPrice;
            InitializeComponent();
            //显示折扣方式
            //1:折扣率 2:固定折扣
            IList<Discount> discountRateList = new List<Discount>();
            IList<Discount> discountOffPayList = new List<Discount>();
            foreach (Discount item in ConstantValuePool.DiscountList)
            {
                if (item.DisplayModel == (int)DiscountDisplayModel.ALL || item.DisplayModel == (int)displayModel)
                {
                    if (expenditure == -1 || expenditure > item.MinQuotas)
                    {
                        if (item.DiscountType == (int)DiscountItemType.DiscountRate)
                        {
                            if (item.DiscountRate <= ConstantValuePool.CurrentEmployee.MinDiscount)
                            {
                                discountRateList.Add(item);
                            }
                        }
                        if (item.DiscountType == (int)DiscountItemType.OffFixPay)
                        {
                            if (item.OffFixPay / item.MinQuotas <= ConstantValuePool.CurrentEmployee.MinDiscount)
                            {
                                discountOffPayList.Add(item);
                            }
                        }
                    }
                }
            }
            //显示折扣率
            int count = discountRateList.Count;
            int maxColumn = (int)Math.Ceiling(Convert.ToDecimal(count) / 5);
            int maxRow = (int)Math.Ceiling(Convert.ToDecimal(count) / maxColumn);
            int space = 8;
            int width = (this.tabPage1.Width - (maxColumn - 1) * space) / maxColumn;
            int height = (this.tabPage1.Height - (maxRow - 1) * space) / maxRow;
            int px = 0, py = 0, index = 1;
            foreach (Discount item in discountRateList)
            {
                CrystalButton btn = new CrystalButton();
                btn.Name = item.DiscountID.ToString();
                btn.Text = item.DiscountName;
                btn.BackColor = Color.Teal;
                btn.Font = new Font("微软雅黑", 12F, FontStyle.Regular);
                btn.ForeColor = Color.White;
                btn.Width = width;
                btn.Height = height;
                btn.Location = new Point(px, py);
                btn.Tag = item;
                btn.Click += new System.EventHandler(this.btnDiscount_Click);
                this.tabPage1.Controls.Add(btn);

                index++;
                if (index > maxColumn)
                {
                    px = 0;
                    py += height + space;
                    index = 1;
                }
                else
                {
                    px += width + space;
                }
            }
            //显示固定折扣
            if (discountOffPayList.Count > 0)
            {
                count = discountOffPayList.Count;
                maxColumn = (int)Math.Ceiling(Convert.ToDecimal(count) / 5);
                maxRow = (int)Math.Ceiling(Convert.ToDecimal(count) / maxColumn);
                width = (this.tabPage2.Width - (maxColumn - 1) * space) / maxColumn;
                height = (this.tabPage2.Height - (maxRow - 1) * space) / maxRow;
                px = 0;
                py = 0;
                index = 1;
                foreach (Discount item in discountOffPayList)
                {
                    CrystalButton btn = new CrystalButton();
                    btn.Name = item.DiscountID.ToString();
                    btn.Text = item.DiscountName;
                    btn.BackColor = Color.Teal;
                    btn.Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular);
                    btn.ForeColor = Color.White;
                    btn.Width = width;
                    btn.Height = height;
                    btn.Location = new Point(px, py);
                    btn.Tag = item;
                    btn.Click += new System.EventHandler(this.btnDiscount_Click);
                    this.tabPage2.Controls.Add(btn);

                    index++;
                    if (index > maxColumn)
                    {
                        px = 0;
                        py += height + space;
                        index = 1;
                    }
                    else
                    {
                        px += width + space;
                    }
                }
            }
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            m_DiscountItem = btn.Tag as Discount;
            this.txtDiscount.Text = m_DiscountItem.DiscountName;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (m_DiscountItem != null)
            {
                m_CurrentDiscount = m_DiscountItem;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCustomPercent_Click(object sender, EventArgs e)
        {
            //权限验证
            bool hasRights = false;
            if (RightsItemCode.FindRights(RightsItemCode.CUSTOMDISCOUNT))
            {
                hasRights = true;
            }
            else
            {
                FormRightsCode form = new FormRightsCode();
                form.ShowDialog();
                if (form.ReturnValue)
                {
                    IList<string> rightsCodeList = form.RightsCodeList;
                    if (RightsItemCode.FindRights(rightsCodeList, RightsItemCode.CUSTOMDISCOUNT))
                    {
                        hasRights = true;
                    }
                }
            }
            if (!hasRights)
            {
                return;
            }
            FormNumericKeypad keyForm = new FormNumericKeypad();
            keyForm.DisplayText = "请输入自定义折扣率( %)";
            keyForm.ShowDialog();
            if (!string.IsNullOrEmpty(keyForm.KeypadValue))
            {
                if (decimal.Parse(keyForm.KeypadValue) < 0)
                {
                    MessageBox.Show("折扣率不能小于零！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    decimal discountRate = 1 - decimal.Parse(keyForm.KeypadValue) / 100;
                    if (discountRate > ConstantValuePool.CurrentEmployee.MinDiscount)
                    {
                        MessageBox.Show("当前已超过您的最低折扣！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    this.txtDiscount.Text = string.Format("折扣率({0}%)", keyForm.KeypadValue);
                    Discount discount = new Discount();
                    discount.DiscountID = new Guid("88888888-8888-8888-8888-888888888888");
                    discount.DiscountName = "自定义折扣";
                    discount.DiscountName2nd = "自定义折扣";
                    discount.DiscountType = (int)DiscountItemType.DiscountRate;
                    discount.DiscountRate = discountRate;
                    m_DiscountItem = discount;
                }
            }
        }

        private void btnCustomFixedAmount_Click(object sender, EventArgs e)
        {
            //权限验证
            bool hasRights = false;
            if (RightsItemCode.FindRights(RightsItemCode.CUSTOMDISCOUNT))
            {
                hasRights = true;
            }
            else
            {
                FormRightsCode form = new FormRightsCode();
                form.ShowDialog();
                if (form.ReturnValue)
                {
                    IList<string> rightsCodeList = form.RightsCodeList;
                    if (RightsItemCode.FindRights(rightsCodeList, RightsItemCode.CUSTOMDISCOUNT))
                    {
                        hasRights = true;
                    }
                }
            }
            if (!hasRights)
            {
                return;
            }
            FormNumericKeypad keyForm = new FormNumericKeypad();
            keyForm.DisplayText = "请输入自定义折扣金额";
            keyForm.ShowDialog();
            if (!string.IsNullOrEmpty(keyForm.KeypadValue))
            {
                if (decimal.Parse(keyForm.KeypadValue) < 0 || decimal.Parse(keyForm.KeypadValue) > m_ActualSellPrice)
                {
                    MessageBox.Show("折扣的金额不能小于零或者超过原始金额！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    decimal discountRate = decimal.Parse(keyForm.KeypadValue) / m_ActualSellPrice;
                    if (discountRate > ConstantValuePool.CurrentEmployee.MinDiscount)
                    {
                        MessageBox.Show("当前已超过您的最低折扣！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    this.txtDiscount.Text = string.Format("折扣金额({0}元)", keyForm.KeypadValue);
                    Discount discount = new Discount();
                    discount.DiscountID = new Guid("88888888-8888-8888-8888-888888888888");
                    discount.DiscountName = "自定义折扣";
                    discount.DiscountName2nd = "自定义折扣";
                    discount.DiscountType = (int)DiscountItemType.OffFixPay;
                    discount.OffFixPay = decimal.Parse(keyForm.KeypadValue);
                    m_DiscountItem = discount;
                }
            }
        }

        private void btnCustomDiscountTo_Click(object sender, EventArgs e)
        {
            //权限验证
            bool hasRights = false;
            if (RightsItemCode.FindRights(RightsItemCode.CUSTOMDISCOUNT))
            {
                hasRights = true;
            }
            else
            {
                FormRightsCode form = new FormRightsCode();
                form.ShowDialog();
                if (form.ReturnValue)
                {
                    IList<string> rightsCodeList = form.RightsCodeList;
                    if (RightsItemCode.FindRights(rightsCodeList, RightsItemCode.CUSTOMDISCOUNT))
                    {
                        hasRights = true;
                    }
                }
            }
            if (!hasRights)
            {
                return;
            }
            FormNumericKeypad keyForm = new FormNumericKeypad();
            keyForm.DisplayText = "请输入打折之后整单的金额";
            keyForm.ShowDialog();
            if (!string.IsNullOrEmpty(keyForm.KeypadValue))
            {
                if (decimal.Parse(keyForm.KeypadValue) < 0 || decimal.Parse(keyForm.KeypadValue) > m_ActualSellPrice)
                {
                    MessageBox.Show("折扣的金额不能小于零或者超过原始金额！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    decimal discountRate = (m_ActualSellPrice - decimal.Parse(keyForm.KeypadValue)) / m_ActualSellPrice;
                    if (discountRate > ConstantValuePool.CurrentEmployee.MinDiscount)
                    {
                        MessageBox.Show("当前已超过您的最低折扣！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    this.txtDiscount.Text = string.Format("打折后整单金额({0}元)", keyForm.KeypadValue);
                    Discount discount = new Discount();
                    discount.DiscountID = new Guid("88888888-8888-8888-8888-888888888888");
                    discount.DiscountName = "自定义折扣";
                    discount.DiscountName2nd = "自定义折扣";
                    discount.DiscountType = (int)DiscountItemType.OffFixPay;
                    discount.OffFixPay = m_ActualSellPrice - decimal.Parse(keyForm.KeypadValue);
                    m_DiscountItem = discount;
                }
            }
        }
    }
}
