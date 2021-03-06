using DBSource;
using Method;
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
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);

            var SuveryMasterDataRow = ForegroundMethod.GetSuveryMasterData(guid); //取得部分問卷資料
            var QuestionDetailDT = ForegroundMethod.GetQuestionDetailAndItemDetail(guid); //取得問題資料

            //build new model and check status
            DateTime today = DateTime.Now;
            DateTime SDate = DateTime.Parse(SuveryMasterDataRow["StartDate"].ToString());
            DateTime EDate = DateTime.Parse(SuveryMasterDataRow["EndDate"].ToString());
            if (DateTime.Compare(today, SDate) < 0)
            {
                //is mean today is early StartDate update status and redirect to list page
                ForegroundMethod.UpdateSuveryStatus(guid, "N"); //true Status letter is "W" is mean "wait" but now use "N" to represent  
                Response.Write("<script type='text/javascript'> alert('發生一些錯誤 即將跳轉至列表頁');location.href = 'TryList.aspx';</script>");
            }
            else if (DateTime.Compare(today, EDate) > 0)
            {
                //is mean today is later EndDate update status and redirect to list page
                ForegroundMethod.UpdateSuveryStatus(guid, "N");
                Response.Write("<script type='text/javascript'> alert('發生一些錯誤 即將跳轉至列表頁');location.href = 'TryList.aspx';</script>");
            }

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
                    string QuestionType = QuestionDetailDR["DetailType"].ToString();
                    string MustKeyIn = QuestionDetailDR["DetailMustKeyin"].ToString();
                    #region 問題標題區
                    Label lblTitle = new Label();
                    lblTitle.Text = "</br>" + QuestionDetailDR["DetailTitle"].ToString();
                    if (MustKeyIn == "Y")
                    {
                        lblTitle.Text += "     (必填問題)" + "</br>";
                    }
                    this.QuestionArea.Controls.Add(lblTitle);
                    #endregion
                    ///SQL語法有使用LEFT JOIN 需要判別欄位空值的問題
                    #region 單多選問題選項數目
                    int ItemCount; //單多選項目總數
                    if (QuestionDetailDR["ItemCount"].ToString() == string.Empty)
                        ItemCount = 0;
                    else
                        ItemCount = (int)QuestionDetailDR["ItemCount"];
                    #endregion
                    //依據QuestionType新增問題
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
                            {
                                textBox.Attributes["required"] = "required";
                                textBox.Attributes["aria-required"] = "true";
                            }
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
                            {
                                RequiredFieldValidator requiredFieldValidator = new RequiredFieldValidator(); //驗證RadioButtonList必填控制項的驗證控制項
                                requiredFieldValidator.ControlToValidate = radioButtonList.ID.ToString();
                                requiredFieldValidator.ErrorMessage = "請確認所有必填項目都有輸入值&選取值";
                                requiredFieldValidator.CssClass = "ErrorMSG";
                                this.QuestionArea.Controls.Add(requiredFieldValidator);
                             
                            }
                  
                            for (int j = 0; j < ItemCount; j++)
                            {
                                string ColName = "Item" + (j + 1).ToString();
                                string ItemName = QuestionDetailDR[ColName].ToString();
                                ListItem item = new ListItem();
                                item.Attributes.Add("name", "Q" + QuestionNo.ToString());
                                item.Text = ItemName;
                                item.Value = ItemName;
                                if (MustKeyIn == "Y")
                                {
                                    item.Attributes.Add("class", "MustKeyInRB");
                                }
                                radioButtonList.Items.Add(item);
                            }
                            this.QuestionArea.Controls.Add(radioButtonList);
                            break;
                        case "QT6":
                            CheckBoxList checkBoxList = new CheckBoxList();
                            checkBoxList.ID = "Q" + QuestionNo.ToString();
                            checkBoxList.Attributes.Add("runat", "server");
                            for (int j = 0; j < ItemCount; j++)
                            {
                                string ColName = "Item" + (j + 1).ToString();
                                string ItemName = QuestionDetailDR[ColName].ToString();
                                ListItem item = new ListItem();
                                item.Attributes.Add("name", "Q" + QuestionNo.ToString());

                                item.Text = ItemName;
                                item.Value = ItemName;
                                if (MustKeyIn == "Y")
                                {
                                    item.Attributes.Add("class", "MustKeyInCBL");
                                }
                                checkBoxList.Items.Add(item);
                            }
                            this.QuestionArea.Controls.Add(checkBoxList);
                            break;
                            #endregion
                    }
                }
                //提示共有幾個問題的Label
                Label lblQCount = new Label();
                lblQCount.Text = "</br></br>共 " + QuestionDetailDT.Rows.Count.ToString() + " 個問題";
                this.QuestionArea.Controls.Add(lblQCount);
            }
        }


        
        /// <summary>put data into session and jump to confirm page</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string StringGuid = Request.QueryString["ID"]; //取得問卷ID
            Guid guid = Guid.Parse(StringGuid);
            // 取得基本必填欄位的值
            string[] UserInfo = new string[]
            {
                this.UserName.Text,
                this.UserPhone.Text,
                this.UserMail.Text,
                this.UserAge.Text
            };
            var UserInfoString = string.Join(",", UserInfo);
            this.Session["UserInfo"] = UserInfoString;
            //
            var QuestionDetailDT = ForegroundMethod.GetQuestionDetailAndItemDetail(guid); //取得問卷問題資料
            int QuestionCount = QuestionDetailDT.Rows.Count;                    //問卷共有幾個問題
            string[] AnswerArray = new string[QuestionCount];                     //依照共有幾個問題建構出一個陣列來放回傳值
            string tempAnswer = string.Empty;                                                 //儲存多選問題時使用者選擇的項目字串
            List<string> TempAnswerList;
            for (int i = 0; i < QuestionCount; i++)
            {
                var QuestionDetailDR = QuestionDetailDT.Rows[i];
                string MustKyeIn = QuestionDetailDR["DetailMustKeyin"].ToString();
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
                #region 取值版本2 目前單多選必填驗證測試實作中
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
                //            var inpList = HttpContext.Current.Request.Form.GetValues($"{ControlName}");
                //            string[] list = inpList ?? new string[] { };
                //            //if(list.Length != 0)
                //            //if (list != null)
                //            //{
                //            //tempAnswer = tempAnswer + list[0] + " ";
                //            tempAnswer = tempAnswer + string.Join("&", list);
                //            //tempAnswer = string.Join("&", list);
                //            //}
                //        }
                //        AnswerArray[i] = tempAnswer;
                //        break;
                //}
                #endregion
                switch (QuestionType)
                {
                    case "QT1":
                    case "QT2":
                    case "QT3":
                    case "QT4":
                        ControlName = "Q" + (i + 1).ToString();
                        TempAnswerList = Request.Form.GetValues($"{ControlName}").ToList();
                        if (string.IsNullOrEmpty(TempAnswerList[0]))
                        {
                            AnswerArray[i] = "非必填問題，使用者未填寫";
                        }
                        else
                        {
                        AnswerArray[i] = TempAnswerList[0];
                        }
                        break;
                    case "QT5":
                        ControlName = "Q" + (i + 1).ToString();
                        var inpList1 = HttpContext.Current.Request.Form.GetValues($"{ControlName}");
                        string[] list1 = inpList1 ?? new string[] { };
                        if (list1.Length == 0)
                        {
                            AnswerArray[i] = "非必填問題，使用者未填寫";
                        }
                        else
                        {
                            AnswerArray[i] = list1[0];
                        }
                        break;
                    case "QT6":
                        int ItemCount = (int)QuestionDetailDR["ItemCount"];

                        for (int j = 0; j < ItemCount; j++)
                        {
                            ControlName = "Q" + (i + 1).ToString() + "$" + j;
                            var inpList = HttpContext.Current.Request.Form.GetValues($"{ControlName}");
                            string[] list = inpList ?? new string[] { };

                            //tempAnswer = tempAnswer + string.Join("&", list);
                            tempAnswer = tempAnswer+ " "+ string.Join(" ", list);
                        }
                        if (MustKyeIn=="Y" && string.IsNullOrWhiteSpace(tempAnswer))
                        {
                            Response.Write("<script>alert('還有必填項目沒勾選')</script>");
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(tempAnswer))
                        {
                            AnswerArray[i] = "非必填問題，使用者未填寫";
                        }
                        else 
                        { 
                            AnswerArray[i] = tempAnswer;
                            tempAnswer = string.Empty;                        
                        }
                        break;
                }
            }

            string ansString = string.Join(",", AnswerArray);
            this.Session["ansString"] = ansString;

            Response.Redirect($"TryConfirmPage.aspx?ID={StringGuid}");
            Response.Write($"<script>alert('{ansString}')</script>");

        }
        #region Method區

        #endregion
    }
}