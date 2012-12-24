using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Top4ever.CustomControl
{
    public partial class WaitingBox : Form
    {
        public delegate void TaskHandler();
        public TaskHandler Task;
        
        private string message = string.Empty;
        public string Message
        {
            get { return message; }
            private set { message = value; }
        }

        private bool m_GradientEnable;
        public bool GradientEnable
        {
            set { m_GradientEnable = value; }
        }

        private const int m_EffectCount = 10;
        private const int m_EffectTime = 500;
        private Timer m_GradientTimer;
        private int m_TimeSpan;
        private int m_MaxWaitTime;
        private int m_WaitTime;
        private bool m_IsShown = true;
        private IAsyncResult m_AsyncResult;

        public WaitingBox()
        {
            int maxWaitTime = 600 * 1000;
            string waitMessage = "正在处理数据，请稍后...";
            bool cancelEnable = true;
            bool timerVisable = true;
            Initialize(waitMessage, maxWaitTime, cancelEnable, timerVisable);
        }

        public WaitingBox(string waitMessage)
        {
            int maxWaitTime = 600 * 1000;
            bool cancelEnable = true;
            bool timerVisable = true;
            Initialize(waitMessage, maxWaitTime, cancelEnable, timerVisable);
        }

        public WaitingBox(bool cancelEnable, bool timerVisable)
        {
            int maxWaitTime = 600 * 1000;
            string waitMessage = "正在处理数据，请稍后...";
            Initialize(waitMessage, maxWaitTime, cancelEnable, timerVisable);
        }

        public WaitingBox(string waitMessage, int maxWaitTime, bool cancelEnable, bool timerVisable)
        {
            Initialize(waitMessage, maxWaitTime, cancelEnable, timerVisable);
        }

        private void Initialize(string waitMessage, int maxWaitTime, bool cancelEnable, bool timerVisable)
        {
            InitializeComponent();
            //initialize form
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            Color[] c = GetRandColor();
            this.panel1.BackColor = c[0];
            this.BackColor = c[1];
            this.labMessage.Text = waitMessage;
            m_GradientTimer = new Timer();
            m_GradientTimer.Interval = m_EffectTime / m_EffectCount;
            m_GradientTimer.Tick += gradientTimer_Tick;
            this.Opacity = 0;
            m_GradientEnable = true;
            m_TimeSpan = 500;
            m_MaxWaitTime = maxWaitTime;
            this.pictureBoxCancel.Visible = cancelEnable;
            this.labTimer.Visible = timerVisable;
            this.timer1.Interval = m_TimeSpan;
            this.timer1.Start();
        }

        private Color[] GetRandColor()
        {
            int rMax = 248;
            int rMin = 204;
            int gMax = 250;
            int gMin = 215;
            int bMax = 250;
            int bMin = 240;
            Random r = new Random(DateTime.Now.Millisecond);
            int r1 = r.Next(rMin, rMax);
            int r2 = r1 + 5;
            int g1 = r.Next(gMin, gMax);
            int g2 = g1 + 5;
            int b1 = r.Next(bMin, bMax);
            int b2 = b1 + 5;
            Color c1 = Color.FromArgb(r1, g1, b1);
            Color c2 = Color.FromArgb(r2, g2, b2);
            Color[] c = { c1, c2 };
            return c;
        }

        private void gradientTimer_Tick(object sender, EventArgs e)
        {
            if (m_IsShown)
            {
                if (this.Opacity >= 1)
                {
                    m_GradientTimer.Stop();
                    m_IsShown = false;
                }
                this.Opacity += 1.00 / m_EffectCount;
            }
            else
            {
                if (this.Opacity <= 0)
                {
                    m_GradientTimer.Stop();
                    m_IsShown = true;
                    this.Close();
                }
                this.Opacity -= 1.00 / m_EffectCount;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_WaitTime += m_TimeSpan;
            this.labTimer.Text = string.Format("{0}秒", m_WaitTime / 1000);
            if (!m_AsyncResult.IsCompleted)
            {
                if (m_WaitTime > m_MaxWaitTime)
                {
                    message = string.Format("处理数据超时{0}秒，结束当前操作！", m_MaxWaitTime / 1000);
                    this.Close();
                }
            }
            else
            {
                message = string.Empty;
                this.Close();
            }
        }

        private void WaitingBox_Shown(object sender, EventArgs e)
        {
            m_AsyncResult = Task.BeginInvoke(null, null);
            if (m_GradientEnable)
            {
                m_GradientTimer.Start();
            }
            else
            {
                this.Opacity = 1;
            }
        }

        private void WaitingBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_GradientEnable)
            {
                if (this.Opacity >= 1)
                    e.Cancel = true;
                m_GradientTimer.Start();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            message = "您结束了当前操作！";
            this.Close();
        }
    }
}
