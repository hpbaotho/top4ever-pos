using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Ink;

namespace Top4ever.CustomControl
{
    public partial class HandwritingPad : UserControl
    {
        [Category("Mouse"), Description("Return value of Ink hand writing")]
        public event InkWritingDelegate UserHandWriting;

        private InkCollector ic;
        private RecognizerContext rct;

        public HandwritingPad()
        {
            InitializeComponent();
        }

        private void HandwritingPad_Load(object sender, EventArgs e)
        {
            ic = new InkCollector(PicInkPad.Handle);
            ic.Enabled = true;
            Recognizers recos = new Recognizers();
            Recognizer chineseReco = recos.GetDefaultRecognizer();
            rct = chineseReco.CreateRecognizerContext();
            rct.Strokes = ic.Ink.Strokes;
        }

        private void btnInkColor_Click(object sender, EventArgs e)
        {
            colorDialog1.FullOpen = true;
            colorDialog1.ShowDialog();
            ic.DefaultDrawingAttributes.Color = colorDialog1.Color;
        }

        private void btnRecognition_Click(object sender, EventArgs e)
        {
            string inkPadValue = ic.Ink.Strokes.ToString();
            if (!ic.CollectingInk)
            {
                Strokes strokesToDelete = ic.Ink.Strokes;
                rct.StopBackgroundRecognition();
                ic.Ink.DeleteStrokes(strokesToDelete);
                rct.Strokes = ic.Ink.Strokes;
                ic.Ink.DeleteStrokes();//清除手写区域笔画;
                PicInkPad.Refresh();
            }
            InkWritingEventArgs ev = new InkWritingEventArgs(inkPadValue);
            if (UserHandWriting != null)
            {
                UserHandWriting(this, ev);
            }
        }
    }

    public delegate void InkWritingDelegate(object sender, InkWritingEventArgs e);

    public class InkWritingEventArgs : EventArgs
    {
        private readonly string pvtInkPadValue;

        public InkWritingEventArgs(string inkPadValue)
        {
            this.pvtInkPadValue = inkPadValue;
        }

        public string InkPadValue
        {
            get
            {
                return pvtInkPadValue;
            }
        }
    }
}
