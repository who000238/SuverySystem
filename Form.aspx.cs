using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBSource;
using System.Data.SqlClient;

namespace SuverySystem
{
    public partial class Form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);

            var dr = GetSuveryData(guid);

            string SuveryStatus;
            int intStatus = Convert.ToInt32(dr["Status"]);
            if (intStatus == 0)
                SuveryStatus = "關閉中";
            else
                SuveryStatus = "開放中";
            if (dr != null)
            {
                this.ltlStatusAndDate.Text = $"{SuveryStatus}</br>{dr["StartDate"]}~{dr["EndDate"]}";
                this.h3Title.InnerText = dr["Title"].ToString();
                this.ltlInnerText.Text = dr["InnerText"].ToString();
                string TypeOrderString = dr["TypeOrder"].ToString();
                string NameOrderString = dr["NameOrder"].ToString();
                string isRequiredString = dr["isRequired"].ToString();
                string[] TypeOrderArray = TypeOrderString.Split(',');
                string[] NameOrderArray = NameOrderString.Split(',');
                string[] isRequiredArray = isRequiredString.Split(',');

                for (int i = 0; i < TypeOrderArray.Length; i++)
                {
                    int QuestionOrder = i + 1;
                    bool isRequired = isRequiredArray[i] == "1" ? true : false;
                    Label lblForbr = new Label();
                    lblForbr.Text = "</br>";
                    Label label = new Label();
                    label.Text = NameOrderArray[i];
                    if (isRequired)
                        label.Text += "(必填)";
                    this.QuestionArea.Controls.Add(label);
                    switch (TypeOrderArray[i])
                    {

                        case "1":
                            TextBox textBox = new TextBox();
                            textBox.ID = "Q" + QuestionOrder.ToString();
                            textBox.TextMode = TextBoxMode.SingleLine;
                            if (isRequired)
                            {
                                textBox.CssClass = "isRequired";
                            }
                            this.QuestionArea.Controls.Add(textBox);
                            this.QuestionArea.Controls.Add(lblForbr);
                            break;
                        case "2":
                            TextBox textBox2 = new TextBox();
                            textBox2.ID = "Q" + QuestionOrder.ToString();
                            textBox2.TextMode = TextBoxMode.Number;
                            if (isRequired)
                            {
                                textBox2.CssClass = "isRequired";
                            }
                            this.QuestionArea.Controls.Add(textBox2);
                            this.QuestionArea.Controls.Add(lblForbr);
                            break;
                        case "3":
                            TextBox textBox3 = new TextBox();
                            textBox3.ID = "Q" + QuestionOrder.ToString();
                            textBox3.TextMode = TextBoxMode.Email;
                            if (isRequired)
                            {
                                textBox3.CssClass = "isRequired";
                            }
                            this.QuestionArea.Controls.Add(textBox3);
                            this.QuestionArea.Controls.Add(lblForbr);
                            break;
                        case "4":
                            TextBox textBox4 = new TextBox();
                            textBox4.ID = "Q" + QuestionOrder.ToString();
                            textBox4.TextMode = TextBoxMode.Date;
                            if (isRequired)
                            {
                                textBox4.CssClass = "isRequired";
                            }
                            this.QuestionArea.Controls.Add(textBox4);
                            this.QuestionArea.Controls.Add(lblForbr);
                            break;
                        case "5":
                        case "6":
                            CheckBox checkBox = new CheckBox();
                            checkBox.ID = "Q" + QuestionOrder.ToString();
                            if (isRequired)
                            {
                                checkBox.CssClass = "isRequired";
                            }
                            this.QuestionArea.Controls.Add(checkBox);
                            this.QuestionArea.Controls.Add(lblForbr);
                            break;
                    }
                }
            }
        }


        public static DataRow GetSuveryData(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @" SELECT TOP(1) * FROM SuveryData
                     WHERE Guid = @Guid
                     ORDER BY [SuverySystem].[dbo].[SuveryData].[No]
                    
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
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


        #region 待刪除
        //public static void CreateQuestionControl(string QuestionType, string QuestionName, bool isRequired, int QuestionOrder)
        //{
        //    switch (QuestionType)
        //    {
        //        case "1":
        //            CreateTitleLabel(QuestionName, isRequired);
        //            TextBox textBox = new TextBox();
        //            textBox.ID = "Q" + QuestionOrder.ToString();
        //            textBox.TextMode = TextBoxMode.SingleLine;
        //            if (isRequired)
        //            {
        //                textBox.CssClass = "isRequired";
        //            }
        //            break;
        //        case "2":
        //            CreateTitleLabel(QuestionName, isRequired);
        //            TextBox textBox2 = new TextBox();
        //            textBox2.ID = "Q" + QuestionOrder.ToString();
        //            textBox2.TextMode = TextBoxMode.Number;
        //            if (isRequired)
        //            {
        //                textBox2.CssClass = "isRequired";
        //            }
        //            break;
        //        case "3":
        //            CreateTitleLabel(QuestionName, isRequired);
        //            TextBox textBox3 = new TextBox();
        //            textBox3.ID = "Q" + QuestionOrder.ToString();
        //            textBox3.TextMode = TextBoxMode.Email;
        //            if (isRequired)
        //            {
        //                textBox3.CssClass = "isRequired";
        //            }
        //            break;
        //        case "4":
        //            CreateTitleLabel(QuestionName, isRequired);
        //            TextBox textBox4 = new TextBox();
        //            textBox4.ID = "Q" + QuestionOrder.ToString();
        //            textBox4.TextMode = TextBoxMode.Date;
        //            if (isRequired)
        //            {
        //                textBox4.CssClass = "isRequired";
        //            }
        //            break;
        //        case "5":
        //            CreateTitleLabel(QuestionName, isRequired);
        //            CheckBox checkBox = new CheckBox();
        //            checkBox.ID = "Q" + QuestionOrder.ToString();
        //            if (isRequired)
        //            {
        //                checkBox.CssClass = "isRequired";
        //            }
        //            break;
        //    }
        //}

        //public static void CreateTitleLabel(string QuestionName, bool isRequired)
        //{
        //    Label label = new Label();
        //    label.Text = QuestionName;
        //    if (isRequired)
        //        label.Text += " (必填)";

        //}
        //public static string GetSuveryName(Guid guid)
        //{
        //    string connectionString = DBHelper.GetConnectionString();
        //    string dbCommandString =
        //         @" SELECT [No] FROM SuveryList
        //             WHERE Guid = @Guid

        //        ";
        //    List<SqlParameter> list = new List<SqlParameter>();
        //    list.Add(new SqlParameter("@Guid", guid));
        //    try
        //    {
        //        var dr = DBHelper.ReadDataRow(connectionString, dbCommandString, list);
        //        string SuveryName = "SuveryNo" + dr["No"].ToString();
        //        return SuveryName;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog(ex);
        //        return null;
        //    }
        //}

        //public static DataRow GetSuveryData(string SuveryName)
        //{
        //    string connectionString = DBHelper.GetConnectionString();
        //    string dbCommandString =
        //       $@" SELECT * FROM {SuveryName}
        //        ";
        //    List<SqlParameter> list = new List<SqlParameter>();
        //    list.Add(new SqlParameter("@TableName", SuveryName));

        //    try
        //    {
        //        return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog(ex);
        //        return null;
        //    }
        //}

        #endregion

        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("List.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);
            var dr = GetSuveryData(guid);
            string TypeOrderString = dr["TypeOrder"].ToString();
            string[] TypeOrderArray = TypeOrderString.Split(',');
            int QuestionCount = TypeOrderArray.Length;
            string[] AnswerArray = new string[QuestionCount];
            for (int i = 1; i <= QuestionCount; i++)
            {
                string ControlName = "Q" + i.ToString();
                List<string> tempAnswerList = Request.Form.GetValues($"{ControlName}").ToList();
                if (tempAnswerList == null )
                {
                    tempAnswerList[i] = "0";
                    AnswerArray[i - 1] = tempAnswerList[i];
                    break;
                }
                AnswerArray[i - 1] = tempAnswerList[0];
            }

            string ansString = string.Join(",", AnswerArray);
            Response.Write($"<script>alert('{ansString}')</script>");
            
        }
    }
}