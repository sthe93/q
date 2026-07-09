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
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QualificationVerificationWeb.Admin
{
    public partial class Approved : BasePage
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
                //Check if user is not Super Admin
                ApplyRoleVisibility();

                if (Session["SearchStartDatet"] != null)
                {
                    string txt = Session["SearchStartDatet"].ToString();

                    txtStartDate.Text = txt.ToString();
                    txtEndDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
                }
                else
                {
                    txtStartDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
                    txtEndDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
                }
                //LoadAcademicDocument();
                gvAllOrders.DataBind();

                await GetApprovedStudentOrders();

            }


            Session["SearchStartDatet"] = null;
            Session["SelectedApplicant"] = null;

        }
        
        private void ApplyRoleVisibility()
        {
            string roleLabel = Session["UserRoleData"] as string ?? "";

            bool isSuperAdmin = roleLabel.Equals("Super Admin", StringComparison.OrdinalIgnoreCase);

            tabApplicationAlert.Visible = isSuperAdmin;
            tabReason.Visible = isSuperAdmin;
            tabRecallOrder.Visible = isSuperAdmin;
        }
        public async Task GetApprovedStudentOrders()
        {
            // Use ApiClient
            var apiClient = new ApiClient();

            string documentType = null;

            if (ddlDocumentType.SelectedItem.Value != "0")
            {


                documentType = ddlDocumentType.SelectedItem.Value;
            }
            var studentId = 0;
            var studentIdentifer = !String.IsNullOrEmpty(txtStudentIdentifer.Text) ? txtStudentIdentifer.Text.Trim() : null;
            var startDate = !String.IsNullOrEmpty(txtStartDate.Text) ? txtStartDate.Text.Trim() : null;
            var endDate = !String.IsNullOrEmpty(txtEndDate.Text) ? txtEndDate.Text.Trim() : null;
            var studentStatus = "2";

            var url = $"Student/GetUnProcessed?studentId={studentId}" +
                      $"&studentIdentifer={studentIdentifer}" +
                      $"&documentTypeID={documentType}" +
                      $"&startDate={startDate}" +
                      $"&endDate={endDate}" +
                      $"&status={studentStatus}";


            var response = await apiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                gvAllOrders.DataSource = JsonConvert.DeserializeObject<List<AcademicRequestSearch>>(json);
                gvAllOrders.DataBind();
            }

        }
        public string GetSlaCssClass(object createdOnDate, object documentType)
        {
            return GetSlaCalculation(createdOnDate, documentType).CssClass;
        }

        public string GetSlaDisplayText(object createdOnDate, object documentType)
        {
            return GetSlaCalculation(createdOnDate, documentType).DisplayText;
        }

        public string GetSlaToolTip(object createdOnDate, object documentType)
        {
            return GetSlaCalculation(createdOnDate, documentType).ToolTip;
        }

        public string GetSlaExpiryDateText(object createdOnDate, object documentType)
        {
            var slaCalculation = GetSlaCalculation(createdOnDate, documentType);
            return slaCalculation.ExpiryDate.HasValue ? slaCalculation.ExpiryDate.Value.ToString("yyyy/MM/dd") : "Not available";
        }

        private SlaCalculationResult GetSlaCalculation(object createdOnDate, object documentType)
        {
            return SlaCalculator.Calculate(Convert.ToString(createdOnDate), Convert.ToString(documentType));
        }
        protected async void gvAllOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAllOrders.PageIndex = e.NewPageIndex;
            await GetApprovedStudentOrders();
        }

        protected void gvAllOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
          
        }


        public bool FeildsValidation()
        {
            if (txtStartDate.Text != "" && txtEndDate.Text != "")
            {

                if (DateTime.Parse(txtEndDate.Text) < DateTime.Parse(txtStartDate.Text))
                {
                    lblErrorMessage.Text = "* Start date must be less than End date.";
                    return false;
                }
            }

            lblErrorMessage.Text = "";

            return true;
        }
        protected async void btnSearch_Click(object sender, EventArgs e)
        {
            if (!FeildsValidation())
                return;

            await GetApprovedStudentOrders();
        }
        protected void sendEmailLink_Click(object sender, EventArgs e)
        {
            txtMessage.Text = "";
            lblErrMessage.Text = "";

            if (((LinkButton)sender).NamingContainer is GridViewRow clickedRow)
            {


                Session["StudendID"] = ((HiddenField)clickedRow.FindControl("hfStudentId")).Value;


                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1", "$('#popUpSendEmailModal').modal('show');", true);

            }
        }
        //

        protected async void sendDocLink_Click(object sender, EventArgs e)
        {
            // Use ApiClient
            var apiClient = new ApiClient();

            txtMessage.Text = "";
            lblErrMessage.Text = "";

            if (((LinkButton)sender).NamingContainer is GridViewRow clickedRow)
            {
                Session["StudendID"] = ((HiddenField)clickedRow.FindControl("hfStudentId")).Value;

                var studentId = Int32.Parse(Session["StudendID"].ToString());

                string url = $"Student/GetStudent?studentId={studentId}";

                var response = await apiClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var requestedDocs = JsonConvert.DeserializeObject<Student>(json);


                    txtEmail.Text = requestedDocs.EmailAddress;

                    Session["ElectronicCopyEmail"] = requestedDocs.EmailAddress;

                    txtEmailYourBody.Text = "Dear " + requestedDocs.FullName + " " + requestedDocs.Surname +
                                            ", \n \n Please see attached for your perusal. \n \n Regards";


                    var url1 = $"Student/GetRequestSummaryInfo?studentnum={studentId}";

                    var response1 = await apiClient.GetAsync(url1);

                    if (response1.IsSuccessStatusCode)
                    {
                        var json1 = await response1.Content.ReadAsStringAsync();

                        var results = JsonConvert.DeserializeObject<List<RequestSummaryInfoData>>(json1);

                        foreach (var item in results)
                        {
                            if (item.DocumentType.Trim().ToLower() == "academic record (modules plus results)")
                            {
                                academicRecordDiv.Visible = true;
                            }

                            if (item.DocumentType.Trim().ToLower() ==
                                "academic transcript supplement (course content including academic record)")
                            {
                                academicTranscriptSupplementDiv.Visible = true;
                            }

                            if (item.DocumentType.Trim().ToLower() == "confirmation letter (general letters)")
                            {
                                confirmationLetterDiv.Visible = true;
                            }
                        }

                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1",
                            "$('#popUpSendDocsModal').modal('show');", true);
                    }
                }
            }
        }

        //

        public string FormatIdNumber(string idNumber)
        {
            if (string.IsNullOrEmpty(idNumber) || idNumber.Length <= 6)
            {
                return idNumber; // Return as-is if too short or empty
            }

            // Show first 6 characters, encrypt the rest with asterisks
            string visiblePart = idNumber.Substring(0, 6);
            string encryptedPart = new string('*', idNumber.Length - 6);

            return visiblePart + encryptedPart;
        }

        protected async void ExportToPDF(object sender, EventArgs e)
        {
            // Use ApiClient
            var apiClient = new ApiClient();

            if (((LinkButton)sender).NamingContainer is GridViewRow clickedRow)
            {

                Session["SelectedStudentID"] = ((HiddenField)clickedRow.FindControl("hfStudentId")).Value;
                var getFilename = ((HiddenField)clickedRow.FindControl("hfReferenceNumber")).Value;

                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                string fileNameLast = getFilename + "_FacultyForm";


                List<ReportParameter> reportParams = new List<ReportParameter>();
                reportParams.Add(new ReportParameter("StudentID", Session["SelectedStudentID"].ToString()));

                // Setup the report viewer object and get the array of bytes
                var viewer = new ReportViewer();

                int snum = int.Parse(Session["SelectedStudentID"].ToString());
                viewer.ProcessingMode = ProcessingMode.Local;


                string url = $"Student/GetOrderInfoData?studentId={snum}" ;

                 var response = await apiClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var selectAcademicRequestOrderInfo = JsonConvert.DeserializeObject<List<OrderInfoData>>(json);
                    
                    

                    viewer.LocalReport.DataSources.Clear();
                    viewer.LocalReport.ReportPath = Server.MapPath("~/Admin/Reports/CorporateGovernanceform.rdl");
                    viewer.LocalReport.SetParameters(reportParams);
                    viewer.LocalReport.DataSources.Add(new ReportDataSource("GetStudentRequestOrderInfo", selectAcademicRequestOrderInfo));

                    byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


                    // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
                    Response.Buffer = true;
                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + fileNameLast + "." + extension);
                    Response.BinaryWrite(bytes); // create the file
                    Response.Flush(); // send it to the client to download
                }
            }
        }

        protected void completed_Click(object sender, EventArgs e)
        {
            if (((LinkButton)sender).NamingContainer is GridViewRow clickedRow)
            {

                Session["SelectedStudentID"] = ((HiddenField)clickedRow.FindControl("hfStudentId")).Value;

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup2", "$('#myModal').modal('show');", true);


            }


        }

        protected void view_Click(object sender, EventArgs e)
        {
            if (((LinkButton)sender).NamingContainer is GridViewRow clickedRow)
            {
                Session["SearchStartDatet"] = txtStartDate.Text;

                var x = ((HiddenField)clickedRow.FindControl("hfStudentStatus")).Value;
                Session["studStatus"] = ((HiddenField)clickedRow.FindControl("hfStudentStatus")).Value;
                Session["SelectedApplicant"] = ((HiddenField)clickedRow.FindControl("hfStudentId")).Value;
                Session["paymentMethodGet"] = ((HiddenField)clickedRow.FindControl("hfPaymentMethod")).Value;
                PreviousUrl = "~/Admin/Approved.aspx";
                Response.Redirect("~/Admin/RequestView.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected async void btnExport_Click(object sender, EventArgs e)
        {
            // Use ApiClient
            var apiClient = new ApiClient();

            string documentType = null;
            var exportFileName = DateTime.Now.ToString("dd_MMMM_yyyy");

            if (ddlDocumentType.SelectedItem.Value != "0")
            {
                documentType = ddlDocumentType.SelectedItem.Value;
            }

            var studentId = 0;
            var studentIdentifer = !String.IsNullOrEmpty(txtStudentIdentifer.Text) ? txtStudentIdentifer.Text.Trim() : null;
            var startDate = !String.IsNullOrEmpty(txtStartDate.Text) ? txtStartDate.Text.Trim() : null;
            var endDate = !String.IsNullOrEmpty(txtEndDate.Text) ? txtEndDate.Text.Trim() : null;
            var studentStatus = "2";


            var url = $"Student/GetExportData?studentId=" + studentId + 
                      $"&studentIdentifer=" + studentIdentifer + 
                      $"&documentTypeID=" + documentType + 
                      $"&startDate=" + startDate + 
                      $"&endDate=" + endDate + 
                      $"&status=" + studentStatus;

            var response = await apiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                gvExport.DataSource = JsonConvert.DeserializeObject<List<ExportData>>(json);

                gvExport.DataBind();

                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    gvExport.AllowPaging = false;
                    this.gvExport.Columns[0].Visible = true;
                    this.gvExport.Columns[1].Visible = true;
                    this.gvExport.Columns[2].Visible = true;
                    this.gvExport.Columns[3].Visible = false;
                    this.gvExport.Columns[4].Visible = true;
                    this.gvExport.Columns[5].Visible = true;
                    this.gvExport.Columns[6].Visible = true;
                    this.gvExport.Columns[7].Visible = true;
                    this.gvExport.Columns[8].Visible = true;
                    this.gvExport.Columns[9].Visible = true;
                    this.gvExport.Columns[10].Visible = true;
                    this.gvExport.Columns[11].Visible = true;
                    this.gvExport.Columns[12].Visible = true;
                    this.gvExport.Columns[13].Visible = true;
                    this.gvExport.Columns[14].Visible = true;
                    this.gvExport.Columns[15].Visible = true;
                    this.gvExport.Columns[16].Visible = true;
                    this.gvExport.Columns[17].Visible = true;
                    this.gvExport.Columns[18].Visible = true;
                    this.gvExport.Columns[19].Visible = false;
                    this.gvExport.Columns[20].Visible = false;
                    this.gvExport.Columns[21].Visible = false;
                    this.gvExport.Columns[22].Visible = false;
                    this.gvExport.Columns[23].Visible = false;

                    this.gvExport.Columns[24].Visible = false;
                    this.gvExport.Columns[25].Visible = false;
                    this.gvExport.Columns[26].Visible = true;
                    this.gvExport.Columns[27].Visible = true;

                    if (gvExport.Rows.Count <= 0)
                    {
                        gvExport.AllowPaging = false;
                        this.gvExport.Columns[0].Visible = false;
                        this.gvExport.Columns[1].Visible = false;
                        this.gvExport.Columns[2].Visible = false;
                        this.gvExport.Columns[3].Visible = false;
                        this.gvExport.Columns[4].Visible = false;
                        this.gvExport.Columns[5].Visible = false;
                        this.gvExport.Columns[6].Visible = false;
                        this.gvExport.Columns[7].Visible = false;
                        this.gvExport.Columns[8].Visible = false;
                        this.gvExport.Columns[9].Visible = false;
                        this.gvExport.Columns[10].Visible = false;
                        this.gvExport.Columns[11].Visible = false;
                        this.gvExport.Columns[12].Visible = false;
                        this.gvExport.Columns[13].Visible = false;
                        this.gvExport.Columns[14].Visible = false;
                        this.gvExport.Columns[15].Visible = false;
                        this.gvExport.Columns[16].Visible = false;
                        this.gvExport.Columns[17].Visible = false;
                        this.gvExport.Columns[18].Visible = false;
                        this.gvExport.Columns[19].Visible = false;
                        this.gvExport.Columns[20].Visible = false;
                        this.gvExport.Columns[21].Visible = false;
                        this.gvExport.Columns[22].Visible = false;
                        this.gvExport.Columns[23].Visible = false;

                        this.gvExport.Columns[24].Visible = false;
                        this.gvExport.Columns[25].Visible = false;
                        this.gvExport.Columns[26].Visible = false;
                        this.gvExport.Columns[27].Visible = false;
                        return;
                    }

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition",
                        "attachment;filename=Approved_" + exportFileName.Trim() + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    //this.BindGridView("", "", "");

                    gvExport.HeaderRow.BackColor = Color.White;

                    foreach (TableCell cell in gvExport.HeaderRow.Cells)
                    {
                        cell.BackColor = gvExport.HeaderStyle.BackColor;
                    }

                    foreach (GridViewRow row in gvExport.Rows)
                    {
                        row.BackColor = Color.White;
                        foreach (TableCell cell in row.Cells)
                        {
                            cell.CssClass = "textmode";
                        }
                    }

                    gvExport.RenderControl(hw);
                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();


                }
            }
        }

        protected async void btnAcademicRecord_Click(object sender, EventArgs e)
        {
            // Use ApiClient
            var apiClient = new ApiClient();

            Session["EmailMessageName"] = null;
            Session["EmailSubject"] = null;

            var emailType = "Academic Record";

            var url = $"Document/GetEmailTemplate?emailMessageName={emailType}";

            var response = await apiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var emailDetails = JsonConvert.DeserializeObject<EmailMessage>(json);

                Session["EmailMessageName"] = emailDetails.EmailMessageName.ToString();
                Session["EmailSubject"] = emailDetails.Subject.ToString();

                var emailMessage =
                    System.Text.RegularExpressions.Regex.Replace(emailDetails.Message.ToString(), "<[^>]*>", "\n");
                txtMessage.Text = emailMessage.Replace("\n\r\n\n", "").Replace("\n \n", "");
                


                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1",
                    "$('#popUpSendEmailModal').modal('show');", true);
            }
        }

        protected async void btnTranscript_Click(object sender, EventArgs e)
        {
            // Use ApiClient
            var apiClient = new ApiClient();

            Session["EmailMessageName"] = null;
            Session["EmailSubject"] = null;

            var emailType = "Transcript Supplement";

            string url = $"Document/GetEmailTemplate?emailMessageName={emailType}";

            var response = await apiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var emailDetails = JsonConvert.DeserializeObject<EmailMessage>(json);

                Session["EmailMessageName"] = emailDetails.EmailMessageName.ToString();
                Session["EmailSubject"] = emailDetails.Subject.ToString();

                var emailMessage =
                    System.Text.RegularExpressions.Regex.Replace(emailDetails.Message.ToString(), "<[^>]*>", "\n");
                txtMessage.Text = emailMessage.Replace("\n\r\n\n", "").Replace("\n \n", "");
              

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1",
                    "$('#popUpSendEmailModal').modal('show');", true);
            }
        }

        protected async void btnSend_Click(object sender, EventArgs e)
        {
            // Use ApiClient
            var apiClient = new ApiClient();

            lblErrMessage.Text = "";
            try
            {

                var studentEmailValidator = new ViewModels.Validator();

                var snum = int.Parse(Session["StudendID"].ToString());
              

                if (txtMessage.Text != "")
                {

                    //Student student = new Student();
                    string url = "Student/GetStudent?studentId=" + snum;

                    var response = await apiClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        var requestedUser = JsonConvert.DeserializeObject<Student>(json);

                        var dataStudent = new
                        {
                            StudentID = requestedUser.StudentID,
                            EmailAddress = requestedUser.EmailAddress
                        };
                        
                        var emailSent = new EmailSentInfo()
                            {
                                StudentID = dataStudent.StudentID,
                                EmailName = Session["EmailMessageName"] != null
                                    ? Session["EmailMessageName"].ToString()
                                    : "Corporate Governance Correspondence",
                                Subject = Session["EmailSubject"] != null
                                    ? Session["EmailSubject"].ToString()
                                    : "Corporate Governance Correspondence",
                                Message = txtMessage.Text.Replace("\r\n", "<br/>"),
                                EmailAddress = dataStudent.EmailAddress,
                                CreatedBy = Session["AcademicRecordLoggedInUser"].ToString()
                            };
                        

                        string updateUrl = "Student/InsertSendEmail";

                        string jsonString = JsonConvert.SerializeObject(emailSent);

                        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");


                        var updateResponse = await apiClient.PostAsync(updateUrl, content);

                        if (updateResponse.IsSuccessStatusCode)
                        {
                            var json1 = await updateResponse.Content.ReadAsStringAsync();

                            studentEmailValidator = JsonConvert.DeserializeObject<ViewModels.Validator>(json1);
                       
                        }

                        Session["StudendID"] = null;


                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1", "$('#popUpSendEmailModal').modal('hide');", true);
                    }
                }
                else
                {
                    lblErrMessage.Text = "Please enter message.";

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1", "$('#popUpSendEmailModal').modal('show');", true);

                }

                Session["EmailMessageName"] = null;
                Session["EmailSubject"] = null;

                txtMessage.Text = "";
            }
            catch (Exception)
            {

            }

        }

        protected async void btnLetter_Click(object sender, EventArgs e)
        {
            // Use ApiClient
            var apiClient = new ApiClient();

            Session["EmailMessageName"] = null;
            Session["EmailSubject"] = null;

            var emailType = "Confirmation Letter";

            string url = $"Document/GetEmailTemplate?emailMessageName={emailType}";

            var response = await apiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var emailDetails = JsonConvert.DeserializeObject<EmailMessage>(json);

                Session["EmailMessageName"] = emailDetails.EmailMessageName.ToString();
                Session["EmailSubject"] = emailDetails.Subject.ToString();

                var emailMessage =
                    System.Text.RegularExpressions.Regex.Replace(emailDetails.Message.ToString(), "<[^>]*>", "\n");
                txtMessage.Text = emailMessage.Replace("\n\r\n\n", "").Replace("\n \n", "");

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1",
                    "$('#popUpSendEmailModal').modal('show');", true);
            }
        }

        protected async void btnConfirm_Click(object sender, EventArgs e)
        {
            // Use ApiClient
            var apiClient = new ApiClient();

            try
            {
               
                var studId = Session["SelectedStudentID"].ToString();

                string changedBy = Session["AcademicRecordLoggedInUser"]?.ToString();

                var updatedStatus = new UpatedStatus()
                {
                    StudentID = int.Parse(studId),
                    ChangedBy = changedBy,
                    StudentStatus = 4,
                    ReasonID = 0,
                    DocumentID = 0,
                    DocumentStatus = 0,
                };

                var url = "Student/UpdateDocumentOrderStatus";
               
                var jsonString = JsonConvert.SerializeObject(updatedStatus);

                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var updateResponse = await apiClient.PostAsync(url, content);

                if (updateResponse.IsSuccessStatusCode)
                {
                    var json1 = await updateResponse.Content.ReadAsStringAsync();
                    var dd = JsonConvert.DeserializeObject<ViewModels.Validator>(json1);
                

                    if (dd.Status == Constants.Success)
                    {
                        Session["SelectedStudentID"] = null;

                    }

                }

                await GetApprovedStudentOrders();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup2", "$('#myModal').modal('hide');", true);
            }
            catch (Exception)
            {

            }

        }

        protected void btnCancelPopUp_Click(object sender, EventArgs e)
        {
            Session["EmailMessageName"] = null;
            Session["EmailSubject"] = null;

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1", "$('#popUpSendEmailModal').modal('hide');", true);


        }

        protected async void btnSendDocs_Click(object sender, EventArgs e)
        {
            // Use ApiClient
            var apiClient = new ApiClient();

            var attachments = new List<EmailAttachmentDto>();
            var transScriptsAttachment = new EmailAttachmentDto();
            var academicRecordAttachment = new EmailAttachmentDto();
            var letterAttachment = new EmailAttachmentDto();


            var email = new EmailWithAttachmentsDto();
            Byte[] academicRecordByteArray;
            Byte[] transScriptsByteArray;
            Byte[] letterByteArray;
            //btnSend_Click

            if (academicTranscriptSupplementFileUpload.HasFile)
            {
                transScriptsByteArray = GetByte(academicTranscriptSupplementFileUpload.PostedFile.InputStream);
                transScriptsAttachment.Attachment = transScriptsByteArray;
                transScriptsAttachment.AttachmentName = academicTranscriptSupplementFileUpload.FileName;
                transScriptsAttachment.Extension = Path.GetExtension(academicTranscriptSupplementFileUpload.FileName);
                attachments.Add(transScriptsAttachment);
            }
            if (academicRecordFileUpload.HasFile)
            {
                academicRecordByteArray = GetByte(academicRecordFileUpload.PostedFile.InputStream);
                academicRecordAttachment.Attachment = academicRecordByteArray;
                academicRecordAttachment.AttachmentName = academicRecordFileUpload.FileName;
                academicRecordAttachment.Extension = Path.GetExtension(academicRecordFileUpload.FileName);
                attachments.Add(academicRecordAttachment);
            }
            if (confirmationLetterFileUpload.HasFile)
            {
                letterByteArray = GetByte(confirmationLetterFileUpload.PostedFile.InputStream);
                letterAttachment.Attachment = letterByteArray;
                letterAttachment.AttachmentName = confirmationLetterFileUpload.FileName;
                letterAttachment.Extension = Path.GetExtension(confirmationLetterFileUpload.FileName);
                attachments.Add(letterAttachment);
            }
            var emails = new List<string>();
            emails.Add(txtEmail.Text);
            //emails.Add(Session["ElectronicCopyEmail"].ToString());
            var emailArray = new string[emails.Count];
            var count = 0;
            foreach (var item in emails)
            {
                emailArray[count] = item;
                count++;
            }
            email.Attachments = attachments;
            email.TO = emailArray;

            
            txtEmailYourBody.Text.Replace("\n \n", "<br/><br/>"); 
            txtEmailYourBody.Text.Replace("\n \n", "<br/><br/>");

            email.Body = txtEmailYourBody.Text;

            const string url = "Email/SendEmailWithAttachments";

            var emailSent = JsonConvert.SerializeObject(email);

            var content = new StringContent(emailSent, Encoding.UTF8, "application/json");

            var updateResponse = await apiClient.PostAsync(url, content);

            if (updateResponse.IsSuccessStatusCode)
            {
                var json1 = await updateResponse.Content.ReadAsStringAsync();
                var dd = JsonConvert.DeserializeObject<ResponseDto>(json1);
            }

        }
        private Byte[] GetByte(Stream fs)
        {
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            return bytes;
        }



        //}
    }
}