using Newtonsoft.Json;
using QualificationVerificationWeb.Admin.Content;
using QualificationVerificationWeb.Helper;
using QualificationVerificationWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QualificationVerificationWeb.Admin
{
    public partial class Test : BasePage
    {

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DateTime now = DateTime.Now;

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
                ////LoadAcademicDocument();


                var staDate = new DateTime(now.Year, now.Month, 1).ToString("yyyy/MM/dd");
                var staDateq = new DateTime(now.Year, now.Month, 1);
                var enDate = staDateq.AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd");
               
                txtStartDate.Text = staDate.ToString();
                txtEndDate.Text = enDate.ToString();

                var startDate = !String.IsNullOrEmpty(txtStartDate.Text) ? txtStartDate.Text.Trim() : staDate.ToString();
                var endDate = !String.IsNullOrEmpty(txtEndDate.Text) ? txtEndDate.Text.Trim() : enDate.ToString();
                ReportStartDate = startDate;
                ReportEndDate = endDate;

                await GetAllStudentOrders(startDate, endDate);
                //// this.BindGrid();
                

            }
            
            Session["SelectedApplicant"] = null;

        }
        private void ApplyRoleVisibility()
        {
            string roleLabel = Session["UserRoleData"] as string ?? "";

            // Debug output (optional)
            // Response.Write("<div style='color:red'>ROLE: " + roleLabel + "</div>");

            bool isSuperAdmin = roleLabel.Equals("Super Admin", StringComparison.OrdinalIgnoreCase);

            tabApplicationAlert.Visible = isSuperAdmin;
            tabReason.Visible = isSuperAdmin;
            tabRecallOrder.Visible = isSuperAdmin;
        }
        public async Task GetAllStudentOrders(string sDate, string eDate)
        {

            // Use ApiClient
            var apiClient = new ApiClient();

            string documentType = null;
            string studentStatus = "4";
            var acRecCount = 0;
            var acTransCount = 0;
            var acRecordAndTransCount = 0;
            var formsCount = 0;



            if (ddlDocumentType.SelectedItem.Value != "0")
            {
                documentType = ddlDocumentType.SelectedItem.Value;
            }


            if (ddlApplicationStatus.SelectedItem.Value != "-1")
            {
                studentStatus = ddlApplicationStatus.SelectedItem.Value;
            }


            var studentIdentifer = !String.IsNullOrEmpty(txtStudentIdentifer.Text) ? txtStudentIdentifer.Text.Trim() : null;

            SDocumentType = documentType;
            SStudentIdentifer = studentIdentifer;
            SStudentStatus = studentStatus;

            var url =  "Student/GetUnProcessed?studentId=" + 0 + 
                       "&studentIdentifer=" + studentIdentifer + 
                       "&documentTypeID=" + documentType + 
                       "&startDate=" + sDate + 
                       "&endDate=" + eDate + 
                       "&status=" + studentStatus;

            var response = await apiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();


                var records = JsonConvert.DeserializeObject<List<AcademicRequestSearch>>(json);


                acRecCount = (from i in records
                    where i.DocumentType.Contains("Record")
                    select i).Count();

                acTransCount = (from i in records
                    where i.DocumentType.Contains("Transcript")
                    select i).Count();

                acRecordAndTransCount = (from i in records
                    where i.DocumentType.Contains("Letter")
                    select i).Count();

                formsCount = (from i in records
                    where i.DocumentType.Contains("Forms")
                    select i).Count();

                gvAllOrders.DataSource = records;
                gvAllOrders.DataBind();



                lblArcRecordCount.Text = "Academic Record: " + acRecCount;
                lbltrasSuppCount.Text = "Academic Transcript Supplement: " + acTransCount;
                lbltrasacRecordAndTransCount.Text = "Confirmation Letter: " + acRecordAndTransCount;
                lblFormsCount.Text = "Forms for Official Bodies: " + formsCount;

                if (ddlDocumentType.SelectedItem.Value == "0")
                {
                    lblArcRecordCount.Visible = true;
                    lbltrasSuppCount.Visible = true;
                    lbltrasacRecordAndTransCount.Visible = true;
                    lblFormsCount.Visible = true;

                }
                else if (ddlDocumentType.SelectedItem.Value == "S")
                {
                    lblArcRecordCount.Visible = false;
                    lbltrasSuppCount.Visible = false;
                    lbltrasacRecordAndTransCount.Visible = true;
                    lblFormsCount.Visible = false;
                }
                else if (ddlDocumentType.SelectedItem.Value == "R")
                {
                    lblArcRecordCount.Visible = true;
                    lbltrasSuppCount.Visible = false;
                    lbltrasacRecordAndTransCount.Visible = false;
                    lblFormsCount.Visible = false;
                }
                else if (ddlDocumentType.SelectedItem.Value == "T")
                {
                    lblArcRecordCount.Visible = false;
                    lbltrasSuppCount.Visible = true;
                    lbltrasacRecordAndTransCount.Visible = false;
                    lblFormsCount.Visible = false;
                }
                else if (ddlDocumentType.SelectedItem.Value == "F")
                {
                    lblArcRecordCount.Visible = false;
                    lbltrasSuppCount.Visible = false;
                    lbltrasacRecordAndTransCount.Visible = false;
                    lblFormsCount.Visible = true;
                }
            }

        }
        protected async void gvAllOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAllOrders.PageIndex = e.NewPageIndex;
            await GetAllStudentOrders(ReportStartDate, ReportEndDate);
        }

        protected void gvAllOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "viewARequest":
                    Session["SelectedApplicant"] = e.CommandArgument.ToString();
                    Response.Redirect("~/Admin/RequestView.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    break;
            }
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

            ReportStartDate = null;
            ReportEndDate = null;

            var startDate = !String.IsNullOrEmpty(txtStartDate.Text) ? txtStartDate.Text.Trim() : null;
            var endDate = !String.IsNullOrEmpty(txtEndDate.Text) ? txtEndDate.Text.Trim() : null;
            ReportStartDate = startDate;
            ReportEndDate = endDate;


            if (!FeildsValidation())
                return;

            await GetAllStudentOrders(startDate, endDate);
        }
        protected async void BtnExport_Click(object sender, EventArgs e)
        {
            await ExportToExcel();
        }
        protected async Task ExportToExcel()
        {
            //To Export all pages
            var exportFileName = DateTime.Now.ToString("dd_MMMM_yyyy");
            gvAllOrders.AllowPaging = false;
            await BindGrid();
            if (gvAllOrders.Rows.Count > 0)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Report_" + exportFileName.Trim() + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);


                    gvAllOrders.HeaderRow.BackColor = Color.White;
                    foreach (TableCell cell in gvAllOrders.HeaderRow.Cells)
                    {
                        cell.BackColor = gvAllOrders.HeaderStyle.BackColor;
                    }
                    foreach (GridViewRow row in gvAllOrders.Rows)
                    {
                        row.BackColor = Color.White;
                        foreach (TableCell cell in row.Cells)
                        {
                            if (row.RowIndex % 2 == 0)
                            {
                                cell.BackColor = gvAllOrders.AlternatingRowStyle.BackColor;
                            }
                            else
                            {
                                cell.BackColor = gvAllOrders.RowStyle.BackColor;
                            }
                            cell.CssClass = "textmode";
                        }
                    }

                    gvAllOrders.RenderControl(hw);

                    //style to format numbers to string
                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();

                }

            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        private async Task BindGrid()
        {
            // Use ApiClient
            var apiClient = new ApiClient();

            string documentType = null;


            documentType = SDocumentType;

        
            var studentIdentifer = SStudentIdentifer;
            var startDate = ReportStartDate;
            var endDate = ReportEndDate;
            var studentStatus = SStudentStatus;

            var url = $"Student/GetReport?studentIdentifer={studentIdentifer}" +
                      $"&documentTypeID={documentType}" +
                      $"&startDate={startDate}" +
                      $"&endDate={endDate}" +
                      $"&status={studentStatus}";

            var response = await apiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                gvAllOrders.DataSource = JsonConvert.DeserializeObject<List<AcademicRequestReport>>(json);
                gvAllOrders.DataBind();
            }


        }
        protected async void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAllOrders.PageIndex = e.NewPageIndex;
            await GetAllStudentOrders(ReportStartDate, ReportEndDate);
        }
    }
}