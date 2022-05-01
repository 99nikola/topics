using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Topics.Repository.Models.DB;

namespace Topics.Authentication
{
    public class CustomMembershipUser : MembershipUser
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<RoleModel> Roles { get; set; }

        public CustomMembershipUser(UserModel user) : base("CustomMembershipProvider", user.Username, user.Username, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            Username = user.Username; 
            FirstName = user.FirstName;
            LastName = user.LastName;
            Roles = user.Roles;
        }
    }
}