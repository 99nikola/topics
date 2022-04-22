using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Topics.Repository.Models.DB;
using Topics.Repository.Models.Error;

namespace Topics.Repository.DBOperations
{
    public class UserOperations
    {
        public static bool CreateUser(UserModel user, string connectionString)
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
            using (SqlConnection connection = new SqlConnection(connectionString))
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

                    bool verified = Crypto.VerifyHashedPassword(reader.GetString(2), password);

                    if (!verified)
                        return null;

                    UserModel user = new UserModel
                    {
                        Username = reader.GetString(0),
                        Email = reader.GetString(1),
                        Password = reader.GetString(2),
                        DisplayName = Utils.ConvertFromDBVal<string>(reader.GetValue(3)),
                        Avatar = Utils.ConvertFromDBVal<string>(reader.GetValue(4)),
                        About = Utils.ConvertFromDBVal<string>(reader.GetValue(5))
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
    }
}
