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
    public partial class TryConfirmPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["ansString"] == null)
            {

            }
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);

            string ansString = this.Session["ansString"].ToString(); //Session內存放的回答字串
            string[] ansStringArray = ansString.Split(',');
            
            string SuveryStatus;
            var SuveryMasterDataRow = GetSuveryMasterData(guid); //取得問卷基本資料
            var QuestionDetailDT = GetQuestionDetailAndItemDetail(guid); //取得問題資料

            if (SuveryMasterDataRow!=null)
            {
            if (SuveryMasterDataRow["Status"].ToString() == "N")
                SuveryStatus = "關閉中";
            else
                SuveryStatus = "開放中";
                this.ltlStatusAndDate.Text = $"{SuveryStatus}</br>{SuveryMasterDataRow["StartDate"]}~{SuveryMasterDataRow["EndDate"]}";
                this.h3Title.InnerText = SuveryMasterDataRow["Title"].ToString();
                for (int i = 0; i < QuestionDetailDT.Rows.Count; i++)
                {
                    var QuestionDetailDR = QuestionDetailDT.Rows[i];
                    Label lblForConfirmMsg = new Label();
                    lblForConfirmMsg.Text = (i + 1).ToString() + ".  " + QuestionDetailDR["DetailTitle"].ToString() + "</br>" + ansStringArray[i] + "</br></br>";
                    this.AnsArea.Controls.Add(lblForConfirmMsg);
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);
            var QuestionDetailDT = GetQuestionDetailAndItemDetail(guid); //取得問題資料
            string ansString = this.Session["ansString"].ToString();
            string[] ansStringArray = ansString.Split(',');
            for (int i = 0; i < QuestionDetailDT.Rows.Count; i++)
            {
                var QuestionDetailDR = QuestionDetailDT.Rows[i];
                int DetailID = (int)QuestionDetailDR["DetailID"];
                string AnswerString = ansStringArray[i];
                SaveSuveryAnswer(guid, DetailID, AnswerString);
            }
            Response.Redirect("TryList.aspx");

        }

        #region Method區

        public static void SaveSuveryAnswer(Guid guid,int DetailID, string AnswerString)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                                            @"
                                            INSERT INTO [dbo].[AnswerDetail]
                                                       ([SuveryID]
                                                       ,[DetailID]
                                                       ,[Answer])
                                                 VALUES
                                                       (@Guid
                                                       ,@DetailID
                                                       ,@Answer)                                          
                                            ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            list.Add(new SqlParameter("@DetailID", DetailID));
            list.Add(new SqlParameter("@Answer", AnswerString));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

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
        /// <summary>取的問卷內問題以及單多選項目資料</summary>
        /// <param name="guid"></param>
        /// <returns></returns>
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

        #endregion

    }
}