﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Topics.Authentication
{
    public class CustomSerializeModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] RoleName { get; set; }
    }
}