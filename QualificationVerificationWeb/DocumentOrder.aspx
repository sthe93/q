<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentOrder.aspx.cs" Inherits="QualificationVerificationWeb.DocumentOrder" MasterPageFile="~/Master.Master" MaintainScrollPositionOnPostback="true" Async="true" %>

<%@ Import Namespace="QualificationVerificationWeb.Helper" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="Content/bootstrap/jquery/jquery.js"></script>
    <link href="<%= ResolveUrl("~/Content/bootstrap/bootstrap-timepicker/css/bootstrap-timepicker.min.css") %>" rel="stylesheet" />
    <link href="<%= ResolveUrl("~/Content/bootstrap/bootstrap-timepicker/css/bootstrap-timepicker.css") %>" rel="stylesheet" />
    <link href="<%= ResolveUrl("~/Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.css") %>"  rel="stylesheet" />
    <script src="Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.js"></script>
    <script src="Scripts/Common/Common.js"></script>
    <!-- Toastr JS -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>
    <!-- SweetAlert2 CDN -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.1/themes/smoothness/jquery-ui.css"/>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.1/themes/smoothness/jquery-ui.css"/>

    <style>
    .btnFeesGrant {
    background-color: #E7540E !Important;
       color: #fff !Important;
    }

.btnCancel {
    margin-left: 30px;
}

.deliveryGridview {
    margin-left: 25px;
                   Width: 582px;
                             border-radius: 35px;
}

                  td, th {
                      padding: 4px 5px 4px 4px !important;
                  }

#divAddSecond {
                         margin-bottom: 0px;
}


.addBtn {
     background-color: #47a447 !important;
     border-radius: 20px !important;
     color: white !important;
     font-weight: bolder !important;
     padding: 0px 6px !important;
}

.deleteBtn {
     background-color: #d9534f !important;
     border-radius: 20px !important;
     color: white !important;
     padding: 0px 6px !important;
}

#calendar {
    max-width: 990px;
    margin: 20px 40px;
}

.form-inline {
    width: 1256px;
}

.modal-block modal-block-lg {
    max-width: 550px;
    width: 95%;
    margin: 50px auto;
    text-align: left;
}

.hiddencol {
    display: none;
}
</style>

<style type="text/css">
.error-message {
    color: red;
    padding-left: 10px;
    display: inline-block;
}
</style>


