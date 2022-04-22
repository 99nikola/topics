using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    connection.ConnectionString = connectionString;
                    connection.Open();

                    SqlCommand insert = new SqlCommand
                    {
                        Connection = connection,

                        CommandText =
                        "INSERT INTO AppUser (username, email, password)" +
                        "VALUES (@Username, @Email, @Password)"
                    };
                    insert.Parameters.AddWithValue("Username", user.Username);
                    insert.Parameters.AddWithValue("Email", user.Email);
                    insert.Parameters.AddWithValue("Password", user.Password);

                    int rowsAffected = insert.ExecuteNonQuery();

                    connection.Close();

                    return rowsAffected == 1;
                }
                catch (SqlException ex)
                {
                    /*if (ex.Number == 2627)
                        return new Error { Message = ex.Message, Number = ex.Number };*/
                    return false;
                }
            }
        }
    }
}
