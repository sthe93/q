using QualificationVerificationWeb.Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using QualificationVerificationWeb.ViewModels;
using System.Web;
using QualificationVerificationWeb.Helper;

namespace QualificationVerificationWeb
{
    public partial class _Default : Page
    {
        private readonly string qualificationVerificationAPI = ConfigurationManager.AppSettings["QualificationVerificationAPI"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            // Add this line
            DateTime cutOffDate = new DateTime(2021, 01, 04);

            // Fetch active alerts
            var activeAlerts = GetActiveAlerts();
            bool hasActiveAlerts = activeAlerts != null && activeAlerts.Any();

            // Update the showNotice value in the cache
            UpdateShowNoticeCache(hasActiveAlerts);

            // Read the showNotice value from the cache
            string showNoticeValue = HttpContext.Current.Cache["showNotice"] as string;
            bool showNotice = showNoticeValue?.ToLower() == "yes";

            // Update UI based on showNotice value
            importantNoticeTable.Visible = showNotice;
            importantNoticeDiv.Visible = showNotice;

            if (!IsPostBack)
            {
                Session.Clear();
                Session["__SSuccessSubmit@!#"] = null;
                BankingInfo();
                GetDocumentTypePrice();


                if (DateTime.Now <= cutOffDate)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1", "$('#mySpecialMessageModal').modal('show');", true);
                    chkAgree.Enabled = false;
                }

                if (hasActiveAlerts)
                {
                    importantNoticeDiv.InnerHtml = FormatAlert(activeAlerts.First());
                }


            }

            Session["AcceptedTerms"] = null;

