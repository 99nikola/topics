using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Topics.Models.SignUp
{
    public class FormErrorModel
    {
        public string Username { get; set; }
        public string Email{ get; set; }
        public string ErrorMessage { get; set; }

    }
}