using System;
using System.Collections.Generic;

using Top4ever.Domain.Promotions;

namespace VechsoftPos.Promotions
{
    public class PromotionCronTriggerService
    {
        private PromotionCronTrigger promotionCronTrigger;

        public PromotionCronTriggerService(PromotionCronTrigger promotionCronTrigger)
        {
            this.promotionCronTrigger = promotionCronTrigger;
        }

        public PromotionCronTrigger PromotionCronTriggers
        {
            set { promotionCronTrigger = value; }
        }

        public bool IsPromotionInTime()
        {
            if (DateTime.Now >= DateTime.Parse(promotionCronTrigger.BeginDate) && DateTime.Now <= DateTime.Parse(promotionCronTrigger.EndDate))
            {
                bool IsEffective = true;
                DayOfWeek curWeek = DateTime.Now.DayOfWeek;
                string curMonth = DateTime.Now.Month.ToString();
                string curDay = DateTime.Now.Day.ToString();
                int curHour = DateTime.Now.Hour;
                int curMinute = DateTime.Now.Minute;
                //判断周或者日
                if (promotionCronTrigger.Week == "?")
                {
                    //判断是否包含当日
                    if (promotionCronTrigger.Day != "*")
                    {
                        string[] dayArr = promotionCronTrigger.Day.Split(',');
                        bool IsContainDay = false;
                        foreach (string day in dayArr)
                        {
                            if (curDay == day)
                            {
                                IsContainDay = true;
                                break;
                            }
                        }
                        if (!IsContainDay)
                        {
                            IsEffective = false;
                        }
                    }
                }
                else
                {
                    //判断是否包含周几
                    //判断包含# 例:当月第几周星期几
                    if (promotionCronTrigger.Week.IndexOf('#') > 0)
                    {
                        string weekIndex = promotionCronTrigger.Week.Split('#')[0];
                        string weekDay = promotionCronTrigger.Week.Split('#')[1];
                        //计算当日是当月的第几周
                        DateTime FirstofMonth = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month.ToString().PadLeft(2,'0') + "-" + "01");
                        int i = (int)FirstofMonth.Date.DayOfWeek;
                        if (i == 0)
                        {
                            i = 7;
                        }
                        int curWeekIndex = (DateTime.Now.Day + i - 1) / 7;
                        if (curWeekIndex != int.Parse(weekIndex))
                        {
                            IsEffective = false;
                        }
                        else
                        {
                            if ((int)curWeek != int.Parse(weekDay))
                            {
                                IsEffective = false;
                            }
                        }
                    }
                    else
                    {
                        //不包含# 例:当月每个星期几
                        string[] weekArr = promotionCronTrigger.Week.Split(',');
                        bool IsContainWeek = false;
                        foreach (string week in weekArr)
                        {
                            if ((int)curWeek == int.Parse(week))
                            {
                                IsContainWeek = true;
                                break;
                            }
                        }
                        if (!IsContainWeek)
                        {
                            IsEffective = false;
                        }
                    }
                }
                //判断时
                if (promotionCronTrigger.Hour != "*")
                {
                    if (promotionCronTrigger.Hour.IndexOf('-') > 0)
                    {
                        string hourMinute = curHour.ToString().PadLeft(2, '0') + ":" + curMinute.ToString().PadLeft(2, '0');
                        if (promotionCronTrigger.Hour.IndexOf(',') > 0) //多个小时时间段
                        {
                            string[] hourArr = promotionCronTrigger.Hour.Split(',');
                            bool IsContainHour = false;
                            foreach (string hour in hourArr)
                            {
                                string beginHour = hour.Split('-')[0].Trim();
                                string endHour = hour.Split('-')[1].Trim();
                                if (string.Compare(hourMinute, beginHour) >= 0 && string.Compare(hourMinute, endHour) <= 0)
                                {
                                    IsContainHour = true;
                                    break;
                                }
                            }
                            if (!IsContainHour)
                            {
                                IsEffective = false;
                            }
                        }
                        else
                        {
                            string beginHour = promotionCronTrigger.Hour.Split('-')[0].Trim();
                            string endHour = promotionCronTrigger.Hour.Split('-')[1].Trim();
                            if (string.Compare(hourMinute, beginHour) < 0 || string.Compare(hourMinute, endHour) > 0)
                            {
                                IsEffective = false;
                            }
                        }
                    }
                    else
                    {
                        if (promotionCronTrigger.Hour.IndexOf(',') > 0) //多个小时
                        {
                            string[] hourArr = promotionCronTrigger.Hour.Split(',');
                            bool IsContainHour = false;
                            foreach (string hour in hourArr)
                            {
                                if (curHour == int.Parse(hour))
                                {
                                    IsContainHour = true;
                                    break;
                                }
                            }
                            if (!IsContainHour)
                            {
                                IsEffective = false;
                            }
                        }
                        else
                        {
                            if (curHour != int.Parse(promotionCronTrigger.Hour))
                            {
                                IsEffective = false;
                            }
                        }
                    }
                }
                //判断分
                if (promotionCronTrigger.Minute != "*")
                {
                    if (promotionCronTrigger.Minute.IndexOf('-') > 0)
                    {
                        if (promotionCronTrigger.Minute.IndexOf(',') > 0) //多个分钟时间段
                        {
                            string[] minuteArr = promotionCronTrigger.Minute.Split(',');
                            bool IsContainMinute = false;
                            foreach (string minute in minuteArr)
                            {
                                string beginMinute = minute.Split('-')[0];
                                string endMinute = minute.Split('-')[1];
                                if (curMinute >= int.Parse(beginMinute) && curMinute <= int.Parse(endMinute))
                                {
                                    IsContainMinute = true;
                                    break;
                                }
                            }
                            if (!IsContainMinute)
                            {
                                IsEffective = false;
                            }
                        }
                        else
                        {
                            string beginMinute = promotionCronTrigger.Minute.Split('-')[0];
                            string endMinute = promotionCronTrigger.Minute.Split('-')[1];
                            if (curMinute < int.Parse(beginMinute) || curMinute > int.Parse(endMinute))
                            {
                                IsEffective = false;
                            }
                        }
                    }
                    else
                    {
                        if (promotionCronTrigger.Minute.IndexOf(',') > 0) //多个分钟
                        {
                            string[] minuteArr = promotionCronTrigger.Minute.Split(',');
                            bool IsContainMinute = false;
                            foreach (string minute in minuteArr)
                            {
                                if (curMinute == int.Parse(minute))
                                {
                                    IsContainMinute = true;
                                    break;
                                }
                            }
                            if (!IsContainMinute)
                            {
                                IsEffective = false;
                            }
                        }
                        else
                        {
                            if (curMinute != int.Parse(promotionCronTrigger.Minute))
                            {
                                IsEffective = false;
                            }
                        }
                    }
                }
                return IsEffective;
            }
            else
            {
                return false;
            }
        }
    }
}