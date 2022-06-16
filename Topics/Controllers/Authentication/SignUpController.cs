using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Topics.Auth;
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

            var exist = userService.DoesExist(signUp.Username, signUp.Email);

            if (exist.Value != null)
            {
                if (exist.Value.Equals("username"))
                    ModelState.AddModelError("Username", exist.Message);
                else
                    ModelState.AddModelError("Email", exist.Message);

                return View(signUp);
            }

            var response = userService.CreateUser(signUp);

            if (!response.Success)
            {
                ViewBag.Message = response.Message;
                return View(signUp);
            }

           
            var getUserRes = userService.GetUser(signUp.Username);
            if (getUserRes.Success)
            {   
                var user = getUserRes.Value;
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