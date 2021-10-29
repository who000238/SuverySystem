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
                            checkBoxList.Attributes.Add("runat", "server");
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
            string StringGuid = Request.QueryString["ID"]; //取得問卷ID
            Guid guid = Guid.Parse(StringGuid);

            var QuestionDetailDT = GetQuestionDetailAndItemDetail(guid); //取得問卷問題資料
            int QuestionCount = QuestionDetailDT.Rows.Count;                    //問卷共有幾個問題
            string[] AnswerArray = new string[QuestionCount];                     //依照共有幾個問題建構出一個陣列來放回傳值
            string tempAnswer = string.Empty;                                                 //儲存多選問題時使用者選擇的項目字串
            List<string> TempAnswerList;
            for (int i = 0; i < QuestionCount; i++)
            {
                var QuestionDetailDR = QuestionDetailDT.Rows[i];
                string QuestionType = QuestionDetailDR["DetailType"].ToString();
                string ControlName;
                #region 取值版本1 //多選還是取不到
                //switch (QuestionType)
                //{
                //    case "QT1":
                //    case "QT2":
                //    case "QT3":
                //    case "QT4":
                //    case "QT5":
                //        ControlName = "Q" + (i + 1).ToString();
                //        TempAnswerList = Request.Form.GetValues($"{ControlName}").ToList();
                //        AnswerArray[i] = TempAnswerList[0];
                //        break;
                //    case "QT6":
                //        int ItemCount = (int)QuestionDetailDR["ItemCount"];
                //        for (int j = 0; j < ItemCount; j++)
                //        {
                //            ControlName = "Q" + (i + 1).ToString() + "$" + j;
                //            TempAnswerList = Request.Form.GetValues($"{ControlName}").ToList();
                //            AnswerArray[i] = TempAnswerList[0];
                //        }
                //        break;
                //}
                #endregion
                switch (QuestionType)
                {
                    case "QT1":
                    case "QT2":
                    case "QT3":
                    case "QT4":
                    case "QT5":
                        ControlName = "Q" + (i + 1).ToString();
                        TempAnswerList = Request.Form.GetValues($"{ControlName}").ToList();
                        AnswerArray[i] = TempAnswerList[0];
                        break;
                    case "QT6":
                        int ItemCount = (int)QuestionDetailDR["ItemCount"];
                        for (int j = 0; j < ItemCount; j++)
                        {
                            ControlName = "Q" + (i + 1).ToString() + "$" + j;
                            var inpList = HttpContext.Current.Request.Form.GetValues($"{ControlName}");
                            string[] list = inpList ?? new string[] { };
                            //if(list.Length != 0)
                            //if (list != null)
                            //{
                            //tempAnswer = tempAnswer + list[0] + " ";
                            tempAnswer = tempAnswer + string.Join("&", list);
                            //tempAnswer = string.Join("&", list);
                            //}
                        }
                        AnswerArray[i] = tempAnswer;
                        break;
                }
            }

            string ansString = string.Join(",", AnswerArray);
            this.Session["ansString"] = ansString;

            Response.Redirect($"TryConfirmPage.aspx?ID={StringGuid}");
            Response.Write($"<script>alert('{ansString}')</script>");

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