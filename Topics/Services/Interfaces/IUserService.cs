using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Repository.Models.Account;
using Topics.Repository.Models.DB;

namespace Topics.Services.Interfaces
{
    public interface IUserService
    {
        bool CreateUser(SignUpViewModel user);
        bool ValidateUser(string username, string password);
        UserModel GetUser(string username, string password);
        UserModel GetUser(string username);
        string GetUsernameByEmail(string email);
    }
}
