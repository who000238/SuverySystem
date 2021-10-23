using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuverySystem
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //CheckBoxList checkBoxList = new CheckBoxList();
            //checkBoxList.ID = "CBL";
            //string ItemName = "選項";
            //for (int i = 0; i < 10; i++)
            //{
            //    checkBoxList.Items.Add(ItemName + i.ToString());
            //}
            //this.TestArea.Controls.Add(checkBoxList);
            Panel panel = new Panel();
            panel.ID = "container";
            Label label = new Label();
            label.Text = "文字";
            panel.Controls.Add(label);
            this.TestArea.Controls.Add(panel);
        }

      
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            this.lbl.Text += Request.Form.GetValues("CBL");
        }
    }
}