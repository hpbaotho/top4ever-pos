using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Top4ever.Entity.Enum;

namespace Top4ever.Entity.Config
{
    [XmlRoot("AppSettingConfig")]
    public class AppSettingConfig
    {
        /// <summary>
        /// IP地址
        /// </summary>
        [XmlElement("IPAddress")]
        public string IPAddress { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        [XmlElement("Port")]
        public int Port { get; set; }
        /// <summary>
        /// 设备
        /// </summary>
        [XmlElement("DeviceNo")]
        public string DeviceNo { get; set; }
        /// <summary>
        /// 第一语言
        /// </summary>
        [XmlElement("Languge1st")]
        public LanguageType Languge1st { get; set; }
        /// <summary>
        /// 第二语言
        /// </summary>
        [XmlElement("Languge2nd")]
        public LanguageType Languge2nd { get; set; }
        /// <summary>
        /// 24小时制
        /// </summary>
        [XmlElement("TimeSystem24H")]
        public bool TimeSystem24H { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        [XmlElement("FontSize")]
        public float FontSize { get; set; }
        /// <summary>
        /// 启用零用金
        /// </summary>
        [XmlElement("UsePettyCash")]
        public bool UsePettyCash { get; set; }
        /// <summary>
        /// 显示简码
        /// </summary>
        [XmlElement("ShowBrevityCode")]
        public bool ShowBrevityCode { get; set; }
        /// <summary>
        /// 抽大钞
        /// </summary>
        [XmlElement("TakeAwayCash")]
        public bool TakeAwayCash { get; set; }
        /// <summary>
        /// 是否沽清
        /// </summary>
        [XmlElement("CheckSoldOut")]
        public bool CheckSoldOut { get; set; }

        [XmlElement("BizUIConfig")]
        public BizUIConfig bizUIConfig { get; set; }

        [XmlElement("PrintConfig")]
        public PrintConfig printConfig { get; set; }

        [XmlElement("CashBoxConfig")]
        public CashBoxConfig cashBoxConfig { get; set; }

        [XmlElement("ClientShowConfig")]
        public ClientShowConfig clientShowConfig { get; set; }
    }
}
