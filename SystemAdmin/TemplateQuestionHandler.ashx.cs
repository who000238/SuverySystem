using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuverySystem.SystemAdmin
{
    /// <summary>
    /// TemplateQuestionHandler 的摘要描述
    /// </summary>
    public class TemplateQuestionHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string actionName = context.Request.QueryString["ActionName"];


            if (string.IsNullOrEmpty(actionName))
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/plain";
                context.Response.Write("ActionName is required");
                context.Response.End();
            }


            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        private void ProcessError(HttpContext context, string msg)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "text/plain";
            context.Response.Write(msg);
            context.Response.End();
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