using DBSource;
using SuverySystem.Models;
using System;
using System.Collections.Generic;
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

            QuestionDetailModel model = new QuestionDetailModel()
            {
                SuveryID = guidString,
                DetailTitle = QuestionTitle,
                DetailType = QuestionType,
                DetailMustKeyin = QuestionIsMustKeyIn,
                ItemName = ItemName

            };

            HttpContext.Current.Session["QuestionDetail"] = model;
            //Response.Write($"<script>alert('{model.SuveryID}')</script>");
            //Response.Write($"<script>alert('{model.DetailTitle}')</script>");
            //Response.Write($"<script>alert('{model.DetailType}')</script>");
            //Response.Write($"<script>alert('{model.DetailMustKeyin}')</script>");
            //Response.Write($"<script>alert('{model.ItemName}')</script>");

        
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
        #endregion

    }
}