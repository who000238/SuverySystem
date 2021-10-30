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
            var AnswerDateilDR = GetAnswerDetail(guid, UserInfo);
            string UserInfoString = AnswerDateilDR["UserInfo"].ToString();
            string[] UserInfoArray = UserInfoString.Split(',');


        }

        public static DataRow GetAnswerDetail(Guid guid,string UserInfo)
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
                return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }
    }
}