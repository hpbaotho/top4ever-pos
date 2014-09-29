﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Top4ever.Common;
using Top4ever.Entity;
using Top4ever.Entity.Config;
using Top4ever.Entity.Enum;
using Top4ever.Hardware;

namespace VechsoftPos
{
    public partial class FormConfig : Form
    {
        private readonly List<ListItem> _listItem = new List<ListItem>();

        public FormConfig()
        {
            InitializeComponent();
            BindClientShowInfo();
            BindPrinterInfo();
            //多语言
            InitMultiLanguageCombox();
            //营业方式
            ListItem item = new ListItem("堂食", Convert.ToString((int)ShopSaleType.DineIn));
            cmbSaleType.Items.Add(item);
            item = new ListItem("外卖", Convert.ToString((int)ShopSaleType.Takeout));
            cmbSaleType.Items.Add(item);
            item = new ListItem("堂食加外卖", Convert.ToString((int)ShopSaleType.DineInAndTakeout));
            cmbSaleType.Items.Add(item);
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {
            txtIP.Text = ConstantValuePool.BizSettingConfig.IPAddress;
            txtPort.Text = ConstantValuePool.BizSettingConfig.Port.ToString();
            txtDeviceNo.Text = ConstantValuePool.BizSettingConfig.DeviceNo;
            txtFont.Text = ConstantValuePool.BizSettingConfig.FontSize.ToString();
            cmbSaleType.SelectedIndex = GetIndexByValue(cmbSaleType, Convert.ToString((int)ConstantValuePool.BizSettingConfig.SaleType));
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
            if (ConstantValuePool.BizSettingConfig.printConfig.Enabled)
            {
                ckbPrinter.Checked = true;
                if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.DRIVER)
                {
                    rbDriverPrinter.Checked = true;
                    cmbPrinter.Text = ConstantValuePool.BizSettingConfig.printConfig.Name;
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
                }
                txtCopies.Text = ConstantValuePool.BizSettingConfig.printConfig.Copies.ToString();
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
                txtCopies.Text = ConstantValuePool.BizSettingConfig.printConfig.Copies.ToString();
                txtCopies.Enabled = false;
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
                }
                else
                {
                    rbCashDrawer.Checked = true;
                    cmbCashDrawerPort.Text = ConstantValuePool.BizSettingConfig.cashBoxConfig.Port;
                }
            }
            else
            {
                rbCashDrawer.Enabled = false;
                cmbCashDrawerPort.Enabled = false;
                rbUsbCashDrawer.Enabled = false;
                txtCashVID.Enabled = false;
                txtCashPID.Enabled = false;
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
                ckbClientShow.Checked = false;
                rbClientShow.Enabled = false;
                cmbClientShowPort.Enabled = false;
                rbUsbClientShow.Enabled = false;
                txtClientVID.Enabled = false;
                txtClientPID.Enabled = false;
                cmbClientShowModel.Enabled = false;
            }
            foreach(BizControl control in ConstantValuePool.BizSettingConfig.bizUIConfig.BizControls)
            {
                if(control.Name == "Group")
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
            alPrinters = null;
            List<string> alPorts = Printer.GetPortList();
            for (int i = 0; i < alPorts.Count; i++)
            {
                cmbPrinterPort.Items.Add(alPorts[i]);
                cmbCashDrawerPort.Items.Add(alPorts[i]);
                cmbClientShowPort.Items.Add(alPorts[i]);
            }
            alPorts = null;
        }

