using Newtonsoft.Json;
using QualificationVerificationWeb.Helper;
using QualificationVerificationWeb.Helper.PayGate;
using QualificationVerificationWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Validator = QualificationVerificationWeb.ViewModels.Validator;


namespace QualificationVerificationWeb
{
    public partial class DocumentOrder : System.Web.UI.Page
    {
        ViewModels.Validator validator = new ViewModels.Validator();
 

        private readonly string qualificationVerificationAPI = ConfigurationManager.AppSettings["QualificationVerificationAPI"].ToString();

        Byte[] bytesDoc = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            
             string eventArgument = Request["__EVENTARGUMENT"];

            // Add this line
            if (!IsPostBack)
            {

                if (Session["StudentNumber"] != null)
                {
                    // Load the draft on first page load
                    CheckAndLoadDraft(Session["StudentNumber"].ToString());
                }


                LoadDeliveryType();
                //LoadAcademicDocument();
                LoadCountry();
                divUserInformation.Visible = true;
                divParentsInfo.Visible = false;
                Session["SpecialInstructionsInd"] = null;

                txtCourier1.Attributes.Add("maxlength", txtCourier1.MaxLength.ToString());

                //insert into gridview

                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[14] { new DataColumn("ComplexAddress"), new DataColumn("StreetAddress"), new DataColumn("Suburb"), new DataColumn("City"),
                                                       new DataColumn("Code"), new DataColumn("Country"), new DataColumn("CountryCode"), new DataColumn("DeliveryMethod"),
                                                       new DataColumn("DeliveryInd"),new DataColumn("Academic Record"),new DataColumn("Transcript Supplement"),new DataColumn("Confirmation Letter"),new DataColumn("Forms For Official Bodies"),new DataColumn("Address")});
                ViewState["Addresses"] = dt;
                BindGrid();

                ClearUploadedFile();
            }

            if (!string.IsNullOrEmpty(eventArgument))
            {


                if (eventArgument == "CLEAR_DRAFT")
                {
                    DeleteDraft(Session["StudentNumber"].ToString());
                }
            }

            //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
            // in FileUpload control but session is Null then Store the values to Session Object as:
            if (Session["FileUpload1"] == null && PaymentProof.HasFile)
            {
                ClearUploadError();
                Session["FileUpload1"] = PaymentProof;
                txtPaymentProof.Value = PaymentProof.FileName;


                if (PaymentProof.HasFiles)
                {

                    //Stream fc = //;


                    Stream fc = PaymentProof.PostedFile.InputStream;

                    //Byte[] bytesDoc = null;
                    if (fc != null)
                    {
                        BinaryReader br = new BinaryReader(fc);
                        Byte[] bytes = br.ReadBytes((Int32)fc.Length);
                        bytesDoc = bytes;
                        Session["PPFileUpload"] = bytes;
                        Session["PPFileName"] = PaymentProof.PostedFile.FileName;
                    }

                }
            }
            // Case 2: On Next PostBack Session has value but FileUpload control is
            // Blank due to PostBack then return the values from session to FileUpload as:
            else if (Session["FileUpload1"] != null && (!PaymentProof.HasFile))
            {
                PaymentProof = (FileUpload)Session["FileUpload1"];
                txtPaymentProof.Value = PaymentProof.FileName;
                Stream fs = PaymentProof.PostedFile.InputStream;
                Session["PPFileUpload1"] = fs;
            }
            // Case 3: When there is value in Session but user want to change the file then
            // In this case we need to change the file in session object also as:
            else if (PaymentProof.HasFile)
            {
                ClearUploadError();
                Session["FileUpload1"] = PaymentProof;
                txtPaymentProof.Value = PaymentProof.FileName;

                if (PaymentProof.HasFiles)
                {
                    Stream fc = PaymentProof.PostedFile.InputStream;

                    //Byte[] bytesDoc = null;
                    if (fc != null)
                    {
                        BinaryReader br = new BinaryReader(fc);
                        Byte[] bytes = br.ReadBytes((Int32)fc.Length);
                        bytesDoc = bytes;
                        Session["PPFileUpload"] = bytes;
                        Session["PPFileName"] = PaymentProof.PostedFile.FileName;
                    }

                }
            }
            //=========================================================================================

            //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
            // in FileUpload control but session is Null then Store the values to Session Object as:
            if (Session["FileUpload2"] == null && SpecialInstruction.HasFile)
            {
                ClearUploadError();
                Session["FileUpload2"] = SpecialInstruction;
                txtSpecialInstruction.Value = SpecialInstruction.FileName;


                if (SpecialInstruction.HasFiles)
                {


                    Stream sp = SpecialInstruction.PostedFile.InputStream;

                    //Byte[] bytesDoc = null;
                    if (sp != null)
                    {
                        BinaryReader bs = new BinaryReader(sp);
                        Byte[] bytes = bs.ReadBytes((Int32)sp.Length);
                        bytesDoc = bytes;
                        Session["SPFileUpload"] = bytes;
                        Session["SPFileName"] = SpecialInstruction.PostedFile.FileName;
                    }

                }
            }
            // Case 2: On Next PostBack Session has value but FileUpload control is
            // Blank due to PostBack then return the values from session to FileUpload as:
            else if (Session["FileUpload2"] != null && (!SpecialInstruction.HasFile))
            {
                SpecialInstruction = (FileUpload)Session["FileUpload2"];
                txtSpecialInstruction.Value = SpecialInstruction.FileName;
            }
            // Case 3: When there is value in Session but user want to change the file then
            // In this case we need to change the file in session object also as:
            else if (SpecialInstruction.HasFile)
            {
                ClearUploadError();
                Session["FileUpload2"] = SpecialInstruction;
                txtSpecialInstruction.Value = SpecialInstruction.FileName;



                if (SpecialInstruction.HasFiles)
                {


                    Stream sp = SpecialInstruction.PostedFile.InputStream;

                    //Byte[] bytesDoc = null;
                    if (sp != null)
                    {
                        BinaryReader bs = new BinaryReader(sp);
                        Byte[] bytes = bs.ReadBytes((Int32)sp.Length);
                        bytesDoc = bytes;
                        Session["SPFileUpload"] = bytes;
                        Session["SPFileName"] = SpecialInstruction.PostedFile.FileName;
                    }

                }
            }

            //=========================================================================================

