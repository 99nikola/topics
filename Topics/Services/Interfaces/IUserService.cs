using System.Web;
using Topics.Repository.Models;
using Topics.Repository.Models.Account;
using Topics.Repository.Models.DB;

namespace Topics.Services.Interfaces
{
    public interface IUserService
    {
        DBResponse CreateUser(SignUpViewModel user);
        DBResponse ValidateUser(string username, string password);
        DBResponse<UserModel> GetUser(SignInViewModel signIn);
        DBResponse<UserModel> GetUser(string username);
        DBResponse<string> GetUsernameByEmail(string email);
        DBResponse<string[]> GetUserRoles(string username);
        DBResponse<string> DoesExist(string username, string email);
        DBResponse<string> DoesExist(string username);
        HttpCookie GetAuthCookie(UserModel user);

    }
}
