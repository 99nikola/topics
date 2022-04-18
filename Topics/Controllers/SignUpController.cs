using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topics.Repository.Models.DB;
using Topics.Services.Interfaces;

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
            if (!userCredentials["password"].Equals(userCredentials["passwordRepeat"]))
            {
                // TODO handle error
                Debug.WriteLine("Passwrods don't match!");
                return View("Index");
            }

            UserModel user = new UserModel
            {
                Username = userCredentials["username"],
                Email = userCredentials["email"],
                Password = userCredentials["password"]
            };

            bool created = userService.CreateUser(user);
            Debug.WriteLine(created);
            return View("Index");
        }
    }
}