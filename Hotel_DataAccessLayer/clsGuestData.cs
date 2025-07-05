using Hotel_DataAccessLayer.ErrorLogs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using SqlConnection = System.Data.SQLite.SQLiteConnection;
using SqlCommand = System.Data.SQLite.SQLiteCommand;
using SqlParameter = System.Data.SQLite.SQLiteParameter;
using SqlDataReader = System.Data.SQLite.SQLiteDataReader;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_DataAccessLayer
{
    public class clsGuestData
    {
        public static bool GetGuestInfoByID(int GuestID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT * 
                            FROM Guests 
                            WHERE GuestID = @GuestID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@GuestID", GuestID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found successfully !
                    IsFound = true;
                    PersonID = (int)reader["PersonID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];

                }

                else
                {
                    // The record wasn't found !
                    IsFound = false;
                }

            }
            catch (Exception ex)
            {
                clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
                IsFound = false;
            }
            finally
            {
                reader?.Close();
                connection.Close();
            }
            return IsFound;
        }

        public static bool GetGuestInfoByPersonID(int PersonID , ref int GuestID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT * 
                            FROM Guests 
                            WHERE PersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found successfully !
                    IsFound = true;
                    GuestID = (int)reader["GuestID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];

                }

                else
                {
                    // The record wasn't found !
                    IsFound = false;
                }

            }
            catch (Exception ex)
            {
                clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
                IsFound = false;
            }
            finally
            {
                reader?.Close();
                connection.Close();
            }
            return IsFound;
        }

        public static bool IsGuestExist(int GuestID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT IsFound = 1 
                             FROM Guests
                             WHERE GuestID = @GuestID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@GuestID", GuestID);

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

        public static bool IsGuestExistByPersonID(int PersonID)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT IsFound = 1 
                             FROM Guests
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

        public static int AddNewGuest(int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            //this function will return the newly added GuestID if it was inserted successfully
            //otherwise it will return -1 

            int GuestID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"INSERT INTO Guests (PersonID,CreatedByUserID,CreatedDate)
                            VALUES (@PersonID,@CreatedByUserID,@CreatedDate);
                            SELECT last_insert_rowid();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@CreatedDate", CreatedDate);


            object InsertedRowID = 0;

            try
            {
                connection.Open();
                InsertedRowID = command.ExecuteScalar();

                //Check if the new GuestID was successfully inserted
                if (InsertedRowID != null)
                {
                    try
                    {
                        GuestID = Convert.ToInt32(InsertedRowID);
                    }
                    catch
                    {
                        GuestID = -1;
                    }
                }

                // Set GuestID to -1 to indicate failure
                else
                    GuestID = -1;
            }

            catch (Exception ex)
            {
                clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
                GuestID = -1;
            }

            finally
            {
                connection.Close();
            }

            return GuestID;
        }

        public static bool UpdateGuestInfo(int GuestID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE Guests
                            SET 
							PersonID = @PersonID,
							CreatedByUserID = @CreatedByUserID,
							CreatedDate = @CreatedDate
                            WHERE GuestID = @GuestID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@GuestID", GuestID);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

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

        public static bool DeleteGuest(int GuestID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"DELETE Guests
                              WHERE GuestID = @GuestID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@GuestID", GuestID);

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

        public static DataTable GetAllGuests()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT GuestID AS 'Guest ID' , People.PersonID AS 'Person ID' , NationalNo AS 'National No' , 
                            FirstName || ' ' || LastName AS 'Full Name', 
                            CASE 
	                            WHEN Gender = 'M' THEN 'Male'
	                            WHEN Gender = 'F' THEN 'Female'
	                            ELSE 'Unknown'
                            END AS 'Gender' ,
                            BirthDate AS 'Birth Date' , CountryName AS 'Nationality',
                            Phone , Email
                            FROM Guests
                            INNER JOIN People ON Guests.PersonID = People.PersonID
                            INNER JOIN Countries ON People.NationalityCountryID = 
                            Countries.CountryID;";

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

        public static async Task<DataTable> GetAllGuestsAsync()
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                string query = @"SELECT GuestID AS 'Guest ID' , People.PersonID AS 'Person ID' , NationalNo AS 'National No' ,
                            FirstName || ' ' || LastName AS 'Full Name',
                            CASE
                                    WHEN Gender = 'M' THEN 'Male'
                                    WHEN Gender = 'F' THEN 'Female'
                                    ELSE 'Unknown'
                            END AS 'Gender' ,
                            BirthDate AS 'Birth Date' , CountryName AS 'Nationality',
                            Phone , Email
                            FROM Guests
                            INNER JOIN People ON Guests.PersonID = People.PersonID
                            INNER JOIN Countries ON People.NationalityCountryID =
                            Countries.CountryID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    DataTable dataTable = new DataTable();

                    try
                    {
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                                dataTable.Load(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        clsGlobal.DBLogger.LogError(ex.Message, ex.GetType().FullName);
                    }

                    return dataTable;
                }
            }
        }

        public static DataTable GetAllGuestCompanions(int GuestID , int BookingID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT GuestCompanionID AS 'Guest Companion ID' , NationalNo AS 'National No' , 
                            FirstName || ' ' || LastName AS 'Full Name', 
                            CASE 
	                            WHEN Gender = 'M' THEN 'Male'
	                            WHEN Gender = 'F' THEN 'Female'
	                            ELSE 'Unknown'
                            END AS 'Gender' ,
                            BirthDate AS 'Birth Date' , CountryName AS 'Nationality',
                            Phone , Email
                            FROM GuestCompanions
                            INNER JOIN People ON GuestCompanions.PersonID = People.PersonID
                            INNER JOIN Countries ON People.NationalityCountryID = 
                            Countries.CountryID
							WHERE GuestID = @GuestID AND BookingID = @BookingID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@GuestID", GuestID);
            command.Parameters.AddWithValue("@BookingID", BookingID);

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

        public static int GetGuestsCount()
        {
            const string query = @"SELECT COUNT(GuestID) FROM Guests;";
            return clsSqlHelper.ExecuteScalarInt(query);
        }

        public static Task<int> GetGuestsCountAsync()
        {
            const string query = @"SELECT COUNT(GuestID) FROM Guests;";
            return clsSqlHelper.ExecuteScalarIntAsync(query);
        }


    }
}
