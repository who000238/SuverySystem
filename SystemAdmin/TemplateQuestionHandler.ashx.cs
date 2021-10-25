using SuverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuverySystem.SystemAdmin
{
    /// <summary>
    /// TemplateQuestionHandler 的摘要描述
    /// </summary>
    public class TemplateQuestionHandler : IHttpHandler , System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string actionName = context.Request.QueryString["actionName"];


            if (string.IsNullOrEmpty(actionName))
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/plain";
                context.Response.Write("ActionName is required");
                context.Response.End();
            }
            if (actionName == "Load")
            {
          
                QuestionDetailModel model = new QuestionDetailModel();
                model = (QuestionDetailModel)HttpContext.Current.Session["QuestionDetail"];

                if (model != null)
                {
                    string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(jsonText);
                }
               
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