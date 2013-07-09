using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;

namespace Top4ever.Print
{
    public class DriverSinglePrint
    {
        private PrintDocument pDoc;
        private Graphics m_Graphics;
        private IList<String> printData;
        private float PX = 0;
        private float PY = 0;
        private Font font = new Font("simsun", 8.25F);

        public DriverSinglePrint(string printerName, string paperName)
        {
            pDoc = new PrintDocument();
            pDoc.PrinterSettings.PrinterName = printerName;
            PaperSize paperSize = new PaperSize(paperName, 300, 2280);//页面大小;
            foreach (PaperSize ps in pDoc.PrinterSettings.PaperSizes)
            {
                if (ps.PaperName.Equals(paperName))
                {
                    paperSize = ps;
                }
            }
            pDoc.DefaultPageSettings.PaperSize = paperSize;
            pDoc.DefaultPageSettings.Landscape = false;//横向打印
            pDoc.PrintPage += new PrintPageEventHandler(pDoc_PrintPage);
        }

        public bool DoPrint(IList<String> printData)
        {
            if (printData != null && printData.Count > 0)
            {
                this.printData = printData;
                bool printResult = false;
                try
                {
                    pDoc.Print();
                    printResult = true;
                }
                catch (Exception ex)
                {
                    //log ex;
                    printResult = false;
                }
                return printResult;
            }
            else
            {
                return false;
            }
        }

        public bool DoPrint(IList<String> printData, Font font)
        {
            if (printData != null && printData.Count > 0)
            {
                this.printData = printData;
                this.font = font;
                bool printResult = false;
                try
                {
                    pDoc.Print();
                    printResult = true;
                }
                catch (Exception ex)
                {
                    //log ex;
                    printResult = false;
                }
                return printResult;
            }
            else
            {
                return false;
            }
        }

        private void pDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            m_Graphics = e.Graphics;
            PX = e.MarginBounds.Left *0.01f * 15.4f;
            PY = e.MarginBounds.Top * 0.01f * 15.4f;
            DrawPaper();
        }

        private void DrawPaper()
        {
            foreach (String text in printData)
            {
                Brush brush = new SolidBrush(Color.Black);
                SizeF size = m_Graphics.MeasureString(text, font);
                m_Graphics.DrawString(text, font, brush, PX, PY);
                PY += size.Height;
            }
        }
    }
}
