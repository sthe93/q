using QualificationVerificationWeb.Helper;
using System;
using System.Web;


namespace QualificationVerificationWeb
{
    public partial class Submitted : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
             
              if (Session["AcceptedTerms"] == null || Session["__SSuccessSubmit@!#"] == null)
              { 
                
                Session["StudentJwtToken"] = null;

                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();

                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                }

                if (Request.Cookies["AuthToken"] != null)
                {
                    Response.Cookies["AuthToken"].Value = string.Empty;
                    Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                }


                Response.Redirect("~/Default.aspx"); 
              }

  
              

            CookiesClearOnDone.ClearCookies(HttpContext.Current.Response);

        }

        protected void btnFAQ_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Faq.aspx");

            Session["AcceptedTerms"] = null;
            Session["__SSuccessSubmit@!#"] = null;
            Session["StudentJwtToken"] = null;
        }

        protected void btnUJSite_Click(object sender, EventArgs e)
        {
            Response.Redirect("https://www.uj.ac.za/");

            Session["AcceptedTerms"] = null;
            Session["__SSuccessSubmit@!#"] = null;
            Session["StudentJwtToken"] = null;
        }


    }
}