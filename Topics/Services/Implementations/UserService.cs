using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Topics.Repository.DBOperations;
using Topics.Repository.Models.Account;
using Topics.Repository.Models.DB;
using Topics.Services.Interfaces;

namespace Topics.Services.Implementations
{
    public class UserService : IUserService
    {

        private string ConnectionString
        {
            get => System.
                    Configuration.
                    ConfigurationManager.
                    ConnectionStrings["TopicsDB"].
                    ConnectionString;
        }
        
        public bool CreateUser(SignUpViewModel user)
        {
            return UserOperations.CreateUser(user, ConnectionString);
        }

        public bool ValidateUser(string username, string password)
        {
            return UserOperations.ValidateUser(username, password, ConnectionString);
        }

        public UserModel GetUser(string username, string password)
        {
            return UserOperations.GetUser(username, password, ConnectionString);
        }

        public UserModel GetUser(string username)
        {
            return UserOperations.GetUser(username, ConnectionString);
        }

        public string GetUsernameByEmail(string email)
        {
            return UserOperations.GetUsernameByEmail(email, ConnectionString);
        }

        public string[] GetUserRoles(string username)
        {
            return UserOperations.GetUserRoles(username, ConnectionString);
        }
    }
}