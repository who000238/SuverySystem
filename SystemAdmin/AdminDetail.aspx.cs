using DBSource;
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
            var CheckSuveryDataExistDR = GetSuveryMaster(guid);
            if (CheckSuveryDataExistDR != null)//表示DB內已經有這筆問卷的細節 則新增按鈕關閉顯示改為顯示修改按鈕
            {
                this.btnSubmit.Visible = false;
                this.btnUpdate.Visible = true;
            }
            //檢查此ID是否已有填答資料
            var AnsDataExistOrNotDT = CheckAnsExistOrNot(guid);
            if(AnsDataExistOrNotDT.Rows.Count>0 )
            {
                this.hfAnswerExistOrNot.Value = "Exist";
            }


            // 常用問題下拉式選單DataSource
            var QuestionTemplateDT = GetQuestionTemplate();
            // 觀看問卷填寫細節頁面Repeater DataSource
            var UserInfoDT = GetUserInfoForSeeDetail(guid);
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
                this.Repeater2.DataSource = list;
                this.Repeater2.DataBind();
            }
            if (!IsPostBack)
            {
                #region 確認資料庫是否有內已存放之問卷內容、若有 輸出到頁面上
                var SuveryMasterDR = GetSuveryMaster(guid);
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
                var QuestionDetailDT = GetQuestionDetail(guid);
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
                #endregion
            }

            #region 統計頁面區
            //取得問卷標題
            var SuveryDataRow = GetSuveryMasterData(guid);
            if (SuveryDataRow != null)
            {
                this.h3Title.InnerText = SuveryDataRow["Title"].ToString();

            }
            //取得問卷問題標題
            var SuveryQuestionTitleDT = GetQuestionDetailAndItemDetail(guid);
            //列印問卷問題標題
            for (int i = 0; i < SuveryQuestionTitleDT.Rows.Count; i++)
            {
                var QuestionDetailDR = SuveryQuestionTitleDT.Rows[i];
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
                            string ItemSelectedCount = GetItemSelectedCount(ItemName);
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
                            string ItemSelectedCount = GetItemSelectedCount(ItemName);
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
                CreateNewSuvery(guid, SuveryTitle, SuverySummary, StartDate, EndDate, Status);
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
                UpdateSuveryDate(guid, SuveryTitle, SuverySummary, StartDate, EndDate, Status);
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
            //RemoveQuestionDetail(guid);
            //RemoveItemDetail(guid);
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
                CreateNewQuestion(guid, QTitle, QType, QMustKeyIn);

                string QItemName = string.Empty;
                if (!string.IsNullOrEmpty(list[i].ItemName))
                {
                    var QDetailID = GetQuestionDetailID(guid, QTitle);
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
                            CreateQuestionItem(QDetailID, guid, Item1, Item2, Item3, Item4, ItemNameArray.Length);
                            break;
                        case 2:
                            Item1 = ItemNameArray[0];
                            Item2 = ItemNameArray[1];
                            CreateQuestionItem(QDetailID, guid, Item1, Item2, Item3, Item4, ItemNameArray.Length);
                            break;
                        case 3:
                            Item1 = ItemNameArray[0];
                            Item2 = ItemNameArray[1];
                            Item3 = ItemNameArray[2];
                            CreateQuestionItem(QDetailID, guid, Item1, Item2, Item3, Item4, ItemNameArray.Length);
                            break;
                        case 4:
                            Item1 = ItemNameArray[0];
                            Item2 = ItemNameArray[1];
                            Item3 = ItemNameArray[2];
                            Item4 = ItemNameArray[3];
                            CreateQuestionItem(QDetailID, guid, Item1, Item2, Item3, Item4, ItemNameArray.Length);
                            break;
                    }
                }
            }
            Response.Redirect(Request.Url.ToString());
        }




        #region Method
        #region CreateQuestionItem(多載方法??)
        public static void CreateQuestionItem(int DetailID, Guid SuveryID, string Item1, string Item2, string Item3, string Item4, int ItemCount)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    INSERT INTO [dbo].[ItemDetail]
                               ([DetailID]
                               ,[SuveryID]
                               ,[Item1]
                               ,[Item2]
                               ,[Item3]
                               ,[Item4]
                               ,[ItemCount])
                         VALUES
                               (@DetailTitle
                               , @SuveryID      
                               ,@Item1       
                               ,@Item2            
                               ,@Item3            
                               ,@Item4            
                               ,@ItemCount)    
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@DetailTitle", DetailID));
            list.Add(new SqlParameter("@SuveryID", SuveryID));
            list.Add(new SqlParameter("@Item1", Item1));
            list.Add(new SqlParameter("@Item2", Item2));
            list.Add(new SqlParameter("@Item3", Item3));
            list.Add(new SqlParameter("@Item4", Item4));
            list.Add(new SqlParameter("@ItemCount", ItemCount));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        #endregion
        /// <summary>清除問題資料表內的內容</summary>
        /// <param name="guid"></param>
        public static void RemoveQuestionDetail(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    DELETE FROM [dbo].[SuveryDetail]
                    WHERE [ItemDetailID] = @SuveryID
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@SuveryID", guid));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }      /// <summary>清除問題項目資料表內的內容</summary>
               /// <param name="guid"></param>
        public static void RemoveItemDetail(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    DELETE FROM [dbo].[ItemDetail]
                    WHERE [ItemDetailID] = @SuveryID
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@SuveryID", guid));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>確認此ID的問卷有無填答內容 若有 在管理者編輯問題內容時提示使用者可能會造成資料對不上進而造成系統跳出EX</summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static DataTable CheckAnsExistOrNot(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @" 
                    SELECT * FROM [SuverySystem].[dbo].[AnswerDetail]
                    WHERE [AnswerDetail].[SuveryID]= @Guid
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

        /// <summary>取得問題、項目明細</summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static DataTable GetQuestionDetail(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @" 
                    SELECT 
                        [SuveryDetail].[DetailID],
                        [SuveryDetail].[DetailTitle],
                        [SuveryDetail].[DetailType],
                        [SuveryDetail].[DetailMustKeyin],
                        [SuveryDetail].[SuveryID],
                        [ItemDetail].[Item1],
                        [ItemDetail].[Item2],
                        [ItemDetail].[Item3],
                        [ItemDetail].[Item4],
                        [ItemDetail].[ItemCount]
                    FROM  [SuveryDetail]
                    LEFT JOIN [ItemDetail] ON [SuveryDetail].[DetailID]=[ItemDetail].[DetailID]
                    WHERE SuveryDetail.[SuveryID]= @Guid
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
        /// <summary>讀取DB內現存之問卷、問題資料</summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static DataRow GetSuveryMaster(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                   @" 
                    SELECT * FROM [SuverySystem].[dbo].[SuveryMaster]
                    WHERE [SuveryID] = @SuveryID
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@SuveryID", guid));

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

        /// <summary>取得問卷資料用於觀看細節</summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static DataTable GetUserInfoForSeeDetail(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    SELECT * FROM [SuverySystem].[dbo].[UserInfo]
                    WHERE [SuveryID] = @SuveryID
                    ORDER BY [No] DESC
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@SuveryID", guid));

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

        public static int GetQuestionDetailID(Guid SuveryID, string DetailTitle)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    SELECT * FROM [SuverySystem].[dbo].[SuveryDetail]
                    WHERE SuveryID= @SuveryID AND DetailTitle= @DetailTitle                  
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@SuveryID", SuveryID));
            list.Add(new SqlParameter("@DetailTitle", DetailTitle));

            try
            {
                var dr = DBHelper.ReadDataRow(connectionString, dbCommandString, list);
                return (int)dr["DetailID"];
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return 0;
            }
        }
        public static void CreateNewQuestion(Guid SuveryID, string DetailTitle, string DetailType, string DetailMustKeyIn)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    INSERT INTO [dbo].[SuveryDetail]
                               ([SuveryID]
                               ,[DetailTitle]
                               ,[DetailType]
                               ,[DetailMustKeyin])
                         VALUES
                               (@SuveryID
                               ,@DetailTitle
                               ,@DetailType
                               ,@DetailMustKeyIn)
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@SuveryID", SuveryID));
            list.Add(new SqlParameter("@DetailTitle", DetailTitle));
            list.Add(new SqlParameter("@DetailType", DetailType));
            list.Add(new SqlParameter("@DetailMustKeyIn", DetailMustKeyIn));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        /// <summary>根據使用者輸入值新增問卷資料</summary>
        /// <param name="guid"></param>
        /// <param name="Title"></param>
        /// <param name="Summary"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Status"></param>
        public static void CreateNewSuvery(Guid guid, string Title, string Summary, string StartDate, string EndDate, string Status)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    INSERT INTO [dbo].[SuveryMaster]
                               ([SuveryID]
                               ,[Title]
                               ,[StartDate]
                               ,[EndDate]
                               ,[Status]
                               ,[Summary])
                         VALUES
                               (@Guid
                               ,@Title
                               ,@StartDate
                               ,@EndDate
                               ,@Status
                               ,@Summary)
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            list.Add(new SqlParameter("@Title", Title));
            list.Add(new SqlParameter("@Summary", Summary));
            list.Add(new SqlParameter("@StartDate", StartDate));
            list.Add(new SqlParameter("@EndDate", EndDate));
            list.Add(new SqlParameter("@Status", Status));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        public static void UpdateSuveryDate(Guid guid, string Title, string Summary, string StartDate, string EndDate, string Status)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                        UPDATE [dbo].[SuveryMaster]
                           SET 
                               [Title] = @Title
                              ,[StartDate] = @StartDate
                              ,[EndDate] = @EndDate
                              ,[Status] = @Status
                              ,[Summary] = @Summary
                         WHERE [SuveryID] = @Guid
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            list.Add(new SqlParameter("@Title", Title));
            list.Add(new SqlParameter("@Summary", Summary));
            list.Add(new SqlParameter("@StartDate", StartDate));
            list.Add(new SqlParameter("@EndDate", EndDate));
            list.Add(new SqlParameter("@Status", Status));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        public static DataTable GetQuestionTemplate()
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    SELECT * FROM [SuverySystem].[dbo].[QuestionTemplateDetail]
                ";
            List<SqlParameter> list = new List<SqlParameter>();
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

        public static DataRow GetQuestionTemplateDrDetail(string QuestionTemplateName)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
               @" 
                    SELECT * FROM [SuverySystem].[dbo].[QuestionTemplateDetail]
                    WHERE QuestionTemplateName = @QuestionTemplateName
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@QuestionTemplateName", QuestionTemplateName));
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
        #region 匯出CSV所需的Method
        public static DataTable GetAnswerUserInfoCount(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @"
                    SELECT 
	                    UserInfo
                      FROM 
                      dbo.[AnswerDetail]
                      WHERE  [AnswerDetail].[SuveryID]=@Guid
                      GROUP BY dbo.[AnswerDetail].[UserInfo]
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
        /// <summary>取得匯出CSV檔案需要的資料</summary>
        /// <param name="guid"></param>
        /// <param name="UserInfo"></param>
        /// <returns></returns>

        public static DataTable GetSingleUserAnswerDetail(string Userinfo)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @"
                SELECT 
	                [AnswerDetail].[UserInfo],
	                [SuveryDetail].[DetailTitle],
	                [AnswerDetail].[Answer]
                  FROM 
                  dbo.[AnswerDetail]
                  JOIN dbo.[SuveryDetail] ON dbo.[AnswerDetail].DetailID =dbo.[SuveryDetail].[DetailID]
                  WHERE  [AnswerDetail].[UserInfo]=@Userinfo
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Userinfo", Userinfo));
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

        #region 統計頁面區
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


        //
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
        //
        public static string GetItemSelectedCount(string ItemName)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                          @" SELECT COUNT([SuverySystem].[dbo].[AnswerDetail].[Answer]) AS SelectedCount
                                FROM  AnswerDetail 
                            WHERE Answer LIKE @ItemName
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@ItemName", "%" + ItemName + "%"));
            try
            {
                var dr = DBHelper.ReadDataRow(connectionString, dbCommandString, list);
                string ItemSelectedCount = dr["SelectedCount"].ToString();
                return ItemSelectedCount;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }
        #endregion
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
            var QuestionTemplateDrDetail = GetQuestionTemplateDrDetail(QuestionTemplateName);
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

            List<CSVDownloadModel> CSVlist = new List<CSVDownloadModel>();


            var UserInfoDT = GetAnswerUserInfoCount(guid);
            for (int i = 0; i < UserInfoDT.Rows.Count; i++)
            {
                CSVDownloadModel model = new CSVDownloadModel();
                //
                string[] tempArray = new string[UserInfoDT.Rows.Count];
                //
                var UserInfoDR = UserInfoDT.Rows[i];
                string UserInfoString = UserInfoDR["UserInfo"].ToString();
                var SingleUserAnswerDT = GetSingleUserAnswerDetail(UserInfoString);
                string csvString = string.Empty;
                csvString = "填表人資料 : " + UserInfoString;
                for (int j = 0; j < SingleUserAnswerDT.Rows.Count; j++)
                {
                    var SingleAnswerDR = SingleUserAnswerDT.Rows[j];
                    string QuestionAndAnswerString = string.Empty;
                    csvString += "問題 : " + SingleAnswerDR["DetailTitle"].ToString() + "    " + "回答 : " + SingleAnswerDR["Answer"].ToString() + "    ";
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
            Response.AddHeader("content-disposition", "attachment; filename=檔名.csv");

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