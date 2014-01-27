using System;
using System.Collections.Generic;
using System.Text;

namespace Top4ever.Print.Entity
{
    public class GoodsOrder
    {
        /// <summary>
        /// 菜品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string GoodsNum { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 单位售价
        /// </summary>
        public string SellPrice { get; set; }

        /// <summary>
        /// 售价总计
        /// </summary>
        public string TotalSellPrice { get; set; }

        /// <summary>
        /// 折扣总计
        /// </summary>
        public string TotalDiscount { get; set; }

        /// <summary>
        /// 得到属性值
        /// </summary>
        /// <param name="fieldName">属性字符</param>
        /// <returns></returns>
        public string GetValue(string fieldName)
        {
            string result;
            switch (fieldName.ToLower())
            {
                case "goodsname":
                    result = this.GoodsName;
                    break;
                case "goodsnum":
                    result = this.GoodsNum;
                    break;
                case "unit":
                    result = this.Unit;
                    break;
                case "sellprice":
                    result = this.SellPrice;
                    break;
                case "totalsellprice":
                    result = this.TotalSellPrice;
                    break;
                case "totaldiscount":
                    result = this.TotalDiscount;
                    break;
                default:
                    result = string.Empty;
                    break;
            }
            return result;
        }
    }
}
