using QualificationVerificationWeb.Helper;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;

namespace QualificationVerificationWeb
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        

            if (!IsPostBack)
            {
                string csrfToken = Guid.NewGuid().ToString();
                Session["CSRFToken"] = csrfToken;
                __RequestVerificationToken.Value = csrfToken;

            }
        }

        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            // Validate CSRF token
            string sessionToken = Session["CSRFToken"] as string;
            string requestToken = __RequestVerificationToken?.Value;

            if (string.IsNullOrEmpty(sessionToken) || string.IsNullOrEmpty(requestToken) || sessionToken != requestToken)
            {
                ShowError("Your session has expired. Please refresh the page and try again.");
                Response.StatusCode = 403;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }

            // Get credentials
            string username = SanitizeInput(Request.Form["txtUsername"]);
            string password = Request.Form["txtPassword"];

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ShowError("Enter Credentials.");
                return;
            }

            try
            {  

                var apiResponse = await Helper.ApiClient.GetJsonToken(username, password);

                if (apiResponse != null && !string.IsNullOrEmpty(apiResponse.Token))
                {
                   
                    // Store JWT token in session
                    Session["JwtToken"] = apiResponse.Token;
                    Session["UserRoleData"] = apiResponse.Roles;
                    Session["CurrentLoggedInUser"] = apiResponse.Username;
                    Session["AcademicRecordLoggedInUser"] = apiResponse.Username;
                    Session["UserAppData"] = apiResponse.ApplicationId;

                    // Also create Forms Authentication ticket for compatibility
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        apiResponse.Username,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        false,
                        apiResponse.Roles,
                        FormsAuthentication.FormsCookiePath
                    );

                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);

                    // Clear CSRF token after successful login
                    Session["CSRFToken"] = null;

                    // Redirect to main page
                    Response.Redirect("~/Admin/Request.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();

                    return;
                }
                else
                {
                    ShowError("Invalid credentials.");
                }
            }
            catch (HttpException ex)
            {
                Logs.WriteErrorLog("Login error: " + ex.ToString());

                if (ex.GetHttpCode() == 401)
                {
                    ShowError("Invalid credentials.");
                }
                else
                {
                    ShowError("An error occurred during login. Please try again.");
                    
                }
            }
            catch (Exception)
            {
                ShowError("Unable to connect to authentication service. Please try again later.");
            }
        }



        private void ShowError(string message)
        {
            loginAlertMessage.Visible = true;
            lblLoginMessage.Text = message;
        }

        private string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            // Remove any characters that might be used for XSLT or XML injection
            string pattern = @"[<>""'&]|(xsl:|script|on\w+=)";
            return Regex.Replace(input, pattern, string.Empty, RegexOptions.IgnoreCase);
        }
    }

   
}