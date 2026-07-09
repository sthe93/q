<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Approved.aspx.cs" Inherits="QualificationVerificationWeb.Admin.Approved" MasterPageFile="~/Admin/AcademicDocumentMaster.Master" ValidateRequest="false" Async="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  
    <script type="text/javascript">
        tinymce.init({ selector: 'textarea', Width: 500, Height: 250, menubar: false, statusbar: false, toolbar: false });
    </script>
    <%--<script src="../Scripts/general.js"></script>--%>
    <style>
        .btnFeesGrant {
            background-color: #E7540E !Important;
            color: #fff !Important;
        }

        .btnCancel {
            margin-left: 30px;
        }

        td, th {
            padding: 4px 5px 4px 4px !important;
        }

        #divAddSecond {
            margin-bottom: 0px;
        }
          .sla-indicator {
            border-radius: 12px;
            color: #fff;
            display: inline-block;
            font-weight: bold;
            line-height: 1.2;
            min-width: 94px;
            padding: 4px 8px;
            text-align: center;
            white-space: nowrap;
        }

        .sla-indicator-green {
            background-color: #3a7d44;
        }

        .sla-indicator-orange {
            background-color: #f0ad4e;
        }

        .sla-indicator-red {
            background-color: #d9534f;
        }

        .sla-indicator-neutral {
            background-color: #777;
        }

        .sla-expiry-date {
            display: block;
            font-size: 11px;
            font-weight: normal;
            margin-top: 2px;
        }
    </style>
    <script type="text/javascript">  
        function pageLoad(sender, args) {
            $('[data-toggle="tooltip"]').tooltip();
        }

    </script>
    <script type="text/javascript">  
        function ShowPopUp() {
            $("#popUpSendEmailModal").modal('show');
        }

    </script>
    <script type="text/javascript">

        function CancelAndClose() {
            $("#popUpSendEmailModal").removeClass('fade').modal('hide');
            return false;
        };
    </script>

    <script type="text/javascript">  
        function myModalShowPopUp() {
            $("#myModal").modal('show');
        }

    </script>
    <script type="text/javascript">

        function myModalCancelAndClose() {
            $("#myModal").removeClass('fade').modal('hide');
            return false;
        };
    </script>

    <script type="text/ecmascript">
        function checkButtonClicked(buttonSelected) {
            document.getElementById("ContentPlaceHolder1_ButtonClicked").value = buttonSelected;
        }
    </script>
    <script type="text/ecmascript">
        function checkButtonClicked(buttonSelected) {
            document.getElementById("ContentPlaceHolder1_ButtonClicked").value = buttonSelected;
        }
    </script>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('[data-toggle="tooltip"]').tooltip();

            var date = new Date();
            date.setDate(date.getDate());
            $('.data-plugin-datepicker ').datepicker(
                {
                    startDate: date,
                    defaultDate: date,
                });
        }

        $(function () {
            var errorMsg = '@TempData["Error"]'
            if (errorMsg != '')
                toastr.error(errorMsg, 'Error Message');
        });

        $(function () {
            var successMsg = '@TempData["Success"]'
            if (successMsg != '')
                toastr.success(successMsg, 'Success Message');
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
                    <a runat="server" id="UnProcessed" href="Request.aspx">
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
                <li id="tabApprove" runat="server" class="active" tabindex="1">
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
                <li id="tabReason" runat="server" tabindex="7" >
                    <a runat="server" id="ReasonController" href="Reason.aspx">
                        <asp:Label runat="server" ID="lblReason"> Manage Reasons </asp:Label></a>
                </li>
            </ul>



            <asp:UpdatePanel ID="pnlManageUSers" runat="server" UpdateMode="Always">
                <ContentTemplate>

                    <div class="form-group">
                        <div class="panel-body">
                            <div class="form-group">
                                <div class="col-md-0">
                                    <div class="form-group" style="margin-left: 2px">
                                        <div class="form-inline">
                                            <div id="divErrMsg" class="form-group" runat="server">
                                                <asp:Label ID="lblErrorMessage" runat="server" Style="text-align: left" ForeColor="Red" Width="547px"></asp:Label>
                                            </div>

                                            <br></br>
                                            <asp:DropDownList ID="ddlDocumentType" runat="server" AutoPostBack="false" Width="200px">
                                                <asp:ListItem Value="0">Select Document Type</asp:ListItem>
                                                <asp:ListItem Text="Academic Record" Value="R"></asp:ListItem>
                                                <asp:ListItem Text="Academic Transcript Supplement" Value="T"></asp:ListItem>
                                                <asp:ListItem Text="Confirmation Letter" Value="S"></asp:ListItem>
                                                <asp:ListItem Text="Forms for Official Bodies" Value="F"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtStudentIdentifer" runat="server" autocomplete="off" class="form-control setHeight" MaxLength="20" placeholder="Enter Student Number or ID/Passport Number or Reference Number" ReadOnly="false" type="text" Width="490px"></asp:TextBox>
                                            <div class="input-group col-md-2">
                                                <asp:TextBox ID="txtStartDate" runat="server" autocomplete="off" class="data-plugin-datepicker form-control setHeight" data-date-format="yyyy/mm/dd" data-plugin-datepicker="" placeholder="Select Start Date" Width="135px"></asp:TextBox>
                                                <span class="input-group-addon">To </span>
                                                <asp:TextBox ID="txtEndDate" runat="server" autocomplete="off" class="data-plugin-datepicker form-control setHeight" data-date-format="yyyy/mm/dd" data-plugin-datepicker="" placeholder="Select End Date" Width="135px"></asp:TextBox>
                                                <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            </div>
                                            <asp:Button ID="btnSearch" runat="server" class="btn btn-primary setHeight" OnClick="btnSearch_Click" Text="Search" />
                                            <div class="form-group" style="margin-left: 5px">
                                                <asp:Button ID="btnExport" runat="server" class="btn btn-primary setHeight" OnClick="btnExport_Click" Text="Export" Width="110px" />
                                            </div>

                                            <br></br>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="col-md-12">
                                        <div style="overflow: auto; overflow-style: panner">
                                            <asp:GridView ID="gvAllOrders" runat="server" AllowPaging="true" PagerSettings-Mode="Numeric" PagerSettings-PageButtonCount="4" Class="table table-bordered table-striped mb-none" PageSize="15" AutoGenerateColumns="False" HeaderStyle-CssClass="control-label" OnPageIndexChanging="gvAllOrders_PageIndexChanging">
                                                <EmptyDataTemplate>
                                                    <div>
                                                        No data found.
                                                    </div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Date Created">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDateCreated" runat="server" Text='<%# Eval("CreatedOnDate")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDocumentType" runat="server" Text='<%# Eval("DocumentType")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="SLA Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSlaStatus" runat="server"
                                                                       CssClass='<%# GetSlaCssClass(Eval("CreatedOnDate"), Eval("DocumentType")) %>'
                                                                       Text='<%# GetSlaDisplayText(Eval("CreatedOnDate"), Eval("DocumentType")) %>'
                                                                       ToolTip='<%# GetSlaToolTip(Eval("CreatedOnDate"), Eval("DocumentType")) %>' />
                                                            <asp:Label ID="lblSlaExpiry" runat="server" CssClass="sla-expiry-date"
                                                                       Text='<%# "Expires: " + GetSlaExpiryDateText(Eval("CreatedOnDate"), Eval("DocumentType")) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Surname">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSurname" runat="server" Text='<%#Eval("Surname") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Full Names">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFullName" runat="server" Text='<%#Eval("FullName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reference Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReferenceNumber" runat="server" Text='<%#Eval("ReferenceNumber") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Student Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStudentNumber" runat="server" Text='<%#Eval("StudentNumber") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Identifier Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIDNumber" runat="server" 
                                                                       Text='<%# FormatIdNumber(Eval("StudentIDNumber").ToString()) %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                  <%--  <asp:TemplateField HeaderText="Identifier Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIDNumber" runat="server" Text='<%#Eval("StudentIDNumber") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Date Approved">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDateApproved" runat="server" Text='<%#Eval("LastUpdated") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approved By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprovedBy" runat="server" Text='<%#Eval("LastUpdatedBy") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Email Address">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmailAddress" runat="server" Text='<%#Eval("EmailAddress") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Phone Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPhoneNumber" runat="server" Text='<%#Eval("PhoneNumber") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Actions">
                                                        <ItemTemplate>

                                                            <asp:HiddenField runat="server" ID="hfStudentStatus" Value='<%# Eval("StudentStatus")%>' />
                                                            <asp:HiddenField runat="server" ID="hfReferenceNumber" Value='<%# Eval("ReferenceNumber")%>' />
                                                            <asp:HiddenField runat="server" ID="hfStudentId" Value='<%# Eval("StudentID")%>' />
                                                            <asp:HiddenField runat="server" ID="hfPaymentMethod" Value='<%# Eval("PaymentMethod")%>' />

                                                            <asp:LinkButton ID="download" runat="server" CommandArgument='<%# Eval("StudentID")%>' CommandName="downloadForm" OnClick="ExportToPDF" CausesValidation="false">Download Form</asp:LinkButton>
                                                            | 
                                                        <asp:LinkButton ID="view" runat="server" CommandArgument='<%# Eval("StudentID")%>' CommandName="viewARequest" OnClick="view_Click" CausesValidation="false">View Request</asp:LinkButton>
                                                            |                                                         
                                                        <asp:LinkButton ID="completed" runat="server" CommandArgument='<%# Eval("StudentID")%>' CommandName="complete" OnClick="completed_Click" CausesValidation="false">Complete</asp:LinkButton>|
                                                        <%--<asp:LinkButton ID="email" runat="server" CommandArgument='<%# Eval("StudentID")%>' CommandName="sendEmail" OnClick="GvAllOrders_RowCommand">Send Email</asp:LinkButton>--%>
                                                            <asp:LinkButton ID="email" runat="server" CommandArgument='<%# Eval("StudentID")%>' CommandName="sendEmail" OnClick="sendEmailLink_Click" CausesValidation="false">Send Email</asp:LinkButton>

                                                            <asp:LinkButton ID="sendDocLink" runat="server" Visible='<%# (Eval("DeliveryType").ToString() == "Electronic Copy") ? true : false %>' CommandArgument='<%# Eval("StudentID")%>' CommandName="sendDocument" OnClick="sendDocLink_Click" CausesValidation="false">| Send Document</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle CssClass="GridPager" />
                                            </asp:GridView>
                                        </div>
                                        <br />
                                        <div id="Div7" style="overflow: auto; overflow-style: panner" runat="server" visible="false" class="form-group">
                                            <asp:GridView ID="gvExport" runat="server" PageSize="2" Width="500px" AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:BoundField Visible="true" DataField="CreatedOnDate" HeaderText="Created On Date" SortExpression="CreatedOnDate" />
                                                    <asp:BoundField Visible="true" DataField="CreatedOnTime" HeaderText="Created On Time" SortExpression="CreatedOnTime" />
                                                    <asp:BoundField Visible="true" DataField="Surname" HeaderText="Surname" SortExpression="Surname" />
                                                    <asp:BoundField Visible="false" DataField="MaidenSurname" HeaderText="Maiden Surname" SortExpression="MaidenSurname" />
                                                    <asp:BoundField Visible="true" DataField="FullName" HeaderText="Full Name" SortExpression="FullName" />
                                                    <asp:BoundField Visible="true" DataField="StudentNumber" HeaderText="Student Number" SortExpression="StudentNumber" />
                                                    <asp:BoundField Visible="true" DataField="StudentIDNumber" HeaderText="Student ID Number" SortExpression="StudentIDNumber" />
                                                    <asp:BoundField Visible="true" DataField="QualificationName" HeaderText="Qualification Name" SortExpression="QualificationName" />
                                                    <asp:BoundField Visible="true" DataField="FacultyName" HeaderText="Faculty Name" SortExpression="FacultyName" />
                                                    <asp:BoundField Visible="true" DataField="FromYear" HeaderText="FromYear" SortExpression="FromYear" />
                                                    <asp:BoundField Visible="true" DataField="ToYear" HeaderText="ToYear" SortExpression="ToYear" />
                                                    <asp:BoundField Visible="true" DataField="EmailAddress" HeaderText="Email Address" SortExpression="EmailAddress" />
                                                    <asp:BoundField Visible="true" DataField="ContactNumber" HeaderText="Contact Number" SortExpression="ContactNumber" />
                                                    <asp:BoundField Visible="true" DataField="RequestStatus" HeaderText="Request Status" SortExpression="RequestStatus" />
                                                    <asp:BoundField Visible="true" DataField="DocumentType" HeaderText="Document Type" SortExpression="DocumentType" />
                                                    <asp:BoundField Visible="true" DataField="DeliveryType" HeaderText="Delivery Type" SortExpression="DeliveryType" />
                                                                                                           
                                                    <asp:BoundField Visible="true" DataField="PaymentMethod" HeaderText="Payment Type" SortExpression="PaymentMethod" />
                                                    <asp:BoundField Visible="true" DataField="PaymentStatus" HeaderText="Transaction Status" SortExpression="PaymentStatus" />
                                                    <asp:BoundField Visible="true" DataField="TotalAmount" HeaderText="Total Amount" SortExpression="TotalAmount" />
                                                    <asp:BoundField Visible="false" DataField="PaygateReference" HeaderText="Paygate Reference" SortExpression="PaygateReference" />
                                                                                                        
                                                    <asp:BoundField Visible="false" DataField="ComplexAddress" HeaderText="Complex Address" SortExpression="ComplexAddress" />
                                                    <asp:BoundField Visible="false" DataField="StreetAddress" HeaderText="Street Address" SortExpression="StreetAddress" />
                                                    <asp:BoundField Visible="false" DataField="Suburb" HeaderText="Suburb" SortExpression="Suburb" />
                                                    <asp:BoundField Visible="false" DataField="City" HeaderText="City" SortExpression="City" />
                                                    <asp:BoundField Visible="false" DataField="Code" HeaderText="Code" SortExpression="Code" />
                                                    <asp:BoundField Visible="false" DataField="Country" HeaderText="Country" SortExpression="Country" />
                                                    <asp:BoundField Visible="true" DataField="LastUpdatedBy" HeaderText="Approved By" SortExpression="LastUpdatedBy" />
                                                    <asp:BoundField Visible="true" DataField="LastUpdated" HeaderText="Date Approved" SortExpression="LastUpdated" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="popUpSendEmail" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <div class="modal fade" id="popUpSendEmailModal" role="dialog" data-backdrop="static" data-keyboard="false">
                        <div class="modal-block modal-block-lg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 id="modalTitle" runat="server" class="modal-title">Send Email </h4>
                                </div>
                                <div id="modalBody" runat="server" class="modal-body">
                                    <p>
                                        <div class="form-inline">
                                            <label class="col-md-2 control-label ">Defaults: </label>
                                            <div class="form-inline">
                                                <asp:Button ID="btnAcademicRecord" CssClass="btn btn-primary " runat="server" Text="Academic Record" CausesValidation="false" Width="150px" OnClick="btnAcademicRecord_Click" OnClientClick="ShowPopUp();" />
                                                <label class="col-md-2 control-label " style="width: 20px"></label>
                                                <asp:Button ID="btnTranscript" CssClass="btn btn-primary btnCancel" runat="server" Text="Transcript" CausesValidation="false" Width="150px" OnClick="btnTranscript_Click" OnClientClick="ShowPopUp();" />
                                                <label class="col-md-2 control-label " style="width: 20px"></label>
                                                <asp:Button ID="btnLetter" CssClass="btn btn-primary btnCancel" runat="server" Text=" Confirmation Letter" CausesValidation="false" Width="200px" OnClick="btnLetter_Click" OnClientClick="ShowPopUp();" />
                                            </div>
                                        </div>
                                        <br></br>
                                        <div class="form-group">
                                            <label class="col-md-2 control-label " style="width: 105px"></label>

                                            <%--<div class="col-md-6" style="padding-top: 8px">--%>
                                            <div class="form-inline">
                                                <asp:TextBox ID="txtMessage" runat="server" Width="650" Height="250" placeholder="Your body message" TextMode="MultiLine"></asp:TextBox>

                                            </div>
                                            <div id="div1" class="form-group" runat="server">
                                                <%--<asp:RequiredFieldValidator ID="rfvMessage" runat="server" ForeColor="Red" ControlToValidate="txtMessage" ErrorMessage="Required"> </asp:RequiredFieldValidator>--%>
                                                <asp:Label ID="lblErrMessage" runat="server" Style="text-align: left" ForeColor="Red" Width="547px"></asp:Label>
                                            </div>
                                            <%--</div>--%>
                                        </div>
                                    </p>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnSend" class="btn btn-primary " UseSubmitBehavior="false" Text="Send" CausesValidation="false" OnClick="btnSend_Click" OnClientClick="ShowPopUp();" />
                                    <asp:Button runat="server" ID="btnCancelPopUp" class="btn btn-primary " UseSubmitBehavior="false" Text="Cancel" OnClick="btnCancelPopUp_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="popupModal" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <div class="modal fade" id="myModal" role="dialog" data-backdrop="static" data-keyboard="false">
                        <div class="modal-dialog" style="padding-top: 20px">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 id="modalHead" runat="server" class="modal-title">Complete</h4>
                                </div>
                                <div style="padding: 25px; align-content: center">
                                    Please confirm that all is in order and the request can be updated to complete.
                                </div>
                                <div class="modal-footer">
                                    <span style="padding: 15px; align-content: center">
                                        <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="btn btn-primary" OnClick="btnConfirm_Click" OnClientClick="myModalShowPopUp();" />
                                        <asp:Button ID="btnConfirmCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" CausesValidation="false" OnClientClick="return myModalCancelAndClose();" />
                                    </span>
                                </div>
                            </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="SendDocsUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <div class="modal fade" id="popUpSendDocsModal" role="dialog" data-backdrop="static" data-keyboard="false">
                        <div class="modal-block modal-block-lg">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 id="H1" runat="server" class="modal-title">Send Documents </h4>
                                </div>
                                <div id="Div2" runat="server" class="modal-body">
                                    <div class="form-group">
                                        <%-- <div class="form-inline">--%>

                                        <div class="form-group">
                                            <div class="col-md-3"></div>
                                            <div class="col-md-6">
                                                <div id="Div5" class="form-group" runat="server">
                                                    <label class="control-label "><span style='color: white'>*</span> Email: </label>

                                                    <asp:TextBox ID="txtEmail" autocomplete="off" runat="server" MaxLength="30" placeholder="Enter Surname" class="form-control" type="text" ReadOnly="true"></asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                        <div id="academicRecordDiv" runat="server" visible="false" class="form-group">
                                            <div class="col-md-3"></div>
                                            <div class="col-md-6">
                                                <div id="Div61" class="form-group" runat="server">
                                                    <label class="control-label "><span style='color: white'>*</span> Academic Record: </label>

                                                    <asp:FileUpload ID="academicRecordFileUpload" runat="server" Height="27px" Width="300px"></asp:FileUpload>
                                                </div>
                                            </div>
                                        </div>

                                        <div id="academicTranscriptSupplementDiv" runat="server" visible="false" class="form-group">
                                            <div class="col-md-3"></div>
                                            <div class="col-md-6">
                                                <div id="Div4" class="form-group" runat="server">
                                                    <label class="control-label "><span style='color: white'>*</span> Academic Transcript Supplement: </label>

                                                    <asp:FileUpload ID="academicTranscriptSupplementFileUpload" runat="server" Height="27px" Width="300px"></asp:FileUpload>
                                                </div>
                                            </div>
                                        </div>

                                        <div id="confirmationLetterDiv" runat="server" visible="false" class="form-group">
                                            <div class="col-md-3"></div>
                                            <div class="col-md-6">
                                                <div id="Div6" class="form-group" runat="server">
                                                    <label class="control-label "><span style='color: white'>*</span> Confirmation Letter: </label>
                                                    <asp:FileUpload ID="confirmationLetterFileUpload" runat="server" Height="27px" Width="300px"></asp:FileUpload>
                                                </div>
                                            </div>
                                        </div>

                                        <%--</div>--%>
                                    </div>
                                    <br></br>
                                    <div class="form-group">
                                        <label class="col-md-2 control-label " style="width: 105px"></label>

                                        <%--<div class="col-md-6" style="padding-top: 8px">--%>
                                        <div class="form-inline">
                                            <asp:TextBox ID="txtEmailYourBody" runat="server" Width="650" Height="250" placeholder="Your body message" TextMode="MultiLine"></asp:TextBox>

                                        </div>
                                        <div id="div3" class="form-group" runat="server">
                                            <%--<asp:RequiredFieldValidator ID="rfvMessage" runat="server" ForeColor="Red" ControlToValidate="txtMessage" ErrorMessage="Required"> </asp:RequiredFieldValidator>--%>
                                            <asp:Label ID="Label1" runat="server" Style="text-align: left" ForeColor="Red" Width="547px"></asp:Label>
                                        </div>
                                        <%--</div>--%>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnSendDocs" class="btn btn-primary " UseSubmitBehavior="false" Text="Send" CausesValidation="false" OnClick="btnSendDocs_Click" OnClientClick="ShowPopUp();" />
                                    <asp:Button runat="server" ID="Button5" class="btn btn-primary " UseSubmitBehavior="false" Text="Cancel" OnClick="btnCancelPopUp_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>
</asp:Content>
