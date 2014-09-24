using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Enum;
using Top4ever.Print;

namespace Top4ever.Pos.Feature
{
    public partial class FormItemSalesReport : Form
    {
        private readonly Dictionary<string, List<GroupPrice>> _dicItemPriceByGroup = new Dictionary<string, List<GroupPrice>>();

        public FormItemSalesReport()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            _dicItemPriceByGroup.Clear();
            dataGirdViewExt1.Rows.Clear();
            string beginDate = dateTimePicker1.Value.ToString("yyyy-MM-dd 00:00:00");
            string endDate = dateTimePicker2.Value.ToString("yyyy-MM-dd 23:59:59");
            IList<GroupPrice> groupPriceList = BusinessReportService.GetInstance().GetItemPriceListByGroup(beginDate, endDate);
            if (groupPriceList != null && groupPriceList.Count > 0)
            {
                decimal totalQty = 0M;
                decimal totalAmount = 0M;
                foreach (GroupPrice item in groupPriceList)
                {
                    totalQty += item.ItemsTotalQty;
                    totalAmount += item.ItemsTotalPrice;
                    if (_dicItemPriceByGroup.ContainsKey(item.GroupName))
                    {
                        _dicItemPriceByGroup[item.GroupName].Add(item);
                    }
                    else
                    {
                        List<GroupPrice> temp = new List<GroupPrice>();
                        temp.Add(item);
                        _dicItemPriceByGroup.Add(item.GroupName, temp);
                    }
                }
                foreach (KeyValuePair<string, List<GroupPrice>> item in _dicItemPriceByGroup)
                {
                    int index = dataGirdViewExt1.Rows.Add();
                    dataGirdViewExt1.Rows[index].Cells["colGroupName"].Value = "[" + item.Key + "]";
                    foreach (GroupPrice groupPrice in item.Value)
                    {
                        index = dataGirdViewExt1.Rows.Add();
                        dataGirdViewExt1.Rows[index].Cells["colItemsName"].Value = groupPrice.ItemsName;
                        dataGirdViewExt1.Rows[index].Cells["colItemsTotalQty"].Value = groupPrice.ItemsTotalQty;
                        dataGirdViewExt1.Rows[index].Cells["colItemsTotalPrice"].Value = groupPrice.ItemsTotalPrice;
                    }
                }
                txtTotalQty.Text = totalQty.ToString("f1");
                txtTotalAmount.Text = totalAmount.ToString("f2");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (_dicItemPriceByGroup.Count > 0)
            {
                List<String> printData = new List<String>();
                printData.Add(GetDataType2("店铺名称：", ConstantValuePool.CurrentShop.ShopName));
                printData.Add(GetDataType2("店铺编号：", ConstantValuePool.CurrentShop.ShopNo));
                printData.Add(GetDataType2("营业日：", DateTime.Now.ToString("yyyy-MM-dd")));
                printData.Add("  ");
                foreach (KeyValuePair<string, List<GroupPrice>> item in _dicItemPriceByGroup)
                {
                    printData.Add("[" + item.Key + "]");
                    foreach (GroupPrice groupPrice in item.Value)
                    {
                        printData.Add(GetDataType3(groupPrice.ItemsName, groupPrice.ItemsTotalQty.ToString("f1"), groupPrice.ItemsTotalPrice.ToString("f2")));
                    }
                    printData.Add("  ");
                }
                printData.Add("       合计数量：" + txtTotalQty.Text + "  合计金额：" + txtTotalAmount.Text);
                if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.DRIVER)
                {
                    string printerName = ConstantValuePool.BizSettingConfig.printConfig.Name;
                    DriverSinglePrint driverPrint = new DriverSinglePrint(printerName, "SpecimenLabel");
                    driverPrint.DoPrint(printData, new Font("simsun", 9F));
                }
            }
        }

        private string GetDataType2(string strText, string strValue)
        {
            string strNull = string.Empty;
            int len1st = CheckTextLength(strText);
            for (int i = 0; i < (26 - len1st); i++)
                strNull += " ";
            return strText + strNull + strValue;
        }

        private string GetDataType3(string strText, string strValue1st, string strValue2nd)
        {
            string result = string.Empty;

            string strNull = string.Empty;
            int len1st = CheckTextLength(strText);
            for (int i = 0; i < (26 - len1st); i++)
                strNull += " ";
            result += strText + strNull;

            strNull = string.Empty;
            int len2nd = CheckTextLength(strValue1st);
            for (int i = 0; i < (10 - len2nd); i++)
                strNull += " ";
            result += strValue1st + strNull + strValue2nd;

            return result;
        }

        private int CheckTextLength(string strText)
        {
            int len = 0;
            for (int i = 0; i < strText.Length; i++)
            {
                byte[] byte_len = Encoding.Default.GetBytes(strText.Substring(i, 1));
                if (byte_len.Length > 1)
                    len += 2; //如果长度大于1，是中文，占两个字节，+2
                else
                    len += 1;  //如果长度等于1，是英文，占一个字节，+1
            }
            return len;
        }
    }
}
