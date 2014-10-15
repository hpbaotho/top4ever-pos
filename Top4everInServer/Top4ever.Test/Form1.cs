using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Top4ever.Domain.Accounts;
using Top4ever.Domain.Transfer;
using Top4ever.Service;

namespace Top4ever.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReminderOrder reminderOrder = new ReminderOrder
            {
                OrderID = new Guid("15629db5-f2bc-4d01-9142-064886f4fdad"),
                OrderDetailsIDList = new List<Guid>
                {
                    new Guid("C5D49437-D8D5-44D0-B4DC-21D7ECB2D813"),
                    new Guid("8191C8C9-10C2-43E7-98B1-CE13343DFFB0")
                },
                EmployeeNo = "999999",
                ReasonName = "加紧"
            };
            bool success = ReminderOrderService.GetInstance().CreateReminderOrder(reminderOrder);

            var customers = CustomersService.GetInstance().GetAllCustomerInfo();
            var basicData = SysBasicDataService.GetInstance().GetSysBasicData();

            Employee employee = null;
            int result = EmployeeService.GetInstance().GetEmployee(this.textBox1.Text, this.textBox2.Text, out employee);
            if (result == 1)
            { 
                
            }
        }
    }
}
