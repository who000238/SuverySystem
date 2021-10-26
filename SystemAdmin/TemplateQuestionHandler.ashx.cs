using SuverySystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Web;

namespace SuverySystem.SystemAdmin
{
    /// <summary>
    /// TemplateQuestionHandler 的摘要描述
    /// </summary>
    public class TemplateQuestionHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        //public void ProcessRequest(HttpContext context)
        //{
        //    string actionName = context.Request.QueryString["actionName"];



        //    List<QuestionDetailModel> list = new List<QuestionDetailModel>();


        //    //QuestionDetailModel model = new QuestionDetailModel();

        //    //if (HttpContext.Current.Session["QuestionDetail"] != null)
        //    //{
        //    //    model = (QuestionDetailModel)HttpContext.Current.Session["QuestionDetail"];
        //    //}
        //    //if(model != null)
        //    //{

        //    //}
        //    #region 自建Table
        //    ////create DataTable
        //    //DataTable QuestionDT = new DataTable();
        //    //DataColumn column;
        //    //DataRow row;
        //    ////create DataCol of QuestintNo
        //    //column = new DataColumn();
        //    //column.DataType = Type.GetType("System.Int32");
        //    //column.ColumnName = "QuestintNo";
        //    //QuestionDT.Columns.Add(column);
        //    ////create DataCol of SuveryID
        //    //column = new DataColumn();
        //    //column.DataType = Type.GetType("System.String");
        //    //column.ColumnName = "SuveryID";
        //    //QuestionDT.Columns.Add(column);
        //    ////create DataCol of DetailTitle
        //    //column = new DataColumn();
        //    //column.DataType = Type.GetType("System.String");
        //    //column.ColumnName = "DetailTitle";
        //    //QuestionDT.Columns.Add(column);
        //    ////create DataCol of DetailType
        //    //column = new DataColumn();
        //    //column.DataType = Type.GetType("System.String");
        //    //column.ColumnName = "DetailType";
        //    //QuestionDT.Columns.Add(column);
        //    ////create DataCol of DetailMustKeyin
        //    //column = new DataColumn();
        //    //column.DataType = Type.GetType("System.String");
        //    //column.ColumnName = "DetailMustKeyin";
        //    //QuestionDT.Columns.Add(column);
        //    ////create DataCol of ItemName
        //    //column = new DataColumn();
        //    //column.DataType = Type.GetType("System.String");
        //    //column.ColumnName = "ItemName";
        //    //QuestionDT
        //    //    .Columns.Add(column);
        //    //if (model != null)
        //    //{
        //    //    row = QuestionDT.NewRow();
        //    //    row["QuestintNo"] = model.QuestionNo;
        //    //    row["SuveryID"] = model.SuveryID;
        //    //    row["DetailTitle"] = model.DetailTitle;
        //    //    row["DetailType"] = model.DetailType;
        //    //    row["DetailMustKeyin"] = model.DetailMustKeyin;
        //    //    row["ItemName"] = model.ItemName;
        //    //}
        //    #endregion


        //    if (string.IsNullOrEmpty(actionName))
        //    {
        //        context.Response.StatusCode = 400;
        //        context.Response.ContentType = "text/plain";
        //        context.Response.Write("ActionName is required");
        //        context.Response.End();
        //    }
        //    if (actionName == "Load")
        //    {
        //        //if (QuestionDT.Rows.Count > 0)
        //        //{
        //        //List<QuestionDetailModel> list = new List<QuestionDetailModel>()
        //        //{
        //        //};
        //        //List<QuestionDetailModel> list = QuestionDT.Select(obj => new QuestionDetailModel()
        //        //{
        //        //    QuestionNo = obj.
        //        //})
        //        if (HttpContext.Current.Session["QuestionDetail"] != null)
        //        {
        //            List<QuestionDetailModel> sourceList = new List<QuestionDetailModel>();

        //            sourceList = (List<QuestionDetailModel>)HttpContext.Current.Session["QuestionDetail"];
        //            list.Add((List<QuestionDetailModel>)sourceList.ToList()) ;
        //            string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(list);
        //            context.Response.ContentType = "application/json";
        //            context.Response.Write(jsonText);
        //        }


        //        //}

        //    }
        //    else
        //    {
        //        context.Response.StatusCode = 404;
        //        context.Response.ContentType = "text/plain";
        //        context.Response.Write("Error");
        //        context.Response.End();
        //    }
        //    if (actionName == "Create")
        //    {

        //    }
        //    if (actionName == "Update")
        //    {

        //    }
        //}

        public void ProcessRequest(HttpContext context)
        {
            string actionName = context.Request.QueryString["actionName"];
            #region 自建Table
            //create DataTable
            DataTable QuestionDT = new DataTable();
            DataColumn column;
            DataRow row;
            //create DataCol of QuestintNo
            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "QuestionNo";
            QuestionDT.Columns.Add(column);
            //create DataCol of SuveryID
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "SuveryID";
            QuestionDT.Columns.Add(column);
            //create DataCol of DetailTitle
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "DetailTitle";
            QuestionDT.Columns.Add(column);
            //create DataCol of DetailType
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "DetailType";
            QuestionDT.Columns.Add(column);
            //create DataCol of DetailMustKeyin
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "DetailMustKeyin";
            QuestionDT.Columns.Add(column);
            //create DataCol of ItemName
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "ItemName";
            QuestionDT.Columns.Add(column);
            #endregion

            if (string.IsNullOrEmpty(actionName))
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/plain";
                context.Response.Write("ActionName is required");
                context.Response.End();
            }

             if (actionName == "Create")
            {
                string SuveryID = context.Request.Form["SuveryID"];
                string DetailTitle = context.Request.Form["DetailTitle"];
                string DetailType = context.Request.Form["DetailType"];
                string DetailMustKeyin = context.Request.Form["DetailMustKeyin"];
                string ItemName = context.Request.Form["ItemName"];

                row = QuestionDT.NewRow();
                row["QuestionNo"] = 1;
                row["SuveryID"] = SuveryID;
                row["DetailTitle"] = DetailTitle;
                row["DetailType"] = DetailType;
                row["DetailMustKeyin"] = DetailMustKeyin;
                row["ItemName"] = ItemName;
                QuestionDT.Rows.Add(row);
            }
            else if (actionName == "Load")
            {

                if (HttpContext.Current.Session["QuestionDetail"] != null)
                {
                    QuestionDetailModel model = (QuestionDetailModel)HttpContext.Current.Session["QuestionDetail"];
                    row = QuestionDT.NewRow();
                    row["QuestionNo"] = model.QuestionNo;
                    row["SuveryID"] = model.SuveryID;
                    row["DetailTitle"] = model.DetailTitle;
                    row["DetailType"] = model.DetailType;
                    row["DetailMustKeyin"] = model.DetailMustKeyin;
                    row["ItemName"] = model.ItemName;
                    QuestionDT.Rows.Add(row);
                }

                List<QuestionDetailModel> list = new List<QuestionDetailModel>();
                foreach (DataRow dr in QuestionDT.Rows)
                {
                    QuestionDetailModel doc = new QuestionDetailModel();
                    doc.QuestionNo = QuestionDT.Rows.Count;
                    doc.SuveryID = dr.Field<string>("SuveryID");
                    doc.DetailTitle = dr.Field<string>("DetailTitle");
                    doc.DetailType = dr.Field<string>("DetailType");
                    doc.DetailMustKeyin = dr.Field<string>("DetailMustKeyin");
                    doc.ItemName = dr.Field<string>("ItemName");
                    list.Add(doc);
                }
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(list);
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