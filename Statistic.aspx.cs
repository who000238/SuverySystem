using DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace SuverySystem
{
    public partial class Statistic : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);
            //取得問卷標題
            var SuveryDataRow = GetSuveryMasterData(guid);
            this.h3Title.InnerText = SuveryDataRow["Title"].ToString();
            //取得問卷問題標題
            var SuveryQuestionTitleDT = GetQuestionDetailAndItemDetail(guid);

            #region 統計文字版
            ////列印問卷問題標題
            //for (int i = 0; i < SuveryQuestionTitleDT.Rows.Count; i++)
            //{
            //    var QuestionDetailDR = SuveryQuestionTitleDT.Rows[i];
            //    string QuestionTitle = QuestionDetailDR["DetailTitle"].ToString();
            //    string QuestionType = QuestionDetailDR["DetailType"].ToString();
            //    int ItemCount; //單多選項目總數
            //    if (QuestionDetailDR["ItemCount"].ToString() == string.Empty)
            //        ItemCount = 0;
            //    else
            //        ItemCount = (int)QuestionDetailDR["ItemCount"];
            //    Label lblTitle = new Label(); //問題標題的lbl
            //    switch (QuestionType)
            //    {
            //        case "QT5":
            //            lblTitle.Text = QuestionTitle + "</br>";
            //            this.StatisticArea.Controls.Add(lblTitle);
            //            for (int j = 0; j < ItemCount; j++)
            //            {

            //                string ColName = "Item" + (j + 1).ToString();
            //                string ItemName = QuestionDetailDR[ColName].ToString();
            //                string ItemSelectedCount = GetItemSelectedCount(ItemName);
            //                Label lblItemTitle = new Label();
            //                lblItemTitle.Text = "&emsp;&emsp;" + ItemName + "&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;" + $"共 : {ItemSelectedCount} 人" + "</br>";
            //                this.StatisticArea.Controls.Add(lblItemTitle);
            //            }
            //            break;
            //        case "QT6":
            //            lblTitle.Text = QuestionTitle + "</br>";
            //            this.StatisticArea.Controls.Add(lblTitle);
            //            for (int j = 0; j < ItemCount; j++)
            //            {
            //                string ColName = "Item" + (j + 1).ToString();
            //                string ItemName = QuestionDetailDR[ColName].ToString();
            //                string ItemSelectedCount = GetItemSelectedCount(ItemName);
            //                Label lblItemTitle = new Label();
            //                lblItemTitle.Text = "&emsp;&emsp;" + ItemName + "&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;" + $"共 : {ItemSelectedCount} 人" + "</br>";
            //                this.StatisticArea.Controls.Add(lblItemTitle);
            //            }
            //            break;
            //    }
            //}
            #endregion
            //列印問卷問題標題
            for (int i = 0; i < SuveryQuestionTitleDT.Rows.Count; i++)
            {
                var QuestionDetailDR = SuveryQuestionTitleDT.Rows[i];                      //AnswerTable內的DR
                string QuestionTitle = QuestionDetailDR["DetailTitle"].ToString();       //問題的Title
                string QuestionType = QuestionDetailDR["DetailType"].ToString();      //問題的Type  
                //單多選項目總數
                int ItemCount;
                if (QuestionDetailDR["ItemCount"].ToString() == string.Empty)
                    ItemCount = 0;
                else
                    ItemCount = (int)QuestionDetailDR["ItemCount"];
                //問題標題的lbl
                //Label lblTitle = new Label();
                //lblTitle.Text = "</br>" + QuestionTitle + "</br>";
                //this.StatisticArea.Controls.Add(lblTitle);


                string[] Title = new string[ItemCount];
                int[] Answer = new int[ItemCount];

                Chart Chart1 = new Chart();
                Title title = new Title();

                switch (QuestionType)
                {

                    case "QT5":

                        for (int j = 0; j < ItemCount; j++)
                        {
                            string ColName = "Item" + (j + 1).ToString();
                            string ItemName = QuestionDetailDR[ColName].ToString();

                            Title[j] = ItemName;

                            string ItemSelectedCount = GetItemSelectedCount(ItemName);

                            Answer[j] = Convert.ToInt32(ItemSelectedCount);

                        }
                        #region 圓餅圖產生

                        //ChartAreas,Series,Legends 基本設定-------------------------------------------------
                        Chart1.ChartAreas.Add("ChartArea1"); //圖表區域集合
                        Chart1.Legends.Add("Legends1"); //圖例集合說明
                        Chart1.Series.Add("Series1"); //數據序列集合
                                                      //設定 Chart-------------------------------------------------------------------------
                        Chart1.Width = 770;
                        Chart1.Height = 400;
                        title.Text = QuestionTitle;
                        title.Alignment = ContentAlignment.MiddleCenter;
                        title.Font = new System.Drawing.Font("Trebuchet MS", 14F, FontStyle.Bold);
                        Chart1.Titles.Add(title);
                        //設定 ChartArea1--------------------------------------------------------------------
                        Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                        Chart1.ChartAreas[0].AxisX.Interval = 1;
                        //設定 Legends-------------------------------------------------------------------------
                        //Chart1.Legends["Legends1"].DockedToChartArea = "ChartArea1"; //顯示在圖表內
                        //Chart1.Legends["Legends1"].Docking = Docking.Bottom; //自訂顯示位置
                        //背景色
                        Chart1.Legends["Legends1"].BackColor = Color.FromArgb(235, 235, 235);
                        //斜線背景
                        Chart1.Legends["Legends1"].BackHatchStyle = ChartHatchStyle.DarkDownwardDiagonal;
                        Chart1.Legends["Legends1"].BorderWidth = 1;
                        Chart1.Legends["Legends1"].BorderColor = Color.FromArgb(200, 200, 200);
                        //設定 Series1-----------------------------------------------------------------------
                        Chart1.Series["Series1"].ChartType = SeriesChartType.Pie;
                        //Chart1.Series["Series1"].ChartType = SeriesChartType.Doughnut;
                        Chart1.Series["Series1"].Points.DataBindXY(Title, Answer);
                        Chart1.Series["Series1"].LegendText = "#VALX: [ #PERCENT{P1} ]"; //X軸 + 百分比
                        Chart1.Series["Series1"].Label = "#VALX\n#PERCENT{P1}"; //X軸 + 百分比
                                                                                //Chart1.Series["Series1"].LabelForeColor = Color.FromArgb(0, 90, 255); //字體顏色
                                                                                //字體設定
                        Chart1.Series["Series1"].Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold);
                        Chart1.Series["Series1"].Points.FindMaxByValue().LabelForeColor = Color.Red;
                        //Chart1.Series["Series1"].Points.FindMaxByValue().Color = Color.Red;
                        //Chart1.Series["Series1"].Points.FindMaxByValue()["Exploded"] = "true";
                        Chart1.Series["Series1"].BorderColor = Color.FromArgb(255, 101, 101, 101);
                        //Chart1.Series["Series1"]["DoughnutRadius"] = "80"; // ChartType為Doughnut時，Set Doughnut hole size
                        //Chart1.Series["Series1"]["PieLabelStyle"] = "Inside"; //數值顯示在圓餅內
                        Chart1.Series["Series1"]["PieLabelStyle"] = "Outside"; //數值顯示在圓餅外
                                                                               //Chart1.Series["Series1"]["PieLabelStyle"] = "Disabled"; //不顯示數值
                                                                               //設定圓餅效果，除 Default 外其他效果3D不適用
                        Chart1.Series["Series1"]["PieDrawingStyle"] = "Default";
                        //Chart1.Series["Series1"]["PieDrawingStyle"] = "SoftEdge";
                        //Chart1.Series["Series1"]["PieDrawingStyle"] = "Concave";
                        /*Random rnd = new Random(); //亂數產生區塊顏色
                        foreach (DataPoint point in Chart1.Series["Series1"].Points)
                        {

                        //pie 顏色

                        point.Color = Color.FromArgb(150, rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                        }*/
                        this.StatisticArea.Controls.Add(Chart1);
                        #endregion

                        break;
                    case "QT6":

                        for (int j = 0; j < ItemCount; j++)
                        {
                            string ColName = "Item" + (j + 1).ToString();
                            string ItemName = QuestionDetailDR[ColName].ToString();
                            Title[j] = ItemName;

                            string ItemSelectedCount = GetItemSelectedCount(ItemName);

                            Answer[j] = Convert.ToInt32(ItemSelectedCount);

                            //Label lblItemTitle = new Label();
                            //lblItemTitle.Text = "&emsp;&emsp;" + ItemName + "&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;" + $"共 : {ItemSelectedCount} 人" + "</br>";
                            //this.StatisticArea.Controls.Add(lblItemTitle);
                        }
                        #region 圓餅圖產生

                        //ChartAreas,Series,Legends 基本設定-------------------------------------------------
                        Chart1.ChartAreas.Add("ChartArea1"); //圖表區域集合
                        Chart1.Legends.Add("Legends1"); //圖例集合說明
                        Chart1.Series.Add("Series1"); //數據序列集合
                                                      //設定 Chart-------------------------------------------------------------------------
                        Chart1.Width = 770;
                        Chart1.Height = 400;
                        title.Text = QuestionTitle;
                        title.Alignment = ContentAlignment.MiddleCenter;
                        title.Font = new System.Drawing.Font("Trebuchet MS", 14F, FontStyle.Bold);
                        Chart1.Titles.Add(title);
                        //設定 ChartArea1--------------------------------------------------------------------
                        Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                        Chart1.ChartAreas[0].AxisX.Interval = 1;
                        //設定 Legends-------------------------------------------------------------------------
                        //Chart1.Legends["Legends1"].DockedToChartArea = "ChartArea1"; //顯示在圖表內
                        //Chart1.Legends["Legends1"].Docking = Docking.Bottom; //自訂顯示位置
                        //背景色
                        Chart1.Legends["Legends1"].BackColor = Color.FromArgb(235, 235, 235);
                        //斜線背景
                        Chart1.Legends["Legends1"].BackHatchStyle = ChartHatchStyle.DarkDownwardDiagonal;
                        Chart1.Legends["Legends1"].BorderWidth = 1;
                        Chart1.Legends["Legends1"].BorderColor = Color.FromArgb(200, 200, 200);
                        //設定 Series1-----------------------------------------------------------------------
                        Chart1.Series["Series1"].ChartType = SeriesChartType.Pie;
                        //Chart1.Series["Series1"].ChartType = SeriesChartType.Doughnut;
                        Chart1.Series["Series1"].Points.DataBindXY(Title, Answer);
                        Chart1.Series["Series1"].LegendText = "#VALX: [ #PERCENT{P1} ]"; //X軸 + 百分比
                        Chart1.Series["Series1"].Label = "#VALX\n#PERCENT{P1}"; //X軸 + 百分比
                                                                                //Chart1.Series["Series1"].LabelForeColor = Color.FromArgb(0, 90, 255); //字體顏色
                                                                                //字體設定
                        Chart1.Series["Series1"].Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold);
                        Chart1.Series["Series1"].Points.FindMaxByValue().LabelForeColor = Color.Red;
                        //Chart1.Series["Series1"].Points.FindMaxByValue().Color = Color.Red;
                        //Chart1.Series["Series1"].Points.FindMaxByValue()["Exploded"] = "true";
                        Chart1.Series["Series1"].BorderColor = Color.FromArgb(255, 101, 101, 101);
                        //Chart1.Series["Series1"]["DoughnutRadius"] = "80"; // ChartType為Doughnut時，Set Doughnut hole size
                        //Chart1.Series["Series1"]["PieLabelStyle"] = "Inside"; //數值顯示在圓餅內
                        Chart1.Series["Series1"]["PieLabelStyle"] = "Outside"; //數值顯示在圓餅外
                                                                               //Chart1.Series["Series1"]["PieLabelStyle"] = "Disabled"; //不顯示數值
                                                                               //設定圓餅效果，除 Default 外其他效果3D不適用
                        Chart1.Series["Series1"]["PieDrawingStyle"] = "Default";
                        //Chart1.Series["Series1"]["PieDrawingStyle"] = "SoftEdge";
                        //Chart1.Series["Series1"]["PieDrawingStyle"] = "Concave";
                        /*Random rnd = new Random(); //亂數產生區塊顏色
                        foreach (DataPoint point in Chart1.Series["Series1"].Points)
                        {

                        //pie 顏色

                        point.Color = Color.FromArgb(150, rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                        }*/
                        this.StatisticArea.Controls.Add(Chart1);
                        #endregion

                        break;
                }
            }
        }
        #region Method
        /// <summary>取得問卷基本資料</summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static DataRow GetSuveryMasterData(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @" SELECT  * FROM [SuverySystem].[dbo].[SuveryMaster]
                     WHERE SuveryID = @Guid
                    
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            try
            {
                return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }
        //public static DataTable GetSuveryQuestionTItle (Guid guid)
        //{
        //    string connectionString = DBHelper.GetConnectionString();
        //    string dbCommandString =
        //         @" SELECT  * FROM 
        //                    [SuverySystem].[dbo].[SuveryDetail]
        //             WHERE [SuverySystem].[dbo].[SuveryDetail].[SuveryID] = @Guid
        //            ORDER BY [SuverySystem].[dbo].[SuveryDetail].[DetailID]
        //        ";
        //    List<SqlParameter> list = new List<SqlParameter>();
        //    list.Add(new SqlParameter("@Guid", guid));
        //    try
        //    {
        //        return DBHelper.ReadDataTable(connectionString, dbCommandString, list);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog(ex);
        //        return null;
        //    }
        //}

        //
        public static DataTable GetQuestionDetailAndItemDetail(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @" SELECT  * FROM 
                            [SuverySystem].[dbo].[SuveryDetail]
					  LEFT JOIN 
                            [SuverySystem].[dbo].[ItemDetail] 
                        ON 
                        [SuverySystem].[dbo].[SuveryDetail].[DetailID] =  [SuverySystem].[dbo].[ItemDetail].[DetailID]
                     WHERE [SuverySystem].[dbo].[SuveryDetail].[SuveryID] = @Guid
                    ORDER BY [SuverySystem].[dbo].[SuveryDetail].[DetailID]
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            try
            {
                return DBHelper.ReadDataTable(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }
        //
        public static string GetItemSelectedCount(string ItemName)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                          @" SELECT COUNT([SuverySystem].[dbo].[AnswerDetail].[Answer]) AS SelectedCount
                                FROM  AnswerDetail 
                            WHERE Answer LIKE @ItemName
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@ItemName", "%" + ItemName + "%"));
            try
            {
                var dr = DBHelper.ReadDataRow(connectionString, dbCommandString, list);
                string ItemSelectedCount = dr["SelectedCount"].ToString();
                return ItemSelectedCount;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }
        #endregion
    }
}