using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Topics.Repository.Models.DB;

namespace Topics.Repository.Models.DB
{
    public class RoleModel
    {
        public string Name { get;  set; } 

        public override bool Equals(object obj)
        {
            return obj is RoleModel model &&
                   Name == model.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}