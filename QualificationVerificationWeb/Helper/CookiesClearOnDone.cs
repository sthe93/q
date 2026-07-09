using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QualificationVerificationWeb.Helper
{
    public class CookiesClearOnDone
    {
        public static void ClearCookies(HttpResponse response)
        {
            // Get all existing cookies
            HttpCookieCollection cookies = HttpContext.Current.Response.Cookies;

            // Iterate through each cookie and set its expiration date to a past date
            foreach (string cookieName in cookies.AllKeys)
            {
                HttpCookie cookie = cookies[cookieName];
                cookie.Expires = DateTime.Now.AddDays(-1);
                response.Cookies.Set(cookie);
            }
        }
    }
}