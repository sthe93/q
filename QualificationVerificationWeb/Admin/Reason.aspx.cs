using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using QualificationVerificationWeb.Admin.Content;
using QualificationVerificationWeb.Helper;
using QualificationVerificationWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Validator = QualificationVerificationWeb.ViewModels.Validator;

namespace QualificationVerificationWeb.Admin
{
    public partial class Reason : BasePage
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
                    Response.Redirect(ResolveUrl("~/Admin/Request.aspx"), false);
                    Context.ApplicationInstance.CompleteRequest();

                // Get user role from session
                string roleLabel = Session["UserRoleData"] as string ?? "";

                // Block if not authorized
                if (!roleLabel.Equals("Super Admin", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Redirect("~/Admin/Request.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }

                gvReasons.DataBind();
                await GetAllReasons();
            }
        }

        private async Task GetAllReasons()
        {
            try
            {
                // Use ApiClient
                var apiClient = new ApiClient();

                string url = "Reason/GetAllReasons";

                var response = await apiClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var reasons = JsonConvert.DeserializeObject<List<QualificationVerificationWeb.ViewModels.Reason>>(json);

                    if (reasons != null && reasons.Any())
                    {
                        // Order by CreatedOnDate descending to get the last created reason first
                        var sortedReasons = reasons
                            .OrderByDescending(r => r.CreatedOnDate)
                            .ToList();

                        gvReasons.DataSource = sortedReasons;
                        gvReasons.DataBind();

                        System.Diagnostics.Debug.WriteLine("Successfully fetched " + reasons.Count + " reasons.");
                    }
                    else
                    {
                        ShowMessage("No reasons found.", "info");
                    }
                }
            }
            catch (Exception ex)
            {
               
                ShowMessage("Error fetching reasons: " + ex.Message, "error");
            }
        }

        private bool ValidateReason(string reasonText, out string validationMessage)
        {
            validationMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(reasonText))
            {
                validationMessage = "Reason is required!";
                return false;
            }

            // Trim any leading or trailing spaces
            reasonText = reasonText.Trim();

            // Count the number of words (split by whitespace)
            var wordCount = reasonText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;

            if (wordCount > 50)
            {
                validationMessage = "Reason cannot exceed 50 words.";
                return false;
            }

            if (reasonText.Length > 150)
            {
                validationMessage = "Reason must not exceed 150 characters.";
                return false;
            }


