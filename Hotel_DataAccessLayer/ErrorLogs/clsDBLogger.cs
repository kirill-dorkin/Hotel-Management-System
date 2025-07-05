using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SqlConnection = System.Data.SQLite.SQLiteConnection;
using SqlCommand = System.Data.SQLite.SQLiteCommand;
using SqlParameter = System.Data.SQLite.SQLiteParameter;
using SqlDataReader = System.Data.SQLite.SQLiteDataReader;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_DataAccessLayer.ErrorLogs
{
    public static class clsDBLogger
    {
        private static bool _LogNewError(string ErrorMessage , string ExceptionType)
        {
            const string query = @"INSERT INTO ErrorLogs (ErrorMessage,ExceptionType,OccurredDate)
                            VALUES (@ErrorMessage,@ExceptionType,@OccurredDate);";

            using (SqlConnection connection = clsDataAccessSettings.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ErrorMessage", ErrorMessage);
                command.Parameters.AddWithValue("@ExceptionType", ExceptionType);
                command.Parameters.AddWithValue("@OccurredDate", DateTime.Now);

                try
                {
                    connection.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static void LogErrorToDatabase(string ErrorMessage,string ExceptionType)
        {
            _LogNewError(ErrorMessage, ExceptionType); 
        }

    }
}
