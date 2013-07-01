using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.CustomControl;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Entity;

namespace Top4ever.Pos.Feature
{
    public partial class FormGoodsSetMeal : Form
    {
        private Dictionary<int, List<GoodsSetMeal>> dicGoodsSetMealByGroup;
        private List<GoodsSetMeal> m_CurrentGoodsSetMealList;
        private int ItemPageIndex = 0;

        private SortedDictionary<int, List<GoodsSetMeal>> _dicResultGoodsSetMeal = new SortedDictionary<int, List<GoodsSetMeal>>();
        public SortedDictionary<int, List<GoodsSetMeal>> DicResultGoodsSetMeal
        {
            get { return _dicResultGoodsSetMeal; }
            set { _dicResultGoodsSetMeal = value; }
        }

        public FormGoodsSetMeal(Dictionary<int, List<GoodsSetMeal>> dicGoodsSetMealByGroup)
        {
            this.dicGoodsSetMealByGroup = dicGoodsSetMealByGroup;
            InitializeComponent();
            btnPageUp.DisplayColor = btnPageUp.BackColor;
            btnPageDown.DisplayColor = btnPageDown.BackColor;
            btnPageUp.Enabled = false;
            btnPageUp.BackColor = ConstantValuePool.DisabledColor;
            btnPageDown.Enabled = false;
            btnPageDown.BackColor = ConstantValuePool.DisabledColor;
        }

        private void FormGoodsSetMeal_Load(object sender, EventArgs e)
        {
            DisplayGroupButton();
            foreach (KeyValuePair<int, List<GoodsSetMeal>> item in dicGoodsSetMealByGroup)
            {
                m_CurrentGoodsSetMealList = item.Value;
                break;
            }
            DisplayGoodsSetMealButton();
            if (ItemPageIndex <= 0)
            {
                ItemPageIndex = 0;
                btnPageUp.Enabled = false;
                btnPageUp.BackColor = ConstantValuePool.DisabledColor;
            }
            if (m_CurrentGoodsSetMealList.Count > 18 * (ItemPageIndex + 1))
            {
                btnPageDown.Enabled = true;
                btnPageDown.BackColor = btnPageDown.DisplayColor;
            }
        }

        private void DisplayGroupButton()
        {
            pnlGroup.Controls.Clear();
            int space = 3;
            int width = (pnlGroup.Width - (dicGoodsSetMealByGroup.Count - 1) * space) / dicGoodsSetMealByGroup.Count;
            int height = pnlGroup.Height - space;
            int px = 0, py = 0;
            foreach (KeyValuePair<int, List<GoodsSetMeal>> item in dicGoodsSetMealByGroup)
            {
                CrystalButton btn = new CrystalButton();
                btn.Name = "button" + item.Key;
                btn.Text = item.Value[0].GroupName;
                btn.Width = width;
                btn.Height = height;
                btn.Location = new Point(px, py);
                btn.ForeColor = Color.White;
                btn.BackColor = Color.DodgerBlue;
                btn.Tag = item.Value;
                btn.Click += new EventHandler(btnGroup_Click);
                pnlGroup.Controls.Add(btn);
                px += width + space;
            }
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            CrystalButton btnGroup = sender as CrystalButton;
            List<GoodsSetMeal> temp = btnGroup.Tag as List<GoodsSetMeal>;
            if (temp != null)
            {
                m_CurrentGoodsSetMealList = temp;
            }
            DisplayGoodsSetMealButton();
            if (ItemPageIndex <= 0)
            {
                ItemPageIndex = 0;
                btnPageUp.Enabled = false;
                btnPageUp.BackColor = ConstantValuePool.DisabledColor;
            }
            if (m_CurrentGoodsSetMealList.Count > 18 * (ItemPageIndex + 1))
            {
                btnPageDown.Enabled = true;
                btnPageDown.BackColor = btnPageDown.DisplayColor;
            }
        }

