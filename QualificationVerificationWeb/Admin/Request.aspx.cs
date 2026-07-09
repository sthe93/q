using Newtonsoft.Json;
using QualificationVerificationWeb.Admin.Content;
using QualificationVerificationWeb.Helper;
using QualificationVerificationWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QualificationVerificationWeb.Admin
{
    public partial class UnProcessed : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

            if (!Page.IsPostBack)
            {

                if (Session["CurrentLoggedInUser"] == null)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        // Recover from Forms Auth ticket
                        FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = identity.Ticket;

                        Session["CurrentLoggedInUser"] = ticket.Name;
                        Session["AcademicRecordLoggedInUser"] = ticket.Name;
                        Session["UserRoleData"] = ticket.UserData;
                    }
                    else
                    {
                        Session.Clear();
                        Session.Abandon(); 
                        FormsAuthentication.SignOut();
                        
                        Response.Redirect("~/Admin/Login.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                        return;
                    }
                }

                string roleLabel = Session["UserRoleData"] as string;
                // Defensive fallback (should not hit if BasePage logic works)
                if (string.IsNullOrEmpty(roleLabel))
                {
                    Response.Clear();
                    Response.Write("Authorization Error: Role not set.");
                    Response.End();
                    return;
                }


                ApplyRoleVisibility(roleLabel);

                // Everything below here runs once
                Page.DataBind();

                txtStartDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
                txtEndDate.Text = DateTime.Now.ToString("yyyy/MM/dd");

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

                await GetStudentOrders();

                Session["SearchStartDatet"] = null;

                Session["SelectedApplicant"] = null;

                Session["paymentMethodGet"] = null;

            }
        }
  
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
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
        private void ApplyRoleVisibility(string roleLabel)
        {
          
            bool isSuperAdmin = roleLabel.Equals("Super Admin", StringComparison.OrdinalIgnoreCase);
            tabApplicationAlert.Visible = isSuperAdmin;
            tabReason.Visible = isSuperAdmin;
            tabRecallOrder.Visible = isSuperAdmin;
        }

        public async Task GetStudentOrders()
        {
            
                string endpoint = $"Student/GetUnProcessed?studentId=0" +
                                  $"&studentIdentifer={txtStudentIdentifer.Text?.Trim()}" +
                                  $"&documentTypeID={(ddlDocumentType.SelectedValue == "0" ? null : ddlDocumentType.SelectedValue)}" +
                                  $"&startDate={txtStartDate.Text?.Trim()}" +
                                  $"&endDate={txtEndDate.Text?.Trim()}" +
                                  $"&status=0";

                // Use ApiClient
                var apiClient = new ApiClient();
                var response = await apiClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<AcademicRequestSearch>>(json);

                    gvAllOrders.DataSource = orders;
                    gvAllOrders.DataBind();
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
        protected async void gvAllOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAllOrders.PageIndex = e.NewPageIndex;
            await GetStudentOrders();
        }
        protected async void btnSearch_Click(object sender, EventArgs e)
        {
            if (!FeildsValidation())
                return;

            await GetStudentOrders();
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
            var studentStatus = "0";

            string url = $"Student/GetExportData?studentId={studentId}" +  
                         $"&studentIdentifer={studentIdentifer}" +
                         $"&documentTypeID={documentType}" +
                         $"&startDate={startDate}" +
                         $"&endDate={endDate}" +
                         $"&status={studentStatus}" ;

          
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
                        "attachment;filename=NotProcessed_" + exportFileName.Trim() + ".xls");
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
  
        protected void gvAllOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                var studentId = DataBinder.Eval(e.Row.DataItem, "StudentID").ToString();

                HyperLink hlView = (HyperLink)e.Row.FindControl("hlViewRequest");

                if(hlView != null)
                {
                    hlView.NavigateUrl = ResolveUrl("~/Admin/RequestView.aspx?studentId=" + studentId);

                }
            }
        }
        protected void view_Click(object sender, EventArgs e)
        {
            if (((LinkButton)sender).NamingContainer is GridViewRow clickedRow)
            {
                Session["SearchStartDatet"] = txtStartDate.Text;

                //var x = ((HiddenField)clickedRow.FindControl("hfStudentStatus")).Value;
                Session["studStatus"] = ((HiddenField)clickedRow.FindControl("hfStudentStatus")).Value;
                Session["SelectedApplicant"] = ((HiddenField)clickedRow.FindControl("hfStudentId")).Value;
                Session["paymentMethodGet"] = ((HiddenField)clickedRow.FindControl("hfPaymentMethod")).Value;
                PreviousUrl = "~/Admin/Request.aspx";
                Response.Redirect("~/Admin/RequestView.aspx",false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }
    }
}