using DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuverySystem
{
    public partial class ConfirmPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["ansString"] == null)
            {

            }
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);

            string ansString = this.Session["ansString"].ToString();
            //Response.Write($"<script>alert('{ansString}')</script>");
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
                string NameOrderString = dr["NameOrder"].ToString();
                string[] NameOrderArray = NameOrderString.Split(',');
                string[] ansStringArray = ansString.Split(',');
                for (int i = 0; i < NameOrderArray.Length; i++)
                {
                    Label label = new Label();
                    label.Text = (i+1).ToString() + ".  " + NameOrderArray[i] + "</br>" + ansStringArray[i] + "</br></br>";
                    this.AnsArea.Controls.Add(label);
                }
            }
        }

        protected void btnModify_Click(object sender, EventArgs e)
        {
            string StringGuid = Request.QueryString["ID"];
            Response.Redirect($"Form.aspx?ID={StringGuid}");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);
            string AnsString = this.Session["ansString"].ToString();
            SaveSuveryAnswer(guid, AnsString);
            
        }

        public static void SaveSuveryAnswer(Guid guid, string AnswerString)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                                            @"
                                            INSERT INTO [dbo].[SuveryAnswer]
                                                       (
                                                        [Guid]
                                                       ,[Answer]
                                                        )
                                                         VALUES
                                                       (@Guid
                                                       ,@AnswerString
                                                        )
                                            ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            list.Add(new SqlParameter("@AnswerString", AnswerString));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
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
    }
}