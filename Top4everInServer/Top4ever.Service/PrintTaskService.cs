using System;
using System.Collections.Generic;

using Top4ever.Domain;
using Top4ever.Domain.OrderRelated;
using Top4ever.Domain.Transfer;
using Top4ever.Interface;

namespace Top4ever.Service
{
    /// <summary>
    /// Summary description for PrintTaskService.
    /// </summary>
    public class PrintTaskService
    {
        private static PrintTaskService _instance = new PrintTaskService();

        private PrintTaskService()
        { }

        public static PrintTaskService GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// 获取打印任务列表
        /// </summary>
        /// <param name="salesOrder">订单实例</param>
        /// <param name="printStyle">1 堂吃, 2 外卖, 3 堂吃兼外卖</param>
        /// <param name="followStyle">1 细跟主, 2 主跟细</param>
        /// <param name="printType">打印类型（0 一单一切, 1 一菜一切）</param>
        /// <param name="taskType">任务类别 1,加单 2,删单 3,催单 4,转台</param>
        /// <param name="reason">原因</param>
        public IList<PrintTask> GetPrintTaskList(SalesOrder salesOrder, int printStyle, int followStyle, int printType, int taskType, string reason)
        {
            if (salesOrder == null || salesOrder.order == null || salesOrder.orderDetailsList.Count == 0) return null;
            IList<PrintTask> printTaskList = new List<PrintTask>();
            DateTime printTime = DateTime.Now;
            Order order = salesOrder.order;
            for (int index = 0; index < salesOrder.orderDetailsList.Count; index++)
            {
                OrderDetails details = salesOrder.orderDetailsList[index];
                if (details.ItemType == (int)OrderItemType.Goods)
                {
                    //1 细跟主, 2 主跟细
                    if (followStyle == 1)
                    {
                        if (string.IsNullOrEmpty(details.PrintSolutionName))
                        {
                            //类似主跟细
                            for (int i = index + 1; i < salesOrder.orderDetailsList.Count; i++)
                            {
                                OrderDetails nextDetails = salesOrder.orderDetailsList[i];
                                if (nextDetails.ItemType == (int)OrderItemType.Goods)
                                {
                                    break;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(nextDetails.PrintSolutionName))
                                    {
                                        string[] printerNameArr = nextDetails.PrintSolutionName.Split(',');
                                        foreach (string printerName in printerNameArr)
                                        {
                                            //打印解决方案
                                            //主项
                                            PrintTask printTask = new PrintTask();
                                            printTask.OrderNo = order.OrderNo;
                                            printTask.PeopleNum = order.PeopleNum;
                                            printTask.EmployeeNo = order.EmployeeNo;
                                            printTask.EmployeeID = order.EmployeeID;
                                            printTask.TranSequence = order.TranSequence;
                                            printTask.EatType = order.EatType;
                                            printTask.DeskName = order.DeskName;
                                            printTask.SubOrderNo = order.SubOrderNo;
                                            printTask.TaskType = taskType;
                                            printTask.PrintTime = printTime;
                                            printTask.Reason = reason;
                                            printTask.IsPrinted = false;
                                            printTask.PrintSolutionName = printerName;
                                            printTask.PrintType = 1;
                                            printTask.GoodsName = details.GoodsName;
                                            printTask.Unit = details.Unit;
                                            printTask.ItemQty = details.ItemQty;
                                            printTaskList.Add(printTask);
                                            //细项或者套餐
                                            printTask = new PrintTask();
                                            printTask.OrderNo = order.OrderNo;
                                            printTask.PeopleNum = order.PeopleNum;
                                            printTask.EmployeeNo = order.EmployeeNo;
                                            printTask.EmployeeID = order.EmployeeID;
                                            printTask.TranSequence = order.TranSequence;
                                            printTask.EatType = order.EatType;
                                            printTask.DeskName = order.DeskName;
                                            printTask.SubOrderNo = order.SubOrderNo;
                                            printTask.TaskType = taskType;
                                            printTask.PrintTime = printTime;
                                            printTask.Reason = reason;
                                            printTask.IsPrinted = false;
                                            printTask.PrintSolutionName = printerName;
                                            printTask.PrintType = 0;
                                            if (nextDetails.ItemType == (int)OrderItemType.Details)
                                            {
                                                printTask.DetailsName = nextDetails.GoodsName;
                                            }
                                            if (nextDetails.ItemType == (int)OrderItemType.SetMeal)
                                            {
                                                printTask.SubGoodsName = nextDetails.GoodsName;
                                            }
                                            printTask.Unit = nextDetails.Unit;
                                            printTask.ItemQty = nextDetails.ItemQty;
                                            printTaskList.Add(printTask);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            string[] printerNameArr = details.PrintSolutionName.Split(',');
                            foreach (string printerName in printerNameArr)
                            {
                                //打印解决方案
                                PrintTask printTask = new PrintTask();
                                printTask.OrderNo = order.OrderNo;
                                printTask.PeopleNum = order.PeopleNum;
                                printTask.EmployeeNo = order.EmployeeNo;
                                printTask.EmployeeID = order.EmployeeID;
                                printTask.TranSequence = order.TranSequence;
                                printTask.EatType = order.EatType;
                                printTask.DeskName = order.DeskName;
                                printTask.SubOrderNo = order.SubOrderNo;
                                printTask.TaskType = taskType;
                                printTask.PrintTime = printTime;
                                printTask.Reason = reason;
                                printTask.IsPrinted = false;
                                printTask.PrintSolutionName = printerName;
                                printTask.PrintType = printType;
                                printTask.GoodsName = details.GoodsName;
                                printTask.Unit = details.Unit;
                                printTask.ItemQty = details.ItemQty;
                                if (printStyle == 1)    //1 堂吃, 2 外卖, 3堂吃兼外卖
                                {
                                    printTaskList.Add(printTask);
                                    for (int i = index + 1; i < salesOrder.orderDetailsList.Count; i++)
                                    {
                                        OrderDetails nextDetails = salesOrder.orderDetailsList[i];
                                        if (nextDetails.ItemType == (int)OrderItemType.Goods)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            //打印解决方案
                                            printTask = new PrintTask();
                                            printTask.OrderNo = order.OrderNo;
                                            printTask.PeopleNum = order.PeopleNum;
                                            printTask.EmployeeNo = order.EmployeeNo;
                                            printTask.EmployeeID = order.EmployeeID;
                                            printTask.TranSequence = order.TranSequence;
                                            printTask.EatType = order.EatType;
                                            printTask.DeskName = order.DeskName;
                                            printTask.SubOrderNo = order.SubOrderNo;
                                            printTask.TaskType = taskType;
                                            printTask.PrintTime = printTime;
                                            printTask.Reason = reason;
                                            printTask.IsPrinted = false;
                                            printTask.PrintSolutionName = printerName;
                                            printTask.PrintType = 0;
                                            if (nextDetails.ItemType == (int)OrderItemType.Details)
                                            {
                                                printTask.DetailsName = nextDetails.GoodsName;
                                            }
                                            if (nextDetails.ItemType == (int)OrderItemType.SetMeal)
                                            {
                                                printTask.SubGoodsName = nextDetails.GoodsName;
                                            }
                                            printTask.Unit = nextDetails.Unit;
                                            printTask.ItemQty = nextDetails.ItemQty;
                                            printTaskList.Add(printTask);
                                        }
                                    }
                                }
                                if (printStyle == 2)    //1 堂吃, 2 外卖, 3堂吃兼外卖
                                {
                                    string totalDetailsName = string.Empty;
                                    for (int i = index + 1; i < salesOrder.orderDetailsList.Count; i++)
                                    {
                                        OrderDetails nextDetails = salesOrder.orderDetailsList[i];
                                        if (nextDetails.ItemType == (int)OrderItemType.Goods)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            totalDetailsName += "," + nextDetails.GoodsName + "*" + nextDetails.ItemQty;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(totalDetailsName))
                                    {
                                        totalDetailsName = totalDetailsName.Substring(1);
                                    }
                                    printTask.TotalDetailsName = totalDetailsName;
                                    printTaskList.Add(printTask);
                                }
                                if (printStyle == 3)    //1 堂吃, 2 外卖, 3堂吃兼外卖
                                {
                                    if (order.EatType == (int)EatWayType.DineIn)
                                    {
                                        printTaskList.Add(printTask);
                                        for (int i = index + 1; i < salesOrder.orderDetailsList.Count; i++)
                                        {
                                            OrderDetails nextDetails = salesOrder.orderDetailsList[i];
                                            if (nextDetails.ItemType == (int)OrderItemType.Goods)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                //打印解决方案
                                                printTask = new PrintTask();
                                                printTask.OrderNo = order.OrderNo;
                                                printTask.PeopleNum = order.PeopleNum;
                                                printTask.EmployeeNo = order.EmployeeNo;
                                                printTask.EmployeeID = order.EmployeeID;
                                                printTask.TranSequence = order.TranSequence;
                                                printTask.EatType = order.EatType;
                                                printTask.DeskName = order.DeskName;
                                                printTask.SubOrderNo = order.SubOrderNo;
                                                printTask.TaskType = taskType;
                                                printTask.PrintTime = printTime;
                                                printTask.Reason = reason;
                                                printTask.IsPrinted = false;
                                                printTask.PrintSolutionName = printerName;
                                                printTask.PrintType = 0;
                                                if (nextDetails.ItemType == (int)OrderItemType.Details)
                                                {
                                                    printTask.DetailsName = nextDetails.GoodsName;
                                                }
                                                if (nextDetails.ItemType == (int)OrderItemType.SetMeal)
                                                {
                                                    printTask.SubGoodsName = nextDetails.GoodsName;
                                                }
                                                printTask.Unit = nextDetails.Unit;
                                                printTask.ItemQty = nextDetails.ItemQty;
                                                printTaskList.Add(printTask);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string totalDetailsName = string.Empty;
                                        for (int i = index + 1; i < salesOrder.orderDetailsList.Count; i++)
                                        {
                                            OrderDetails nextDetails = salesOrder.orderDetailsList[i];
                                            if (nextDetails.ItemType == (int)OrderItemType.Goods)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                totalDetailsName += "," + nextDetails.GoodsName + "*" + nextDetails.ItemQty;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(totalDetailsName))
                                        {
                                            totalDetailsName = totalDetailsName.Substring(1);
                                        }
                                        printTask.TotalDetailsName = totalDetailsName;
                                        printTaskList.Add(printTask);
                                    }
                                }
                            }
                        }
                    }
                    //1 细跟主, 2 主跟细
                    if (followStyle == 2)
                    {
                        bool onlyMainItem = true;
                        for (int i = index + 1; i < salesOrder.orderDetailsList.Count; i++)
                        {
                            OrderDetails nextDetails = salesOrder.orderDetailsList[i];
                            if (nextDetails.ItemType == (int)OrderItemType.Goods)
                            {
                                if (onlyMainItem)
                                {
                                    if (!string.IsNullOrEmpty(details.PrintSolutionName))
                                    {
                                        string[] printerNameArr = details.PrintSolutionName.Split(',');
                                        foreach (string printerName in printerNameArr)
                                        {
                                            //打印解决方案
                                            PrintTask printTask = new PrintTask();
                                            printTask.OrderNo = order.OrderNo;
                                            printTask.PeopleNum = order.PeopleNum;
                                            printTask.EmployeeNo = order.EmployeeNo;
                                            printTask.EmployeeID = order.EmployeeID;
                                            printTask.TranSequence = order.TranSequence;
                                            printTask.EatType = order.EatType;
                                            printTask.DeskName = order.DeskName;
                                            printTask.SubOrderNo = order.SubOrderNo;
                                            printTask.TaskType = taskType;
                                            printTask.PrintTime = printTime;
                                            printTask.Reason = reason;
                                            printTask.IsPrinted = false;
                                            printTask.PrintSolutionName = printerName;
                                            printTask.PrintType = printType;
                                            printTask.GoodsName = details.GoodsName;
                                            printTask.Unit = details.Unit;
                                            printTask.ItemQty = details.ItemQty;
                                            printTaskList.Add(printTask);
                                        }
                                    }
                                }
                                break;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(nextDetails.PrintSolutionName))
                                {
                                    string[] printerNameArr = nextDetails.PrintSolutionName.Split(',');
                                    foreach (string printerName in printerNameArr)
                                    {
                                        //打印解决方案
                                        //主项
                                        PrintTask printTask = new PrintTask();
                                        printTask.OrderNo = order.OrderNo;
                                        printTask.PeopleNum = order.PeopleNum;
                                        printTask.EmployeeNo = order.EmployeeNo;
                                        printTask.EmployeeID = order.EmployeeID;
                                        printTask.TranSequence = order.TranSequence;
                                        printTask.EatType = order.EatType;
                                        printTask.DeskName = order.DeskName;
                                        printTask.SubOrderNo = order.SubOrderNo;
                                        printTask.TaskType = taskType;
                                        printTask.PrintTime = printTime;
                                        printTask.Reason = reason;
                                        printTask.IsPrinted = false;
                                        printTask.PrintSolutionName = printerName;
                                        printTask.PrintType = 1;
                                        printTask.GoodsName = details.GoodsName;
                                        printTask.Unit = details.Unit;
                                        printTask.ItemQty = details.ItemQty;
                                        printTaskList.Add(printTask);
                                        //细项或者套餐
                                        printTask = new PrintTask();
                                        printTask.OrderNo = order.OrderNo;
                                        printTask.PeopleNum = order.PeopleNum;
                                        printTask.EmployeeNo = order.EmployeeNo;
                                        printTask.EmployeeID = order.EmployeeID;
                                        printTask.TranSequence = order.TranSequence;
                                        printTask.EatType = order.EatType;
                                        printTask.DeskName = order.DeskName;
                                        printTask.SubOrderNo = order.SubOrderNo;
                                        printTask.TaskType = taskType;
                                        printTask.PrintTime = printTime;
                                        printTask.Reason = reason;
                                        printTask.IsPrinted = false;
                                        printTask.PrintSolutionName = printerName;
                                        printTask.PrintType = 0;
                                        if (nextDetails.ItemType == (int)OrderItemType.Details)
                                        {
                                            printTask.DetailsName = nextDetails.GoodsName;
                                        }
                                        if (nextDetails.ItemType == (int)OrderItemType.SetMeal)
                                        {
                                            printTask.SubGoodsName = nextDetails.GoodsName;
                                        }
                                        printTask.Unit = nextDetails.Unit;
                                        printTask.ItemQty = nextDetails.ItemQty;
                                        printTaskList.Add(printTask);
                                    }
                                    onlyMainItem = false;
                                }
                            }
                        }
                    }
                }
            }
            return printTaskList;
        }

        private enum OrderItemType
        {
            /// <summary>
            /// 菜品
            /// </summary>
            Goods = 1,
            /// <summary>
            /// 细项
            /// </summary>
            Details = 2,
            /// <summary>
            /// 套餐
            /// </summary>
            SetMeal = 3
        }

        private enum EatWayType
        {
            /// <summary>
            /// 堂食
            /// </summary>
            DineIn = 1,
            /// <summary>
            /// 外卖
            /// </summary>
            Takeout = 2,
            /// <summary>
            /// 外送
            /// </summary>
            OutsideOrder = 3
        }
    }
}
