using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Top4ever.CustomControl;
using Top4ever.Domain;
using Top4ever.Domain.Accounts;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Domain.OrderRelated;
using Top4ever.Entity.Config;
using Top4ever.Entity.Enum;
using Top4ever.Print;
using Top4ever.Domain.Promotions;

namespace Top4ever.Entity
{
    public class ConstantValuePool
    {
        private static Color _PressedColor = Color.Black;
        private static Color _DisabledColor = Color.Gray;

        /// <summary>
        /// 按钮选择时的颜色
        /// </summary>
        public static Color PressedColor 
        {
            get { return _PressedColor; }
        }

        /// <summary>
        /// 禁止的颜色
        /// </summary>
        public static Color DisabledColor 
        {
            get { return _DisabledColor; }
        }
        /// <summary>
        /// 当前语言
        /// </summary>
        public static LanguageType CurrentLanguage { get; set; }

        public static Employee CurrentEmployee { get; set; }

        public static Shop CurrentShop { get; set; }

        public static SystemConfig SysConfig { get; set; }

        public static IList<Notice> NoticeList { get; set; }

        public static IList<BizRegion> RegionList { get; set; }

        public static IList<Discount> DiscountList { get; set; }

        public static IList<PayoffWay> PayoffWayList { get; set; }

        public static IList<Reason> ReasonList { get; set; }

        public static IList<GoodsGroup> GoodsGroupList { get; set; }

        public static IList<DetailsGroup> DetailsGroupList { get; set; }

        public static IList<GoodsSetMeal> GoodsSetMealList { get; set; }

        public static IList<GoodsSetMeal> DetailsSetMealList { get; set; }

        public static IList<GoodsCronTrigger> GoodsCronTriggerList { get; set; }

        public static IList<ButtonStyle> ButtonStyleList { get; set; }

        public static IList<Promotion> PromotionList { get; set; }

        public static IList<PromotionCondition> PromotionConditionList { get; set; }

        public static IList<PromotionCronTrigger> PromotionCronTriggerList { get; set; }

        public static IList<PromotionPresent> PromotionPresentList { get; set; }
        /// <summary>
        /// 品项组限时特价
        /// </summary>
        public static IList<GoodsLimitedTimeSale> GroupLimitedTimeSaleList { get; set; }
        /// <summary>
        /// 品项限时特价
        /// </summary>
        public static IList<GoodsLimitedTimeSale> GoodsLimitedTimeSaleList { get; set; }
        /// <summary>
        /// 品项组组合销售
        /// </summary>
        public static IList<GoodsCombinedSale> GroupCombinedSaleList { get; set; }
        /// <summary>
        /// 品项组合销售
        /// </summary>
        public static IList<GoodsCombinedSale> GoodsCombinedSaleList { get; set; }

        /// <summary>
        /// 程序的配置
        /// </summary>
        public static AppSettingConfig BizSettingConfig { get; set; }
        /// <summary>
        /// 来电宝编号
        /// </summary>
        public static int TelCallID { get; set; }
        /// <summary>
        /// 来电宝是否正常工作
        /// </summary>
        public static bool IsTelCallWorking { get; set; }

        /// <summary>
        /// 第二屏对象
        /// </summary>
        public static Form SecondScreenForm { get; set; }
        /// <summary>
        /// 桌况对象
        /// </summary>
        public static Form DeskForm { get; set; }
    }
}
