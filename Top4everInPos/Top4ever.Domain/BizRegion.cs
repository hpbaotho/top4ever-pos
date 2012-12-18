using System;
using System.Collections.Generic;

namespace Top4ever.Domain
{
    public class BizRegion
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid RegionID { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// px
        /// </summary>
        public int PX { get; set; }
        /// <summary>
        /// py
        /// </summary>
        public int PY { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 外键
        /// </summary>
        public Guid ButtonStyleID { get; set; }
        /// <summary>
        /// 桌号信息
        /// </summary>
        public IList<BizDesk> BizDeskList { get; set; }
    }
}
