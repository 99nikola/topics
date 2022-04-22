using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topics.Constants;
using Topics.Models.SignUp;
using Topics.Repository.Models.DB;
using Topics.Services.Interfaces;

namespace Topics.Controllers
{
    public class SignInController : Controller
    {
        private IUserService userService;

        public SignInController(IUserService userService)
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
            UserModel user = userService.GetUser(userCredentials["username"], userCredentials["password"]);
            if (user != null)
                return Redirect(Routes.HOME);

            FormErrorModel formUser = new FormErrorModel
            {
                Username = userCredentials["username"],
                ErrorMessage = "Username/password combination is not correct."
            };

            return View(formUser);
        }
    }
}