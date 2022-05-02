using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Topics.Authentication;
using Topics.Repository.Models.Account;
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
            /*if (User.Identity.IsAuthenticated)
                return SignOut();*/

            ViewBag.ReturnUrl = returnUrl;
            return View(new SignInViewModel());
        }

        [HttpPost]
        public ActionResult Index(SignInViewModel signIn, string returnUrl = "")
        {
            if (!ModelState.IsValid || !Membership.ValidateUser(signIn.Username, signIn.Password))
            {
                ModelState.AddModelError("", "Something went wrong : Username or Password invalid ^_^");
                return View(signIn);
            }

            var user = (CustomMembershipUser)Membership.GetUser(signIn.Username, false);

            if (user != null)
            {
                HttpCookie authCookie = userService.GetAuthCookie(user);
                Response.Cookies.Add(authCookie);
            }

            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

            return Redirect("/");
        }
    }
}