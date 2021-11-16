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
    public partial class TryConfirmPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["ansString"] == null)//session內沒有存放資料但跳進來此頁面，跳出錯誤訊息及跳頁
            {
                Response.Write("<script type='text/javascript'> alert('發生一些錯誤 即將跳轉至列表頁');location.href = 'TryList.aspx';</script>");
                return;
            }
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);
            //Session內存放的回答字串
            string ansString = this.Session["ansString"].ToString(); 
            string[] ansStringArray = ansString.Split(',');

            string UserInfoString = this.Session["UserInfo"].ToString();
            string[] UserInfoArray = UserInfoString.Split(',');
            Label lblForUserInfo = new Label();
            lblForUserInfo.Text = $"姓名 : {UserInfoArray[0]} </br> 電話 : {UserInfoArray[1]}</br>E-Mail : {UserInfoArray[2]}</br> 年齡 : {UserInfoArray[3]} </br></br>";
            this.AnsArea.Controls.Add(lblForUserInfo);

            string SuveryStatus;
            //取得問卷基本資料
            var SuveryMasterDataRow = ForegroundMethod.GetSuveryMasterData(guid);
            //取得問題資料
            var QuestionDetailDT = ForegroundMethod. GetQuestionDetailAndItemDetail(guid); 

            if (SuveryMasterDataRow != null)
            {
                if (SuveryMasterDataRow["Status"].ToString() == "N")
                    SuveryStatus = "關閉中";
                else
                    SuveryStatus = "開放中";
                this.ltlStatusAndDate.Text = $"{SuveryStatus}</br>{SuveryMasterDataRow["StartDate"]}~{SuveryMasterDataRow["EndDate"]}";
                this.h3Title.InnerText = SuveryMasterDataRow["Title"].ToString();
                for (int i = 0; i < QuestionDetailDT.Rows.Count; i++)
                {
                    var QuestionDetailDR = QuestionDetailDT.Rows[i];
                    Label lblForConfirmMsg = new Label();

                    //string tempAnsString = ansStringArray[i];
                    //if (tempAnsString.Contains("&"))
                    //{
                    //    string[] CBLAnsString = tempAnsString.Split('&');
                    //    tempAnsString = string.Join(" ", CBLAnsString);

                    //    //string[] CBLAnsString = tempAnsString.Split('&');
                    //    //for (int j = 0; j < CBLAnsString.Length; j++)
                    //    //{
                    //    //    if (!string.IsNullOrEmpty(CBLAnsString[j]))
                    //    //    {
                    //    //        tempAnsString = string.Join(",", CBLAnsString[j]);
                    //    //    }
                    //    //}

                    //}

                    lblForConfirmMsg.Text = (i + 1).ToString() + ".  " + QuestionDetailDR["DetailTitle"].ToString() + "</br>" + ansStringArray[i] + "</br></br>";
                    this.AnsArea.Controls.Add(lblForConfirmMsg);
                }
            }
        }
        /// <summary>送出問卷填答內容</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);
            //取得問題資料
            var QuestionDetailDT = ForegroundMethod.GetQuestionDetailAndItemDetail(guid);
            //取得固定問題之使用者資料
            var UserInfoString = this.Session["UserInfo"].ToString();
            ForegroundMethod.SaveUserInfo(guid, UserInfoString);
            //取得使用者輸入問卷回覆內容
            string ansString = this.Session["ansString"].ToString();
            string[] ansStringArray = ansString.Split(',');
            for (int i = 0; i < QuestionDetailDT.Rows.Count; i++)
            {
                var QuestionDetailDR = QuestionDetailDT.Rows[i];
                int DetailID = (int)QuestionDetailDR["DetailID"];
                string AnswerString = ansStringArray[i];
                ForegroundMethod. SaveSuveryAnswer(guid, DetailID, AnswerString, UserInfoString);
            }
            Response.Redirect("TryList.aspx");
        }

        #region Method區
       
        #endregion

    }
}