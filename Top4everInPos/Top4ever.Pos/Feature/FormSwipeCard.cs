using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VechsoftPos.Feature
{
    public partial class FormSwipeCard : Form
    {
        private string _cardNumber = string.Empty;
        public String CardNumber
        {
            get { return _cardNumber; }
        }

        public FormSwipeCard()
        {
            InitializeComponent();
        }

        private void FrmSwipeCard_Load(object sender, EventArgs e)
        {
            this.Text = "Ë¢¿¨µÇÂ¼";
            this.label1.Text = "ÇëË¢¿¨µÇÂ¼ÏµÍ³";
        }

        private void txtSwipeCard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                _cardNumber = this.txtSwipeCard.Text;
                this.Close();
            }
        }
    }
}