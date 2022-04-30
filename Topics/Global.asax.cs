using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Topics.Authentication;

namespace Topics
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies["auth"];
            if (authCookie == null)
                return;
            
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            var serializeModel = JsonConvert.DeserializeObject<CustomSerializeModel>(authTicket.UserData);
            CustomPrincipal principal = new CustomPrincipal(authTicket.Name);
            //principal.UserId= serializeModel.UserID;
            principal.FirstName = serializeModel.FirstName;
            principal.LastName = serializeModel.LastName;
            principal.Roles = serializeModel.RoleName;

            HttpContext.Current.User = principal;
        }
    }
}
