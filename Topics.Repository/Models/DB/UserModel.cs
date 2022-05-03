using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Repository.Models.DB
{
    public class UserModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string About { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<RoleModel> Roles { get; set; }
        
    }
}
