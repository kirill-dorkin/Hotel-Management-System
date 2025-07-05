using Hotel_DataAccessLayer.ErrorLogs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_DataAccessLayer
{
    public class clsUserData
    {
        public static bool GetUserInfoByID(int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool IsFound = false;

            const string query = @"SELECT *
                            FROM Users
                            WHERE UserID = @UserID;";

            using (SqlConnection connection = clsDataAccessSettings.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", UserID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            PersonID = (int)reader["PersonID"];
                            UserName = (string)reader["UserName"];
                            Password = (string)reader["Password"];
                            IsActive = (bool)reader["IsActive"];
                        }
                        else
                        {
                            IsFound = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
                    IsFound = false;
                }
            }
            return IsFound;
        }

        public static bool GetUserInfoByUsernameAndPassword(string UserName, string Password , ref int UserID, ref int PersonID, ref bool IsActive)
        {
            bool IsFound = false;

            const string query = @"SELECT *
                            FROM Users
                            WHERE UserName = @UserName AND Password = @Password;";

            using (SqlConnection connection = clsDataAccessSettings.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@Password", Password);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;
                            UserID = (int)reader["UserID"];
                            PersonID = (int)reader["PersonID"];
                            UserName = (string)reader["UserName"];
                            Password = (string)reader["Password"];
                            IsActive = (bool)reader["IsActive"];
                        }
                        else
                        {
                            IsFound = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
                    IsFound = false;
                }
            }
            return IsFound;
        }

        public static bool IsUserExist(int UserID)
        {
            bool IsFound = false;

            SqlConnection connection = clsDataAccessSettings.CreateConnection();

            string query = @"SELECT IsFound = 1 
                             FROM Users
                             WHERE UserID = @UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            object reader = null;

            try
            {
                connection.Open();
                reader = command.ExecuteScalar();
                IsFound = (reader != null);
            }

            catch (Exception ex)
            {
                clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
            }

            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static bool IsUserExistByPersonID(int PersonID)
        {
            bool IsFound = false;

            SqlConnection connection = clsDataAccessSettings.CreateConnection();

            string query = @"SELECT IsFound = 1 
                             FROM Users
                             WHERE PersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            object reader = null;

            try
            {
                connection.Open();
                reader = command.ExecuteScalar();
                IsFound = (reader != null);
            }

            catch (Exception ex)
            {
                clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
            }

            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static bool IsUserExist(string UserName)
        {
            bool IsFound = false;

            SqlConnection connection = clsDataAccessSettings.CreateConnection();

            string query = @"SELECT IsFound = 1 
                             FROM Users
                             WHERE UserName = @UserName;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            object reader = null;

            try
            {
                connection.Open();
                reader = command.ExecuteScalar();
                IsFound = (reader != null);
            }

            catch (Exception ex)
            {
                clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
            }

            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            //this function will return the newly added UserID if it was inserted successfully
            //otherwise it will return -1 

            int UserID = -1;

            SqlConnection connection = clsDataAccessSettings.CreateConnection();

            string query = @"INSERT INTO Users (PersonID,UserName,Password,IsActive)
                            VALUES (@PersonID,@UserName,@Password,@IsActive);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);


            object InsertedRowID = 0;

            try
            {
                connection.Open();
                InsertedRowID = command.ExecuteScalar();

                //Check if the new UserID was successfully inserted
                if (InsertedRowID != null)
                {
                    try
                    {
                        UserID = Convert.ToInt32(InsertedRowID);
                    }
                    catch
                    {
                        UserID = -1;
                    }
                }

                // Set UserID to -1 to indicate failure
                else
                    UserID = -1;
            }

            catch (Exception ex)
            {
                clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
                UserID = -1;
            }

            finally
            {
                connection.Close();
            }

            return UserID;
        }

        public static bool UpdateUserInfo(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            SqlConnection connection = clsDataAccessSettings.CreateConnection();

            string query = @"UPDATE Users
                            SET 
							PersonID = @PersonID,
							UserName = @UserName,
							Password = @Password,
							IsActive = @IsActive
                            WHERE UserID = @UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);

            int rowsAffected = 0;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
            }

            finally
            {
                connection.Close();
            }

            return rowsAffected != 0;
        }

        public static bool DeleteUser(int UserID)
        {
            SqlConnection connection = clsDataAccessSettings.CreateConnection();

            string query = @"DELETE Users
                              WHERE UserID = @UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            int rowsAffected = 0;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
            }

            finally
            {
                connection.Close();
            }

            return rowsAffected != 0;
        }

        public static DataTable GetAllUsers()
        {
            SqlConnection connection = clsDataAccessSettings.CreateConnection();

            string query = @"SELECT UserID as 'User ID', Users.PersonID as 'Person ID', FirstName + ' ' + LastName as 'Full Name',
                            UserName , IsActive as 'Is Active'
                            FROM Users
                            INNER JOIN People ON People.PersonID = Users.PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = null;

            DataTable dataTable = new DataTable();

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dataTable.Load(reader);
                }
            }

            catch (Exception ex)
            {
                clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
            }

            finally
            {
                reader?.Close();
                connection.Close();
            }

            return dataTable;
        }

        public static bool UpdateUserPassword(int UserID, string NewPassword)
        {
            SqlConnection connection = clsDataAccessSettings.CreateConnection();

            string query = @"UPDATE Users
                            SET 
							Password = @NewPassword
                            WHERE UserID = @UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@NewPassword", NewPassword);

            int rowsAffected = 0;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
            }

            finally
            {
                connection.Close();
            }

            return rowsAffected != 0;
        }

        public static int GetUsersCount()
        {
            int UsersCount = 0;

            SqlConnection connection = clsDataAccessSettings.CreateConnection();

            string query = @"SELECT COUNT(UserID)
                            FROM Users;";

            SqlCommand command = new SqlCommand(query, connection);

            object reader = null;

            try
            {
                connection.Open();
                reader = command.ExecuteScalar();

                UsersCount = (int)reader;
            }

            catch (Exception ex)
            {
                clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
            }

            finally
            {
                connection.Close();
            }

            return UsersCount;
        }


    }
}
