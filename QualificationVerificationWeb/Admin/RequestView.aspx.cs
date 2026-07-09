using Newtonsoft.Json;
using QualificationVerificationWeb.Admin.Content;
using QualificationVerificationWeb.Helper;
using QualificationVerificationWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace QualificationVerificationWeb.Admin
{
    public partial class RequestView : BasePage
    {


        protected async void Page_Load(object sender, EventArgs e)
        {

            // Check for token
            if (Session["JwtToken"] == null)
            {
                Session.Clear();
                Session.Abandon();
                Response.Redirect("~/Admin/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            var user = Session["AcademicRecordLoggedInUser"];
           
            if (AcademicRecordLoggedInUser == null && user == null)
                Response.Redirect("~/Admin/Request.aspx", false);
                Context.ApplicationInstance.CompleteRequest();


            if (Session["SelectedApplicant"] != null && !IsPostBack)
            {
                await GetOrderInformation();
                ddlRequestStatus.Enabled = false;
                Div2.Visible = false;

                var stdStatus = Session["studStatus"].ToString();

                var paymentMethodGet = Session["paymentMethodGet"].ToString();

                if(paymentMethodGet == "Online Payment") 
                {
                   
                    onlineGet.Visible = true;
                    divRefPaygate.Visible = true;
                } 
                else 
                {
                  
                    onlineGet.Visible = false;
                    divRefPaygate.Visible = false;
                }

                if (stdStatus == "3" && lblDeclineReasons.InnerText != "")
                {
                    Div2.Visible = true;
                }

                if (stdStatus != "0")
                {
                    lblProvePay.Visible = true;
                    if (int.Parse(Session["IDorPassportDocumentID"].ToString()) != 0)
                    {
                        lblIDorPassportCorrect.Visible = true;
                    }
                    lblRequestStatus.Visible = true;
                    lblDeclineReasons.Visible = true;
                    btnBack.Visible = true;
                    Div10.Visible = true;

                }
                else
                {
                    ddlProofPayment.Visible = true;

                    if (int.Parse(Session["IDorPassportDocumentID"].ToString()) != 0)
                    {
                        ddlProofIDorPassport.Visible = true;
                    }
                    ddlDeclineReasons.Visible = true;
                    ddlRequestStatus.Visible = true;
                    btnSave.Visible = true;
                    btnClose.Visible = true;
                    Div10.Visible = false;
                }

                if (stdStatus == "5" ||stdStatus == "9")
                {
                    await LoadRequestStatusForFinancialBlock();
                    ddlRequestStatus.Enabled = true;
                    lblRequestStatus.Visible = false;
                    ddlRequestStatus.Visible = true;
                    btnBack.Visible = false;
                    btnSave.Visible = true;
                    btnClose.Visible = true;
                    Div2.Visible = false;
                }


                if (stdStatus == "2" || stdStatus == "3"|| stdStatus == "4")
                {
                    if (Session["UserRoleData"].ToString() == "Super Admin")
                    {
                        Div12.Visible = true;
                    }
                    else
                    {
                        Div12.Visible = false;
                    }
                }
                else
                {
                    Div12.Visible = false;
                }


            }
            else if (Session["SelectedApplicant"] == null)
            {
                Response.Redirect("~/Admin/Request.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }

        }

        public async Task GetOrderInformation()
        {
            int stud = int.Parse(Session["SelectedApplicant"].ToString());
            var stdStatus = Session["studStatus"].ToString();



            var endpoint = $"Student/GetRequestView?studentId={stud}" +  
                           $"&studentIdentifer={null}" + 
                           $"&documentTypeID={null}" + 
                           $"&startDate={null}" + 
                           $"&endDate={null}" + 
                           $"&status={stdStatus}";

            // Use ApiClient
            var apiClient = new ApiClient();
            var response = await apiClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var currentStudent = JsonConvert.DeserializeObject<List<AcademicRequestReport>>(json);

                if (currentStudent.Count != 0)
                {
                    foreach (var item in currentStudent)
                    {
                        lblDateCreated.InnerText = item.CreatedOnDate.ToString();
                        lblSurname.InnerText = item.Surname.ToString();
                        lblFullName.InnerText = item.FullName.ToString();
                        lblStudentNumber.InnerText = item.StudentNumber.ToString();
                        lblIdentifier.InnerText = item.StudentIDNumber.ToString();
                        lblEmail.InnerText = item.EmailAddress == null ? " " : item.EmailAddress.ToString();
                        lblDocumentType.InnerText = item.DocumentType.ToString();
                        lblMethod.InnerText = item.DeliveryType.ToString();
                        lblReference.InnerText = item.ReferenceNumber.ToString();
                        lblTransactionDate.InnerText =
                        item.PaymentMethod == "Online Payment" ? "Transaction Date" : "DateCreated";
                        //lblComplexAddress.InnerText = item.ComplexAddress.ToString();
                        //lblStreetAddress.InnerText = item.StreetAddress.ToString();
                        //lblSuburb.InnerText = item.Suburb.ToString();
                        //lblCity.InnerText = item.City.ToString();
                        //lblCode.InnerText = item.Code.ToString();
                        //lblCountry.InnerText = item.Country.ToString();
                        lblTotalCost.InnerText = "R " + item.TotalAmount;
                        lblAmountPaid.InnerText = "R " + item.TotalAmount;
                        lblProvePay.InnerText = item.ProofOfPayment.ToString();
                        lblRequestStatus.InnerText = item.RequestStatus.ToString();
                        lblDeclineReasons.InnerText = item.Reason.ToString();
                        lblMaidenSurname.InnerText = item.MaidenSurname.ToString();
                        lblSpecialInstructions.InnerText = item.CourierInstructions.ToString();
                        Session["StudentID"] = item.StudentID.ToString();
                        Session["DocumentID"] = item.DocumentID.ToString();
                        Session["DocumentGuidID"] = item.DocumentGuidID.ToString();
                        Session["SpecialinstructionsDocumentID"] = item.SpecialinstructionsDocumentID.ToString();
                        Session["SpecialinstructionsDocumentGuidId"] = item.SpecialinstructionsDocumentGuidId.ToString();
                        lblDuplicateOf.InnerText = item.DuplicateOf.ToString();
                        Session["ReferenceNumber"] = item.ReferenceNumber.ToString();
                        lblTnCselected.InnerText = item.ThirdParty_or_Student.ToString();
                        Session["IDorPassportDocumentID"] = item.IDorPassportDocumentID.ToString();
                        Session["IDorPassportDocumentGuidID"] = item.IDorPassportDocumentGuidID.ToString();
                        lblIDorPassportCorrect.InnerText = item.IDorPassportStatus.ToString();
                        lblPaymentMethod.InnerText = item.PaymentMethod;
                        lblPaymentStatus.InnerText = item.PaymentStatus;
                        lblReferencePaygate.InnerText = item.PaygateReference;
                        lblTransactionDate.InnerText = item.TransactionDate;
                    }

                    var sDocumentId = "0";

                    sDocumentId = Session["SpecialinstructionsDocumentID"].ToString();
                    divReportBack.Visible = true;


                    if (int.Parse(Session["DocumentID"].ToString()) != 0)
                    {
                        var setValue = 1;

                        iframeDocument.Src = @"ShowOrderDocument.aspx?DocumetId=" + Session["DocumentGuidID"];

                        RadioButtonList1.Enabled = true;
                        RadioButtonList1.SelectedValue = setValue.ToString();

                        lblPaymentStatusGet.InnerText = "Proof of payment status : ";
                        ddlProofPayment.Items.FindByValue("0").Text = "Select proof of payment status";
                    }
                    else
                    {
                        var setValue = 2;


                        iframeDocument.Src = @"ShowOrderDocument.aspx?DocumetId=" + Session["IDorPassportDocumentGuidID"];

                        RadioButtonList1.Enabled = false;
                        RadioButtonList1.SelectedValue = setValue.ToString();

                        lblPaymentStatusGet.InnerText = "Online payment status : ";
                        ddlProofPayment.Items.FindByValue("0").Text = "Select online payment status";
                    }

                    if (lblDuplicateOf.InnerText != "")
                    {
                        lblDuplicateOf.Visible = true;
                        lblDuplicateOf2.Visible = true;
                    }
                    else
                    {
                        lblDuplicateOf.Visible = false;
                        lblDuplicateOf2.Visible = false;
                    }

                    if (sDocumentId != "0")
                    {
                        lbtnInstructionDowmload.Visible = true;
                    }
                    else
                    {
                        lbtnInstructionDowmload.Visible = false;
                    }
                }
            }
        }

        public async Task LoadRequestStatusForFinancialBlock()
        {
            lblErrorMessage.Text = "";

            int num = 1;

            var url = "Student/GetStudentStatusForFinancialBlock?financialBlock="+ num;

            // Use ApiClient
            var apiClient = new ApiClient();
            var response = await apiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                ddlRequestStatus.DataSource = JsonConvert.DeserializeObject<List<StudentStatus>>(json);
                ddlRequestStatus.DataTextField = "StatusDesc";
                ddlRequestStatus.DataValueField = "StatusId";
                ddlRequestStatus.DataBind();
                ddlRequestStatus.Items.Insert(0, new ListItem("Select Request Status", "0"));
            }
        }
        public async Task LoadRequestStatusForRecall()
        {
            lblErrorMessage.Text = "";

            var url2 =  "Student/GetStudentStatus?selectInd=" + 3;

            // Use ApiClient
            var apiClient = new ApiClient();
            var response = await apiClient.GetAsync(url2);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                ddlRequestStatus.DataSource = JsonConvert.DeserializeObject<List<StudentStatus>>(json);
                ddlRequestStatus.DataTextField = "StatusDesc";
                ddlRequestStatus.DataValueField = "StatusId";
                ddlRequestStatus.DataBind();
                ddlRequestStatus.Items.Insert(0, new ListItem("Select Request Status", "0"));
                ddlRequestStatus.Items[0].Enabled = false;
                ddlRequestStatus.SelectedIndex = 1;
            }
        }

        public async Task LoadRequestStatus()
        {
            lblErrorMessage.Text = "";

            int sendValue = 0;

            if (int.Parse(Session["IDorPassportDocumentID"].ToString()) != 0)
            {

                if (ddlProofPayment.SelectedValue == "2" || ddlProofIDorPassport.SelectedValue == "2")
                {
                    sendValue = 2;
                }
                else
                {
                    sendValue = 1;
                }

            }
            else
            {

                if (ddlProofPayment.SelectedValue == "2")
                {
                    sendValue = 2;
                }
                else
                {
                    sendValue = 1;
                }
            }

            var url2 = $"Student/GetStudentStatus?selectInd={sendValue}";

            // Use ApiClient
            var apiClient = new ApiClient();
            var response = await apiClient.GetAsync(url2);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                ddlRequestStatus.DataSource = JsonConvert.DeserializeObject<List<StudentStatus>>(json);
                ddlRequestStatus.DataTextField = "StatusDesc";
                ddlRequestStatus.DataValueField = "StatusId";
                ddlRequestStatus.DataBind();
                ddlRequestStatus.Items.Insert(0, new ListItem("Select Request Status", "0"));
            }
        }

        public async Task LoadReason()
        {
            lblErrorMessage.Text = "";

            const string url2 = "Student/GetDeclineReasons";

            // Use ApiClient
            var apiClient = new ApiClient();
            var response = await apiClient.GetAsync(url2);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                ddlDeclineReasons.DataSource = JsonConvert.DeserializeObject<List<DeclineReasons>>(json);
                ddlDeclineReasons.DataTextField = "Reason";
                ddlDeclineReasons.DataValueField = "ReasonID";
                ddlDeclineReasons.DataBind();
                ddlDeclineReasons.Items.Insert(0, new ListItem("Select Decline Reasons ", "0"));
            }
        }

        public async Task LoadReasonForRecall()
        {
            lblErrorMessage.Text = "";

            const string url2 = "Student/GetDeclineReasonsForRecall";


            // Use ApiClient
            var apiClient = new ApiClient();
            var response = await apiClient.GetAsync(url2);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                ddlDeclineReasons.DataSource = JsonConvert.DeserializeObject<List<DeclineReasons>>(json);
                ddlDeclineReasons.DataTextField = "Reason";
                ddlDeclineReasons.DataValueField = "ReasonID";
                ddlDeclineReasons.DataBind();
                ddlDeclineReasons.Items.Insert(0, new ListItem("Select Decline Reasons ", "0"));
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PreviousUrl) && (PreviousUrl == "~/Admin/FinancialBlock.aspx"|| PreviousUrl == "~/Admin/RecallOrder.aspx"))
            { Response.Redirect(PreviousUrl); }
            else { Response.Redirect("~/Admin/Request.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }



            Session["SelectedApplicant"] = null;

        }

        protected async void ddlProofPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(Session["IDorPassportDocumentID"].ToString()) != 0)
            {
                if (ddlProofPayment.SelectedValue != "0" && ddlProofIDorPassport.SelectedValue != "0")
                {
                    await LoadRequestStatus();
                    ddlRequestStatus.Enabled = true;

                }
                else
                {
                    ddlRequestStatus.Enabled = false;
                }
            }
            else
            {
                if (ddlProofPayment.SelectedValue != "0")
                {
                    await LoadRequestStatus();
                    ddlRequestStatus.Enabled = true;

                }
                else
                {
                    ddlRequestStatus.Enabled = false;
                }
            }
        }

        protected async void ddlProofIDorPassport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProofIDorPassport.SelectedValue != "0" && ddlProofPayment.SelectedValue != "0")
            {
                await LoadRequestStatus();
                ddlRequestStatus.Enabled = true;

            }
            else
            {
                ddlRequestStatus.Enabled = false;
            }
        }

        protected async void ddlRequestStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string studStatus = Session["studStatus"]?.ToString();

            if (
                ddlRequestStatus.SelectedValue == "3" && new[] { "0", "5", "9" }.Contains(studStatus))

            {
                await LoadReason();
                ddlDeclineReasons.Visible = true;
                lblDeclineReasons.Visible = false;
                Div2.Visible = true;
            }
            else
            {
                Div2.Visible = false;
            }
        }
        public bool FeildsValidation()
        {

            if (ddlRequestStatus.SelectedValue != "9")
            {
                string studStatus = Session["studStatus"]?.ToString();

                if (ddlProofPayment.SelectedValue == "0" && !new[] {"5", "9" }.Contains(studStatus))
                {
                    lblErrorMessage.Text = "* Please select proof of payment status.";

                    return false;
                }
            }


            if (ddlRequestStatus.SelectedValue == "0")
            {
                lblErrorMessage.Text = "* Please select request status.";

                return false;
            }

            if ((ddlRequestStatus.SelectedValue == "3" || ddlRequestStatus.SelectedValue == "9") && ddlDeclineReasons.SelectedValue == "0" && Session["studStatus"].ToString() != "5")
            {
                lblErrorMessage.Text = "* Please select decline reasons.";

                return false;
            }


            return true;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            lblSaveMessage.InnerText = "";
            lblSaveMessage.Visible = true;

             int studentStatus = 0;
            studentStatus = int.Parse(ddlRequestStatus.SelectedItem.Value);
            
            if (!FeildsValidation())
                return;

           

            if (studentStatus == 2)
            {
                lblSaveMessage.InnerText = "Please confirm that all is in order and the request can be updated to approved.";
            }

            if (studentStatus == 3)
            {
                lblSaveMessage.InnerText = "Please confirm that all is not in order and the request can be updated to declined.";
            }

            if (studentStatus == 5)
            {
                lblSaveMessage.InnerText = "Please confirm that the requester have outstanding fees.";
            }
            if (studentStatus == 9)
            {
                lblSaveMessage.InnerText = "Please confirm that all is not in order and the request can be updated to recall.";
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup2", "$('#mySaveModal').modal('show');", true);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session["SelectedApplicant"] = null;

            if (!string.IsNullOrEmpty(PreviousUrl))
                Response.Redirect(PreviousUrl, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        public async Task BindGrid()
        {
            var studentId = int.Parse(Session["StudentID"].ToString());

            string url =  $"Student/GetRequestSummaryInfo?studentnum={studentId}" ;

            // Use ApiClient
            var apiClient = new ApiClient();
            var response = await apiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                gvSummary.DataSource = JsonConvert.DeserializeObject<List<RequestSummaryInfoData>>(json);

                gvSummary.DataBind();
            }
        }

        protected async void lbtnAddressAndRequest_Click(object sender, EventArgs e)
        {

            await BindGrid();

            gvSentEmail.Visible = false;
            sentEmail.Visible = false;


            viewEmail.Visible = false;
            lblMessage.Visible = false;

            gvSummary.Visible = true;
            modalHead.Visible = true;



            popupModal.Update();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1", "$('#myModal').modal('show');", true);

        }

        public void Downloadpdf(byte[] pdf)
        {
            var refNum = Session["ReferenceNumber"].ToString();
            string fileExtension = "";

            Byte[] bytes = pdf;
            Response.Clear();
            bool isImage = IsValidImage(pdf);
            Response.Clear();
            if (isImage)
            {
                Response.ContentType = "image/jpeg";
                fileExtension = ".jpg";
            }
            else
            {
                Response.ContentType = "application/pdf";
                fileExtension = ".pdf";
            }


            Response.AddHeader("content-disposition", "attachment;filename=" + refNum.ToString().Trim() + "_SpecialInstructionsForm" + fileExtension.ToString().Trim());
            Response.Buffer = true;
            Response.BinaryWrite(pdf);
            Response.Flush();
        }


        public static bool IsValidImage(byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                    System.Drawing.Image.FromStream(ms);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }
        protected async void lbtnInstructionDowmload_Click(object sender, EventArgs e)
        {
            try
            {

               
                    if (!string.IsNullOrEmpty(Session["SpecialinstructionsDocumentID"].ToString()))
                    {
                          
                            var documentId = Guid.Parse(Session["SpecialinstructionsDocumentGuidID"].ToString());

                
                          var endpoint = $"Document/GetDocumentByPropertyId?documentID={documentId}";


                            // Use ApiClient
                            var apiClient = new ApiClient();
                            var rest = await apiClient.GetAsync(endpoint);
               

                            if (rest.IsSuccessStatusCode)
                            {
                                var doc = rest.Content.ReadAsAsync<Document>().Result;

                                 Downloadpdf(doc.DocumentFile);

                                //else return field could not be found
                            }
                      
                    }
               
            }

            catch (Exception ex)
            {
                Logs.WriteErrorLog("lbtnInstructionDowmload_Click : " + ex.Message);
            }
        }


        protected async void lbtnEmails_Click(object sender, EventArgs e)
        {
            var studentId = int.Parse(Session["StudentID"].ToString());

            var endpoint = $"Student/GetSentEmailInfo?studentnum={studentId}";

            var apiClient = new ApiClient();
            var response = await apiClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                gvSentEmail.DataSource = JsonConvert.DeserializeObject<List<GetSentEmailData>>(json);
                gvSentEmail.DataBind();

                gvSummary.Visible = false;
                modalHead.Visible = false;
                viewEmail.Visible = false;
                lblMessage.Visible = false;

                gvSentEmail.Visible = true;
                sentEmail.Visible = true;

                // CRITICAL: Update BOTH UpdatePanels
                popupModal.Update();
                popUpSendEmail.Update();  // Add this line - forces refresh of the email modal

                // Use a timeout to ensure DOM is ready
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showEmailModal",
                    "setTimeout(function() { $('#myModal').modal('show'); }, 200);", true);
            }
        }

        protected async void gvSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSummary.PageIndex = e.NewPageIndex;
            await BindGrid();
        }
        protected async void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {             
                // Use ApiClient
                var apiClient = new ApiClient();
                var validator = new ViewModels.Validator();

                // Safely parse Session values
                var studentid = Session["StudentID"] != null ? int.Parse(Session["StudentID"].ToString()) : 0;
                var studStatus = Session["studStatus"]?.ToString() ?? "0";

                // Decline Reason logic
                var declineReasonId = 0;
                if ((ddlRequestStatus.SelectedValue == "3"|| ddlRequestStatus.SelectedValue == "9") && studStatus != "5" &&
                    ddlDeclineReasons.SelectedItem != null)
                {
                    declineReasonId = int.Parse(ddlDeclineReasons.SelectedItem.Value);
                }

                var upatedStatus = new UpatedStatus()
                {
                    StudentID = studentid,
                    ChangedBy = Session["AcademicRecordLoggedInUser"]?.ToString() ?? string.Empty,
                    StudentStatus = ddlRequestStatus.SelectedItem != null ?
                                  int.Parse(ddlRequestStatus.SelectedItem.Value) : 0,
                    ReasonID = declineReasonId,
                    DocumentID = Session["DocumentID"] != null ?
                                int.Parse(Session["DocumentID"].ToString()) : 0,
                    DocumentStatus = ddlProofPayment.SelectedItem != null ?
                                   int.Parse(ddlProofPayment.SelectedItem.Value) : 0,
                    IDorPassportDocumentID = Session["IDorPassportDocumentID"] != null ?
                                           int.Parse(Session["IDorPassportDocumentID"].ToString()) : 0,
                    IDorPassportDocumentStatus = (Session["IDorPassportDocumentID"] != null &&
                                                int.Parse(Session["IDorPassportDocumentID"].ToString()) != 0) ?
                                               (ddlProofIDorPassport.SelectedItem != null ?
                                                int.Parse(ddlProofIDorPassport.SelectedItem.Value) : 0) : 0,
                };

                // ---- Update Status API Call ----
                var updateUrl = "Student/UpdateDocumentOrderStatus";

                var updateJson = JsonConvert.SerializeObject(upatedStatus);

                var content = new StringContent(updateJson, Encoding.UTF8, "application/json");

   
                var updateResponse = await apiClient.PostAsync(updateUrl, content);

                if (updateResponse.IsSuccessStatusCode)
                {
                    var json = await updateResponse.Content.ReadAsStringAsync();

                    validator = JsonConvert.DeserializeObject<ViewModels.Validator>(json);
                }


                // ---- Build Email ----
                    var emailBuilder = new EmailBuilder();

                // Fetch Student Details
                string studentUrl = $"Student/GetStudent?studentId={upatedStatus.StudentID}";
                

                var response = await apiClient.GetAsync(studentUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                   var requestedD = JsonConvert.DeserializeObject<Student>(json);


                    if (requestedD != null)
                    {
                        emailBuilder.Requester = requestedD.FullName;
                        emailBuilder.RefNumber = requestedD.ReferenceNumber;
                    }
                }

                // Fetch Decline Reason if applicable
                if (upatedStatus.ReasonID != 0)
                {
                    emailBuilder.DeclineReason = upatedStatus.ReasonID.ToString();
                  
                }

                //---Choose email type----

                var emailTypeToSend = upatedStatus.StudentStatus == 3 ? "Decline" : "Turnaround";

                // ---- Send Email if Update was Successful ----
                if (validator.Status == Constants.Success && upatedStatus.StudentStatus != 9)
                {

                    var url33 = $"Email/SendEmail?studentID={studentid}&emailType={WebUtility.UrlEncode(emailTypeToSend)}";
                
                    var jsonString33 = JsonConvert.SerializeObject(emailBuilder);

                    var content1 = new StringContent(jsonString33, Encoding.UTF8, "application/json");


                    var responseEmail = await apiClient.PostAsync(url33, content1);


                    if (responseEmail.IsSuccessStatusCode)
                    {
                        var json = await responseEmail.Content.ReadAsStringAsync();

                        var output = JsonConvert.DeserializeObject<ResponseDto>(json);
                    
                        validator.Status = output.Status ? "Success" : "Error";
                    }

                    Session["SelectedApplicant"] = null;
                    Response.Redirect("~/Admin/Request.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    
                }

                if (upatedStatus.StudentStatus == 9)
                {
                    Session["SelectedApplicant"] = null;
                    Response.Redirect("~/Admin/RecallOrder.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    
                }


                // Close Modal
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup2", "$('#mySaveModal').modal('hide');", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"!!! EXCEPTION: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                Logs.WriteErrorLog("btnConfirm_Click : " + ex.Message);
            }
        }



        protected void lbtnViewemail_Click(object sender, EventArgs e)
        {
            if (((LinkButton)sender).NamingContainer is GridViewRow clickedRow)
            {
                string x = ((HiddenField)clickedRow.FindControl("hfMessage")).Value;

                // Format the message with HTML line breaks
                string formattedMessage = x.Replace("\n", "<br />");

                // Set the message in the label
                lblMessage.InnerHtml = formattedMessage;
                lblMessage.Visible = true;

                // Hide other sections
                gvSummary.Visible = false;
                modalHead.Visible = false;
                gvSentEmail.Visible = false;
                sentEmail.Visible = false;
                viewEmail.Visible = true;

                // IMPORTANT: Force the popUpSendEmail UpdatePanel to refresh
                popUpSendEmail.Update();

                // Use a longer delay and ensure the modal is fully initialized
                string script = @"
            setTimeout(function() {
                var modal = $('#popUpSendEmailModal');
                if (modal.length > 0) {
                    modal.modal({
                        backdrop: 'static',
                        keyboard: false,
                        show: true
                    });
                    console.log('Modal shown successfully');
                } else {
                    console.log('Modal not found in DOM');
                    alert('Modal not found. Please refresh the page.');
                }
            }, 500);
        ";

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showEmailModal", script, true);
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedValue == "1")
            {
                iframeDocument.Src = @"ShowOrderDocument.aspx?DocumetId=" + Session["DocumentGuidID"];
            }

            if (RadioButtonList1.SelectedValue == "2")
            {
                iframeDocument.Src = @"ShowOrderDocument.aspx?DocumetId=" + Session["IDorPassportDocumentGuidID"];
            }
        }

        protected async void lblRecall_OnClick(object sender, EventArgs e)
        {
            await LoadRequestStatusForRecall();
            ddlRequestStatus.Enabled = true;
            lblRequestStatus.Visible = false;
            ddlRequestStatus.Visible = true;
            await LoadReasonForRecall();
            btnBack.Visible = false;
            btnSave.Visible = true;
            btnClose.Visible = true;
            Div2.Visible = true;
            ddlDeclineReasons.Visible = true;
            lblDeclineReasons.Visible = false;
            Div12.Visible = false;
        }
    }
}