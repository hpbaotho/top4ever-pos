using System;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Domain.GoodsRelated;
using Top4ever.Domain.OrderRelated;

namespace Top4ever.Domain.Transfer
{
    public class SysBasicData
    {
        public Shop CurrentShop { get; set; }

        public IList<BizRegion> RegionList { get; set; }

        public IList<Discount> DiscountList { get; set; }

        public IList<PayoffWay> PayoffWayList { get; set; }

        public IList<Reason> ReasonList { get; set; }

        public IList<GoodsGroup> GoodsGroupList { get; set; }

        public IList<DetailsGroup> DetailsGroupList { get; set; }

        public IList<ButtonStyle> ButtonStyleList { get; set; }

        public SystemConfig SysConfig { get; set; }
    }
}
