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
        public static DBResponse<string> DoesExist(string username, string connectionString)
        {
            return DoesExist(username, "", connectionString);
        }

        public static DBResponse<string> DoesExist(string username, string email, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();

                    SqlCommand select = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"
                            SELECT username, email
                            FROM [User]
                            WHERE username = @username OR email = @email
                            ;"
                    };
                    select.Parameters.AddWithValue("username", username);
                    select.Parameters.AddWithValue("email", email);

                    SqlDataReader reader = select.ExecuteReader();
                    reader.Read();

                    string _username = Utils.ConvertFromDBVal<string>(reader.GetValue(0));
                    string _email = Utils.ConvertFromDBVal<string>(reader.GetValue(1));

                    if (_username != null && username.Equals(_username))
                        return new DBResponse<string>() { Success = false, Message = "Username is already used.", Value = "username" };

                    if (_email != null && email.Equals(_email))
                        return new DBResponse<string>() { Success = false, Message = "Email is already used.", Value = "email" };

                    connection.Close();

                    return new DBResponse<string>() { Success = true, Value = null };
                } 
                catch (Exception ex)
                {
                    return new DBResponse<string>() { Success = false, Message = ex.Message };
                }
            }
        }
        
        public static DBResponse CreateUser(SignUpViewModel user, string connectionString)
        {
            if (!user.IsValid())
                return new DBResponse() { Success = false, Message = "Form is invalid.", };

            if (!user.Password.Equals(user.ConfirmPassword))
                return new DBResponse() { Success = false, Message = "Passwords don't match." };

            
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
                            INSERT INTO [User] (username, email, password, firstName, lastName, role)
                            VALUES (@username, @email, @password, @firstName, @lastName, 'basic') 
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
                        return new DBResponse() { Success = false, Message = "Something went wrong. Unable to create user." };
                    }

                    connection.Close();

                    return new DBResponse() { Success = true };
                }
                catch (SqlException ex)
                {
                    return new DBResponse() { Success = false, Message = ex.Message };
                }
            }
        }

        public static DBResponse<UserModel> GetUser(SignInViewModel signIn, string connectionString)
        {
            if (!signIn.IsValid())
                return new DBResponse<UserModel>() { Success = false, Message = "Form is invalid." };

            DBResponse<UserModel> response = GetUser(signIn.Username, connectionString);
            if (!response.Success)
                return response;

            UserModel user = response.Value;

            bool verified = Crypto.VerifyHashedPassword(user.HashedPassword, signIn.Password);

            if (!verified)
                return new DBResponse<UserModel>() { Success = false, Message = "Wrong password, try again." };

            response.Value.HashedPassword = null;
            return response;
        }
    
        public static DBResponse<UserModel> GetUser(string username, string connectionString)
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
                            FROM [User]
	                        WHERE username = @username
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
                        Role = new RoleModel() { Name =  reader.GetString(4) },
                        FirstName = Utils.ConvertFromDBVal<string>(reader.GetValue(5)),
                        LastName = Utils.ConvertFromDBVal<string>(reader.GetValue(6)),
                        Avatar = Utils.ConvertFromDBVal<string>(reader.GetValue(7)),
                        About = Utils.ConvertFromDBVal<string>(reader.GetValue(8)),
                    };

                    connection.Close();

                    return new DBResponse<UserModel>() { Value = user, Success = true };
                }
                catch (Exception ex)
                { 
                    return new DBResponse<UserModel>() { Success = false, Message = ex.Message };
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
                            FROM [User]
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

                    return new DBResponse() { Success = true };
                } catch (Exception ex)
                {
                    return new DBResponse() { Success = false, Message = ex.Message };
                }
            }

        }
    
        public static DBResponse<string> GetUsernameByEmail(string email, string connectionString)
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
                            FROM [User]
                            WHERE email = @email 
                            ;"
                    };
                    selectUsername.Parameters.AddWithValue("email", email);
                    SqlDataReader reader = selectUsername.ExecuteReader();
                    reader.Read();

                    string username = reader.GetString(0);

                    connection.Close();

                    return new DBResponse<string>() { Success = true, Value = username };
                } catch(Exception ex)
                {
                    return new DBResponse<string>() { Success = false, Message = ex.Message };
                }
            }
        }

        public static DBResponse<RoleModel> GetUserRole(string username, string connectionString)
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
                            SELECT role
                            FROM [User]
                            WHERE username = @username;
                            ;"
                    };
                    selectUserRoles.Parameters.AddWithValue("username", username);

                    SqlDataReader reader = selectUserRoles.ExecuteReader();

                    RoleModel roleModel = new RoleModel { Name = reader.GetString(0) };
                    

                    connection.Close();
                    return new DBResponse<RoleModel>() { Success = true, Value = roleModel };
                }
                catch (Exception ex)
                {
                    return new DBResponse<RoleModel>() { Success = false, Message = ex.Message };
                }
            }
        }

    }
}
