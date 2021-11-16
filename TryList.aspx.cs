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

namespace SuverySystem
{
    public partial class TryList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //檢查是否已登入
            var Logined = this.Session["Logined"];
            if (Logined != null)
            {
                this.LoginLink.Visible = false;
            }
            else
            {
                this.LoginedLink.Visible = false;
                this.btnLogout.Visible = false;
            }


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
                var dt = GetSuveryList();

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
                    string StatusString = CheckStatus(DateTime.Parse(dr["StartDate"].ToString()), DateTime.Parse(dr["EndDate"].ToString()));
                    model.Status = StatusString;
                    if(StatusString == "尚未開始" ||
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
                var dt = SearchSuvery(txtSreach, SDate, EDate);


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
                    string StatusString = CheckStatus(DateTime.Parse(dr["StartDate"].ToString()), DateTime.Parse(dr["EndDate"].ToString()));
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
        /// <summary>檢查搜尋列使用者輸入日期的值</summary>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <returns></returns>
        public static string DateCompare(DateTime SDate, DateTime EDate)
        {
            if (DateTime.Compare(SDate, EDate) < 0)
                return "true";
            else if (DateTime.Compare(SDate, EDate) == 0)
                return "Same Date";
            else
                return "false";
        }
        /// <summary>檢查DB讀取出的日期判別問卷的狀態</summary>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <returns></returns>
        public static string CheckStatus(DateTime SDate, DateTime EDate)
        {
            DateTime Today = DateTime.Today;
            if (DateTime.Compare(Today, SDate) < 0)
            {
                return "尚未開始";
            }
            else if (DateTime.Compare(Today, SDate) >= 0 &&
                DateTime.Compare(Today, EDate) <= 0)
            {
                return "開放中";
            }
            else
                return "關閉中";
        }
        /// <summary>搜尋按鈕事件</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            //chcek Date is OK or Not
            DateTime SDate = Convert.ToDateTime(txtSDate);
            DateTime EDate = Convert.ToDateTime(txtEDate);
            string DateResult = DateCompare(SDate, EDate);
            if (DateResult == "false")
            {
                Response.Write("<script>alert('日期格式有誤，請確認後再次輸入')</script>");
                return;
            }




            var dt = SearchSuvery(txtSreach, SDate, EDate);
            if (dt.Rows.Count > 0)
            {
                Response.Redirect($"TryList.aspx?Page=1&Search={txtSreach}&StartDate={txtSDate}&EndDate={txtEDate}");
            }
            else
            {
                Response.Write("<script>alert('查無資料!!')</script>");
            }

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

        /// <summary>找出所有問卷資料用於RP控制項</summary>
        /// <returns></returns>
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
        /// <summary>依照使用者輸入的標題、日期查找問卷資料</summary>
        /// <param name="txtInput"></param>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <returns></returns>
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

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            this.Session["Logined"] = null;
            Response.Redirect(Request.Url.ToString());
        }
    }
}