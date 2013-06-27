using System;
using System.Collections.Generic;
using System.Xml;

namespace Top4ever.Common
{
    public class BaseConfigManager
    {
        private readonly string configFilePath = string.Empty; //配置文件的路径

        public BaseConfigManager(string configFilePath)
        {
            this.configFilePath = configFilePath;
        }

        public virtual string GetConfigValue()
        {
            object objConfig = CacheHelper.GetInstance().Get(configFilePath);
            if (objConfig == null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(@configFilePath);
                string xmlString = xmlDoc.OuterXml;
                CacheHelper.GetInstance().Insert(configFilePath, xmlString);
                return xmlString;
            }
            else
            {
                return objConfig.ToString();
            }
        }
    }
}
