using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Top4ever.ClientService;
using Top4ever.CustomControl;
using Top4ever.Domain.Customers;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Entity;

namespace Top4ever.Pos.TakeawayCall
{
    public partial class FormHistoryCallOrder : Form
    {
        private string m_Telephone;
        private Action<object, EventArgs> m_OrderAction;
        private GoodsGroup m_GoodsGroup;

        public FormHistoryCallOrder(string telephone, Action<object, EventArgs> orderAction, ref GoodsGroup currentGoodsGroup)
        {
            m_Telephone = telephone;
            m_OrderAction = orderAction;
            m_GoodsGroup = currentGoodsGroup;
            InitializeComponent();
        }

        private void FormHistoryCallOrder_Load(object sender, EventArgs e)
        {
            this.lbTitle.Text = string.Format("电话为 {0} 的历史排行前10位订餐", m_Telephone);
            IList<TopSellGoods> topSellGoodsList = CustomersService.GetInstance().GetTopSellGoods(m_Telephone);
            if (topSellGoodsList != null && topSellGoodsList.Count > 0)
            {
                foreach (TopSellGoods item in topSellGoodsList)
                {
                    int index = dataGirdViewExt1.Rows.Add();
                    dataGirdViewExt1.Rows[index].Cells[0].Value = item.GoodsName;
                    dataGirdViewExt1.Rows[index].Cells[1].Value = item.SellPrice;
                    dataGirdViewExt1.Rows[index].Cells[2].Value = item.Times;
                    dataGirdViewExt1.Rows[index].Cells[3].Value = item.GoodsID;
                    dataGirdViewExt1.Rows[index].Cells[4].Value = item.CanDiscount;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGirdViewExt1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGirdViewExt1.CurrentRow != null)
            {
                int selectIndex = dataGirdViewExt1.CurrentRow.Index;
                Guid goodsID = new Guid(dataGirdViewExt1.Rows[selectIndex].Cells["colGoodsID"].Value.ToString());
                Goods goods = null;
                foreach (GoodsGroup goodsGroup in ConstantValuePool.GoodsGroupList)
                {
                    if (goodsGroup.GoodsList != null)
                    {
                        foreach (Goods item in goodsGroup.GoodsList)
                        {
                            if (item.GoodsID == goodsID)
                            {
                                //修改当前品项组的值，为了读取可能存在的折扣
                                m_GoodsGroup.GoodsGroupID = goodsGroup.GoodsGroupID;
                                m_GoodsGroup.GoodsGroupNo = goodsGroup.GoodsGroupNo;
                                m_GoodsGroup.GoodsGroupName = goodsGroup.GoodsGroupName;
                                m_GoodsGroup.GoodsGroupName2nd = goodsGroup.GoodsGroupName2nd;
                                m_GoodsGroup.ButtonStyleID = goodsGroup.ButtonStyleID;
                                m_GoodsGroup.GoodsList = goodsGroup.GoodsList;
                                goods = item;
                                break;
                            }
                        }
                        if (goods != null)
                        {
                            break;
                        }
                    }
                }
                CrystalButton btn = new CrystalButton();
                btn.Tag = goods;
                if (m_OrderAction != null)
                {
                    m_OrderAction(btn, null);
                }
            }
        }
    }
}
