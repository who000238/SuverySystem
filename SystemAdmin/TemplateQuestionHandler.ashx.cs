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
            string guid = context.Request.QueryString["ID"];

            #region 自建Table
            ////create DataTable
            //DataTable QuestionDT = new DataTable();
            //DataColumn column;
            //DataRow row;
            ////create DataCol of QuestintNo
            //column = new DataColumn();
            //column.DataType = Type.GetType("System.Int32");
            //column.ColumnName = "QuestionNo";
            //QuestionDT.Columns.Add(column);
            ////create DataCol of SuveryID
            //column = new DataColumn();
            //column.DataType = Type.GetType("System.String");
            //column.ColumnName = "SuveryID";
            //QuestionDT.Columns.Add(column);
            ////create DataCol of DetailTitle
            //column = new DataColumn();
            //column.DataType = Type.GetType("System.String");
            //column.ColumnName = "DetailTitle";
            //QuestionDT.Columns.Add(column);
            ////create DataCol of DetailType
            //column = new DataColumn();
            //column.DataType = Type.GetType("System.String");
            //column.ColumnName = "DetailType";
            //QuestionDT.Columns.Add(column);
            ////create DataCol of DetailMustKeyin
            //column = new DataColumn();
            //column.DataType = Type.GetType("System.String");
            //column.ColumnName = "DetailMustKeyin";
            //QuestionDT.Columns.Add(column);
            ////create DataCol of ItemName
            //column = new DataColumn();
            //column.DataType = Type.GetType("System.String");
            //column.ColumnName = "ItemName";
            //QuestionDT.Columns.Add(column);
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
                switch (DetailType)
                {
                    case "QT1":
                        DetailType = "文字方塊(文字)";
                        break;
                    case "QT2":
                        DetailType = "文字方塊(數字)";
                        break;
                    case "QT3":
                        DetailType = "文字方塊(E-Mail)";
                        break;
                    case "QT4":
                        DetailType = "文字方塊(日期)";
                        break;
                    case "QT5":
                        DetailType = "單選方塊";
                        break;
                    case "QT6":
                        DetailType = "多選方塊";
                        break;
                }
                string DetailMustKeyin;
                //
                var temp = context.Request.Form["DetailMustKeyin"];
                //
                    if (context.Request.Form["DetailMustKeyin"] == "Y")
                {
                    DetailMustKeyin = "Y";
                }
                else
                {
                    DetailMustKeyin = "N";
                }
                string ItemName = context.Request.Form["ItemName"];




                if (HttpContext.Current.Session["QuestionDetail"] == null)
                {
                    QuestionDetailModel model = new QuestionDetailModel()
                    {
                        SuveryID = SuveryID,
                        DetailTitle = DetailTitle,
                        DetailType = DetailType,
                        DetailMustKeyin = ((DetailMustKeyin == "Y") ? "Yes" : "No"),
                        ItemName = ItemName
                    };
                    List<QuestionDetailModel> list = new List<QuestionDetailModel>();
                    model.QuestionNo = list.Count + 1;
                    list.Add(model);
                    HttpContext.Current.Session["QuestionDetail"] = list;

                }
                else
                {
                    QuestionDetailModel model = new QuestionDetailModel()
                    {
                        SuveryID = SuveryID,
                        DetailTitle = DetailTitle,
                        DetailType = DetailType,
                        DetailMustKeyin = ((DetailMustKeyin == "Y") ? "Yes" : "No"),
                        ItemName = ItemName
                    };
                    List<QuestionDetailModel> list = (List<QuestionDetailModel>)HttpContext.Current.Session["QuestionDetail"];
                    model.QuestionNo = list.Count + 1;
                    list.Add(model);
                    HttpContext.Current.Session["QuestionDetail"] = list;
                }


            }
            else if (actionName == "Delete")
            {
                string rowID = context.Request.Form["ID"];
                int id = Convert.ToInt32(rowID);
                List<QuestionDetailModel> list = (List<QuestionDetailModel>)HttpContext.Current.Session["QuestionDetail"];
                list.RemoveAt(id);

                HttpContext.Current.Session["QuestionDetail"] = list;


                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                context.Response.ContentType = "application/json";
                context.Response.Write(jsonText);

            }
            else if (actionName == "Load")
            {

                if (HttpContext.Current.Session["QuestionDetail"] != null)
                {
                    var list = (List<QuestionDetailModel>)HttpContext.Current.Session["QuestionDetail"];

                    string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(jsonText);
                }

            }
            else if (actionName == "query")
            {
                string rowID = context.Request.Form["ID"];
                int id = Convert.ToInt32(rowID);
                List<QuestionDetailModel> list = (List<QuestionDetailModel>)HttpContext.Current.Session["QuestionDetail"];

                //QuestionDetailModel model = list[id - 1];
                QuestionDetailModel model = new QuestionDetailModel();
                model.DetailTitle = list[id - 1].DetailTitle;
                var QType = list[id - 1].DetailType;
                switch (QType)
                {
                    case "文字方塊(文字)":
                        QType = "QT1";
                        break;
                    case "文字方塊(數字)":
                        QType = "QT2";
                        break;
                    case "文字方塊(E-Mail)":
                        QType = "QT3";
                        break;
                    case "文字方塊(日期)":
                        QType = "QT4";
                        break;
                    case "單選方塊":
                        QType = "QT5";
                        break;
                    case "多選方塊":
                        QType = "QT6";
                        break;
                }
                model.DetailType = QType;
                model.DetailMustKeyin = list[id - 1].DetailMustKeyin;
                if (list[id - 1].ItemName != "")
                {
                    model.ItemName = list[id - 1].ItemName;
                }
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(model);
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