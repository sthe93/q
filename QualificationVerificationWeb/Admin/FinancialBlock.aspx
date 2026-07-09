<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinancialBlock.aspx.cs" Inherits="QualificationVerificationWeb.Admin.FinancialBlock" MasterPageFile="~/Admin/AcademicDocumentMaster.Master" Async="true"%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <script type="text/javascript">
        function pageLoad(sender, args) {
            $('[data-toggle="tooltip"]').tooltip();
        }

        $(function () {
            var date = new Date();
            date.setDate(date.getDate());
            $(".data-plugin-datepicker").datepicker({
                //startDate: new Date(date.getFullYear(), date.getMonth(), date.getDate()),
                defaultDate: date,
                endDate: new Date(date.getFullYear(), date.getMonth(), date.getDate())

            });
        });

    </script>
    <style type="text/css">
        .setHeight {
            height: 41px !important;
        }

        .GridPager a, .GridPager span
        {
            display: block;
            height: 20px;
            width: 25px;
            font-weight: bold;
            text-align: center;
            text-decoration: none;
        }
        .GridPager a
        {
           border: 1px solid #969696;
           border-radius: 4px;
        }
        .GridPager span
        {
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
                <li id="tabFinancialBlock" runat="server" tabindex="3" class="active" >
                    <a runat="server" id="FinancialBlockLink" href="FinancialBlock.aspx">
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
                <li id="tabApplicationAlert" runat="server" tabindex="6">
                    <a runat="server" id="applicationAlertControl" href="ApplicationAlert.aspx">
                        <asp:Label runat="server" ID="lblApplicationAlert"> Application Alert </asp:Label></a>
                </li>
                <li id="tabReason" runat="server" tabindex="7" >
                    <a runat="server" id="ReasonController" href="Reason.aspx">
                        <asp:Label runat="server" ID="lblReason"> Manage Reasons </asp:Label></a>
                </li>
            </ul>

            <%--</div>
    <div class="tab-content">--%>
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
                                            <asp:DropDownList ID="ddlDocumentType" runat="server" AutoPostBack="false" Width="200px" >
                                                <asp:ListItem Value="0">Select Document Type</asp:ListItem>
                                                <asp:ListItem Text="Academic Record" Value="R"></asp:ListItem>
                                                <asp:ListItem Text="Academic Transcript Supplement" Value="T"></asp:ListItem>
                                                <asp:ListItem Text="Confirmation Letter" Value="S"></asp:ListItem>
                                                <asp:ListItem Text="Forms for Official Bodies" Value="F"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtStudentIdentifer" runat="server" autocomplete="off" class="form-control setHeight" MaxLength="20" placeholder="Enter Student Number or ID/Passport Number or Reference Number " ReadOnly="false" type="text" Width="490px"></asp:TextBox>
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
                                                    <asp:TemplateField HeaderText="Updated">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastUpdated" runat="server" Text='<%#Eval("LastUpdated") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Updated By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastUpdatedBy" runat="server" Text='<%#Eval("LastUpdatedBy") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Actions">
                                                        <ItemTemplate>

                                                            <asp:HiddenField runat="server" ID="hfStudentStatus" Value='<%# Eval("StudentStatus")%>' />
                                                            <asp:HiddenField runat="server" ID="hfStudentId" Value='<%# Eval("StudentID")%>' />
                                                               <asp:HiddenField runat="server" ID="hfPaymentMethod" Value='<%# Eval("PaymentMethod")%>' />

                                                            <asp:LinkButton ID="view" runat="server" CommandArgument='<%# Eval("StudentID")%>' CommandName="viewARequest" OnClick="view_Click" CausesValidation="false">View Request</asp:LinkButton>
                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle  CssClass="GridPager"/>
                                            </asp:GridView>
                                        </div>
                                        <div id="Div7" style="overflow: auto; overflow-style: panner" runat="server" visible="false" class="form-group">
                                            <asp:GridView ID="gvExport" runat="server" PageSize="2" Width="700px" AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:BoundField Visible="true" DataField="CreatedOnDate" HeaderText="Created On Date" SortExpression="CreatedOnDate" />
                                                    <asp:BoundField Visible="true" DataField="CreatedOnTime" HeaderText="Created On Time" SortExpression="CreatedOnTime" />
                                                    <asp:BoundField Visible="true" DataField="Surname" HeaderText="Surname" SortExpression="Surname" />
                                                    <asp:BoundField Visible="true" DataField="MaidenSurname" HeaderText="Maiden Surname" SortExpression="MaidenSurname" />
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
                                                    <%--<asp:BoundField Visible="true" DataField="ReferenceNumber" HeaderText="Reference Number" SortExpression="ReferenceNumber" />--%>
                                                     <asp:BoundField Visible="false" DataField="PaygateReference" HeaderText="Paygate Reference" SortExpression="PaygateReference" />
                                                    
                                                    <asp:BoundField Visible="false" DataField="ComplexAddress" HeaderText="Complex Address" SortExpression="ComplexAddress" />
                                                    <asp:BoundField Visible="false" DataField="StreetAddress" HeaderText="Street Address" SortExpression="StreetAddress" />
                                                    <asp:BoundField Visible="false" DataField="Suburb" HeaderText="Suburb" SortExpression="Suburb" />
                                                    <asp:BoundField Visible="false" DataField="City" HeaderText="City" SortExpression="City" />
                                                    <asp:BoundField Visible="false" DataField="Code" HeaderText="Code" SortExpression="Code" />
                                                    <asp:BoundField Visible="false" DataField="Country" HeaderText="Country" SortExpression="Country" />
                                                    <asp:BoundField Visible="true" DataField="LastUpdatedBy" HeaderText="Last Updated By" SortExpression="LastUpdatedBy" />
                                                    <asp:BoundField Visible="true" DataField="LastUpdated" HeaderText="Last Updated" SortExpression="LastUpdated" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
