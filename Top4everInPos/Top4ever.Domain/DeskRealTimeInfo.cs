using System;
using System.Collections.Generic;

namespace Top4ever.Domain
{
    public class DeskRealTimeInfo
    {
        /// <summary>
        /// 桌号
        /// </summary>
        public string DeskName { get; set; }
        /// <summary>
        /// 状态 0：空闲，1：已占，2：WP7占用
        /// </summary>
        public int DeskStatus { get; set; }
        /// <summary>
        /// 占用的设备编号
        /// </summary>
        public string DeviceNo { get; set; }
        /// <summary>
        /// 是否分单
        /// </summary>
        public bool IsSplitOrder { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNum { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal ConsumptionMoney { get; set; }
    }
}
