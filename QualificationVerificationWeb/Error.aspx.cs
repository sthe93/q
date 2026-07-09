using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QualificationVerificationWeb
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //Session["AcceptedTerms"] = null;
            //Session["__SSuccessSubmit@!#"] = null;

            //Session.Clear();
            //Session.Abandon();
            //Session.RemoveAll();

            //if (Request.Cookies["ASP.NET_SessionId"] != null)
            //{
            //    Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
            //    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            //}

            //if (Request.Cookies["AuthToken"] != null)
            //{
            //    Response.Cookies["AuthToken"].Value = string.Empty;
            //    Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            //}

            //FormsAuthentication.SignOut();

        }
    }
}