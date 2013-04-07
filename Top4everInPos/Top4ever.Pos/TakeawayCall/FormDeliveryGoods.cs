using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

using Top4ever.ClientService;
using Top4ever.CustomControl;
using Top4ever.Domain.Accounts;
using Top4ever.Domain.Customers;
using Top4ever.Domain.Transfer;
using Top4ever.Entity;
using Top4ever.Entity.Enum;
using Top4ever.Pos.Feature;
using Top4ever.Print;
using Top4ever.Print.Entity;

namespace Top4ever.Pos.TakeawayCall
{
    public partial class FormDeliveryGoods : Form
    {
        private bool m_HandwritingEnabled = false;
        private SalesOrder _salesOrder = null;
        private PrintData _printData = null;
        private string _telephone;
        private string _customerName;
        private string _address;
        private bool _hasDeliveried = false;
        public bool HasDeliveried
        {
            get { return _hasDeliveried; }
        }

        public FormDeliveryGoods(SalesOrder salesOrder, PrintData printData, string telephone, string customerName, string address)
        {
            _salesOrder = salesOrder;
            _printData = printData;
            _telephone = telephone;
            _customerName = customerName;
            _address = address;
            InitializeComponent();
        }

        private void FormDeliveryGoods_Load(object sender, EventArgs e)
        {
            ShowHandwriting();
            if (_salesOrder != null)
            {
                this.txtTranSeq.Text = _salesOrder.order.TranSequence.ToString();
                this.txtTotalAmount.Text = (_salesOrder.order.ActualSellPrice + _salesOrder.order.ServiceFee).ToString("f2");
                this.txtTelephone.Text = _telephone;
                this.txtName.Text = _customerName;
                this.txtAddress.Text = _address;
            }
        }

        private void btnHandwriting_Click(object sender, EventArgs e)
        {
            m_HandwritingEnabled = !m_HandwritingEnabled;
            ShowHandwriting();
        }

        private void ShowHandwriting()
        {
            if (m_HandwritingEnabled)
            {
                this.Height = 578;
                this.handwritingPad1.Visible = true;
                this.btnHandwriting.Text = "手写输入∧";
            }
            else
            {
                this.Height = 350;
                this.handwritingPad1.Visible = false;
                this.btnHandwriting.Text = "手写输入∨";
            }
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            string inputString = this.txtRemark.Text;
            if (!string.IsNullOrEmpty(inputString))
            {
                this.txtRemark.Select();
                SendKeys.Send("{BACKSPACE}");
            }
        }

        private void txtEmployeeNo_MouseDown(object sender, MouseEventArgs e)
        {
            FormNumericKeypad keyForm = new FormNumericKeypad(false);
            keyForm.DisplayText = "请输入用户名";
            keyForm.ShowDialog();
            if (!string.IsNullOrEmpty(keyForm.KeypadValue))
            {
                string employeeNo = keyForm.KeypadValue;
                this.txtEmployeeNo.Text = employeeNo;
                Employee employee = null;
                EmployeeService employeeService = new EmployeeService();
                int result = employeeService.GetEmployeeByNo(employeeNo, ref employee);
                if (result == 1)
                {
                    this.txtEmployeeNo.Text = employee.EmployeeNo;
                    this.txtEmployeeName.Text = employee.Name;
                }
                else if (result == 2)
                {
                    MessageBox.Show("用户名不存在，请检查输入是否正确！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("获取客户信息失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            CustomerOrder customerOrder = new CustomerOrder();
            customerOrder.OrderID = _salesOrder.order.OrderID;
            customerOrder.Remark = this.txtRemark.Text.Trim();
            customerOrder.DeliveryEmployeeNo = this.txtEmployeeNo.Text.Trim();
            CustomersService customerService = new CustomersService();
            if (customerService.UpdateTakeoutOrderStatus(customerOrder))
            {
                //打印
                _printData.CustomerPhone = this.txtTelephone.Text.Trim();
                _printData.CustomerName = this.txtName.Text.Trim();
                _printData.DeliveryAddress = this.txtAddress.Text.Trim();
                _printData.Remark = this.txtRemark.Text.Trim();
                _printData.DeliveryEmployeeName = this.txtEmployeeName.Text.Trim();
                
                if (ConstantValuePool.BizSettingConfig.printConfig.Enabled)
                {
                    int copies = ConstantValuePool.BizSettingConfig.printConfig.Copies;
                    string paperWidth = ConstantValuePool.BizSettingConfig.printConfig.PaperWidth;
                    string configPath = @"PrintConfig\InstructionPrintOrderSetting.config";
                    string layoutPath = @"PrintConfig\DeliveryOrder.ini";
                    if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.DRIVER)
                    {
                        configPath = @"PrintConfig\PrintOrderSetting.config";
                        string printerName = ConstantValuePool.BizSettingConfig.printConfig.Name;
                        DriverOrderPrint printer = new DriverOrderPrint(printerName, paperWidth, "SpecimenLabel");
                        for (int i = 0; i < copies; i++)
                        {
                            printer.DoPrint(_printData, layoutPath, configPath);
                        }
                    }
                    if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.COM)
                    {
                        string port = ConstantValuePool.BizSettingConfig.printConfig.Name;
                        if (port.Length > 3)
                        {
                            if (port.Substring(0, 3).ToUpper() == "COM")
                            {
                                string portName = port.Substring(0, 4).ToUpper();
                                InstructionOrderPrint printer = new InstructionOrderPrint(portName, 9600, Parity.None, 8, StopBits.One, paperWidth);
                                for (int i = 0; i < copies; i++)
                                {
                                    printer.DoPrint(_printData, layoutPath, configPath);
                                }
                            }
                        }
                    }
                    if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.ETHERNET)
                    {
                        string IPAddress = ConstantValuePool.BizSettingConfig.printConfig.Name;
                        InstructionOrderPrint printer = new InstructionOrderPrint(IPAddress, 9100, paperWidth);
                        for (int i = 0; i < copies; i++)
                        {
                            printer.DoPrint(_printData, layoutPath, configPath);
                        }
                    }
                    if (ConstantValuePool.BizSettingConfig.printConfig.PrinterPort == PortType.USB)
                    {
                        string VID = ConstantValuePool.BizSettingConfig.printConfig.VID;
                        string PID = ConstantValuePool.BizSettingConfig.printConfig.PID;
                        InstructionOrderPrint printer = new InstructionOrderPrint(VID, PID, paperWidth);
                        for (int i = 0; i < copies; i++)
                        {
                            printer.DoPrint(_printData, layoutPath, configPath);
                        }
                    }
                }

                _hasDeliveried = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("出货失败，请重新操作！", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void handwritingPad1_UserHandWriting(object sender, InkWritingEventArgs e)
        {
            this.txtRemark.Text += e.InkPadValue;
            this.txtRemark.TabIndex = 0;
            this.txtRemark.Focus();
        }
    }
}
