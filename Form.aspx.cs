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
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);
            string SuveryName = GetSuveryName(guid);
            var dr = GetSuveryData(SuveryName);
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
                string[] TypeOrderArray = TypeOrderString.Split(',');
                string[] NameOrderArray = NameOrderString.Split(',');
                for (int i = 0; i < TypeOrderArray.Length; i++)
                {
                    
                }
          
            }
        }
        public static string GetSuveryName(Guid guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                 @" SELECT [No] FROM SuveryList
                     WHERE Guid = @Guid
                    
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@Guid", guid));
            try
            {
                var dr = DBHelper.ReadDataRow(connectionString, dbCommandString, list);
                string SuveryName = "SuveryNo" + dr["No"].ToString();
                return SuveryName;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }

        public static DataRow GetSuveryData(string SuveryName)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
               $@" SELECT * FROM {SuveryName}
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@TableName", SuveryName));

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
    }
}