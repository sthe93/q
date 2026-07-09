<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestView.aspx.cs" Inherits="QualificationVerificationWeb.Admin.RequestView" MasterPageFile="~/Admin/AcademicDocumentMaster.Master" ValidateRequest="false" Async="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        tinymce.init({ selector: 'textarea', Width: 500, Height: 250, menubar: false, statusbar: false, toolbar: false });
    </script>
    <script type="text/javascript">

        function CancelAndClose() {
            $("#myModal").removeClass('fade').modal('hide')
            return false;
        }

        function ReasonSelected() {
            var selectedReasonValue = $('#ContentPlaceHolder1_ddlReason').val();

            if (selectedReasonValue == 0) {
                return false;
            }
            else {
                return true;
            }
        }

    </script>
    
       <script type="text/javascript">  
           function mySaveModalPopUp() {
               $("#mySaveModal").modal('show');
           }

    </script>
    <script type="text/javascript">

        function mySaveModalCancelAndClose() {
            $("#mySaveModal").removeClass('fade').modal('hide');
            return false;
        };
    </script>

      <script type="text/javascript">

          function myEmailModalCancelAndClose() {
              $("#popUpSendEmailModal").removeClass('fade').modal('hide');
              return false;
          };
    </script>

          <script type="text/javascript">

              function myEmailModalPopUp() {
                  $("#popUpSendEmailModal").modal('show');
                  return false;
              };
    </script>
    <style type="text/css">
    /* Force all links in modals to be clickable */
    .modal a, 
    .modal .btn-link,
    .modal .btn,
    .modal input[type="submit"],
    .modal button {
        pointer-events: auto !important;
        z-index: 9999 !important;
        position: relative !important;
    }
    
    /* Ensure grid view links are clickable */
    .table a {
        pointer-events: auto !important;
        cursor: pointer !important;
    }
    
    /* Fix any potential overlay issues */
    .modal-backdrop {
        z-index: 1040 !important;
    }
    
    .modal {
        z-index: 1050 !important;
    }
    
    .modal-content {
        z-index: 1060 !important;
    }
    
    /* Ensure grid view inside modal is clickable */
    #gvSentEmail {
        position: relative;
        z-index: 1070;
    }
    
    #gvSentEmail a {
        display: inline-block !important;
        padding: 5px 10px !important;
        background-color: #007bff !important;
        color: white !important;
        border-radius: 3px !important;
        text-decoration: none !important;
    }
    
    #gvSentEmail a:hover {
        background-color: #0056b3 !important;
        cursor: pointer !important;
    }
</style>
    <script type="text/javascript">
        $(document).ready(function () {
            // Check if the modal content is visible and clickable
            $(document).on('mouseenter', '#gvSentEmail a', function () {
                console.log('Mouse entered View link');
                $(this).css('background-color', 'yellow');
            });

            $(document).on('click', '#gvSentEmail a', function (e) {
                console.log('CLICK DETECTED on View link');
                e.preventDefault();
                alert('Click intercepted! The link is clickable but event is being blocked');
                return false;
            });

            // Check for overlays
            setInterval(function () {
                var modal = $('#popUpSendEmailModal');
                if (modal.is(':visible')) {
                    var links = modal.find('a:contains("View")');
                    if (links.length > 0) {
                        var position = links.first().position();
                        var centerX = position.left + (links.first().width() / 2);
                        var centerY = position.top + (links.first().height() / 2);

                        // Get element at the link's position
                        var elemAtPosition = document.elementFromPoint(centerX, centerY);
                        console.log('Element at View link position:', elemAtPosition);

                        if (elemAtPosition !== links.first()[0]) {
                            console.log('!! Something is overlaying the View link !!');
                            console.log('Overlay element:', elemAtPosition);
                        }
                    }
                }
            }, 1000);
        });
    </script>

