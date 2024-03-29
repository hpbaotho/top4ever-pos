﻿using Top4ever.BLL.Enum;

namespace Top4ever.BLL
{
    public class CmdRoute
    {
        public static byte[] RouteHandler(int commandId, byte[] itemBuffer)
        {
            byte[] byteRet;
            switch ((Command)commandId)
            {
                case Command.ID_USERLOGIN:
                    byteRet = EmployeeBLL.EmployeeLogin(itemBuffer);
                    break;
                case Command.ID_GET_SYSBASICDATA:
                    byteRet = SysBasicDataBLL.GetSysBasicData();
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
                    byteRet = DailyBalanceBLL.CheckLastDailyStatement(itemBuffer);
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
                case Command.ID_CREATE_PREPAYORDER:
                    byteRet = PayingOrderBLL.CreatePrePayOrder(itemBuffer);
                    break;
                case Command.ID_GET_DELIVERYORDERLIST:
                    byteRet = OrderBLL.GetDeliveryOrderList(itemBuffer);
                    break;
                case Command.ID_UPDATE_TAKEOUTORDERSTATUS:
                    byteRet = CustomersBLL.UpdateTakeoutOrderStatus(itemBuffer);
                    break;
                case Command.ID_CREATE_CUSTOMERINFO:
                    byteRet = CustomersBLL.CreateCustomerInfo(itemBuffer);
                    break;
                case Command.ID_UPDATE_CUSTOMERINFO:
                    byteRet = CustomersBLL.UpdateCustomerInfo(itemBuffer);
                    break;
                case Command.ID_GET_CUSTOMERINFOBYPHONE:
                    byteRet = CustomersBLL.GetCustomerInfoByPhone(itemBuffer);
                    break;
                case Command.ID_GET_ALLCUSTOMERINFO:
                    byteRet = CustomersBLL.GetAllCustomerInfo();
                    break;
                case Command.ID_GET_EMPLOYEEBYNO:
                    byteRet = EmployeeBLL.GetEmployeeByNo(itemBuffer);
                    break;
                case Command.ID_GET_CUSTOMERORDER:
                    byteRet = CustomersBLL.GetCustomerOrder(itemBuffer);
                    break;
                case Command.ID_CREATE_CUSTOMERORDER:
                    byteRet = CustomersBLL.CreateCustomerOrder(itemBuffer);
                    break;
                case Command.ID_GET_GOODSCHECKSTOCK:
                    byteRet = GoodsBLL.GetGoodsCheckStock();
                    break;
                case Command.ID_UPDATE_REDUCEDGOODSQTY:
                    byteRet = GoodsBLL.UpdateReducedGoodsQty(itemBuffer);
                    break;
                case Command.ID_GET_VIPCARD:
                    byteRet = VIPCardBLL.SearchVIPCard(itemBuffer);
                    break;
                case Command.ID_GET_VIPCARDTRADELIST:
                    byteRet = VIPCardTradeBLL.GetVIPCardTradeList(itemBuffer);
                    break;
                case Command.ID_GET_CARDDISCOUNTRATE:
                    byteRet = VIPCardBLL.GetCardDiscountRate(itemBuffer);
                    break;
                case Command.ID_UPDATE_CARDPASSWORD:
                    byteRet = VIPCardBLL.UpdateCardPassword(itemBuffer);
                    break;
                case Command.ID_UPDATE_CARDSTATUS:
                    byteRet = VIPCardBLL.UpdateVIPCardStatus(itemBuffer);
                    break;
                case Command.ID_ADD_CARDSTOREDVALUE:
                    byteRet = VIPCardTradeBLL.AddVIPCardStoredValue(itemBuffer);
                    break;
                case Command.ID_ADD_CARDPAYMENT:
                    byteRet = VIPCardTradeBLL.AddVIPCardPayment(itemBuffer);
                    break;
                case Command.ID_REFUNDPAYMENT:
                    byteRet = VIPCardTradeBLL.RefundVIPCardPayment(itemBuffer);
                    break;
                case Command.ID_GET_SHOP:
                    byteRet = ShopBLL.GetCurrentShop();
                    break;
                case Command.ID_GET_ITEMPRICELISTBYGROUP:
                    byteRet = BusinessReportBLL.GetItemPriceListByGroup(itemBuffer);
                    break;
                case Command.ID_GET_ALLDELETEDITEMS:
                    byteRet = OrderDetailsBLL.GetAllDeletedItems(itemBuffer);
                    break;
                case Command.ID_UPDATE_EMPLOYEEPASSWORD:
                    byteRet = EmployeeBLL.UpdateEmployeePassword(itemBuffer);
                    break;
                case Command.ID_GET_LASTCUSTOMPRICE:
                    byteRet = OrderDetailsBLL.GetLastCustomPrice(itemBuffer);
                    break;
                case Command.ID_GET_HOURSALESREPORT:
                    byteRet = OrderBLL.GetHourSalesReport(itemBuffer);
                    break;
                case Command.ID_GET_DAILYBALANCETIME:
                    byteRet = DailyBalanceBLL.GetDailyBalanceTime(itemBuffer);
                    break;
                case Command.ID_GET_HANDOVER_RECORD:
                    byteRet = HandoverBLL.GetHandoverRecord(itemBuffer);
                    break;
                case Command.ID_GET_REPORTDATABYHANDOVERRECORDID:
                    byteRet = BusinessReportBLL.GetReportDataByHandoverRecordID(itemBuffer);
                    break;
                case Command.ID_CREATE_CALLRECORD:
                    byteRet = CustomersBLL.CreateOrUpdateCallRecord(itemBuffer);
                    break;
                case Command.ID_GET_TOPSELLGOODS:
                    byteRet = CustomersBLL.GetTopSellGoods(itemBuffer);
                    break;
                case Command.ID_GET_CALLRECORD:
                    byteRet = CustomersBLL.GetCallRecordByStatus(itemBuffer);
                    break;
                case Command.ID_GET_TOPSELLGOODSBYTIME:
                    byteRet = OrderDetailsBLL.GetTopSellGoodsByTime(itemBuffer);
                    break;
                case Command.ID_GET_DAILYSTATEMENTINDAYS:
                    byteRet = DailyBalanceBLL.GetDailyStatementInDays(itemBuffer);
                    break;

                case Command.ANDROID_GET_GOODSGROUPLIST:
                    byteRet = SysBasicDataBLL.GetGoodsGroupListInAndroid();
                    break;
                case Command.ANDROID_GET_GOODSLIST:
                    byteRet = SysBasicDataBLL.GetGoodsListInAndroid(itemBuffer);
                    break;
                case Command.ANDROID_GET_DESKLIST:
                    byteRet = BizDeskBLL.GetDeskList();
                    break;
                case Command.ANDROID_SUBMIT_ORDER:
                    byteRet = SalesOrderBLL.SubmitOrderInAndroid(itemBuffer);
                    break;
                case Command.ANDROID_GET_GOODSIMAGE:
                    byteRet = GoodsBLL.GetGoodsImage(itemBuffer);
                    break;
                case Command.ANDROID_GET_TOPSALEGOODS:
                    byteRet = SysBasicDataBLL.GetTopSaleGoodsInAndroid();
                    break;

                default:
                    byteRet = new byte[0];
                    break;
            }
            return byteRet;
        }
    }
}
