using System;

namespace Top4ever.Domain
{
    public class SystemConfig
    {
        /// <summary>
        /// 是否去尾
        /// </summary>
        public bool IsCutTail { get; set; }
        /// <summary>
        /// 去尾类别
        /// </summary>
        public int CutTailType { get; set; }
        /// <summary>
        /// 去尾保留位数
        /// </summary>
        public int CutTailDigit { get; set; }
        /// <summary>
        /// 是否循环流水号
        /// </summary>
        public bool IsCycleTranSeq { get; set; }
        /// <summary>
        /// 起始流水号
        /// </summary>
        public int StartTranSeq { get; set; }
        /// <summary>
        /// 流水号间隔
        /// </summary>
        public int IntervalTranSeq { get; set; }
        /// <summary>
        /// 包含厨打
        /// </summary>
        public bool IncludeKitchenPrint { get; set; }
        /// <summary>
        /// 打印类型（1堂吃, 2外卖, 3堂吃兼外卖）
        /// </summary>
        public int PrintStyle { get; set; }
        /// <summary>
        /// 跟随类型（1细跟主, 2主跟细）
        /// </summary>
        public int FollowStyle { get; set; }
        /// <summary>
        /// 打印类型（0 一单一切, 1 一菜一切）
        /// </summary>
        public int PrintType { get; set; }
        /// <summary>
        /// 最近数据同步时间
        /// </summary>
        public DateTime LastDataSyncTime { get; set; }
        /// <summary>
        /// 固定服务费
        /// </summary>
        public decimal FixedServiceFee { get; set; }
        /// <summary>
        /// 按百分比收取服务费
        /// </summary>
        public decimal ServiceFeePercent { get; set; }
        /// <summary>
        /// 服务费开始时间段1
        /// </summary>
        public string ServiceFeeBeginTime1 { get; set; }
        /// <summary>
        /// 服务费结束时间段1
        /// </summary>
        public string ServiceFeeEndTime1 { get; set; }
        /// <summary>
        /// 服务费开始时间段2
        /// </summary>
        public string ServiceFeeBeginTime2 { get; set; }
        /// <summary>
        /// 服务费结束时间段2
        /// </summary>
        public string ServiceFeeEndTime2 { get; set; }
    }
}
