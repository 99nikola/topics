using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Repository.Models.DB
{
    public class DBValue<T> : DBResponse
    {
        public T Value { get; set; }
    }

}
