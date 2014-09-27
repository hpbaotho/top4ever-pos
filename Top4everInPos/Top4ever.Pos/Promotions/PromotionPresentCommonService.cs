using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Top4ever.Domain.GoodsRelated;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Promotions;
using Top4ever.Entity;
using Top4ever.Entity.Enum;

namespace VechsoftPos.Promotions
{
    public class PromotionPresentCommonService : PromotionPresentService
    {
        private decimal m_TotalMoney = 0M;
        private PromotionPresent m_ActivePromotionPresent;
        private decimal m_MaxAmount = 0M;

        public PromotionPresentCommonService(decimal totalMoney, decimal totalQuantity, IList<PromotionPresent> promotionPresentList)
        {
            m_TotalMoney = totalMoney;
            //获得满足条件的最大额度的金额
            foreach (PromotionPresent item in promotionPresentList)
            {
                if (item.TotalMoney <= totalMoney && item.TotalQuantity <= totalQuantity)
                {
                    if (item.TotalMoney > m_MaxAmount)
                    {
                        m_MaxAmount = item.TotalMoney;
                        m_ActivePromotionPresent = item;
                    }
                }
            }
        }

        public override void GetPromotionPresents(DataGridView dgvGoodsOrder)
        {
            if (m_ActivePromotionPresent.Classification == 1)
            {
                //赠品
                decimal giftNum = 0;
                if (m_ActivePromotionPresent.IsMultiple)
                {
                    giftNum = Math.Floor(m_TotalMoney / m_MaxAmount) * (decimal)m_ActivePromotionPresent.Quantity;
                }
                else
                {
                    giftNum = (decimal)m_ActivePromotionPresent.Quantity;
                }
                Goods goods = new Goods();
                goods.GoodsID = (Guid)m_ActivePromotionPresent.GoodsID;
                goods.GoodsNo = m_ActivePromotionPresent.GoodsNo;
                goods.GoodsName = goods.GoodsName2nd = m_ActivePromotionPresent.GoodsName;
                goods.Unit = m_ActivePromotionPresent.Unit;
                goods.SellPrice = (decimal)m_ActivePromotionPresent.SellPrice;
                goods.CanDiscount = false;
                goods.AutoShowDetails = false;
                goods.PrintSolutionName = m_ActivePromotionPresent.PrintSolutionName;
                goods.DepartID = (Guid)m_ActivePromotionPresent.DepartID;
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
            if (m_ActivePromotionPresent.Classification == 2)
            {
                //折扣率
                Discount discount = new Discount();
                discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                discount.DiscountName = "促销折扣";
                discount.DiscountType = (int)DiscountItemType.DiscountRate;
                discount.DiscountRate = (decimal)m_ActivePromotionPresent.DiscountRate;
                discount.OffFixPay = 0;
                decimal limitCount = 0;
                foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
                {
                    decimal goodsDiscount = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value);
                    if (goodsDiscount == 0)
                    {
                        dr.Cells["GoodsDiscount"].Value = -Convert.ToDecimal(dr.Cells["GoodsPrice"].Value) * m_ActivePromotionPresent.DiscountRate;
                        dr.Cells["GoodsDiscount"].Tag = discount;
                        limitCount += Convert.ToDecimal(dr.Cells["GoodsNum"].Value);
                    }
                    if (limitCount >= m_ActivePromotionPresent.DiscountLimit) break;
                }
            }
            if (m_ActivePromotionPresent.Classification == 3)
            {
                //固定折扣
                Discount discount = new Discount();
                discount.DiscountID = new Guid("66666666-6666-6666-6666-666666666666");
                discount.DiscountName = "促销折扣";
                discount.DiscountType = (int)DiscountItemType.OffFixPay;
                discount.DiscountRate = 0;
                discount.OffFixPay = (decimal)m_ActivePromotionPresent.OffFixPay;
                decimal limitCount = 0;
                foreach (DataGridViewRow dr in dgvGoodsOrder.Rows)
                {
                    decimal goodsDiscount = Convert.ToDecimal(dr.Cells["GoodsDiscount"].Value);
                    if (goodsDiscount == 0)
                    {
                        dr.Cells["GoodsDiscount"].Value = -m_ActivePromotionPresent.OffFixPay;
                        dr.Cells["GoodsDiscount"].Tag = discount;
                        limitCount += Convert.ToDecimal(dr.Cells["GoodsNum"].Value);
                    }
                    if (limitCount >= m_ActivePromotionPresent.DiscountLimit) break;
                }
            }
        }
    }
}