<script type="text/javascript">  
             function pageLoad(sender, args) {
                 var changevalue = "";
                 $('[data-toggle="tooltip"]').tooltip();

                 $(document).ready(function () {
                 $('.fetch_results').find('input:text').val('');

                 });


             }

    function validateUplod1(fileUploaded, file, oFile) {
    $(file).css("display", "none");
        var maxFileSize = 5242880; // 5MB -> 5 * 1024 * 1024
        var fileUpload = $(fileUploaded);

        var filename = oFile.files[0].name;
        var dotPosition = filename.lastIndexOf(".");
        var fileExt = filename.substring(dotPosition);

        var txtS = document.getElementById('<%=txtSpecialInstruction.ClientID%>').value;
            var txtI = document.getElementById('<%=txtIdPassport.ClientID%>').value;
            var txtP = document.getElementById('<%=txtPaymentProof.ClientID%>').value;


            if (fileUpload.val() == '') {

            }
            else {
                if (fileUpload[0].files[0].size < maxFileSize) {
                    if (fileExt != ".pdf" && fileExt != ".jpg" && fileExt != ".jpeg" && fileExt != ".png" && fileExt != ".PDF" && fileExt != ".JPG" && fileExt != ".JPEG" && fileExt != ".PNG") {
                        $(file).css("display", "inline");
                        fileUpload.val("");
                        $("#<%=txtSpecialInstruction.ClientID%>").val('');
                        return false;
                    }
                    else {
                        <%-- if(txtS == txtI || txtS == txtP)
                              {
                                 $(file).css("display", "inline");
                                fileUpload.val("");
                                 $("#<%=txtSpecialInstruction.ClientID%>").val('');
                                return false;
                              }
                              else{
                                  }--%>
                    }

                }
                else {
                    $(file).css("display", "inline");
                    fileUpload.val("");
                    $("#<%=txtSpecialInstruction.ClientID%>").val('');
                    return false;
                }
            }

        }


        function validateUplod2(fileUploaded, file, oFile) {
            $(file).css("display", "none");
            var maxFileSize = 5242880; // 5MB -> 5 * 1024 * 1024
            var fileUpload = $(fileUploaded);
            var filename = oFile.files[0].name;
            var dotPosition = filename.lastIndexOf(".");
            var fileExt = filename.substring(dotPosition);

            var txtS = document.getElementById('<%=txtSpecialInstruction.ClientID%>').value;
            var txtI = document.getElementById('<%=txtIdPassport.ClientID%>').value;
            var txtP = document.getElementById('<%=txtPaymentProof.ClientID%>').value;


            if (fileUpload.val() == '') {

            }
            else {
                if (fileUpload[0].files[0].size < maxFileSize) {
                    if (fileExt != ".pdf" && fileExt != ".jpg" && fileExt != ".jpeg" && fileExt != ".png" && fileExt != ".PDF" && fileExt != ".JPG" && fileExt != ".JPEG" && fileExt != ".PNG") {
                        $(file).css("display", "inline");
                        fileUpload.val("");
                        $("#<%=txtPaymentProof.ClientID%>").val('');
                        return false;
                    }
                    else {

                    }

                }
                else {
                    $(file).css("display", "inline");
                    fileUpload.val("");
                    $("#<%=txtPaymentProof.ClientID%>").val('');
                    return false;
                }
            }

        }

        function validateUplod3(fileUploaded, file, oFile) {
            $(file).css("display", "none");
            var maxFileSize = 5242880; // 5MB -> 5 * 1024 * 1024
            var fileUpload = $(fileUploaded);
            var filename = oFile.files[0].name;
            var dotPosition = filename.lastIndexOf(".");
            var fileExt = filename.substring(dotPosition);

            var txtS = document.getElementById('<%=txtSpecialInstruction.ClientID%>').value;
            var txtI = document.getElementById('<%=txtIdPassport.ClientID%>').value;
            var txtP = document.getElementById('<%=txtPaymentProof.ClientID%>').value;

            if (fileUpload.val() == '') {

            }
            else {
                if (fileUpload[0].files[0].size < maxFileSize) {
                    if (fileExt != ".pdf" && fileExt != ".jpg" && fileExt != ".jpeg" && fileExt != ".png" && fileExt != ".PDF" && fileExt != ".JPG" && fileExt != ".JPEG" && fileExt != ".PNG") {
                        $(file).css("display", "inline");
                        fileUpload.val("");
                        $("#<%=txtIdPassport.ClientID%>").val('');
                        return false;
                    }
                    else {

                    }

                }
                else {
                    $(file).css("display", "inline");
                    fileUpload.val("");
                    $("#<%=txtIdPassport.ClientID%>").val('');
                    return false;
                }
            }

        }


    </script>


    <script type="text/javascript">  
        function callPaymentProof(oFile) {
            var filename1 = oFile.files[0].name;
            var cleaner = "";

            $("#<%=lblErrorMessage.ClientID%>").val(cleaner);
            $("#<%=txtPaymentProof.ClientID%>").val(filename1);

        }

        function callSpecialInstruction(oFile) {
            var filename1 = oFile.files[0].name;
            var cleaner = "";

            $("#<%=lblErrorMessage.ClientID%>").val(cleaner);
            $("#<%=txtSpecialInstruction.ClientID%>").val(filename1);

        }

        function callCopyIdPassport(oFile) {
            var filename1 = oFile.files[0].name;
            var cleaner = "";

            $("#<%=lblErrorMessage.ClientID%>").val(cleaner);
            $("#<%=txtIdPassport.ClientID%>").val(filename1);

        }


        <%--        function setSpecialInstruction() {
                //alert("Test");
                //var text1= "1";
                //Session["SpecialInstructionsInd"] = text1;
                //alert(text1);

                <%Session["SpecialInstructionsInd"] = "2";%>
                var session_value='<%=Session["SpecialInstructionsInd"]%>'; 
                alert(session_value); 
                
            }--%>
    </script>
    <!--Mandatory script pop up -->

    <script type="text/javascript">
        // Show the modal
        function showMandatoryInstructionModal() {
            $('#myMandatoryInstructionModal').modal('show');
        }

        // Handle OK button click
        function myMandatoryInstructionModalCancelAndClose() {
            $('#myMandatoryInstructionModal').modal('hide');
        }
    </script>

    <script type="text/javascript">

        function CancelAndClose() {
            $("#popUpConfirmationModal").removeClass('fade').modal('hide')
            return false;
        }

        $(document).ready(function () {

            $('[id*=lstAcademicDocument]').multiselect({
                includeSelectAllOption: false
            });
        });


    </script>

    <script type="text/javascript">

        function myModalCancelAndClose() {
            $("#myModal").removeClass('fade').modal('hide');
            return false;
        };
    </script>

    <script type="text/javascript">
        function mySpecialInstructionModalCancelAndClose() {
            $("#mySpecialInstructionModal").removeClass('fade').modal('hide');
            return false;
        };
    </script>

    <script type="text/javascript">
        function mySpecialInstructionModalShow() {
            $('#mySpecialInstructionModal').modal('show');
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('[id*=lstDocumentType]').multiselect({
                includeSelectAllOption: false
            });
        });
    </script>
    <script type="text/ecmascript">
        function checkButtonClicked(buttonSelected) {
            document.getElementById("ContentPlaceHolder1_ButtonClicked").value = buttonSelected;
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            $('[id*=gvAddress]').footable();
        });
    </script>

    <script type="text/javascript">
        function Validate_main() {
            var isValid = false;
            isValid = Page_ClientValidate('Email1');
            if (isValid) {
                isValid = Page_ClientValidate('Phone1');
            }
            return isValid;
        }

        function Validate_pop() {
            var isValid = false;

            isValid = Page_ClientValidate('Code1');

            if (isValid) {
                isValid = Page_ClientValidate('Street1');
            }
            if (isValid) {
                isValid = Page_ClientValidate('Suburb1');
            }
            if (isValid) {
                isValid = Page_ClientValidate('City1');
            }
            if (isValid) {
                isValid = Page_ClientValidate('Code10');
            }
            if (isValid) {
                isValid = Page_ClientValidate('Country');
            }


            return isValid;
        }
    </script>

    <script type="text/javascript">
        function redirectToDefault() {
            window.location.href = "Default.aspx";
        }
    </script>

    <script type="text/javascript">

        function processPayment(payGateId, payRequestId, reference, checksum, postData) {
            localStorage.setItem("PAYGATE_ID", payGateId);
            localStorage.setItem("PAY_REQUEST_ID", payRequestId);
            localStorage.setItem("REFERENCE", reference);
            localStorage.setItem("CHECKSUM", checksum);

            window.location.href = config.serverPath + "PagGateRedirect.aspx";
            //window.open(config.serverPath + "PagGateRedirect.aspx", "_blank");
        }

    </script>

    <script type="text/javascript">

        function ShowPaymentConfirmationStatusModal(message) {
            /*debugger;*/
            document.getElementById('confirmationMessage').innerText = message;
            document.getElementById('paymentConfirmationModal').style.display = 'block';
        }


        function ShowFailedPaymentStatusModal(message) {

            document.getElementById('failedPaymentMessage').innerText = message;
            document.getElementById('failedPaymentModal').style.display = 'block';


        }

        function closeStatusModal() {

            document.getElementById('failedPaymentModal').style.display = 'none';

            window.location.href = "Default.aspx";
        }

    </script>
    <script>
        function loadDraft() {
            // Ensure all elements are visible
            var parentDiv = document.getElementById('<%= divParentsInfo.ClientID %>');
            if (parentDiv) parentDiv.style.display = 'block';


            // Trigger the postback
            __doPostBack('', 'LOAD_DRAFT');
        }
    </script>
    <script>
        function updateFileInputDisplay(inputId, fileName) {
            // Find the corresponding text input
            var textInput = document.getElementById(inputId);
            if (textInput) {
                textInput.value = fileName || ''; // Handle null/undefined filenames

                // Also update the display textbox if it exists
                var displayTextBox = document.getElementById(inputId + '_txtDisplay');
                if (displayTextBox) {
                    displayTextBox.value = fileName || '';
                }
            }
        }
    </script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- Script for Asterisk show-->
    <script type="text/javascript">
        function updateOutsideAsterisk() {
            var show = false;

            $('.official-forms-ddl').each(function () {
                var val = parseInt($(this).val());
                if (!isNaN(val) && val >= 1 && val <= 4) {
                    show = true;
                    return false; // break the loop early
                }
            });
            console.log("Asterisk should be shown?", show);
            
            if (show) {
                $('#asteriskSpecialInstruction').show();

            } else {
                $('#asteriskSpecialInstruction').hide();
            }
        }

        $(document).ready(function () {
            // Run initially on page load
            updateOutsideAsterisk();
           // debugger;

            // Run every time any dropdown changes inside GridView
            $(document).on('change', '.official-forms-ddl', function () {
                updateOutsideAsterisk();
            });
        });

        // Handle partial postbacks inside UpdatePanel
        Sys.Application.add_load(function () {
            updateOutsideAsterisk();
        });

    </script>





    <div class="tab-content">
        <div class="form-group">
            <div class="panel-body" style="width:65%">
                <div class="form-inline">
                    <br />
                    <br />
                    <br />
                    <asp:Label ID="lblErrorMessage1" runat="server" Style="text-align: left" ForeColor="Red" Width="1200px"></asp:Label>
                </div>
                <br />
                <div runat="server" id="divUserInformation">
                    <div id="Div19" class="form-group" runat="server">
                        <label class="col-md-2 control-label ">ID/Passport Number : </label>
                        <div class="form-inline">
                            <asp:TextBox ID="txtID_PassNumber" autocomplete="off" runat="server" placeholder="Enter ID/Passport Number" MaxLength="20" class="form-control" type="text" ReadOnly="false" Width="300px"></asp:TextBox>
                        </div>
                    </div>
                
                    <br />
                    <div id="Div21" class="form-group" runat="server">
                        <label class="col-md-2 control-label "></label>
                        <div class="form-inline">
                            <asp:Button ID="btnNext" CssClass="btn btn-primary " runat="server" Text="Search" CausesValidation="true" OnClick="btnNext_Click" />
                            <asp:Button ID="btnCancelOut" CssClass="btn btn-primary btnCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancelOut_Click" />
                        </div>
                    </div>
                </div>
                <div id="divDraftInfo" runat="server" visible="false" class="alert alert-info">
                    You're currently editing a saved draft. Please complete all required fields and submit when ready.
                </div>
                <%-- parents info--%>
                <div id="parents-section">
                <div runat="server" id="divParentsInfo" visible="false">
                    <h3><b>Academic Document Order Form</b>
                    </h3>
                    <div id="divErrMsg" class="form-group" runat="server">
                        <asp:Label ID="lblErrorMessage" runat="server" Style="text-align: left" ForeColor="Red" Width="547px"></asp:Label>
                    </div>
                    <div id="Div5" class="form-group" runat="server">
                        <label class="col-md-2 control-label "><span style='color: white'>*</span> Surname : </label>
                        <div class="form-inline">
                            <asp:TextBox ID="txtSurname1" autocomplete="off" runat="server" MaxLength="30" placeholder="Enter Surname" class="form-control" type="text" ReadOnly="true" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator
                                runat="server"
                                ControlToValidate="txtSurname1"
                                ID="rfvtxtSurname1" ForeColor="Red" ErrorMessage="Please enter Surname" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div id="Div61" class="form-group" runat="server">
                        <label class="col-md-2 control-label "><span style='color: white'>*</span> Initials : </label>
                        <div class="form-inline">
                            <asp:TextBox ID="txtInitials1" autocomplete="off" runat="server" MaxLength="30" placeholder="Enter Initials" class="form-control" type="text" ReadOnly="true" Width="300px"></asp:TextBox>
                        </div>
                    </div>
                    <div id="Div8" class="form-group" runat="server" visible="false">
                        <label class="col-md-2 control-label "><span style='color: white'>*</span> Maiden Surname : </label>
                        <div class="form-inline">
                            <asp:TextBox ID="txtMaiden1" autocomplete="off" runat="server" MaxLength="30" placeholder="Enter Maiden Surname" class="form-control" type="text" ReadOnly="false" Width="300px"></asp:TextBox>
                        </div>
                    </div>
                    <div id="Div4" class="form-group" runat="server" visible="false">
                        <label class="col-md-2 control-label "><span style='color: white'>*</span> Full Name : </label>
                        <div class="form-inline">
                            <asp:TextBox ID="txtName1" autocomplete="off" runat="server" placeholder="Enter Name" MaxLength="30" class="form-control" type="text" ReadOnly="true" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName1" ID="rfvtxtName1" ForeColor="Red" ErrorMessage="Please enter Full Name" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div id="Div1" class="form-group" runat="server" visible="false">
                        <label class="col-md-2 control-label "><span style='color: white'>*</span> Student Number : </label>
                        <div class="form-inline">
                            <asp:TextBox ID="txtStudentnumber1" autocomplete="off" runat="server" placeholder="Enter Student Number" MaxLength="13" class="form-control" type="text" ReadOnly="true" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStudentnumber1" ID="rfvtxtStudentnumber1" ForeColor="Red" ErrorMessage="Please enter Student Number" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="revStudentNumber1" ControlToValidate="txtStudentnumber1" Display="Dynamic" ErrorMessage="Only numbers allowed." ForeColor="Red" ValidationExpression="^[+]?\d*$"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div id="Div7" class="form-group" runat="server">
                        <label class="col-md-2 control-label "><span style='color: white'>*</span> ID/Passport Number : </label>
                        <div class="form-inline">
                            <asp:TextBox ID="txtIDNumber1" autocomplete="off" runat="server" placeholder="Enter ID Number" MaxLength="20" class="form-control" type="text" ReadOnly="true" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtIDNumber1" ID="rfvtxtIDNumber1" ForeColor="Red" ErrorMessage="Please enter Identification Number (ID/Passport)" Display="Dynamic"></asp:RequiredFieldValidator>
                            <%--<asp:RegularExpressionValidator runat="server" ID="revtxtIDNumber1" ControlToValidate="txtIDNumber1" Display="Dynamic" ErrorMessage="Special characters not allowed." ForeColor="Red" ValidationExpression="^([a-zA-Z0-9_\s\-]*)$"></asp:RegularExpressionValidator>--%>
                        </div>
                    </div>
                    <div id="Div131" class="form-group">
                        <label class="col-md-1 control-label" style="width: 969px;"><b>Please enter current contact email address where correspondence will be sent too.</b></label>
                    </div>
                    <div id="Div2" class="form-group" runat="server">
                        <label class="col-md-2 control-label "><span style='color: red'>*</span> Contact Email Address : </label>
                        <div class="form-inline">
                            <asp:TextBox ID="txtEmail1" autocomplete="off" runat="server" placeholder="Enter Email Address" ValidationGroup="Email1" class="form-control" type="text" ReadOnly="false" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail1" ID="rfvtxtEmail1" ForeColor="Red" ErrorMessage="Please enter Email Address" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="revtxtEmail1" ControlToValidate="txtEmail1" ValidationGroup="Email1" Display="Dynamic" ErrorMessage="Invalid Email Address" ForeColor="Red" ValidationExpression="^([a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]){1,70}$"></asp:RegularExpressionValidator>

                        </div>
                    </div>
                    <div id="Div3" class="form-group" runat="server">
                        <label class="col-md-2 control-label "><span style='color: red'>*</span> Mobile Number : </label>
                        <div class="form-inline">
                            <asp:TextBox ID="txtPhone1" autocomplete="off" runat="server" ValidationGroup="Phone1" placeholder="Enter Phone Number" MaxLength="15" class="form-control" type="text" ReadOnly="false" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhone1" ID="rfvtxtPhone1" ForeColor="Red" ErrorMessage="Please enter Phone Number" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator runat="server" ID="revtxtPhone1" ControlToValidate="txtPhone1" ValidationGroup="Phone1" Display="Dynamic" ErrorMessage="Only Numbers Allowed." ForeColor="Red" ValidationExpression="^[+]?\d*$"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <hr />
                    <%--<div class="form-group" runat="server">--%>
                    <%--<p><b>Our records shows that you were registered for the qualification(s) below, you will receive the Academic Record for all registered qualification(s) and Transcript Supplement for all the passed modules.</b></p>--%>
                    <%--   </div> --%>
                    <div class="form-group">
                        <label class="col-md-2 control-label " style="text-align: left; width: 1000px"><b>Our records shows that you were registered for the qualification(s) below, you will receive the Academic Record for all registered qualification(s) and Transcript Supplement for all the passed modules.</b></label>
                    </div>
                    <br />
                    <%--Class="table table-bordered table-responsive table-striped mb-none"--%>
                    <asp:GridView ID="gvQualificationShow" runat="server" Class="deliveryGridview table table-bordered table-striped mb-none" PageSize="6" AutoGenerateColumns="False" HeaderStyle-CssClass="control-label" OnPageIndexChanging="gvQualificationShow_PageIndexChanging">
                        <EmptyDataTemplate>
                            <div>
                                No data found.
                            </div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="Qualification Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblQualificationName" runat="server" Text='<%# Eval("QUALIFICATION_NAME")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>

                    <asp:GridView ID="gvQualification" runat="server" Class="deliveryGridview table table-bordered table-striped mb-none" PageSize="6" HeaderStyle-CssClass="control-label" Visible="false">
                        <EmptyDataTemplate>
                            <div>
                                No data found.
                            </div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="Qualification Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblQualificationName" runat="server" Text='<%# Eval("QUALIFICATION_NAME")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Faculty Name" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFacultyName" runat="server" Text='<%# Eval("FACULTY_SCHOOL_NAME")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="From Year" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFromYear" runat="server" Text='<%#Eval("STARTYEAR") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="To Year" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblToYear" runat="server" Text='<%#Eval("ENDYEAR") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currently Residing Faculty" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrentlyResidingFaculty" runat="server" Text='<%#Eval("CurrentlyResidingFaculty") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <hr />


                    <div class="form-group">
                        <label class="col-md-2 control-label " style="text-align: left; width: 1200px"><b>Click on 'Create Order and indicate Delivery method' button to request document(s).</b></label>
                    </div>
                    <div id="Div24" class="form-group" runat="server" visible="true">
                        <div class="form-group">
                            <label class="col-md-2 control-label "><span style='color: red'>*</span> Delivery method : </label>
                            <div class="form-inline" id="Div39" runat="server">
                                <asp:Button ID="btnAddAddress" runat="server" CausesValidation="false" CssClass="btn btn-primary" Text="Create Order and indicate Delivery method" Style="margin-left: 15px;" Width="300px" OnClick="btnAddAddress_Click" />
                                <a href="#" data-toggle="tooltip" title="Calculator will assist you to calculate the estimated total cost payable for your order. Please note that payment is per document type requested."
                                    style="padding-left: 10px"><span style="font-style: italic; font-size: x-small" onclick="return false">To display calculator </span></a><b><a href="Calculate.aspx" target="_blank" rel="noopener noreferrer"><span style="font-style: italic; font-size: x-small">click here</span></a></b>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanelgvAddress" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvAddress" runat="server" AutoGenerateColumns="false" BorderWidth="0px" CssClass="table table-responsive" GridLines="Horizontal" OnRowDeleting="OnRowDeleting" OnRowDataBound="gvAddress_RowDataBound" Style="margin-left: 15px;" ItemStyle-Font-Size="Smaller">
                                <Columns>
                                    <asp:BoundField DataField="ComplexAddress" HeaderText="Complex Address" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="StreetAddress" HeaderText="Street Address" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="Suburb" HeaderText="Suburb" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="City" HeaderText="City" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="Code" HeaderText="Code" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:TemplateField HeaderText="Country" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCountry" DataField="Country" runat="server" Text='<%# Eval("Country") %>' />
                                            <asp:HiddenField runat="server" ID="CountryCode" Value='<%# Eval("CountryCode")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Address" HeaderText="Send to Address" ItemStyle-Width="15%" HtmlEncode="false" />
                                    <asp:TemplateField HeaderText="Delivery Method" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeliveryMethod" DataField="DeliveryMethod" runat="server" Text='<%# Eval("DeliveryMethod") %>' />
                                            <asp:HiddenField runat="server" ID="DeliveryInd" Value='<%# Eval("DeliveryInd")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Academic Record" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <span style='color: white'></span>
                                            <asp:Label ID="lblAcademicRecord" runat="server" Text='<%# Eval("Academic Record") %>' Visible="false" />
                                            <asp:UpdatePanel ID="UpdatePanelNumberofAcademicRecord" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlNumberofAcademicRecord" runat="server" Height="45px" Width="80px" AppendDataBoundItems="true" AutoPostBack="true" SelectedValue='<%# Eval("Academic Record") %>' OnSelectedIndexChanged="ddlNumberofAcademicRecord_SelectedIndexChanged">
                                                        <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlNumberofAcademicRecord" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transcript Supplement" ItemStyle-Width="8%">
                                        <ItemTemplate>
                                            <span style='color: white'></span>
                                            <asp:Label ID="lblTranscriptSupplement" runat="server" Text='<%# Eval("Transcript Supplement") %>' Visible="false" />
                                            <asp:UpdatePanel ID="UpdatePanelNumberofTranscriptSupplement" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlNumberofTranscriptSupplement" runat="server" Height="45px" Width="80px" AppendDataBoundItems="true" AutoPostBack="true" SelectedValue='<%# Eval("Transcript Supplement") %>' OnSelectedIndexChanged="ddlNumberofTranscriptSupplement_SelectedIndexChanged">
                                                        <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>

                                                    <asp:AsyncPostBackTrigger ControlID="ddlNumberofTranscriptSupplement" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Confirmation Letter" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <span style='color: white'></span>
                                            <asp:Label ID="lblConfirmationLetter" runat="server" Text='<%# Eval("Confirmation Letter") %>' Visible="false" />
                                            <asp:UpdatePanel ID="UpdatePanelNumberofConfirmationLetter" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlNumberofConfirmationLetter" runat="server" Height="45px" Width="80px" AppendDataBoundItems="true" AutoPostBack="true" SelectedValue='<%# Eval("Confirmation Letter") %>' OnSelectedIndexChanged="ddlNumberofConfirmationLetter_SelectedIndexChanged">
                                                        <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>

                                                    <asp:AsyncPostBackTrigger ControlID="ddlNumberofConfirmationLetter" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Forms For Official Bodies" ItemStyle-Width="5%" HeaderStyle-Width="15%" ItemStyle-Font-Size="Small">
                                        <HeaderTemplate>
                                            <span style="color: red">*</span> Forms For Official Bodies
                                            <a href="#" data-toggle="tooltip" title="e.g. WES, CORU, CGFNS" style="padding-left: 5px">
                                                <span class="fa fa-question-circle" onclick="return false"></span>
                                            </a>
                                        </HeaderTemplate>

                                        <ItemTemplate>
                                            <asp:Label ID="lblFormsForOfficialBodies" runat="server" Text='<%# Eval("Forms For Official Bodies") %>' Visible="false" />
                                            <asp:DropDownList ID="ddlNumberofFormsForOfficialBodies"
                                                runat="server"
                                                Height="45px"
                                                Width="80px"
                                                AppendDataBoundItems="true"
                                                AutoPostBack="true"
                                                CssClass="forms-dropdown official-forms-ddl"
                                                SelectedValue='<%# Eval("Forms For Official Bodies") %>'
                                                OnSelectedIndexChanged="ddlNumberofFormsForOfficialBodies_SelectedIndexChanged">
                                                <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                            </asp:DropDownList>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnAddAnotherAddress" runat="server" OnClick="lbtnAddAnotherAddress_Click" CausesValidation="false">Add another delivery method</asp:LinkButton>|
                                            <asp:LinkButton ID="lbtnDelete" runat="server" Text="Delete" CommandName="Delete" CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <div id="Div10" runat="server" style="margin-left: 500px" visible="false">
                        <div class="form-inline">
                            <label class="col-md-2 control-label "></label>
                            <p>
                                <b>Total Amount Payable  :
                                <asp:TextBox ID="txtTotalAmount" autocomplete="off" runat="server" MaxLength="30" class="form-control" type="text" ReadOnly="true" Width="150px"></asp:TextBox></b>
                            </p>
                        </div>
                    </div>
                    <!-- Forms for official bodies special instruction validation-->
                    <div>
                    </div>
                    <hr />

                    <table border="0">
                        <tr>
                            <td>
                                <label class="col-md-2 control-label" style="width: 300px"><span style='color: red'>*</span> Payment Method:</label>
                            </td>
                            <td>
                                <asp:DropDownList ID="paymentMethod" runat="server" CssClass="form-control" Style="width: 250px" AutoPostBack="true" OnSelectedIndexChanged="paymentMethod_SelectedIndexChanged">
                                    <asp:ListItem Text="Select Payment Method" Value="" Disabled="true" Selected="true"></asp:ListItem>
                                    <asp:ListItem Text="Online Payment" Value="Online Payment"></asp:ListItem>
                                    <asp:ListItem Text="Bank Deposit" Value="Bank Deposit"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPaymentMethod" runat="server" ControlToValidate="paymentMethod" InitialValue=""
                                    ErrorMessage="Please select a payment method" ForeColor="Red" Display="Dynamic" />

                            </td>
                        </tr>

                        <tr>
                            <td>
                                <label class="col-md-2 control-label" style="width: 300px"><span style='color: red'>*</span> Upload Copy of Id/Passport: <a href="#" data-toggle="tooltip" title="Only PDF and Image uploads are allowed(Max 5 MB)" style="padding-left: 10px"><span class="fa fa-question-circle" onclick="return false"></span></a></label>
                            </td>
                            <td>
                                <input type="text" id="txtIdPassport" runat="server" class="form-control" readonly="readonly" text="" style="width: 250px;" /></td>
                            <td>
                                <asp:FileUpload ID="IdPassport" runat="server" Height="27px" Width="300px" onchange="callCopyIdPassport(this); validateUplod3('#ContentPlaceHolder1_IdPassport','#ContentPlaceHolder1_revIdPassport',this); validateFileName3('#ContentPlaceHolder1_IdPassport','#ContentPlaceHolder1_revtxtIdPassport',this);" Style="color: transparent;"></asp:FileUpload></td>
                            <td style="width: 400px">
                                <asp:RegularExpressionValidator runat="server" ID="revIdPassport" ControlToValidate="IdPassport" ForeColor="Red" ErrorMessage="Only PDF and Image uploads allowed (Max 5 MB)" Display="Dynamic" ValidationExpression="^.*\.(jpg|JPG|jpeg|JPEG|pdf|PDF|png|PNG)$"></asp:RegularExpressionValidator>
                                <asp:Label ID="lblErrorMessage2" runat="server" Style="text-align: left" ForeColor="Red" Width="1200px"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>

                        <tr>
                            <!-- The Asterisk Outside GridView-->
                            <td>
                                <label class="col-md-2 control-label " style="width: 300px">
                                    <span id="asteriskSpecialInstruction" style='color: red; display: none'>*</span>Upload Special Instruction form:
                                    <a href="#" data-toggle="tooltip" title="Only PDF and Image uploads are allowed(Max 5 MB)" style="padding-left: 10px">
                                        <span class="fa fa-question-circle" onclick="return false"></span>
                                    </a>
                                </label>
                            </td>
                            <td>
                                <input type="text" id="txtSpecialInstruction" runat="server" class="form-control" readonly="readonly" text="" style="width: 250px;" />
                            </td>
                            <td>
                                <!-- Error load  control-->
                                <asp:Label ID="lblErrorMessage4"
                                    runat="server"
                                    Style="display: block; text-align: left; margin-bottom: 5px;"
                                    ForeColor="Red" Width="100%"></asp:Label>
                                <!-- File Upload control-->
                                <asp:FileUpload ID="SpecialInstruction"
                                    runat="server"
                                    Height="27px"
                                    Width="300px"
                                    onchange="callSpecialInstruction(this); validateUplod1('#ContentPlaceHolder1_SpecialInstruction','#ContentPlaceHolder1_revSpecialInstruction',this); validateFileName1('#ContentPlaceHolder1_SpecialInstruction','#ContentPlaceHolder1_revtxtSpecialInstruction',this);"
                                    Style="color: transparent;"></asp:FileUpload>
                                <asp:Label ID="Label1" runat="server" ForeColor="Red" />
                                <asp:RequiredFieldValidator
                                    ID="rfvSpecialInstruction"
                                    runat="server"
                                    ControlToValidate="SpecialInstruction"
                                    ErrorMessage="Special Instruction Form is required."
                                    ForeColor="Red"
                                    Display="Dynamic"
                                    ValidationGroup="vgSubmit"
                                    Enabled="false" />
                            </td>
                            <td style="width: 400px">
                                <asp:RegularExpressionValidator runat="server" ID="revSpecialInstruction" ControlToValidate="SpecialInstruction" ForeColor="Red" ErrorMessage="Only PDF and Image uploads allowed (Max 5 MB)" Display="Dynamic" ValidationExpression="^.*\.(jpg|JPG|jpeg|JPEG|pdf|PDF|png|PNG)$"></asp:RegularExpressionValidator>

                            </td>
                        </tr>
                        <tr>
                            <!--Special Instructions DIV TXT -->
                            <td colspan="4">
                                <div class="form-group">
                                    <label class="col-md-2 control-label " style="text-align: left; width: 310px">
                                        <span style='color: white'>*</span>Special Instructions:
                                    </label>
                                    <div class="form-inline">
                                        <asp:TextBox ID="txtCourier1" autocomplete="off" TextMode="MultiLine" runat="server" placeholder="Enter Courier Services Instructions" MaxLength="220" class="form-control" type="text" ReadOnly="false" Width="300px"></asp:TextBox>
                                        <a href="#" data-toggle="tooltip" title="WHAT ARE SPECIAL INSTRUCTIONS? These requests include: Students applying to study or work abroad may be requested to attach additional documents/forms such as WES, CGFNS, NMC, Embassies. These forms can be uploaded under this section together with the instructions. Alumni and non-graduated students who require information regarding practicals that need to be included in the Transcript Supplement are required to indicate this. Requests to submit Academic Records/Transcripts Supplement to a third party should be specified with relevant details. Should you not have any Special Instructions for your order then you may leave this section blank." style="padding-left: 10px">
                                            <span style="font-style: italic; font-size: x-small" onclick="return false">For more information Click here</span>
                                        </a>
                                    </div>
                                </div>
                            </td>
                            <!--Special Instructions DIV TXT-->
                        </tr>


                        <asp:Panel ID="proofOfPaymentRow" runat="server">
                            <tr>
                                <td>
                                    <label class="col-md-2 control-label" style="width: 300px">
                                        <span style='color: red'>*</span> Upload Proof of payment:
                                        <a href="#" data-toggle="tooltip" title="Only PDF and Image uploads are allowed (Max 5 MB)" style="padding-left: 10px">
                                            <span class="fa fa-question-circle" onclick="return false"></span>
                                        </a>
                                    </label>
                                </td>
                                <td>
                                    <input type="text" id="txtPaymentProof" runat="server" class="form-control" readonly="readonly" text="" style="width: 250px;" />
                                </td>
                                <td>
                                    <asp:FileUpload ID="PaymentProof" runat="server" Height="27px" Width="300px" onchange="callPaymentProof(this); validateUplod2('#ContentPlaceHolder1_PaymentProof','#ContentPlaceHolder1_revPaymentProof',this); validateFileName2('#ContentPlaceHolder1_PaymentProof','#ContentPlaceHolder1_revtxtPaymentProof',this);" Style="color: transparent;"></asp:FileUpload>
                                </td>
                                <td style="width: 400px">
                                    <asp:RegularExpressionValidator runat="server" ID="revPaymentProof" ControlToValidate="PaymentProof" ForeColor="Red" ErrorMessage="Only PDF and Image uploads allowed (Max 5 MB)" Display="Dynamic" ValidationExpression="^.*\.(jpg|JPG|jpeg|JPEG|pdf|PDF|png|PNG)$"></asp:RegularExpressionValidator>
                                    <%--<asp:RegularExpressionValidator runat="server" ID="revtxtPaymentProof" ControlToValidate="txtPaymentProof" ForeColor="Red" ErrorMessage="File is already uploaded. Choose a different file." Display="Dynamic"></asp:RegularExpressionValidator>--%>
                                    <asp:Label ID="lblErrorMessage3" runat="server" Style="text-align: left" ForeColor="Red" Width="1200px"></asp:Label>
                                </td>
                            </tr>
                        </asp:Panel>

                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>

                    </table>

                    <%--<br>--%>
                    <!--Buttons on the bottom of Document Order Page -->
                    <hr />
                    <div id="Div14" class="form-inline" runat="server">
                        <label class="col-md-4 control-label"></label>
                        <div class="form-group">
                            <span style='margin-left: 30px;'></span>
                            <span id="btnSaveContainer">
                                <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Submit" OnClientClick="return Validate_main()" OnClick="btnSave_Click" />
                            </span>
                            <span id="btnSaveDraftContainer">
                                <asp:Button ID="btnSaveDraft" runat="server" Text="Save and Continue Later"
                                    CssClass="btn btn-primary" OnClick="btnSaveDraft_Click" CausesValidation="false"/>
                            </span>
                            <span id="btnCancel2Container">
                                <asp:Button ID="btnCancel2" CssClass="btn btn-primary btnCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
                            </span>

                        </div>
                    </div>
                </div>
                <!--Buttons on the bottom of Document Order Page END-->
                 </div>
            </div>
        </div>
        <!--Pop UP models -->
        <asp:UpdatePanel ID="popUpConfirmation" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal fade" id="popUpConfirmationModal" role="dialog" data-backdrop="static" data-keyboard="false">
                    <div class="modal-block modal-block-lg">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 id="ConfirmTitle" runat="server" class="modal-title" visible="false">Confirm Request</h4>
                                <h4 id="AddressTitle" runat="server" class="modal-title" visible="false">New Request</h4>
                            </div>
                            <div id="modalBody" runat="server" class="modal-body">
                                <p>

                                    <div id="messageshow" class="form-group" runat="server" visible="false">
                                        <div class="col-md-8" style="padding-top: 8px">
                                            <div class="input-group">
                                                <asp:Label ID="lblPopUpMessage" runat="server" Style="text-align: right; padding-top: 8px"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="duplicatemessage" class="form-group" runat="server" visible="false">
                                        <div class="col-md-8" style="padding-top: 8px">
                                            <div class="input-group">
                                                Total amount due R  <b>
                                                    <asp:Label ID="lblPopUpDuplicateMessage" runat="server" Style="text-align: right; padding-top: 8px;"> </asp:Label>
                                                </b>. <span style="color: red">Duplicate detected -</span> You submitted a similar request that is being processed. Do you want to continue with this request? If yes, click on the confirm button or click cancel button to edit.
                                                <br></br>
                                                <label class="col-md-2 control-label " style="text-align: left; width: 200px">
                                            </div>
                                        </div>
                                    </div>
                                    <br>
                                    <div id="divAcademicDocumentMulti" runat="server" class="form-group" visible="false">
                                        <div class="form-inline">
                                            <label class="col-md-2 control-label" style="text-align: left; width: 200px"><span style='color: red'>*</span> Delivery Method : </label>
                                            <asp:UpdatePanel runat="server" ID="AcademicDocumentUpdatePanel">
                                                <ContentTemplate>
                                                    <div class="form-inline">
                                                        <div class="form-inline">
                                                            <asp:DropDownList runat="server" Width="300px" AutoPostBack="true" ID="ddlAcademicDocument" OnSelectedIndexChanged="ddlAcademicDocument_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <a href="#" data-toggle="tooltip" title="Click on Delivery Method dropdown to select." style="padding-left: 10px"><span class="fa fa-question-circle"></span></a>
                                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlAcademicDocument" ID="rfvAcademicDocument" InitialValue="0" ForeColor="Red" Style="display: inline; padding-left: 20px;" ErrorMessage="Please select Delivery Type" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <asp:Panel ID="pnlElectronicCopyDestination" runat="server" Visible="false" CssClass="form-inline" Style="margin-top: 10px;">
                                                        <label class="col-md-2 control-label" style="text-align: left; width: 200px">
                                                            <span style='color: red'>*</span> Electronic Copy Destination:
                                                        </label>
                                                        <asp:DropDownList runat="server" ID="ddlElectronicDestination" AutoPostBack="true" OnSelectedIndexChanged="ddlElectronicDestination_SelectedIndexChanged"
                                                            Width="300px">
                                                            <asp:ListItem Text="-- Select --" Value="" />
                                                            <asp:ListItem Text="Local" Value="Local" />
                                                            <asp:ListItem Text="International" Value="International" />
                                                        </asp:DropDownList>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlCountry" runat="server" Visible="false" CssClass="form-inline" Style="margin-top: 10px;">
                                                        <label class="col-md-2 control-label" style="text-align: left; width: 200px">
                                                            <span style='color: red'>*</span> Specify Country:
                                                        </label>
                                                      <%--  <asp:TextBox runat="server" ID="txtCountryDestination" Width="300px" />--%>
                                                        <div class="form-inline">
                                                            <asp:DropDownList runat="server" Width="300px" AutoPostBack="false" ValidationGroup="Country" ID="ddlCountryElectronicInternationalOrder"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCountryElectronicInternationalOrder" InitialValue="0" ValidationGroup="Country" ID="rfvddlCountryElectronicInternationalOrder" ForeColor="Red" ErrorMessage="Please select Country" Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </asp:Panel>

                                                </ContentTemplate>


                                                <Triggers>

                                                    <asp:PostBackTrigger ControlID="btnSave" />
                                                    <asp:PostBackTrigger ControlID="btnConfirm" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                        <br>
                                        <div id="notice" class="form-group" runat="server" visible="false">
                                            <div class="col-md-6" style="padding-top: 8px">
                                                <div class="input-group">
                                                    <asp:Label ID="lblNoticeMessage" runat="server" Style="text-align: right; padding-top: 8px"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <br>
                                    </div>
                                    <div runat="server" id="divAddress" class="form-group" visible="false">
                                        <div id="Div13" class="form-group">
                                            <label class="col-md-1 control-label" style="width: 969px;">Physical Address : <b>(PO Box address will not be accepted (if courier is selected)) </b></label>
                                        </div>
                                        <div id="Div11" class="form-group" runat="server">
                                            <label class="col-md-2 control-label " style="text-align: left; width: 200px"><span style='color: white'>*</span>Complex Address :</label>
                                            <div class="form-inline">
                                                <asp:TextBox ID="txtComplex1" autocomplete="off" runat="server" MaxLength="35" placeholder="Enter Complex Address" class="form-control" type="text" ReadOnly="false" Width="300px"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="Div9" class="form-group" runat="server">
                                            <label class="col-md-2 control-label " style="text-align: left; width: 200px"><span style='color: red'>*</span> Street Address : </label>
                                            <div class="form-inline">
                                                <asp:TextBox ID="txtStreet1" autocomplete="off" runat="server" MaxLength="35" placeholder="Enter Street Address" class="form-control" type="text" ReadOnly="false" Width="300px"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStreet1" ValidationGroup="Street1" ID="rfvtxtStreet1" ForeColor="Red" ErrorMessage="Please enter Street Address" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div id="Div12" class="form-group" runat="server">
                                            <label class="col-md-2 control-label " style="text-align: left; width: 200px"><span style='color: red'>*</span> Suburb : </label>
                                            <div class="form-inline">
                                                <asp:TextBox ID="txtSuburb1" autocomplete="off" runat="server" MaxLength="20" placeholder="Enter Suburb" class="form-control" type="text" ReadOnly="false" Width="300px"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSuburb1" ValidationGroup="Suburb1" ID="rfvtxtSuburb1" ForeColor="Red" ErrorMessage="Please enter Suburb" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div id="Div15" class="form-group" runat="server">
                                            <label class="col-md-2 control-label " style="text-align: left; width: 200px"><span style='color: red'>*</span> City : </label>
                                            <div class="form-inline">
                                                <asp:TextBox ID="txtCity1" autocomplete="off" runat="server" MaxLength="20" placeholder="Enter City" class="form-control" type="text" ReadOnly="false" Width="300px"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCity1" ValidationGroup="City1" ID="rfvtxtCity1" ForeColor="Red" ErrorMessage="Please enter City" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div id="Div16" class="form-group" runat="server" visible="false">
                                            <label class="col-md-2 control-label " style="text-align: left; width: 200px"><span style='color: red'>*</span> Code : </label>
                                            <div class="form-inline">
                                                <asp:TextBox ID="txtCode1" autocomplete="off" runat="server" MaxLength="8" placeholder="Enter Code" ValidationGroup="Code1" class="form-control" type="text" ReadOnly="false" Width="300px"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCode1" ValidationGroup="Code10" ID="rfvtxtCode1" ForeColor="Red" ErrorMessage="Please enter Code" Display="Dynamic"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator runat="server" ID="revtxtCode1" ControlToValidate="txtCode1" ValidationGroup="Code1" Display="Dynamic" ErrorMessage="Only Numbers Allowed." ForeColor="Red" ValidationExpression="^[+]?\d*$"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div id="Div66" class="form-group" runat="server" visible="false">
                                            <label class="col-md-2 control-label " style="text-align: left; width: 200px"><span style='color: red'>*</span> Code : </label>
                                            <div class="form-inline">
                                                <asp:TextBox ID="txtCode2" autocomplete="off" runat="server" MaxLength="8" placeholder="Enter Code" ValidationGroup="Code1" class="form-control" type="text" ReadOnly="false" Width="300px"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCode2" ValidationGroup="Code10" ID="RequiredFieldValidator1" ForeColor="Red" ErrorMessage="Please enter Code" Display="Dynamic"></asp:RequiredFieldValidator>
                                                <%--<asp:RegularExpressionValidator runat="server" ID="revtxtCode1" ControlToValidate="txtCode1" ValidationGroup="Code1" Display="Dynamic" ErrorMessage="Only Numbers Allowed." ForeColor="Red" ValidationExpression="^[+]?\d*$"></asp:RegularExpressionValidator>--%>
                                            </div>
                                        </div>
                                        <div id="Div17" class="form-group" runat="server">
                                            <label class="col-md-2 control-label " style="text-align: left; width: 200px"><span style='color: red'>*</span> Country : </label>
                                            <div class="form-inline">
                                                <asp:DropDownList runat="server" Width="175px" AutoPostBack="false" ValidationGroup="Country" ID="ddlCountry"></asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCountry" InitialValue="0" ValidationGroup="Country" ID="rfvddlCountry" ForeColor="Red" ErrorMessage="Please select Country" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </p>
                            </div>
                            <div class="modal-footer">
                                <asp:Button runat="server" ID="btnConfirm" class="btn btn-primary " UseSubmitBehavior="false" Text="Confirm" CausesValidation="false" OnClick="btnConfirm_Click" Visible="false" />
                                <asp:Button runat="server" ID="btnSave_Add" class="btn btn-primary " UseSubmitBehavior="true" Text="Add" OnClick="btnSave_Add_Click" OnClientClick="return Validate_pop()" Visible="false" Width="150px" />
                                <asp:Button runat="server" ID="btnCancelPopUp" class="btn btn-primary " UseSubmitBehavior="false" Text="Cancel" CausesValidation="false" OnClick="btnCancelPopUp_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="popupModal" runat="server">
            <ContentTemplate>
                <div class="modal fade" id="myModal" role="dialog" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog" style="padding-top: 20px">
                        <div class="modal-content" style="width: 65%">
                            <div class="modal-header">
                                <h4 id="modalHead" runat="server" class="modal-title">Financial Block</h4>
                            </div>
                            <div style="padding: 25px; align-content: center">
                                Kindly note that we are unable to process your request as your Academic Record is currently blocked. This may be due to outstanding fees and/or other matters. Please contact Student Finance at on 011 559 4451/3662 to resolve the block.
                            </div>
                            <div class="modal-footer">
                                <%--<span style="padding: 15px; align-content: center">--%>
                                <%--<asp:Button ID="Button1" runat="server" Text="Confirm" CssClass="btn btn-primary" OnClick="btnConfirm_Click" OnClientClick="myModalShowPopUp();"/>--%>
                                <asp:Button ID="btnRequestClose" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnRequestClose_Click" />
                                <%--</span>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!--Pop UP model for special instructions txt: START -->
        <asp:UpdatePanel ID="popupSpecialInstructionModal" runat="server">
            <ContentTemplate>
                <div class="modal fade" id="mySpecialInstructionModal" role="dialog" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog" style="padding-top: 20px">
                        <div class="modal-content" style="width: 65%">
                            <div class="modal-header">
                                <h4 id="SpecialInstructionModalHead" runat="server" class="modal-title">Special Instruction</h4>
                            </div>
                            <div style="padding: 25px; align-content: center">
                                You have not completed the special instructions section. Do you want to Proceed? If yes, click on the submit button or click cancel button to complete the special instructions section.
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnSpecialInstructionYesPopUp" runat="server" Text="Submit" CssClass="btn btn-primary" OnClientClick="setSpecialInstruction();" OnClick="btnSave_Click" />
                                <asp:Button ID="btnSpecialInstructionCancelPopUp" runat="server" Text="Cancel" CssClass="btn btn-primary" CausesValidation="false" OnClientClick="mySpecialInstructionModalCancelAndClose();" OnClick="btnSpecialInstructionCancelPopUp_Click1" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!--Pop UP model for special instructions txt: END -->


        <!--Pop UP model for popupMandatoryInstructionModal UPLOAD: START -->
        <asp:UpdatePanel ID="popupMandatoryInstructionModal" runat="server">
            <ContentTemplate>
                <div class="modal fade" id="myMandatoryInstructionModal" role="dialog" data-backdrop="static" data-keyboard="false">
                    <div class="modal-dialog" style="padding-top: 20px">
                        <div class="modal-content" style="width: 65%">
                            <div class="modal-header">
                                <h4 class="modal-title">Required Upload</h4>
                            </div>
                            <div class="modal-body" style="padding: 25px">
                                <p>You must upload the <b>Special Instruction form</b>  when ordering <b>Forms for Official Bodies</b>. Please upload the required file before submitting your request.</p>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" onclick="myMandatoryInstructionModalCancelAndClose();">OK</button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="PopupexpiredSessionModal" runat="server">
            <ContentTemplate>
                <div class="modal fade" id="expiredSessionModal" style="display: none" <%--role="dialog" data-backdrop="static" data-keyboard="false"--%>>
                    <div class="modal-dialog" style="padding-top: 20px">
                        <div class="modal-content" style="width: 65%">
                            <div class="modal-header">
                                <h4 id="H1" runat="server" class="modal-title">Expired Session</h4>
                            </div>
                            <div style="padding: 25px; text-align: center">
                                Kindly note that your session has expired. Once the session expires, no data is saved. Please re-login to place your order.
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" id="okayButton" onclick="redirectToDefault()">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


        <asp:UpdatePanel ID="PopUpPaymentConfirmation" runat="server">
            <ContentTemplate>
                <div id="paymentConfirmationModal" class="modal" style="display: none">
                    <div class="modal-dialog" style="padding-top: 20px">
                        <div class="modal-content" style="width: 65%">
                            <div class="modal-header">
                                <h4 id="H2" runat="server" class="modal-title">Confirmation Required for Payment </h4>
                            </div>
                            <div style="padding: 25px; text-align: center">
                                <p id="confirmationMessage"></p>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" id="paymentOkayButton" onclick="closeStatusModal()">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


        <asp:UpdatePanel ID="PopupFailedPayment" runat="server">
            <ContentTemplate>
                <div id="failedPaymentModal" class="modal" style="display: none">
                    <div class="modal-dialog" style="padding-top: 20px">
                        <div class="modal-content" style="width: 65%">
                            <div class="modal-header">
                                <h4 id="H3" runat="server" class="modal-title">Failed Payment</h4>
                            </div>
                            <div style="padding: 25px; text-align: center">
                                <p id="failedPaymentMessage"></p>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" id="btnFailPay" onclick="closeStatusModal();">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>



    </div>

</asp:Content>


