using System;
using System.Collections.Generic;
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
        }


        protected void btnCancle_Click(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

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
            //string boolString = this.hfBool.Value.ToString();
            //if (boolString=="true")
            //{
            //    Response.Write($"<script>alert('成功')</script>");
            //}
            //else
            //    Response.Write($"<script>alert('Nope')</script>");
            string txtQName = this.txtQuestionName.Text;
            string txtQType = this.ddlQuestionType.SelectedItem.Value;
            switch (txtQType)
            {
                case "QT1":
                    txtQType = "文字方塊-文字";
                    break;
                case "QT2":
                    txtQType = "文字方塊-數字";
                    break;
                case "QT3":
                    txtQType = "文字方塊-E-Mail";
                    break;
                case "QT4":
                    txtQType = "文字方塊-日期";
                    break;
                case "QT5":
                    txtQType = "單選方塊";
                    break;
                case "QT6":
                    txtQType = "多選方塊";
                    break;
            }
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
       
                case "單選方塊":
                case "多選方塊":
                    txtItemName = this.txtItemName.Text;
                    break;
            }

        }
    }
}