            if (chkAgree.Checked == true && (chkthirdparty.Checked == true || chkujStudent.Checked == true))
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1", "$('#myModal').modal('hide');", true);
            }
        }

        private void UpdateShowNoticeCache(bool hasActiveAlerts)
        {
            // Update the cache with the latest showNotice value
            HttpContext.Current.Cache["showNotice"] = hasActiveAlerts ? "yes" : "no";
        }

        private List<ApplicationAlert> GetActiveAlerts()
        {
            string url = qualificationVerificationAPI + "ApplicationAlert/GetActiveAlerts";
            var activeAlerts = BusinessRules.HttpHelper.HttpCallJson<List<ApplicationAlert>>(url, WebRequestMethods.Http.Get, null);
            return activeAlerts;
        }

        private string FormatAlert(ApplicationAlert alert)
        {
            return $@"
            <h1 style='color: red; font-size: 18px;'><strong>Important Notice:</strong></h1>
            <br>
            <b><p style='font-size: 14px;'>{alert.Message}</p></b>
            <b><p style='font-size: 14px;'>To ensure business continuity, please email your request to <a href='mailto:transcripts@uj.ac.za'>transcripts@uj.ac.za</a> for assistance.</p></b>
            <br />
            <b><p style='font-size: 14px;'>We apologise for the inconvenience caused.</p></b>
            <br>
            <b><p style='font-size: 14px;'>Yours Sincerely,</p></b>
            <h3 style='font-size: 16px;'><strong>Corporate Governance</strong></h3>";
        }




        public void GetDocumentTypePrice()
        {
            string url = qualificationVerificationAPI + "Student/GetDocumentTypePrice";
            var deliveryTypePriceList = BusinessRules.HttpHelper.HttpCallJson<List<GetAcademicDocumentTypePrice>>(url, WebRequestMethods.Http.Get, null).ToList();

            // Create a DataTable to hold the data
            DataTable dt = new DataTable();
            dt.Columns.Add("DocumentType");
            dt.Columns.Add("Collect");
            dt.Columns.Add("Courier___South_Africa");
            dt.Columns.Add("Courier___International");
            dt.Columns.Add("TurnAroundTime");

            if (deliveryTypePriceList != null)
            {
                foreach (var item in deliveryTypePriceList)
                {
                    string documentType = item.DocumentType;

                    // Check if the document type is "Forms For Official Bodies"
                    if (documentType == "Forms For Official Bodies")
                    {
                        // Append the additional information
                        documentType = "Forms For Official Bodies (e.g. WES, CORU, CGFNS)";
                    }

                    dt.Rows.Add(
                        documentType,
                        item.Collect,
                        item.Courier___South_Africa,
                        item.Courier___International,
                        item.TurnAroundTime);
                }

                // Store the DataTable in ViewState
                ViewState["DocumentPrice"] = dt;
                BindGrid();
            }
        }


        public void BankingInfo()
        {
            string url = qualificationVerificationAPI + "BankingDetails/GetBankInfo";
            var getBanking = BusinessRules.HttpHelper.HttpCallJson<BankingDetail>(url, WebRequestMethods.Http.Get, null);


            //var getBanking = detailsRepository.GetBankInfo();

            AccountName.Text = getBanking.AccountName;
            Bank.Text = getBanking.BankName;
            Branch.Text = getBanking.BranchName;
            BranchCode.Text = getBanking.BranchCode;
            AccountNumber.Text = getBanking.AccountNumber;
            //Reference.Text = getBanking.Reference;
        }
        protected void btnTnC_Click(object sender, EventArgs e)
        {

            lblErrorMessage.Text = "";

            //popupModal.Update();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1", "$('#myModal').modal('show');", true);
        }


        protected void btnCancelAgree_Click(object sender, EventArgs e)
        {
            ///implementation
        }
        protected void btnAgree_Click(object sender, EventArgs e)
        {
            // Store anything in session if needed
            Session["AcceptedPrivacy"] = "Yes";

            // Redirect with anchor to scroll to divUserInformation
            ScriptManager.RegisterStartupScript(this, this.GetType(), "redirectToUserInfo",
                "window.location='DocumentOrder.aspx#divUserInformation';", true);
        }

        protected void chkAccept_CheckedChanged(object sender, EventArgs e)
        {

            //popupModal.Update();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1", "$('#myModal').modal('show');", true);

            if (chkAgree.Checked == true)
            {
                lblErrorMessage.Text = "";
                btnNext.Enabled = true;
                Session["AcceptedTerms"] = "Yes";
            }
            else
            {
                btnNext.Enabled = false;
                Session["AcceptedTerms"] = null;
            }
        }
        //Here we define the agree button
        protected void btnContinue_Click(object sender, EventArgs e)
        {


            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1", "$('#myModal').removeClass('fade').modal('hide');", true);
            Response.Redirect("~/DocumentOrder.aspx", false);
            Context.ApplicationInstance.CompleteRequest();



        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            if (chkAgree.Checked == false)
            {
                lblErrorMessage.Text = "* Please Agree to the Terms and Conditions.";
            }
            else
            {
                if (chkthirdparty.Checked == true)
                {
                    Session["UjStudent"] = "No";
                    Session["ThirdParty"] = "Yes";
                }

                if (chkujStudent.Checked == true)
                {
                    Session["ThirdParty"] = "No";
                    Session["UjStudent"] = "Yes";
                }

                Session["Completed"] = "Yes";
                Session["AcceptedTerms"] = "Yes";

                // Close terms modal and show privacy modal
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "closeTerms", "$('#myModal').removeClass('fade').modal('hide');", true);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "showPrivacy", "$('#privacyModal').modal('show');", true);
            }
        }

        protected void btnPrivacyAccept_Click(object sender, EventArgs e)
        {
            Session["AcceptedPrivacy"] = "Yes";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "closePrivacy", "$('#privacyModal').removeClass('fade').modal('hide');", true);
            Response.Redirect("~/DocumentOrder.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnPrivacyDecline_Click(object sender, EventArgs e)
        {
            // Clear any previous selections
            Session["AcceptedTerms"] = null;
            Session["ThirdParty"] = null;
            Session["UjStudent"] = null;

            // Reset checkboxes
            chkAgree.Checked = false;
            chkujStudent.Checked = false;
            chkthirdparty.Checked = false;
            btnNext.Enabled = false;

            // Close privacy modal and show terms modal again
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "closePrivacy", "$('#privacyModal').removeClass('fade').modal('hide');", true);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "showTerms", "$('#myModal').modal('show');", true);
        }

        protected void chkAgree_CheckedChanged(object sender, EventArgs e)
        {

            lblErrorMessage.Text = "";

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session["AcceptedTerms"] = null;
            Session["ThirdParty"] = null;
            Session["UjStudent"] = null;
            chkAgree.Checked = false;
            chkujStudent.Checked = false;
            chkthirdparty.Checked = false;
            btnNext.Enabled = false;
            lblErrorMessage.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "successPopup1", "$('#myModal').removeClass('fade').modal('hide');", true);
        }

        protected void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            chkAgree.Checked = true;
        }

        protected void btnRequestClose_Click(object sender, EventArgs e)
        {

        }

        private void BindGrid()
        {
            if (ViewState["DocumentPrice"] != null)
            {
                DataTable dt = (DataTable)ViewState["DocumentPrice"];
                gvDocumentType_Price.DataSource = dt;
                gvDocumentType_Price.DataBind();
            }
        }










    }

}