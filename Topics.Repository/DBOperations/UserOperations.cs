using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Helpers;
using Topics.Repository.Models.Account;
using Topics.Repository.Models.DB;

namespace Topics.Repository.DBOperations
{
    public class UserOperations
    {
        public static bool CreateUser(SignUpViewModel user, string connectionString)
        {
            if (!user.IsValid())
                return false;

            Debug.WriteLine(user.FirstName + " " + user.LastName);
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    string hashedPassword = Crypto.HashPassword(user.Password);

                    connection.ConnectionString = connectionString;
                    connection.Open();

                    SqlCommand insert = new SqlCommand
                    {
                        Connection = connection,

                        CommandText =
                        "INSERT INTO tUser (username, email, password, firstName, lastName) " +
                        "VALUES (@username, @email, @password, @firstName, @lastName); "
                    };
                    insert.Parameters.AddWithValue("username", user.Username);
                    insert.Parameters.AddWithValue("email", user.Email);
                    insert.Parameters.AddWithValue("password", hashedPassword);
                    insert.Parameters.AddWithValue("firstName", user.FirstName);
                    insert.Parameters.AddWithValue("lastName", user.LastName);
                   

                    int rowsAffected = insert.ExecuteNonQuery();

                    connection.Close();

                    return rowsAffected == 1;
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public static UserModel GetUser(string username, string password, string connectionString)
        {
            UserModel user = GetUser(username, connectionString);

            bool verified = Crypto.VerifyHashedPassword(user.HashedPassword, password);

            if (!verified)
                return null;

            return user;
        }
    
        public static UserModel GetUser(string username, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();

                    SqlCommand selectUser = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText =
                            "SELECT * " +
                            "FROM tUser " +
                            "WHERE username = @username ;"
                    };

                    selectUser.Parameters.AddWithValue("username", username);

                    SqlDataReader reader = selectUser.ExecuteReader();

                    reader.Read();

                    UserModel user = new UserModel()
                    {
                        Username = reader.GetString(0),
                        Email = reader.GetString(1),
                        HashedPassword = reader.GetString(2),
                        FirstName = Utils.ConvertFromDBVal<string>(reader.GetValue(3)),
                        LastName = Utils.ConvertFromDBVal<string>(reader.GetValue(4))
                    };

                    connection.Close();

                    return user;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }
       
        public static bool ValidateUser(string username, string password, string connectionString)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    SqlCommand selectUser = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText =
                            "SELECT password " +
                            "FROM tUser " +
                            "WHERE username = @username ;"
                    };

                    selectUser.Parameters.AddWithValue("username", username);

                    connection.Open();
                    SqlDataReader reader = selectUser.ExecuteReader();
                    reader.Read();

                    string hashedPassword = Utils.ConvertFromDBVal<string>(reader.GetValue(0));
                    connection.Close();

                    if (hashedPassword == null)
                        return false;

                    bool isValid = Crypto.VerifyHashedPassword(hashedPassword, password);

                    return isValid;
                } catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return false;
                }
            }

        }
    
        public static string GetUsernameByEmail(string email, string connectionString)
        {
            try
            {

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                SqlCommand selectUsername = new SqlCommand()
                {
                    Connection = connection,
                    CommandText =
                        "SELECT username " +
                        "FROM tUser " +
                        "WHERE email = @email ;"
                };
                selectUsername.Parameters.AddWithValue("email", email);
                SqlDataReader reader = selectUsername.ExecuteReader();
                reader.Read();

                string username = reader.GetString(0);

                connection.Close();

                return username;
            }
            } catch(Exception ex)
            {
                Debug.Write(ex.Message);
                return null;
            }
        }

    }
}