<%--    <script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    { 
        if(!Page.IsPostBack)
        {
            RadioButtonList1.Items.Add(new ListItem("Proof of Payment", "1"));
            RadioButtonList1.Items.Add(new ListItem("Proof of ID or Passport", "2"));
        }
    }
   </script>    --%>    

    <style type="text/css">
        .RBL label
        {
            /*display: block;*/
             margin-right:20px;
        }

        .RBL td
        {
            /*margin-left: 50px;*/
            width: 250px;
            
        }
        .xtraField {
            color: #D95B28;
        }
        .modal-dialog{
            margin:50px !important;
        }
        .setHeight {
            height: 41px !important;
        }

        .alignCheckBox {
            width: 25px !important;
        }

        .setWidth {
            width: 110px !important;
        }

        .btnSubmitPadding {
            margin-left: 210px !important;
        }

        </style>
    <div class="tab-content">
        <asp:UpdatePanel ID="pnlManageAppointments" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="form-group">
                    <div class="panel-body">
                        <div class="form-group">
                            <div class="col-md-0">
                                <div runat="server" id="divUserInformation">
                                    <div id="divErrMsg" class="form-group" runat="server">
                                            <asp:Label ID="lblErrorMessage" runat="server" Style="text-align: left" ForeColor="Red" Width="547px"></asp:Label>
                                     </div>
                                    <table border="0">
                                        <tr>
                                            <td style="width:10%"><label class="col-md-0 control-label"><b>Date Created : </b> </label></td>
                                            <td style="width:20%"><label id="lblDateCreated" runat="server" class="col-md-0 control-label"></label></td>
                                            <td style="width:10%"><label class="col-md-0 control-label"><b>Student Number :</b></label></td>
                                            <td style="width:20%"><label id="lblStudentNumber" runat="server" class="col-md-0 control-label"></label></td>
                                            <td style="width:10%"><label class="col-md-0 control-label" ><b>Email Address : </b></label></td>
                                            <td style="width:20%"><label id="lblEmail" runat="server" class="col-md-0 control-label"></label></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                         <tr>
                                            <td style="width:10%"><label class="col-md-0 control-label"><b>Surname :</b></label></td>
                                            <td style="width:20%"><label id="lblSurname" runat="server" class="col-md-0 control-label"></label></td>
                                            <td style="width:10%"><label class="col-md-0 control-label"><b>Full Name : </b></label></td>
                                            <td style="width:20%"><label id="lblFullName" runat="server" class="col-md-0 control-label"></label></td>
                                            <td style="width:10%"><label class="col-md-0 control-label"><b>Maiden Surname : </b></label></td>
                                            <td style="width:20%"><label id="lblMaidenSurname" runat="server" class="col-md-0 control-label"></label></td>
                                        </tr>
                                         <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                         <tr>
                                            <td style="width: 10%"><label class="col-md-0 control-label"><b>Identifier : </b></label></td>
                                            <td style="width: 20%"><label id="lblIdentifier" runat="server" class="col-md-0 control-label" ></label></td>
                                            <td style="width: 10%"></td>
                                            <td style="width: 20%"></td>
                                            <td style="width: 10%"></td>
                                            <td style="width: 20%"></td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                               </table>

                                   <hr/>
                                    <table border="0">
                                        <tr style="height:20%">
                                            <td style="width:15%"><label id="lblDuplicateOf2"  runat="server" visible ="true"><b>Duplicate Request of : </b></label></td>
                                            <td style="width:45%"><label id="lblDuplicateOf" runat="server"  visible="true" ></label></td>
                                            <td style="width:15%"><label id="lblRef" class="xtraField" runat="server"><b>Reference: </b></label></td>
                                            <td style="width:25%"><label id="lblReference" runat="server"  visible="true"></label> </td>
                                           
                                        </tr>
                                        <tr style="height:10%">
                                            <td></td>
                                            <td></td>
                                            <td><label id="lblPaymentMethod2" class="xtraField"  runat="server" visible ="true"><b>Payment Type : </b></label></td>
                                            <td><label id="lblPaymentMethod" runat="server"  visible="true" ></label></td>
                                        </tr>

                                        <div id="onlineGet" runat="server" visible="false">
                                        <tr style="height:10%;">
                                            <td></td>
                                            <td></td>
                                            <td><label id="lblAmount" runat="server" class="xtraField" ><b>Amount Paid: </b></label></td>
                                            <td><label id="lblAmountPaid" runat="server"></label></td>
                                        </tr>
                                        <tr style="height:10%;">
                                           <td></td>
                                           <td></td>
                                           <td style="width:15%"><label id="lblPaymentStatus2"  runat="server" class="xtraField"  visible ="true"><b>Transaction Status : </b></label></td>
                                           <td style="width:25%"><label id="lblPaymentStatus" runat="server"  visible="true" ></label></td>                                         
                                        </tr>
                                        <tr style="height:10%;">
                                           <td></td>
                                           <td></td>
                                           <td><label id="lblTransactionD" class="xtraField"  runat="server"><b>Transaction Date: </b></label></td>
                                           <td><label id="lblTransactionDate" runat="server"  visible="true"></label></td>
                                        </tr>

                                        </div>
                                         <tr style="height:20%">
                                           <td style="width: 15%"><label id="lblTnC" runat="server"><b>Terms and Condition : </b></label></td>
                                           <td style="width:45%"><label id="lblTnCselected" runat="server" ></label></td>
                                        <div id="divRefPaygate" runat="server" visible="false">
                                           <td><label id="lblRefPaygate" class="xtraField" runat="server"><b>Paygate Reference: </b></label></td>
                                           <td><label id="lblReferencePaygate" runat="server"  visible="true"></label> </td>

                                        </div>
                                         </tr>
                                       
                                        <tr style="height:10%">
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr style="height:20%">
                                            <td style="width: 15%"><label ><b>Delivery Method : </b></label></td>
                                            <td ><label id="lblMethod" runat="server" ></label></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr style="height:10%">
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr style="height:20%">
                                            <td style="width: 15%"><label><b>Document Type :</b></label></td>
                                            <td ><label id="lblDocumentType" runat="server"></label></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                          <tr style="height:10%">
                                            <td></td>
                                            <td></td>
                                        </tr >
                                           <tr style="height:20%">
                                            <td style="width: 15%">&nbsp;</td>
                                            <td><label id="lblTotalCost" runat="server"></label></td>
                                               <td></td>
                                               <td></td>
                                        </tr> 
                                           <tr style="height:10%">
                                            <td></td>
                                            <td></td>
                                               <td></td>
                                               <td></td>
                                        </tr>
                                         <tr style="height:20%">
                                            <td style="width: 15%"><label></label></td>
                                            <td ><label><asp:LinkButton ID="lbtnAddressAndRequest"  runat="server" OnClick="lbtnAddressAndRequest_Click" CausesValidation="false">View Request Summary Information</asp:LinkButton> 
                                        </label></td>
                                        </tr>
                                    </table>
                          
                                    <br />
                                    <%--ssrs--%>
                                    <div id="divReportBack" runat="server">
                                        <iframe id="iframeDocument" runat="server" allowfullscreen="" frameborder="0" scrolling="yes" style="width: 100%; height: 55em; margin: 0px; border: 0px; overflow: scroll; -ms-zoom: 0.53;"></iframe>
                                        <br />
                                        <br />
                                        <table border="0">
                                            <tr style="height:45px">
                                                <td style="width:25%"><label> <b>View Documents : </b></label></td>
                                                <td><asp:RadioButtonList  ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="RBL" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" Enabled="false">
                                                    <asp:ListItem Text=" Proof of Payment" Value= "1" Selected="True" />
                                                    <asp:ListItem Text=" Proof of ID or Passport" Value="2"  />
                                                    </asp:RadioButtonList> 
                                                <label id="lblRadioB"  visible="false"></label> </td>
                                            </tr>
                                            <tr style="height:25px">
                                                <td></td>
                                                <td></td>
                                            </tr>
                                             <tr>
                                                <td style="width:25%"><label><b>Special instructions : </b> </label></td>
                                                <td><label id="lblSpecialInstructions" runat="server"  style="width:800px;"></label></td>
                                              
                                            </tr>
                                            
                                             <tr style="height:25px">
                                                <td></td>
                                                <td></td>
                                            </tr>  
                                          
                                              <tr style="height:45px">
                                                <td style="width:25%"><label><b>Special instructions Download : </b></label></td>
                                                <td><asp:LinkButton ID="lbtnInstructionDowmload"  runat="server"  CommandName="viewsome" CausesValidation="false" OnClick="lbtnInstructionDowmload_Click" Visible="false">Download Form</asp:LinkButton> </td>
                                              
                                            </tr>
                                             <tr style="height:25px">
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <%--<div id="Div14" runat="server" visible="false">  --%>
                                             <tr style="height:45px">
                                                <td style="width:25%"><label id="lblPaymentStatusGet" runat="server"  visible="true" style="font-weight:bold;"></label></td>
                                                <td> <asp:DropDownList ID="ddlProofPayment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProofPayment_SelectedIndexChanged" Visible="false">
                                                <asp:ListItem Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Valid" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Invalid" Value="2"></asp:ListItem>
                                                 </asp:DropDownList>
                                                <label id="lblProvePay" runat="server"  visible="false"></label></td>
                                               
                                            </tr> 
                                          <%--  </div>--%>
                                            <tr style="height:25px">
                                                <td></td>
                                                <td></td>
                                            </tr>
                                           
                                            <tr style="height:45px">
                                                <td style="width:25%"><label><b>Proof of ID or Passport status : </b></label></td>
                                                <td><asp:DropDownList ID="ddlProofIDorPassport" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProofIDorPassport_SelectedIndexChanged" Visible="false">
                                                <asp:ListItem Value="0">Select proof of ID or Passport status</asp:ListItem>
                                                <asp:ListItem Text="Valid" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Invalid" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <label id="lblIDorPassportCorrect" runat="server"  visible="false"></label></td>
                                               
                                            </tr>
                                               
                                             <tr style="height:25px">
                                                <td></td>
                                                <td></td>
                                            </tr>
                                             <tr style="height:45px">
                                                <td style="width:25%"><label> <b>Request status : </b></label></td>
                                                <td><asp:DropDownList ID="ddlRequestStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestStatus_SelectedIndexChanged" Visible="false">
                                                    <asp:ListItem Value="0">Select request status</asp:ListItem>
                                                    </asp:DropDownList>
                                                   <asp:RequiredFieldValidator ID="rfvRequestStatus" runat="server" ControlToValidate="ddlRequestStatus" Display="Dynamic" ErrorMessage="Please select Request Status" ForeColor="Red" InitialValue="0" Style="display: inline; padding-left: 20px;"></asp:RequiredFieldValidator>
                                                <label id="lblRequestStatus" runat="server"  visible="false"></label></td>
                                                
                                            </tr>
                                             <tr style="height:25px">
                                                <td></td>
                                                <td></td>
                                            </tr>
                                              <div id="Div2" runat="server" visible="false">
                                             <tr style="height:45px">
                                                <td style="width:25%"><label><b>Decline reasons : </b></label></td>
                                                <td><asp:DropDownList ID="ddlDeclineReasons" runat="server" AutoPostBack="true" Visible="false">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvDeclineReasons" runat="server" ControlToValidate="ddlDeclineReasons" Display="Dynamic" ErrorMessage="Please select Decline Reasons" ForeColor="Red" InitialValue="0" Style="display: inline; padding-left: 20px;"></asp:RequiredFieldValidator>
                                                <label id="lblDeclineReasons" runat="server" style="width: 900px" visible="false"></label></td>
                                                
                                            </tr>
                                                  </div>
                                            
                                            <div id="Div12" runat="server" class="form-inline" visible ="false">
                                                 
                                             <%--<tr style="height:25px">
                                                <td></td>
                                                <td></td>
     
                                            </tr>--%>
                                           
                                             <tr style="height:45px">
                                                  <td style="width:25%"> <label ><b>Recall : </b></label></td>
                                                  <td> <asp:LinkButton ID="lblRecall"  runat="server"  CommandName="viewsome" CausesValidation="false" OnClick="lblRecall_OnClick" >Recall Order</asp:LinkButton> </td>
                                            </tr>

                                            </div>
                                             <tr style="height:25px">
                                                <td></td>
                                                <td></td>
                                                
                                            </tr>
                                            <div id="Div10" runat="server" class="form-inline" visible ="false">
                                             <tr style="height:45px">
                                                <td style="width:25%"> <label ><b>Emails sent : </b></label></td>
                                                <td> <asp:LinkButton ID="lbtnEmails"  runat="server"  CommandName="viewsome" CausesValidation="false" OnClick="lbtnEmails_Click" >View Emails</asp:LinkButton> </td>
                                            </tr>

                                            </div>
                                             <tr style="height:25px">
                                                <td></td>
                                                <td></td>
                                              
                                            </tr>

                                        </table>
                                         
                                   
                                    <div class="form-group">
                                        <asp:Button ID="btnSave" runat="server" CausesValidation="false" CssClass=" btn btn-primary" OnClick="btnSave_Click" Text="Save" Visible="false" />
                                        <asp:Button ID="btnClose" runat="server" CausesValidation="false" CssClass="btn btn-primary" OnClick="btnClose_Click" Text="Cancel" Visible="false" />
                                        <asp:Button ID="btnBack" runat="server" CausesValidation="false" CssClass="btn btn-primary" OnClick="btnBack_Click" Text="Back" Visible="false" />
                                    </div>
                                    
                                    <br>
                                        <br></br>
                                        <br>
                                        <br></br>
                                        <br>
                                        <br></br>
                                        <br>
                                        <br></br>
                                        </br>
                                        </br>
                                        </br>
                                        </br>
                                </div>
                            </div>
                        </div>
                    </div>
                     <asp:UpdatePanel ID="popupModal" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                    <ContentTemplate>
                        <div class="modal fade" id="myModal" role="dialog" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog" style="padding-top: 20px">
                                <div class="modal-content" style="width:200%; margin-left: 25px">
                                    <div class="modal-header">
                                        <h4 id="modalHead" runat="server" class="modal-title" visible="false">Request Summary Information </h4>
                                        <h4 id="sentEmail" runat="server" class="modal-title" visible="false">Emails Sent</h4>
                                        <h4 id="viewEmail" runat="server" class="modal-title" visible="false">View Email</h4>
                                    </div>
                                    <br />
                                    <div>
                                         <asp:GridView ID="gvSummary" runat="server" AllowPaging="true" Class="table table-bordered table-striped mb-none" Font-Size="Small"  PageSize="12" AutoGenerateColumns="False" HeaderStyle-CssClass="control-label" Width="95%" Style="margin-left: 30px;" Visible="false" OnPageIndexChanging="gvSummary_PageIndexChanging">
                                                <EmptyDataTemplate>
                                                    <div>
                                                        No data found.
                                                    </div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Delivery" ItemStyle-Width="15%" ItemStyle-Font-Size="Small">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDeliveryType" runat="server" Text='<%# Eval("DeliveryType")%>' ItemStyle-Font-Size="Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document" ItemStyle-Width="20%" ItemStyle-Font-Size="Small">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDocumentType" runat="server"  Text='<%# Eval("DocumentType")%>' Font-Size="Small"  />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Number of Copies" ItemStyle-Width="10%" ItemStyle-Font-Size="Small">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNumberCopies" runat="server" Text='<%#Eval("NumberCopies") %>' Font-Size="Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Address" ItemStyle-Width="20%" ItemStyle-Font-Size="Small">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' Font-Size="Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   
                                                </Columns>
                                            </asp:GridView>
                                         
                                          <asp:GridView runat="server" ID="gvSentEmail" AllowPaging="True" Class="table table-bordered table-striped mb-none" PageSize="15" AutoGenerateColumns="False" HeaderStyle-CssClass="control-label" Visible="false">
                                            <EmptyDataTemplate>
                                                <div>
                                                    No data found.
                                                </div>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Date Created">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCreatedDate" runat="server" Text='<%# Eval("CreatedDate")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Email Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmailName" runat="server" Text='<%# Eval("EmailName")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Actions">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfEmailSentID" runat="server" Value='<%# Eval("EmailSentID") %>' />
                                                        <asp:HiddenField ID="hfMessage" runat="server" Value='<%# Eval("Message") %>' />
                                                        <asp:LinkButton ID="lbtnViewemail" runat="server" CommandName="viewsome" CausesValidation="false" OnClick="lbtnViewemail_Click" >View</asp:LinkButton> 
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>

                                        </asp:GridView>
                                        <%--<label id="lblMessage" runat="server" visible="false"  class="col-md-2 control-label" style="width: 40%" />--%>
                                         
                                    </div>
                                    <br />
                                    <div>
                                        <span style="padding: 30%; align-content: center">
                                            <asp:Button ID="btnpopClose" runat="server" Text="Close" CssClass="btn btn-primary btnSubmitPadding" CausesValidation="false" />
                                        </span>
                                    </div>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

               <asp:UpdatePanel ID="popupSaveModal" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                    <ContentTemplate>
                        <div class="modal fade" id="mySaveModal" role="dialog" data-backdrop="static" data-keyboard="false" style="align-content:center">
                            <div class="modal-dialog" style="padding-top: 20px">
                                <div class="modal-content" style="align-items:center;margin-left:70%;width:100%">
                                    <div class="modal-header">
                                        <h4 id="H1" runat="server" class="modal-title">Process Request</h4>
                                    </div>
                                    <div style="padding: 25px; align-content: center">
                                       <%--<asp:TextBox ID="txtMessage" runat="server" Width="450" Height="250"  placeholder="Your body message" visible="false" ReadOnly="false" style="margin-left: 100px;"></asp:TextBox>--%>
                                       <label id="lblSaveMessage" runat="server" visible="false" class="col-md-2 control-label" style="width: 100%" />
                                        <br />
                                    </div>
                                    <div class="modal-footer" >
                                        <span style="padding: 15px; align-content: center">
                                            <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="btn btn-primary" OnClick="btnConfirm_Click" OnClientClick="mySaveModalPopUp();"/>
                                            <asp:Button ID="btnConfirmCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" CausesValidation="false" OnClientClick="return mySaveModalCancelAndClose();" />
                                        </span>
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
                                <h4 id="modalTitle" runat="server" class="modal-title">View Email</h4>
                            </div>
                            <div id="modalBody" runat="server" class="modal-body">
                                <p>
                                   
                                    <div class="form-group">
                                         <label class="col-md-2 control-label " style="width: 105px"></label>
                                        
                                        <%--<div class="col-md-6" style="padding-top: 8px">--%>
                                        <div class="form-inline">
                                            <%--<asp:TextBox ID="txtMessage" runat="server" Width="650" Height="250" placeholder="Your body message" TextMode="MultiLine" ></asp:TextBox>--%>
                                            <label id="lblMessage" runat="server" visible="false"  class="col-md-2 control-label" style="width: 100%"  />
                                        </div>
                                        
                                        <%--</div>--%>
                                    </div>
                                </p>
                            </div>
                            <div class="modal-footer" >
                                 <span style="padding: 15px; align-content:center;">
                                <asp:Button runat="server" ID="btnCloseEmailPopUp" class="btn btn-primary btnSubmitPadding"  CausesValidation="false" Text ="Close" OnClientClick="myEmailModalCancelAndClose(); "  />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
