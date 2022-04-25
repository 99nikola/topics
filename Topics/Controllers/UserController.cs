using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topics.Authentication;

namespace Topics.Controllers
{
    [CustomAuthorize(Roles = "User")]
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}