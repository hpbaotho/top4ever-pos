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

        public static IList<BizRegion> RegionList { get; set; }

        public static IList<Discount> DiscountList { get; set; }

        public static IList<PayoffWay> PayoffWayList { get; set; }

        public static IList<Reason> ReasonList { get; set; }

        public static IList<GoodsGroup> GoodsGroupList { get; set; }

        public static IList<DetailsGroup> DetailsGroupList { get; set; }

        public static IList<GoodsSetMeal> GoodsSetMealList { get; set; }

        public static IList<GoodsSetMeal> DetailsSetMealList { get; set; }

        public static IList<ButtonStyle> ButtonStyleList { get; set; }

        /// <summary>
        /// 菜品组按钮
        /// </summary>
        public static IList<CrystalButton> GoodsGroupButton { get; set; }
        /// <summary>
        /// 菜品按钮
        /// </summary>
        public static Dictionary<string, List<CrystalButton>> DicGoodsButton { get; set; }
        /// <summary>
        /// 细项按钮
        /// </summary>
        public static Dictionary<string, List<CrystalButton>> DicDetailsButton { get; set; }

        /// <summary>
        /// 外卖菜品组按钮
        /// </summary>
        public static IList<CrystalButton> GoodsGroupButtonList { get; set; }
        /// <summary>
        /// 外卖菜品按钮
        /// </summary>
        public static Dictionary<string, List<CrystalButton>> DicGoodsButtonList { get; set; }
        /// <summary>
        /// 外卖细项按钮
        /// </summary>
        public static Dictionary<string, List<CrystalButton>> DicDetailsButtonList { get; set; }

        /// <summary>
        /// 程序的配置
        /// </summary>
        public static AppSettingConfig BizSettingConfig { get; set; }

        /// <summary>
        /// 桌况对象
        /// </summary>
        public static Form DeskForm { get; set; }
    }
}
