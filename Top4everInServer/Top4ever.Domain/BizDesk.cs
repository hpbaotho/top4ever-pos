using System;
using System.Collections.Generic;

namespace Top4ever.Domain
{
    public class BizDesk
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid DeskID { get; set; }
        /// <summary>
        /// 桌号
        /// </summary>
        public string DeskName { get; set; }
        /// <summary>
        /// 状态 0：空闲，1：已占，2：WP7占用
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 最低消费
        /// </summary>
        public decimal MinConsumption { get; set; }
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
        /// 占用的设备编号
        /// </summary>
        public string DeviceNo { get; set; }
        /// <summary>
        /// 桌区
        /// </summary>
        public Guid RegionID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
