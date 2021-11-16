using DBSource;
using Method;
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
    public partial class TemplateQuestion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtItemName.Visible = false;
            var TemplateDT = BackgroundMethod. GetQuestionTemplateDT();
            List<QuestionTemplateModel> list = new List<QuestionTemplateModel>();
            for (int i = 0; i < TemplateDT.Rows.Count; i++)
            {
                QuestionTemplateModel model = new QuestionTemplateModel();
                var dr = TemplateDT.Rows[i];
                model.QuestionTemplateNo = (int)dr["QuestionTemplateNo"];
                model.QuestionTemplateName = dr["QuestionTemplateName"].ToString();
                var QType = dr["QuestionTemplateType"].ToString();
                switch (QType)
                {
                    case "QT1":
                        QType = "文字方塊-文字";
                        break;
                    case "QT2":
                        QType = "文字方塊-數字";
                        break;
                    case "QT3":
                        QType = "文字方塊-E-Mail";
                        break;
                    case "QT4":
                        QType = "文字方塊-日期";
                        break;
                    case "QT5":
                        QType = "單選方塊";
                        break;
                    case "QT6":
                        QType = "多選方塊";
                        break;
                }
                model.QuestionTemplateType = QType;
                model.QuestionTemplateMustKeyIn = dr["QuestionTemplateMustKeyIn"].ToString();
                list.Add(model);
            }
            if (!IsPostBack)
            {

            this.Repeater1.DataSource = list;
            this.Repeater1.DataBind();
            }
        }

        protected void dllQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var SelectedIndex = this.ddlQuestionType.SelectedValue;
            switch (SelectedIndex)
            {
                case "QT5":
                case "QT6":
                    this.txtItemName.Visible = true;
                    break;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            string txtQName = this.txtQuestionName.Text;
            string txtQType = this.ddlQuestionType.SelectedItem.Value;

            string txtMustKeyIn = string.Empty;
            if (this.MustKeyIn.Checked == true)
            {
                txtMustKeyIn = "Y";
            }
            else
            {
                txtMustKeyIn = "N";
            }
            string txtItemName = string.Empty;
            switch (txtQType)
            {

                case "QT5":
                case "QT6":
                    txtItemName = this.txtItemName.Text;
                    break;
            }
            if (!string.IsNullOrWhiteSpace(txtItemName))
            {
                BackgroundMethod. CreateQuestionTemplate(txtQName, txtQType, txtMustKeyIn, txtItemName);
            }
            else
            {
                BackgroundMethod.CreateQuestionTemplate(txtQName, txtQType, txtMustKeyIn);
            }
            Response.Redirect(Request.Url.ToString());
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var rowID = this.Request.Form["hfRowID"];
            Response.Write($"<script>alert('{rowID}')</script>");
        }
    }
}