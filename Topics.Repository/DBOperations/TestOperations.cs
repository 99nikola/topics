using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Repository.Models.DB;

namespace Topics.Repository.DBOperations
{
    public class TestOperations
    {
        public static List<GenderModel> GetGenders(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();

                    SqlCommand select = new SqlCommand();
                    select.Connection = connection;
                    select.CommandText = "SELECT * FROM Gender;";

                    SqlDataReader reader = select.ExecuteReader();

                    List<GenderModel> genders = new List<GenderModel>();
                    while (reader.Read())
                    {
                        genders.Add(new GenderModel(reader.GetString(0)));
                    }

                    connection.Close();

                    return genders;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
