using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Topics.Repository.DBOperations;
using Topics.Repository.Models.DB;
using Topics.Services.Interfaces;

namespace Topics.Services.Implementations
{
    public class TestService : ITestService
    {
        public List<GenderModel> GetGenders()
        {
            string connectionString = System.
                                        Configuration.
                                        ConfigurationManager.
                                        ConnectionStrings["TopicsDB"].
                                        ConnectionString;

            return TestOperations.GetGenders(connectionString);
        }
    }
}