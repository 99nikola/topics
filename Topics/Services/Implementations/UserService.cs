using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Topics.Repository.DBOperations;
using Topics.Repository.Models.DB;
using Topics.Services.Interfaces;

namespace Topics.Services.Implementations
{
    public class UserService : IUserService
    {
        
        public bool CreateUser(UserModel user)
        {
            string connectionString = System.
                                        Configuration.
                                        ConfigurationManager.
                                        ConnectionStrings["TopicsDB"].
                                        ConnectionString;
            return UserOperations.CreateUser(user, connectionString);
        }

        public UserModel GetUser(string username, string password)
        {
            string connectionString = System.
                                        Configuration.
                                        ConfigurationManager.
                                        ConnectionStrings["TopicsDB"].
                                        ConnectionString;

            return UserOperations.GetUser(username, password, connectionString);
        }
    }
}