        private void DisplayGoodsSetMealButton()
        {
            pnlItem.Controls.Clear();
            int space = 3;
            int width = (pnlItem.Width - 4 * space) / 5;
            int height = (pnlItem.Height - 4 * space) / 4;
            int px = 0, py = 0;
            int i = 0;
            for (int index = ItemPageIndex * 18; index < m_CurrentGoodsSetMealList.Count; index++)
            {
                CrystalButton btn = new CrystalButton();
                btn.Text = m_CurrentGoodsSetMealList[index].GoodsName;
                btn.Width = width;
                btn.Height = height;
                btn.Location = new Point(px, py);
                btn.ForeColor = Color.White;
                btn.BackColor = btn.DisplayColor = Color.SeaGreen;
                if (IsGoodsSetMealItemInList(m_CurrentGoodsSetMealList[index]))
                {
                    btn.BackColor = Color.Teal;
                }
                btn.Tag = m_CurrentGoodsSetMealList[index];
                btn.Click += new EventHandler(btnItem_Click);
                pnlItem.Controls.Add(btn);
                i++;
                px += width + space;
                if (i % 5 == 0)
                {
                    px = 0;
                    py += height + space;
                }
            }
            btnPageUp.Width = width;
            btnPageUp.Height = height;
            btnPageUp.Location = new Point(3 * (width + space), 3 * (height + space));
            pnlItem.Controls.Add(btnPageUp);
            btnPageDown.Width = width;
            btnPageDown.Height = height;
            btnPageDown.Location = new Point(4 * (width + space), 3 * (height + space));
            pnlItem.Controls.Add(btnPageDown);
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            CrystalButton btnItem = sender as CrystalButton;
            GoodsSetMeal goodsSetMealItem = btnItem.Tag as GoodsSetMeal;
            if (goodsSetMealItem != null)
            {
                if (IsGoodsSetMealItemInList(goodsSetMealItem))
                {
                    btnItem.BackColor = btnItem.DisplayColor;
                    foreach (GoodsSetMeal item in _dicResultGoodsSetMeal[goodsSetMealItem.GroupNo])
                    {
                        if (item.GoodsID == goodsSetMealItem.GoodsID && item.GroupNo == goodsSetMealItem.GroupNo)
                        {
                            _dicResultGoodsSetMeal[goodsSetMealItem.GroupNo].Remove(item);
                            break;
                        }
                    }
                }
                else
                {
                    btnItem.BackColor = Color.Teal;
                    if (_dicResultGoodsSetMeal.ContainsKey(goodsSetMealItem.GroupNo))
                    {
                        _dicResultGoodsSetMeal[goodsSetMealItem.GroupNo].Add(goodsSetMealItem);
                    }
                    else
                    {
                        List<GoodsSetMeal> goodsSubItemList = new List<GoodsSetMeal>();
                        goodsSubItemList.Add(goodsSetMealItem);
                        _dicResultGoodsSetMeal.Add(goodsSetMealItem.GroupNo, goodsSubItemList);
                    }
                }
            }
        }

        private bool IsGoodsSetMealItemInList(GoodsSetMeal goodsSetMealItem)
        {
            bool IsContains = false;
            if (_dicResultGoodsSetMeal.ContainsKey(goodsSetMealItem.GroupNo))
            {
                foreach (GoodsSetMeal item in _dicResultGoodsSetMeal[goodsSetMealItem.GroupNo])
                {
                    if (item.GoodsID == goodsSetMealItem.GoodsID && item.GroupNo == goodsSetMealItem.GroupNo)
                    {
                        IsContains = true;
                        break;
                    }
                }
            }
            return IsContains;
        }

        private void btnPageUp_Click(object sender, EventArgs e)
        {
            ItemPageIndex--;
            DisplayGoodsSetMealButton();
            if (ItemPageIndex <= 0)
            {
                ItemPageIndex = 0;
                btnPageUp.Enabled = false;
                btnPageUp.BackColor = ConstantValuePool.DisabledColor;
            }
            if (m_CurrentGoodsSetMealList.Count > 18 * (ItemPageIndex + 1))
            {
                btnPageDown.Enabled = true;
                btnPageDown.BackColor = btnPageDown.DisplayColor;
            }
        }

        private void btnPageDown_Click(object sender, EventArgs e)
        {
            ItemPageIndex++;
            DisplayGoodsSetMealButton();
            if (ItemPageIndex > 0)
            {
                btnPageUp.Enabled = true;
                btnPageUp.BackColor = btnPageUp.DisplayColor;
            }
            if (m_CurrentGoodsSetMealList.Count <= 18 * (ItemPageIndex + 1))
            {
                btnPageDown.Enabled = false;
                btnPageDown.BackColor = ConstantValuePool.DisabledColor;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //验证是否满足条件
            foreach (KeyValuePair<int, List<GoodsSetMeal>> item in dicGoodsSetMealByGroup)
            {
                if (item.Value[0].IsRequired)
                {
                    if (!_dicResultGoodsSetMeal.ContainsKey(item.Key) || (int)item.Value[0].LimitedQty != _dicResultGoodsSetMeal[item.Key].Count)
                    {
                        MessageBox.Show(string.Format("{0}数量为'{1}'的条件不满足", item.Value[0].GroupName, item.Value[0].LimitedQty), "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    //可选如果已经选择，则必须满足数量条件
                    if (_dicResultGoodsSetMeal.ContainsKey(item.Key) && _dicResultGoodsSetMeal[item.Key].Count > 0)
                    {
                        if ((int)item.Value[0].LimitedQty != _dicResultGoodsSetMeal[item.Key].Count)
                        {
                            MessageBox.Show(string.Format("{0}数量为'{1}'的条件不满足", item.Value[0].GroupName, item.Value[0].LimitedQty), "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _dicResultGoodsSetMeal.Clear();
            this.Close();
        }
    }
}
