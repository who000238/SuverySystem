using DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
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


            //列印問卷問題標題
            for (int i = 0; i < SuveryQuestionTitleDT.Rows.Count; i++)
            {
                var QuestionDetailDR = SuveryQuestionTitleDT.Rows[i];
                string QuestionTitle = QuestionDetailDR["DetailTitle"].ToString();
                string QuestionType = QuestionDetailDR["DetailType"].ToString();
                int ItemCount; //單多選項目總數
                if (QuestionDetailDR["ItemCount"].ToString() == string.Empty)
                    ItemCount = 0;
                else
                    ItemCount = (int)QuestionDetailDR["ItemCount"];
                switch (QuestionType)
                {
                    case "QT5":
                    case "QT6":
                        Label lblTitle = new Label();
                        lblTitle.Text = QuestionTitle + "</br>";
                        this.StatisticArea.Controls.Add(lblTitle);
                        for (int j = 0; j < ItemCount; j++)
                        {
                            
                            string ColName = "Item" + (j + 1).ToString();
                            string ItemName = QuestionDetailDR[ColName].ToString();
                            string ItemSelectedCount = GetItemSelectedCount(ItemName);
                            Label lblItemTitle = new Label();
                            lblItemTitle.Text = "&emsp;&emsp;"+ ItemName + "&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;" + $"共 : {ItemSelectedCount} 人"+"</br>";
                            this.StatisticArea.Controls.Add(lblItemTitle);
                        }
                        break;
                }
 
            }
            //var dt = GetSuveryAnswerData(guid);
            //GridView gridView = new GridView();
            //gridView.DataSource = dt;
            //gridView.DataBind();
            //this.StatisticArea.Controls.Add(gridView);
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
        public static DataTable GetSuveryQuestionTItle (Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @" SELECT  * FROM 
                            [SuverySystem].[dbo].[SuveryDetail]
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
                            WHERE Answer = @ItemName
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@ItemName", ItemName));
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
        //public static DataTable GetSuveryAnswerData(Guid guid)
        //{
        //    {
        //        string connectionString = DBHelper.GetConnectionString();
        //        string dbCommandString =
        //                                     @" 
        //                SELECT
        //                 [dbo].[SuveryList].[Title],
        //                 [dbo].[SuveryList].[Guid],
        //                 [dbo].[SuveryData].[TypeOrder],
        //                 [dbo].[SuveryData].[NameOrder],
        //                 [dbo].[SuveryAnswer].[Answer]
        //                FROM [dbo].[SuveryList]
        //                JOIN[dbo].[SuveryData] ON[dbo].[SuveryList].[Guid] =  [dbo].[SuveryData].[Guid]
        //                JOIN 	[dbo].[SuveryAnswer] ON [dbo].[SuveryList].[Guid] =	[dbo].[SuveryAnswer].[Guid]
        //                WHERE[dbo].[SuveryList].[Guid]  = @Guid

        //        ";
        //        List<SqlParameter> list = new List<SqlParameter>();
        //        list.Add(new SqlParameter("@Guid", guid));
        //        try
        //        {
        //            return DBHelper.ReadDataTable(connectionString, dbCommandString, list);
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.WriteLog(ex);
        //            return null;
        //        }
        //    }
        //}
    }
}