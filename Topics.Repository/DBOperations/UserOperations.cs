using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Topics.Repository.Models.Account;
using Topics.Repository.Models.DB;

namespace Topics.Repository.DBOperations
{
    public class UserOperations
    {
        public static bool CreateUser(SignUpViewModel user, string connectionString)
        {
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
                        "INSERT INTO AppUser (username, email, password) " +
                        "VALUES (@username, @email, @password); "
                    };
                    insert.Parameters.AddWithValue("username", user.Username);
                    insert.Parameters.AddWithValue("email", user.Email);
                    insert.Parameters.AddWithValue("password", hashedPassword);

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
                            "FROM AppUser " +
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
                            "FROM AppUser " +
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
    
    }
}
