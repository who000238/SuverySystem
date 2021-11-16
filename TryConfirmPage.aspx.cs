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
            if (this.Session["ansString"] == null)//session內沒有存放資料但跳進來此頁面，跳出錯誤訊息及跳頁
            {
                Response.Write("<script type='text/javascript'> alert('發生一些錯誤 即將跳轉至列表頁');location.href = 'TryList.aspx';</script>");
                return;
            }
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);
            //Session內存放的回答字串
            string ansString = this.Session["ansString"].ToString(); 
            string[] ansStringArray = ansString.Split(',');

            string UserInfoString = this.Session["UserInfo"].ToString();
            string[] UserInfoArray = UserInfoString.Split(',');
            Label lblForUserInfo = new Label();
            lblForUserInfo.Text = $"姓名 : {UserInfoArray[0]} </br> 電話 : {UserInfoArray[1]}</br>E-Mail : {UserInfoArray[2]}</br> 年齡 : {UserInfoArray[3]} </br></br>";
            this.AnsArea.Controls.Add(lblForUserInfo);

            string SuveryStatus;
            //取得問卷基本資料
            var SuveryMasterDataRow = GetSuveryMasterData(guid);
            //取得問題資料
            var QuestionDetailDT = GetQuestionDetailAndItemDetail(guid); 

            if (SuveryMasterDataRow != null)
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

                    //string tempAnsString = ansStringArray[i];
                    //if (tempAnsString.Contains("&"))
                    //{
                    //    string[] CBLAnsString = tempAnsString.Split('&');
                    //    tempAnsString = string.Join(" ", CBLAnsString);

                    //    //string[] CBLAnsString = tempAnsString.Split('&');
                    //    //for (int j = 0; j < CBLAnsString.Length; j++)
                    //    //{
                    //    //    if (!string.IsNullOrEmpty(CBLAnsString[j]))
                    //    //    {
                    //    //        tempAnsString = string.Join(",", CBLAnsString[j]);
                    //    //    }
                    //    //}

                    //}

                    lblForConfirmMsg.Text = (i + 1).ToString() + ".  " + QuestionDetailDR["DetailTitle"].ToString() + "</br>" + ansStringArray[i] + "</br></br>";
                    this.AnsArea.Controls.Add(lblForConfirmMsg);
                }
            }
        }
        /// <summary>送出問卷填答內容</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);
            //取得問題資料
            var QuestionDetailDT = GetQuestionDetailAndItemDetail(guid);
            //取得固定問題之使用者資料
            var UserInfoString = this.Session["UserInfo"].ToString();
            SaveUserInfo(guid, UserInfoString);
            //取得使用者輸入問卷回覆內容
            string ansString = this.Session["ansString"].ToString();
            string[] ansStringArray = ansString.Split(',');
            for (int i = 0; i < QuestionDetailDT.Rows.Count; i++)
            {
                var QuestionDetailDR = QuestionDetailDT.Rows[i];
                int DetailID = (int)QuestionDetailDR["DetailID"];
                string AnswerString = ansStringArray[i];
                SaveSuveryAnswer(guid, DetailID, AnswerString, UserInfoString);
            }
            Response.Redirect("TryList.aspx");
        }

        #region Method區
        /// <summary>保存問卷回答</summary>
        /// <param name="guid"></param>
        /// <param name="DetailID"></param>
        /// <param name="AnswerString"></param>
        /// <param name="UserInfoString"></param>
        public static void SaveSuveryAnswer(Guid guid, int DetailID, string AnswerString, string UserInfoString)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                                            @"
                                            INSERT INTO [dbo].[AnswerDetail]
                                                       ([SuveryID]
                                                       ,[DetailID]
                                                       ,[Answer]
                                                       ,[UserInfo]
                                                       ,[CreateTime])
                                                 VALUES
                                                       (@Guid
                                                       ,@DetailID
                                                       ,@Answer
                                                       ,@UserInfo
                                                       ,@CreateTime)                  
                                            ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            list.Add(new SqlParameter("@DetailID", DetailID));
            list.Add(new SqlParameter("@Answer", AnswerString));
            list.Add(new SqlParameter("@UserInfo", UserInfoString));
            list.Add(new SqlParameter("@CreateTime", DateTime.Now));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        /// <summary>保存固定問題之使用者資訊</summary>
        /// <param name="guid"></param>
        /// <param name="UserInfo"></param>
        public static void SaveUserInfo(Guid guid,string UserInfo)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                                            @"
                                            INSERT INTO [dbo].[UserInfo]
                                                       ([SuveryID]
                                                       ,[UserInfo]
                                                       ,[CreateTime])
                                                 VALUES
                                                       (@SuveryID
                                                       ,@UserInfo
                                                       ,@CreateTime)              
                                            ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@SuveryID", guid));
            list.Add(new SqlParameter("@UserInfo", UserInfo));
            list.Add(new SqlParameter("@CreateTime", DateTime.Now));

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