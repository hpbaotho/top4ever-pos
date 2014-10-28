using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Windows.Forms;

using Top4ever.Common;
using Top4ever.Entity;
using Top4ever.Entity.Config;
using Top4ever.Entity.Enum;
using Top4ever.Hardware;
using Top4ever.Hardware.ClientShow;

namespace VechsoftPos
{
    public partial class FormConfig : Form
    {
        private readonly List<ListItem> _listItem = new List<ListItem>();

        public FormConfig()
        {
            InitializeComponent();
            btnTestCashBox.DisplayColor = btnTestCashBox.BackColor;
            btnTestClientShow.DisplayColor = btnTestClientShow.BackColor;
            BindClientShowInfo();
            BindPrinterInfo();
            this.cmbPrinter.SelectedIndexChanged += new System.EventHandler(this.cmbPrinter_SelectedIndexChanged);
            //多语言
            InitMultiLanguageCombox();
            //营业方式
            ListItem item = new ListItem("堂食", Convert.ToString((int) ShopSaleType.DineIn));
            cmbSaleType.Items.Add(item);
            item = new ListItem("外卖", Convert.ToString((int) ShopSaleType.Takeout));
            cmbSaleType.Items.Add(item);
            item = new ListItem("堂食加外卖", Convert.ToString((int) ShopSaleType.DineInAndTakeout));
            cmbSaleType.Items.Add(item);
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {
            txtIP.Text = ConstantValuePool.BizSettingConfig.IPAddress;
            txtPort.Text = ConstantValuePool.BizSettingConfig.Port.ToString();
            txtDeviceNo.Text = ConstantValuePool.BizSettingConfig.DeviceNo;
            txtFont.Text = ConstantValuePool.BizSettingConfig.FontSize.ToString();
            cmbSaleType.SelectedIndex = GetIndexByValue(cmbSaleType, Convert.ToString((int) ConstantValuePool.BizSettingConfig.SaleType));
            txtBreakDays.Text = ConstantValuePool.BizSettingConfig.BreakDays.ToString();
            txtLoginImage.Text = ConstantValuePool.BizSettingConfig.LoginImagePath;
            txtDeskImage.Text = ConstantValuePool.BizSettingConfig.DeskImagePath;
            ckbSecondScreen.Checked = ConstantValuePool.BizSettingConfig.SecondScreenEnabled;
            txtVideoPath.Text = ConstantValuePool.BizSettingConfig.ScreenVideoPath;
            txtImagePath.Text = ConstantValuePool.BizSettingConfig.ScreenImagePath;
            if (ConstantValuePool.BizSettingConfig.UsePettyCash)
            {
                ckbPettyCash.Checked = true;
            }
            else
            {
                ckbPettyCash.Checked = false;
            }
            if (ConstantValuePool.BizSettingConfig.ShowBrevityCode)
            {
                ckbBriefCode.Checked = true;
            }
            else
            {
                ckbBriefCode.Checked = false;
            }
            if (ConstantValuePool.BizSettingConfig.TakeAwayCash)
            {
                ckbTakeAwayCash.Checked = true;
            }
            else
            {
                ckbTakeAwayCash.Checked = false;
            }
            if (ConstantValuePool.BizSettingConfig.ShowSoldOutQty)
            {
                ckbSoldOutQty.Checked = true;
            }
            else
            {
                ckbSoldOutQty.Checked = false;
            }
            if (ConstantValuePool.BizSettingConfig.DirectShipping)
            {
                ckbDirectShipping.Checked = true;
            }
            else
            {
                ckbDirectShipping.Checked = false;
            }
            if (ConstantValuePool.BizSettingConfig.CarteMode)
            {
                ckbCarteMode.Checked = true;
            }
            else
            {
                ckbCarteMode.Checked = false;
            }
            if (ConstantValuePool.BizSettingConfig.TakeoutPrint)
            {
                ckbTakeoutPrint.Checked = true;
            }
            else
            {
                ckbTakeoutPrint.Checked = false;
            }
            if (ConstantValuePool.BizSettingConfig.printConfig.Enabled)
            {
                ckbPrinter.Checked = true;
                if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.DRIVER)
                {
                    rbDriverPrinter.Checked = true;
                    cmbPrinter.Text = ConstantValuePool.BizSettingConfig.printConfig.Name;
                    cmbPaperName.Text = ConstantValuePool.BizSettingConfig.printConfig.PaperName;
                }
                if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.COM)
                {
                    rbPrinterPort.Checked = true;
                    cmbPrinterPort.Text = ConstantValuePool.BizSettingConfig.printConfig.Name;
                }
                if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.ETHERNET)
                {
                    rbNetPrinter.Checked = true;
                    txtIPAddress.Text = ConstantValuePool.BizSettingConfig.printConfig.Name;
                }
                if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.USB)
                {
                    rbUsbPort.Checked = true;
                    txtPrinterVID.Text = ConstantValuePool.BizSettingConfig.printConfig.VID;
                    txtPrinterPID.Text = ConstantValuePool.BizSettingConfig.printConfig.PID;
                    txtEndpointId.Text = ConstantValuePool.BizSettingConfig.printConfig.EndpointID;
                }
                txtCopies.Text = ConstantValuePool.BizSettingConfig.printConfig.Copies.ToString();
                txtOrderCopies.Text = ConstantValuePool.BizSettingConfig.printConfig.OrderCopies.ToString();
                cmbPaperWidth.Text = ConstantValuePool.BizSettingConfig.printConfig.PaperWidth;
            }
            else
            {
                ckbPrinter.Checked = false;
                rbDriverPrinter.Enabled = false;
                rbPrinterPort.Enabled = false;
                cmbPrinter.Enabled = false;
                cmbPrinterPort.Enabled = false;
                rbNetPrinter.Enabled = false;
                txtIPAddress.Enabled = false;
                rbUsbPort.Enabled = false;
                txtPrinterVID.Enabled = false;
                txtPrinterPID.Enabled = false;
                txtEndpointId.Enabled = false;
                txtCopies.Text = ConstantValuePool.BizSettingConfig.printConfig.Copies.ToString();
                txtOrderCopies.Text = ConstantValuePool.BizSettingConfig.printConfig.OrderCopies.ToString();
                txtCopies.Enabled = false;
                txtOrderCopies.Enabled = false;
                cmbPaperWidth.Enabled = false;
            }
            if (ConstantValuePool.BizSettingConfig.cashBoxConfig.Enabled)
            {
                ckbCashDrawer.Checked = true;
                if (ConstantValuePool.BizSettingConfig.cashBoxConfig.IsUsbPort)
                {
                    rbUsbCashDrawer.Checked = true;
                    txtCashVID.Text = ConstantValuePool.BizSettingConfig.cashBoxConfig.VID;
                    txtCashPID.Text = ConstantValuePool.BizSettingConfig.cashBoxConfig.PID;
                    txtCashEndpoint.Text = ConstantValuePool.BizSettingConfig.cashBoxConfig.EndpointID;
                }
                else
                {
                    rbCashDrawer.Checked = true;
                    cmbCashDrawerPort.Text = ConstantValuePool.BizSettingConfig.cashBoxConfig.Port;
                }
            }
            else
            {
                btnTestCashBox.Enabled = false;
                btnTestCashBox.BackColor = ConstantValuePool.DisabledColor;
                rbCashDrawer.Enabled = false;
                cmbCashDrawerPort.Enabled = false;
                rbUsbCashDrawer.Enabled = false;
                txtCashVID.Enabled = false;
                txtCashPID.Enabled = false;
                txtCashEndpoint.Enabled = false;
            }
            if (ConstantValuePool.BizSettingConfig.telCallConfig.Enabled)
            {
                ckbTelCall.Enabled = true;
                cmbTelCallModel.Enabled = true;
                cmbTelCallModel.SelectedIndex = ConstantValuePool.BizSettingConfig.telCallConfig.Model;
            }
            else
            {
                ckbTelCall.Enabled = false;
                cmbTelCallModel.Enabled = false;
            }
            if (ConstantValuePool.BizSettingConfig.clientShowConfig.Enabled)
            {
                ckbClientShow.Checked = true;
                if (ConstantValuePool.BizSettingConfig.clientShowConfig.IsUsbPort)
                {
                    rbUsbClientShow.Checked = true;
                    txtClientVID.Text = ConstantValuePool.BizSettingConfig.clientShowConfig.VID;
                    txtClientPID.Text = ConstantValuePool.BizSettingConfig.clientShowConfig.PID;
                    txtClientEndpoint.Text = ConstantValuePool.BizSettingConfig.clientShowConfig.EndpointID;
                }
                else
                {
                    rbClientShow.Checked = true;
                    cmbClientShowPort.Text = ConstantValuePool.BizSettingConfig.clientShowConfig.Port;
                }
                cmbClientShowModel.Text = ConstantValuePool.BizSettingConfig.clientShowConfig.ClientShowModel;
                cmbClientShowType.SelectedIndex = GetIndexByValue(cmbClientShowType, ConstantValuePool.BizSettingConfig.clientShowConfig.ClientShowType);
            }
            else
            {
                btnTestClientShow.Enabled = false;
                btnTestClientShow.BackColor = ConstantValuePool.DisabledColor;
                ckbClientShow.Checked = false;
                rbClientShow.Enabled = false;
                cmbClientShowPort.Enabled = false;
                rbUsbClientShow.Enabled = false;
                txtClientVID.Enabled = false;
                txtClientPID.Enabled = false;
                txtClientEndpoint.Enabled = false;
                cmbClientShowModel.Enabled = false;
            }
            foreach (BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
            {
                if (control.Name == "Group")
                {
                    txtGoodsGroupRows.Text = control.RowsCount.ToString();
                    txtGoodsGroupRows.Enabled = false;
                    txtGoodsGroupColumns.Text = control.ColumnsCount.ToString();
                    txtGoodsGroupColumns.Enabled = false;
                }
                else if (control.Name == "Item")
                {
                    txtGoodsRows.Text = control.RowsCount.ToString();
                    txtGoodsRows.Enabled = false;
                    txtGoodsColumns.Text = control.ColumnsCount.ToString();
                    txtGoodsColumns.Enabled = false;
                }
                else if (control.Name == "Payoff")
                {
                    txtPayoffWayRows.Text = control.RowsCount.ToString();
                    txtPayoffWayRows.Enabled = false;
                    txtPayoffWayColumns.Text = control.ColumnsCount.ToString();
                    txtPayoffWayColumns.Enabled = false;
                }
            }
        }

        private void BindClientShowInfo()
        {
            cmbClientShowModel.Items.Add("CD7110");
            cmbClientShowModel.Items.Add("Led8N");
            cmbClientShowModel.Items.Add("Senor");
            cmbClientShowModel.Items.Add("VFD220E");
            ListItem item = new ListItem("单排", "SingleLine");
            cmbClientShowType.Items.Add(item);
            item = new ListItem("双排", "DualLine");
            cmbClientShowType.Items.Add(item);
        }

        private void BindPrinterInfo()
        {
            List<string> alPrinters = Printer.GetPrinterList();
            for (int i = 0; i < alPrinters.Count; i++)
            {
                cmbPrinter.Items.Add(alPrinters[i]);
            }
            List<string> alPorts = Printer.GetPortList();
            for (int i = 0; i < alPorts.Count; i++)
            {
                cmbPrinterPort.Items.Add(alPorts[i]);
                cmbCashDrawerPort.Items.Add(alPorts[i]);
                cmbClientShowPort.Items.Add(alPorts[i]);
            }
        }

        private void InitMultiLanguageCombox()
        {
            switch (ConstantValuePool.BizSettingConfig.Languge1st)
            {
                case LanguageType.SIMPLIFIED:
                    cmbLanguge1st.SelectedIndexChanged -= new System.EventHandler(cmbLanguge1st_SelectedIndexChanged);
                    cmbLanguge2nd.SelectedIndexChanged -= new System.EventHandler(cmbLanguge2nd_SelectedIndexChanged);
                    _listItem.Clear();
                    _listItem.Add(new ListItem("简体", Convert.ToString((int) LanguageType.SIMPLIFIED)));
                    _listItem.Add(new ListItem("繁体", Convert.ToString((int) LanguageType.TRADITIONAL)));
                    _listItem.Add(new ListItem("英文", Convert.ToString((int) LanguageType.ENGLISH)));
                    BindFirstLanguage();
                    cmbLanguge1st.SelectedIndex = GetIndexByValue(cmbLanguge1st, Convert.ToString((int) ConstantValuePool.BizSettingConfig.Languge1st));
                    BindSecondLanguage();
                    cmbLanguge2nd.SelectedIndex = GetIndexByValue(cmbLanguge2nd, Convert.ToString((int) ConstantValuePool.BizSettingConfig.Languge2nd));
                    cmbLanguge1st.SelectedIndexChanged += new System.EventHandler(cmbLanguge1st_SelectedIndexChanged);
                    cmbLanguge2nd.SelectedIndexChanged += new System.EventHandler(cmbLanguge2nd_SelectedIndexChanged);
                    break;
                case LanguageType.TRADITIONAL:
                    cmbLanguge1st.SelectedIndexChanged -= new System.EventHandler(cmbLanguge1st_SelectedIndexChanged);
                    cmbLanguge2nd.SelectedIndexChanged -= new System.EventHandler(cmbLanguge2nd_SelectedIndexChanged);
                    _listItem.Clear();
                    _listItem.Add(new ListItem("簡體", Convert.ToString((int) LanguageType.SIMPLIFIED)));
                    _listItem.Add(new ListItem("繁體", Convert.ToString((int) LanguageType.TRADITIONAL)));
                    _listItem.Add(new ListItem("英文", Convert.ToString((int) LanguageType.ENGLISH)));
                    BindFirstLanguage();
                    cmbLanguge1st.SelectedIndex = GetIndexByValue(cmbLanguge1st, Convert.ToString((int) ConstantValuePool.BizSettingConfig.Languge1st));
                    BindSecondLanguage();
                    cmbLanguge2nd.SelectedIndex = GetIndexByValue(cmbLanguge2nd, Convert.ToString((int) ConstantValuePool.BizSettingConfig.Languge2nd));
                    cmbLanguge1st.SelectedIndexChanged += new System.EventHandler(cmbLanguge1st_SelectedIndexChanged);
                    cmbLanguge2nd.SelectedIndexChanged += new System.EventHandler(cmbLanguge2nd_SelectedIndexChanged);
                    break;
                case LanguageType.ENGLISH:
                    cmbLanguge1st.SelectedIndexChanged -= new System.EventHandler(cmbLanguge1st_SelectedIndexChanged);
                    cmbLanguge2nd.SelectedIndexChanged -= new System.EventHandler(cmbLanguge2nd_SelectedIndexChanged);
                    _listItem.Clear();
                    _listItem.Add(new ListItem("Chinese-Simplified", Convert.ToString((int) LanguageType.SIMPLIFIED)));
                    _listItem.Add(new ListItem("Chinese-Traditional", Convert.ToString((int) LanguageType.TRADITIONAL)));
                    _listItem.Add(new ListItem("English", Convert.ToString((int) LanguageType.ENGLISH)));
                    BindFirstLanguage();
                    cmbLanguge1st.SelectedIndex = GetIndexByValue(cmbLanguge1st, Convert.ToString((int) ConstantValuePool.BizSettingConfig.Languge1st));
                    BindSecondLanguage();
                    cmbLanguge2nd.SelectedIndex = GetIndexByValue(cmbLanguge2nd, Convert.ToString((int) ConstantValuePool.BizSettingConfig.Languge2nd));
                    cmbLanguge1st.SelectedIndexChanged += new System.EventHandler(cmbLanguge1st_SelectedIndexChanged);
                    cmbLanguge2nd.SelectedIndexChanged += new System.EventHandler(cmbLanguge2nd_SelectedIndexChanged);
                    break;
            }
        }

        private void BindFirstLanguage()
        {
            cmbLanguge1st.Items.Clear();
            foreach (ListItem item in _listItem)
            {
                cmbLanguge1st.Items.Add(item);
            }
        }

        private void BindSecondLanguage()
        {
            cmbLanguge2nd.Items.Clear();
            foreach (ListItem item in _listItem)
            {
                if (int.Parse(item.Value) != (int) ConstantValuePool.BizSettingConfig.Languge1st)
                {
                    cmbLanguge2nd.Items.Add(item);
                }
            }
        }

        private void cmbLanguge1st_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem item = cmbLanguge1st.SelectedItem as ListItem;
            ConstantValuePool.BizSettingConfig.Languge1st = (LanguageType) int.Parse(item.Value);
            BindSecondLanguage();
            cmbLanguge2nd.SelectedIndex = 0;

            InitMultiLanguageCombox();
        }

        private void cmbLanguge2nd_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem item = cmbLanguge2nd.SelectedItem as ListItem;
            ConstantValuePool.BizSettingConfig.Languge2nd = (LanguageType) int.Parse(item.Value);
        }

        private int GetIndexByValue(ComboBox cb, string value)
        {
            int index = 0, selectedIndex = -1;
            foreach (object obj in cb.Items)
            {
                if (((ListItem) obj).Value == value)
                {
                    selectedIndex = index;
                    break;
                }
                index++;
            }
            return selectedIndex;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //布局
            List<BizControl> bizControls = new List<BizControl>();
            BizControl control = new BizControl
            {
                Name = "Group",
                RowsCount = int.Parse(txtGoodsGroupRows.Text.Trim()),
                ColumnsCount = int.Parse(txtGoodsGroupColumns.Text.Trim())
            };
            bizControls.Add(control);
            control = new BizControl
            {
                Name = "Item",
                RowsCount = int.Parse(txtGoodsRows.Text.Trim()),
                ColumnsCount = int.Parse(txtGoodsColumns.Text.Trim())
            };
            bizControls.Add(control);
            control = new BizControl
            {
                Name = "Payoff",
                RowsCount = int.Parse(txtPayoffWayRows.Text.Trim()),
                ColumnsCount = int.Parse(txtPayoffWayColumns.Text.Trim())
            };
            bizControls.Add(control);
            //打印
            PrintConfig printConfig = new PrintConfig {Enabled = ckbPrinter.Checked};
            if (ckbPrinter.Checked)
            {
                if (rbDriverPrinter.Checked)
                {
                    printConfig.PrinterPort = PortType.DRIVER;
                    printConfig.Name = cmbPrinter.Text;
                    printConfig.PaperName = cmbPaperName.Text;
                    if (string.IsNullOrEmpty(printConfig.Name))
                    {
                        MessageBox.Show("打印机名称不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(printConfig.PaperName))
                    {
                        MessageBox.Show("纸张名称不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (rbPrinterPort.Checked)
                {
                    printConfig.PrinterPort = PortType.COM;
                    printConfig.Name = cmbPrinterPort.Text;
                    if (string.IsNullOrEmpty(printConfig.Name))
                    {
                        MessageBox.Show("端口号不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (rbNetPrinter.Checked)
                {
                    printConfig.PrinterPort = PortType.ETHERNET;
                    printConfig.Name = txtIPAddress.Text;
                    if (string.IsNullOrEmpty(printConfig.Name))
                    {
                        MessageBox.Show("IP地址不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (rbUsbPort.Checked)
                {
                    printConfig.PrinterPort = PortType.USB;
                    printConfig.VID = txtPrinterVID.Text.Trim();
                    printConfig.PID = txtPrinterPID.Text.Trim();
                    printConfig.EndpointID = txtEndpointId.Text.Trim();
                    if (string.IsNullOrEmpty(printConfig.VID))
                    {
                        MessageBox.Show("VID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(printConfig.PID))
                    {
                        MessageBox.Show("PID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(printConfig.EndpointID))
                    {
                        MessageBox.Show("EndpointID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                int copies;
                int.TryParse(txtCopies.Text.Trim(), out copies);
                printConfig.Copies = copies > 0 ? copies : 1;
                int orderCopies;
                int.TryParse(txtOrderCopies.Text.Trim(), out orderCopies);
                printConfig.OrderCopies = orderCopies > 0 ? orderCopies : 1;
                printConfig.PaperWidth = cmbPaperWidth.Text.Trim();
            }
            //钱箱
            CashBoxConfig cashboxConfig = new CashBoxConfig();
            cashboxConfig.Enabled = ckbCashDrawer.Checked;
            if (ckbCashDrawer.Checked)
            {
                if (rbCashDrawer.Checked)
                {
                    cashboxConfig.IsUsbPort = false;
                    cashboxConfig.Port = cmbCashDrawerPort.Text;
                    if (string.IsNullOrEmpty(cashboxConfig.Port))
                    {
                        MessageBox.Show("端口号不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (rbUsbCashDrawer.Checked)
                {
                    cashboxConfig.IsUsbPort = true;
                    cashboxConfig.VID = txtCashVID.Text.Trim();
                    cashboxConfig.PID = txtCashPID.Text.Trim();
                    cashboxConfig.EndpointID = txtCashEndpoint.Text.Trim();
                    if (string.IsNullOrEmpty(cashboxConfig.VID))
                    {
                        MessageBox.Show("VID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(cashboxConfig.PID))
                    {
                        MessageBox.Show("PID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(cashboxConfig.EndpointID))
                    {
                        MessageBox.Show("EndpointID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            //来电宝
            TelCallConfig telCallConfig = new TelCallConfig {Enabled = ckbTelCall.Checked};
            if (ckbTelCall.Checked)
            {
                telCallConfig.Model = cmbTelCallModel.SelectedIndex;
                if (telCallConfig.Model == 0)
                {
                    if (!LDT.Plugin_Tel(1))
                    {
                        ConstantValuePool.IsTelCallWorking = false;
                        MessageBox.Show("来电宝设备没有接入POS机！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    for (int i = 1; i <= LDT.DevCount_Tel(); i++)
                    {
                        if (LDT.Begin_Tel(i, '1') == 0)
                        {
                            ConstantValuePool.IsTelCallWorking = false;
                            MessageBox.Show("开启来电宝设备失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                        else
                        {
                            ConstantValuePool.TelCallID = i;
                        }
                    }
                }
                if (telCallConfig.Model == 1)
                {
                    if (LDT.Begin_Tel(64, '1') == 0)
                    {
                        ConstantValuePool.IsTelCallWorking = false;
                        MessageBox.Show("开启来电宝设备失败！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        ConstantValuePool.TelCallID = 64;
                    }
                }
            }
            //客显
            ClientShowConfig clientShowConfig = new ClientShowConfig {Enabled = ckbClientShow.Checked};
            if (ckbClientShow.Checked)
            {
                if (rbClientShow.Checked)
                {
                    clientShowConfig.IsUsbPort = false;
                    clientShowConfig.Port = cmbClientShowPort.Text;
                    if (string.IsNullOrEmpty(clientShowConfig.Port))
                    {
                        MessageBox.Show("端口号不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (rbUsbClientShow.Checked)
                {
                    clientShowConfig.IsUsbPort = true;
                    clientShowConfig.VID = txtClientVID.Text.Trim();
                    clientShowConfig.PID = txtClientPID.Text.Trim();
                    clientShowConfig.EndpointID = txtClientEndpoint.Text.Trim();
                    if (string.IsNullOrEmpty(clientShowConfig.VID))
                    {
                        MessageBox.Show("VID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(clientShowConfig.PID))
                    {
                        MessageBox.Show("PID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(clientShowConfig.EndpointID))
                    {
                        MessageBox.Show("EndpointID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                string clientShowModel = cmbClientShowModel.Text;
                ListItem item = cmbClientShowType.SelectedItem as ListItem;
                if (item.Value == "SingleLine")
                {
                    if (clientShowModel == "VFD220E")
                    {
                        MessageBox.Show(string.Format("抱歉，程序暂时不支持'{0}'型号的{1}客显！", clientShowModel, "单排"));
                        return;
                    }
                }
                if (item.Value == "DualLine")
                {
                    if (clientShowModel == "CD7110Type" || clientShowModel == "Led8N")
                    {
                        MessageBox.Show(string.Format("抱歉，程序暂时不支持'{0}'型号的{1}客显！", clientShowModel, "双排"));
                        return;
                    }
                }
                clientShowConfig.ClientShowModel = clientShowModel;
                clientShowConfig.ClientShowType = item.Value;
            }
            //组装
            AppSettingConfig appConfig = new AppSettingConfig();
            appConfig.IPAddress = txtIP.Text;
            appConfig.Port = int.Parse(txtPort.Text);
            appConfig.DeviceNo = txtDeviceNo.Text;
            //多语言
            ListItem item1 = cmbLanguge1st.SelectedItem as ListItem;
            ListItem item2 = cmbLanguge2nd.SelectedItem as ListItem;
            appConfig.Languge1st = (LanguageType) int.Parse(item1.Value);
            appConfig.Languge2nd = (LanguageType) int.Parse(item2.Value);
            appConfig.FontSize = float.Parse(txtFont.Text);
            ListItem itemType = cmbSaleType.SelectedItem as ListItem;
            appConfig.SaleType = (ShopSaleType) int.Parse(itemType.Value);
            appConfig.BreakDays = int.Parse(txtBreakDays.Text);
            appConfig.LoginImagePath = txtLoginImage.Text.Trim();
            appConfig.DeskImagePath = txtDeskImage.Text.Trim();
            appConfig.SecondScreenEnabled = ckbSecondScreen.Checked;
            appConfig.ScreenVideoPath = txtVideoPath.Text.Trim();
            appConfig.ScreenImagePath = txtImagePath.Text.Trim();
            appConfig.UsePettyCash = ckbPettyCash.Checked;
            appConfig.ShowBrevityCode = ckbBriefCode.Checked;
            appConfig.TakeAwayCash = ckbTakeAwayCash.Checked;
            appConfig.ShowSoldOutQty = ckbSoldOutQty.Checked;
            appConfig.DirectShipping = ckbDirectShipping.Checked;
            appConfig.CarteMode = ckbCarteMode.Checked;
            appConfig.TakeoutPrint = ckbTakeoutPrint.Checked;
            appConfig.bizUIConfig = new BizUIConfig {BizControls = bizControls};
            appConfig.printConfig = printConfig;
            appConfig.cashBoxConfig = cashboxConfig;
            appConfig.telCallConfig = telCallConfig;
            appConfig.clientShowConfig = clientShowConfig;
            //存储在静态对象中
            ConstantValuePool.BizSettingConfig = appConfig;
            //输出到文件
            XmlUtil.Serialize<AppSettingConfig>(appConfig, "Config/AppSetting.config");
            MessageBox.Show("保存成功，请退出系统后重新登录！");
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ckbGoodsGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbGoodsGroup.Checked)
            {
                txtGoodsGroupRows.Enabled = true;
                txtGoodsGroupColumns.Enabled = true;
            }
            else
            {
                txtGoodsGroupRows.Enabled = false;
                txtGoodsGroupColumns.Enabled = false;
            }
        }

        private void ckbGoods_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbGoods.Checked)
            {
                txtGoodsRows.Enabled = true;
                txtGoodsColumns.Enabled = true;
            }
            else
            {
                txtGoodsRows.Enabled = false;
                txtGoodsColumns.Enabled = false;
            }
        }

        private void ckbPayoffWay_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbPayoffWay.Checked)
            {
                txtPayoffWayRows.Enabled = true;
                txtPayoffWayColumns.Enabled = true;
            }
            else
            {
                txtPayoffWayRows.Enabled = false;
                txtPayoffWayColumns.Enabled = false;
            }
        }

        private void ckbPrinter_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbPrinter.Checked)
            {
                rbDriverPrinter.Enabled = true;
                rbPrinterPort.Enabled = true;
                cmbPrinter.Enabled = true;
                cmbPrinterPort.Enabled = true;
                rbNetPrinter.Enabled = true;
                txtIPAddress.Enabled = true;
                rbUsbPort.Enabled = true;
                txtPrinterVID.Enabled = true;
                txtPrinterPID.Enabled = true;
                txtEndpointId.Enabled = true;
                txtCopies.Enabled = true;
                txtOrderCopies.Enabled = true;
                cmbPaperWidth.Enabled = true;
            }
            else
            {
                rbDriverPrinter.Enabled = false;
                rbPrinterPort.Enabled = false;
                cmbPrinter.Enabled = false;
                cmbPrinterPort.Enabled = false;
                rbNetPrinter.Enabled = false;
                txtIPAddress.Enabled = false;
                rbUsbPort.Enabled = false;
                txtPrinterVID.Enabled = false;
                txtPrinterPID.Enabled = false;
                txtEndpointId.Enabled = false;
                txtCopies.Enabled = false;
                txtOrderCopies.Enabled = false;
                cmbPaperWidth.Enabled = false;
            }
        }

        private void ckbCashDrawer_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbCashDrawer.Checked)
            {
                btnTestCashBox.Enabled = true;
                btnTestCashBox.BackColor = btnTestCashBox.DisplayColor;
                rbCashDrawer.Enabled = true;
                cmbCashDrawerPort.Enabled = true;
                rbUsbCashDrawer.Enabled = true;
                txtCashVID.Enabled = true;
                txtCashPID.Enabled = true;
                txtCashEndpoint.Enabled = true;
            }
            else
            {
                btnTestCashBox.Enabled = false;
                btnTestCashBox.BackColor = ConstantValuePool.DisabledColor;
                rbCashDrawer.Enabled = false;
                cmbCashDrawerPort.Enabled = false;
                rbUsbCashDrawer.Enabled = false;
                txtCashVID.Enabled = false;
                txtCashPID.Enabled = false;
                txtCashEndpoint.Enabled = false;
            }
        }

        private void ckbTelCall_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbTelCall.Checked)
            {
                cmbTelCallModel.Enabled = true;
            }
            else
            {
                cmbTelCallModel.Enabled = false;
            }
        }

        private void ckbClientShow_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbClientShow.Checked)
            {
                btnTestClientShow.Enabled = true;
                btnTestClientShow.BackColor = btnTestClientShow.DisplayColor;
                rbClientShow.Enabled = true;
                cmbClientShowPort.Enabled = true;
                rbUsbClientShow.Enabled = true;
                txtClientVID.Enabled = true;
                txtClientPID.Enabled = true;
                txtClientEndpoint.Enabled = true;
                cmbClientShowModel.Enabled = true;
                cmbClientShowType.Enabled = true;
            }
            else
            {
                btnTestClientShow.Enabled = false;
                btnTestClientShow.BackColor = ConstantValuePool.DisabledColor;
                rbClientShow.Enabled = false;
                cmbClientShowPort.Enabled = false;
                rbUsbClientShow.Enabled = false;
                txtClientVID.Enabled = false;
                txtClientPID.Enabled = false;
                txtClientEndpoint.Enabled = false;
                cmbClientShowModel.Enabled = false;
                cmbClientShowType.Enabled = false;
            }
        }

        private void btnLoadLogin_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog1 = new OpenFileDialog();
            fileDialog1.InitialDirectory = Application.StartupPath; //初始目录
            fileDialog1.Filter = "picture（*.jpg;*.bmp;*.gif）|*.jpg;*.bmp;*.gif|All files (*.*)|*.*"; //文件的类型
            fileDialog1.FilterIndex = 1;
            fileDialog1.RestoreDirectory = true;
            if (fileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtLoginImage.Text = fileDialog1.FileName;
            }
        }

        private void btnLoadDesk_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog1 = new OpenFileDialog();
            fileDialog1.InitialDirectory = Application.StartupPath; //初始目录
            fileDialog1.Filter = "picture（*.jpg;*.bmp;*.gif）|*.jpg;*.bmp;*.gif|All files (*.*)|*.*"; //文件的类型
            fileDialog1.FilterIndex = 1;
            fileDialog1.RestoreDirectory = true;
            if (fileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDeskImage.Text = fileDialog1.FileName;
            }
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtVideoPath.Text = dialog.SelectedPath;
            }
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            string strPath = txtImagePath.Text;
            OpenFileDialog fileDialog1 = new OpenFileDialog();
            fileDialog1.InitialDirectory = strPath; //初始目录
            fileDialog1.Filter = "picture（*.jpg;*.bmp;*.gif）|*.jpg;*.bmp;*.gif|All files (*.*)|*.*"; //文件的类型
            fileDialog1.FilterIndex = 1;
            fileDialog1.RestoreDirectory = true;
            if (fileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtImagePath.Text = fileDialog1.FileName;
            }
        }

        private void ckbSecondScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbSecondScreen.Checked)
            {
                txtVideoPath.Enabled = true;
                txtImagePath.Enabled = true;
                btnLoadFile.Enabled = true;
                btnLoadImage.Enabled = true;
            }
            else
            {
                txtVideoPath.Enabled = false;
                txtImagePath.Enabled = false;
                btnLoadFile.Enabled = false;
                btnLoadImage.Enabled = false;
            }
        }

        private void rbDriverPrinter_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDriverPrinter.Checked)
            {
                cmbPrinter.Enabled = true;
                cmbPrinterPort.Enabled = false;
                txtIPAddress.Enabled = false;
                txtPrinterVID.Enabled = false;
                txtPrinterPID.Enabled = false;
                txtEndpointId.Enabled = false;
                cmbPaperName.Enabled = true;
            }
        }

        private void rbPrinterPort_CheckedChanged(object sender, EventArgs e)
        {
            cmbPrinter.Enabled = false;
            cmbPrinterPort.Enabled = true;
            txtIPAddress.Enabled = false;
            txtPrinterVID.Enabled = false;
            txtPrinterPID.Enabled = false;
            txtEndpointId.Enabled = false;
            cmbPaperName.Enabled = false;
        }

        private void rbNetPrinter_CheckedChanged(object sender, EventArgs e)
        {
            cmbPrinter.Enabled = false;
            cmbPrinterPort.Enabled = false;
            txtIPAddress.Enabled = true;
            txtPrinterVID.Enabled = false;
            txtPrinterPID.Enabled = false;
            txtEndpointId.Enabled = false;
            cmbPaperName.Enabled = false;
        }

        private void rbUsbPort_CheckedChanged(object sender, EventArgs e)
        {
            cmbPrinter.Enabled = false;
            cmbPrinterPort.Enabled = false;
            txtIPAddress.Enabled = false;
            txtPrinterVID.Enabled = true;
            txtPrinterPID.Enabled = true;
            txtEndpointId.Enabled = true;
            cmbPaperName.Enabled = false;
        }

        private void rbCashDrawer_CheckedChanged(object sender, EventArgs e)
        {
            cmbCashDrawerPort.Enabled = true;
            txtCashVID.Enabled = false;
            txtCashPID.Enabled = false;
            txtCashEndpoint.Enabled = false;
        }

        private void rbUsbCashDrawer_CheckedChanged(object sender, EventArgs e)
        {
            cmbCashDrawerPort.Enabled = false;
            txtCashVID.Enabled = true;
            txtCashPID.Enabled = true;
            txtCashEndpoint.Enabled = true;
        }

        private void rbClientShow_CheckedChanged(object sender, EventArgs e)
        {
            cmbClientShowPort.Enabled = true;
            txtClientVID.Enabled = false;
            txtClientPID.Enabled = false;
            txtClientEndpoint.Enabled = false;
        }

        private void rbUsbClientShow_CheckedChanged(object sender, EventArgs e)
        {
            cmbClientShowPort.Enabled = false;
            txtClientVID.Enabled = true;
            txtClientPID.Enabled = true;
            txtClientEndpoint.Enabled = true;
        }

        private void cmbPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox printer = sender as ComboBox;
            if (printer == null) return;
            string printerName = printer.Text;
            PrintDocument printDocument = new PrintDocument();
            if (!string.IsNullOrEmpty(printerName))
            {
                printDocument.PrinterSettings.PrinterName = printerName;
            }
            cmbPaperName.Items.Clear();
            foreach (PaperSize ps in printDocument.PrinterSettings.PaperSizes)
            {
                cmbPaperName.Items.Add(ps.PaperName);
            }
        }

        private void btnTestCashBox_Click(object sender, EventArgs e)
        {
            if (!ckbCashDrawer.Checked || (!rbCashDrawer.Checked && !rbUsbCashDrawer.Checked))
            {
                MessageBox.Show("请先对钱箱进行设置！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (rbCashDrawer.Checked)
                {
                    string port = cmbCashDrawerPort.Text;
                    if (string.IsNullOrEmpty(port))
                    {
                        MessageBox.Show("端口号不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (port.IndexOf("COM", StringComparison.OrdinalIgnoreCase) >= 0 && port.Length > 3)
                    {
                        if (port.Substring(0, 3).ToUpper() == "COM")
                        {
                            string portName = port.Substring(0, 4).ToUpper();
                            CashBox.Open(portName, 9600, Parity.None, 8, StopBits.One);
                        }
                    }
                    else if (port.IndexOf("LPT", StringComparison.OrdinalIgnoreCase) >= 0 && port.Length > 3)
                    {
                        if (port.Substring(0, 3).ToUpper() == "LPT")
                        {
                            string lptName = port.Substring(0, 4).ToUpper();
                            CashBox.Open(lptName);
                        }
                    }
                }
                if (rbUsbCashDrawer.Checked)
                {
                    string vid = txtCashVID.Text.Trim();
                    string pid = txtCashPID.Text.Trim();
                    string endpointId = txtCashEndpoint.Text.Trim();
                    if (string.IsNullOrEmpty(vid))
                    {
                        MessageBox.Show("VID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(pid))
                    {
                        MessageBox.Show("PID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(endpointId))
                    {
                        MessageBox.Show("EndpointID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    CashBox.Open(vid, pid, endpointId);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("钱箱测试打开失败，错误信息：" + exception, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTestClientShow_Click(object sender, EventArgs e)
        {
            if (!ckbClientShow.Checked || (!rbClientShow.Checked && !rbUsbClientShow.Checked))
            {
                MessageBox.Show("请先对客显进行设置！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ListItem item = cmbClientShowType.SelectedItem as ListItem;
            if (item == null) return;

            string clientShowModel = cmbClientShowModel.Text;
            string clientShowType = item.Value;
            if (clientShowType.Equals("SingleLine", StringComparison.OrdinalIgnoreCase))
            {
                if (clientShowModel.Equals("VFD220E", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(string.Format("抱歉，程序暂时不支持'{0}'型号的{1}客显！", clientShowModel, "单排"));
                    return;
                }
            }
            else if (clientShowType.Equals("DualLine", StringComparison.OrdinalIgnoreCase))
            {
                if (clientShowModel.Equals("CD7110Type", StringComparison.OrdinalIgnoreCase) || clientShowModel.Equals("Led8N", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(string.Format("抱歉，程序暂时不支持'{0}'型号的{1}客显！", clientShowModel, "双排"));
                    return;
                }
            }
            try
            {
                if (rbClientShow.Checked)
                {
                    string port = cmbClientShowPort.Text;
                    if (string.IsNullOrEmpty(port))
                    {
                        MessageBox.Show("端口号不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (port.IndexOf("COM", StringComparison.OrdinalIgnoreCase) >= 0 && port.Length > 3)
                    {
                        if (port.Substring(0, 3).ToUpper() == "COM")
                        {
                            string portName = port.Substring(0, 4).ToUpper();
                            //显示
                            if (clientShowType.Equals("SingleLine", StringComparison.OrdinalIgnoreCase))
                            {
                                if (clientShowModel.Equals("CD7110", StringComparison.OrdinalIgnoreCase))
                                {
                                    CD7110Type cd7110Type = new CD7110Type(portName, 9600, Parity.None, 8, StopBits.One);
                                    cd7110Type.ShowReceipt("100.00");
                                }
                                else if (clientShowModel.Equals("Led8N", StringComparison.OrdinalIgnoreCase))
                                {
                                    Led8NType led8NType = new Led8NType(portName, 9600, Parity.None, 8, StopBits.One);
                                    led8NType.ShowReceipt("100.00");
                                }
                                else if (clientShowModel.Equals("Senor", StringComparison.OrdinalIgnoreCase))
                                {
                                    SenorSingleType senorSingleType = new SenorSingleType(portName, 9600, Parity.None, 8, StopBits.One);
                                    senorSingleType.ShowReceipt("100.00");
                                }
                            }
                            else if (clientShowType.Equals("DualLine", StringComparison.OrdinalIgnoreCase))
                            {
                                if (clientShowModel.Equals("Senor", StringComparison.OrdinalIgnoreCase))
                                {
                                    SenorDualType senorDualType = new SenorDualType(portName, 9600, Parity.None, 8, StopBits.One);
                                    senorDualType.ShowUnitAndTotalPrice("88.00", "188.00");
                                }
                                else if (clientShowModel.Equals("VFD220E", StringComparison.OrdinalIgnoreCase))
                                {
                                    VFD220EType vfd220EType = new VFD220EType(portName, 9600, Parity.None, 8, StopBits.One);
                                    vfd220EType.ShowUnitAndTotalPrice("88.00", "188.00");
                                }
                            }
                        }
                    }
                }
                if (rbUsbClientShow.Checked)
                {
                    string vid = txtClientVID.Text.Trim();
                    string pid = txtClientPID.Text.Trim();
                    string endpointId = txtClientEndpoint.Text.Trim();
                    if (string.IsNullOrEmpty(vid))
                    {
                        MessageBox.Show("VID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(pid))
                    {
                        MessageBox.Show("PID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(endpointId))
                    {
                        MessageBox.Show("EndpointID不能为空！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    //显示
                    if (clientShowType.Equals("SingleLine", StringComparison.OrdinalIgnoreCase))
                    {
                        if (clientShowModel.Equals("CD7110", StringComparison.OrdinalIgnoreCase))
                        {
                            CD7110Type cd7110Type = new CD7110Type(vid, pid, endpointId);
                            cd7110Type.ShowReceipt("100.00");
                        }
                        else if (clientShowModel.Equals("Led8N", StringComparison.OrdinalIgnoreCase))
                        {
                            Led8NType led8NType = new Led8NType(vid, pid, endpointId);
                            led8NType.ShowReceipt("100.00");
                        }
                        else if (clientShowModel.Equals("Senor", StringComparison.OrdinalIgnoreCase))
                        {
                            SenorSingleType senorSingleType = new SenorSingleType(vid, pid, endpointId);
                            senorSingleType.ShowReceipt("100.00");
                        }
                    }
                    else if (clientShowType.Equals("DualLine", StringComparison.OrdinalIgnoreCase))
                    {
                        if (clientShowModel.Equals("Senor", StringComparison.OrdinalIgnoreCase))
                        {
                            SenorDualType senorDualType = new SenorDualType(vid, pid, endpointId);
                            senorDualType.ShowUnitAndTotalPrice("88.00", "188.00");
                        }
                        else if (clientShowModel.Equals("VFD220E", StringComparison.OrdinalIgnoreCase))
                        {
                            VFD220EType vfd220EType = new VFD220EType(vid, pid, endpointId);
                            vfd220EType.ShowUnitAndTotalPrice("88.00", "188.00");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("客显测试打开失败，错误信息：" + exception, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}