        private void InitMultiLanguageCombox()
        {
            switch (ConstantValuePool.BizSettingConfig.Languge1st)
            {
                case LanguageType.SIMPLIFIED:
                    cmbLanguge1st.SelectedIndexChanged -= new System.EventHandler(cmbLanguge1st_SelectedIndexChanged);
                    cmbLanguge2nd.SelectedIndexChanged -= new System.EventHandler(cmbLanguge2nd_SelectedIndexChanged);
                    _listItem.Clear();
                    _listItem.Add(new ListItem("简体", Convert.ToString((int)LanguageType.SIMPLIFIED)));
                    _listItem.Add(new ListItem("繁体", Convert.ToString((int)LanguageType.TRADITIONAL)));
                    _listItem.Add(new ListItem("英文", Convert.ToString((int)LanguageType.ENGLISH)));
                    BindFirstLanguage();
                    cmbLanguge1st.SelectedIndex = GetIndexByValue(cmbLanguge1st, Convert.ToString((int)ConstantValuePool.BizSettingConfig.Languge1st));
                    BindSecondLanguage();
                    cmbLanguge2nd.SelectedIndex = GetIndexByValue(cmbLanguge2nd, Convert.ToString((int)ConstantValuePool.BizSettingConfig.Languge2nd));
                    cmbLanguge1st.SelectedIndexChanged += new System.EventHandler(cmbLanguge1st_SelectedIndexChanged);
                    cmbLanguge2nd.SelectedIndexChanged += new System.EventHandler(cmbLanguge2nd_SelectedIndexChanged);
                    break;
                case LanguageType.TRADITIONAL:
                    cmbLanguge1st.SelectedIndexChanged -= new System.EventHandler(cmbLanguge1st_SelectedIndexChanged);
                    cmbLanguge2nd.SelectedIndexChanged -= new System.EventHandler(cmbLanguge2nd_SelectedIndexChanged);
                    _listItem.Clear();
                    _listItem.Add(new ListItem("簡體", Convert.ToString((int)LanguageType.SIMPLIFIED)));
                    _listItem.Add(new ListItem("繁體", Convert.ToString((int)LanguageType.TRADITIONAL)));
                    _listItem.Add(new ListItem("英文", Convert.ToString((int)LanguageType.ENGLISH)));
                    BindFirstLanguage();
                    cmbLanguge1st.SelectedIndex = GetIndexByValue(cmbLanguge1st, Convert.ToString((int)ConstantValuePool.BizSettingConfig.Languge1st));
                    BindSecondLanguage();
                    cmbLanguge2nd.SelectedIndex = GetIndexByValue(cmbLanguge2nd, Convert.ToString((int)ConstantValuePool.BizSettingConfig.Languge2nd));
                    cmbLanguge1st.SelectedIndexChanged += new System.EventHandler(cmbLanguge1st_SelectedIndexChanged);
                    cmbLanguge2nd.SelectedIndexChanged += new System.EventHandler(cmbLanguge2nd_SelectedIndexChanged);
                    break;
                case LanguageType.ENGLISH:
                    cmbLanguge1st.SelectedIndexChanged -= new System.EventHandler(cmbLanguge1st_SelectedIndexChanged);
                    cmbLanguge2nd.SelectedIndexChanged -= new System.EventHandler(cmbLanguge2nd_SelectedIndexChanged);
                    _listItem.Clear();
                    _listItem.Add(new ListItem("Chinese-Simplified", Convert.ToString((int)LanguageType.SIMPLIFIED)));
                    _listItem.Add(new ListItem("Chinese-Traditional", Convert.ToString((int)LanguageType.TRADITIONAL)));
                    _listItem.Add(new ListItem("English", Convert.ToString((int)LanguageType.ENGLISH)));
                    BindFirstLanguage();
                    cmbLanguge1st.SelectedIndex = GetIndexByValue(cmbLanguge1st, Convert.ToString((int)ConstantValuePool.BizSettingConfig.Languge1st));
                    BindSecondLanguage();
                    cmbLanguge2nd.SelectedIndex = GetIndexByValue(cmbLanguge2nd, Convert.ToString((int)ConstantValuePool.BizSettingConfig.Languge2nd));
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
                if (int.Parse(item.Value) != (int)ConstantValuePool.BizSettingConfig.Languge1st)
                {
                    cmbLanguge2nd.Items.Add(item);
                }
            }
        }

        private void cmbLanguge1st_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem item = cmbLanguge1st.SelectedItem as ListItem;
            ConstantValuePool.BizSettingConfig.Languge1st = (LanguageType)int.Parse(item.Value);
            BindSecondLanguage();
            cmbLanguge2nd.SelectedIndex = 0;

            InitMultiLanguageCombox();
        }

