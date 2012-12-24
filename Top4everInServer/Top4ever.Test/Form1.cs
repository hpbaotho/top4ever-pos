using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Top4ever.Domain;
using Top4ever.Domain.OrderRelated;
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
            Employee employee = null;
            int result = EmployeeService.GetInstance().GetEmployee(this.textBox1.Text, this.textBox2.Text, out employee);
            if (result == 1)
            { 
                
            }
            //Order order = OrderService.GetInstance().GetOrder("502");
            //SysBasicData sysBasicData = SysBasicDataService.GetInstance().GetSysBasicData();
        }
    }
}
