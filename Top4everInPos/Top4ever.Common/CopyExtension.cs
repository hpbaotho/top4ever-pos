using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Top4ever.Common
{
    public class CopyExtension
    {
        public static T Clone<T>(T obj)
        {
            using (Stream stream = new MemoryStream())
            {
                //利用序列化与反序列化完成引用对象的复制
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