        private void cmbLanguge2nd_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem item = cmbLanguge2nd.SelectedItem as ListItem;
            ConstantValuePool.BizSettingConfig.Languge2nd = (LanguageType)int.Parse(item.Value);
        }

        private int GetIndexByValue(ComboBox cb, string value)
        {
            int index = 0, selectedIndex = -1;
            foreach (object obj in cb.Items)
            {
                if (((ListItem)obj).Value == value)
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
                RowsCount = int.Parse(txtGoodsGroupRows.Text), 
                ColumnsCount = int.Parse(txtGoodsGroupColumns.Text)
            };
            bizControls.Add(control);
            control = new BizControl
            {
                Name = "Item", 
                RowsCount = int.Parse(txtGoodsRows.Text), 
                ColumnsCount = int.Parse(txtGoodsColumns.Text)
            };
            bizControls.Add(control);
            control = new BizControl
            {
                Name = "Payoff", 
                RowsCount = int.Parse(txtPayoffWayRows.Text), 
                ColumnsCount = int.Parse(txtPayoffWayColumns.Text)
            };
            bizControls.Add(control);
            //打印
            PrintConfig printConfig = new PrintConfig {Enabled = ckbPrinter.Checked};
            if (rbDriverPrinter.Checked)
            {
                printConfig.PrinterPort = PortType.DRIVER;
                printConfig.Name = cmbPrinter.Text;
            }
            if (rbPrinterPort.Checked)
            {
                printConfig.PrinterPort = PortType.COM;
                printConfig.Name = cmbPrinterPort.Text;
            }
            if (rbNetPrinter.Checked)
            {
                printConfig.PrinterPort = PortType.ETHERNET;
                printConfig.Name = txtIPAddress.Text;
            }
            if (rbUsbPort.Checked)
            {
                printConfig.PrinterPort = PortType.USB;
                printConfig.VID = txtPrinterVID.Text;
                printConfig.PID = txtPrinterPID.Text;
            }
            printConfig.Copies = int.Parse(txtCopies.Text);
            printConfig.PaperWidth = cmbPaperWidth.Text;
            //钱箱
            CashBoxConfig cashboxConfig = new CashBoxConfig();
            cashboxConfig.Enabled = ckbCashDrawer.Checked;
            if (rbCashDrawer.Checked)
            {
                cashboxConfig.IsUsbPort = false;
                cashboxConfig.Port = cmbCashDrawerPort.Text;
            }
            if (rbUsbCashDrawer.Checked)
            {
                cashboxConfig.IsUsbPort = true;
                cashboxConfig.VID = txtCashVID.Text;
                cashboxConfig.PID = txtCashPID.Text;
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
            if (rbClientShow.Checked)
            {
                clientShowConfig.IsUsbPort = false;
                clientShowConfig.Port = cmbClientShowPort.Text;
            }
            if (rbUsbClientShow.Checked)
            {
                clientShowConfig.IsUsbPort = true;
                clientShowConfig.VID = txtClientVID.Text;
                clientShowConfig.PID = txtClientPID.Text;
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
            //组装
            AppSettingConfig appConfig = new AppSettingConfig();
            appConfig.IPAddress = txtIP.Text;
            appConfig.Port = int.Parse(txtPort.Text);
            appConfig.DeviceNo = txtDeviceNo.Text;
            //多语言
            ListItem item1 = cmbLanguge1st.SelectedItem as ListItem;
            ListItem item2 = cmbLanguge2nd.SelectedItem as ListItem;
            appConfig.Languge1st = (LanguageType)int.Parse(item1.Value);
            appConfig.Languge2nd = (LanguageType)int.Parse(item2.Value);
            appConfig.FontSize = float.Parse(txtFont.Text);
            ListItem itemType = cmbSaleType.SelectedItem as ListItem;
            appConfig.SaleType = (ShopSaleType)int.Parse(itemType.Value);
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
            appConfig.bizUIConfig = new BizUIConfig { BizControls = bizControls };
            appConfig.printConfig = printConfig;
            appConfig.cashBoxConfig = cashboxConfig;
            appConfig.telCallConfig = telCallConfig;
            appConfig.clientShowConfig = clientShowConfig;
            //存储在静态对象中
            ConstantValuePool.BizSettingConfig = appConfig;
            //输出到文件
            XmlUtil.Serialize<AppSettingConfig>(appConfig, "Config/AppSetting.config");
            MessageBox.Show("保存成功！");
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
                txtCopies.Enabled = true;
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
                txtCopies.Enabled = false;
                cmbPaperWidth.Enabled = false;
            }
        }

        private void ckbCashDrawer_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbCashDrawer.Checked)
            {
                rbCashDrawer.Enabled = true;
                cmbCashDrawerPort.Enabled = true;
                rbUsbCashDrawer.Enabled = true;
                txtCashVID.Enabled = true;
                txtCashPID.Enabled = true;
            }
            else
            {
                rbCashDrawer.Enabled = false;
                cmbCashDrawerPort.Enabled = false;
                rbUsbCashDrawer.Enabled = false;
                txtCashVID.Enabled = false;
                txtCashPID.Enabled = false;
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
                rbClientShow.Enabled = true;
                cmbClientShowPort.Enabled = true;
                rbUsbClientShow.Enabled = true;
                txtClientVID.Enabled = true;
                txtClientPID.Enabled = true;
                cmbClientShowModel.Enabled = true;
            }
            else
            {
                rbClientShow.Enabled = false;
                cmbClientShowPort.Enabled = false;
                rbUsbClientShow.Enabled = false;
                txtClientVID.Enabled = false;
                txtClientPID.Enabled = false;
                cmbClientShowModel.Enabled = false;
            }
        }

        private void btnLoadLogin_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog1 = new OpenFileDialog();
            fileDialog1.InitialDirectory = Application.StartupPath;//初始目录
            fileDialog1.Filter = "picture（*.jpg;*.bmp;*.gif）|*.jpg;*.bmp;*.gif|All files (*.*)|*.*";//文件的类型
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
            fileDialog1.InitialDirectory = Application.StartupPath;//初始目录
            fileDialog1.Filter = "picture（*.jpg;*.bmp;*.gif）|*.jpg;*.bmp;*.gif|All files (*.*)|*.*";//文件的类型
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
            fileDialog1.InitialDirectory = strPath;//初始目录
            fileDialog1.Filter = "picture（*.jpg;*.bmp;*.gif）|*.jpg;*.bmp;*.gif|All files (*.*)|*.*";//文件的类型
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
    }
}
