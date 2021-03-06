using DBSource;
using Method;
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

                BackgroundMethod.DeleteSuvery(SuveryID);

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



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}