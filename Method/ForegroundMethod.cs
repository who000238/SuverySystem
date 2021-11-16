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
    public class ForegroundMethod
    {

        #region List
        /// <summary>檢查搜尋列使用者輸入日期的值</summary>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <returns></returns>
        public static string DateCompare(DateTime SDate, DateTime EDate)
        {
            if (DateTime.Compare(SDate, EDate) < 0)
                return "true";
            else if (DateTime.Compare(SDate, EDate) == 0)
                return "Same Date";
            else
                return "false";
        }

        /// <summary>檢查DB讀取出的日期判別問卷的狀態</summary>
        /// <param name="SDate"></param>
        /// <param name="EDate"></param>
        /// <returns></returns>
        public static string CheckStatus(DateTime SDate, DateTime EDate)
        {
            DateTime Today = DateTime.Today;
            if (DateTime.Compare(Today, SDate) < 0)
            {
                return "尚未開始";
            }
            else if (DateTime.Compare(Today, SDate) >= 0 &&
                DateTime.Compare(Today, EDate) <= 0)
            {
                return "開放中";
            }
            else
                return "關閉中";
        }

        /// <summary>找出所有問卷資料用於RP控制項</summary>
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
        /// <summary>依照使用者輸入的標題、日期查找問卷資料</summary>
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
        #endregion
        #region Form

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
        /// <summary>DB內的資料不會自動過期 在這邊驗證日期及狀態 若已過期問卷狀態改為關閉</summary>
        /// <param name="guid"></param>
        /// <param name="StatusString"></param>
        public static void UpdateSuveryStatus(Guid guid, string StatusString)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                                            @"
                                                UPDATE [dbo].[SuveryMaster]
                                                   SET
                                                      [Status] = @StatusString
                                                   WHERE [SuveryID] = @Guid
                                            ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));

            list.Add(new SqlParameter("@StatusString", StatusString));
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
        #region ConfirmPage
           /// <summary>保存問卷回答</summary>
        /// <param name="guid"></param>
        /// <param name="DetailID"></param>
        /// <param name="AnswerString"></param>
        /// <param name="UserInfoString"></param>
        public static void SaveSuveryAnswer(Guid guid, int DetailID, string AnswerString, string UserInfoString)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                                            @"
                                            INSERT INTO [dbo].[AnswerDetail]
                                                       ([SuveryID]
                                                       ,[DetailID]
                                                       ,[Answer]
                                                       ,[UserInfo]
                                                       ,[CreateTime])
                                                 VALUES
                                                       (@Guid
                                                       ,@DetailID
                                                       ,@Answer
                                                       ,@UserInfo
                                                       ,@CreateTime)                  
                                            ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            list.Add(new SqlParameter("@DetailID", DetailID));
            list.Add(new SqlParameter("@Answer", AnswerString));
            list.Add(new SqlParameter("@UserInfo", UserInfoString));
            list.Add(new SqlParameter("@CreateTime", DateTime.Now));
            try
            {
                int effectRows = DBHelper.ModifyData(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        /// <summary>保存固定問題之使用者資訊</summary>
        /// <param name="guid"></param>
        /// <param name="UserInfo"></param>
        public static void SaveUserInfo(Guid guid,string UserInfo)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                                            @"
                                            INSERT INTO [dbo].[UserInfo]
                                                       ([SuveryID]
                                                       ,[UserInfo]
                                                       ,[CreateTime])
                                                 VALUES
                                                       (@SuveryID
                                                       ,@UserInfo
                                                       ,@CreateTime)              
                                            ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@SuveryID", guid));
            list.Add(new SqlParameter("@UserInfo", UserInfo));
            list.Add(new SqlParameter("@CreateTime", DateTime.Now));

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
        #region Statistic

       
        /// <summary>取得單選選項個數</summary>
        /// <param name="ItemName"></param>
        /// <returns></returns>
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
        /// <summary>取得複選選項個數</summary>
        /// <param name="ItemName"></param>
        /// <returns></returns>
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
    }
}
