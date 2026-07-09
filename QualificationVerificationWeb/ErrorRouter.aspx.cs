using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QualificationVerificationWeb
{
    public partial class ErrorRouter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                var originalPath = Request.Unvalidated["aspxerrorpath"]?.ToLower() ?? "";


                var currentPath = Request.Url.AbsolutePath.ToLower();

                //Prevent infinite loop
                if (currentPath.EndsWith("/admin/error.aspx") ||currentPath.EndsWith("/error.aspx"))
                {
                    return;
                }

                if (originalPath.Contains("/admin"))
                {
                    Response.Redirect("~/Admin/Error.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
                else
                {
                    Response.Redirect("~/Error.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
            }
            catch(Exception ex)
            {
                Response.Redirect("~/Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
    }
}