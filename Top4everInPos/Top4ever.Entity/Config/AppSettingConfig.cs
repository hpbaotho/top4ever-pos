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
        /// 营业方式
        /// </summary>
        [XmlElement("SaleType")]
        public ShopSaleType SaleType { get; set; }
        /// <summary>
        /// 营业中断日期
        /// </summary>
        [XmlElement("BreakDays")]
        public int BreakDays { get; set; }
        /// <summary>
        /// 登录图片路径
        /// </summary>
        [XmlElement("LoginImagePath")]
        public string LoginImagePath { get; set; }
        /// <summary>
        /// 桌面图片路径
        /// </summary>
        [XmlElement("DeskImagePath")]
        public string DeskImagePath { get; set; }
        /// <summary>
        /// 第二屏是否启用
        /// </summary>
        [XmlElement("SecondScreenEnabled")]
        public bool SecondScreenEnabled { get; set; }
        /// <summary>
        /// 第二屏视频路径
        /// </summary>
        [XmlElement("ScreenVideoPath")]
        public string ScreenVideoPath { get; set; }
        /// <summary>
        /// 第二屏图片路径
        /// </summary>
        [XmlElement("ScreenImagePath")]
        public string ScreenImagePath { get; set; }
        /// <summary>
        /// 启用零用金
        /// </summary>
        [XmlElement("UsePettyCash")]
        public bool UsePettyCash { get; set; }
        /// <summary>
        /// 启用抽大钞
        /// </summary>
        [XmlElement("TakeAwayCash")]
        public bool TakeAwayCash { get; set; }
        /// <summary>
        /// 自动显示简码
        /// </summary>
        [XmlElement("ShowBrevityCode")]
        public bool ShowBrevityCode { get; set; }
        /// <summary>
        /// 显示沽清数量
        /// </summary>
        [XmlElement("ShowSoldOutQty")]
        public bool ShowSoldOutQty { get; set; }
        /// <summary>
        /// 直接出货
        /// </summary>
        [XmlElement("DirectShipping")]
        public bool DirectShipping { get; set; }
        /// <summary>
        /// 餐牌模式
        /// </summary>
        [XmlElement("CarteMode")]
        public bool CarteMode { get; set; }

        [XmlElement("BizUIConfig")]
        public BizUIConfig bizUIConfig { get; set; }

        [XmlElement("PrintConfig")]
        public PrintConfig printConfig { get; set; }

        [XmlElement("CashBoxConfig")]
        public CashBoxConfig cashBoxConfig { get; set; }

        [XmlElement("TelCallConfig")]
        public TelCallConfig telCallConfig { get; set; }

        [XmlElement("ClientShowConfig")]
        public ClientShowConfig clientShowConfig { get; set; }
    }
}
