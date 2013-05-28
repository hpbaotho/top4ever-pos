using System;
using System.Collections.Generic;

namespace Top4ever.Entity
{
    public class RightsItemCode
    {
        public static readonly string DAILYSTATEMENT = "MA0";                 //日结
        public static readonly string PETTYCASHMODIFY = "MA1";              //零用金修改
        public static readonly string HOURSTURNOVER = "MA2";                //小时营业额
        public static readonly string HISTORYSEARCH = "MA3";                  //历史日结/交班查询
        public static readonly string SINGLEGOODSSTATISTICS = "MA4";    //单品销量统计
        public static readonly string DELETEDGOODSREPORT = "MA5";       //删除品项报表
        public static readonly string PETTYCASHREPORT = "MA6";              //零用金报表
        public static readonly string HISTORYDAILYREPORT = "MA7";         //历史日结汇总表
        public static readonly string PUNCHINREPORT = "MA8";                //打卡报表
        public static readonly string LOGSEARCH = "MA9";                        //日志查询

        public static readonly string CANCELBILL = "BA0";           //取消账单
        public static readonly string SINGLEDISCOUNT = "BA1";       //单品折扣
        public static readonly string CHECKOUT = "BA2";             //结账
        public static readonly string PRECHECKOUT = "BA3";          //预结
        public static readonly string MEMBERDISCOUNT = "BA4";       //会员折扣
        public static readonly string WHOLEDISCOUNT = "BA5";        //整单折扣
        public static readonly string CUTSERVICEFEE = "BA6";        //去服务费
        public static readonly string CANCELGOODS = "BA7";          //退菜
        public static readonly string PAIDBILLMODIFY = "BA8";       //账单修改
        public static readonly string DELETEPAIDGOODS = "BA9";      //单品删除
        public static readonly string DELETEPAIDBILL = "BAA";       //整单删除

        public static readonly string TAKEORDER = "RA0";        //点单
        public static readonly string CLEARDESK = "RA1";        //清空
        public static readonly string TURNTABLE = "RA2";        //转台
        public static readonly string REFORM = "RA3";           //重整
        public static readonly string SPLITBILL = "RA4";        //分单
        public static readonly string PLACEORDER = "RA5";       //落单
        public static readonly string PAYMENT = "RA6";          //支付
        public static readonly string CUSTOMTASTE = "RA7";      //自定义口味
        public static readonly string CUSTOMDISCOUNT = "RA8";   //自定义折扣

        public static bool FindRights(string rightsCode)
        {
            bool hasRights = false;
            foreach (string item in ConstantValuePool.CurrentEmployee.RightsCodeList)
            {
                if (rightsCode == item)
                {
                    hasRights = true;
                    break;
                }
            }
            return hasRights;
        }

        public static bool FindRights(IList<string> rightsCodeList, string rightsCode)
        {
            bool hasRights = false;
            foreach (string item in rightsCodeList)
            {
                if (rightsCode == item)
                {
                    hasRights = true;
                    break;
                }
            }
            return hasRights;
        }
    }
}
