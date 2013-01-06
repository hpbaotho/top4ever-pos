using System;

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
    }
}
