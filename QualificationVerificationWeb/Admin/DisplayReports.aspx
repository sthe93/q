<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisplayReports.aspx.cs" Inherits="QualificationVerificationWeb.Admin.Test" MasterPageFile="~/Admin/AcademicDocumentMaster.Master" Async="true"  %>
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

                <li id="tabUnProcessed" runat="server" class="tabUnProcessed" visible="true" tabindex="0">
                    <a runat="server" id="UnProcessed" href="~/Admin/Request.aspx">
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
                    <a runat="server" id="Approve" href="~/Admin/Approved.aspx">
                        <asp:Label runat="server" ID="lblApprove"> Approved Request </asp:Label></a>
                </li>
                <li id="tabDeclined" runat="server" tabindex="2">
                    <a runat="server" id="Declined" href="~/Admin/Declined.aspx">
                        <asp:Label runat="server" ID="lblDeclined"> Declined Request </asp:Label></a>
                </li>
                
                <li id="tabCompleted" runat="server" tabindex="4">
                    <a runat="server" id="Completed" href="~/Admin/Completed.aspx">
                        <asp:Label runat="server" ID="lblCompleted"> Completed Request </asp:Label></a>
                </li>
                <li id="tabReport" runat="server" tabindex="5" class="active">
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

                <%--new tab called recall orders--%>




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
                                            <asp:DropDownList runat="server" ID="ddlDocumentType" Width="200px" AutoPostBack="false">
                                                <asp:ListItem Value="0">Select Document Type</asp:ListItem>
                                                <asp:ListItem Value="R" Text="Academic Record"></asp:ListItem>
                                                <asp:ListItem Value="T" Text="Academic Transcript Supplement"></asp:ListItem>
                                                <asp:ListItem Value="S" Text="Confirmation Letter"></asp:ListItem>
                                                <asp:ListItem Value="F" Text="Forms for Official Bodies" ></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList runat="server" ID="ddlApplicationStatus" Width="170px" AutoPostBack="false">
                                                <asp:ListItem Value="-1">Select Application Status</asp:ListItem>
                                                <asp:ListItem Value="2" Text="Approved"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Completed"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Declined"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="Financial block"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="Not Processed"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtStudentIdentifer" runat="server" MaxLength="20" autocomplete="off" placeholder="Enter Student Number or ID/Passport Number or Reference Number" class="form-control setHeight" type="text" ReadOnly="false" Width="490px"></asp:TextBox>
                                            <div class="input-group col-md-2">
                                            <asp:TextBox ID="txtStartDate" runat="server" autocomplete="off" class="data-plugin-datepicker form-control setHeight" data-date-format="yyyy/mm/dd" data-plugin-datepicker="" placeholder="Select Start Date" Width="135px"></asp:TextBox>
                                            <span class="input-group-addon">To </span>
                                            <asp:TextBox ID="txtEndDate" runat="server" autocomplete="off" class="data-plugin-datepicker form-control setHeight" data-date-format="yyyy/mm/dd" data-plugin-datepicker="" placeholder="Select End Date" Width="135px"></asp:TextBox>
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            </div>
                                            <asp:Button runat="server" ID="btnSearch" class="btn btn-primary setHeight" Text="Search" OnClick="btnSearch_Click" />
                                            <div class="form-group" style="margin-left: 5px">
                                                <asp:Button runat="server" ID="btnExport" class="btn btn-primary setHeight" Width="110px" Text="Export"   OnClick="BtnExport_Click" />
                                            </div>
                                          
                                        </div>
                                    </div>

                                    <br />
 
                                    <div class="form-group" style="margin-left: 2px">
                                        <asp:Label ID="lblArcRecordCount" runat="server" Text="" Visible="false"></asp:Label>
                                        <br />
                                        <asp:Label ID="lbltrasSuppCount" runat="server" Text="" Visible="false"></asp:Label>
                                        <br />
                                        <asp:Label ID="lbltrasacRecordAndTransCount" runat="server" Text="" Visible="false"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblFormsCount" runat="server" Text="" Visible="false"></asp:Label>
                                    </div>

                                    <br />
                                    <div class="col-md-12">
                                        <div style="overflow: auto; overflow-style: panner">
                                            <asp:GridView ID="gvAllOrders" runat="server" AllowPaging="true" PagerSettings-Mode="Numeric" PagerSettings-PageButtonCount="4" Class="table table-bordered table-striped mb-none" PageSize="15" AutoGenerateColumns="False" HeaderStyle-CssClass="control-label" OnPageIndexChanging="gvAllOrders_PageIndexChanging" OnRowCommand="gvAllOrders_RowCommand">
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
                                                    <asp:TemplateField HeaderText="Student Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStudentNumber" runat="server" Text='<%#Eval("StudentNumber") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDocumentType" runat="server" Text='<%# Eval("DocumentType")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delivery Method">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDeliveryType" runat="server" Text='<%#Eval("DeliveryType") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <%--country indication column--%>


                                                    <asp:TemplateField HeaderText="Country Indication">
                                                    <ItemTemplate>
                                                      <asp:Label ID="lblCountryIndication" runat="server" Text='<%#Eval("CountryIndication") %>' />
                                                     </ItemTemplate>
                                                     </asp:TemplateField>



                                                    <asp:TemplateField HeaderText="Request Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRequestStatus" runat="server" Text='<%#Eval("RequestStatus") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Last Updated">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastUpdated" runat="server" Text='<%#Eval("LastUpdated") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Last Updated By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLastUpdatedBy" runat="server" Text='<%#Eval("LastUpdatedBy") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Number Of Days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="NumberOfDays" runat="server" Text='<%#Eval("NumberOfDays") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle  CssClass="GridPager"/>
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
        </div>
   </div>
    
    </asp:Content>