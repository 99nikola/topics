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
            if (authTicket == null)
                return;

            SerializeModel serializeModel = JsonConvert.DeserializeObject<SerializeModel>(authTicket.UserData);
            Principal principal = new Principal(authTicket.Name);
            principal.Username = serializeModel.Username;
            principal.Roles = serializeModel.Roles.ToArray();

            HttpContext.Current.User = principal;
        }
    }
}
