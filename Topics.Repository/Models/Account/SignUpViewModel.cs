using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Topics.Repository.Models.Account
{
    public class SignUpViewModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(FirstName) ||
                string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(ConfirmPassword))
                return false;

            return true;
        }
    }
}