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

        public Discount CurrentDiscount
        {
            get { return m_CurrentDiscount; }
        }

        public FormDiscount(DiscountDisplayModel displayModel)
        {
            InitializeComponent();
            //显示折扣方式
            int count = ConstantValuePool.DiscountList.Count;
            int maxColumn = (int)Math.Ceiling(Convert.ToDecimal(count) / 5);
            int maxRow = (int)Math.Ceiling(Convert.ToDecimal(count) / maxColumn);
            int space = 15;
            int width = (this.pnlDiscount.Width - (maxColumn - 1) * space) / maxColumn;
            int height = (this.pnlDiscount.Height - (maxRow - 1) * space) / maxRow;
            int px = 0, py = 0, index = 1;
            foreach (Discount item in ConstantValuePool.DiscountList)
            {
                if (item.DisplayModel == (int)DiscountDisplayModel.ALL || item.DisplayModel == (int)displayModel)
                {
                    bool hasRights = false;
                    if (item.DiscountType == (int)DiscountItemType.DiscountRate)
                    {
                        if (item.DiscountRate < ConstantValuePool.CurrentEmployee.MinDiscount)
                        {
                            hasRights = true;
                        }
                    }
                    else if (item.DiscountType == (int)DiscountItemType.OffFixPay)
                    {
                        if (item.OffFixPay / item.MinQuotas < ConstantValuePool.CurrentEmployee.MinDiscount)
                        {
                            hasRights = true;
                        }
                    }
                    if (!hasRights) continue;

                    CrystalButton btn = new CrystalButton();
                    btn.Name = item.DiscountID.ToString();
                    btn.Text = item.DiscountName;
                    btn.Width = width;
                    btn.Height = height;
                    btn.Location = new Point(px, py);
                    btn.Tag = item;
                    btn.Click += new System.EventHandler(this.btnDiscount_Click);
                    pnlDiscount.Controls.Add(btn);

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
    }
}
