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
    public partial class Statistic : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string StringGuid = Request.QueryString["ID"];
            Guid guid = Guid.Parse(StringGuid);
            var dt = GetSuveryAnswerData(guid);
            GridView gridView = new GridView();
            gridView.DataSource = dt;
            gridView.DataBind();
            this.TestArea.Controls.Add(gridView);
        }

        public static DataTable GetSuveryAnswerData(Guid guid)
        {
            {
                string connectionString = DBHelper.GetConnectionString();
                string dbCommandString =
                                             @" 
                        SELECT
	                        [dbo].[SuveryList].[Title],
	                        [dbo].[SuveryList].[Guid],
	                        [dbo].[SuveryData].[TypeOrder],
	                        [dbo].[SuveryData].[NameOrder],
	                        [dbo].[SuveryAnswer].[Answer]
                        FROM [dbo].[SuveryList]
                        JOIN[dbo].[SuveryData] ON[dbo].[SuveryList].[Guid] =  [dbo].[SuveryData].[Guid]
                        JOIN 	[dbo].[SuveryAnswer] ON [dbo].[SuveryList].[Guid] =	[dbo].[SuveryAnswer].[Guid]
                        WHERE[dbo].[SuveryList].[Guid]  = @Guid
                    
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
        }
    }
}