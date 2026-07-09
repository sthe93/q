<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reason.aspx.cs" Inherits="QualificationVerificationWeb.Admin.Reason" MasterPageFile="~/Admin/AcademicDocumentMaster.Master" Async="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <%--  <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.1/jquery-ui.min.js"></script>--%>
    <!-- Toastr JS -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>
    <!-- SweetAlert2 CDN -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
  <%--  <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.1/themes/smoothness/jquery-ui.css"--%>> 

   <%-- <script type="text/javascript" src="js/bs.pagination.js"></script>--%>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('[data-toggle="tooltip"]').tooltip();
        }
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

                <li id="tabApplicationAlert" runat="server" tabindex="6" >
                    <a runat="server" id="ApplicationAlertController" href="ApplicationAlert.aspx">
                        <asp:Label runat="server" ID="lblApplicationAlert"> Application Alert </asp:Label></a>
                </li>
                <li id="tabReason" runat="server" tabindex="7" class="active">
                    <a runat="server" id="ReasonController" href="Reason.aspx">
                        <asp:Label runat="server" ID="lblReason"> Manage Reasons </asp:Label></a>
                </li>
            </ul>

            <asp:UpdatePanel ID="pnlReason" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:HiddenField ID="hfReasonId" runat="server" />
                    <div class="form-group">
                        <div class="panel-body">
                            <div class="form-group">
                                <div class="col-md-12">

                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblReasonName">Reason</asp:Label>
                                        <asp:TextBox ID="txtReason" runat="server" CssClass="form-control setHeight" MaxLength="150" placeholder="Enter reason"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblInstruction">Instruction</asp:Label>
                                        <asp:TextBox ID="txtInstruction" runat="server" CssClass="form-control" TextMode="MultiLine" 
                                                     Rows="5" MaxLength="500"
                                                     placeholder="Enter instruction ..."></asp:TextBox>
                                      
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblIsActive" AssociatedControlID="chkIsActive">Status</asp:Label>
                                        <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
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
                                
                                    <asp:GridView ID="gvReasons" runat="server" AllowPaging="true" PagerSettings-Mode="Numeric" PagerSettings-PageButtonCount="4" Class="table table-bordered table-striped mb-none" PageSize="15" AutoGenerateColumns="False" HeaderStyle-CssClass="control-label" OnPageIndexChanging="gvReasons_PageIndexChanging" OnRowCommand="gvReasons_RowCommand">
    <EmptyDataTemplate>
        <div>No reasons found.</div>
    </EmptyDataTemplate>
    <Columns>
        <asp:TemplateField HeaderText="Reason">
            <ItemTemplate>
                <asp:Label ID="lblReason" runat="server" Text='<%# Eval("Reason1") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Instruction">
            <ItemTemplate>
                <asp:Label ID="lblInstruction" runat="server" Text='<%# Eval("EmailInstruction") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

    
        <asp:TemplateField HeaderText="Created By">
            <ItemTemplate>
                <asp:Label ID="lblCreatedBy" runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

     
        <asp:TemplateField HeaderText="Date Created">
            <ItemTemplate>
                <asp:Label ID="lblCreatedOn" runat="server" Text='<%# Eval("CreatedOnDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Status">
            <ItemTemplate>
                <asp:Label ID="lblStatus" runat="server" 
                           Text='<%# Convert.ToBoolean(Eval("IsActive")) ? "Enabled" : "Disabled" %>' 
                           CssClass='<%# Convert.ToBoolean(Eval("IsActive")) ? "text-success font-weight-bold" : "text-danger font-weight-bold" %>'>
                </asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditReason" CommandArgument='<%# Eval("ReasonId") %>' CssClass="btn btn-sm btn-info" Text="Edit" />
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