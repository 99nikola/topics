using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topics.Repository.Models.DB;
using Topics.Services.Interfaces;
using Topics.Models.SignUp;
using Topics.Constants;
using System.Web.Routing;

namespace Topics.Controllers
{
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
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection userCredentials)
        {
            FormErrorModel formUser = new FormErrorModel
            {
                Username = userCredentials["username"],
                Email = userCredentials["email"],
                ErrorMessage = "Passwords don't match!"
            };

            if (!userCredentials["password"].Equals(userCredentials["passwordRepeat"]))
            {
                return View(formUser);
            }


            UserModel user = new UserModel
            {
                Username = userCredentials["username"],
                Email = userCredentials["email"],
                Password = userCredentials["password"]
            };

            bool created = userService.CreateUser(user);
            
            if (created)
                return Redirect(Routes.HOME);

            return View();
        }
    }
}