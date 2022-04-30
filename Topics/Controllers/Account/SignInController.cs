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
                CustomSerializeModel userModel = new CustomSerializeModel()
                {
                    Username = user.Username,
                    /*FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleName = user.Roles.Select(role => role.Name).ToList()*/
                };

                string userData = JsonConvert.SerializeObject(userModel);
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    1, signIn.Username, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                );

                string enTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie("auth", enTicket);
                Response.Cookies.Add(faCookie);
            }

            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

            return Redirect("/");
        }
    }
}