using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topics.Authentication;

namespace Topics.Controllers
{
    public class HomeController : Controller
    {
        [CustomAuthorize(Roles = "basic")]
        public ActionResult Index()
        {
            return View();
        }
    }
}