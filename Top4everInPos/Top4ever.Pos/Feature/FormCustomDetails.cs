using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Top4ever.Pos.Feature
{
    public partial class FormCustomDetails : Form
    {
        private string _goodsName;
        private string _tasteName;

        public string CustomTasteName
        {
            get { return _tasteName; }
        }

        public FormCustomDetails(string goodsName)
        {
            InitializeComponent();
            _goodsName = goodsName;
        }

        private void FormCustomDetails_Load(object sender, EventArgs e)
        {
            txtGoodsName.Text = _goodsName.Replace("-", "");
        }

        private void txtCustomTaste_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                if (!string.IsNullOrEmpty(txtCustomTaste.Text))
                {
                    string detailsPrefix = string.Empty;
                    if (_goodsName.IndexOf('-') >= 0)
                    {
                        int index = _goodsName.LastIndexOf('-');
                        detailsPrefix = _goodsName.Substring(0, index + 1);
                        detailsPrefix += "--";
                    }
                    else
                    {
                        detailsPrefix = "--";
                    }
                    _tasteName = detailsPrefix + txtCustomTaste.Text.Trim();
                    this.Close();
                }
            }
        }
    }
}
