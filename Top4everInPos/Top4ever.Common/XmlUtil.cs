using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Top4ever.Common
{
    public class XmlUtil
    {
        /// <summary>
        /// 将对象序列化为文件
        /// </summary>
        public static void Serialize<T>(T obj, string fileName)
        {
            using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);
            }
        }

        /// <summary>
        /// 将文件反序列化为对象
        /// </summary>
        public static T Deserialize<T>(string fileName)
        {
            using (XmlTextReader reader = new XmlTextReader(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// 对象序列化成 XML String
        /// </summary>
        public static string XmlSerialize<T>(T obj)
        {
            string xmlString = string.Empty;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
            }
            return xmlString;
        }

        /// <summary>
        /// XML String 反序列化成对象
        /// </summary>
        public static T XmlDeserialize<T>(string xmlString)
        {
            T t = default(T);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (Stream xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                {
                    Object obj = xmlSerializer.Deserialize(xmlReader);
                    t = (T)obj;
                }
            }
            return t;
        }
    }
}
