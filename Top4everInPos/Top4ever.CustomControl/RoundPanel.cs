using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Top4ever.CustomControl
{
    public sealed class RoundPanel : Panel
    {
        // 圆角弧度
        private int _radius;

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
        }

        public RoundPanel()
        {
            InitializeComponent();
            this.Padding = new Padding(0, 0, 0, 0);
            this.Margin = new Padding(0, 0, 0, 0);
        }

        public RoundPanel(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// 圆角弧度
        /// </summary>
        [Browsable(true)]
        [Description("圆角弧度(0为不要圆角)")]
        public int MatrixRound
        {
            get
            {
                return _radius;
            }
            set
            {
                _radius = value < 0 ? 0 : value;
                base.Refresh();
            }
        }

        /// <summary>
        /// 圆角代码
        /// </summary>
        public void Round()
        {
            GraphicsPath oPath = new GraphicsPath();
            const int x = 0;
            const int y = 0;
            int thisWidth = this.Width;
            int thisHeight = this.Height;
            int angle = _radius;
            if (angle > 0)
            {
                oPath.AddArc(x, y, angle, angle, 180, 90);                                 // 左上角
                oPath.AddArc(thisWidth - angle, y, angle, angle, 270, 90);                 // 右上角
                oPath.AddArc(thisWidth - angle, thisHeight - angle, angle, angle, 0, 90);  // 右下角
                oPath.AddArc(x, thisHeight - angle, angle, angle, 90, 90);                 // 左下角
                oPath.CloseAllFigures();
                Region = new System.Drawing.Region(oPath);
            }
            else
            {
                oPath.AddLine(x + angle, y, thisWidth - angle, y);                         // 顶端
                oPath.AddLine(thisWidth, y + angle, thisWidth, thisHeight - angle);        // 右边
                oPath.AddLine(thisWidth - angle, thisHeight, x + angle, thisHeight);       // 底边
                oPath.AddLine(x, y + angle, x, thisHeight - angle);                        // 左边
                oPath.CloseAllFigures();
                Region = new System.Drawing.Region(oPath);
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            //圆角
            Round();
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            base.Refresh();
        }
    }
}
