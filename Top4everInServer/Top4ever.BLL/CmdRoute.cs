using System;
using System.Collections.Generic;
using System.Text;

using Top4ever.BLL.Enum;

namespace Top4ever.BLL
{
    public class CmdRoute
    {
        public static byte[] RouteHandler(int commandID, byte[] itemBuffer)
        {
            byte[] byteRet = null;
            switch ((Command)commandID)
            {
                case Command.ID_USERLOGIN:
                    byteRet = EmployeeBLL.EmployeeLogin(itemBuffer);
                    break;
                case Command.ID_GET_SYSBASICDATA:
                    byteRet = SysBasicDataBLL.GetSysBasicData(itemBuffer);
                    break;
                case Command.ID_GET_DESKBYNAME:
                    byteRet = BizDeskBLL.GetBizDeskByName(itemBuffer);
                    break;
                case Command.ID_UPDATE_DESKSTATUS:
                    byteRet = BizDeskBLL.UpdateBizDeskStatus(itemBuffer);
                    break;
                case Command.ID_CREATE_SALESORDER:
                    byteRet = SalesOrderBLL.CreateSalesOrder(itemBuffer);
                    break;
                case Command.ID_GET_SALESORDER:
                    byteRet = SalesOrderBLL.GetSalesOrder(itemBuffer);
                    break;
                case Command.ID_UPDATE_SALESORDER:
                    byteRet = SalesOrderBLL.UpdateSalesOrder(itemBuffer);
                    break;
                case Command.ID_PAYFORORDER:
                    byteRet = PayingOrderBLL.PayForOrder(itemBuffer);
                    break;
                case Command.ID_DELETE_SINGLEORDER:
                    byteRet = DeletedOrderBLL.DeleteSingleOrder(itemBuffer);
                    break;
                case Command.ID_DELETE_WHOLEORDER:
                    byteRet = DeletedOrderBLL.DeleteWholeOrder(itemBuffer);
                    break;
                case Command.ID_GET_DESKREALTIMEINFO:
                    byteRet = BizDeskBLL.GetDeskRealTimeInfo(itemBuffer);
                    break;
                case Command.ID_UPDATE_LADEORDERDETAILS:
                    byteRet = OrderDetailsBLL.LadeOrderDetails(itemBuffer);
                    break;
                case Command.ID_GET_ORDERLIST:
                    byteRet = OrderBLL.GetOrderList(itemBuffer);
                    break;
                case Command.ID_SPLIT_SALESORDER:
                    byteRet = SalesOrderBLL.SplitSalesOrder(itemBuffer);
                    break;
                case Command.ID_ORDER_DESKOPERATE:
                    byteRet = OrderBLL.OrderDeskOperate(itemBuffer);
                    break;
                case Command.ID_CREATE_HANDOVER:
                    byteRet = HandoverBLL.CreateHandover(itemBuffer);
                    break;
                case Command.ID_CREATE_DAILYBALANCE:
                    byteRet = DailyBalanceBLL.CreateDailyBalance(itemBuffer);
                    break;
                case Command.ID_GET_REPORTDATABYHANDOVER:
                    byteRet = BusinessReportBLL.GetReportDataByHandover(itemBuffer);
                    break;
                case Command.ID_GET_REPORTDATABYDAILYSTATEMENT:
                    byteRet = BusinessReportBLL.GetReportDataByDailyStatement(itemBuffer);
                    break;
                case Command.ID_CREATE_REMINDERORDER:
                    byteRet = ReminderBLL.CreateReminderOrder(itemBuffer);
                    break;
                case Command.ID_GET_RIGHTSCODELIST:
                    byteRet = EmployeeBLL.GetRightsCodeList(itemBuffer);
                    break;
                case Command.ID_GET_DAILYTIMEINTERVAL:
                    byteRet = DailyBalanceBLL.GetDailyStatementTimeInterval();
                    break;
                case Command.ID_GET_ORDERLISTBYSEARCH:
                    byteRet = OrderBLL.GetOrderListBySearch(itemBuffer);
                    break;
                case Command.ID_SWIPINGCARDTOLOGIN:
                    byteRet = EmployeeBLL.SwipingCardToLogin(itemBuffer);
                    break;
                case Command.ID_GET_SALESORDERBYBILLSEARCH:
                    byteRet = SalesOrderBLL.GetSalesOrderByBillSearch(itemBuffer);
                    break;
                case Command.ID_DELETE_PAIDWHOLEORDER:
                    byteRet = DeletedOrderBLL.DeletePaidWholeOrder(itemBuffer);
                    break;
                case Command.ID_CHECK_LASTDAILYSTATEMENT:
                    byteRet = DailyBalanceBLL.CheckLastDailyStatement();
                    break;
                case Command.ID_UPDATE_ORDERSTATUS:
                    byteRet = OrderBLL.UpdateOrderStatus(itemBuffer);
                    break;
                case Command.ID_DELETE_PAIDSINGLEORDER:
                    byteRet = DeletedOrderBLL.DeletePaidSingleOrder(itemBuffer);
                    break;
                case Command.ID_MODIFY_PAIDORDER:
                    byteRet = ModifyOrderBLL.ModifyForOrder(itemBuffer);
                    break;

                default:
                    byteRet = null;
                    break;
            }
            return byteRet;
        }
    }
}
