using DBSource;
using SuverySystem.Models;
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
    public partial class Detail1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtAnswer.Enabled = false;
            string SuveryID = Request.QueryString["ID"].ToString();
            this.hfSuveryID.Value = SuveryID;

            var QuestionTemplateDT = GetQuestionTemplate();
            for (int i = 0; i < QuestionTemplateDT.Rows.Count; i++)
            {
                var QuestionTemplateDR = QuestionTemplateDT.Rows[i];
                this.TemplateQddl.Items.Add(QuestionTemplateDR["QuestionTemplateName"].ToString());
            }
            //if (HttpContext.Current.Session["QuestionDetail"] != null)
            //{
            //    Response.Write("<script>alert('QuestionDetail is exist !')</script>");
            //}
            //else
            //    Response.Write("<script>alert('QuestionDetail is null !')</script>");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string guidString = Request.QueryString["ID"];
            Guid guid = Guid.Parse(guidString);
            // get input

            string SuveryTitle = this.txtSuveryTitle.Text;
            string SuverySummary = this.txtSummary.Text;
            string StartDate = this.txtStartDate.Text;
            string EndDate = this.txtEndDate.Text;
            string Status;
            if (this.StatusCheck.Checked == true)
                Status = "Y";
            else
                Status = "N";
            // checkinput
            if (string.IsNullOrEmpty(SuveryTitle) ||
               string.IsNullOrEmpty(SuverySummary) ||
               string.IsNullOrEmpty(StartDate) ||
               string.IsNullOrEmpty(EndDate))
            {
                Response.Write("<script>alert('請確認所有輸入框都有輸入值')</script>");
                return;
            }
            else
            {
                //string SuveryMaster = SuveryTitle + "," + SuverySummary + "," + StartDate + "," + EndDate + "," + Status;
                //this.Session["SuveryMaster"] = SuveryMaster;
                //Response.Write($"<script>alert('{SuveryMaster}')</script>");

                CreateNewSuvery(guid, SuveryTitle, SuverySummary, StartDate, EndDate, Status);

                return;
            }
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string guidString = Request.QueryString["ID"];
            string QuestionTitle = this.txtQuestion.Text;
            string QuestionType = this.QTypeddl.SelectedValue;
            string QuestionIsMustKeyIn;
            if (this.QMustKeyIn.Checked)
                QuestionIsMustKeyIn = "Y";
            else
                QuestionIsMustKeyIn = "N";
            string ItemName = string.Empty;
            string QuestionDetail;
            if (QuestionType == "QT5" || QuestionType == "QT6")
            {
                ItemName = this.txtAnswer.Text;
            }
            if (string.IsNullOrEmpty(ItemName))
                QuestionDetail = QuestionTitle + QuestionType + QuestionIsMustKeyIn;
            else
                QuestionDetail = QuestionTitle + QuestionType + QuestionIsMustKeyIn + ItemName;

     
            if (HttpContext.Current.Session["QuestionDetail"] == null)
            {
                QuestionDetailModel model = new QuestionDetailModel()
                {
                    SuveryID = guidString,
                    DetailTitle = QuestionTitle,
                    DetailType = QuestionType,
                    DetailMustKeyin = QuestionIsMustKeyIn,
                    ItemName = ItemName
                };
                List<QuestionDetailModel> list = new List<QuestionDetailModel>();
                list.Add(model);
                HttpContext.Current.Session["QuestionDetail"] = list;
            }
            else
            {
                List<QuestionDetailModel> sourceList = (List<QuestionDetailModel>)HttpContext.Current.Session["QuestionDetail"];

                QuestionDetailModel model = new QuestionDetailModel()
                {
                    SuveryID = guidString,
                    DetailTitle = QuestionTitle,
                    DetailType = QuestionType,
                    DetailMustKeyin = QuestionIsMustKeyIn,
                    ItemName = ItemName
                };


                List<QuestionDetailModel> list = new List<QuestionDetailModel>();
                list.Add(model);
                HttpContext.Current.Session["QuestionDetail"] = sourceList;

            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancle2_Click(object sender, EventArgs e)
        {

        }

        protected void btnSubmit2_Click(object sender, EventArgs e)
        {

        }

        protected void QTypeddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.QTypeddl.SelectedItem.Value == "QT5" || this.QTypeddl.SelectedItem.Value == "QT6")
                this.txtAnswer.Enabled = true;
            else
            {
                this.txtAnswer.Text = string.Empty;
                this.txtAnswer.Enabled = false;
            }
        }


        #region Method
        public static void CreateNewSuvery(Guid guid, string Title, string Summary, string StartDate, string EndDate, string Status)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    INSERT INTO [dbo].[SuveryMaster]
                               ([SuveryID]
                               ,[Title]
                               ,[StartDate]
                               ,[EndDate]
                               ,[Status]
                               ,[Summary])
                         VALUES
                               (@Guid
                               ,@Title
                               ,@StartDate
                               ,@EndDate
                               ,@Status
                               ,@Summary)
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            list.Add(new SqlParameter("@Title", Title));
            list.Add(new SqlParameter("@Summary", Summary));
            list.Add(new SqlParameter("@StartDate", StartDate));
            list.Add(new SqlParameter("@EndDate", EndDate));
            list.Add(new SqlParameter("@Status", Status));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        public static DataTable GetQuestionTemplate()
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    SELECT * FROM [SuverySystem].[dbo].[QuestionTemplateDetail]
                ";
            List<SqlParameter> list = new List<SqlParameter>();
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

        public static DataRow GetQuestionTemplateDrDetail(string QuestionTemplateName)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
               @" 
                    SELECT * FROM [SuverySystem].[dbo].[QuestionTemplateDetail]
                    WHERE QuestionTemplateName = @QuestionTemplateName
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@QuestionTemplateName", QuestionTemplateName));
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
        #endregion


        protected void TemplateQddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            string QuestionTemplateName = this.TemplateQddl.SelectedItem.Text;
            var QuestionTemplateDrDetail = GetQuestionTemplateDrDetail(QuestionTemplateName);
            string QName = QuestionTemplateDrDetail["QuestionTemplateName"].ToString();
            string QType = QuestionTemplateDrDetail["QuestionTemplateType"].ToString();
            string QMustKeyIn = QuestionTemplateDrDetail["QuestionTemplateMustKeyIn"].ToString();
            string QuestionTemplateItemName=string.Empty;
            if (QuestionTemplateDrDetail["QuestionTemplateItemName"] != null)
            {
                QuestionTemplateItemName = QuestionTemplateDrDetail["QuestionTemplateItemName"].ToString();
            }

            this.txtQuestion.Text = QName;
            switch (QType)
            {
                case"QT1":
                    this.QTypeddl.SelectedIndex = 0;
                    break;
                case "QT2":

                    this.QTypeddl.SelectedIndex = 1;
                    break;
                case "QT3":
                    this.QTypeddl.SelectedIndex = 2;
                    break;
                case "QT4":
                    this.QTypeddl.SelectedIndex = 3;
                    break;
                case"QT5":
                    this.QTypeddl.SelectedIndex = 4;
                    break;
                case "QT6":
                    this.QTypeddl.SelectedIndex = 5;
                    break;
            }
            if (QMustKeyIn =="Y")
            {
                this.QMustKeyIn.Checked = true;
            }
            else
            {
                this.QMustKeyIn.Checked = false;
            }
            if (!string.IsNullOrEmpty(QuestionTemplateItemName))
            {
                this.txtAnswer.Text = QuestionTemplateItemName;
            }
        }
    }
}