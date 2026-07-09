using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace QualificationVerificationWeb.Admin.Content
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (Session["CurrentLoggedInUser"] == null)
                {

                    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                    if (authCookie != null)
                    {
                        try
                        {
                            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                            if (ticket != null && !ticket.Expired)
                            {
                                Session["UserRoleData"] = ticket.UserData;
                                Session["CurrentLoggedInUser"] = ticket.Name;
                                Session["AcademicRecordLoggedInUser"] = ticket.Name;
                            }
                        }
                        catch
                        {
                            // If decryption fails, force logout
                            FormsAuthentication.SignOut();
                            Session.Clear();
                            Session.Abandon();
                            Response.Redirect("~/Admin/Login.aspx", true);
                            return;
                        }

                    }
                }
            }
            else
            {
                // User is not authenticated at all → redirect to login
                Response.Redirect("~/Admin/Login.aspx", true);
                return;
            }

        }
        public List<string> QuestionID
        {
            get { return Session["SessionGuid"] == null ? null : (List<string>)Session["SessionGuid"]; }
            set { Session["SessionGuid"] = value; }
        }

        public TableRow Row { get; set; }

        public TableCell Cell { get; set; }
        public Label Label { get; set; }


        public string LoggedInUser
        {
            get
            {
                return Session["LoggedInUser"] == null ? null : (string)Session["LoggedInUser"];
            }
            set
            {
                Session["LoggedInUser"] = value;
            }
        }

        // Set the current user in session
        public string AcademicRecordLoggedInUser
        {
            get
            {
                return Session["ApplicationUser"] == null ? null : (string)Session["ApplicationUser"];
            }
            set
            {
                Session["ApplicationUser"] = value;
            }
        }

        // Set datatime in session 
        public string AcademicRecordDatetime
        {
            get
            {
                return Session["datetime"] == null ? null : (string)Session["datetime"];
            }
            set
            {
                Session["datetime"] = value;
            }
        }


        // Set siteid in session
        public string AcademicRecordSiteId
        {
            get
            {
                return Session["siteid"] == null ? null : (string)Session["siteid"];
            }
            set
            {
                Session["siteid"] = value;
            }
        }

        public string PreviousUrl
        {
            get
            {
                return Session["PreviousUrl"] == null ? null : (string)Session["PreviousUrl"];
            }
            set
            {
                Session["PreviousUrl"] = value;
            }
        }

        public string ReportStartDate
        {
            get
            {
                return Session["ReportStartDate"] == null ? null : (string)Session["ReportStartDate"];
            }
            set
            {
                Session["ReportStartDate"] = value;
            }
        }

        public string ReportEndDate
        {
            get
            {
                return Session["ReportEndDate"] == null ? null : (string)Session["ReportEndDate"];
            }
            set
            {
                Session["ReportEndDate"] = value;
            }
        }
        public string SDocumentType
        {
            get
            {
                return Session["SDocumentType"] == null ? null : (string)Session["SDocumentType"];
            }
            set
            {
                Session["SDocumentType"] = value;
            }
        }

        public string SStudentIdentifer
        {
            get
            {
                return Session["SStudentIdentifer"] == null ? null : (string)Session["SStudentIdentifer"];
            }
            set
            {
                Session["SStudentIdentifer"] = value;
            }
        }

        public string SStudentStatus
        {
            get
            {
                return Session["SStudentStatus"] == null ? null : (string)Session["SStudentStatus"];
            }
            set
            {
                Session["SStudentStatus"] = value;
            }
        }
        public static string GetAppSettings(string name)
        {
            string retVal = ConfigurationManager.AppSettings[name];
            if (retVal == null)
            {
                throw (new System.Exception("Configuration Setting Not Found. Please Set " + name, null));
            }
            return retVal;
        }

        public string GetQueryValue(string name)
        {
            string result = Request.QueryString[name];
            return result;
        }

        public bool QStrExists(string name)
        {
            return GetQueryValue(name) != null;
        }

        // Check user session
        //public bool IsUserSessionNotTimedOut()
        //{
        //    if (QStrExists("datetime") || AcademicRecordDatetime != null)
        //    {

        //        DateTime dt = Convert.ToDateTime(HiveLogic.SecurityHelper.Decrypt(AcademicRecordDatetime, AcademicRecordSiteId));

        //        if (dt.AddHours(int.Parse(GetAppSettings("SessionTimedOut"))) > DateTime.Now)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            Session.Clear();
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public bool IsUserAuthorisedForErecruitment(String username)
        {
            bool IsAuthorised = false;

            //do AD validation

            return IsAuthorised;
        }

        public void setTabsToggle(string selectedTab)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ActiveToggle", "toggleTabClicked('" + selectedTab + "')", true);
        }


    }
}