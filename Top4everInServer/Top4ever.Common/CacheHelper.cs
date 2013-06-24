using System;
using System.Collections.Generic;

namespace Top4ever.Common
{
    public class CacheHelper
    {
        private static CacheHelper instance;
        private static object obj = new object();
        private Dictionary<string, object> curDic = new Dictionary<string, object>();

        private CacheHelper() { }

        /// <summary>
        /// 获取Cache单例
        /// </summary>
        public static CacheHelper GetInstance()
        {
            if (instance == null)
            {
                lock (obj)
                {
                    if (instance == null)
                    {
                        instance = new CacheHelper();
                    }
                }
            }
            return instance;
        }

        public object Get(string key)
        {
            if (curDic.ContainsKey(key))
            {
                return curDic[key];
            }
            else
            {
                return null;
            }
        }

        public void Insert(string key, object value)
        {
            if (curDic.ContainsKey(key))
            {
                curDic[key] = value;
            }
            else
            {
                curDic.Add(key, value);
            }
        }

        public object Remove(string key)
        {
            if (curDic.ContainsKey(key))
            {
                object value = null;
                curDic.Remove(key);
                return value;
            }
            else
            {
                return null;
            }
        }
    }
}
