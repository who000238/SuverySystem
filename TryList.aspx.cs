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
    public partial class TryList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string txtSreach = Request.QueryString["Search"];
            string txtSDate = Request.QueryString["StartDate"];
            string txtEDate = Request.QueryString["EndDate"];
            DateTime SDate = Convert.ToDateTime(txtSDate);
            DateTime EDate = Convert.ToDateTime(txtEDate);
            //check QueryString is null or not
            if (string.IsNullOrEmpty(txtSreach) ||
                string.IsNullOrEmpty(txtSDate) ||
                string.IsNullOrEmpty(txtEDate))
            {
                DateTime today = DateTime.Today;
                //var dt = SuveryManager.GetSuveryList();
                var dt = GetSuveryList();
                // 測試日期字串轉換
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    var dr = dt.Rows[i];
                //    DateTime tempSD = DateTime.Parse(dr["StartDate"].ToString());
                //    DateTime tempED = DateTime.Parse(dr["EndDate"].ToString());
                //    string SD = tempSD.ToString("yyyy-MM-dd");
                //    string ED = tempED.ToString("yyyy-MM-dd");
                //    dr["StartDate"] = SD;
                //    dr["EndDate"] = ED;
                //}
                //
                this.Repeater1.DataSource = dt;
                this.Repeater1.DataBind();
            }
            else
            {
                var dt = SearchSuvery(txtSreach, SDate, EDate);
                this.Repeater1.DataSource = dt;
                this.Repeater1.DataBind();
            }

        }
        public static string CheckStatus(DateTime SDate, DateTime EDate)
        {
            DateTime Today = DateTime.Today;
            if (DateTime.Compare(Today,SDate)>0||
                DateTime.Compare(Today, SDate)==0)
            {
                return "開放中";
            }
            else if (DateTime.Compare(Today, EDate) < 0 ||
                 DateTime.Compare(Today, SDate) == 0)
            {
                return "開放中";

            }
            else
            return "關閉中";
        }
        protected void btnSreach_Click(object sender, EventArgs e)
        {
            string txtSreach = this.txtSuveryTitle.Text;
            string txtSDate = this.txtStartDate.Text;
            string txtEDate = this.txtEndDate.Text;
            //check input empty or not
            if (string.IsNullOrEmpty(txtSreach) ||
                string.IsNullOrEmpty(txtSDate) ||
                string.IsNullOrEmpty(txtEDate))
            {
                Response.Write("<script>alert('請確認所有欄位都有輸入值!!')</script>");
                return;
            }

            DateTime SDate = Convert.ToDateTime(txtSDate);
            DateTime EDate = Convert.ToDateTime(txtEDate);

            var dt = SearchSuvery(txtSreach, SDate, EDate);
            if (dt.Rows.Count > 0)
            {
                Response.Redirect($"TryList.aspx?Page=1&Search={txtSreach}&StartDate={txtSDate}&EndDate={txtEDate}");
            }
            else
            {
                Response.Write("<script>alert('查無資料!!')</script>");
            }
            //this.Repeater1.DataSource = dt;
            //this.Repeater1.DataBind();
        }




        public static DataTable GetSuveryList()
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" SELECT 
                      SuveryNo,
                      SuveryID,
                      Title,
                      StartDate,
                        EndDate,
                    Status
                    FROM [SuverySystem].[dbo].[SuveryMaster]
                    ORDER BY SuveryNo DESC
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            try
            {
                return DBHelper.ReadDataTable(connStr, dbCommand, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }

        public static DataTable SearchSuvery(string txtInput, DateTime SDate, DateTime EDate)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                 $@" SELECT *FROM  [SuverySystem].[dbo].[SuveryMaster]
                       WHERE Title Like @Title
                        AND StartDate >= @StartDate
                        AND EndDate <= @EndDate
                ";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Title", "%" + txtInput + "%"));
            list.Add(new SqlParameter("@StartDate", SDate));
            list.Add(new SqlParameter("@EndDate", EDate));

            try
            {
                return DBHelper.ReadDataTable(connStr, dbCommand, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }
    }
}