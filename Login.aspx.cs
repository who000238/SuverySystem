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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //檢查是否已登入
            var Logined = this.Session["Logined"];
            if (Logined != null)
            {
                Response.Redirect("/SystemAdmin/AdminList.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string inpAcc = this.txtAcc.Text;
            string inpPwd = this.txtPwd.Text;

            if (string.IsNullOrWhiteSpace(inpAcc)||string.IsNullOrWhiteSpace(inpPwd))
            {
                Response.Write("<script>alert('帳號、密碼為必填')</script>");
                return;
            }

            var dr = TryLogin(inpAcc, inpPwd);

            if (dr != null)
            {
                this.Session["Logined"] = "true";
                Response.Redirect("/SystemAdmin/AdminList.aspx");
            }
            else
            {
                Response.Write("<script>alert('帳號、密碼有誤')</script>");
                return;
            }
        }

        protected void btnCreateAcc_Click(object sender, EventArgs e)
        {

        }
        public static DataRow TryLogin(string Acc, string pwd)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                 $@" SELECT *FROM  [SuverySystem].[dbo].[AdminTable]
                       WHERE [Account] = @Admin 
                       And [Password] = @Password
                     
                ";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Admin", Acc));
            list.Add(new SqlParameter("@Password", pwd));

            try
            {
                return DBHelper.ReadDataRow(connStr, dbCommand, list);
            
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }
    }
}