using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace QualificationVerificationWeb.Admin
{
    public partial class AcademicDocumentMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnsureCsrfToken();

                //Safely retrieve from session or User.Identity
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var id = (FormsIdentity)HttpContext.Current.User.Identity;
                    var ticket = id.Ticket;

                    lblUserName.Text = ticket.Name;
                    lblUserRole.Text = ticket.UserData;
                }
                else
                {
                    lblUserName.Text = "Unknown";
                    lblUserRole.Text = "None";
                }
            }
        }


        private void EnsureCsrfToken()
        {
            string csrfToken = Session["CSRFToken"] as string;

            if (string.IsNullOrEmpty(csrfToken))
            {
                csrfToken = Guid.NewGuid().ToString();
                Session["CSRFToken"] = csrfToken;
            }

            __RequestVerificationToken.Value = csrfToken;
        }


        protected void btnLogout_Click(object sender, EventArgs e)
        {
            //Session["CurrentLoggedInUser"] = null;
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

            FormsAuthentication.SignOut();

            Response.Redirect("~/Admin/Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        #region Start: Getter and Setters
        public string UserNameLabel
        {
            get
            {
                return lblUserName.Text;
            }
            set
            {
                lblUserName.Text = value;
            }
        }

        public string UserRoleLabel
        {
            get
            {
                return lblUserRole.Text;
            }
            set
            {
                lblUserRole.Text = value;
            }
        }
        #endregion End: Getter and Setters

    }
}