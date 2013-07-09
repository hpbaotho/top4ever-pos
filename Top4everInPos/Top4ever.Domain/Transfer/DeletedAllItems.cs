using System;
using System.Collections.Generic;

namespace Top4ever.Domain.Transfer
{
    public class DeletedAllItems
    {
        /// <summary>
        /// 整单删除品项
        /// </summary>
        public IList<DeletedItem> DeletedOrderItemList { get; set; }
        /// <summary>
        /// 单品删除品项
        /// </summary>
        public IList<DeletedItem> DeletedGoodsItemList { get; set; }
    }
}
