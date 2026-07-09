using DocumentFormat.OpenXml.ExtendedProperties;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace QualificationVerificationWeb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            try
            {
                // Code that runs on application startup
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Startup crash: " + ex.ToString());
                throw;
            }
  
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    FormsIdentity _identity = new FormsIdentity(authTicket);
                    string[] roles = new string[] { authTicket.UserData };

                    GenericPrincipal _principal = new GenericPrincipal(_identity, roles);
                    Context.User = _principal;

                }
            }
        }
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            var page = HttpContext.Current.Handler as System.Web.UI.Page;
            if (page != null && Context.Session != null)
            {
                page.ViewStateUserKey = Session.SessionID;
            }
        }


    }
}