            //Case: 1 When the page is submitted for the first time(First PostBack) and there is file 
            // in FileUpload control but session is Null then Store the values to Session Object as:
            if (Session["FileUpload3"] == null && IdPassport.HasFile)
            {
                ClearUploadError();
                Session["FileUpload3"] = IdPassport;
                txtIdPassport.Value = IdPassport.FileName;

                if (IdPassport.HasFiles)
                {


                    Stream sp = IdPassport.PostedFile.InputStream;

                    //Byte[] bytesDoc = null;
                    if (sp != null)
                    {
                        BinaryReader bs = new BinaryReader(sp);
                        Byte[] bytes = bs.ReadBytes((Int32)sp.Length);
                        bytesDoc = bytes;
                        Session["CPFileUpload"] = bytes;
                        Session["CPFileName"] = IdPassport.PostedFile.FileName;
                    }

                }
            }
            // Case 2: On Next PostBack Session has value but FileUpload control is
            // Blank due to PostBack then return the values from session to FileUpload as:
            else if (Session["FileUpload3"] != null && (!IdPassport.HasFile))
            {
                IdPassport = (FileUpload)Session["FileUpload3"];
                txtIdPassport.Value = IdPassport.FileName;
            }
            // Case 3: When there is value in Session but user want to change the file then
            // In this case we need to change the file in session object also as:
            else if (IdPassport.HasFile)
            {
                ClearUploadError();
                Session["FileUpload3"] = IdPassport;
                txtIdPassport.Value = IdPassport.FileName;

                if (IdPassport.HasFiles)
                {


                    Stream sp = IdPassport.PostedFile.InputStream;

                    //Byte[] bytesDoc = null;
                    if (sp != null)
                    {
                        BinaryReader bs = new BinaryReader(sp);
                        Byte[] bytes = bs.ReadBytes((Int32)sp.Length);
                        bytesDoc = bytes;
                        Session["CPFileUpload"] = bytes;
                        Session["CPFileName"] = IdPassport.PostedFile.FileName;
                    }

                }
            }
        }


        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            var token = HttpContext.Current?.Session["StudentJwtToken"] as string;

            try
            {
                if (string.IsNullOrEmpty(txtStudentnumber1.Text))
                {
                    lblErrorMessage.Text = "Student number is required";
                    return;
                }

                Session["IsDraft"] = true;
                
                   

                  Validator  validator;
               

                if (int.Parse(Session["DraftStudentId"].ToString()) > 0)
                {

                    var student = UpdateStudentDraft(Session["DraftStudentId"].ToString());

                    // Delete existing draft before saving a new one
                    string deleteUrl = qualificationVerificationAPI + "Student/ClearDraftOrderAsync" ;
                    
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(student);
                    validator = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(deleteUrl, WebRequestMethods.Http.Post, JSONString,token);

                }
                else
                {
                    validator = CreateStudent();
                }
                
                var resi = RecordSaveInBD(validator);


                if (resi.Status == Constants.Success ||resi.Status == null)
                {
                    Session["IsDraft"] = null;

                    if (paymentMethod.SelectedValue == "Bank Deposit")
                    {
                        ShowMessage("Your application has been saved as draft. You can continue later.", "success", "Submitted.aspx");
                    }
                    else
                    {
                        ShowMessage("Your application has been saved as draft. You can continue later.", "success", "Submitted.aspx");
                    }
                }
                else
                {
                    lblErrorMessage.Text = "Error saving draft: " + validator.Reason;
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(ex);
                Response.Redirect("~/Error.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string clientToken = Request.Form["__RequestVerificationToken"];

            var token = HttpContext.Current?.Session["StudentJwtToken"] as string;


            if (!FileNameValidation())
                return;



            if (!FeildsValidation())
                return; 
            try 
            { 
                /////////////////////////////////////////////////////////////////////////////////
                lblErrorMessage.Text = "";
                Session["CheckDuplicate"] = "0";

                List<CheckStudent> checkStudentList = new List<CheckStudent>();

                foreach (GridViewRow row in gvAddress.Rows)
                {
                    string ac_num = "0";
                    string ts_num = "0";
                    string cl_num = "0";
                    string forms_num = "0";
                    string deliveryInd = "";
                    string degree = "";

                    ac_num = ((DropDownList)row.FindControl("ddlNumberofAcademicRecord")).SelectedItem.Value;
                    ts_num = ((DropDownList)row.FindControl("ddlNumberofTranscriptSupplement")).SelectedItem.Value;
                    cl_num = ((DropDownList)row.FindControl("ddlNumberofConfirmationLetter")).SelectedItem.Value;
                    forms_num = ((DropDownList)row.FindControl("ddlNumberofFormsForOfficialBodies")).SelectedItem.Value;

                    deliveryInd = ((HiddenField)row.FindControl("DeliveryInd")).Value;

                    if (ac_num != "0")
                    {
                        degree = "Academic Record (Modules plus Results)";

                        string url = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" + deliveryInd + "&documentType=" + degree;
                        int academicDocumentID = BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url, WebRequestMethods.Http.Get, null).AcademicDocumentID;

                        CheckStudent checkStudent = new CheckStudent()
                        {
                            StudentNumber = txtStudentnumber1.Text.ToString(),
                            AcademicDocumentID = academicDocumentID,
                            NumberCopies = int.Parse(ac_num),
                            ComplexAddress = row.Cells[0].Text,
                            StreetAddress = row.Cells[1].Text,
                            Suburb = row.Cells[2].Text,
                            City = row.Cells[3].Text,
                            Code = row.Cells[4].Text,
                            CountryCode = ((HiddenField)row.FindControl("CountryCode")).Value

                        };
                        checkStudentList.Add(checkStudent);

                    }

                    if (ts_num != "0")
                    {
                        degree = "Academic Transcript Supplement (Course content including Academic Record)";

                        string url1 = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" + deliveryInd + "&documentType=" + degree;
                        int academicDocumentID = BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url1, WebRequestMethods.Http.Get, null).AcademicDocumentID;



                        CheckStudent checkStudent = new CheckStudent()
                        {
                            StudentNumber = txtStudentnumber1.Text.ToString(),
                            AcademicDocumentID = academicDocumentID,
                            NumberCopies = int.Parse(ts_num),
                            ComplexAddress = row.Cells[0].Text,
                            StreetAddress = row.Cells[1].Text,
                            Suburb = row.Cells[2].Text,
                            City = row.Cells[3].Text,
                            Code = row.Cells[4].Text,
                            CountryCode = ((HiddenField)row.FindControl("CountryCode")).Value

                        };
                        checkStudentList.Add(checkStudent);

                    }

                    if (cl_num != "0")
                    {
                        degree = "Confirmation Letter (General letters)";

                        string url2 = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" + deliveryInd + "&documentType=" + degree;
                        int academicDocumentID = BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url2, WebRequestMethods.Http.Get, null).AcademicDocumentID;


                        CheckStudent checkStudent = new CheckStudent()
                        {
                            StudentNumber = txtStudentnumber1.Text.ToString(),
                            AcademicDocumentID = academicDocumentID,
                            NumberCopies = int.Parse(cl_num),
                            ComplexAddress = row.Cells[0].Text,
                            StreetAddress = row.Cells[1].Text,
                            Suburb = row.Cells[2].Text,
                            City = row.Cells[3].Text,
                            Code = row.Cells[4].Text,
                            CountryCode = ((HiddenField)row.FindControl("CountryCode")).Value

                        };
                        checkStudentList.Add(checkStudent);

                    }
                    if (forms_num != "0")
                    {
                        degree = "Forms for Official Bodies";

                        string url3 = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" + deliveryInd + "&documentType=" + degree;
                        int academicDocumentID = BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url3, WebRequestMethods.Http.Get, null).AcademicDocumentID;


                        CheckStudent checkStudent = new CheckStudent()
                        {
                            StudentNumber = txtStudentnumber1.Text.ToString(),
                            AcademicDocumentID = academicDocumentID,
                            NumberCopies = int.Parse(forms_num),
                            ComplexAddress = row.Cells[0].Text,
                            StreetAddress = row.Cells[1].Text,
                            Suburb = row.Cells[2].Text,
                            City = row.Cells[3].Text,
                            Code = row.Cells[4].Text,
                            CountryCode = ((HiddenField)row.FindControl("CountryCode")).Value

                        };
                        checkStudentList.Add(checkStudent);

                    }

                }
                string documentTypeXml = string.Empty;

                if (checkStudentList != null)
                {
                    documentTypeXml = SerializationHelper.XmlHelper.SerializeObject(checkStudentList);
                    documentTypeXml = documentTypeXml.Replace("<?xml version=\"1.0\"?>", "").Replace("ArrayOfCheckStudent", "root").Replace("CheckStudent", "documentType");

                }

                string url5 = qualificationVerificationAPI + "Student/StudentRequestDuplicateCheck";
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(documentTypeXml);
                var studentCheckValidator = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url5, WebRequestMethods.Http.Post, JSONString, token);



                /////////////////////////////////////////////////////////////////////////////////////////  

            
                decimal totalAmount = Calculate();

                if (studentCheckValidator.Status == Constants.NotExist)
                {

                    if (totalAmount.ToString() != "0")
                    {
                        if (paymentMethod.SelectedValue == "Bank Deposit")
                        {
                            lblPopUpMessage.Text = " Total amount due R <b>" + totalAmount.ToString() + "</b> . Please click on the confirmation button to submit your request or cancel to edit.";
                        }
                        else
                        {
                            lblPopUpMessage.Text = " Total amount due R <b>" + totalAmount.ToString() + "</b> . Please click on the confirmation button to submit your request and make payment or cancel to edit.You will be redirected to the payment page.";
                        }


                    }

                    btnConfirm.Visible = true;
                    ConfirmTitle.Visible = true;
                    messageshow.Visible = true;
                    duplicatemessage.Visible = false;

                    AddressTitle.Visible = false;
                    divAddress.Visible = false;
                    btnSave_Add.Visible = false;
                    divAcademicDocumentMulti.Visible = false;
                    lblPopUpDuplicateMessage.Visible = false;

                    popUpConfirmation.Update();
                    ScriptManager.RegisterStartupScript(sender as Control, this.GetType(), "successPopup1", "$('#popUpConfirmationModal').modal('show');", true);
                }

                if (studentCheckValidator.Status == Constants.Exist)
                {
                    if (totalAmount.ToString() != "0")
                    {
                       lblPopUpDuplicateMessage.Text = totalAmount.ToString();
                    }

                    btnConfirm.Visible = true;
                    ConfirmTitle.Visible = true;
                    messageshow.Visible = false;
                    duplicatemessage.Visible = true;

                    AddressTitle.Visible = false;
                    divAddress.Visible = false;
                    btnSave_Add.Visible = false;
                    divAcademicDocumentMulti.Visible = false;

                    popUpConfirmation.Update();
                    ScriptManager.RegisterStartupScript(sender as Control, this.GetType(), "successPopup1", "$('#popUpConfirmationModal').modal('show');", true);

                    Session["CheckDuplicate"] = "1";

                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(ex);
                Response.Redirect("~/Error.aspx");
            }

        }

        private ViewModels.Validator CreateStudent()
        {
            var token = HttpContext.Current?.Session["StudentJwtToken"] as string;

            var validatorL = new ViewModels.Validator();

            var currentYear = DateTime.Today.ToString("yyyy");

            var checkDuplicate = Session["CheckDuplicate"] != null ? Session["CheckDuplicate"].ToString() : "0";
            var ujStudent = Session["UjStudent"] != null ? Session["UjStudent"].ToString() : "No";
            var PostIDNumbers = Session["PostID"] != null ? Session["PostID"].ToString() : "";
            var isDraftSet = Session["IsDraft"].ToString();

            var _student = new Student()
            {
                StudentIDNumber = PostIDNumbers,
                StudentNumber = txtStudentnumber1.Text,
                CreatedBy = txtStudentnumber1.Text,
                LastUpdatedBy = txtStudentnumber1.Text,
                Surname = txtSurname1.Text,
                FullName = txtName1.Text,
                EmailAddress = txtEmail1.Text,
                PhoneNumber = txtPhone1.Text,
                CourierInstructions = txtCourier1.Text,
                ApplicationYear = currentYear,
                StudentStatus = Convert.ToInt32(FeesGrantStatus.Pending),
                AcceptedTerms = true,
                MaidenSurname = txtMaiden1.Text,
                ConfirmDuplicateOrder = checkDuplicate == "1" ? true : false,
                ConfirmDuplicateOrderDt = checkDuplicate == "1" ? DateTime.Now : (DateTime?)null,
                ThirdParty_or_Student = ujStudent == "Yes" ? true : false,
                PaymentMethod = paymentMethod.SelectedValue,
                IsDraft = bool.Parse(isDraftSet)
            };

            string url4 = qualificationVerificationAPI + "Student/CreateStudent";
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(_student);
            validatorL = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url4, WebRequestMethods.Http.Post, JSONString,token);

            return validatorL;
        }


        private Student UpdateStudentDraft(string studentId)
        {
            var validatorL = new ViewModels.Validator();

            var currentYear = DateTime.Today.ToString("yyyy");

           
            var ujStudent = Session["UjStudent"] != null ? Session["UjStudent"].ToString() : "No";
       

            var student = new Student()
            {
                StudentID = int.Parse(studentId),
                EmailAddress = txtEmail1.Text,
                PhoneNumber = txtPhone1.Text,
                CourierInstructions = txtCourier1.Text,
                ApplicationYear = currentYear,
                ThirdParty_or_Student = ujStudent == "Yes" ? true : false,
                PaymentMethod = paymentMethod.SelectedValue
            };

            return student;
        }

        private string GetDeliveryMethodName(string deliveryInd)
        {
            // Implement logic to convert deliveryInd to display name
            switch (deliveryInd)
            {
                case "C": return "Collect";
                case "S": return "Courier (South Africa)";
                case "I": return "Courier (International)";
                case "E": return "Electronic Copy";
                default: return deliveryInd;
            }
        }
        public bool FileNameValidation()
        {
            ClearUploadError();

            if (txtSpecialInstruction.Value != "")
            {

                if (txtSpecialInstruction.Value == txtIdPassport.Value || txtSpecialInstruction.Value == txtPaymentProof.Value)
                {
                    lblErrorMessage4.Text = "File is already uploaded. Choose a different file.";
                    return false;
                }

            }

            if (txtPaymentProof.Value != "")
            {
                if (txtPaymentProof.Value == txtIdPassport.Value || txtPaymentProof.Value == txtSpecialInstruction.Value)
                {

                    lblErrorMessage3.Text = "File is already uploaded. Choose a different file.";
                    return false;
                }
            }

            if (txtIdPassport.Value != "")
            {
                if (txtIdPassport.Value == txtPaymentProof.Value || txtIdPassport.Value == txtSpecialInstruction.Value)
                {
                    lblErrorMessage2.Text = "File is already uploaded. Choose a different file.";
                    return false;
                }
            }


            return true;
        }

        public bool FeildsValidation()
        {
            lblErrorMessage.Text = "";
            bool shouldStop = false;


            int qualCount = 0;
            DataTable dt = (DataTable)ViewState["Addresses"];

            qualCount = dt.Rows.Count;

            if (qualCount == 0)
            {
                lblErrorMessage.Text = "* Please add delivery method.";
                return false;
            }

            if (txtStudentnumber1.Text.Length < 9)
            {
                lblErrorMessage.Text = "* Invalid Student number.";
                return false;
            }


            if (txtPhone1.Text.Length < 10)
            {
                lblErrorMessage.Text = "* Invalid Contact number.";
                return false;
            }
            if (txtPhone1.Text == "")
            {
                lblErrorMessage.Text = "* Please Enter Contact number.";
                return false;
            }

            if (txtEmail1.Text == "")
            {
                lblErrorMessage.Text = "* Please Enter Contact Email Address.";
                return false;
            }

            if (paymentMethod.SelectedValue == "")
            {
                lblErrorMessage.Text = "* Please select a payment method.";
                return false;
            }


            foreach (GridViewRow row in gvAddress.Rows)
            {
                string ac_num = "0";
                string ts_num = "0";
                string cl_num = "0";
                string forms_num = "0";

                ac_num = ((DropDownList)row.FindControl("ddlNumberofAcademicRecord")).SelectedItem.Value;
                ts_num = ((DropDownList)row.FindControl("ddlNumberofTranscriptSupplement")).SelectedItem.Value;
                cl_num = ((DropDownList)row.FindControl("ddlNumberofConfirmationLetter")).SelectedItem.Value;
                forms_num = ((DropDownList)row.FindControl("ddlNumberofFormsForOfficialBodies")).SelectedItem.Value;

                if (ac_num == "0" && ts_num == "0" && cl_num == "0" && forms_num == "0")
                {
                    lblErrorMessage.Text = "* Please select a minimun of one document type per row on your request(s).";
                    return false;
                }

            }
            if (/*IdPassport.HasFiles == false &&*/ Session["CPFileUpload"] == null)
            {
                lblErrorMessage.Text = "* Please upload Copy of Id/Passport.";
                return false;
            }

            if (/*PaymentProof.HasFiles == false &&*/ paymentMethod.SelectedValue == "Bank Deposit" && Session["PPFileUpload"] == null)
            {

                lblErrorMessage.Text = "* Please upload proof of payment.";
                return false;
            }


            // Check if any FormsForOfficialBodies were selected
            bool isSpecialInstructionRequired = false;

            foreach (GridViewRow row in gvAddress.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlforms = (DropDownList)row.FindControl("ddlNumberofFormsForOfficialBodies");
                    if (ddlforms != null && int.TryParse(ddlforms.SelectedValue, out int selectedValue))
                    {
                        if (selectedValue > 0)
                        {
                            isSpecialInstructionRequired = true;

                            break;
                        }
                    }
                }
            }

            // Store or remove session indicator for special Instructions
            if (isSpecialInstructionRequired)
            {
                Session["SpecialInstructionsInd"] = "1";
            }
            else
            {
                Session.Remove("SpecialInstructionsInd");
            }


            //Show error if required field is missing
            if (isSpecialInstructionRequired && !SpecialInstruction.HasFile)
            {
                /*lblErrorMessage4.Text = "Please upload Special Instruction Form when requesting Forms for Official Bodies.";
                lblErrorMessage4.ForeColor = System.Drawing.Color.Red;*/

                // Show the modal popup
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showMandatoryInstruction", "showMandatoryInstructionModal();", true);

                shouldStop = true;
                return false;           //must return false in a bool-returning method to exit immediately
            }
            else if ((Session["AcceptedTerms"] != null || Session["Completed"] != null) &&
                string.IsNullOrWhiteSpace(txtCourier1.Text) && Session["SpecialInstructionInd"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showSpecialInstructionModal", "mySpecialInstructionModalShow();", true);
                Session["SpecialInstructionInd"] = "2";
                shouldStop = true;
                return false;
            }
            return !shouldStop;
        }

        private bool PayGate()
        {
            btnSave.Text = "Processing ...";
            btnSave.Enabled = false;

            var rootUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);



            var payGateService = new PayGateService
            {
                PayGateUrl = Settings.GetPayGateUrl,
                PayGateQueryUrl = Settings.GetPayGateQueryUrl,
                PayGateId = Settings.GetPayGateId,
                Reference = Session["StudentID"].ToString(),
                Amount = Session["txtTotalAmount"].ToString(),
                Currency = Constants.Currency,
                ReturnUrl = rootUrl + Settings.ReturnUrl,
                TransactionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Locale = Constants.Locale,
                Country = Constants.Country,
                CustomerEmail = txtEmail1.Text,
                EncryptionKey = Settings.GetEncryptionKey
            };

            LogValues("PayGateUrl", payGateService.PayGateUrl);
            LogValues("PayGateQueryUrl", payGateService.PayGateQueryUrl);
            LogValues("PayGateId", payGateService.PayGateId);
            LogValues("Reference", payGateService.Reference);
            LogValues("Amount", payGateService.Amount);
            LogValues("Currency", payGateService.Currency);
            LogValues("ReturnUrl", payGateService.ReturnUrl);
            LogValues("TransactionDate", payGateService.TransactionDate);
            LogValues("Locale", payGateService.Locale);
            LogValues("Country", payGateService.Country);
            LogValues("CustomerEmail", payGateService.CustomerEmail);
            LogValues("EncryptionKey", payGateService.EncryptionKey);


            var initiatePaymentResult = payGateService.InitiatePayment();

            LogValues("initiatePaymentResult", initiatePaymentResult.Message);


            if (initiatePaymentResult.Success)
            {
                CreateOnlinePaymentSession();

                payGateService.PayRequestId = initiatePaymentResult.Data["PAY_REQUEST_ID"];
                payGateService.ProcessPayment(this.Page, initiatePaymentResult.Data["PAYGATE_ID"], initiatePaymentResult.Data["PAY_REQUEST_ID"], initiatePaymentResult.Data["REFERENCE"], initiatePaymentResult.Data["CHECKSUM"]);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ShowStatusModal", "<script>ShowFailedPaymentStatusModal('" + initiatePaymentResult.Message + "');</script>");
                string script = $"ShowFailedPaymentStatusModal('{initiatePaymentResult.Message}');";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowStatusModal", $"<script>{script}</script>");

                return false;
            }

            return true;
        }


        private ViewModels.Validator CreateOnlinePaymentSession()
        {
            var token = HttpContext.Current?.Session["StudentJwtToken"] as string;

            var validatorL = new ViewModels.Validator();

            var _onlinePaymentSession = new OnlinePaymentSession()
            {

                StudentID = int.Parse(Session["StudentID"].ToString()),
                TotalAmount = Session["txtTotalAmount"].ToString(),
                TryAgain = int.Parse(Session["TryAgain"].ToString()),
                CreatedOnDate = DateTime.Now,
                Token = token
            };

            string url4 = qualificationVerificationAPI + "OnlinePaymentSession/AddOnlinePaymentSession";
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(_onlinePaymentSession);
            validatorL = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url4, WebRequestMethods.Http.Post, JSONString,token);

            return validatorL;

        }


        private ViewModels.Validator RecordSaveInBD(ViewModels.Validator validator)
        {
            var token = HttpContext.Current?.Session["StudentJwtToken"] as string;

            string extention = string.Empty;

            int generatedStudentId = 0;

            var _student = new Student() { StudentNumber = txtStudentnumber1.Text };

            var studentDocumentsJoinValidator = new ViewModels.Validator();

            if (validator.Status == Constants.Success)
            {

         
                //add Copy of Id/Passport
                var studentIdPassportValidator = new ViewModels.Validator();

                if (Session["CPFileUpload"] != null)
                {

                    extention = Path.GetExtension((string)Session["CPFileName"]).ToLower().Trim();

                    var documentF = SafeGetBytes(Session["CPFileUpload"]);

                    var idPassportCopy = new Document()
                    {
                        DocumentTypeID = (int)FeesGrantDocumentType.IDCopy,
                        Document1 = _student.StudentNumber + "_IdPassportCopy" + extention,
                        CreatedBy = _student.StudentNumber,
                        LastUpdatedBy = _student.StudentNumber,
                        DocumentStatus = Convert.ToInt32(FeesGrantStatus.Pending),
                        DocumentFile = documentF

                    };

                    string url4 = qualificationVerificationAPI + "Document/AddDocument";
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(idPassportCopy);
                    studentIdPassportValidator = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url4, WebRequestMethods.Http.Post, JSONString, token);

                }

                extention = string.Empty;

                if (studentIdPassportValidator.Status == Constants.Success)
                {
                    generatedStudentId = validator.ID;
                    var _studentDocuments = new StudentDocument()
                    {
                        StudentID = validator.ID,
                        DocumentID = studentIdPassportValidator.ID,
                    };

                    string url = qualificationVerificationAPI + "Document/AddStudentDocument";
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(_studentDocuments);
                    BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url, WebRequestMethods.Http.Post, JSONString, token);
                }

                
                AddQualificationDocument(generatedStudentId);

        
                if (!bool.Parse(Session["IsDraft"].ToString()))
                {
                    //add Special Instruction form 
              

                    string url2 = qualificationVerificationAPI + "Student/UpdateReferenceNUmber?studentID=" +
                                  generatedStudentId;

                   BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url2, WebRequestMethods.Http.Get, token);

                }


                //add Special Instruction form 
                var studentSpecialInstrucDocumentsValidator = new ViewModels.Validator();

                if (Session["SPFileUpload"] != null)
                {

                    extention = Path.GetExtension((string)Session["SPFileName"]).ToLower().Trim();

                    var documentF = SafeGetBytes(Session["SPFileUpload"]);

                    var paymentProof = new Document()
                    {
                        DocumentTypeID = (int)FeesGrantDocumentType.SpecialInstruction,
                        Document1 = _student.StudentNumber + "_SpecialInstruction" + extention,
                        CreatedBy = _student.StudentNumber,
                        LastUpdatedBy = _student.StudentNumber,
                        DocumentStatus = Convert.ToInt32(FeesGrantStatus.Pending),
                        DocumentFile = documentF

                    };

                    string url = qualificationVerificationAPI + "Document/AddDocument";
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(paymentProof);
                    studentSpecialInstrucDocumentsValidator = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url, WebRequestMethods.Http.Post, JSONString, token);

                }
                extention = string.Empty;

                if (studentSpecialInstrucDocumentsValidator.Status == Constants.Success)
                {
                    generatedStudentId = validator.ID;

                    var _studentDocuments = new StudentDocument()
                    {
                        StudentID = validator.ID,
                        DocumentID = studentSpecialInstrucDocumentsValidator.ID,
                    };

                    string url4 = qualificationVerificationAPI + "Document/AddStudentDocument";
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(_studentDocuments);
                    BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url4, WebRequestMethods.Http.Post, JSONString, token);


                }

                //add student payment proof form 
                var studentDocumentsValidator = new ViewModels.Validator();

                if (Session["PPFileUpload"] != null && paymentMethod.SelectedValue == "Bank Deposit")
                {

                    extention = Path.GetExtension((string)Session["PPFileName"]).ToLower().Trim();

                    var documentF = SafeGetBytes(Session["PPFileUpload"]);

                    var paymentProof = new Document()
                    {
                        DocumentTypeID = (int)FeesGrantDocumentType.PaymentProof,
                        Document1 = _student.StudentNumber + "_PayementProof" + extention,
                        CreatedBy = _student.StudentNumber,
                        LastUpdatedBy = _student.StudentNumber,
                        DocumentStatus = Convert.ToInt32(FeesGrantStatus.Pending),
                        DocumentFile = documentF

                    };


                    string url = qualificationVerificationAPI + "Document/AddDocument";
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(paymentProof);
                    studentDocumentsValidator = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url, WebRequestMethods.Http.Post, JSONString, token);

                }

                extention = string.Empty;

                if (studentDocumentsValidator.Status == Constants.Success)
                {
                    generatedStudentId = validator.ID;

                    var _studentDocuments = new StudentDocument()
                    {
                        StudentID = validator.ID,
                        DocumentID = studentDocumentsValidator.ID,
                    };


                    string url4 = qualificationVerificationAPI + "Document/AddStudentDocument";
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(_studentDocuments);
                    studentDocumentsJoinValidator = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url4, WebRequestMethods.Http.Post, JSONString, token);

                }
            }
            return studentDocumentsJoinValidator;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;

            var token = HttpContext.Current?.Session["StudentJwtToken"] as string;

            lblErrorMessage.Text = "";

            Session["TryAgain"] = "1";
            Session["IsDraft"] = false;

            try
            {
                var currentYear = DateTime.Today.ToString("yyyy");

                var selectedPaymentMethod = paymentMethod.SelectedValue;

                Validator validator;

                if (int.Parse(Session["DraftStudentId"].ToString()) > 0)
                {
                    
                    var student = UpdateStudentDraft(Session["DraftStudentId"].ToString());

                    // Delete existing draft before saving a new one
                    string deleteUrl = qualificationVerificationAPI + "Student/ClearDraftOrderAsync";

                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(student);
                    validator = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(deleteUrl, WebRequestMethods.Http.Post, JSONString, token);
                }
                else
                {
                    validator = CreateStudent();
                }

                Session["StudentID"] = validator.ID.ToString();
                Session["txtTotalAmount"] = txtTotalAmount.Text.Replace("R", "").Replace(",", "").Replace(" ", "").Replace(".", "").Replace(".", "").ToString();


                if (selectedPaymentMethod == "Online Payment" && validator.Status == Constants.Success)
                {
                    OnlinePaymentStep(validator);
                }


                if (selectedPaymentMethod == "Bank Deposit" && validator.Status == Constants.Success)
                {
                    var resi = RecordSaveInBD(validator);

                    if (resi.Status == Constants.Success)
                    {
                        string url = qualificationVerificationAPI + "Student/SendEmail?studentID=" + validator.ID + "&emailType=Acknowledgement";
                        validator = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url, WebRequestMethods.Http.Get, token);
                    }


                    Session["Completed"] = null;

                    Session["__SSuccessSubmit@!#"] = true;

                    ScriptManager.RegisterStartupScript(sender as Control, this.GetType(), "successPopup1", "$('#popUpConfirmationModal').removeClass('fade').modal('hide');", true);

                    if (selectedPaymentMethod == "Bank Deposit")
                    {
                        Response.Redirect("~/Submitted.aspx",false);
                        Context.ApplicationInstance.CompleteRequest();
                    }

                    txtPaymentProof.Value = "";
                    txtSpecialInstruction.Value = "";
                    txtIdPassport.Value = "";
                    Session["FileUpload1"] = null;
                    Session["FileUpload2"] = null;
                    Session["FileUpload3"] = null;
                }



            }
            catch (Exception ex)
            {
                LogValues("Exception", ex.StackTrace);
                Logs.WriteErrorLog(ex);
                Response.Redirect("~/Error.aspx");
            }
        }



        private void AddQualificationDocument(int generatedStudentId)
        {
            var token = HttpContext.Current?.Session["StudentJwtToken"] as string;

            var studentAcademicDocumentValidator = new ViewModels.Validator();


            List<StudentAcademicDocumentAddressDto> studentOrderList = new List<StudentAcademicDocumentAddressDto>();
            foreach (GridViewRow row in gvAddress.Rows)
            {
                string ac_num = "0";
                string ts_num = "0";
                string cl_num = "0";
                string forms_num = "0";
                string deliveryInd = "";
                string degree = "";

                ac_num = ((DropDownList)row.FindControl("ddlNumberofAcademicRecord")).SelectedItem.Value;
                ts_num = ((DropDownList)row.FindControl("ddlNumberofTranscriptSupplement")).SelectedItem.Value;
                cl_num = ((DropDownList)row.FindControl("ddlNumberofConfirmationLetter")).SelectedItem.Value;
                forms_num = ((DropDownList)row.FindControl("ddlNumberofFormsForOfficialBodies")).SelectedItem.Value;



                deliveryInd = ((HiddenField)row.FindControl("DeliveryInd")).Value;

                if (ac_num != "0")
                {
                    degree = "Academic Record (Modules plus Results)";

                    string url11 = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" + deliveryInd + "&documentType=" + degree;
                    int academicDocumentID = BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url11, WebRequestMethods.Http.Get, null).AcademicDocumentID;


                    #region
                    StudentAcademicDocumentAddressDto getDocumentAddress = new StudentAcademicDocumentAddressDto()
                    {
                        StudentID = generatedStudentId,
                        AcademicDocumentID = academicDocumentID,
                        NumberCopies = int.Parse(ac_num),
                        ComplexAddress = row.Cells[0].Text,
                        StreetAddress = row.Cells[1].Text,
                        Suburb = row.Cells[2].Text,
                        City = row.Cells[3].Text,
                        Code = row.Cells[4].Text,
                        CountryCode = ((HiddenField)row.FindControl("CountryCode")).Value

                    };
                    #endregion

                    studentOrderList.Add(getDocumentAddress);

                }

                if (ts_num != "0")
                {
                    degree = "Academic Transcript Supplement (Course content including Academic Record)";

                    string url12 = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" + deliveryInd + "&documentType=" + degree;
                    int academicDocumentID = BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url12, WebRequestMethods.Http.Get, null).AcademicDocumentID;


                    #region
                    StudentAcademicDocumentAddressDto getDocumentAddress = new StudentAcademicDocumentAddressDto()
                    {
                        StudentID = generatedStudentId,
                        AcademicDocumentID = academicDocumentID,
                        NumberCopies = int.Parse(ts_num),
                        ComplexAddress = row.Cells[0].Text,
                        StreetAddress = row.Cells[1].Text,
                        Suburb = row.Cells[2].Text,
                        City = row.Cells[3].Text,
                        Code = row.Cells[4].Text,
                        CountryCode = ((HiddenField)row.FindControl("CountryCode")).Value

                    };
                    #endregion
                    studentOrderList.Add(getDocumentAddress);

                }

                if (cl_num != "0")
                {
                    degree = "Confirmation Letter (General letters)";

                    string url31 = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" + deliveryInd + "&documentType=" + degree;
                    int academicDocumentID = BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url31, WebRequestMethods.Http.Get, null).AcademicDocumentID;


                    #region
                    StudentAcademicDocumentAddressDto getDocumentAddress = new StudentAcademicDocumentAddressDto()
                    {
                        StudentID = generatedStudentId,
                        AcademicDocumentID = academicDocumentID,
                        NumberCopies = int.Parse(cl_num),
                        ComplexAddress = row.Cells[0].Text,
                        StreetAddress = row.Cells[1].Text,
                        Suburb = row.Cells[2].Text,
                        City = row.Cells[3].Text,
                        Code = row.Cells[4].Text,
                        CountryCode = ((HiddenField)row.FindControl("CountryCode")).Value

                    };
                    #endregion
                    studentOrderList.Add(getDocumentAddress);

                }
                if (forms_num != "0")
                {
                    degree = "Forms for Official Bodies";

                    string url41 = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" + deliveryInd + "&documentType=" + degree;
                    int academicDocumentID = BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url41, WebRequestMethods.Http.Get, null).AcademicDocumentID;


                    #region
                    StudentAcademicDocumentAddressDto getDocumentAddress = new StudentAcademicDocumentAddressDto()
                    {
                        StudentID = generatedStudentId,
                        AcademicDocumentID = academicDocumentID,
                        NumberCopies = int.Parse(forms_num),
                        ComplexAddress = row.Cells[0].Text,
                        StreetAddress = row.Cells[1].Text,
                        Suburb = row.Cells[2].Text,
                        City = row.Cells[3].Text,
                        Code = row.Cells[4].Text,
                        CountryCode = ((HiddenField)row.FindControl("CountryCode")).Value

                    };
                    #endregion
                    studentOrderList.Add(getDocumentAddress);

                }

            }

            string studentAcademicDocumentAddressXml = string.Empty;

            if (studentOrderList != null)
            {
                studentAcademicDocumentAddressXml = SerializationHelper.XmlHelper.SerializeObject(studentOrderList);
                studentAcademicDocumentAddressXml = studentAcademicDocumentAddressXml.Replace("<?xml version=\"1.0\"?>", "").Replace("ArrayOfStudentAcademicDocumentAddressDto", "root").Replace("StudentAcademicDocumentAddressDto", "studentAcademicDocumentAddress");

           

                string url5 = qualificationVerificationAPI + "Student/AddStudentAcademicDocumentAddress";
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(studentAcademicDocumentAddressXml);
                studentAcademicDocumentValidator = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url5, WebRequestMethods.Http.Post, JSONString, token);

            }
           

            var studentQualificationstValidator = new ViewModels.Validator();

            List<StudentQualificationData> studentQualificationList = new List<StudentQualificationData>();

            foreach (GridViewRow row in gvQualification.Rows)
            {

                StudentQualificationData studentQualification = new StudentQualificationData()
                {
                    StudentID = generatedStudentId,
                    QualificationName = (((Label)(row.FindControl("lblQualificationName"))).Text.Trim()),
                    FacultyName = (((Label)(row.FindControl("lblFacultyName"))).Text.Trim()),
                    FromYear = (((Label)(row.FindControl("lblFromYear"))).Text.Trim()),
                    ToYear = (((Label)(row.FindControl("lblToYear"))).Text.Trim()),
                    CurrentlyResidingFaculty = (((Label)(row.FindControl("lblCurrentlyResidingFaculty"))).Text.Trim())

                };


                studentQualificationList.Add(studentQualification);


            }

            string studentQualificationXml = string.Empty;

            if (studentQualificationList != null)
            {
                studentQualificationXml = SerializationHelper.XmlHelper.SerializeObject(studentQualificationList);
                studentQualificationXml = studentQualificationXml.Replace("<?xml version=\"1.0\"?>", "").Replace("ArrayOfStudentQualificationData", "root").Replace("StudentQualificationData", "studentQualification");

            

                string url3 = qualificationVerificationAPI + "Document/AddQualificationDocument";
                string JSONString3 = string.Empty;
                JSONString3 = JsonConvert.SerializeObject(studentQualificationXml);
                studentQualificationstValidator = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url3, WebRequestMethods.Http.Post, JSONString3,token);
            }
           


        }

        public void LogValues(string variable, string error)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ResponseLog.txt");

            string logEntry = $"{DateTime.Now} - {variable}: {error}";

            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }


        private string MaskDigitsID(string input)
        {
            //take first 6 characters
            string firstPart = input.Substring(0, 6);

            //take last 4 characters
            int len = input.Length;
            string lastPart = input.Substring(len - 2, 2);

            //take the middle part (****)
            int middlePartLenght = len - (firstPart.Length + lastPart.Length);
            string middlePart = new String('*', middlePartLenght);

            return firstPart + middlePart + lastPart;
        }

        private string MaskDigitsPassport(string input)
        {
            //take first 6 characters
            string firstPart = input.Substring(0, 3);

            //take the last part (****)
            int len = input.Length;
            int lastPartLenght = len - (firstPart.Length);
            string lastPart = new String('*', lastPartLenght);

            return firstPart + lastPart;
        }
        public void GetStudentQualification(string studentNumber, string idNumber)
        {
            var token = HttpContext.Current?.Session["StudentJwtToken"] as string;

            string url2 = qualificationVerificationAPI + "Student/GetStudentQualificationData?studentNumber=" + null + "&idNumber=" + idNumber;
            List<StudentQualificationAPIData> qualList = BusinessRules.HttpHelper.HttpCallJson<List<StudentQualificationAPIData>>(url2, WebRequestMethods.Http.Get, token).ToList();

            List<StudentQualificationAPIData2> qualList1 = new List<StudentQualificationAPIData2>();

            if (qualList != null)
            {
                foreach (var item in qualList)
                {
                    txtStudentnumber1.Text = item.STUDENT_NUMBER.ToString();
                    txtName1.Text = item.FIRST_NAMES.ToString();
                    txtSurname1.Text = item.SURNAME.ToString();
                    //txtIDNumber1.Text = item.NI_PI.ToString();
                    txtIDNumber1.Text = (item.NI_PI.ToString().Length < 13) ? MaskDigitsPassport(item.NI_PI.ToString()) : MaskDigitsID(item.NI_PI.ToString());
                    txtInitials1.Text = item.INITIALS.ToString();
                    Session["PostID"] = item.NI_PI.ToString();

                    qualList1.Add(new StudentQualificationAPIData2 { QUALIFICATION_NAME = item.QUALIFICATION_NAME });
                }

                gvQualificationShow.DataSource = qualList1.Distinct();
                gvQualificationShow.DataBind();

                gvQualification.DataSource = qualList;
                gvQualification.DataBind();

            }
        }

        public void LoadDeliveryType()
        {
            string url = qualificationVerificationAPI + "Student/GetDeliveryType";
            ddlAcademicDocument.DataSource = BusinessRules.HttpHelper.HttpCallJson<List<AcademicDocumentData>>(url, WebRequestMethods.Http.Get, null).ToList();

            ddlAcademicDocument.DataTextField = "DeliveryType";
            ddlAcademicDocument.DataValueField = "DeliveryInd";
            ddlAcademicDocument.DataBind();
            ddlAcademicDocument.Items.Insert(0, new ListItem("Select Delivery Method", "0"));
        }

        public void ClearUploadError()
        {
            lblErrorMessage3.Text = "";
            lblErrorMessage2.Text = "";
            lblErrorMessage4.Text = "";
        }

        public void LoadCountry()
        {
            string url = qualificationVerificationAPI + "Student/GetCountryList";
            ddlCountry.DataSource = BusinessRules.HttpHelper.HttpCallJson<List<CountryData>>(url, WebRequestMethods.Http.Get, null).ToList();

            ddlCountry.DataTextField = "CountryName";
            ddlCountry.DataValueField = "CountryCode";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));

        }

        public void LoadCountryElectronicInternationalOrder()
        {
            string url = qualificationVerificationAPI + "Student/GetCountryList";
            ddlCountryElectronicInternationalOrder.DataSource = BusinessRules.HttpHelper.HttpCallJson<List<CountryData>>(url, WebRequestMethods.Http.Get,null).ToList();

            ddlCountryElectronicInternationalOrder.DataTextField = "CountryName";
            ddlCountryElectronicInternationalOrder.DataValueField = "CountryCode";
            ddlCountryElectronicInternationalOrder.DataBind();
            ddlCountryElectronicInternationalOrder.Items.Insert(0, new ListItem("Select Country", "0"));
        }
        public void ClearUploadedFile()
        {

            txtSpecialInstruction.Value = "";
            txtIdPassport.Value = "";
            txtPaymentProof.Value = "";
            Session["FileUpload1"] = null;
            Session["FileUpload2"] = null;
            Session["FileUpload3"] = null;

            //Session.Clear();
            // Stop Caching in IE
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            // Stop Caching in Firefox
            Response.Cache.SetNoStore();
            Response.Cache.AppendCacheExtension("no-cache");
            Response.Expires = 0;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

            ClearUploadedFile();
            Response.Redirect("~/Default.aspx");

        }

        protected void btnCancelOut_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Default.aspx");
        }

        protected void ddlAcademicDocument_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblPopUpValidationMessage.Text = "";

            if (ddlAcademicDocument.SelectedValue == "E")
            {
                SetCollectAndElectronicDisplay();
                pnlElectronicCopyDestination.Visible = true;
                pnlCountry.Visible = ddlElectronicDestination.SelectedValue == "International";
            }
            else if (ddlAcademicDocument.SelectedValue == "S" || ddlAcademicDocument.SelectedValue == "I")
            {
                pnlElectronicCopyDestination.Visible = false;
                pnlCountry.Visible = false;

                divAddress.Visible = true;
                divAcademicDocumentMulti.Visible = true;
                AddressTitle.Visible = true;
                btnSave_Add.Visible = true;

                btnConfirm.Visible = false;
                ConfirmTitle.Visible = false;
                notice.Visible = false;

                Div24.Visible = true;

                if (ddlAcademicDocument.SelectedValue == "S")
                {
                    Div17.Visible = false;
                    Div66.Visible = false;
                    Div16.Visible = true;
                    ddlCountry.SelectedItem.Value = "ZAF";
                    ddlCountry.SelectedItem.Text = "South Africa";
                }
                else
                {
                    Div17.Visible = true;
                    Div66.Visible = true;
                    Div16.Visible = false;
                }
            }
            else
            {
                SetCollectAndElectronicDisplay();
                pnlElectronicCopyDestination.Visible = false;
                pnlCountry.Visible = false;
                ddlElectronicDestination.ClearSelection();
                ddlCountryElectronicInternationalOrder.Items.Clear();
            }

            AcademicDocumentUpdatePanel.Update();
            popUpConfirmation.Update();
            ScriptManager.RegisterStartupScript(sender as Control, this.GetType(), "successPopup1", "$('#popUpConfirmationModal').modal('show');", true);
        }
        protected void ddlElectronicDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlElectronicDestination.SelectedValue == "International")
            {
                LoadCountryElectronicInternationalOrder();
                pnlCountry.Visible = true;
            }
            else
            {
                pnlCountry.Visible = false;
                //txtCountryDestination.Text = string.Empty;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#popUpConfirmationModal').modal('show');", true);


            // Optional: update panel manually if needed
               //AcademicDocumentUpdatePanel.Update();
        }

        private void DisplyCollectAndElectronic(object sender)
        {
            SetCollectAndElectronicDisplay();
            AcademicDocumentUpdatePanel.Update();
            popUpConfirmation.Update();
            ScriptManager.RegisterStartupScript(sender as Control, this.GetType(), "successPopup1", "$('#popUpConfirmationModal').modal('show');", true);
        }

        private void SetCollectAndElectronicDisplay()
        {
            divAddress.Visible = false;
            divAcademicDocumentMulti.Visible = true;
            AddressTitle.Visible = true;
            btnSave_Add.Visible = true;

            btnConfirm.Visible = false;
            ConfirmTitle.Visible = false;
            notice.Visible = false;


            lblNoticeMessage.Text = "<p>HOW LONG WILL IT TAKE TO RECEIVE MY DOCUMENTS?</p><p> Academic record:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;3 working days</ p > < p > Transcript supplement: &nbsp; &nbsp; &nbsp; &nbsp; 20 working days</ p > ";
        }
        protected void BindGrid()
        {
            gvAddress.DataSource = (DataTable)ViewState["Addresses"];
            gvAddress.DataBind();
        }

        public bool AddressValidation()
        {
            lblPopUpValidationMessage.Text = "";

            if (txtStreet1.Text != "")
            {
                var searchFor = new List<string>();
                searchFor.Add("POBOX");
                searchFor.Add("P.OBOX");
                searchFor.Add("P-OBOX");
                searchFor.Add("P_OBOX");

                string compareString = Regex.Replace(txtStreet1.Text.ToUpper(), @"\s", "");

                bool containsAnySearchString = searchFor.Any(word => compareString.Contains(word));

                if (containsAnySearchString == true)
                {
                    lblPopUpValidationMessage.Text = "* PO Box address is not allowed.";
                    return false;
                }
            }

            if (ddlAcademicDocument.SelectedValue == "0")
            {
                lblPopUpValidationMessage.Text = "* Please select Delivery Method.";
                rfvAcademicDocument.IsValid = false;
                return false;
            }

            if (ddlAcademicDocument.SelectedValue == "E")
            {
                if (String.IsNullOrWhiteSpace(ddlElectronicDestination.SelectedValue))
                {
                    lblPopUpValidationMessage.Text = "* Please select Electronic Copy Destination.";
                    pnlElectronicCopyDestination.Visible = true;
                    rfvElectronicDestination.IsValid = false;
                    return false;
                }

                if (ddlElectronicDestination.SelectedValue == "International" &&
                    (ddlCountryElectronicInternationalOrder.SelectedItem == null || ddlCountryElectronicInternationalOrder.SelectedValue == "0"))
                {
                    lblPopUpValidationMessage.Text = "* Please select Country.";
                    pnlElectronicCopyDestination.Visible = true;
                    pnlCountry.Visible = true;
                    rfvddlCountryElectronicInternationalOrder.IsValid = false;
                    return false;
                }
            }


            return true;
        }

        protected void btnSave_Add_Click(object sender, EventArgs e)
        {

            lblErrorMessage.Text = "";
            lblPopUpValidationMessage.Text = "";

            if (!AddressValidation())
            {
                popUpConfirmation.Update();
                ScriptManager.RegisterStartupScript(sender as Control, this.GetType(), "showNewRequestValidation", "$('#popUpConfirmationModal').modal('show');", true);
                return;
            }


            if (ddlAcademicDocument.SelectedValue == "S")
            {

                ddlCountry.SelectedItem.Value = "ZAF";
                ddlCountry.SelectedItem.Text = "South Africa";
            }

            if (ddlAcademicDocument.SelectedValue == "C")
            {

                DataTable dt = (DataTable)ViewState["Addresses"];


                bool ifCollectExist = false;

                foreach (DataRow row in dt.Rows)
                {
                    if (row["DeliveryMethod"].ToString() == "Collect")
                    {
                        ifCollectExist = true;
                    }
                }

                if (!ifCollectExist)
                {
                    dt.Rows.Add("", "", "", "", "", "", "", ddlAcademicDocument.SelectedItem.Text.Trim(), ddlAcademicDocument.SelectedItem.Value.Trim(), "0", "0", "0", "0", "");
                    ViewState["Addresses"] = dt;
                    BindGrid();

                }
                else
                {
                    lblPopUpValidationMessage.Text = "* Collect delivery method can only be select once per Request application.";
                }

            }

            //TODO: Refector this
            if (ddlAcademicDocument.SelectedValue == "E")
            {

                DataTable dt = (DataTable)ViewState["Addresses"];


                bool ifCollectExist = false;

                foreach (DataRow row in dt.Rows)
                {
                    if (row["DeliveryMethod"].ToString() == "Electronic Copy")
                    {
                        ifCollectExist = true;
                    }
                }

                if (!ifCollectExist)
                {
                    if (ddlElectronicDestination.SelectedItem.Value == "International")
                    {
                        string countryCode = ddlCountryElectronicInternationalOrder.SelectedItem.Value.Trim();

                        dt.Rows.Add("", "", "", "", "", "", countryCode , ddlAcademicDocument.SelectedItem.Text.Trim(), ddlAcademicDocument.SelectedItem.Value.Trim(), "0", "0", "0", "0", "");
                    }
                    else
                    {
                         dt.Rows.Add("", "", "", "", "", "", "ZAF", ddlAcademicDocument.SelectedItem.Text.Trim(),ddlAcademicDocument.SelectedItem.Value.Trim(), "0", "0", "0", "0", "");
                    }

                    ViewState["Addresses"] = dt;
                    BindGrid();

                }
                else
                {
                    lblPopUpValidationMessage.Text = "* Electronic Copy method can only be select once per Request application.";
                }

            }
            //TODO: Refector this


           
            if ((ddlAcademicDocument.SelectedValue == "S") || (ddlAcademicDocument.SelectedValue == "I"))
            {
                if ((txtStreet1.Text == "" || txtSuburb1.Text == "" || txtSuburb1.Text == "" || txtCity1.Text == "" || ddlCountry.SelectedItem.Value == "0"))
                {
                    lblPopUpValidationMessage.Text = "* Please capture all the required fields.";
                }
                else
                {
                    var s = "";

                    if (ddlAcademicDocument.SelectedValue == "S")
                    {
                        s = txtCode1.Text.Trim();
                    }
                    else
                    {
                        s = txtCode2.Text.Trim();
                    }



                    DataTable dt = (DataTable)ViewState["Addresses"];

                    bool ifExist = false;

                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["ComplexAddress"].ToString().Trim() == txtComplex1.Text.Trim() &&
                            row["StreetAddress"].ToString().Trim() == txtStreet1.Text.Trim() &&
                             row["Suburb"].ToString().Trim() == txtSuburb1.Text.Trim()
                             && row["City"].ToString().Trim() == txtCity1.Text.Trim()
                             && row["Code"].ToString().Trim() == s.ToString()
                              && row["Country"].ToString().Trim() == ddlCountry.SelectedItem.Text.Trim()
                              && row["CountryCode"].ToString().Trim() == ddlCountry.SelectedItem.Value.Trim())
                        {
                            ifExist = true;
                        }
                    }

                    if (!ifExist)
                    {

                        string Address = txtComplex1.Text.Trim() + "<br/>" + txtStreet1.Text.Trim() + "<br/>" + txtSuburb1.Text.Trim() + "<br/>" + txtCity1.Text.Trim() + "<br/>" + s.ToString() + "<br/>" + ddlCountry.SelectedItem.Text.Trim();

                        dt.Rows.Add(txtComplex1.Text.Trim(), txtStreet1.Text.Trim(), txtSuburb1.Text.Trim(), txtCity1.Text.Trim(), s.ToString(), ddlCountry.SelectedItem.Text.Trim(), ddlCountry.SelectedItem.Value.Trim(),
                                   ddlAcademicDocument.SelectedItem.Text.Trim(), ddlAcademicDocument.SelectedItem.Value.Trim(), "0", "0", "0", "0", Address.Trim());
                        ViewState["Addresses"] = dt;
                        BindGrid();



                        txtComplex1.Text = string.Empty;
                        txtStreet1.Text = string.Empty;
                        txtSuburb1.Text = string.Empty;
                        txtCity1.Text = string.Empty;
                        txtCode1.Text = string.Empty;
                        txtCode2.Text = string.Empty;
                        LoadCountry();

                    }
                    else
                    {
                        lblPopUpValidationMessage.Text = "* Added address is already in Addres list.";
                    }

                }
            }
            ddlAcademicDocument.SelectedIndex = 0;
            divAddress.Visible = false;

            decimal totalAmount = Calculate();
            txtTotalAmount.Text = "R " + totalAmount.ToString();

            if (gvAddress.Rows.Count >= 1)
            {
                Div39.Visible = false;
                Div10.Visible = true;
                txtTotalAmount.Text = !String.IsNullOrEmpty(txtTotalAmount.Text) ? txtTotalAmount.Text : "R 0";
            }

        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            AddNewRowToGrid();



            int index = Convert.ToInt32(e.RowIndex);
            DataTable dt = ViewState["Addresses"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["Addresses"] = dt;
            BindGrid();

            decimal totalAmount = Calculate();
            txtTotalAmount.Text = "R " + totalAmount.ToString();

            if (gvAddress.Rows.Count == 0)
            {
                Div39.Visible = true;
                Div10.Visible = false;
                txtTotalAmount.Text = "R 0";
            }
        }

        public bool FeildsValidationSearchPage()
        {

            if (txtID_PassNumber.Text == "")
            {
                lblErrorMessage1.Text = "* Please Enter ID/Passport number.";
                return false;
            }


            return true;
        }
        private void CheckAndLoadDraft(string studentNumber)
        {
            var token = HttpContext.Current?.Session["StudentJwtToken"] as string;
            try
            {
                string url = qualificationVerificationAPI + $"Student/GetStudentDraft?studentNumber={studentNumber}";
                System.Diagnostics.Debug.WriteLine($"API URL: {url}");

                var draftData = BusinessRules.HttpHelper.HttpCallJson<StudentDraftViewModel>(url, WebRequestMethods.Http.Get,token);
                System.Diagnostics.Debug.WriteLine("=== API Response Debug ===");
                System.Diagnostics.Debug.WriteLine($"API Call Status: {(draftData != null ? "Success" : "Failed - Null Response")}");
                if (draftData?.Student != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Successfully loaded draft for {draftData.Student.StudentNumber}");

                    // Store basic info
                    ViewState["DraftEmail"] = draftData.Student.EmailAddress;
                    ViewState["DraftPhone"] = draftData.Student.PhoneNumber;
                    ViewState["DraftCourier"] = draftData.Student.CourierInstructions;
                    ViewState["DraftPaymentMethod"] = draftData.Student.PaymentMethod;
                    ViewState["HasDraft"] = true;
                    Session["DraftStudentId"] = draftData.Student.StudentID != 0 ? draftData.Student.StudentID.ToString() : "0";

                    // Handle documents
                    if (draftData.AcademicDocuments != null && draftData.AcademicDocuments.Any())
                    {
                        var academicDictionary = draftData.AcademicDocuments
                            .Where(d => d.DeliveryInd != null) // Only keep academic documents with actual content
                            .GroupBy(d => d.DeliveryInd)
                            .ToDictionary(g => g.Key, g => g.First());

                        // Store as JSON to ensure reliable deserialization
                        ViewState["DraftAcademicDocuments"] = Newtonsoft.Json.JsonConvert.SerializeObject(academicDictionary);
                      
                    }
                    // Handle documents
                    if (draftData.Documents != null && draftData.Documents.Any())
                    {
                        var docDictionary = draftData.Documents
                            .Where(d => d.DocumentFile != null && d.DocumentFile.Length > 0) // Only keep documents with actual content
                            .GroupBy(d => d.DocumentTypeID)
                            .ToDictionary(g => g.Key, g => g.First());

                        // Store as JSON to ensure reliable deserialization
                        ViewState["DraftDocuments"] = Newtonsoft.Json.JsonConvert.SerializeObject(docDictionary);
                        System.Diagnostics.Debug.WriteLine($"Stored {docDictionary.Count} documents with content in ViewState");
                    }

                    // Load the data immediately
                    LoadDraftDataImmediately(draftData.Student);

                    divDraftInfo.Visible = true;

                   
                    string script = @"
                                    Swal.fire({
                                        title: 'Saved Draft Found',
                                        text: 'You have a saved draft. Would you like to continue where you left off?',
                                        icon: 'question',

                                        showCancelButton: true,

                                        confirmButtonColor: '#f2651c',
                                        cancelButtonColor: '#6c757d',

                                        confirmButtonText: 'Yes, continue',
                                        cancelButtonText: 'No, start fresh'

                                    }).then((result) => {
                                        if (result.isConfirmed) {
                                            __doPostBack('', 'LOAD_DRAFT');
                                        } else {
                                            __doPostBack('', 'CLEAR_DRAFT');
                                        }
                                    });
                                ";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showDraftMessage", script, true);

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Draft student is null - either no draft exists or API call failed");
                    ViewState["HasDraft"] = false;
                    divDraftInfo.Visible = false;
                    Session["DraftStudentId"] = "0";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex}");
                Logs.WriteErrorLog(ex);
            }
        }

        private void DeleteDraft(string studentNumber)
        {
                 var token = HttpContext.Current?.Session["StudentJwtToken"] as string;
                try
                {

                    string url = qualificationVerificationAPI + $"Student/GetStudentDraft?studentNumber={studentNumber}";

                    var draftData = BusinessRules.HttpHelper.HttpCallJson<StudentDraftViewModel>(url, WebRequestMethods.Http.Get,token);

                    if (draftData.Student != null)
                    {

                        var stdId = draftData.Student.StudentID.ToString();

                        var student = UpdateStudentDraft(stdId);

                        // Delete existing draft before saving a new one
                        string deleteUrl = qualificationVerificationAPI + "Student/DeleteDraftOrderAsync";

                        string JSONString = string.Empty;
                        JSONString = JsonConvert.SerializeObject(student);
                        validator = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(deleteUrl,
                            WebRequestMethods.Http.Post, JSONString,token);


                        if (validator.Status == Constants.Success)
                        {
                            DeleteDraftClearUp();

                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception: {ex}");
                    Logs.WriteErrorLog(ex);
                }
        }

        public void DeleteDraftClearUp()
        {
            txtEmail1.Text = "";
            txtPhone1.Text = "";
            txtSpecialInstruction.Value = "";
            txtIdPassport.Value = "";
            txtPaymentProof.Value = "";
            txtCourier1.Text = "";
            paymentMethod.ClearSelection();

            divDraftInfo.Visible = false;
            proofOfPaymentRow.Visible = true;
            btnSave.Text = "Submit";

            Session["FileUpload1"] = null;
            Session["FileUpload2"] = null;
            Session["FileUpload3"] = null;

            Session["CPFileUpload"] = null;
            Session["CPFileName"] = null;
            Session["SPFileName"] = null;
            Session["SPFileUpload"] = null;
            Session["PPFileName"] = null;
            Session["PPFileUpload"] = null;
            Session["IsDraft"] = false;
            Session["DraftStudentId"] = 0;

        }


        private void ShowMessage(string message, string type, string redirectUrl = null)
        {
            string escapedMessage = System.Web.HttpUtility.JavaScriptStringEncode(message);
            string icon = type.ToLower() == "success" ? "success" : "error";
            string position = "top-end";
            string bgColor = type.ToLower() == "success" ? "#28a745" : "#dc3545";
            string textColor = "#fff";
            string confirmButtonColor = "#f2651c";

            // If redirect URL provided, show button and redirect on click/timer
            string redirectScript = string.IsNullOrEmpty(redirectUrl) ? "" : $@"
        .then((result) => {{
            if (result.isConfirmed || result.dismiss === Swal.DismissReason.timer) {{
                window.location.href = '{redirectUrl}';
            }}
        }})";

            string script = $@"
                                Swal.fire({{
                                    icon: '{icon}',
                                    title: '{(icon == "success" ? "Success" : "Error")}',
                                    text: '{escapedMessage}',
                                    position: '{position}',
                                    background: '{bgColor}',
                                    color: '{textColor}',
                                    confirmButtonColor: '{confirmButtonColor}',
                                    showConfirmButton: {(!string.IsNullOrEmpty(redirectUrl)).ToString().ToLower()},
                                    confirmButtonText: 'Continue',
                                    timer: {(string.IsNullOrEmpty(redirectUrl) ? "5000" : "8000")},
                                    timerProgressBar: true,
                                    didOpen: (toast) => {{
                                        toast.addEventListener('mouseenter', Swal.stopTimer)
                                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                                    }}
                                }}){redirectScript};";

            ScriptManager.RegisterStartupScript(this, GetType(), "showMessage", script, true);
        }
        private void LoadDraftDataImmediately(Student draftStudent)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== Starting LoadDraftDataImmediately ===");

                // Basic info loading (unchanged)
                txtEmail1.Text = draftStudent.EmailAddress ?? string.Empty;
                txtPhone1.Text = draftStudent.PhoneNumber ?? string.Empty;
                txtCourier1.Text = draftStudent.CourierInstructions ?? string.Empty;

                // Payment method (unchanged)
                if (!string.IsNullOrEmpty(draftStudent.PaymentMethod))
                {
                    paymentMethod.ClearSelection();
                    var item = paymentMethod.Items.FindByValue(draftStudent.PaymentMethod);
                    if (item != null) item.Selected = true;

                    if (draftStudent.PaymentMethod == "Bank Deposit" || string.IsNullOrEmpty(draftStudent.PaymentMethod))
                    {
                        proofOfPaymentRow.Visible = true;
                        btnSave.Text = "Submit";
                    }
                    else if (draftStudent.PaymentMethod == "Online Payment")
                    {
                        proofOfPaymentRow.Visible = false;
                        btnSave.Text = "Pay Now & Submit";
                    }
                }

                // NEW: Robust ViewState document handling
                if (ViewState["DraftAcademicDocuments"] != null)
                {
                    System.Diagnostics.Debug.WriteLine("ViewState contains DraftAcademicDocuments");

                    try
                    {
                        var json = ViewState["DraftAcademicDocuments"].ToString();
                        System.Diagnostics.Debug.WriteLine($"JSON length: {json.Length}");

                        // Try JSON deserialization if binary fails
                        var academicDocuments = Newtonsoft.Json.JsonConvert
                            .DeserializeObject<Dictionary<string, StudentAcademicDocumentOrderDto>>(json);

                       

                        if (academicDocuments != null && academicDocuments.Any())
                        {
                            

                            var academicDocumentList = academicDocuments.Values.ToList();

                            System.Diagnostics.Debug.WriteLine($"Successfully loaded {academicDocumentList.Count} academic documents from dictionary");

                            // Document loading logic
                            LoadSavedAddresses(academicDocumentList);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Failed to deserialize academic documents");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Academic Documents deserialization error: {ex.Message}");
                    }
                }


                // NEW: Robust ViewState document handling
                if (ViewState["DraftDocuments"] != null)
                {
                    System.Diagnostics.Debug.WriteLine("ViewState contains DraftDocuments");

                    try
                    {
                        // Try JSON deserialization if binary fails
                        var documents = ViewState["DraftDocuments"] as Dictionary<int, DocumentDto>;

                        if (documents == null)
                        {
                            System.Diagnostics.Debug.WriteLine("Binary deserialization failed, trying JSON");
                            var json = ViewState["DraftDocuments"].ToString();
                            documents = Newtonsoft.Json.JsonConvert
                                .DeserializeObject<Dictionary<int, DocumentDto>>(json);
                        }

                        if (documents != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"Successfully loaded {documents.Count} documents");

                            // Document loading logic
                            LoadDocumentsFromDictionary(documents);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Failed to deserialize documents");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Document deserialization error: {ex.Message}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No DraftDocuments in ViewState");
                }

                System.Diagnostics.Debug.WriteLine("=== Completed LoadDraftDataImmediately ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {ex}");
                Logs.WriteErrorLog(ex);
            }
        }

        private void LoadDocumentsFromDictionary(Dictionary<int, DocumentDto> documents)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== Loading documents from dictionary ===");

                // ID/Passport
                if (documents.TryGetValue((int)FeesGrantDocumentType.IDCopy, out var idDoc) && idDoc != null)
                {
                    txtIdPassport.Value = idDoc.Document1;
                    Session["CPFileName"] = idDoc.Document1;
                    if (idDoc.DocumentFile != null && idDoc.DocumentFile.Length > 0)
                    {
                        //Session["IdPassportFile"] = idDoc.DocumentFile;
                        Session["CPFileUpload"] = idDoc.DocumentFile;
                        System.Diagnostics.Debug.WriteLine($"Loaded ID document file ({idDoc.DocumentFile.Length} bytes)");
                    }
                }

                // Special Instruction
                if (documents.TryGetValue((int)FeesGrantDocumentType.SpecialInstruction, out var siDoc) && siDoc != null)
                {
                    txtSpecialInstruction.Value = siDoc.Document1;
                    Session["SPFileName"] = siDoc.Document1;
                    if (siDoc.DocumentFile != null && siDoc.DocumentFile.Length > 0)
                    {
                        //Session["SpecialInstructionFile"] = siDoc.DocumentFile;
                        Session["SPFileUpload"] = siDoc.DocumentFile;
                        System.Diagnostics.Debug.WriteLine($"Loaded Special Instruction file ({siDoc.DocumentFile.Length} bytes)");
                    }
                }

                // Payment Proof
                if (documents.TryGetValue((int)FeesGrantDocumentType.PaymentProof, out var ppDoc) && ppDoc != null)
                {
                    txtPaymentProof.Value = ppDoc.Document1;
                    Session["PPFileName"] = ppDoc.Document1;
                    if (ppDoc.DocumentFile != null && ppDoc.DocumentFile.Length > 0)
                    {
                        //Session["PaymentProofFile"] = ppDoc.DocumentFile;
                        Session["PPFileUpload"] = ppDoc.DocumentFile;
                        System.Diagnostics.Debug.WriteLine($"Loaded Payment Proof file ({ppDoc.DocumentFile.Length} bytes)");
                    }
                }

                UpdateFileDisplayScript();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading documents: {ex}");
                Logs.WriteErrorLog(ex);
            }
        }

        private void UpdateFileDisplayScript()
        {
            string script = $@"
                            try {{
                                console.log('Attempting to update file displays');
                                {GetFileUpdateScript(IdPassport.ClientID, txtIdPassport.Value)}
                                {GetFileUpdateScript(SpecialInstruction.ClientID, txtSpecialInstruction.Value)}
                                {GetFileUpdateScript(PaymentProof.ClientID, txtPaymentProof.Value)}
                                console.log('File displays updated');
                            }} catch(e) {{
                                console.error('Error updating file displays:', e);
                            }}";

            ScriptManager.RegisterStartupScript(this, GetType(), "updateFileDisplay", script, true);
        }

        private string GetFileUpdateScript(string controlId, string fileName)
        {
            return $"updateFileInputDisplay('{controlId}', '{HttpUtility.JavaScriptStringEncode(fileName)}');";
        }
        public class StudentDraftViewModel
        {
            public Student Student { get; set; } = new Student();
            public List<DocumentDto> Documents { get; set; } = new List<DocumentDto>();
            public List<StudentAcademicDocumentOrderDto> AcademicDocuments { get; set; } = new List<StudentAcademicDocumentOrderDto>();
        }
        
        protected async void btnNext_Click(object sender, EventArgs e)
        {
            Session["StudentJwtToken"] = null;

            if (!FeildsValidationSearchPage())
                return;


            string studentnum1 = "";
            string idpassnum = "";

            lblErrorMessage1.Text = string.Empty;


            Session["ID_PassNumber"] = txtID_PassNumber.Text;


            idpassnum = txtID_PassNumber.Text;

            var apiResponse = await ApiClient.GetStudentJsonToken(idpassnum);

            if (apiResponse != null && !string.IsNullOrEmpty(apiResponse.Token))
            {
                // Store JWT token in session
                Session["StudentJwtToken"] = apiResponse.Token;
                Session["UserRoleData"] = apiResponse.Roles;

                // Also create Forms Authentication ticket for compatibility
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    apiResponse.StudentNum,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    apiResponse.Roles,
                    FormsAuthentication.FormsCookiePath
                );

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(authCookie);

                if (Session["ID_PassNumber"].ToString() != "")
                {
                    GetStudentQualification(null, idpassnum);
                }

                studentnum1 = txtStudentnumber1.Text.ToString();

                if (studentnum1 == "")
                {
                    divUserInformation.Visible = true;
                    divParentsInfo.Visible = false;
                    lblErrorMessage1.Text =
                        "* This is an invalid ID or Passport number. Please ensure you are using the ID or Passport number as it appears on your academic record/qualification certificate.";
                    //txtStudentNumber.Text = string.Empty;
                    txtID_PassNumber.Text = string.Empty;
                }
                else
                {
                    string url = qualificationVerificationAPI + "Student/GetStudentFeeStatus?studentNumber=" +
                                 studentnum1;
                    var feesStatus =
                        BusinessRules.HttpHelper.HttpCallJson<StudentFeeStatusData>(url, WebRequestMethods.Http.Get,
                            apiResponse.Token);



                    if (feesStatus.OutStandingFeeStatus == "N" || feesStatus.OutStandingFeeStatus == null || feesStatus == null)
                    {
                        divUserInformation.Visible = false;
                        divParentsInfo.Visible = true;
                        lblErrorMessage1.Text = "";

                        // Store student number
                        Session["StudentNumber"] = studentnum1;

                        // Call CheckAndLoadDraft directly instead of using postback
                        CheckAndLoadDraft(studentnum1);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(sender as Control, this.GetType(), "successPopup2",
                            "$('#myModal').modal('show');", true);
                    }
                }
            }
            else
            {
                divUserInformation.Visible = true;
                divParentsInfo.Visible = false;
                lblErrorMessage1.Text =
                    "* This is an invalid ID or Passport number. Please ensure you are using the ID or Passport number as it appears on your academic record/qualification certificate.";
               
                txtID_PassNumber.Text = string.Empty;
            }
        }

        protected void btnCancelPopUp_Click(object sender, EventArgs e)
        {
            
                ScriptManager.RegisterStartupScript(sender as Control, this.GetType(), "successPopup1", "$('#popUpConfirmationModal').removeClass('fade').modal('hide');", true);
        }

        protected void gvQualificationShow_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvQualificationShow.PageIndex = e.NewPageIndex;

            string idpassnum = "";


            idpassnum = Session["ID_PassNumber"].ToString();

            if (idpassnum != "")
            {

                GetStudentQualification(null, idpassnum);

            }
        }

        protected void btnAddAddress_Click(object sender, EventArgs e)
        {

            divAcademicDocumentMulti.Visible = true;
            AddressTitle.Visible = true;
            btnSave_Add.Visible = true;

            btnConfirm.Visible = false;
            ConfirmTitle.Visible = false;
            messageshow.Visible = false;

            txtComplex1.Text = string.Empty;
            txtStreet1.Text = string.Empty;
            txtSuburb1.Text = string.Empty;
            txtCity1.Text = string.Empty;
            txtCode1.Text = string.Empty;
            txtCode2.Text = string.Empty;
            LoadCountry();

            pnlElectronicCopyDestination.Visible = false;
            pnlCountry.Visible = false;
            ddlAcademicDocument.ClearSelection();
            ddlElectronicDestination.ClearSelection();
            ddlCountryElectronicInternationalOrder.Items.Clear();

            popUpConfirmation.Update();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1", "$('#popUpConfirmationModal').modal('show');", true);

        }


        private decimal Calculate()
        {
           

            decimal totalAmount = 0;

            foreach (GridViewRow row in gvAddress.Rows)
            {
                string ac_num = "0";
                string ts_num = "0";
                string cl_num = "0";
                string forms_num = "0";
                string deliveryInd = "";
                string degree = "";
                decimal FeeAmount = 0;
                decimal CollectFee = 0;
                decimal totalac = 0;
                decimal totalts = 0;
                decimal totalcl = 0;
                decimal totalfm = 0;
                decimal totalsendfee = 0;

                ac_num = ((DropDownList)row.FindControl("ddlNumberofAcademicRecord")).SelectedItem.Value;
                ts_num = ((DropDownList)row.FindControl("ddlNumberofTranscriptSupplement")).SelectedItem.Value;
                cl_num = ((DropDownList)row.FindControl("ddlNumberofConfirmationLetter")).SelectedItem.Value;
                forms_num = ((DropDownList)row.FindControl("ddlNumberofFormsForOfficialBodies")).SelectedItem.Value;


                deliveryInd = ((HiddenField)row.FindControl("DeliveryInd")).Value;


                if (ac_num != "0" || ts_num != "0" || cl_num != "0" || forms_num != "0")
                {
                    degree = "Academic Record (Modules plus Results)";

                    string url = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" + deliveryInd + "&documentType=" + degree;
                    FeeAmount = (decimal)BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url, WebRequestMethods.Http.Get, null).D_DocumentFee;

                    string url1 = qualificationVerificationAPI + "Student/GetCollectFees?documentType=" + degree;
                    CollectFee = (decimal)BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url1, WebRequestMethods.Http.Get, null).D_DocumentFee;

                    if (deliveryInd != "C")
                    { totalsendfee += (FeeAmount - CollectFee); }

                }


                if (ac_num != "0")
                {
                    degree = "Academic Record (Modules plus Results)";

                    string url = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" + deliveryInd + "&documentType=" + degree;
                    FeeAmount = (decimal)BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url, WebRequestMethods.Http.Get, null).D_DocumentFee;

                    string url1 = qualificationVerificationAPI + "Student/GetCollectFees?documentType=" + degree;
                    CollectFee = (decimal)BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url1, WebRequestMethods.Http.Get, null).D_DocumentFee;

                    if (deliveryInd != "C")
                    { totalac += /*(FeeAmount - CollectFee) +*/ (CollectFee * int.Parse(ac_num)); }
                    else
                    { totalac += FeeAmount * int.Parse(ac_num); }


                }

                if (ts_num != "0")
                {
                    degree = "Academic Transcript Supplement (Course content including Academic Record)";

                    string url = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" + deliveryInd + "&documentType=" + degree;
                    FeeAmount = (decimal)BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url, WebRequestMethods.Http.Get, null).D_DocumentFee;

                    string url1 = qualificationVerificationAPI + "Student/GetCollectFees?documentType=" + degree;
                    CollectFee = (decimal)BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url1, WebRequestMethods.Http.Get, null).D_DocumentFee;


                    if (deliveryInd != "C")
                    { totalts += /*(FeeAmount - CollectFee) +*/(CollectFee * int.Parse(ts_num)); }
                    else
                    { totalts += FeeAmount * int.Parse(ts_num); }

                }

                if (cl_num != "0")
                {
                    degree = "Confirmation Letter (General letters)";

                    string url = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" + deliveryInd + "&documentType=" + degree;
                    FeeAmount = (decimal)BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url, WebRequestMethods.Http.Get, null).D_DocumentFee;

                    string url1 = qualificationVerificationAPI + "Student/GetCollectFees?documentType=" + degree;
                    CollectFee = (decimal)BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url1, WebRequestMethods.Http.Get, null).D_DocumentFee;

                    if (deliveryInd != "C")
                    { totalcl += /*(FeeAmount - CollectFee) +*/ (CollectFee * int.Parse(cl_num)); }
                    else
                    { totalcl += FeeAmount * int.Parse(cl_num); }

                }

                if (forms_num != "0")
                {
                    degree = "Forms for Official Bodies";

                    string url = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" + deliveryInd + "&documentType=" + degree;
                    FeeAmount = (decimal)BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url, WebRequestMethods.Http.Get, null).D_DocumentFee;

                    string url1 = qualificationVerificationAPI + "Student/GetCollectFees?documentType=" + degree;
                    CollectFee = (decimal)BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url1, WebRequestMethods.Http.Get, null).D_DocumentFee;

                    if (deliveryInd != "C")
                    { totalfm += /*(FeeAmount - CollectFee) +*/ (CollectFee * int.Parse(forms_num)); }
                    else
                    { totalfm += FeeAmount * int.Parse(forms_num); }

                }
                totalAmount += ((totalac + totalts + totalcl + totalfm) + totalsendfee);
            }

            return totalAmount;
        }
        private void AddNewRowToGrid()
        {

            if (ViewState["Addresses"] != null)

            {

                DataTable dt = (DataTable)ViewState["Addresses"];


                if (dt.Rows.Count > 0)

                {


                    ViewState["Addresses"] = dt;



                    for (int i = 0; i < dt.Rows.Count; i++)

                    {

                        //extract the DropDownList Selected Items

                        DropDownList ddl1 = (DropDownList)gvAddress.Rows[i].Cells[8].FindControl("ddlNumberofAcademicRecord");
                        if (ddl1 != null)
                        {
                            string selectedValue = dt.Rows[i]["Academic Record"].ToString();
                            ListItem selectedItem = ddl1.Items.FindByText(selectedValue); // Check by text
                            if (selectedItem != null)
                            {
                                ddl1.SelectedValue = selectedItem.Value;
                            }
                            else
                            {
                                // Handle the case where the value doesn't exist in the dropdown
                                ddl1.SelectedIndex = 0; // Set to a default value (or handle accordingly)
                            }
                        }



                        DropDownList ddl2 = (DropDownList)gvAddress.Rows[i].Cells[9].FindControl("ddlNumberofTranscriptSupplement");

                        DropDownList ddl3 = (DropDownList)gvAddress.Rows[i].Cells[10].FindControl("ddlNumberofConfirmationLetter");
                        DropDownList ddl4 = (DropDownList)gvAddress.Rows[i].Cells[11].FindControl("ddlNumberofFormsForOfficialBodies");


                        // Update the DataRow with the DDL Selected Items

                        dt.Rows[i]["Academic Record"] = ddl1.SelectedItem.Text;

                        dt.Rows[i]["Transcript Supplement"] = ddl2.SelectedItem.Text;

                        dt.Rows[i]["Confirmation Letter"] = ddl3.SelectedItem.Text;

                        dt.Rows[i]["Forms for Official Bodies"] = ddl4.SelectedItem.Text;

                    }

                    //Rebind the Grid with the current data

                    gvAddress.DataSource = dt;

                    gvAddress.DataBind();

                }

            }

            else

            {

                Response.Write("ViewState is null");

            }

            //Set Previous Data on Postbacks

            SetPreviousData();

        }

        private void SetPreviousData()

        {

            int rowIndex = 0;

            if (ViewState["Addresses"] != null)

            {

                DataTable dt = (DataTable)ViewState["Addresses"];

                if (dt.Rows.Count > 0)

                {

                    for (int i = 0; i < dt.Rows.Count; i++)

                    {

                        //Set the Previous Selected Items on Each DropDownList on Postbacks

                        DropDownList ddl1 = (DropDownList)gvAddress.Rows[i].Cells[1].FindControl("ddlNumberofAcademicRecord");

                        DropDownList ddl2 = (DropDownList)gvAddress.Rows[i].Cells[2].FindControl("ddlNumberofTranscriptSupplement");

                        DropDownList ddl3 = (DropDownList)gvAddress.Rows[i].Cells[3].FindControl("ddlNumberofConfirmationLetter");
                        DropDownList ddl4 = (DropDownList)gvAddress.Rows[i].Cells[4].FindControl("ddlNumberofFormsForOfficialBodies");


                        if (i < dt.Rows.Count - 1)

                        {

                            ddl1.ClearSelection();

                            ddl1.Items.FindByText(dt.Rows[i]["Academic Record"].ToString()).Selected = true;



                            ddl2.ClearSelection();

                            ddl2.Items.FindByText(dt.Rows[i]["Transcript Supplement"].ToString()).Selected = true;



                            ddl3.ClearSelection();

                            ddl3.Items.FindByText(dt.Rows[i]["Confirmation Letter"].ToString()).Selected = true;

                            ddl4.ClearSelection();

                            ddl4.Items.FindByText(dt.Rows[i]["Forms for Official Bodies"].ToString()).Selected = true;


                        }



                        rowIndex++;

                    }

                }

            }

        }

        protected void btnRequestClose_Click(object sender, EventArgs e)
        {
            Session["AcceptedTerms"] = null;

            Session["Completed"] = null;
            Response.Redirect("~/Default.aspx");

            ScriptManager.RegisterStartupScript(sender as Control, this.GetType(), "successPopup2", "$('#myModal').modal('hide');", true);
        }

        protected void lbtnAddAnotherAddress_Click(object sender, EventArgs e)
        {

            AddNewRowToGrid();



            divAcademicDocumentMulti.Visible = true;
            AddressTitle.Visible = true;
            btnSave_Add.Visible = true;

            btnConfirm.Visible = false;
            ConfirmTitle.Visible = false;
            messageshow.Visible = false;
            pnlElectronicCopyDestination.Visible = false;
            pnlCountry.Visible = false;

            txtComplex1.Text = string.Empty;
            txtStreet1.Text = string.Empty;
            txtSuburb1.Text = string.Empty;
            txtCity1.Text = string.Empty;
            txtCode1.Text = string.Empty;
            txtCode2.Text = string.Empty;
            
            LoadCountry();

            popUpConfirmation.Update();
            ScriptManager.RegisterStartupScript(sender as Control, this.GetType(), "successPopup1", "$('#popUpConfirmationModal').modal('show');", true);
        }

        protected void gvAddress_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //LinkButton lnk = e.Row.FindControl("lbtnAddAnotherAddress") as LinkButton;


                if (gvAddress.Rows.Count > 2)
                {

                    foreach (GridViewRow row in gvAddress.Rows)
                    {

                        var lnk = row.FindControl("lbtnAddAnotherAddress") as LinkButton;
                        if (lnk != null)
                        {
                            lnk.Visible = false;
                        }
                    }
                    LinkButton last = e.Row.FindControl("lbtnAddAnotherAddress") as LinkButton;
                    if (last != null)
                    {
                        last.Visible = false;
                    }


                }

                //Wire up dropdownList selectedIndexChanged event
                DropDownList ddlForms = e.Row.FindControl("ddlNumberofFormsForOfficialBodies") as DropDownList;
                if (ddlForms != null)
                {
                    ddlForms.AutoPostBack = true;
                    ddlForms.SelectedIndexChanged += ddlNumberofFormsForOfficialBodies_SelectedIndexChanged;
                }


            }


        }

        protected void btnSpecialInstructionCancelPopUp_Click1(object sender, EventArgs e)
        {
            Session["SpecialInstructionsInd"] = null;

        }

        protected void ddlNumberofConfirmationLetter_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal totalAmount = Calculate();
            txtTotalAmount.Text = "R " + totalAmount.ToString();
        }

        protected void ddlNumberofTranscriptSupplement_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal totalAmount = Calculate();
            txtTotalAmount.Text = "R " + totalAmount.ToString();
        }

        protected void ddlNumberofAcademicRecord_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal totalAmount = Calculate();
            txtTotalAmount.Text = "R " + totalAmount.ToString();
        }
        protected void ddlNumberofFormsForOfficialBodies_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal totalAmount = Calculate();
            txtTotalAmount.Text = "R " + totalAmount.ToString();

            //Get selected value from dropdown
            DropDownList ddl = sender as DropDownList;

            if (ddl != null && int.TryParse(ddl.SelectedValue, out int selectedValue))
            {
                //Enable RequiredFieldValidator if 1-4 forms for official bodies selected
                rfvSpecialInstruction.Enabled = selectedValue >= 1 && selectedValue <= 4;

                //trigger JS after postback
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "checkSpecial",
                    $"updateOutsideAsterisk();",
                    true
                    );
            }

        }

        protected void paymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPaymentMethod = paymentMethod.SelectedValue;

            if (selectedPaymentMethod == "Bank Deposit" || string.IsNullOrEmpty(selectedPaymentMethod))
            {
                proofOfPaymentRow.Visible = true;
                btnSave.Text = "Submit";
                //btnSave.Visible = true;
                //btnPaySave.Visible = false;
            }
            else if (selectedPaymentMethod == "Online Payment")
            {
                proofOfPaymentRow.Visible = false;
                btnSave.Text = "Pay Now & Submit";
                //btnPaySave.Visible = true;
            }
        }

        private void OnlinePaymentStep(ViewModels.Validator validator)
        {
            var token = HttpContext.Current?.Session["StudentJwtToken"] as string;

            int studentIdInt = int.Parse(Session["StudentID"].ToString());

            string url1 = qualificationVerificationAPI + "Student/GetStudent?studentId=" + studentIdInt;
            var student = BusinessRules.HttpHelper.HttpCallJson<Student>(url1, WebRequestMethods.Http.Get, token);


            if (student != null)
            {
                student.StudentStatus = 7;

                string url = qualificationVerificationAPI + "Student/UpdateStudent";
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(student);
                BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url, WebRequestMethods.Http.Post, JSONString,token);

                Logs.WriteErrorLog("Update student table status option (7 seven) : " + student.StudentStatus.ToString());
            }

            bool paymentResult = PayGate();

            if (paymentResult)
            {
                RecordSaveInBD(validator);

                txtPaymentProof.Value = "";
                txtSpecialInstruction.Value = "";
                txtIdPassport.Value = "";
                Session["FileUpload1"] = null;
                Session["FileUpload2"] = null;
                Session["FileUpload3"] = null;

                Session["Completed"] = null;

                Session["__SSuccessSubmit@!#"] = true;

            }
            else
            {
                Session["AcceptedTerms"] = null;

                Session["Completed"] = null;

                Session["TryAgain"] = null;
            }

        }

        public static byte[] SafeGetBytes(object rawData)
        {
            if (rawData == null || rawData == DBNull.Value)
                return null;

            if (rawData is byte[] bytes)
                return bytes;

            if (rawData is string s)
            {
                // JSON -> Base64 -> bytes
                try
                {
                    return Convert.FromBase64String(s);
                }
                catch (FormatException)
                {
                    Logs.WriteErrorLog("DocumentFile string was not valid Base64. File already corrupted.");
                    return null;
                }
            }

            Logs.WriteErrorLog($"Cannot convert type {rawData.GetType()} to byte[].");
            return null;
        }

        private void LoadSavedAddresses(List<StudentAcademicDocumentOrderDto> savedAddresses)
        {
            try
            {
                // Initialize DataTable with correct structure
                DataTable dt = InitializeDataTable();

                // Add saved addresses
                foreach (var address in savedAddresses)
                {
                    // Apply the same validation logic as in Save method
                    if (address.DeliveryInd == "C") // Collect
                    {
                        bool ifCollectExist = false;
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["DeliveryMethod"].ToString() == "Collect")
                            {
                                ifCollectExist = true;
                                break;
                            }
                        }

                        if (!ifCollectExist)
                        {
                            dt.Rows.Add(ConvertToDataRow(address, dt));
                        }
                        else
                        {
                            lblErrorMessage.Text = "* Collect delivery method can only be selected once per Request application.";
                        }
                    }
                    else if (address.DeliveryInd == "E") // Electronic Copy
                    {
                        bool ifElectronicExist = false;
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["DeliveryMethod"].ToString() == "Electronic Copy")
                            {
                                ifElectronicExist = true;
                                break;
                            }
                        }

                        if (!ifElectronicExist)
                        {
                            dt.Rows.Add(ConvertToDataRow(address, dt));
                        }
                        else
                        {
                            lblErrorMessage.Text = "* Electronic Copy method can only be selected once per Request application.";
                        }
                    }
                    else
                    {
                        // For other delivery methods, just add them
                        dt.Rows.Add(ConvertToDataRow(address, dt));
                    }
                }

                // Save to ViewState and bind to grid
                ViewState["Addresses"] = dt;
                BindGrid();

                // Calculate total amount
                decimal totalAmount = Calculate();
                txtTotalAmount.Text = "R " + totalAmount.ToString();

                // Show/hide UI elements based on grid rows
                if (gvAddress.Rows.Count >= 1)
                {
                    Div39.Visible = false;
                    Div10.Visible = true;
                    txtTotalAmount.Text = !String.IsNullOrEmpty(txtTotalAmount.Text) ? txtTotalAmount.Text : "R 0";
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = $"Error loading saved addresses: {ex.Message}";
                // Log the error
            }
        }

        private DataRow ConvertToDataRow(StudentAcademicDocumentOrderDto address, DataTable dt)
        {
            DataRow row = dt.NewRow();

           
            row["ComplexAddress"] = address.ComplexAddress;
            row["StreetAddress"] = address.StreetAddress;
            row["Suburb"] = address.Suburb;
            row["City"] = address.City;
            row["Code"] = address.Code;
            row["Country"] = address.Country;
            row["CountryCode"] = address.CountryCode;

            row["DeliveryMethod"] = address.DeliveryMethod;
            row["DeliveryInd"] = address.DeliveryInd;

            row["Academic Record"] = address.AcademicRecord;
            row["Transcript Supplement"] = address.TranscriptSupplement;
            row["Confirmation Letter"] = address.ConfirmationLetter;
            row["Forms For Official Bodies"] = address.FormsForOfficialBodies; 
            row["Address"] = address.Address; 

            return row;
        }

        private DataTable InitializeDataTable()
        {
            DataTable dt = new DataTable();

            // 1-7: Address components (hidden in grid)
            dt.Columns.Add("ComplexAddress", typeof(string));
            dt.Columns.Add("StreetAddress", typeof(string));
            dt.Columns.Add("Suburb", typeof(string));
            dt.Columns.Add("City", typeof(string));
            dt.Columns.Add("Code", typeof(string));
            dt.Columns.Add("Country", typeof(string));
            dt.Columns.Add("CountryCode", typeof(string));

            // 8: Combined Address (displayed as "Send to Address")
            

            // 9: DeliveryMethod
            dt.Columns.Add("DeliveryMethod", typeof(string));

            // 10: DeliveryInd (hidden field value)
            dt.Columns.Add("DeliveryInd", typeof(string));

            // 11-14: Document counts
            dt.Columns.Add("Academic Record", typeof(string));
            dt.Columns.Add("Transcript Supplement", typeof(string));
            dt.Columns.Add("Confirmation Letter", typeof(string));
            dt.Columns.Add("Forms For Official Bodies", typeof(string));
            dt.Columns.Add("Address", typeof(string));

            return dt;
        }
    }
}