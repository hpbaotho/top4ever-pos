using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Promotions;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Entity.Enum;

namespace VechsoftPos.Promotions
{
    public class PromotionPresentGiftService : PromotionPresentService
    {
        private decimal m_TotalMoney = 0M;
        private decimal m_TotalQuantity = 0M;
        private IList<PromotionPresent> m_PromotionPresentList;

        public PromotionPresentGiftService(decimal totalMoney, decimal totalQuantity, IList<PromotionPresent> promotionPresentList)
        {
            m_TotalMoney = totalMoney;
            m_TotalQuantity = totalQuantity;
            m_PromotionPresentList = promotionPresentList;
        }

        public override void GetPromotionPresents(DataGridView dgvGoodsOrder)
        {
            foreach (PromotionPresent item in m_PromotionPresentList)
            {
                if (item.TotalMoney <= m_TotalMoney && item.TotalQuantity <= m_TotalQuantity)
                {
                    if (item.Classification == 1)
                    {
                        //赠品
                        decimal giftNum = 0;
                        if (item.IsMultiple)
                        {
                            giftNum = Math.Floor(m_TotalMoney / item.TotalMoney) * (decimal)item.Quantity;
                        }
                        else
                        {
                            giftNum = (decimal)item.Quantity;
                        }
                        Goods goods = new Goods();
                        goods.GoodsID = (Guid)item.GoodsID;
                        goods.GoodsNo = item.GoodsNo;
                        goods.GoodsName = goods.GoodsName2nd = item.GoodsName;
                        goods.Unit = item.Unit;
                        goods.SellPrice = (decimal)item.SellPrice;
                        goods.CanDiscount = false;
                        goods.AutoShowDetails = false;
                        goods.PrintSolutionName = item.PrintSolutionName;
                        goods.DepartID = (Guid)item.DepartID;
                        int index = dgvGoodsOrder.Rows.Add(new DataGridViewRow());
                        dgvGoodsOrder.Rows[index].Cells["ItemID"].Value = goods.GoodsID;
                        dgvGoodsOrder.Rows[index].Cells["ItemID"].Tag = goods;
                        dgvGoodsOrder.Rows[index].Cells["GoodsNum"].Value = giftNum;
                        dgvGoodsOrder.Rows[index].Cells["GoodsName"].Value = goods.GoodsName;
                        dgvGoodsOrder.Rows[index].Cells["GoodsPrice"].Value = goods.SellPrice;
                        dgvGoodsOrder.Rows[index].Cells["GoodsDiscount"].Value = 0;
                        dgvGoodsOrder.Rows[index].Cells["ItemType"].Value = OrderItemType.Goods;
                        dgvGoodsOrder.Rows[index].Cells["CanDiscount"].Value = goods.CanDiscount;
                        dgvGoodsOrder.Rows[index].Cells["ItemUnit"].Value = goods.Unit;
                    }
                    //单选和多选只能是赠品
                }
            }
        }
    }
}
