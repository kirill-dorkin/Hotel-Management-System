using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using SqlConnection = System.Data.SQLite.SQLiteConnection;
using SqlCommand = System.Data.SQLite.SQLiteCommand;
using SqlParameter = System.Data.SQLite.SQLiteParameter;
using SqlDataReader = System.Data.SQLite.SQLiteDataReader;
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
