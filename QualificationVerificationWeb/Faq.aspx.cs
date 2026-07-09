using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QualificationVerificationWeb
{
    public partial class Faq : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.close();</script>");
        }
    }
}