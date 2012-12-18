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
    public partial class FormReason : Form
    {
        private string m_ItemName = string.Empty; //品项名称
        private decimal m_ItemNum = 0; //原始数量
        private decimal m_DelItemNum = 0; //删除数量
        private Reason m_CurrentReason;
        private CrystalButton m_PreviousBtn;
        /// <summary>
        /// 1：单品删除， 2：整单删除
        /// </summary>
        private int m_ReasonType = 1;

        #region Output
        public decimal DelItemNum
        {
            get { return m_DelItemNum; }
        }

        public Reason CurrentReason
        {
            get { return m_CurrentReason; }
        }
        #endregion

        public FormReason()
        {
            InitializeComponent();
            this.label1.Text = "请选择取消账单的理由：";
            this.txtItemNun.Visible = false;
            this.btnNumber.Visible = false;
            this.pnlReason.Location = new Point(this.pnlReason.Location.X, this.pnlReason.Location.Y - 30);
            this.pnlReason.Height = this.pnlReason.Height + 30;
            m_ReasonType = 2;
        }

        public FormReason(string itemName, decimal itemNum)
        {
            m_ItemName = itemName;
            m_ItemNum = itemNum;
            m_ReasonType = 1;
            InitializeComponent();
            if (itemNum == 1M)
            {
                m_DelItemNum = itemNum;
                this.txtItemNun.Text = itemName + "*" + itemNum.ToString("f1");
            }
        }

        private void FormReason_Load(object sender, EventArgs e)
        {
            InitializeReasonButton();
        }

        private void InitializeReasonButton()
        {
            int reasonCount = 0;
            foreach (Reason item in ConstantValuePool.ReasonList)
            {
                if (item.ReasonType == (int)ReasonItemType.DeleteItem)
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
                    if (item.ReasonType == (int)ReasonItemType.DeleteItem)
                    {
                        btn = new CrystalButton();
                        btn.Name = item.ReasonID.ToString();
                        btn.Tag = item;
                        btn.Text = item.ReasonName;
                        btn.Width = width;
                        btn.Height = height;
                        btn.Location = new Point(px, py);
                        btn.BackColor = Color.Blue;
                        btn.DisplayColor = btn.BackColor;
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

        private void btnNumber_Click(object sender, EventArgs e)
        {
            FormNumericKeypad keyForm = new FormNumericKeypad();
            keyForm.DisplayText = "请输入删除数量";
            keyForm.ShowDialog();
            if (!string.IsNullOrEmpty(keyForm.KeypadValue))
            {
                if (decimal.Parse(keyForm.KeypadValue) <= m_ItemNum && decimal.Parse(keyForm.KeypadValue) > 0)
                {
                    m_DelItemNum = decimal.Parse(keyForm.KeypadValue);
                    this.txtItemNun.Text = m_ItemName + "*" + m_DelItemNum.ToString("f1");
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (m_ReasonType == 1)
            {
                if (m_DelItemNum > 0 && m_CurrentReason != null)
                {
                    this.Close();
                }
            }
            if (m_ReasonType == 2)
            {
                if (m_CurrentReason != null)
                {
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_DelItemNum = 0;
            m_CurrentReason = null;
            this.Close();
        }        
    }
}
