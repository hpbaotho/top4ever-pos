using System;

namespace Top4ever.Domain
{
    public class SystemDictionary
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid SysDictionaryID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}
