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
    /// AdminTemplateQuestionHandler 的摘要描述
    /// </summary>
    public class AdminTemplateQuestionHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string actionName = context.Request.QueryString["actionName"];

            if (actionName == "Delete")
            {
                string QuestionTemplateNo = context.Request.Form["ID"];
                BackgroundMethod.DeleteQuestionTemplate(QuestionTemplateNo);
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