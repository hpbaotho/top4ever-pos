using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Top4ever.Domain.OrderRelated;

namespace Top4ever.Pos.Promotions
{
    public abstract class PromotionPresentService
    {
        public abstract void GetPromotionPresents(DataGridView dgvGoodsOrder);
    }
}
