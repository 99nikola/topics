using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Topics.Repository.Models.Account
{
    public class SignInViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                return false;

            return true;
        }
    }
}