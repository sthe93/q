using QualificationVerificationWeb.Helper;
using QualificationVerificationWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Helpers;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QualificationVerificationWeb
{
    public partial class Calculate : System.Web.UI.Page
    {

        private readonly string qualificationVerificationAPI = ConfigurationManager.AppSettings["QualificationVerificationAPI"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");



            if (!IsPostBack)
            {
               
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[8] {
                    new DataColumn("DeliveryMethod"),
                    new DataColumn("DeliveryInd"),
                    new DataColumn("Academic Record"),
                    new DataColumn("Transcript Supplement"),
                    new DataColumn("Confirmation Letter"),
                    new DataColumn("Send to multiple address"),
                    new DataColumn("Number of Address"),
                    new DataColumn("Forms for Official Bodies") // Added the missing column
                });

                ViewState["Addresses"] = dt;
                BindGrid();
                GetDeliveryType();
            }


        }
        protected void BindGrid()
        {
            gvAddress.DataSource = (DataTable)ViewState["Addresses"];
            gvAddress.DataBind();

        }


        public void GetDeliveryType()
        {

            string url = qualificationVerificationAPI + "Student/GetDeliveryType";
            var deliveryList = BusinessRules.HttpHelper.HttpCallJson<List<AcademicDocumentData>>(url, WebRequestMethods.Http.Get, null).ToList();


            //List<AcademicDocumentData> deliveryList = studentRepository.GetDeliveryType().ToList();

            // Remove "Collect" and "Electronic Copy" from deliveryList
            deliveryList.RemoveAll(item => item.DeliveryType == "Collect");


            DataTable dt = (DataTable)ViewState["Addresses"];

            if (deliveryList != null)
            {
                foreach (var item in deliveryList)
                {
                    dt.Rows.Add(item.DeliveryType.ToString(), item.DeliveryInd.ToString(), "0", "0", "0", "0", "0", "0");
                    ViewState["Addresses"] = dt;

                }
                BindGrid();
            }


        }

        public bool FeildsValidation()
        {



            foreach (GridViewRow row in gvAddress.Rows)
            {


                if ((row.FindControl("ddlSendToMultipleAddress") as DropDownList).SelectedValue == "1")
                {



                    string c_num = "0", sa_num = "0", int_num = "0", forms_num = "0";
                    int totalsum = 0;

                    string nump = (row.FindControl("ddlNumberofAddress") as DropDownList).SelectedValue;
                    c_num = (row.FindControl("ddlNumberofAcademicRecord") as DropDownList).SelectedValue;
                    sa_num = (row.FindControl("ddlNumberofTranscriptSupplement") as DropDownList).SelectedValue;
                    int_num = (row.FindControl("ddlNumberofConfirmationLetter") as DropDownList).SelectedValue;
                    forms_num = (row.FindControl("ddlNumberofFormsForOfficialBodies") as DropDownList).SelectedValue; // New field

                    totalsum = int.Parse(c_num) + int.Parse(sa_num) + int.Parse(int_num) + int.Parse(forms_num);


                    if (totalsum > 1 && (int.Parse(nump.ToString()) == 0))
                    {
                        lblErrorMessage.Text = "* Number of address must be greater than 0 where send to multiple address is 'Yes'.";
                        (row.FindControl("ddlNumberofAddress") as DropDownList).Enabled = true;
                        return false;
                    }

                }

            }



            foreach (GridViewRow row in gvAddress.Rows)
            {
                if ((row.FindControl("ddlSendToMultipleAddress") as DropDownList).SelectedValue == "1")
                {
                    string c_num = "0", sa_num = "0", int_num = "0", forms_num = "0";
                    int totalsum = 0;

                    string num1 = (row.FindControl("ddlNumberofAddress") as DropDownList).SelectedValue;
                    c_num = (row.FindControl("ddlNumberofAcademicRecord") as DropDownList).SelectedValue;
                    sa_num = (row.FindControl("ddlNumberofTranscriptSupplement") as DropDownList).SelectedValue;
                    int_num = (row.FindControl("ddlNumberofConfirmationLetter") as DropDownList).SelectedValue;
                    forms_num = (row.FindControl("ddlNumberofFormsForOfficialBodies") as DropDownList).SelectedValue; // New field

                    totalsum = int.Parse(c_num) + int.Parse(sa_num) + int.Parse(int_num) + int.Parse(forms_num);

                    if (int.Parse(num1.ToString()) > totalsum)
                    {
                        lblErrorMessage.Text = "* Number of address cannot be more than number of copies per row.";
                        (row.FindControl("ddlNumberofAddress") as DropDownList).Enabled = true;
                        return false;
                    }

                    if (totalsum == 1 && int.Parse(num1.ToString()) > 1)
                    {
                        lblErrorMessage.Text = "* Please select more number of copies cannot send one copy to multiple address per row.";
                        (row.FindControl("ddlNumberofAddress") as DropDownList).Enabled = true;
                        return false;
                    }
                }

            }


            return true;
        }



        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
           
            if (!FeildsValidation())
                return;

            try
            {
                decimal totalAmount = 0;

                foreach (GridViewRow row in gvAddress.Rows)
                {
                    string numbercopi = GetDropDownValue(row, "ddlNumberofAddress", "1");
                    decimal totalac = CalculateTotalForDocument(row, "ddlNumberofAcademicRecord", "Academic Record (Modules plus Results)", ref totalAmount, ref numbercopi);
                    decimal totalts = CalculateTotalForDocument(row, "ddlNumberofTranscriptSupplement", "Academic Transcript Supplement (Course content including Academic Record)", ref totalAmount, ref numbercopi);
                    decimal totalcl = CalculateTotalForDocument(row, "ddlNumberofConfirmationLetter", "Confirmation Letter (General letters)", ref totalAmount, ref numbercopi);
                    decimal totalfb = CalculateTotalForDocument(row, "ddlNumberofFormsForOfficialBodies", "Forms for Official Bodies", ref totalAmount, ref numbercopi);

                    totalAmount += (totalac + totalts + totalcl + totalfb);

                    EnableDropDown(row, "ddlNumberofAddress");
                }

                DisplayTotalAmount(totalAmount);
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog(ex);
                Response.Redirect("~/Error.aspx");
            }
        }

        private string GetDropDownValue(GridViewRow row, string dropDownId, string defaultValue)
        {
            var dropDown = row.FindControl(dropDownId) as DropDownList;
            if (dropDown == null || dropDown.SelectedItem == null)
            {
                // Handle null or missing DropDownList (optional log)
                return defaultValue;
            }
            return dropDown.SelectedItem.Value;
        }


        private decimal CalculateTotalForDocument(GridViewRow row, string dropDownId, string degree, ref decimal totalAmount, ref string numbercopi)
        {
            decimal total = 0;
            string number = GetDropDownValue(row, dropDownId, "0");

            if (number != "0")
            {
                string deliveryInd = GetHiddenFieldValue(row, "DeliveryInd");
                decimal FeeAmount = GetFeeAmount(degree, deliveryInd);
                decimal CollectFee = GetCollectFee(degree);

                total = CalculateDocumentTotal(number, FeeAmount, CollectFee, deliveryInd);
            }

            return total;
        }

        private string GetHiddenFieldValue(GridViewRow row, string hiddenFieldId)
        {
            var hiddenField = row.FindControl(hiddenFieldId) as HiddenField;
            return hiddenField != null ? hiddenField.Value : string.Empty;
        }

        
        private decimal GetFeeAmount(string degree, string deliveryInd)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(degree)) throw new ArgumentException("Invalid document type");
            if (string.IsNullOrWhiteSpace(deliveryInd) || deliveryInd.Length != 1)
                throw new ArgumentException("Invalid delivery indicator");

            string url = qualificationVerificationAPI + "Student/GetFeesAmount?deliveryInd=" +
                         WebUtility.UrlEncode(deliveryInd) + "&documentType=" +
                         WebUtility.UrlEncode(degree);

            var fees = BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url, WebRequestMethods.Http.Get, null).D_DocumentFee;
            return (decimal)fees;
        }

        private decimal GetCollectFee(string degree)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(degree)) throw new ArgumentException("Invalid document type");

            string url = qualificationVerificationAPI + "Student/GetCollectFees?documentType=" +
                         WebUtility.UrlEncode(degree);
            var collect = BusinessRules.HttpHelper.HttpCallJson<AcademicDocumentData>(url, WebRequestMethods.Http.Get, null).D_DocumentFee;
            return (decimal)collect;
        }
        private decimal CalculateDocumentTotal(string number, decimal feeAmount, decimal collectFee, string deliveryInd)
        {
            decimal total = 0;
            if (deliveryInd != "C")
            {
                total = collectFee * int.Parse(number);
            }
            else
            {
                total = feeAmount * int.Parse(number);
            }
            return total;
        }

        private void EnableDropDown(GridViewRow row, string dropDownId)
        {
            var dropDown = row.FindControl(dropDownId) as DropDownList;
            if (dropDown != null)
            {
                dropDown.Enabled = true;
            }
        }

        private void DisplayTotalAmount(decimal totalAmount)
        {
            if (totalAmount != 0)
            {
                lblTotalAmount.Text = "R " + totalAmount.ToString("N2"); // format as currency
                Div10.Visible = true;
            }
        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            Div10.Visible = false;
            DataTable dt = (DataTable)ViewState["Addresses"];
            dt.Clear();

            gvAddress.DataSource = dt;
            gvAddress.DataBind();


            GetDeliveryType();

        }



        protected void btnClose_Click(object sender, EventArgs e)
        {
            //btnClose.Attributes.Add("OnClick", "window.close();");
            Response.Write("<script>window.close();</script>");
        }

        protected void ddlNumberofAcademicRecord_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";

            DropDownList selectedDropDown = (DropDownList)sender;

            GridViewRow gvr = (GridViewRow)((DropDownList)sender).Parent.Parent;

            DropDownList ddlAcademicRecord = (DropDownList)gvr.FindControl("ddlNumberofAcademicRecord");
            ddlAcademicRecord.SelectedValue = selectedDropDown.SelectedValue;
            string DeliveryInd = ((HiddenField)gvr.FindControl("DeliveryInd")).Value;

            DataTable dt = (DataTable)ViewState["Addresses"];

            foreach (DataRow row in dt.Rows)
            {
                if (row.ItemArray[1].ToString() == DeliveryInd)
                {
                    row.SetField("Academic Record", ddlAcademicRecord.SelectedValue);
                }
            }

            gvAddress.DataSource = dt;
            gvAddress.DataBind();
        }

        protected void ddlNumberofTranscriptSupplement_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";

            DropDownList selectedDropDown = (DropDownList)sender;

            GridViewRow gvr = (GridViewRow)((DropDownList)sender).Parent.Parent;
            DropDownList ddlTranscriptSupplement = (DropDownList)gvr.FindControl("ddlNumberofTranscriptSupplement");
            ddlTranscriptSupplement.SelectedValue = selectedDropDown.SelectedValue;
            string DeliveryInd = ((HiddenField)gvr.FindControl("DeliveryInd")).Value;


            DataTable dt = (DataTable)ViewState["Addresses"];

            foreach (DataRow row in dt.Rows)
            {
                if (row.ItemArray[1].ToString() == DeliveryInd)
                {
                    row.SetField("Transcript Supplement", ddlTranscriptSupplement.SelectedValue);
                }
            }

            gvAddress.DataSource = dt;
            gvAddress.DataBind();
        }

        protected void ddlNumberofConfirmationLetter_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";

            DropDownList selectedDropDown = (DropDownList)sender;

            GridViewRow gvr = (GridViewRow)((DropDownList)sender).Parent.Parent;
            DropDownList ddlConfirmationLetter = (DropDownList)gvr.FindControl("ddlNumberofConfirmationLetter");
            ddlConfirmationLetter.SelectedValue = selectedDropDown.SelectedValue;
            string DeliveryInd = ((HiddenField)gvr.FindControl("DeliveryInd")).Value;


            DataTable dt = (DataTable)ViewState["Addresses"];

            foreach (DataRow row in dt.Rows)
            {
                if (row.ItemArray[1].ToString() == DeliveryInd)
                {
                    row.SetField("Confirmation Letter", ddlConfirmationLetter.SelectedValue);
                }
            }

            gvAddress.DataSource = dt;
            gvAddress.DataBind();

        }
        protected void ddlNumberofFormsForOfficialBodies_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";

            DropDownList selectedDropDown = (DropDownList)sender;

            GridViewRow gvr = (GridViewRow)((DropDownList)sender).Parent.Parent;
            DropDownList ddlNumberofFormsForOfficialBodies = (DropDownList)gvr.FindControl("ddlNumberofFormsForOfficialBodies");
            ddlNumberofFormsForOfficialBodies.SelectedValue = selectedDropDown.SelectedValue;
            string DeliveryInd = ((HiddenField)gvr.FindControl("DeliveryInd")).Value;

            DataTable dt = (DataTable)ViewState["Addresses"];

            foreach (DataRow row in dt.Rows)
            {
                if (row.ItemArray[1].ToString() == DeliveryInd)
                {
                    row.SetField("Forms for Official Bodies", ddlNumberofFormsForOfficialBodies.SelectedValue);
                }
            }

            gvAddress.DataSource = dt;
            gvAddress.DataBind();
        }


        protected void ddlNumberofAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";

            DropDownList selectedDropDown = (DropDownList)sender;

            GridViewRow gvr = (GridViewRow)((DropDownList)sender).Parent.Parent;
            DropDownList ddlAddress = (DropDownList)gvr.FindControl("ddlNumberofAddress");
            ddlAddress.SelectedValue = selectedDropDown.SelectedValue;
            string DeliveryInd = ((HiddenField)gvr.FindControl("DeliveryInd")).Value;

            foreach (GridViewRow row in gvAddress.Rows)
            {
                if (((HiddenField)row.FindControl("DeliveryInd")).Value == DeliveryInd)
                {

                    //(row.FindControl("chkSendToMultipleAddress") as CheckBox).Checked = false;
                    //Response.Write("false");

                    string ac_num = "0";
                    string ts_num = "0";
                    string cl_num = "0";
                    string fb_num = "0";
                    int Total = 0;

                    ac_num = ((DropDownList)gvr.FindControl("ddlNumberofAcademicRecord")).SelectedItem.Value;
                    ts_num = ((DropDownList)gvr.FindControl("ddlNumberofTranscriptSupplement")).SelectedItem.Value;
                    cl_num = ((DropDownList)gvr.FindControl("ddlNumberofConfirmationLetter")).SelectedItem.Value;
                    fb_num = ((DropDownList)gvr.FindControl("ddlNumberofFormsForOfficialBodies")).SelectedItem.Value;

                    Total = int.Parse(ac_num.ToString()) + int.Parse(ts_num.ToString()) + int.Parse(cl_num.ToString()) + int.Parse(fb_num.ToString());

                    if (Total == 0)
                    {
                        lblErrorMessage.Text = "* Please select a minimum of one document type per row on your request(s).";
                        (row.FindControl("ddlNumberofAddress") as DropDownList).Enabled = true;
                    }
                }
            }


            DataTable dt = (DataTable)ViewState["Addresses"];

            foreach (DataRow row in dt.Rows)
            {
                if (row.ItemArray[1].ToString() == DeliveryInd)
                {
                    row.SetField("Number of Address", ddlAddress.SelectedValue);

                    if (ddlAddress.SelectedValue.ToString() != "0")
                    {
                        row.SetField("Send to multiple address", 1);
                    }
                    else
                    {
                        row.SetField("Send to multiple address", 0);
                    }

                }
            }


            gvAddress.DataSource = dt;
            gvAddress.DataBind();
        }

    }

}