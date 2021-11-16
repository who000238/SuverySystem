using DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuverySystem.SystemAdmin
{
    public partial class SeeAnswerDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string SuveryID = Request.QueryString["ID"];
            Guid guid = Guid.Parse(SuveryID);
            string UserInfo = Request.QueryString["UserInfo"];
            var AnswerDateilDT = GetAnswerDetail(guid, UserInfo);
            if (AnswerDateilDT.Rows.Count >0)
            {
                string UserInfoString = AnswerDateilDT.Rows[0]["UserInfo"].ToString();
                string[] UserInfoArray = UserInfoString.Split(',');
                this.tbName.Text = "姓名 : " + UserInfoArray[0];
                this.tbPhone.Text = "電話 : " + UserInfoArray[1];
                this.tbEMail.Text = "信箱 : " + UserInfoArray[2];
                this.tbAge.Text = "年齡 : " + UserInfoArray[3];


                for (int i = 0; i < AnswerDateilDT.Rows.Count; i++)
                {
                    var dr = AnswerDateilDT.Rows[i];
                    Label lblForTitle = new Label();
                    Label lblForAnswer = new Label();
                    lblForTitle.Text = "問題標題 : " + dr["DetailTitle"].ToString();
                    lblForAnswer.Text = "回答 :          " + dr["Answer"].ToString() + "</br></br>";
                    this.AnswerPost.Controls.Add(lblForTitle);
                    this.AnswerPost.Controls.Add(lblForAnswer);
                }
            }
            else
            {
                Response.Write("<script>alert('出現一些問題，即將返回上一頁');location.href=window.history.back();</script>");
            }

          
        }
        /// <summary>取得三張資料表內的內容分別是問題清單，選項清單，答案清單</summary>
        /// <param name="guid"></param>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public static  DataTable GetAnswerDetail(Guid guid,string UserInfo)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @"
                        SELECT 
			                        SuveryDetail.DetailID,
			                        SuveryDetail.SuveryID,
			                        SuveryDetail.DetailTitle,
			                        SuveryDetail.DetailType,
			                        SuveryDetail.DetailMustKeyin,
			                        ItemDetail.Item1,
			                        ItemDetail.Item2,
			                        ItemDetail.Item3,
			                        ItemDetail.Item4,
			                        ItemDetail.ItemCount,
			                        AnswerDetail.Answer,
			                        AnswerDetail.CreateTime,
			                        AnswerDetail.UserInfo
                        FROM 

                                    [SuverySystem].[dbo].[SuveryDetail]
                         LEFT JOIN 
                                    [SuverySystem].[dbo].[ItemDetail] 
                        ON 
                                    [SuverySystem].[dbo].[SuveryDetail].[DetailID] =  [SuverySystem].[dbo].[ItemDetail].[DetailID]
                        JOIN 
                                [SuverySystem].[dbo].[AnswerDetail] 
                        ON 
                                [SuverySystem].[dbo].[SuveryDetail].[DetailID] =[SuverySystem].[dbo].[AnswerDetail].[DetailID]
                        WHERE 
                                [SuverySystem].[dbo].[SuveryDetail].[SuveryID] = @Guid
                        AND
		                        [SuverySystem].[dbo].[AnswerDetail].[UserInfo] = @UserInfo
                        ORDER BY [SuverySystem].[dbo].[SuveryDetail].[DetailID]                      
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            list.Add(new SqlParameter("@UserInfo", UserInfo));
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
    }
}