using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Topics.Authentication;
using Topics.DataAccess;

namespace Topics.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
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
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleName = user.Roles.Select(role => role.Name).ToList()
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
            string signUpMessage = string.Empty;

            if (ModelState.IsValid)
            {
                string username = Membership.GetUserNameByEmail(signUp.Email);
                if (!string.IsNullOrEmpty(username))
                {
                    ModelState.AddModelError("Warning Email", "Sorry: Email already exists");
                    return View(signUp);
                }

                var user = new User()
                {
                    Username = signUp.Username,
                    FirstName = signUp.FirstName,
                    LastName = signUp.LastName,
                    Email = signUp.Email,
                    Password = signUp.Password,
                    ActivationCode = Guid.NewGuid()
                };

                userService.CreateUser(user);
                signUpMessage = "Your account has been created successfully :D";
                signUpStatus = true;

                // Verification Email
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