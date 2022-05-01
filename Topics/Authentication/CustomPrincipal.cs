﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Topics.Authentication
{
    public class CustomPrincipal : IPrincipal
    {
        public int Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }


        public IIdentity Identity
        {
            get; private set;
        }

        public bool IsInRole(string role)
        {
            return Roles.Any(r => role.Contains(r));
        }

        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }
    }
}