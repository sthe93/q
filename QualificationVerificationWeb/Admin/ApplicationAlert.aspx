<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplicationAlert.aspx.cs" Inherits="QualificationVerificationWeb.Admin.ApplicationAlert" MasterPageFile="~/Admin/AcademicDocumentMaster.Master" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.1/jquery-ui.min.js"></script>--%>
    <!-- Toastr JS -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>
    <!-- SweetAlert2 CDN -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <%--<link rel="stylesheet" href="https://code.jquery.com/ui/1.13.1/themes/smoothness/jquery-ui.css">--%> 
  <!-- jQuery Timepicker CSS -->
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/timepicker/1.3.5/jquery.timepicker.min.css">

<!-- jQuery Timepicker JS -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/timepicker/1.3.5/jquery.timepicker.min.js"></script>

   <%-- <script type="text/javascript" src="js/bs.pagination.js"></script>--%>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('[data-toggle="tooltip"]').tooltip();
        }

        $(function () {


            $(".timepicker").timepicker({
                showMeridian: false,
                minuteStep: 1,
                timeFormat: 'HH:mm' // Ensure two-digit hour and minute format
            });
        });
    </script>

    <style type="text/css">
        .setHeight {
            height: 41px !important;
        }

        .GridPager a, .GridPager span {
            display: block;
            height: 20px;
            width: 25px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }

        .GridPager a {
            border: 1px solid #969696;
            border-radius: 4px;
        }

        .GridPager span {
            border: 1px solid #3AC0F2;
            border-radius: 4px;
        }
    </style>

    <div class="row">
        <div id="tabParent" class="tabs tabs-primary">
            <ul class="nav nav-tabs">
                <li id="tabUnProcessed" runat="server" visible="true" tabindex="0">
                    <a runat="server" id="UnProcessedControl" href="Request.aspx">
                        <asp:Label runat="server" ID="lblProfDashbord"> Not Processed Requests </asp:Label></a>
                </li>
                <li id="tabRecallOrder" runat="server" tabindex="8">
                    <a runat="server" id="RecallOrder" href="RecallOrder.aspx">
                        <asp:Label runat="server" ID="lblRecallOrder"> Recall Order </asp:Label></a>
                </li>
                <li id="tabFinancialBlock" runat="server" tabindex="3">
                    <a runat="server" id="FinancialBlock" href="FinancialBlock.aspx">
                        <asp:Label runat="server" ID="lblFinancialBlock"> Financial block </asp:Label></a>
                </li>
                <li id="tabApprove" runat="server" class="tabApprove" tabindex="1">
                    <a runat="server" id="Approve" href="Approved.aspx">
                        <asp:Label runat="server" ID="lblApprove"> Approved Request </asp:Label></a>
                </li>
                <li id="tabDeclined" runat="server" tabindex="2">
                    <a runat="server" id="Declined" href="Declined.aspx">
                        <asp:Label runat="server" ID="lblDeclined"> Declined Request </asp:Label></a>
                </li>
                <li id="tabCompleted" runat="server" tabindex="4">
                    <a runat="server" id="Completed" href="Completed.aspx">
                        <asp:Label runat="server" ID="lblCompleted"> Completed Request </asp:Label></a>
                </li>
                <li id="tabReport" runat="server" tabindex="5">
                    <a runat="server" id="Report" href="DisplayReports.aspx">
                        <asp:Label runat="server" ID="lblREport"> Report </asp:Label></a>
                </li>
                <li id="tabApplicationAlert" runat="server" tabindex="6" class="active">
                    <a runat="server" id="ApplicationAlertController" href="ApplicationAlert.aspx">
                        <asp:Label runat="server" ID="lblApplicationAlert"> Application Alert </asp:Label></a>
                </li>
                <li id="tabReason" runat="server" tabindex="7" >
                    <a runat="server" id="ReasonController" href="Reason.aspx">
                        <asp:Label runat="server" ID="lblReason"> Manage Reasons </asp:Label></a>
                </li>
            </ul>

            <asp:UpdatePanel ID="pnlApplicationAlert" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:HiddenField ID="hfAlertId" runat="server" />
                    <div class="form-group">
                        <div class="panel-body">
                            <div class="form-group">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblStartDate">Start Date</asp:Label>
                                        <asp:TextBox ID="txtStartDate" runat="server" autocomplete="off" class="data-plugin-datepicker form-control setHeight" data-date-format="yyyy/mm/dd" data-plugin-datepicker="" placeholder="Select Start Date"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblStartTime">Start Time</asp:Label>
                                        <asp:TextBox ID="txtStartTime" runat="server" CssClass="timepicker form-control setHeight" placeholder="Select Start Time"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblEndDate">End Date</asp:Label>
                                        <asp:TextBox ID="txtEndDate" runat="server" autocomplete="off" class="data-plugin-datepicker form-control setHeight" data-date-format="yyyy/mm/dd" data-plugin-datepicker="" placeholder="Select End Date"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblEndTime">End Time</asp:Label>
                                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="timepicker form-control setHeight" placeholder="Select End Time"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblMessage">Message</asp:Label>
                                        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control" MaxLength="1000" placeholder="Enter your message here"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" />
                                        <asp:Label runat="server" AssociatedControlID="chkIsActive">Is Active</asp:Label>
                                    </div>

                                    <div class="form-group">
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Width="109px" Text="Create New" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-warning" Text="Update" OnClick="btnUpdate_Click" Visible="false" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="panel-body">
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:GridView ID="gvAlerts" runat="server" AllowPaging="true" PagerSettings-Mode="Numeric" PagerSettings-PageButtonCount="4" Class="table table-bordered table-striped mb-none" PageSize="15" AutoGenerateColumns="False" HeaderStyle-CssClass="control-label" OnPageIndexChanging="gvAlerts_PageIndexChanging" OnRowCommand="gvAlerts_RowCommand">
                                        <EmptyDataTemplate>
                                            <div>No alerts found.</div>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Start Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("StartDate", "{0:yyyy/MM/dd}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Start Time">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStartTime" runat="server" Text='<%# Eval("StartTime", "{0:hh\\:mm}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="End Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("EndDate", "{0:yyyy/MM/dd}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="End Time">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEndTime" runat="server" Text='<%# Eval("EndTime", "{0:hh\\:mm}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Message">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMessage" runat="server" Text='<%# Eval("Message") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" 
                                                               Text='<%# Convert.ToBoolean(Eval("IsActive")) ? "Activated" : "Deactivated" %>' 
                                                               CssClass='<%# Convert.ToBoolean(Eval("IsActive")) ? "text-success font-weight-bold" : "text-danger font-weight-bold" %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Actions">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("AlertId") %>' CommandName="UpdateAlert" Text="Edit" />
                                                 <%--   <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("AlertId") %>' CommandName="DeleteAlert" Text="Delete" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="GridPager" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>