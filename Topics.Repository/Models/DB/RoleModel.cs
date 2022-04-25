using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Topics.Repository.Models.DB;

namespace Topics.Repository.Models.DB
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserModel> Users { get; set; }
    }
}