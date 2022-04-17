using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Repository.Models.DB
{
    public class GenderModel
    {
        public GenderModel(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
