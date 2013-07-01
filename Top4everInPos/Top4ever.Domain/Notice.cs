using System;

namespace Top4ever.Domain
{
    public class Notice
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid NoticeID { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string NoticeContent { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDateTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDateTime { get; set; }
    }
}
