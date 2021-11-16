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
    public partial class List1 : System.Web.UI.Page
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
                var dt = BackgroundMethod.GetSuveryList();
                List<ListModel> list = new List<ListModel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    ListModel model = new ListModel();
                    model.SuveryID = dr["SuveryID"].ToString();
                    model.No = (int)dr["SuveryNo"];
                    model.Title = dr["Title"].ToString();
                    model.StartDate = DateTime.Parse(dr["StartDate"].ToString());
                    model.EndDate = DateTime.Parse(dr["EndDate"].ToString());
                    string StatusString = BackgroundMethod.CheckStatus(DateTime.Parse(dr["StartDate"].ToString()), DateTime.Parse(dr["EndDate"].ToString()));
                    model.Status = StatusString;
                    if (StatusString == "尚未開始" ||
                        StatusString == "關閉中")
                    {
                        model.ClassName = "disabled";
                    }
                    else
                    {
                        model.ClassName = "class";
                    }
                    list.Add(model);
                }
                int pagesize = this.ucPager.PageSize;
                var pageList = this.GetPagedList(list, pagesize);

                this.Repeater1.DataSource = pageList;
                this.Repeater1.DataBind();


                this.ucPager.TotalSize = list.Count;
                this.ucPager.Bind();
            }
            else
            {
                var dt = BackgroundMethod.SearchSuvery(txtSreach, SDate, EDate);

                List<ListModel> list = new List<ListModel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    ListModel model = new ListModel();
                    model.SuveryID = dr["SuveryID"].ToString();
                    model.No = (int)dr["SuveryNo"];
                    model.Title = dr["Title"].ToString();
                    model.StartDate = DateTime.Parse(dr["StartDate"].ToString());
                    model.EndDate = DateTime.Parse(dr["EndDate"].ToString());
                    string StatusString = BackgroundMethod.CheckStatus(DateTime.Parse(dr["StartDate"].ToString()), DateTime.Parse(dr["EndDate"].ToString()));
                    model.Status = StatusString;
                    list.Add(model);
                }

                int pagesize = this.ucPager.PageSize;
                var pageList = this.GetPagedList(list, pagesize);

                this.Repeater1.DataSource = pageList;
                this.Repeater1.DataBind();


                this.ucPager.TotalSize = list.Count;
                this.ucPager.txtSearch = txtSreach;
                this.ucPager.StartDate = txtSDate;
                this.ucPager.EndDate = txtEDate;
                this.ucPager.BindWithSerach();
            }
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
            var dt = BackgroundMethod.SearchSuvery(txtSreach, SDate, EDate);
            if (dt.Rows.Count > 0)
            {
                Response.Redirect($"AdminList.aspx?Page=1&Search={txtSreach}&StartDate={txtSDate}&EndDate={txtEDate}");
            }
            else
            {
                Response.Write("<script>alert('查無資料!!')</script>");
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string guid = Guid.NewGuid().ToString();
            Response.Redirect($"AdminDetail.aspx?ID={guid}");
        }

        #region 分頁控制項用
        private int GetCurrentPage()
        {
            string pageText = Request.QueryString["Page"];

            if (string.IsNullOrWhiteSpace(pageText))
                return 1;

            int intPage;
            if (!int.TryParse(pageText, out intPage))
                return 1;

            if (intPage <= 0)
                return 1;

            return intPage;
        }

        private List<ListModel> GetPagedList(List<ListModel> list, int pagesize)
        {
            int startIndex = (this.GetCurrentPage() - 1) * pagesize;
            return list.Skip(startIndex).Take(pagesize).ToList();
        }
        #endregion
        
    }
}