            return true;
        }


        protected async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {   // Use ApiClient
                var apiClient = new ApiClient();

                string reasonText = txtReason.Text;

                // Call the validation method
                if (!ValidateReason(reasonText, out string validationMessage))
                {
                    ShowMessage(validationMessage, "error");
                    return; // Stop execution if validation fails
                }

                // Check if the reason already exists
                if (await IsReasonDuplicate(reasonText))
                {
                    ShowMessage("Reason with the same name already exists.", "error");
                    return;
                }

                var isChecked = chkIsActive.Checked ? true : false;

                var newReason = new QualificationVerificationWeb.ViewModels.Reason
                {
                    Reason1 = reasonText.Trim(),
                    CreatedBy = Session["AcademicRecordLoggedInUser"]?.ToString(), // Replace with actual user
                    CreatedOnDate = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    LastUpdatedBy = Session["AcademicRecordLoggedInUser"]?.ToString(), // Replace with actual user
                    IsActive = isChecked,
                    EmailInstruction = txtInstruction.Text
                };

                string url = "Reason/AddReason";
                string jsonData = JsonConvert.SerializeObject(newReason);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var updateResponse = await apiClient.PostAsync(url, content);

                if (updateResponse.IsSuccessStatusCode)
                {
                    var json1 = await updateResponse.Content.ReadAsStringAsync();
                    var validator = JsonConvert.DeserializeObject<Validator>(json1);


                    if (validator != null && validator.Status == Constants.Success)
                    {
                        ShowMessage("Reason saved successfully!", "success");
                        ClearForm();
                        await GetAllReasons();
                    }
                    else
                    {
                        ShowMessage(validator?.Reason ?? "Failed to save reason.", "error");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "error");
            }
        }


        protected async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                // Use ApiClient
                var apiClient = new ApiClient();

                string reasonText = txtReason.Text;

                // Call the validation method
                if (!ValidateReason(reasonText, out string validationMessage))
                {
                    ShowMessage(validationMessage, "error");
                    return; // Stop execution if validation fails
                }

                // Check if the reason already exists (excluding the current reason)
                if (await IsReasonDuplicate(reasonText, Convert.ToInt32(hfReasonId.Value)))
                {
                    ShowMessage("Reason with the same name already exists.", "error");
                    return;
                }

               
                var isChecked = chkIsActive.Checked ? true : false;

                var updatedReason = new QualificationVerificationWeb.ViewModels.Reason
                {
                    ReasonID = Convert.ToInt32(hfReasonId.Value),
                    Reason1 = reasonText.Trim(),
                    LastUpdated = DateTime.Now,
                    LastUpdatedBy = Session["AcademicRecordLoggedInUser"]?.ToString(), // Replace with actual user
                    IsActive = isChecked,
                    EmailInstruction = txtInstruction.Text
                };

                string url = "Reason/UpdateReason";
                string jsonData = JsonConvert.SerializeObject(updatedReason);

               var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var updateResponse = await apiClient.PostAsync(url, content);

                if (updateResponse.IsSuccessStatusCode)
                {
                    var json1 = await updateResponse.Content.ReadAsStringAsync();
                    var validator = JsonConvert.DeserializeObject<Validator>(json1);

                    if (validator != null && validator.Status == Constants.Success)
                    {
                        ShowMessage("Reason updated successfully!", "success");
                        ClearForm();
                        await GetAllReasons();
                    }
                    else
                    {
                        ShowMessage(validator?.Reason ?? "Failed to update reason.", "error");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "error");
            }
        }


        protected async void gvReasons_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditReason")
            {
                int reasonId = Convert.ToInt32(e.CommandArgument);

                await GetReasonById(reasonId);
            }
            else if (e.CommandName == "DeleteReason")
            {
                int reasonId = Convert.ToInt32(e.CommandArgument);
                await DeleteReason(reasonId);
            }
        }

        private async Task GetReasonById(int reasonId)
        {
           
            // Use ApiClient
            var apiClient = new ApiClient();


            string url = $"Reason/GetReasonById?reasonId={reasonId}" ;

            var response = await apiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var reason = JsonConvert.DeserializeObject<QualificationVerificationWeb.ViewModels.Reason>(json);



                if (reason != null)
                {
                    hfReasonId.Value = reason.ReasonID.ToString();
                    txtReason.Text = reason.Reason1;
                    chkIsActive.Checked = reason.IsActive;
                    txtInstruction.Text = reason.EmailInstruction;
                    btnSave.Visible = false;
                    btnUpdate.Visible = true;
                }
            }
        }
        private async Task<bool> IsReasonDuplicate(string reasonText, int? excludeReasonId = null)
        {
            try
            {

                // Use ApiClient
                var apiClient = new ApiClient();


                string url = "Reason/GetAllReasons";

                var response = await apiClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var reasons = JsonConvert.DeserializeObject<List<QualificationVerificationWeb.ViewModels.Reason>>(json);



                    if (reasons != null && reasons.Any())
                    {
                        // Check for duplicate reason (excluding the current reason being edited)
                        return reasons.Any(r =>
                            r.Reason1.Equals(reasonText, StringComparison.OrdinalIgnoreCase) &&
                            (!excludeReasonId.HasValue || r.ReasonID != excludeReasonId.Value));
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                ShowMessage("Error checking duplicate reason: " + ex.Message, "error");
                return false;
            }
        }

        private async Task DeleteReason(int reasonId)
        {
           
            var apiClient = new ApiClient();

            string url = $"Reason/DeleteReason?reasonId={reasonId}";

            var response = await apiClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var validator = JsonConvert.DeserializeObject<Validator>(json);


                if (validator != null && validator.Status == Constants.Success)
                {
                    ShowMessage("Reason deleted successfully!", "success");
                    await GetAllReasons();
                }
                else
                {
                    ShowMessage(validator?.Reason ?? "Failed to delete reason.", "error");
                }
            }
        }

        protected async void gvReasons_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReasons.PageIndex = e.NewPageIndex;
            await GetAllReasons();
        }

        private void ClearForm()
        {
            hfReasonId.Value = string.Empty;
            txtReason.Text = string.Empty;
            txtInstruction.Text = string.Empty;
            btnSave.Visible = true;
            btnUpdate.Visible = false;
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

    }
}