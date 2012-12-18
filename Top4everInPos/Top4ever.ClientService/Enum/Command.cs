using System;

namespace Top4ever.ClientService.Enum
{
    public enum Command
    {
        ID_USERLOGIN = 1,
        ID_GET_SYSBASICDATA = 2,
        ID_GET_DESKBYNAME = 3,
        ID_UPDATE_DESKSTATUS = 4,
        ID_CREATE_SALESORDER = 5,
        ID_GET_SALESORDER = 6,
        ID_UPDATE_SALESORDER = 7,
        ID_PAYFORORDER = 8,
        ID_DELETE_SINGLEORDER = 9,
        ID_DELETE_WHOLEORDER = 10,
        ID_GET_DESKREALTIMEINFO = 11,
        ID_UPDATE_LADEORDERDETAILS = 12,
        ID_GET_ORDERLIST = 13,
        ID_SPLIT_SALESORDER = 14,
        ID_ORDER_DESKOPERATE = 15,
        ID_CREATE_HANDOVER = 16,
        ID_CREATE_DAILYBALANCE = 17
    }
}
