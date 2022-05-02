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
    public class SignUpController : Controller
    {
        private IUserService userService;
        
        public SignUpController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new SignUpViewModel());
        }

        [HttpPost]
        public ActionResult Index(SignUpViewModel signUp)
        {
            if (!ModelState.IsValid || !signUp.IsValid())
            {
                ViewBag.Message = "Something went wrong!";
                return View(signUp);
            }

            if (!signUp.Password.Equals(signUp.ConfirmPassword))
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords don't match.");
                return View(signUp);
            }

            var user = (CustomMembershipUser)Membership.GetUser(signUp.Username, false);
            if (user != null)
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(signUp);
            }

            string username = Membership.GetUserNameByEmail(signUp.Email);
            if (username != null)
            {
                ModelState.AddModelError("Email", "Email already in use");
                return View(signUp);
            }

            bool created = userService.CreateUser(signUp);
            if (!created)
            {
                ViewBag.Message = "Something went wrong, try again.";
                return View(signUp);
            }

            user = (CustomMembershipUser)Membership.GetUser(signUp.Username, false);

            if (user != null)
            {
                HttpCookie authCookie = userService.GetAuthCookie(user);
                Response.Cookies.Add(authCookie);
            }

            return Redirect("/");
        }
        
        /*[HttpGet]
        [CustomAuthorize]
        public ActionResult SetupProfile()
        {

        }*/
    }
}