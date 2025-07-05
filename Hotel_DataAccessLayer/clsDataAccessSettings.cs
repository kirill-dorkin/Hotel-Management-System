using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_DataAccessLayer
{
    internal static class clsDataAccessSettings
    {
        public static readonly string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
