using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Topics.Authentication
{
    public class SerializeModel
    {
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}