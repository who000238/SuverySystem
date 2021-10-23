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
    public partial class TryForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);

            var SuveryMasterDataRow = GetSuveryMasterData(guid); //取得部分問卷資料
            var QuestionDetailDT = GetQuestionDetailAndItemDetail(guid); //取得問題資料
            if (SuveryMasterDataRow != null)
            {
                #region 辨別問卷狀態
                string SuveryStatus;
                if (SuveryMasterDataRow["Status"].ToString() == "N")
                    SuveryStatus = "關閉中";
                else
                    SuveryStatus = "開放中";
                #endregion
                #region 給予頁面上方資料
                this.ltlStatusAndDate.Text = $"{SuveryStatus}</br>{SuveryMasterDataRow["StartDate"]}~{SuveryMasterDataRow["EndDate"]}";
                this.h3Title.InnerText = SuveryMasterDataRow["Title"].ToString();
                this.ltlInnerText.Text = SuveryMasterDataRow["Summary"].ToString();
                #endregion
                //動態生成問題
                for (int i = 0; i < QuestionDetailDT.Rows.Count; i++)
                {
                    int QuestionNo = i + 1;

                    var QuestionDetailDR = QuestionDetailDT.Rows[i];
                    #region 問題標題區
                    Label lblTitle = new Label();
                    lblTitle.Text = "</br>" + QuestionDetailDR["DetailTitle"].ToString();
                    this.QuestionArea.Controls.Add(lblTitle);
                    #endregion
                    string QuestionType = QuestionDetailDR["DetailType"].ToString();
                    string MustKeyIn = QuestionDetailDR["DetailMustKeyin"].ToString();
                    ///SQL語法有使用LEFT JOIN 需要判別欄位空值的問題
                    #region 單多選問題選項數目

                    int ItemCount; //單多選項目總數
                    if (QuestionDetailDR["ItemCount"].ToString() == string.Empty)
                        ItemCount = 0;
                    else
                        ItemCount = (int)QuestionDetailDR["ItemCount"];
                    #endregion
                    ///依據QuestionType新增問題
                    switch (QuestionType)
                    {
                        #region 文字方塊
                        case "QT1":
                        case "QT2":
                        case "QT3":
                        case "QT4":
                            TextBox textBox = new TextBox();
                            textBox.ID = "Q" + QuestionNo.ToString();
                            switch (QuestionType)
                            {
                                case "QT1":
                                    textBox.TextMode = TextBoxMode.SingleLine;
                                    break;
                                case "QT2":
                                    textBox.TextMode = TextBoxMode.Number;
                                    break;
                                case "QT3":
                                    textBox.TextMode = TextBoxMode.Email;
                                    break;
                                case "QT4":
                                    textBox.TextMode = TextBoxMode.Date;
                                    break;

                            }
                            if (MustKeyIn == "Y")
                                textBox.CssClass = "Answer MustKeyIn";
                            else
                                textBox.CssClass = "Answer";
                            this.QuestionArea.Controls.Add(textBox);
                            break;
                        #endregion
                        #region 單多選方塊
                        case "QT5":
                            RadioButtonList radioButtonList = new RadioButtonList();
                            radioButtonList.ID = "Q" + QuestionNo.ToString();
                            if (MustKeyIn == "Y")
                                radioButtonList.CssClass = "Answer MustKeyIn";
                            else
                                radioButtonList.CssClass = "Answer";
                            for (int j = 0; j < ItemCount; j++)
                            {
                                string ColName = "Item" + (j + 1).ToString();
                                string ItemName = QuestionDetailDR[ColName].ToString();
                                ListItem item = new ListItem();
                                item.Attributes.Add("name", "Q" + QuestionNo.ToString());
                                item.Text = ItemName;
                                item.Value = ItemName;
                                radioButtonList.Items.Add(item);
                            }
                            this.QuestionArea.Controls.Add(radioButtonList);
                            break;
                        case "QT6":
                            CheckBoxList checkBoxList = new CheckBoxList();
                            checkBoxList.ID = "Q" + QuestionNo.ToString();
                            if (MustKeyIn == "Y")
                                checkBoxList.CssClass = "Answer MustKeyIn";
                            else
                                checkBoxList.CssClass = "Answer";
                            for (int j = 0; j < ItemCount; j++)
                            {
                                string ColName = "Item" + (j + 1).ToString();
                                string ItemName = QuestionDetailDR[ColName].ToString();
                                ListItem item = new ListItem();
                                item.Attributes.Add("name", "Q" + QuestionNo.ToString());
                                item.Text = ItemName;
                                item.Value = ItemName;
                                checkBoxList.Items.Add(item);
                            }
                            this.QuestionArea.Controls.Add(checkBoxList);

                            break;
                            #endregion
                    }
                }
                //提示共有幾個問題的Label
                Label lblQCount = new Label();
                lblQCount.Text = "共 " + QuestionDetailDT.Rows.Count.ToString() + " 個問題";
                this.QuestionArea.Controls.Add(lblQCount);
            }
        }



        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("TryList.aspx");

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);

            var QuestionDetailDT = GetQuestionDetailAndItemDetail(guid); //取得問題資料

        }
        #region Method區
        /// <summary>取得問卷基本資料</summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static DataRow GetSuveryMasterData(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @" SELECT  * FROM [SuverySystem].[dbo].[SuveryMaster]
                     WHERE SuveryID = @Guid
                    
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
        /// <summary>取的問卷內問題以及單多選項目資料</summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static DataTable GetQuestionDetailAndItemDetail(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @" SELECT  * FROM 
                            [SuverySystem].[dbo].[SuveryDetail]
					  LEFT JOIN 
                            [SuverySystem].[dbo].[ItemDetail] 
                        ON 
                        [SuverySystem].[dbo].[SuveryDetail].[DetailID] =  [SuverySystem].[dbo].[ItemDetail].[DetailID]
                     WHERE [SuverySystem].[dbo].[SuveryDetail].[SuveryID] = @Guid
                    ORDER BY [SuverySystem].[dbo].[SuveryDetail].[DetailID]
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            try
            {
                return DBHelper.ReadDataTable(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }

        #endregion
    }
}