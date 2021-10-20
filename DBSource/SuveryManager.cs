using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBSource
{
    public class SuveryManager
    {
        public static DataTable GetSuveryList()
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" SELECT 
                      No,
                      Guid,
                      Title,
                      InnerText,
                      StartDate,
                        EndDate,
                    Status
                    FROM SuveryList
                    ORDER BY No DESC
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
    }
}
