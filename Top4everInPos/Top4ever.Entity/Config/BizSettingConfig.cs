using System;
using System.Collections.Generic;

using Top4ever.Entity.Enum;

namespace Top4ever.Entity.Config
{
    public class BizSettingConfig
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 设备
        /// </summary>
        public string DeviceNo { get; set; }
        /// <summary>
        /// 第一语言
        /// </summary>
        public LanguageType Languge1st { get; set; }
        /// <summary>
        /// 第二语言
        /// </summary>
        public LanguageType Languge2nd { get; set; }
        /// <summary>
        /// 24小时制
        /// </summary>
        public bool TimeSystem24H { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        public float FontSize { get; set; }
        /// <summary>
        /// 启用零用金
        /// </summary>
        public bool UsePettyCash { get; set; }
        /// <summary>
        /// 显示简码
        /// </summary>
        public bool ShowBrevityCode { get; set; }
        /// <summary>
        /// 抽大钞
        /// </summary>
        public bool TakeAwayCash { get; set; }
        /// <summary>
        /// 是否沽清
        /// </summary>
        public bool CheckSoldOut { get; set; }
    }
}
