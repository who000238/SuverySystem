using DBSource;
using Method;
using SuverySystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SuverySystem.SystemAdmin
{
    public partial class Detail1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtAnswer.Enabled = false;
            string SuveryID = Request.QueryString["ID"].ToString();
            Guid guid = Guid.Parse(SuveryID);
            this.hfSuveryID.Value = SuveryID;
            //檢查guid是否已有問卷資料or問題資料
            var CheckSuveryDataExistDR = BackgroundMethod.GetSuveryMaster(guid);
            if (CheckSuveryDataExistDR != null)//表示DB內已經有這筆問卷的細節 則新增按鈕關閉顯示改為顯示修改按鈕
            {
                this.btnSubmit.Visible = false;
                this.btnUpdate.Visible = true;
            }
            //檢查此ID是否已有填答資料
            var AnsDataExistOrNotDT = BackgroundMethod.CheckAnsExistOrNot(guid);
            if (AnsDataExistOrNotDT.Rows.Count > 0)
            {
                this.hfAnswerExistOrNot.Value = "Exist";
            }
            // 常用問題下拉式選單DataSource
            var QuestionTemplateDT = BackgroundMethod.GetQuestionTemplate();
            // 觀看問卷填寫細節頁面Repeater DataSource
            var UserInfoDT = BackgroundMethod.GetUserInfoForSeeDetail(guid);
            if (!IsPostBack)
            {
                //系結常用問題下拉式選單
                for (int i = 0; i < QuestionTemplateDT.Rows.Count; i++)
                {
                    var QuestionTemplateDR = QuestionTemplateDT.Rows[i];
                    this.TemplateQddl.Items.Add(QuestionTemplateDR["QuestionTemplateName"].ToString());
                }
                List<UserInfoModel> list = new List<UserInfoModel>();
                for (int i = 0; i < UserInfoDT.Rows.Count; i++)
                {
                    UserInfoModel userInfoModel = new UserInfoModel();
                    var dr = UserInfoDT.Rows[i];
                    userInfoModel.No = UserInfoDT.Rows.Count - i;
                    userInfoModel.SuveryID = dr["SuveryID"].ToString();
                    string UserInfoString = dr["UserInfo"].ToString();
                    userInfoModel.UserInfoString = UserInfoString;
                    var UserInfoArray = UserInfoString.Split(',');
                    userInfoModel.UserInfoName = UserInfoArray[0];
                    DateTime createDate = (DateTime)dr["CreateTime"];
                    userInfoModel.CreateTimeString = createDate.ToString("yyyy-MM-dd hh:mm");

                    list.Add(userInfoModel);
                }
                int pagesize = this.ucPagerForDetail.PageSize;
                var pagedList = this.GetPagedDetailList(list, pagesize);

                this.Repeater2.DataSource = pagedList;
                this.Repeater2.DataBind();

                this.ucPagerForDetail.SuveruID = SuveryID;
                this.ucPagerForDetail.TotalSize = list.Count;
                this.ucPagerForDetail.Bind();
            }
            if (!IsPostBack)
            {
                #region 確認資料庫是否有內已存放之問卷內容、若有 輸出到頁面上
                var SuveryMasterDR = BackgroundMethod.GetSuveryMaster(guid);
                if (SuveryMasterDR != null)
                {
                    SuveryMasterModel model = new SuveryMasterModel()
                    {
                        SuveryNo = Guid.Parse(SuveryMasterDR["SuveryID"].ToString()),
                        Title = SuveryMasterDR["Title"].ToString(),
                        StartDate = DateTime.Parse(SuveryMasterDR["StartDate"].ToString()),
                        EndDate = DateTime.Parse(SuveryMasterDR["EndDate"].ToString()),
                        Status = SuveryMasterDR["Status"].ToString(),
                        Summary = SuveryMasterDR["Summary"].ToString()
                    };
                    //HttpContext.Current.Session["SuveryMaster"] = model;
                    this.txtSuveryTitle.Text = model.Title;
                    this.txtSummary.Text = model.Summary;
                    this.txtStartDate.Text = model.StartDate.ToString("yyyy-MM-dd");
                    this.txtEndDate.Text = model.EndDate.ToString("yyyy-MM-dd");
                }

                #endregion
                #region 確認資料庫內是否有已存放之問題內容、若有 加入到List內 放至Session中
                //確認session["QuestionDetail"]中有沒有資料 若有 就代表已在其他地方執行過寫入 則不在此處讀取資料庫並寫入
                var DetailList = HttpContext.Current.Session["QuestionDetail"];
                //
                if (DetailList == null)
                {
                    var QuestionDetailDT = BackgroundMethod.GetQuestionDetail(guid);
                    if (QuestionDetailDT != null)
                    {
                        List<QuestionDetailModel> list = new List<QuestionDetailModel>();
                        for (int i = 0; i < QuestionDetailDT.Rows.Count; i++)
                        {
                            var QuestionDetailDR = QuestionDetailDT.Rows[i];

                            string ItemNames = string.Empty;
                            int ItemCount;
                            if (QuestionDetailDR["ItemCount"].ToString() == string.Empty)
                                ItemCount = 0;
                            else
                                ItemCount = (int)QuestionDetailDR["ItemCount"];
                            if (ItemCount != 0)
                            {
                                for (int j = 0; j < ItemCount; j++)
                                {
                                    ItemNames += QuestionDetailDR[$"Item{j + 1}"].ToString();
                                }
                            }

                            string DetailType = QuestionDetailDR["DetailType"].ToString();
                            switch (DetailType)
                            {
                                case "QT1":
                                    DetailType = "文字方塊(文字)";
                                    break;
                                case "QT2":
                                    DetailType = "文字方塊(數字)";
                                    break;
                                case "QT3":
                                    DetailType = "文字方塊(E-Mail)";
                                    break;
                                case "QT4":
                                    DetailType = "文字方塊(日期)";
                                    break;
                                case "QT5":
                                    DetailType = "單選方塊";
                                    break;
                                case "QT6":
                                    DetailType = "多選方塊";
                                    break;
                            }


                            QuestionDetailModel model = new QuestionDetailModel()
                            {
                                QuestionNo = i + 1,
                                SuveryID = QuestionDetailDR["SuveryID"].ToString(),
                                DetailTitle = QuestionDetailDR["DetailTitle"].ToString(),
                                DetailType = DetailType,
                                DetailMustKeyin = QuestionDetailDR["DetailMustKeyin"].ToString(),
                                ItemName = ItemNames

                            };
                            list.Add(model);
                        }
                        HttpContext.Current.Session["QuestionDetail"] = list;
                    }
                }

                #endregion
            }
            #region 統計頁面區
            //取得問卷標題
            var SuveryDataRow = BackgroundMethod.GetSuveryMasterData(guid);
            if (SuveryDataRow != null)
            {
                this.h3Title.InnerText = SuveryDataRow["Title"].ToString();
            }
            //取得問卷問題標題
            var SuveryQuestionTitleDT = BackgroundMethod.GetQuestionDetailAndItemDetail(guid);
            //列印問卷問題標題
            for (int i = 0; i < SuveryQuestionTitleDT.Rows.Count; i++)
            {
                var QuestionDetailDR = SuveryQuestionTitleDT.Rows[i];
                int DetailID = (int)QuestionDetailDR["DetailID"];
                string QuestionTitle = QuestionDetailDR["DetailTitle"].ToString();
                string QuestionType = QuestionDetailDR["DetailType"].ToString();
                int ItemCount; //單多選項目總數
                if (QuestionDetailDR["ItemCount"].ToString() == string.Empty)
                    ItemCount = 0;
                else
                    ItemCount = (int)QuestionDetailDR["ItemCount"];
                Label lblTitle = new Label(); //問題標題的lbl
                switch (QuestionType)
                {
                    case "QT5":
                        lblTitle.Text = QuestionTitle + "</br>";
                        this.StatisticArea.Controls.Add(lblTitle);
                        for (int j = 0; j < ItemCount; j++)
                        {

                            string ColName = "Item" + (j + 1).ToString();
                            string ItemName = QuestionDetailDR[ColName].ToString();
                            string ItemSelectedCount = BackgroundMethod.GetItemSelectedCount(ItemName, DetailID);
                            Label lblItemTitle = new Label();
                            lblItemTitle.Text = "&emsp;&emsp;" + ItemName + "&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;" + $"共 : {ItemSelectedCount} 人" + "</br>";
                            this.StatisticArea.Controls.Add(lblItemTitle);
                        }
                        break;
                    case "QT6":
                        lblTitle.Text = QuestionTitle + "</br>";
                        this.StatisticArea.Controls.Add(lblTitle);
                        for (int j = 0; j < ItemCount; j++)
                        {

                            string ColName = "Item" + (j + 1).ToString();
                            string ItemName = QuestionDetailDR[ColName].ToString();
                            string ItemSelectedCount = BackgroundMethod.GetItemSelectedCount(ItemName);
                            Label lblItemTitle = new Label();
                            lblItemTitle.Text = "&emsp;&emsp;" + ItemName + "&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;" + $"共 : {ItemSelectedCount} 人" + "</br>";
                            this.StatisticArea.Controls.Add(lblItemTitle);
                        }
                        break;
                }
            }
            #endregion
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string guidString = Request.QueryString["ID"];
            Guid guid = Guid.Parse(guidString);
            // get input
            string SuveryTitle = this.txtSuveryTitle.Text;
            string SuverySummary = this.txtSummary.Text;
            string StartDate = this.txtStartDate.Text;
            string EndDate = this.txtEndDate.Text;
            string Status;
            if (this.StatusCheck.Checked == true)
                Status = "Y";
            else
                Status = "N";
            // checkinput
            if (string.IsNullOrEmpty(SuveryTitle) ||
               string.IsNullOrEmpty(SuverySummary) ||
               string.IsNullOrEmpty(StartDate) ||
               string.IsNullOrEmpty(EndDate))
            {
                Response.Write("<script>alert('請確認所有輸入框都有輸入值')</script>");
                return;
            }
            else
            {
                //Response.Write($"<script>alert('{SuveryMaster}')</script>");
                BackgroundMethod.CreateNewSuvery(guid, SuveryTitle, SuverySummary, StartDate, EndDate, Status);
                return;
            }
        }
        /// <summary>問卷編輯內頁編輯問卷資料按鈕事件 </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string guidString = Request.QueryString["ID"];
            Guid guid = Guid.Parse(guidString);
            // get input
            string SuveryTitle = this.txtSuveryTitle.Text;
            string SuverySummary = this.txtSummary.Text;
            string StartDate = this.txtStartDate.Text;
            string EndDate = this.txtEndDate.Text;
            string Status;
            if (this.StatusCheck.Checked == true)
                Status = "Y";
            else
                Status = "N";
            // checkinput
            if (string.IsNullOrEmpty(SuveryTitle) ||
               string.IsNullOrEmpty(SuverySummary) ||
               string.IsNullOrEmpty(StartDate) ||
               string.IsNullOrEmpty(EndDate))
            {
                Response.Write("<script>alert('請確認所有輸入框都有輸入值')</script>");
                return;
            }
            else
            {
                BackgroundMethod.UpdateSuveryDate(guid, SuveryTitle, SuverySummary, StartDate, EndDate, Status);
                return;
            }

        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminList.aspx");
        }
        /// <summary>送出session中存放的問題資料</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit2_Click(object sender, EventArgs e)
        {
            string IDString = Request.QueryString["ID"];
            Guid guid = Guid.Parse(IDString);
            //// 清空問題內容資料表及項目內容資料表
            BackgroundMethod.RemoveQuestionDetail(guid);
            BackgroundMethod.RemoveItemDetail(guid);
            ////
            var list = (List<QuestionDetailModel>)HttpContext.Current.Session["QuestionDetail"];
            for (int i = 0; i < list.Count; i++)
            {
                string QTitle = list[i].DetailTitle;
                string QType = string.Empty;
                var TempQType = list[i].DetailType;
                switch (TempQType)
                {
                    case "文字方塊(文字)":
                        QType = "QT1";
                        break;
                    case "文字方塊(數字)":
                        QType = "QT2";
                        break;
                    case "文字方塊(E-Mail)":
                        QType = "QT3";
                        break;
                    case "文字方塊(日期)":
                        QType = "QT4";
                        break;
                    case "單選方塊":
                        QType = "QT5";
                        break;
                    case "多選方塊":
                        QType = "QT6";
                        break;
                }
                string QMustKeyIn = string.Empty;
                var TempQMustKeyIn = list[i].DetailMustKeyin;
                switch (TempQMustKeyIn)
                {
                    case "Yes":
                        QMustKeyIn = "Y";
                        break;
                    case "No":
                        QMustKeyIn = "N";
                        break;
                }
                BackgroundMethod.CreateNewQuestion(guid, QTitle, QType, QMustKeyIn);
                string QItemName = string.Empty;
                if (!string.IsNullOrEmpty(list[i].ItemName))
                {
                    var QDetailID = BackgroundMethod.GetQuestionDetailID(guid, QTitle);
                    QItemName = list[i].ItemName;
                    var ItemNameArray = QItemName.Split(',');
                    string Item1 = string.Empty;
                    string Item2 = string.Empty;
                    string Item3 = string.Empty;
                    string Item4 = string.Empty;
                    switch (ItemNameArray.Length)
                    {
                        case 1:
                            Item1 = ItemNameArray[0];
                            BackgroundMethod.CreateQuestionItem(QDetailID, guid, Item1, Item2, Item3, Item4, ItemNameArray.Length);
                            break;
                        case 2:
                            Item1 = ItemNameArray[0];
                            Item2 = ItemNameArray[1];
                            BackgroundMethod.CreateQuestionItem(QDetailID, guid, Item1, Item2, Item3, Item4, ItemNameArray.Length);
                            break;
                        case 3:
                            Item1 = ItemNameArray[0];
                            Item2 = ItemNameArray[1];
                            Item3 = ItemNameArray[2];
                            BackgroundMethod.CreateQuestionItem(QDetailID, guid, Item1, Item2, Item3, Item4, ItemNameArray.Length);
                            break;
                        case 4:
                            Item1 = ItemNameArray[0];
                            Item2 = ItemNameArray[1];
                            Item3 = ItemNameArray[2];
                            Item4 = ItemNameArray[3];
                            BackgroundMethod.CreateQuestionItem(QDetailID, guid, Item1, Item2, Item3, Item4, ItemNameArray.Length);
                            break;
                    }
                }
            }
            Response.Redirect(Request.Url.ToString());
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

        private List<UserInfoModel> GetPagedDetailList(List<UserInfoModel> list, int pagesize)
        {
            int startIndex = (this.GetCurrentPage() - 1) * pagesize;
            return list.Skip(startIndex).Take(pagesize).ToList();
        }
        #endregion
        #region DDL_SelectedIndexChange
        protected void QTypeddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.QTypeddl.SelectedItem.Value == "QT5" || this.QTypeddl.SelectedItem.Value == "QT6")
                this.txtAnswer.Enabled = true;
            else
            {
                this.txtAnswer.Text = string.Empty;
                this.txtAnswer.Enabled = false;
            }
        }
        protected void TemplateQddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            string QuestionTemplateName = this.TemplateQddl.SelectedItem.Text;
            var QuestionTemplateDrDetail = BackgroundMethod.GetQuestionTemplateDrDetail(QuestionTemplateName);
            string QName = QuestionTemplateDrDetail["QuestionTemplateName"].ToString();
            string QType = QuestionTemplateDrDetail["QuestionTemplateType"].ToString();
            string QMustKeyIn = QuestionTemplateDrDetail["QuestionTemplateMustKeyIn"].ToString();
            string QuestionTemplateItemName = string.Empty;
            if (QuestionTemplateDrDetail["QuestionTemplateItemName"] != null)
            {
                QuestionTemplateItemName = QuestionTemplateDrDetail["QuestionTemplateItemName"].ToString();
            }

            this.txtQuestion.Text = QName;
            switch (QType)
            {
                case "QT1":
                    this.QTypeddl.SelectedIndex = 0;
                    break;
                case "QT2":

                    this.QTypeddl.SelectedIndex = 1;
                    break;
                case "QT3":
                    this.QTypeddl.SelectedIndex = 2;
                    break;
                case "QT4":
                    this.QTypeddl.SelectedIndex = 3;
                    break;
                case "QT5":
                    this.QTypeddl.SelectedIndex = 4;
                    break;
                case "QT6":
                    this.QTypeddl.SelectedIndex = 5;
                    break;
            }
            if (QMustKeyIn == "Y")
            {
                this.QMustKeyIn.Checked = true;
            }
            else
            {
                this.QMustKeyIn.Checked = false;
            }
            if (!string.IsNullOrEmpty(QuestionTemplateItemName))
            {
                this.txtAnswer.Text = QuestionTemplateItemName;
            }
        }
        #endregion
        protected void btnCSVDownload_Click(object sender, EventArgs e)
        {
            string SuveryID = Request.QueryString["ID"].ToString();
            Guid guid = Guid.Parse(SuveryID);

            string SuveryTitle = BackgroundMethod.GetSuveryTitle(guid);

            List<CSVDownloadModel> CSVlist = new List<CSVDownloadModel>();


            var UserInfoDT = BackgroundMethod.GetAnswerUserInfoCount(guid);
            for (int i = 0; i < UserInfoDT.Rows.Count; i++)
            {
                CSVDownloadModel model = new CSVDownloadModel();
                //
                string[] tempArray = new string[UserInfoDT.Rows.Count];
                //
                var UserInfoDR = UserInfoDT.Rows[i];
                //
                string UserInfoString = UserInfoDR["UserInfo"].ToString();
                var SingleUserAnswerDT = BackgroundMethod.GetSingleUserAnswerDetail(UserInfoString);

                string[] UserInfoArray = UserInfoString.Split(',');
                string InfoString = "姓名:" + UserInfoArray[0] + " 電話:" + UserInfoArray[1] + " 信箱:" + UserInfoArray[2] + " 年齡:" + UserInfoArray[3];

                string csvString = "填表人資料 - " + InfoString;
                //csvString =  
                //
                //var SingleUserAnswerDT = GetSingleUserAnswerDetail(UserInfoString);
                //string csvString = string.Empty;
                //csvString = "填表人資料 : " + UserInfoString;
                for (int j = 0; j < SingleUserAnswerDT.Rows.Count; j++)
                {
                    var SingleAnswerDR = SingleUserAnswerDT.Rows[j];
                    string QuestionAndAnswerString = string.Empty;
                    csvString += "   問題 : " + SingleAnswerDR["DetailTitle"].ToString() + "    " + "回答 : " + SingleAnswerDR["Answer"].ToString() + "    ";
                    tempArray[i] = csvString;

                }
                model.CSVString = tempArray[i];
                CSVlist.Add(model);
            }
            for (int i = 0; i < CSVlist.Count; i++)
            {
                var tempstring = CSVlist[i].CSVString.ToString();
                Response.Write($"<script>alert('{tempstring}')</script>");
            }

            //
            Response.Clear();
            Response.ContentType = "text/comma-separated-values;charset=BIG5";
            Response.AddHeader("content-disposition", $"attachment; filename={SuveryTitle}-填寫資料.csv");

            StreamWriter sw = new StreamWriter(Response.OutputStream, Encoding.GetEncoding("BIG5"));
            for (int i = 0; i < CSVlist.Count; i++)
            {
                sw.Write(CSVlist[i].CSVString.ToString() + "\r\n");
            }
            sw.WriteLine();
            sw.Close();

            Response.End();
            //
        }
    }
}