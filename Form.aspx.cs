using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBSource;
using System.Data.SqlClient;

namespace SuverySystem
{
    public partial class Form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //問題種類共六種 文字方塊共四種1.文字 2.數字 3.電郵 4.日期 第五、六種單選和複選方塊 且 必填與否兩分法 共有12種模式 這邊用數字1~8 與羅馬字ABCD 來代表這12種類的問題   
            //if (this.Session["ansString"] == null)
            //{
            if (!Page.IsPostBack)
            {

                string StringGuid = Request.QueryString["ID"];
                Guid guid = Guid.Parse(StringGuid);

                var dr = GetSuveryData(guid);

                string SuveryStatus;
                int intStatus = Convert.ToInt32(dr["Status"]);
                if (intStatus == 0)
                    SuveryStatus = "關閉中";
                else
                    SuveryStatus = "開放中";
                if (dr != null)
                {
                    this.ltlStatusAndDate.Text = $"{SuveryStatus}</br>{dr["StartDate"]}~{dr["EndDate"]}";
                    this.h3Title.InnerText = dr["Title"].ToString();
                    this.ltlInnerText.Text = dr["InnerText"].ToString();
                    string TypeOrderString = dr["TypeOrder"].ToString();
                    string NameOrderString = dr["NameOrder"].ToString();
                    string ItemNameString = dr["ItemName"].ToString();
                    string[] TypeOrderArray = TypeOrderString.Split(',');
                    string[] NameOrderArray = NameOrderString.Split(',');
                    string[] ItemNameArray = ItemNameString.Split(',');

                    for (int i = 0; i < TypeOrderArray.Length; i++)
                    {
                        int QuestionOrder = i + 1;
                        Label lblForbr = new Label();
                        lblForbr.Text = "</br>";

                        Label lblQuestionName = new Label();
                        lblQuestionName.Text = NameOrderArray[i];

                        this.QuestionArea.Controls.Add(lblQuestionName);
                        string QTString = TypeOrderArray[i];
                        string[] QTStringArray = QTString.Split('|');
                        switch (QTStringArray[0])
                        {
                            #region 文字方塊
                            case "1":
                                TextBox textBox1 = new TextBox();
                                textBox1.ID = "Q" + QuestionOrder.ToString();
                                textBox1.TextMode = TextBoxMode.SingleLine;
                                textBox1.CssClass = "Answer";
                                this.QuestionArea.Controls.Add(textBox1);
                                this.QuestionArea.Controls.Add(lblForbr);
                                break;

                            case "2":
                                TextBox textBox2 = new TextBox();
                                textBox2.ID = "Q" + QuestionOrder.ToString();
                                textBox2.TextMode = TextBoxMode.Number;
                                textBox2.CssClass = "Answer";
                                this.QuestionArea.Controls.Add(textBox2);
                                this.QuestionArea.Controls.Add(lblForbr);
                                break;
                            case "3":
                                TextBox textBox3 = new TextBox();
                                textBox3.ID = "Q" + QuestionOrder.ToString();
                                textBox3.TextMode = TextBoxMode.Email;
                                textBox3.CssClass = "Answer";
                                this.QuestionArea.Controls.Add(textBox3);
                                this.QuestionArea.Controls.Add(lblForbr);
                                break;
                            case "4":
                                TextBox textBox4 = new TextBox();
                                textBox4.ID = "Q" + QuestionOrder.ToString();
                                textBox4.TextMode = TextBoxMode.Date;
                                textBox4.CssClass = "Answer";
                                this.QuestionArea.Controls.Add(textBox4);
                                this.QuestionArea.Controls.Add(lblForbr);
                                break;
                            case "5":
                                lblQuestionName.Text += "(必填)";
                                TextBox textBox5 = new TextBox();
                                textBox5.ID = "Q" + QuestionOrder.ToString();
                                textBox5.TextMode = TextBoxMode.SingleLine;
                                textBox5.CssClass = "isRequired Answer";

                                this.QuestionArea.Controls.Add(textBox5);
                                this.QuestionArea.Controls.Add(lblForbr);
                                break;


                            case "6":
                                lblQuestionName.Text += "(必填)";
                                TextBox textBox6 = new TextBox();
                                textBox6.ID = "Q" + QuestionOrder.ToString();
                                textBox6.TextMode = TextBoxMode.Number;
                                textBox6.CssClass = "isRequired Answer";
                                this.QuestionArea.Controls.Add(textBox6);
                                this.QuestionArea.Controls.Add(lblForbr);
                                break;

                            case "7":
                                lblQuestionName.Text += "(必填)";
                                TextBox textBox7 = new TextBox();
                                textBox7.ID = "Q" + QuestionOrder.ToString();
                                textBox7.TextMode = TextBoxMode.Email;
                                textBox7.CssClass = "isRequired Answer";

                                this.QuestionArea.Controls.Add(textBox7);
                                this.QuestionArea.Controls.Add(lblForbr);
                                break;
                            case "8":
                                lblQuestionName.Text += "(必填)";
                                TextBox textBox8 = new TextBox();
                                textBox8.ID = "Q" + QuestionOrder.ToString();
                                textBox8.TextMode = TextBoxMode.Date;
                                textBox8.CssClass = "isRequired Answer";

                                this.QuestionArea.Controls.Add(textBox8);
                                this.QuestionArea.Controls.Add(lblForbr);
                                break;
                            #endregion
                            #region 單多選
                            //case "A":
                            //    RadioButtonList radioButtonList = new RadioButtonList();
                            //    radioButtonList.ID = "Q" + QuestionOrder.ToString();
                            //    for (int j = 0; j < NameOrderArray.Length; j++)
                            //    {
                            //        radioButtonList.Items.Add(NameOrderArray[j]);
                            //    }
                            //    this.QuestionArea.Controls.Add(radioButtonList);
                            //    this.QuestionArea.Controls.Add(lblForbr);
                            //    break;
                            //case "B":
                            //    CheckBoxList checkBoxList = new CheckBoxList();
                            //    checkBoxList.ID = "Q" + QuestionOrder.ToString();
                            //    for (int j = 0; j < NameOrderArray.Length; j++)
                            //    {
                            //        checkBoxList.Items.Add(NameOrderArray[j]);
                            //    }

                            //    this.QuestionArea.Controls.Add(checkBoxList);
                            //    this.QuestionArea.Controls.Add(lblForbr);
                            //    break;
                            //case "C":
                            //    for (int j = 0; j < ItemNameArray.Length; j++)
                            //    {
                            //        lblQuestionName.Text += "(必填)";
                            //        RadioButtonList radioButtonList2 = new RadioButtonList();
                            //        radioButtonList2.ID = "Q" + QuestionOrder.ToString();
                            //        radioButtonList2.CssClass = "isRequired";
                            //        string tempString = ItemNameArray[j];
                            //        string[] tempStringArray = tempString.Split('|');
                            //        for (int k = 0; k < Convert.ToInt32(tempStringArray[0]); k++)
                            //        {
                            //            radioButtonList2.Items.Add(tempStringArray[k + 1]);
                            //        }
                            //        this.QuestionArea.Controls.Add(radioButtonList2);
                            //        this.QuestionArea.Controls.Add(lblForbr);
                            //    }

                            //    //string tempString = ItemNameArray[i-1];
                            //    //string[] tempStringArray = tempString.Split('|');
                            //    //for (int k = 0; k < tempStringArray.Length; k++)
                            //    //{
                            //    //    radioButtonList2.Items.Add(tempStringArray[k]);
                            //    //}

                            //    //int count = 0;
                            //    //string tempString = ItemNameArray[count];
                            //    //string[] tempStringArray = tempString.Split('|');
                            //    //while (count < tempStringArray.Length)
                            //    //{
                            //    //    radioButtonList2.Items.Add(tempStringArray[count]);
                            //    //    count++;
                            //    //}

                            //    break;
                            //case "D":
                            //    lblQuestionName.Text += "(必填)";
                            //    CheckBoxList checkBoxList2 = new CheckBoxList();
                            //    checkBoxList2.ID = "Q" + QuestionOrder.ToString();
                            //    checkBoxList2.CssClass = "isRequired";
                            //    for (int j = 0; j < NameOrderArray.Length; j++)
                            //    {
                            //        checkBoxList2.Items.Add(NameOrderArray[j]);
                            //    }

                            //    this.QuestionArea.Controls.Add(checkBoxList2);
                            //    this.QuestionArea.Controls.Add(lblForbr);
                            //    break;
                            #endregion
                            #region 版本2
                            //case "A":
                            //    RadioButtonList radioButtonList = new RadioButtonList();
                            //    radioButtonList.ID = "Q" + QuestionOrder.ToString();
                            //    radioButtonList.CssClass = "Answer";
                            //    for (int j = 1; j < QTStringArray.Length; j++)
                            //    {
                            //        radioButtonList.Items.Add(QTStringArray[j]);
                            //    }
                            //    this.QuestionArea.Controls.Add(radioButtonList);
                            //    this.QuestionArea.Controls.Add(lblForbr);
                            //    break;
                            //case "B":
                            //    CheckBoxList checkBoxList = new CheckBoxList();
                            //    checkBoxList.ID = "Q" + QuestionOrder.ToString();
                            //    checkBoxList.CssClass = "Answer";

                            //    for (int j = 1; j < QTStringArray.Length; j++)
                            //    {
                            //        checkBoxList.Items.Add(QTStringArray[j]);
                            //    }
                            //    this.QuestionArea.Controls.Add(checkBoxList);
                            //    this.QuestionArea.Controls.Add(lblForbr);
                            //    break;
                            //case "C":
                            //    lblQuestionName.Text += "(必填)";
                            //    RadioButtonList radioButtonList2 = new RadioButtonList();
                            //    radioButtonList2.ID = "Q" + QuestionOrder.ToString();
                            //    radioButtonList2.CssClass = "isRequired Answer";
                            //    for (int j = 1; j < QTStringArray.Length; j++)
                            //    {
                            //        radioButtonList2.Items.Add(QTStringArray[j]);
                            //    }
                            //    this.QuestionArea.Controls.Add(radioButtonList2);
                            //    this.QuestionArea.Controls.Add(lblForbr);
                            //    break;
                            //case "D":
                            //    lblQuestionName.Text += "(必填)";
                            //    CheckBoxList checkBoxList2 = new CheckBoxList();
                            //    checkBoxList2.ID = "Q" + QuestionOrder.ToString();
                            //    checkBoxList2.CssClass = "isRequired Answer";
                            //    for (int j = 1; j < QTStringArray.Length; j++)
                            //    {
                            //        checkBoxList2.Items.Add(QTStringArray[j]);
                            //    }
                            //    this.QuestionArea.Controls.Add(checkBoxList2);
                            //    this.QuestionArea.Controls.Add(lblForbr);
                            //    break;
                            #endregion
                            #region 版本3
                            case "A":
                                RadioButtonList radioButtonList = new RadioButtonList();
                                radioButtonList.ID = "Q" + QuestionOrder.ToString();
                                radioButtonList.CssClass = "Answer";
                                for (int j = 1; j < QTStringArray.Length; j++)
                                {
                                    ListItem item = new ListItem();
                                    item.Attributes.Add("name", "Q" + QuestionOrder.ToString());
                                    item.Value = QTStringArray[j];
                                    item.Text = QTStringArray[j];
                                    radioButtonList.Items.Add(item);
                                }
                                this.QuestionArea.Controls.Add(radioButtonList);
                                this.QuestionArea.Controls.Add(lblForbr);
                                break;

                            case "B":
                                CheckBoxList checkBoxList = new CheckBoxList();
                                checkBoxList.ID = "Q" + QuestionOrder.ToString();
                                checkBoxList.CssClass = "Answer";

                                for (int j = 1; j < QTStringArray.Length; j++)
                                {

                                    ListItem item = new ListItem();
                                    item.Attributes.Add("name", "Q" + QuestionOrder.ToString());
                                    item.Value = QTStringArray[j];
                                    item.Text = QTStringArray[j];
                                    checkBoxList.Items.Add(item);
                                }
                                this.QuestionArea.Controls.Add(checkBoxList);
                                this.QuestionArea.Controls.Add(lblForbr);
                                break;

                            case "C":
                                lblQuestionName.Text += "(必填)";
                                RadioButtonList radioButtonList2 = new RadioButtonList();
                                radioButtonList2.ID = "Q" + QuestionOrder.ToString();
                                radioButtonList2.CssClass = "isRequired Answer";
                                for (int j = 1; j < QTStringArray.Length; j++)
                                {
                                    ListItem item = new ListItem();
                                    item.Attributes.Add("name", "Q" + QuestionOrder.ToString());
                                    item.Value = QTStringArray[j];
                                    item.Text = QTStringArray[j];
                                    radioButtonList2.Items.Add(item);
                                }
                                this.QuestionArea.Controls.Add(radioButtonList2);
                                this.QuestionArea.Controls.Add(lblForbr);
                                break;

                            case "D":
                                lblQuestionName.Text += "(必填)";
                                CheckBoxList checkBoxList2 = new CheckBoxList();
                                checkBoxList2.ID = "Q" + QuestionOrder.ToString();
                                checkBoxList2.CssClass = "isRequired Answer";
                                for (int j = 1; j < QTStringArray.Length; j++)
                                {
                                    ListItem item = new ListItem();
                                    item.Attributes.Add("name", "Q" + QuestionOrder.ToString());
                                    item.Value = QTStringArray[j];
                                    item.Text = QTStringArray[j];
                                    checkBoxList2.Items.Add(item);
                                }
                                this.QuestionArea.Controls.Add(checkBoxList2);
                                this.QuestionArea.Controls.Add(lblForbr);
                                break;
                                #endregion
                        }
                    }
                }

            }
        }
        /// <summary> 取得問卷資料</summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static DataRow GetSuveryData(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @" SELECT TOP(1) * FROM [SuverySystem].[dbo].[SuveryData]
                     WHERE Guid = @Guid
                     ORDER BY [SuverySystem].[dbo].[SuveryData].[No]
                    
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


        #region 待刪除
        //public static void CreateQuestionControl(string QuestionType, string QuestionName, bool isRequired, int QuestionOrder)
        //{
        //    switch (QuestionType)
        //    {
        //        case "1":
        //            CreateTitleLabel(QuestionName, isRequired);
        //            TextBox textBox = new TextBox();
        //            textBox.ID = "Q" + QuestionOrder.ToString();
        //            textBox.TextMode = TextBoxMode.SingleLine;
        //            if (isRequired)
        //            {
        //                textBox.CssClass = "isRequired";
        //            }
        //            break;
        //        case "2":
        //            CreateTitleLabel(QuestionName, isRequired);
        //            TextBox textBox2 = new TextBox();
        //            textBox2.ID = "Q" + QuestionOrder.ToString();
        //            textBox2.TextMode = TextBoxMode.Number;
        //            if (isRequired)
        //            {
        //                textBox2.CssClass = "isRequired";
        //            }
        //            break;
        //        case "3":
        //            CreateTitleLabel(QuestionName, isRequired);
        //            TextBox textBox3 = new TextBox();
        //            textBox3.ID = "Q" + QuestionOrder.ToString();
        //            textBox3.TextMode = TextBoxMode.Email;
        //            if (isRequired)
        //            {
        //                textBox3.CssClass = "isRequired";
        //            }
        //            break;
        //        case "4":
        //            CreateTitleLabel(QuestionName, isRequired);
        //            TextBox textBox4 = new TextBox();
        //            textBox4.ID = "Q" + QuestionOrder.ToString();
        //            textBox4.TextMode = TextBoxMode.Date;
        //            if (isRequired)
        //            {
        //                textBox4.CssClass = "isRequired";
        //            }
        //            break;
        //        case "5":
        //            CreateTitleLabel(QuestionName, isRequired);
        //            CheckBox checkBox = new CheckBox();
        //            checkBox.ID = "Q" + QuestionOrder.ToString();
        //            if (isRequired)
        //            {
        //                checkBox.CssClass = "isRequired";
        //            }
        //            break;
        //    }
        //}

        //public static void CreateTitleLabel(string QuestionName, bool isRequired)
        //{
        //    Label label = new Label();
        //    label.Text = QuestionName;
        //    if (isRequired)
        //        label.Text += " (必填)";

        //}
        //public static string GetSuveryName(Guid guid)
        //{
        //    string connectionString = DBHelper.GetConnectionString();
        //    string dbCommandString =
        //         @" SELECT [No] FROM SuveryList
        //             WHERE Guid = @Guid

        //        ";
        //    List<SqlParameter> list = new List<SqlParameter>();
        //    list.Add(new SqlParameter("@Guid", guid));
        //    try
        //    {
        //        var dr = DBHelper.ReadDataRow(connectionString, dbCommandString, list);
        //        string SuveryName = "SuveryNo" + dr["No"].ToString();
        //        return SuveryName;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog(ex);
        //        return null;
        //    }
        //}

        //public static DataRow GetSuveryData(string SuveryName)
        //{
        //    string connectionString = DBHelper.GetConnectionString();
        //    string dbCommandString =
        //       $@" SELECT * FROM {SuveryName}
        //        ";
        //    List<SqlParameter> list = new List<SqlParameter>();
        //    list.Add(new SqlParameter("@TableName", SuveryName));

        //    try
        //    {
        //        return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog(ex);
        //        return null;
        //    }
        //}

        #endregion
        /// <summary>取消按鈕</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("List.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);
            var dr = GetSuveryData(guid);
            string TypeOrderString = dr["TypeOrder"].ToString();
            string[] TypeOrderArray = TypeOrderString.Split(',');
            int QuestionCount = TypeOrderArray.Length;
            string[] AnswerArray = new string[QuestionCount];


            //for (int i = 1; i <= QuestionCount; i++)
            //{
            //    string ControlName = "Q" + i.ToString();
            //    List<string> tempAnswerList = Request.Form.GetValues($"{ControlName}").ToList();

            //    AnswerArray[i - 1] = tempAnswerList[0];
            //}

            #region 版本1

            for (int i = 1; i <= QuestionCount; i++)
            {
                string ControlName;
                List<string> tempAnswerList;
                switch (TypeOrderArray[i - 1].Split('|')[0])
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "A":
                    case "C":
                        ControlName = "Q" + i.ToString();
                        tempAnswerList = Request.Form.GetValues($"{ControlName}").ToList();

                        AnswerArray[i - 1] = tempAnswerList[0];
                        break;
                    case "B":
                    case "D":
                        for (int j = 0; j < TypeOrderArray[i - 1].Split('|').Length - 1; j++)
                        {
                            ControlName = "Q" + i.ToString() + "$" + j.ToString();
                            tempAnswerList = Request.Form.GetValues($"{ControlName}").ToList();
                            AnswerArray[i - 1] = tempAnswerList[0];
                        }
                        break;
                }

                #endregion

                string ansString = string.Join(",", AnswerArray);
                this.Session["ansString"] = ansString;

                Response.Redirect($"ConfirmPage.aspx?ID={StringGuid}");

                Response.Write($"<script>alert('{ansString}')</script>");
                }
            }
        }
    }
}