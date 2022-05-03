using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Helpers;
using Topics.Repository.Models;
using Topics.Repository.Models.Account;
using Topics.Repository.Models.DB;

namespace Topics.Repository.DBOperations
{
    public class UserOperations
    {
        public static DBResponse CreateUser(SignUpViewModel user, string connectionString)
        {
            if (!user.IsValid())
                return new DBResponse() { Success = false, Message = "SignUpViewModel is invalid." };

            
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

                        CommandText = @"
                            INSERT INTO tUser (username, email, password, firstName, lastName)
                            VALUES (@username, @email, @password, @firstName, @lastName) 
                            ;"
                    };
                    insert.Parameters.AddWithValue("username", user.Username);
                    insert.Parameters.AddWithValue("email", user.Email);
                    insert.Parameters.AddWithValue("password", hashedPassword);
                    insert.Parameters.AddWithValue("firstName", user.FirstName);
                    insert.Parameters.AddWithValue("lastName", user.LastName);
                   

                    int rowsAffected = insert.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        connection.Close();
                        return new DBResponse() { Success = false, Message = "0 rows affected, something went wrong." };
                    }

                    insert.CommandText =
                        "INSERT INTO " +
                        "User_Role (username) " +
                        "VALUES (@username) ;";

                    rowsAffected = insert.ExecuteNonQuery();

                    connection.Close();

                    if (rowsAffected == 1)
                        return new DBResponse() { Success = true, Message = "User successfully inserted." };
                    else
                        return new DBResponse() { Success = false, Message = "0 rows affected, something went wrong." };
                }
                catch (SqlException ex)
                {
                    return new DBResponse() { Success = false, Message = ex.Message };
                }
            }
        }

        public static DBResponse GetUser(string username, string password, string connectionString)
        {
            DBResponse response = GetUser(username, connectionString);
            if (!response.Success)
                return response;

            UserModel user = ((DBValue<UserModel>)response).Value;

            bool verified = Crypto.VerifyHashedPassword(user.HashedPassword, password);

            if (!verified)
            {
                new DBResponse() { Success = false, Message = "Wrong password, try again." };
            }

            return response;
        }
    
        public static DBResponse GetUser(string username, string connectionString)
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
                        CommandText = @"
                            SELECT *
                            FROM tUser AS u
	                        JOIN User_Role AS ur
	                        ON u.username = ur.username
	                        WHERE u.username = @username
                            ;"
                    };

                    selectUser.Parameters.AddWithValue("username", username);

                    SqlDataReader reader = selectUser.ExecuteReader();

                    reader.Read();

                    UserModel user = new UserModel()
                    {
                        Username = reader.GetString(1),
                        Email = reader.GetString(2),
                        HashedPassword = reader.GetString(3),
                        FirstName = Utils.ConvertFromDBVal<string>(reader.GetValue(4)),
                        LastName = Utils.ConvertFromDBVal<string>(reader.GetValue(5)),
                        Avatar = Utils.ConvertFromDBVal<string>(reader.GetValue(6)),
                        About = Utils.ConvertFromDBVal<string>(reader.GetValue(7)),
                        Roles = new HashSet<RoleModel>() { new RoleModel() { Name = reader.GetString(9) } }
                    };

                    connection.Close();

                    return new DBValue<UserModel>() { Value = user, Success = true };
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return new DBResponse() { Success = false, Message = ex.Message };
                }
            }
        }
       
        public static DBResponse ValidateUser(string username, string password, string connectionString)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return new DBResponse() { Success = false, Message = "Invalid username or password." };

            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    SqlCommand selectUser = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"
                            SELECT password
                            FROM tUser
                            WHERE username = @username 
                            ;"
                    };

                    selectUser.Parameters.AddWithValue("username", username);

                    connection.Open();
                    SqlDataReader reader = selectUser.ExecuteReader();
                    reader.Read();

                    string hashedPassword = Utils.ConvertFromDBVal<string>(reader.GetValue(0));
                    connection.Close();

                    if (hashedPassword == null)
                        return new DBResponse() { Success = false, Message = "Wrong password, try again." };

                    bool isValid = Crypto.VerifyHashedPassword(hashedPassword, password);

                    return new DBValue<bool>() { Value = true };
                } catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return new DBResponse() { Success = false, Message = ex.Message };
                }
            }

        }
    
        public static DBResponse GetUsernameByEmail(string email, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();

                    SqlCommand selectUsername = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"
                            SELECT username
                            FROM tUser
                            WHERE email = @email 
                            ;"
                    };
                    selectUsername.Parameters.AddWithValue("email", email);
                    SqlDataReader reader = selectUsername.ExecuteReader();
                    reader.Read();

                    string username = reader.GetString(0);

                    connection.Close();

                    return new DBValue<string>() { Success = true, Value = username };
                } catch(Exception ex)
                {
                    return new DBResponse() { Success = false, Message = ex.Message };
                }
            }
        }

        public static DBResponse GetUserRoles(string username, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();

                    SqlCommand selectUserRoles = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"
                            SELECT roleName
                            FROM User_Role
                            WHERE username = @username;
                            ;"
                    };
                    selectUserRoles.Parameters.AddWithValue("username", username);

                    SqlDataReader reader = selectUserRoles.ExecuteReader();

                    List<string> roles = new List<string>();
                    
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }

                    connection.Close();
                    return new DBValue<string[]>() { Success = true, Value = roles.ToArray() };
                }
                catch (Exception ex)
                {
                    return new DBResponse() { Success = false, Message = ex.Message };
                }
            }
        }

    }
}
