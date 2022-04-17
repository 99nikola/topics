using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Repository.Models.DB;

namespace Topics.Services.Interfaces
{
    public interface ITestService
    {
        List<GenderModel> GetGenders();
    }
}
