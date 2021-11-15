using DBSource;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SuverySystem.SystemAdmin
{
    /// <summary>
    /// AdminListHandler 的摘要描述
    /// </summary>
    public class AdminListHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string actionName = context.Request.QueryString["actionName"];

            if (actionName == "Delete")
            {
                string SuveryID = context.Request.Form["ID"];

                DeleteSuvery(SuveryID);

                string returnString = "Success";

                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(returnString);
                context.Response.ContentType = "application/json";
                context.Response.Write(jsonText);
            }
            else
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "text/plain";
                context.Response.Write("Error");
                context.Response.End();

            }
        }


        public static void DeleteSuvery(string guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    DELETE FROM [dbo].[SuveryMaster]
                    WHERE [SuveryID] = @SuveryID
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@SuveryID", guid));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}