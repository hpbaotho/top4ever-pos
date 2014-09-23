using System;
using System.Windows.Forms;
using Top4ever.Domain.Accounts;
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
            decimal price = OrderDetailsService.GetInstance().GetLastCustomPrice(new Guid("b48b0413-c908-4202-8472-874fd18fa85c"));

            Employee employee = null;
            int result = EmployeeService.GetInstance().GetEmployee(this.textBox1.Text, this.textBox2.Text, out employee);
            if (result == 1)
            { 
                
            }
        }
    }
}
