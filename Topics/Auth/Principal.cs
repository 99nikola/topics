using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Topics.Repository.Models.DB;

namespace Topics.Auth
{
    public class Principal : IPrincipal
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public RoleModel Role { get; set; }


        public IIdentity Identity
        {
            get; private set;
        }

        public bool IsInRole(string role)
        {
            return role.Contains(Role.Name);
        }

        public Principal(string username)
        {
            Identity = new GenericIdentity(username);
        }
    }
}