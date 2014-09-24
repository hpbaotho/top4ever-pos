using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Top4ever.ClientService;
using Top4ever.Domain.Transfer;
using Top4ever.Domain.Customers;

namespace Top4ever.Pos.Feature
{
    public partial class FormChartReport : Form
    {
        public FormChartReport()
        {
            InitializeComponent();
            BuildChartPie();
            BuildChartColumn();
        }

        private void BuildChartPie()
        {
            ChartArea chartArea1 = new ChartArea();
            chartArea1.Area3DStyle.IsClustered = true;
            chartArea1.Area3DStyle.IsRightAngleAxes = false;
            chartArea1.Area3DStyle.Perspective = 10;
            chartArea1.Area3DStyle.PointGapDepth = 0;
            chartArea1.Area3DStyle.Rotation = 0;
            chartArea1.Area3DStyle.WallWidth = 0;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BackSecondaryColor = System.Drawing.Color.Transparent;
            chartArea1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.BorderWidth = 0;
            chartArea1.Name = "Default";
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.chartPie.ChartAreas.Add(chartArea1);

            Legend legend1 = new Legend();
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.Docking = Docking.Bottom;
            legend1.Enabled = true;
            legend1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            legend1.IsTextAutoFit = true;
            legend1.Name = "Default";
            this.chartPie.Legends.Add(legend1);

            Title title1 = new Title();
            title1.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold);
            title1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            title1.Name = "Title";
            title1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            title1.ShadowOffset = 3;
            title1.Text = "销量前十的产品统计饼图";
            this.chartPie.Titles.Add(title1);

            Series series1 = new Series();
            series1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            series1.ChartArea = "Default";
            series1["PieLabelStyle"] = "Outside";
            series1["PieDrawingStyle"] = "Concave";
            series1.ChartType = SeriesChartType.Pie;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(65)))), ((int)(((byte)(140)))), ((int)(((byte)(240)))));
            series1.Legend = "Default";
            series1.Name = "Default";
            this.chartPie.Series.Add(series1);
        }

        private void BuildChartColumn()
        {
            ChartArea chartArea1 = new ChartArea();
            chartArea1.Area3DStyle.Inclination = 15;
            chartArea1.Area3DStyle.IsClustered = true;
            chartArea1.Area3DStyle.IsRightAngleAxes = false;
            chartArea1.Area3DStyle.Perspective = 10;
            chartArea1.Area3DStyle.Rotation = 10;
            chartArea1.Area3DStyle.WallWidth = 0;
            chartArea1.AxisX.LabelAutoFitMaxFontSize = 8;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.LabelStyle.Format = "yyyy-MM-dd";
            chartArea1.AxisX.LabelStyle.IsEndLabelVisible = false;
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.LabelAutoFitMaxFontSize = 8;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY.LabelStyle.Format = "C0";
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.BackColor = System.Drawing.Color.OldLace;
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.Name = "Default";
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.chartColumn.ChartAreas.Add(chartArea1);

            Title title1 = new Title();
            title1.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold);
            title1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            title1.Name = "Title";
            title1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            title1.ShadowOffset = 3;
            title1.Text = "销售金额的统计条形图";
            this.chartColumn.Titles.Add(title1);

            Series series1 = new Series();
            series1.ChartType = SeriesChartType.Column; //图标集类型
            //series1.Color = Color.Red; //线条颜色
            series1.BorderWidth = 2; //线条宽度
            series1.ShadowOffset = 1; //阴影宽度
            series1.IsValueShownAsLabel = true;
            series1.MarkerStyle = MarkerStyle.Diamond;
            series1.MarkerSize = 8;
            series1.XValueType = ChartValueType.DateTime;
            series1.ChartArea = "Default";
            series1.Name = "Default";
            this.chartColumn.Series.Add(series1);
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            if (this.dateTimePicker4.Value > this.dateTimePicker3.Value)
            {
                string beginDate = this.dateTimePicker3.Value.ToString("yyyy-MM-dd");
                string endDate = this.dateTimePicker4.Value.ToString("yyyy-MM-dd");

                IList<TopSellGoods> topSellGoodsList = OrderDetailsService.GetInstance().GetTopSellGoodsByTime(beginDate, endDate);
                if (topSellGoodsList != null && topSellGoodsList.Count > 0)
                {
                    BindChartPieData(topSellGoodsList);
                }
            }
        }

        private void BindChartPieData(IList<TopSellGoods> topSellGoodsList)
        {
            // Populate series data
            string[] xValues = topSellGoodsList.Select(g => g.GoodsName).ToArray();
            int[] yValues = topSellGoodsList.Select(g => g.Times).ToArray();
            Series series1 = chartPie.Series["Default"];
            series1.Points.Clear();
            series1.Points.DataBindXY(xValues, yValues);
            // Explode selected country
            foreach (DataPoint point in series1.Points)
            {
                point["Exploded"] = "false";
                if (point.AxisLabel == topSellGoodsList[0].GoodsName)
                {
                    point["Exploded"] = "true";
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.dateTimePicker2.Value > this.dateTimePicker1.Value)
            {
                string beginDate = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string endDate = this.dateTimePicker2.Value.ToString("yyyy-MM-dd");

                IList<DailyStatementInDay> dailyStatementList = DailyBalanceService.GetInstance().GetDailyStatementInDays(beginDate, endDate);
                if (dailyStatementList != null && dailyStatementList.Count > 0)
                {
                    BindChartColumnData(dailyStatementList);
                }
            }
        }

        private void BindChartColumnData(IList<DailyStatementInDay> dailyStatementList)
        {
            // Populate series data
            Series series1 = chartColumn.Series["Default"];
            series1.Points.Clear();
            foreach (DailyStatementInDay item in dailyStatementList)
            {
                series1.Points.AddXY(item.BelongToDate, item.ActualTotalIncome);
            }
        }
    }
}
