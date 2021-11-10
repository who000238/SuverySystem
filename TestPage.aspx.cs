using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace SuverySystem
{
    public partial class TestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            #region RBList
            //RadioButtonList radioButtonList = new RadioButtonList();
            //radioButtonList.ID = "gender";
            //ListItem item1 = new ListItem();
            //item1.Value = "BOY";
            //item1.Text = "BOY";
            ////item1.Attributes.Add("GroupName", "gender");
            //radioButtonList.Items.Add(item1);
            //ListItem item2 = new ListItem();
            //item2.Value = "GIRL";
            //item2.Text = "GIRL";
            ////item2.Attributes.Add("GroupName", "gender");
            //radioButtonList.Items.Add(item2);
            #endregion
            #region CBList
            CheckBoxList checkBoxList = new CheckBoxList();
            checkBoxList.ID = "Food";

            #endregion

            RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator();
            requiredFieldValidator.ErrorMessage = "check";
            requiredFieldValidator.ControlToValidate = "gender";

            //this.div.Controls.Add(radioButtonList);
            this.div.Controls.Add(requiredFieldValidator);
            #region 圓餅圖
            //string[] X = new string[] { "1", "3", "5" };
            //int[] Y = new int[] { 1, 2, 3 };
            //Chart chart = new Chart();
            //chart.Titles.Add("Pie Chart");
            //Series series = new Series();
            //series.Name = "S1";
            //series.ChartType = SeriesChartType.Pie;
            //series.Points.DataBindXY(X, Y);
            //chart.Series.Add(series);
            //this.div.Controls.Add(chart);

            //string[] xValues = { "0-20", "20-40", "40-60", "60-80", "80-100" };
            //int[] yValues = { 5, 18, 45, 17, 2 };

            ////ChartAreas,Series,Legends 基本設定-------------------------------------------------

            //Chart Chart1 = new Chart();
            //Chart1.ChartAreas.Add("ChartArea1"); //圖表區域集合
            //Chart1.Legends.Add("Legends1"); //圖例集合說明
            //Chart1.Series.Add("Series1"); //數據序列集合



            ////設定 Chart-------------------------------------------------------------------------

            //Chart1.Width = 770;
            //Chart1.Height = 400;
            //Title title = new Title();
            //title.Text = "圓餅圖";
            //title.Alignment = ContentAlignment.MiddleCenter;
            //title.Font = new System.Drawing.Font("Trebuchet MS", 14F, FontStyle.Bold);
            //Chart1.Titles.Add(title);



            ////設定 ChartArea1--------------------------------------------------------------------

            //Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //Chart1.ChartAreas[0].AxisX.Interval = 1;



            ////設定 Legends-------------------------------------------------------------------------

            ////Chart1.Legends["Legends1"].DockedToChartArea = "ChartArea1"; //顯示在圖表內

            ////Chart1.Legends["Legends1"].Docking = Docking.Bottom; //自訂顯示位置

            ////背景色

            //Chart1.Legends["Legends1"].BackColor = Color.FromArgb(235, 235, 235);
            ////斜線背景

            //Chart1.Legends["Legends1"].BackHatchStyle = ChartHatchStyle.DarkDownwardDiagonal;
            //Chart1.Legends["Legends1"].BorderWidth = 1;
            //Chart1.Legends["Legends1"].BorderColor = Color.FromArgb(200, 200, 200);



            ////設定 Series1-----------------------------------------------------------------------

            //Chart1.Series["Series1"].ChartType = SeriesChartType.Pie;

            ////Chart1.Series["Series1"].ChartType = SeriesChartType.Doughnut;

            //Chart1.Series["Series1"].Points.DataBindXY(xValues, yValues);
            //Chart1.Series["Series1"].LegendText = "#VALX: [ #PERCENT{P1} ]"; //X軸 + 百分比
            //Chart1.Series["Series1"].Label = "#VALX\n#PERCENT{P1}"; //X軸 + 百分比

            ////Chart1.Series["Series1"].LabelForeColor = Color.FromArgb(0, 90, 255); //字體顏色

            ////字體設定

            //Chart1.Series["Series1"].Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold);
            //Chart1.Series["Series1"].Points.FindMaxByValue().LabelForeColor = Color.Red;

            ////Chart1.Series["Series1"].Points.FindMaxByValue().Color = Color.Red;

            ////Chart1.Series["Series1"].Points.FindMaxByValue()["Exploded"] = "true";

            //Chart1.Series["Series1"].BorderColor = Color.FromArgb(255, 101, 101, 101);



            ////Chart1.Series["Series1"]["DoughnutRadius"] = "80"; // ChartType為Doughnut時，Set Doughnut hole size

            ////Chart1.Series["Series1"]["PieLabelStyle"] = "Inside"; //數值顯示在圓餅內

            //Chart1.Series["Series1"]["PieLabelStyle"] = "Outside"; //數值顯示在圓餅外

            ////Chart1.Series["Series1"]["PieLabelStyle"] = "Disabled"; //不顯示數值

            ////設定圓餅效果，除 Default 外其他效果3D不適用

            //Chart1.Series["Series1"]["PieDrawingStyle"] = "Default";
            ////Chart1.Series["Series1"]["PieDrawingStyle"] = "SoftEdge";

            ////Chart1.Series["Series1"]["PieDrawingStyle"] = "Concave";



            ///*Random rnd = new Random(); //亂數產生區塊顏色

            //foreach (DataPoint point in Chart1.Series["Series1"].Points)
            //{

            ////pie 顏色

            //point.Color = Color.FromArgb(150, rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));

            //}*/

            //this.div.Controls.Add(Chart1);
            #endregion

        }

        protected void btn_Click(object sender, EventArgs e)
        {
            Response.Redirect("TryList.aspx");
        }
    }
}