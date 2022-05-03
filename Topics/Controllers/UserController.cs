using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topics.Authentication;

namespace Topics.Controllers
{
    [@Authorize(Roles = "nope")]
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}