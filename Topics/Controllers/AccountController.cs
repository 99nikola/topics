using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Topics.Authentication;
using Topics.Repository.Models.Account;
using Topics.Services.Interfaces;

namespace Topics.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private IUserService userService;
        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SignIn(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
                return SignOut();

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(SignInViewModel signIn, string returnUrl = "")
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
                HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                Response.Cookies.Add(faCookie);
            }

            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(SignUpViewModel signUp)
        {
            bool signUpStatus = false;
            string signUpMessage;

            if (ModelState.IsValid)
            {
                var user = (CustomMembershipUser)Membership.GetUser(signUp.Username, false);
                Debug.WriteLine(user);
                if (user != null)
                {
                    ModelState.AddModelError("ErrorUsername", "Sorry: Username already exists");
                    return View(signUp);
                }


                userService.CreateUser(signUp);
                signUpMessage = "Your account has been created successfully :D";
                signUpStatus = true;
            } else
            {
                signUpMessage = "Something went wrong!";
            }

            ViewBag.Message = signUpMessage;
            ViewBag.Status = signUpStatus;

            return View(signUp);
        }

        public ActionResult SignOut()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn", "Account", null);
        }
    }
}