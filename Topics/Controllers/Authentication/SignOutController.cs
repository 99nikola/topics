using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Topics.Services.Interfaces;

namespace Topics.Controllers
{
    public class SignOutController : Controller
    {
        private IUserService userService;
        public SignOutController(IUserService userService)
        {
            this.userService = userService;
        }

        public ActionResult Index()
        {
            HttpCookie cookie = new HttpCookie("auth", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
            FormsAuthentication.SignOut();
            return Redirect("/signin");
        }
    }
}