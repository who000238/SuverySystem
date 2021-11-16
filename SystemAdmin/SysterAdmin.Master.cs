using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuverySystem.SystemAdmin
{
    public partial class SysterAdmin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //檢查是否已登入
            var Logined = this.Session["Logined"];
            if (Logined == null)
            {
                Response.Redirect("/TryList.aspx");
            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            this.Session["Logined"] = null;
            Response.Redirect("/TryList.aspx");
        }
    }
}