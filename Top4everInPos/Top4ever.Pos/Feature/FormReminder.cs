using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.Common;
using Top4ever.CustomControl;
using Top4ever.Domain;
using Top4ever.Domain.OrderRelated;
using Top4ever.Entity;
using Top4ever.Entity.Enum;

namespace VechsoftPos.Feature
{
    public partial class FormReminder : Form
    {
        private Reason m_CurrentReason;
        private CrystalButton m_PreviousBtn;

        #region Output
        public Reason CurrentReason
        {
            get { return m_CurrentReason; }
        }
        #endregion

        public FormReminder()
        {
            InitializeComponent();
        }

        private void FormReminder_Load(object sender, EventArgs e)
        {
            InitializeReasonButton();
        }

        private void InitializeReasonButton()
        {
            int reasonCount = 0;
            foreach (Reason item in ConstantValuePool.ReasonList)
            {
                if (item.ReasonType == (int)ReasonItemType.Reminder)
                {
                    reasonCount++;
                }
            }
            if (reasonCount > 0)
            {
                //判断每行显示多少列
                int maxColumn = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(reasonCount)));
                if (maxColumn * maxColumn < reasonCount)
                {
                    maxColumn++;
                }
                //显示多少行
                int perLine = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(reasonCount) / maxColumn));
                //计算button长宽
                int space = 6;
                int width = (this.pnlReason.Width - space * (maxColumn - 1)) / maxColumn;
                int height = (this.pnlReason.Height - space * (perLine - 1)) / perLine;

                int count = 1;
                int px = 0, py = 0;
                CrystalButton btn;
                foreach (Reason item in ConstantValuePool.ReasonList)
                {
                    if (item.ReasonType == (int)ReasonItemType.Reminder)
                    {
                        btn = new CrystalButton();
                        btn.Name = item.ReasonID.ToString();
                        btn.Tag = item;
                        btn.Text = item.ReasonName;
                        btn.Width = width;
                        btn.Height = height;
                        btn.Location = new Point(px, py);
                        foreach (ButtonStyle btnStyle in ConstantValuePool.ButtonStyleList)
                        {
                            if (item.ButtonStyleID.Equals(btnStyle.ButtonStyleID))
                            {
                                float emSize = (float)btnStyle.FontSize;
                                FontStyle style = FontStyle.Regular;
                                btn.Font = new Font(btnStyle.FontName, emSize, style);
                                btn.ForeColor = ColorConvert.RGB(btnStyle.ForeColor);
                                btn.BackColor = btn.DisplayColor = ColorConvert.RGB(btnStyle.BackColor);
                                break;
                            }
                        }
                        btn.Click += new System.EventHandler(this.btnReason_Click);
                        this.pnlReason.Controls.Add(btn);

                        count++;
                        if (count > maxColumn)
                        {
                            px = 0;
                            py += height + space;
                            count = 1;
                        }
                        else
                        {
                            px += width + space;
                        }
                    }
                }
            }
        }

        private void btnReason_Click(object sender, EventArgs e)
        {
            CrystalButton btn = sender as CrystalButton;
            m_CurrentReason = btn.Tag as Reason;
            this.txtReason.Text = m_CurrentReason.ReasonName;
            if (m_PreviousBtn == null)
            {
                btn.BackColor = ConstantValuePool.PressedColor;
                m_PreviousBtn = btn;
            }
            else
            {
                m_PreviousBtn.BackColor = m_PreviousBtn.DisplayColor;
                btn.BackColor = ConstantValuePool.PressedColor;
                m_PreviousBtn = btn;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (m_CurrentReason != null)
            {
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_CurrentReason = null;
            this.Close();
        }
    }
}
