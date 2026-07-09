using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using QualificationVerificationWeb.Admin.Content;
using QualificationVerificationWeb.Helper;
using QualificationVerificationWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Validator = QualificationVerificationWeb.ViewModels.Validator;

namespace QualificationVerificationWeb.Admin
{
    public partial class ApplicationAlert : BasePage
    {

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Context.User.Identity.IsAuthenticated)
                {
                    FormsIdentity id = (FormsIdentity)User.Identity;
                    FormsAuthenticationTicket ticket = id.Ticket;

                    //set session again if missing(helps restoreafter tab re open or idle.
                    if (Session["AcademicRecordLoggedInUser"] == null && Session["UserRoleData"] == null)
                    {
                        Session["AcademicRecordLoggedInUser"] = ticket.Name;
                        Session["UserRoleData"] = ticket.UserData;
                    }

                }
                else
                {
                    //redirect to login if not authenticated
                    Session.Clear();
                    Session.Abandon();
                    Response.Redirect("~/Admin/Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }

                var user = Session["AcademicRecordLoggedInUser"];

                if (AcademicRecordLoggedInUser == null && user == null)
                    Response.Redirect(ResolveUrl("~/Admin/Request.aspx"),false);
                    Context.ApplicationInstance.CompleteRequest();

                // Get user role from session
                string roleLabel = Session["UserRoleData"] as string ?? "";

                // Block if not authorized
                if (!roleLabel.Equals("Super Admin", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Redirect("~/Admin/Request.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                gvAlerts.DataBind();
                await GetAllAlerts();
            }
        }

        private async Task GetAllAlerts()
        {
            // Use ApiClient
            var apiClient = new ApiClient();

            try
            {
                string url = "ApplicationAlert/GetAllAlerts";

                var response = await apiClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var activeAlerts = JsonConvert.DeserializeObject<List<ApplicationAlert>>(json);
                   
                
                    if (activeAlerts != null && activeAlerts.Any())
                    {
                        gvAlerts.DataSource = activeAlerts;
                        gvAlerts.DataBind();
                        System.Diagnostics.Debug.WriteLine("Successfully fetched " + activeAlerts.Count + " alerts.");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No active alerts found.");
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error fetching alerts: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);
            }
        }






        private void ShowMessage(string message, string type)
        {
            string escapedMessage = System.Web.HttpUtility.JavaScriptStringEncode(message);
            string icon = type.ToLower() == "success" ? "success" : "error"; // Supports "success" or "error"
            string position = "top-end"; // Position the alert in the top-right corner
            string bgColor = type.ToLower() == "success" ? "#28a745" : "#dc3545"; // Set background color based on success or error
            string textColor = "#fff"; // Text color
            string confirmButtonColor = "#f2651c"; // Button color

            string script = $@"
        Swal.fire({{
            icon: '{icon}',
            title: '{(icon == "success" ? "Success" : "Error")}',
            text: '{escapedMessage}',
            position: '{position}',
            background: '{bgColor}',
            color: '{textColor}',
            confirmButtonColor: '{confirmButtonColor}',
            showConfirmButton: false,
            timer: 5000  // Optional: auto-close after 1.5 seconds
        }});";

            ScriptManager.RegisterStartupScript(this, GetType(), "showMessage", script, true);
        }





        protected async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Use ApiClient
                var apiClient = new ApiClient();

                // Validate Message
                string message = txtMessage.Text.Trim();

                if (string.IsNullOrEmpty(message))
                {
                    ShowMessage("Message cannot be empty.", "error");
                    return;
                }

                int wordCount = message.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
                if (wordCount > 1000)
                {
                    ShowMessage("Message cannot exceed 1000 words.", "error");
                    return;
                }

                // Validate Start Date and End Date
                DateTime startDate, endDate;
                if (!DateTime.TryParse(txtStartDate.Text.Trim(), out startDate))
                {
                    ShowMessage("Invalid Start Date format.", "error");
                    return;
                }

                if (!DateTime.TryParse(txtEndDate.Text.Trim(), out endDate))
                {
                    ShowMessage("Invalid End Date format.", "error");
                    return;
                }

                if (endDate < DateTime.Today)
                {
                    ShowMessage("End Date cannot be a past date.", "error");
                    return;
                }
                if (startDate < DateTime.Today)
                {
                    ShowMessage("Start Date cannot be a past date.", "error");
                    return;
                }
                if (startDate > endDate)
                {
                    ShowMessage("Start Date cannot be after End Date.", "error");
                    return;
                }

                // Validate Start Time and End Time
                DateTime startTime, endTime;
                if (!DateTime.TryParseExact(txtStartTime.Text.Trim(), "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime))
                {
                    ShowMessage("Invalid Start Time format. Please use HH:mm format.", "error");
                    return;
                }

                if (!DateTime.TryParseExact(txtEndTime.Text.Trim(), "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime))
                {
                    ShowMessage("Invalid End Time format. Please use HH:mm format.", "error");
                    return;
                }

                // If the dates are the same, ensure that End Time is not earlier than Start Time
                if (startDate == endDate && endTime.TimeOfDay < startTime.TimeOfDay)
                {
                    ShowMessage("End Time cannot be earlier than Start Time on the same day.", "error");
                    return;
                }

                // Proceed with saving the alert
                string startTimeStr = startTime.ToString("HH:mm");
                string endTimeStr = endTime.ToString("HH:mm");
                string createdBy = Session["AcademicRecordLoggedInUser"]?.ToString(); // Replace with actual user from session or authentication
                DateTime createdDate = DateTime.Now; // Current timestamp
                bool isActive = chkIsActive.Checked;

                // Construct the JSON object dynamically
                var alertData = new
                {
                    AlertId = 0, // Assuming this is a new alert
                    Message = message,
                    StartDate = startDate.ToString("yyyy/MM/dd"),
                    StartTime = startTimeStr,
                    EndDate = endDate.ToString("yyyy/MM/dd"), // Ensure consistent date format
                    EndTime = endTimeStr,
                    CreatedBy = createdBy,
                    CreatedDate = createdDate.ToString("yyyy-MM-ddTHH:mm:ss"), // ISO 8601 format
                    ModifiedBy = (string)null,
                    ModifiedDate = (string)null,
                    IsActive = isActive
                };

                 
                // Call the API to add the alert
                var url1 =  "ApplicationAlert/AddAlert";
                
                // Serialize the object to JSON
                var jsonData = JsonConvert.SerializeObject(alertData);

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
              
                var updateResponse = await apiClient.PostAsync(url1, content);

                if (updateResponse.IsSuccessStatusCode)
                {
                    var json1 = await updateResponse.Content.ReadAsStringAsync();
                    var validator = JsonConvert.DeserializeObject<Validator>(json1);



                    if (validator != null && validator.Status == Constants.Success)
                    {
                        ShowMessage("Alert saved successfully!", "success");
                        ClearForm();
                        await GetAllAlerts(); // Refresh the grid view
                    }
                    else
                    {
                        // Display API returned message if available
                        string errorMessage = validator != null && !string.IsNullOrEmpty(validator.Reason)
                            ? validator.Reason
                            : "Failed to save alert. Please try again.";

                        ShowMessage(errorMessage, "error");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception and show an error message
                ShowMessage(ex.Message, "error");
            }
        }


        protected async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Use ApiClient
                var apiClient = new ApiClient();

                // Extract values from the form controls
                int alertId = Convert.ToInt32(hfAlertId.Value); // Get the AlertId from the hidden field
                string startDate = txtStartDate.Text.Trim();
                string startTime = txtStartTime.Text.Trim();
                string endDate = txtEndDate.Text.Trim();
                string endTime = txtEndTime.Text.Trim();
                string message = txtMessage.Text.Trim();
                string modifiedBy = Session["AcademicRecordLoggedInUser"]?.ToString(); // Replace with actual user from session or authentication
                DateTime modifiedDate = DateTime.Now; // Current timestamp
                bool isActive = chkIsActive.Checked;

                // Construct the JSON object dynamically
                var alertData = new
                {
                    AlertId = alertId, // Use the existing AlertId for update
                    Message = message,
                    StartDate = startDate,
                    StartTime = startTime,
                    EndDate = endDate,
                    EndTime = endTime,
                    CreatedBy = (string)null, // Not needed for update
                    CreatedDate = (string)null, // Not needed for update
                    ModifiedBy = modifiedBy,
                    ModifiedDate = modifiedDate.ToString("yyyy-MM-ddTHH:mm:ss"), // ISO 8601 format
                    IsActive = isActive
                };

  

                // Call the API to update the alert
                string url = "ApplicationAlert/UpdateAlert";
             
                // Serialize the object to JSON
                var jsonData = JsonConvert.SerializeObject(alertData);

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var updateResponse = await apiClient.PostAsync(url, content);

                if (updateResponse.IsSuccessStatusCode)
                {
                    var json1 = await updateResponse.Content.ReadAsStringAsync();
                    var validator = JsonConvert.DeserializeObject<Validator>(json1);



                    if (validator != null && validator.Status == Constants.Success)
                    {
                        ShowMessage("Alert saved successfully!", "success");
                        ClearForm();
                        await GetAllAlerts(); // Refresh the grid view
                    }
                    else
                    {
                        // Display API returned message if available
                        string errorMessage = validator != null && !string.IsNullOrEmpty(validator.Reason)
                            ? validator.Reason
                            : "Failed to save alert. Please try again.";

                        ShowMessage(errorMessage, "error"); // Call ShowMessage with "error"
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception and show an error message
                ShowMessage(ex.Message, "error");
            }
        }


        protected async void gvAlerts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateAlert")
            {
                int alertId = Convert.ToInt32(e.CommandArgument);
                await GetAlertById(alertId);
            }
            else if (e.CommandName == "DeleteAlert")
            {
                int alertId = Convert.ToInt32(e.CommandArgument);
                await DeleteAlert(alertId);
            }
        }

        private async Task GetAlertById(int alertId)
        {
          
            // Use ApiClient
            var apiClient = new ApiClient();

          
                string url = $"ApplicationAlert/GetAlertById?alertId={alertId}"; 

                var response = await apiClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var alert = JsonConvert.DeserializeObject<ApplicationAlert>(json);



                    if (alert != null)
                    {
                        hfAlertId.Value = alert.AlertId.ToString();
                        txtStartDate.Text = alert.StartDate.ToString("yyyy/MM/dd");
                        txtStartTime.Text = alert.StartTime.ToString();
                        txtEndDate.Text = alert.EndDate.ToString("yyyy/MM/dd");
                        txtEndTime.Text = alert.EndTime.ToString();
                        txtMessage.Text = alert.Message;
                        chkIsActive.Checked = alert.IsActive;

                        btnSave.Visible = false;
                        btnUpdate.Visible = true;
                    }
                }
        }

        private async Task DeleteAlert(int alertId)
        {
            var apiClient = new ApiClient();

            string url = $"ApplicationAlert/DeleteAlert?alertId={alertId}";

            var response = await apiClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var alertDel = JsonConvert.DeserializeObject<bool>(json);

                if (alertDel)
                {
                    await GetAllAlerts();
                }
            }
        }

        protected async void gvAlerts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAlerts.PageIndex = e.NewPageIndex;
            await GetAllAlerts();
        }

        private void ClearForm()
        {
            hfAlertId.Value = string.Empty;
            txtStartDate.Text = string.Empty;
            txtStartTime.Text = string.Empty;
            txtEndDate.Text = string.Empty;
            txtEndTime.Text = string.Empty;
            txtMessage.Text = string.Empty;

            btnSave.Visible = true;
            btnUpdate.Visible = false;
        }
    }

    public partial class ApplicationAlert
    {
        public int AlertId { get; set; }
        public string Message { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.TimeSpan StartTime { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.TimeSpan EndTime { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}