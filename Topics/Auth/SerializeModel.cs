using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Topics.Repository.Models.DB;

namespace Topics.Auth
{
    public class SerializeModel
    {
        public string Username { get; set; }
        public RoleModel Role { get; set; }
    }
}