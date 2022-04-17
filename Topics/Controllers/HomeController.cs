using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topics.Repository.Models.DB;
using Topics.Services.Implementations;
using Topics.Services.Interfaces;

namespace Topics.Controllers
{
    public class HomeController : Controller
    {
        private ITestService testService;

        public HomeController(ITestService testService)
        {
            this.testService = testService;
        }

        public ActionResult Index()
        {
            ViewBag.Genders = testService.GetGenders();
            return View();
        }
    }
}