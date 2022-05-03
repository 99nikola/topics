using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Topics.Authentication;
using Topics.Repository.Models;
using Topics.Repository.Models.Account;
using Topics.Repository.Models.DB;

namespace Topics.Services.Interfaces
{
    public interface IUserService
    {
        DBResponse CreateUser(SignUpViewModel user);
        DBResponse ValidateUser(string username, string password);
        DBResponse GetUser(string username, string password);
        DBResponse GetUser(string username);
        DBResponse GetUsernameByEmail(string email);
        DBResponse GetUserRoles(string username);
        HttpCookie GetAuthCookie(UserModel user);

    }
}
