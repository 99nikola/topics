using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Topics.Authentication;
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

        public HttpCookie GetAuthCookie(CustomMembershipUser user)
        {
            CustomSerializeModel userModel = new CustomSerializeModel()
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = user.Roles.Select(role => role.Name).ToList()
            };

            string userData = JsonConvert.SerializeObject(userModel);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
            );

            string enTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie authCookie = new HttpCookie("auth", enTicket);
            return authCookie;
        }
    }
}