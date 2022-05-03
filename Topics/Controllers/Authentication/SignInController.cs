using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Topics.Authentication;
using Topics.Repository.Models.Account;
using Topics.Repository.Models.DB;
using Topics.Services.Interfaces;

namespace Topics.Controllers
{
    [AllowAnonymous]
    public class SignInController : Controller
    {
        private IUserService userService;

        public SignInController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult Index(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "SignOut");

            ViewBag.ReturnUrl = returnUrl;
            return View(new SignInViewModel());
        }

        [HttpPost]
        public ActionResult Index(SignInViewModel signIn, string returnUrl = "")
        {
            var exist = userService.DoesExist(signIn.Username);
            if (exist.Value == null)
            {
                ModelState.AddModelError("Username", "Username doesn't exist.");
                return View(signIn);
            }
                
            var res = userService.GetUser(signIn);

            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
                return View(signIn);
            }

            var user = res.Value;
            HttpCookie authCookie = userService.GetAuthCookie(user);
            Response.Cookies.Add(authCookie);

            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

            return Redirect("/");
        }
    }
}