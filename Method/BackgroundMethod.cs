using DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Method
{
    public class BackgroundMethod
    {
        #region List
        /// <summary>取得清單用DT</summary>
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
        /// <summary>搜尋問卷</summary>
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
        /// <summary>檢查狀態</summary>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <returns></returns>
        public static string CheckStatus(DateTime SDate, DateTime EDate)
        {
            DateTime Today = DateTime.Today;
            //if (DateTime.Compare(Today, SDate) < 0)
            //{
            //    return "尚未開始";
            //}
            //else 
            if (DateTime.Compare(Today, SDate) >= 0 &&
                DateTime.Compare(Today, EDate) <= 0)
            {
                return "開放中";
            }
            else
                return "關閉中";
        }
        #region 泛型處理常式
        public static void DeleteSuvery(string guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    DELETE FROM [dbo].[SuveryMaster]
                    WHERE [SuveryID] = @SuveryID
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
        #endregion
        #endregion
        #region Detail

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
                    WHERE [SuveryID] = @SuveryID
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
        /// <summary>清除問題項目資料表內的內容</summary>
        /// <param name="guid"></param>
        public static void RemoveItemDetail(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    DELETE FROM [dbo].[ItemDetail]
                    WHERE [SuveryID] = @SuveryID
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
        /// <summary>取得問題在資料表內的整數ID</summary>
        /// <param name="SuveryID"></param>
        /// <param name="DetailTitle"></param>
        /// <returns></returns>
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
        /// <summary>新增問題至資料表中</summary>
        /// <param name="SuveryID"></param>
        /// <param name="DetailTitle"></param>
        /// <param name="DetailType"></param>
        /// <param name="DetailMustKeyIn"></param>
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
        /// <summary>更新問卷資料</summary>
        /// <param name="guid"></param>
        /// <param name="Title"></param>
        /// <param name="Summary"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Status"></param>
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
        /// <summary>取得樣板問題</summary>
        /// <returns></returns>
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
        /// <summary>取得樣板問題資料列細節</summary>
        /// <param name="QuestionTemplateName"></param>
        /// <returns></returns>
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
        /// <summary>取得填答總人數</summary>
        /// <param name="guid"></param>
        /// <returns></returns>
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
        /// <summary>取得CSV匯出檔案用問卷名稱</summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string GetSuveryTitle(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                          @" 
                        SELECT 
                             [SuveryMaster].[Title]
                          FROM [SuverySystem].[dbo].[SuveryMaster]
                          WHERE [SuveryMaster].[SuveryID]=@Guid
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            try
            {
                var dr = DBHelper.ReadDataRow(connectionString, dbCommandString, list);
                string Title = dr["Title"].ToString();
                return Title;
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
        /// <summary>取得問題及項目細節</summary>
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
        //單選用
        public static string GetItemSelectedCount(string ItemName, int DetailID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                          @" SELECT COUNT([SuverySystem].[dbo].[AnswerDetail].[Answer]) AS SelectedCount
                                FROM  AnswerDetail 
                            WHERE Answer LIKE @ItemName AND [DetailID] = @DetailID
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@ItemName", ItemName));
            list.Add(new SqlParameter("@DetailID", DetailID));
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
        //多選用
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
        #region TemplateQuestion
        /// <summary>新增單多選帶選項樣板問題</summary>
        /// <param name="QuestionTemplateName"></param>
        /// <param name="QuestionTemplateType"></param>
        /// <param name="QuestionTemplateMustKeyIn"></param>
        /// <param name="QuestionTemplateItemName"></param>
        public static void CreateQuestionTemplate(string QuestionTemplateName,
    string QuestionTemplateType, string QuestionTemplateMustKeyIn, string
    QuestionTemplateItemName)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    INSERT INTO [dbo].[QuestionTemplateDetail]
                               ([QuestionTemplateName]
                               ,[QuestionTemplateType]
                               ,[QuestionTemplateMustKeyIn]
                               ,[QuestionTemplateItemName])
                         VALUES
                               (@QuestionTemplateName
                               ,@QuestionTemplateType
                               ,@QuestionTemplateMustKeyIn
                               ,@QuestionTemplateItemName)
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@QuestionTemplateName", QuestionTemplateName));
            list.Add(new SqlParameter("@QuestionTemplateType", QuestionTemplateType));
            list.Add(new SqlParameter("@QuestionTemplateMustKeyIn", QuestionTemplateMustKeyIn));
            list.Add(new SqlParameter("@QuestionTemplateItemName", QuestionTemplateItemName));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        /// <summary>新增文字方塊樣板問題</summary>
        /// <param name="QuestionTemplateName"></param>
        /// <param name="QuestionTemplateType"></param>
        /// <param name="QuestionTemplateMustKeyIn"></param>
        public static void CreateQuestionTemplate(string QuestionTemplateName,
           string QuestionTemplateType, string QuestionTemplateMustKeyIn
           )
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    INSERT INTO [dbo].[QuestionTemplateDetail]
                               ([QuestionTemplateName]
                               ,[QuestionTemplateType]
                               ,[QuestionTemplateMustKeyIn]
                               )
                         VALUES
                               (@QuestionTemplateName
                               ,@QuestionTemplateType
                               ,@QuestionTemplateMustKeyIn
                               )
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@QuestionTemplateName", QuestionTemplateName));
            list.Add(new SqlParameter("@QuestionTemplateType", QuestionTemplateType));
            list.Add(new SqlParameter("@QuestionTemplateMustKeyIn", QuestionTemplateMustKeyIn));

            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        /// <summary>取得問題樣板DT</summary>
        /// <returns></returns>
        public static DataTable GetQuestionTemplateDT()
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                      SELECT * FROM [dbo].[QuestionTemplateDetail]
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
        #region 泛型處理常式
        public static void DeleteQuestionTemplate(string QuestionTemplateNo)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" 
                    DELETE FROM [dbo].[QuestionTemplateDetail]
                    WHERE [QuestionTemplateNo] = @QuestionTemplateNo
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@QuestionTemplateNo", QuestionTemplateNo));
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
        #endregion
        #region SeeAnswerDetail
        /// <summary>取得三張資料表內的內容分別是問題清單，選項清單，答案清單</summary>
        /// <param name="guid"></param>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        public static DataTable GetAnswerDetail(Guid guid, string UserInfo)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @"
                        SELECT 
			                        SuveryDetail.DetailID,
			                        SuveryDetail.SuveryID,
			                        SuveryDetail.DetailTitle,
			                        SuveryDetail.DetailType,
			                        SuveryDetail.DetailMustKeyin,
			                        ItemDetail.Item1,
			                        ItemDetail.Item2,
			                        ItemDetail.Item3,
			                        ItemDetail.Item4,
			                        ItemDetail.ItemCount,
			                        AnswerDetail.Answer,
			                        AnswerDetail.CreateTime,
			                        AnswerDetail.UserInfo
                        FROM 

                                    [SuverySystem].[dbo].[SuveryDetail]
                         LEFT JOIN 
                                    [SuverySystem].[dbo].[ItemDetail] 
                        ON 
                                    [SuverySystem].[dbo].[SuveryDetail].[DetailID] =  [SuverySystem].[dbo].[ItemDetail].[DetailID]
                        JOIN 
                                [SuverySystem].[dbo].[AnswerDetail] 
                        ON 
                                [SuverySystem].[dbo].[SuveryDetail].[DetailID] =[SuverySystem].[dbo].[AnswerDetail].[DetailID]
                        WHERE 
                                [SuverySystem].[dbo].[SuveryDetail].[SuveryID] = @Guid
                        AND
		                        [SuverySystem].[dbo].[AnswerDetail].[UserInfo] = @UserInfo
                        ORDER BY [SuverySystem].[dbo].[SuveryDetail].[DetailID]                      
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            list.Add(new SqlParameter("@UserInfo", UserInfo));
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
