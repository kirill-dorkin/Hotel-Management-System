using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Hotel_DataAccessLayer.ErrorLogs;

namespace Hotel_DataAccessLayer
{
    internal static class clsSqlHelper
    {
        public static int ExecuteScalarInt(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = clsDataAccessSettings.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null && parameters.Length > 0)
                    command.Parameters.AddRange(parameters);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return (result != null && int.TryParse(result.ToString(), out int count)) ? count : 0;
                }
                catch (Exception ex)
                {
                    clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
                    return 0;
                }
            }
        }

        public static async Task<int> ExecuteScalarIntAsync(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = clsDataAccessSettings.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null && parameters.Length > 0)
                    command.Parameters.AddRange(parameters);

                try
                {
                    await connection.OpenAsync();
                    object result = await command.ExecuteScalarAsync();
                    return (result != null && int.TryParse(result.ToString(), out int count)) ? count : 0;
                }
                catch (Exception ex)
                {
                    clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
                    return 0;
                }
            }
        }
    }
}
