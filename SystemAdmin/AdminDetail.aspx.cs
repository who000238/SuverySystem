﻿using DBSource;
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
                    userInfoModel.No = UserInfoDT.Rows.Count-i;
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


            #region 統計頁面區
            //取得問卷標題
            var SuveryDataRow = GetSuveryMasterData(guid);
            this.h3Title.InnerText = SuveryDataRow["Title"].ToString();
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
                //string SuveryMaster = SuveryTitle + "," + SuverySummary + "," + StartDate + "," + EndDate + "," + Status;
                //this.Session["SuveryMaster"] = SuveryMaster;
                //Response.Write($"<script>alert('{SuveryMaster}')</script>");
                CreateNewSuvery(guid, SuveryTitle, SuverySummary, StartDate, EndDate, Status);
                return;
            }
        }
        protected void btnCancle_Click(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            //string guidString = Request.QueryString["ID"];
            //string QuestionTitle = this.txtQuestion.Text;
            //string QuestionType = this.QTypeddl.SelectedValue;
            //string QuestionIsMustKeyIn;
            //if (this.QMustKeyIn.Checked)
            //    QuestionIsMustKeyIn = "Y";
            //else
            //    QuestionIsMustKeyIn = "N";
            //string ItemName = string.Empty;
            //string QuestionDetail;
            //if (QuestionType == "QT5" || QuestionType == "QT6")
            //{
            //    ItemName = this.txtAnswer.Text;
            //}
            //if (string.IsNullOrEmpty(ItemName))
            //    QuestionDetail = QuestionTitle + QuestionType + QuestionIsMustKeyIn;
            //else
            //    QuestionDetail = QuestionTitle + QuestionType + QuestionIsMustKeyIn + ItemName;


            //if (HttpContext.Current.Session["QuestionDetail"] == null)
            //{
            //    QuestionDetailModel model = new QuestionDetailModel()
            //    {
            //        SuveryID = guidString,
            //        DetailTitle = QuestionTitle,
            //        DetailType = QuestionType,
            //        DetailMustKeyin = QuestionIsMustKeyIn,
            //        ItemName = ItemName
            //    };
            //    List<QuestionDetailModel> list = new List<QuestionDetailModel>();
            //    list.Add(model);
            //    HttpContext.Current.Session["QuestionDetail"] = list;
            //}
            //else
            //{
            //    List<QuestionDetailModel> sourceList = (List<QuestionDetailModel>)HttpContext.Current.Session["QuestionDetail"];

            //    QuestionDetailModel model = new QuestionDetailModel()
            //    {
            //        SuveryID = guidString,
            //        DetailTitle = QuestionTitle,
            //        DetailType = QuestionType,
            //        DetailMustKeyin = QuestionIsMustKeyIn,
            //        ItemName = ItemName
            //    };


            //    List<QuestionDetailModel> list = new List<QuestionDetailModel>();
            //    list.Add(model);
            //    HttpContext.Current.Session["QuestionDetail"] = sourceList;

            //}

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancle2_Click(object sender, EventArgs e)
        {
            List<QuestionDetailModel> list = (List<QuestionDetailModel>)this.Session["QuestionDetail"];
            int count = list.Count;
            Response.Write($"<script>alert('{count}'</script>");
        }

        protected void btnSubmit2_Click(object sender, EventArgs e)
        {
            string IDString = Request.QueryString["ID"];
            Guid id = Guid.Parse(IDString);

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
                CreateNewQuestion(id, QTitle, QType, QMustKeyIn);

                string QItemName = string.Empty;
                if (!string.IsNullOrEmpty(list[i].ItemName))
                {
                    var QDetailID = GetQuestionDetailID(id, QTitle);
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
                                CreateQuestionItem(QDetailID, id, Item1, Item2, Item3, Item4, ItemNameArray.Length);
                                break;
                            case 2:
                                Item1 = ItemNameArray[0];
                                Item2 = ItemNameArray[1];
                                CreateQuestionItem(QDetailID, id, Item1, Item2, Item3, Item4, ItemNameArray.Length);
                                break;
                            case 3:
                                Item1 = ItemNameArray[0];
                                Item2 = ItemNameArray[1];
                                Item3 = ItemNameArray[2];
                                CreateQuestionItem(QDetailID, id, Item1, Item2, Item3, Item4, ItemNameArray.Length);
                                break;
                            case 4:
                                Item1 = ItemNameArray[0];
                                Item2 = ItemNameArray[1];
                                Item3 = ItemNameArray[2];
                                Item4 = ItemNameArray[3];
                                CreateQuestionItem(QDetailID, id, Item1, Item2, Item3, Item4, ItemNameArray.Length);
                                break;
                        }
                }
            }
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
        //public static DataTable GetSuveryQuestionTItle (Guid guid)
        //{
        //    string connectionString = DBHelper.GetConnectionString();
        //    string dbCommandString =
        //         @" SELECT  * FROM 
        //                    [SuverySystem].[dbo].[SuveryDetail]
        //             WHERE [SuverySystem].[dbo].[SuveryDetail].[SuveryID] = @Guid
        //            ORDER BY [SuverySystem].[dbo].[SuveryDetail].[DetailID]
        //        ";
        //    List<SqlParameter> list = new List<SqlParameter>();
        //    list.Add(new SqlParameter("@Guid", guid));
        //    try
        //    {
        //        return DBHelper.ReadDataTable(connectionString, dbCommandString, list);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog(ex);
        //        return null;
        //    }
        //}

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
                csvString = "填表人資料 : "+UserInfoString;
                for (int j = 0; j < SingleUserAnswerDT.Rows.Count; j++)
                {
                    var SingleAnswerDR = SingleUserAnswerDT.Rows[j];
                    string QuestionAndAnswerString = string.Empty;
                    csvString += "問題 : " +SingleAnswerDR["DetailTitle"].ToString() + "    " + "回答 : "+SingleAnswerDR["Answer"].ToString() +"    ";
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