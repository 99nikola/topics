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


            if (userService.GetUser(signUp.Username).Success)
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(signUp);
            }

            if (userService.GetUsernameByEmail(signUp.Email).Success)
            {
                ModelState.AddModelError("Email", "Email already in use");
                return View(signUp);
            }

            var createUserRes = userService.CreateUser(signUp);
            if (!createUserRes.Success)
            {
                ViewBag.Message = createUserRes.Message;
                return View(signUp);
            }
            var getUserRes = userService.GetUser(signUp.Username);
            if (getUserRes.Success)
            {   
                var user = ((DBValue<UserModel>)getUserRes).Value;
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