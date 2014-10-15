using System;

namespace Top4ever.Domain.GoodsRelated
{
    /// <summary>
    /// 品项与细项组关系
    /// </summary>
    [Serializable]
    public class GoodsDetailsGroup
    {
        public Guid GoodsID { get; set; }

        public Guid DetailsGroupID { get; set; }
    }
}
