﻿using DBSource;
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
            var TemplateDT = GetQuestionTemplateDT();
            List<QuestionTemplateModel> list = new List<QuestionTemplateModel>();

            for (int i = 0; i < TemplateDT.Rows.Count; i++)
            {
                QuestionTemplateModel model = new QuestionTemplateModel();
                var dr = TemplateDT.Rows[i];
                model.QuestionTemplateNo = (i + 1).ToString();
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
         
            this.Repeater1.DataSource = list;
            this.Repeater1.DataBind();
        }

        protected void dllQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var SelectedIndex = this.ddlQuestionType.SelectedValue;
            switch (SelectedIndex)
            {
                case "5":
                case "6":
                    this.txtItemName.Visible = true;
                    break;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            string txtQName = this.txtQuestionName.Text;
            string txtQType = this.ddlQuestionType.SelectedItem.Value;
            //switch (txtQType)
            //{
            //    case "QT1":
            //        txtQType = "文字方塊-文字";
            //        break;
            //    case "QT2":
            //        txtQType = "文字方塊-數字";
            //        break;
            //    case "QT3":
            //        txtQType = "文字方塊-E-Mail";
            //        break;
            //    case "QT4":
            //        txtQType = "文字方塊-日期";
            //        break;
            //    case "QT5":
            //        txtQType = "單選方塊";
            //        break;
            //    case "QT6":
            //        txtQType = "多選方塊";
            //        break;
            //}
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
                CreateQuestionTemplate(txtQName, txtQType, txtMustKeyIn, txtItemName);
            }
            else
            {
                CreateQuestionTemplate(txtQName, txtQType, txtMustKeyIn);
            }

        }
        public static void CreateQuestionTemplate(string QuestionTemplateName,
            string QuestionTemplateType, string QuestionTemplateMustKeyIn, string
            QuestionTemplateItemName)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    INSERT INTO [dbo].[QuestionTemplateDetail]
                               ([QuestionTemplateName]
                               ,[QuestionTemplateType]
                               ,[QuestionTemplateMustKeyIn]
                               ,[QuestionTemplateItemName])
                         VALUES
                               (@QuestionTemplateName
                               ,@QuestionTemplateType
                               ,@QuestionTemplateMustKeyIn
                               ,@QuestionTemplateItemName)
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@QuestionTemplateName", QuestionTemplateName));
            list.Add(new SqlParameter("@QuestionTemplateType", QuestionTemplateType));
            list.Add(new SqlParameter("@QuestionTemplateMustKeyIn", QuestionTemplateMustKeyIn));
            list.Add(new SqlParameter("@QuestionTemplateItemName", QuestionTemplateItemName));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        public static void CreateQuestionTemplate(string QuestionTemplateName,
           string QuestionTemplateType, string QuestionTemplateMustKeyIn
           )
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    INSERT INTO [dbo].[QuestionTemplateDetail]
                               ([QuestionTemplateName]
                               ,[QuestionTemplateType]
                               ,[QuestionTemplateMustKeyIn]
                               )
                         VALUES
                               (@QuestionTemplateName
                               ,@QuestionTemplateType
                               ,@QuestionTemplateMustKeyIn
                               )
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@QuestionTemplateName", QuestionTemplateName));
            list.Add(new SqlParameter("@QuestionTemplateType", QuestionTemplateType));
            list.Add(new SqlParameter("@QuestionTemplateMustKeyIn", QuestionTemplateMustKeyIn));

            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        public static DataTable GetQuestionTemplateDT()
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                      SELECT * FROM [dbo].[QuestionTemplateDetail]
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}