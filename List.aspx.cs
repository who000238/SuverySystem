using DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuverySystem
{
    public partial class List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            var dt = SuveryManager.GetSuveryList();

            //if (dt.Rows.Count > 0) // check is empty data
            //{
            //    this.gvSuveryList.DataSource = dt;
            //    this.gvSuveryList.DataBind();
            //}

            this.Repeater1.DataSource = dt;
            this.Repeater1.DataBind();
        }

        protected void btnSreach_Click(object sender, EventArgs e)
        {
            string txtSreach = this.txtSuveryTitle.Text;
            string txtSDate = this.txtStartDate.Text;
            string txtEDate = this.txtEndDate.Text;

            DateTime SDate = Convert.ToDateTime(txtSDate);
            DateTime EDate = Convert.ToDateTime(txtEDate);
        }

        public static int CheckDate(DateTime today, DateTime StartDate, DateTime EndDate)
        {
            if (StartDate <= today && today <= EndDate)
                return 1;
            else
                //if (today <= StartDate || EndDate <= today)
                return 0;
        }

        //protected void gvSuveryList_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    var row = e.Row;
        //    if (row.RowType == DataControlRowType.DataRow)
        //    {
        //        Label lbl = row.FindControl("lblStatus") as Label;
        //        var dr = row.DataItem as DataRowView;
        //        int StatusType = dr.Row.Field<int>("Status");

        //        if (StatusType == 0)
        //        {
        //            lbl.Text = "關閉中";
        //        }
        //        else
        //        {
        //            lbl.Text = "開放中";
        //        }
        //    }
        //}

        //protected void gvSuveryList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    var dt = SuveryManager.GetSuveryList();

        //    //if (dt.Rows.Count > 0) // check is empty data
        //    //{
        //    gvSuveryList.PageIndex = e.NewPageIndex;
        //    this.gvSuveryList.DataSource = dt;
        //    this.gvSuveryList.DataBind();
        //    //}
        //}
    }
}