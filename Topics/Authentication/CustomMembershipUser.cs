using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Topics.DataAccess;

namespace Topics.Authentication
{
    public class CustomMembershipUser : MembershipUser
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Role> Roles { get; set; }

        public CustomMembershipUser(User user) : base("CustomMembershipProvider", user.Username, user.Id, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.Id; 
            FirstName = user.FirstName;
            LastName = user.LastName;
            Roles = user.Roles;
        }
    }
}