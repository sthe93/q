<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QualificationVerificationWeb._Default" %>

<%@ Import Namespace="QualificationVerificationWeb.Helper" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Content/bootstrap/jquery/jquery.js"></script>
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-timepicker/css/bootstrap-timepicker.min.css")%>" rel="stylesheet" />
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-timepicker/css/bootstrap-timepicker.css")%>" rel="stylesheet" />
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.css")%>" rel="stylesheet" />
    <script src="/Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.js"></script>

    <style>
        .btnFeesGrant {
            background-color: #E7540E !Important;
            color: #fff !Important;
        }

        .btnCancel {
            margin-left: 30px;
        }

        .mycheckbox input[type="checkbox"] {
            margin-right: 5px;
        }a

        .deliveryGridview {
            margin-left: 15px;
            Width: 782px;
        }

        table.deliveryGridview tr td:nth-child(2) {
            width: 10%;
        }

        td, th {
            padding: 4px 5px 4px 4px !important;
        }

        #divAddSecond {
            margin-bottom: 0px;
        }

        .btn.btn-primary[disabled] {
            background-color: gray;
            border-color: gray
        }

        .modal-dialog {
            max-width: 90%;
            width: 95%;
            margin: 50px auto;
            text-align: left;
        }
        /* Add to your existing style section */
#privacyModal .modal-dialog {
    max-width: 90%;
    width: 95%;
    margin: 50px auto;
    text-align: left;


}
/* Add to your existing style section */
.btn-default {
    background-color: #f8f9fa;
    color: #333;
    border: 1px solid #ccc;
}

.btn-default:hover {
    background-color: #e2e6ea;
    border-color: #dae0e5;
}
    </style>
    <script>
        function showPrivacyModal() {
            $("#privacyModal").modal('show');
            return false;
        };

        function closePrivacyModal() {
            $("#privacyModal").removeClass('fade').modal('hide');
            return false;
        };
    </script>
    <script type="text/javascript">  
        function pageLoad(sender, args) {
            $('[data-toggle="tooltip"]').tooltip();


        }


        function validateUplod(fileUploaded, file) {
            $(file).css("display", "none");
            var maxFileSize = 5242880; // 5MB -> 5 * 1024 * 1024
            var fileUpload = $(fileUploaded);
            if (fileUpload.val() == '') {
            }
            else {
                if (fileUpload[0].files[0].size < maxFileSize) {
                }
                else {
                    $(file).css("display", "inline");
                    fileUpload.val("");
                    return false;
                }
            }
        }

        function test() {
            var checked1 = $('#<%= chkAgree.ClientID %>').is(':checked');

            //alert(checked1);

            if (checked1) {
                $('[id$=btnNext]').attr('disabled', false);
                return false;
            }
            else {
                $('[id$=btnNext]').attr('disabled', true);
                return true;
            }
        }

        function thirdparty() {
            var checked2 = $('#<%= chkthirdparty.ClientID %>').is(':checked');



            if (checked2) {

                //alert(checked2);
                $('[id$=chkAgree]').attr('disabled', false);
                $('[id$=chkujStudent]').attr('disabled', true);
                return false;
            }
            else {
                $('[id$=chkAgree]').attr('disabled', true);
                $('[id$=chkAgree]').attr('checked', false);
                $('[id$=btnNext]').attr('disabled', true);
                $('[id$=chkujStudent]').attr('disabled', false);
                return true;
            }
        }

        function ujStudent() {
            var checked3 = $('#<%= chkujStudent.ClientID %>').is(':checked');



            if (checked3) {
                //alert(checked3);
                $('[id$=chkAgree]').attr('disabled', false);
                $('[id$=chkthirdparty]').attr('disabled', true)
                return false;
            }
            else {
                $('[id$=chkAgree]').attr('disabled', true);
                $('[id$=chkAgree]').attr('checked', false);
                $('[id$=btnNext]').attr('disabled', true);
                $('[id$=chkthirdparty]').attr('disabled', false)
                return true;
            }
        }

        function IECheckbox() {
            if (ifmDefault.readyState != 'complete') {
                $('[id$=chkAgree]').attr('disabled', true);
            }
        }
        function OtherCheckbox() {
            $('[id$=chkAgree]').attr('disabled', true);
        }
    </script>

    <script type="text/javascript">
        function myModalShowPopUp() {
            $("#myModal").modal('show');
            return false;

        };

    </script>
    <a href="SqlServerTypes/">SqlServerTypes/</a>
    <script type="text/javascript">

        function myModalCancelAndClose() {
            $("#myModal").removeClass('fade').modal('hide');
            return false;
        };
    </script>
    <script type="text/javascript">

        $(document).ready(function () {

            $('[id*=lstAcademicDocument]').multiselect({
                includeSelectAllOption: false
            });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            $('[id*=lstDocumentType]').multiselect({
                includeSelectAllOption: false
            });
        });
    </script>

    <script type="text/javascript">
        // Show the modal
        function showDefaultModal() {
            $('#Default').modal('show');
        }

        // Hide the modal
        function closeDefaultModal() {
            $('#Default').modal('hide');
        }
    </script>

    <script type="text/javascript">
        function myModalShowPopUp() {
            $("#Default").modal('show');
            return false;
        }
    </script>



    <div class="tab-content">
        <asp:UpdatePanel ID="pnlCreateOrder" runat="server" UpdateMode="Always">
            <ContentTemplate>

                <div class="panel-body">
                    <div id="AppliedFeedback" runat="server" style="text-align: start;" class="row">
                        <br />
                        <br />
                        <br />
                        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Style="text-align: left" Text="" Width="700px"></asp:Label>

                        <div style="text-align: left">
                            <div id="importantNoticeDiv" runat="server" style="border: 2px solid red; padding: 15px; background-color: rgba(231, 84, 14, 0.2);">
                                <!-- The content will be dynamically populated from the code-behind -->
                            </div>

                            <p>
                                <table id="importantNoticeTable" runat="server">
                                    <tr>
                                        <td>
                                            <label></label>
                                        </td>
                                    </tr>
                                </table>
                            </p>
                            <br />
                            <p>&nbsp; Thank you for using the Corporate Governance Academic Document Order Platform. This platform offers you the convenience to submit an order online for the following documents: </p>


                            <br />
                            <p>
                                <table border="0">
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">• </td>
                                        <td>
                                            <label><b>Academic Transcript/Record</b> (Document containing qualifications registered/obtained and modules registered reflecting overall outcomes/marks obtained) – * Only.<%-- – * Confirmation letter included--%> </label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">• </td>
                                        <td>
                                            <label><b>Transcript Supplement</b> (Curriculum Supplement of all modules completed successfully per qualification) – * Academic Record included. </label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">• </td>
                                        <td>
                                            <label><b>Confirmation/Special Letters</b> – * Only.</label>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">• </td>
                                        <td>
                                            <label><b>Forms</b> – * Any form that needs to be completed for an official body (e.g., WES, CORU, CGFNS). – * Ensure that you upload the form when placing your order. </label>
                                        </td>

                                    </tr>
                                </table>
                            </p>
                            <br />
                            <p>&nbsp; Before you proceed, please ensure you have the following compulsory documents at hand, which will be required to successfully process your order: </p>

                            <p>
                                <table border="0">
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">• </td>
                                        <td>
                                            <label><b>Copy of ID/Passport</b> (In terms of POPIA and PAIA, the University is required to verify your credentials before academic information can be ordered and released).</label></td>

                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">• </td>
                                        <td>
                                            <label><b>Proof of payment</b> (For ease of reference, we have included a calculator to assist you in establishing the required amount payable for the documents you wish to order). The proof of payment needs to reflect the correct amount and the QV bank details clearly. </label>
                                        </td>

                                    </tr>

                                </table>
                            </p>
                            <br />
                            <p>&nbsp; <b>Follow the necessary steps below to place an order:</b> </p>
                            <br />
                            <p><u>Alumni/Non-registered</u></p>
                            <%-- <br />--%>
                            <p>
                                <table border="0">
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">1. </td>
                                        <td>
                                            <label>Click on <b>Order Here</b></label></td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">2. </td>
                                        <td>
                                            <label>Once you have read the terms and conditions scroll down to confirm acceptance by ticking the relevant boxes and then clicking <b>next</b>. </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">3. </td>
                                        <td>
                                            <label>Login using ID/Passport number. </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">4. </td>
                                        <td>
                                            <label>Capture contact details for email and mobile number. </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">5. </td>
                                        <td>
                                            <label>Click on <b>‘Create order and include Delivery method’</b> button and choose the document(s) (Academic Record/Transcript Supplement/Letter) you want to order. </label>
                                            <br />
                                            <br />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="vertical-align: top; text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;• </td>
                                        <td>
                                            <label><b>Electronic Option</b> – This means that you are only requesting an electronic copy of the ordered document (s) to be emailed to you. </label>

                                        </td>

                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;• </td>
                                        <td>
                                            <label><b>Collect Option</b> – This means that an appointment will be allocated to you or your designated courier to collect the hardcopies of your ordered document (s) once they are in readiness. </label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td style="vertical-align: top; text-align: left"></td>
                                        <td>

                                            <b>N.B The Qualification Verification Unit no longer offers courier services.</b>
                                            <br />
                                            <p>
                                                Details of a provider is indicated below that you can contact or you can arrange with a courier of
                                                        your choice to assist with the collection of your documents from our office. Any third party
                                                        collecting on your behalf must produce written consent from yourself requesting QV to release
                                                        your document.
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left"></td>
                                        <td>&nbsp;<b>Courier Company Name: RAM</b>
                                            <br />
                                            &nbsp;<b>Website: <a rel="noopener noreferrer" href="https://www.ram.co.za/" target="_blank">https://www.ram.co.za/ </a></b>
                                            <br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">6. </td>
                                        <td>
                                            <label>Attach a copy of alumnus/previous student <b>ID/Passport</b> </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">7. </td>
                                        <td>
                                            <label>Attach a copy of your <b>Proof of payment</b> </label>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td style="vertical-align: top; text-align: left">8. </td>
                                        <td>
                                            <label>Attach the written consent of the alumnus/previous student as per the template provided <b>(Only applicable if you are a third party requesting information)</b>. </label>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">8. </td>
                                        <td>
                                            <label>Capture any required information under <b>‘Special instructions’</b> or indicate N/A if there are no special instructions. Attach any other <b>special documents</b> such as forms required by international institutions (e.g. CGFNS, NNAS, WES etc.) </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">9. </td>
                                        <td>
                                            <label>Click <b>submit</b></label></td>
                                    </tr>
                                </table>
                            </p>
                            <br />
                            <b>
                                <p>
                                    Please note that you will receive a system-triggered email notification with a unique reference number once your order has been successfully submitted. If this has not been received, it means that your order was not successfully lodged on HIVE
                            and you will be required to re-apply. A second system-generated email notification will be sent once your payment has been vetted. This email is a confirmation that your order has been approved for processing. Should there be an issue with your application an email will be sent to you in this regard.
                                </p>
                            </b>
                            <br />
                            <b>
                                <p>
                                    Please note that payment can either be made via Bank Deposit (see banking details below) or via Online Payment by entering your card details. Should you wish to utilise the Bank Deposit option, you will be required to make the payment using
                                the banking details below and attach the Proof of Payment when placing your order on the Platform. Should you wish to utilise the Online Payment method, you may follow the prompts to enter your card details when placing your order on the Platform.
                                </p>
                            </b>
                            <br />
                            &nbsp;&nbsp;<b>Banking Details for Electronic Payments:</b>
                            <br />
                            &nbsp;&nbsp;Bank : <b>
                                <asp:Label ID="Bank" Style="text-align: center" runat="server" Text=""></asp:Label></b>
                            <br />
                            &nbsp;&nbsp;Account Name : <b>
                                <asp:Label ID="AccountName" Style="text-align: center" runat="server" Text=""></asp:Label></b>
                            <br />
                            &nbsp;&nbsp;Account Number : <b>
                                <asp:Label ID="AccountNumber" Style="text-align: center" runat="server" Text=""></asp:Label></b>
                            <br />
                            &nbsp;&nbsp;Branch Code : <b>
                                <asp:Label ID="BranchCode" Style="text-align: center" runat="server" Text=""></asp:Label></b>
                            <br />
                            &nbsp;&nbsp;Swift Code : <b>
                                <asp:Label ID="Branch" Style="text-align: center" runat="server" Text=""></asp:Label></b>&nbsp;
                            <br>
                            <label>&nbsp;&nbsp;Reference : <b>161250.20.12270</b></label>
                            <br />

                            <p>

                                <p>&nbsp;&nbsp;&nbsp;&nbsp;<b>Document Type Processing Times and Costing </b></p>


                                <asp:GridView CellSpacing="2" CssClass="deliveryGridview" ID="gvDocumentType_Price" runat="server" AutoGenerateColumns="False" Class="table table-bordered table-striped table-responsive mb-none">
                                    <Columns>
                                        <asp:BoundField DataField="DocumentType" HeaderText="Document Type" ReadOnly="True" SortExpression="DocumentType" />
                                        <asp:BoundField DataField="Collect" HeaderText="Cost" ReadOnly="True" SortExpression="Collect" />
                                        <asp:BoundField DataField="Courier_South_Africa" HeaderText="Courier South-Africa" ReadOnly="True" SortExpression="Courier_South_Africa" Visible="false" />
                                        <asp:BoundField DataField="Courier_International" HeaderText="Courier International" ReadOnly="True" SortExpression="Courier_International" Visible="false" />
                                        <asp:BoundField DataField="TurnAroundTime" HeaderText="Turn Around" ReadOnly="True" SortExpression="TurnAroundTime" />
                                    </Columns>
                                </asp:GridView>


                            </p>
                            <br />


                            <p>&nbsp;Please <b><a href="Calculate.aspx" target="_blank" rel="noopener noreferrer">click here</a></b> to use payment calculator that will assist you to calculate the estimated total cost payable - <b>Please note that payment is per document type requested. Each document type outlines your entire academic journey at UJ.</b></p>
                            <br />
                            <p>&nbsp; <b>N.B</b> Kindly note that you will be notified via the email you provided on the order platform when your document is in readiness for collection. Walk-ins will not be assisted as collection of a document is by appointment <b>only</b>.</p>
                            <br />


                            <p>&nbsp; <b>Terms and Conditions for Refund Requests </b></p>
                            <br />
                            <p><u>The following Terms and Conditions are applicable when considering refund requests:</u></p>

                            <p>
                                <table border="0">
                                        <tr>
                                        <td style="vertical-align: top; text-align: left">1. </td>
                                        <td>
                                            <label>Refund requests relating to Academic Transcripts and Confirmation Letters will only be considered if the request for a refund is submitted by the client on the same day that the order is placed. Where a refund request is submitted after one working day, the request may be declined, as the necessary administrative processes would have commenced in order to meet the applicable turnaround times.</label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">2. </td>
                                        <td>
                                            <label>Refund requests relating to Transcript Supplements will only be considered if the request for a refund is submitted by the client within 48 hours of placing the order. Where a refund request is submitted after 48 hours, the request may be declined, as the necessary administrative processes would have commenced in order to meet the applicable turnaround times.</label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">3. </td>
                                        <td>
                                            <label>In respect of historical academic records, where the relevant Faculty is unable to provide an Academic Transcript or Transcript Supplement and a Confirmation Letter is issued instead, the applicable Confirmation Letter fee of R100.00 will be deducted from the amount paid, and the remaining balance will be refunded to the affected client.</label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">4. </td>
                                        <td>
                                            <label>Clients are requested to ensure that their order is successfully submitted on the Online Ordering Platform within 7 (seven) days of making payment. Refund requests where payment has been made without a corresponding order being submitted on the Online Ordering Platform will be considered on a case-by-case basis, taking into account the period that has elapsed since payment was made.</label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">5. </td>
                                        <td>
                                            <label>Refund requests relating to orders that have not resulted in the issuing of the requested document(s) must be submitted within 3 (three) months from the date of the transaction and within the same financial year in which the transaction occurred. Requests for a refund submitted after the stipulated period may not be considered.</label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">6. </td>
                                        <td>
                                            <label>Kindly note that all approved refunds will be processed directly to the client’s UJ student account.</label>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                 <p>&nbsp;&nbsp; For enquiries related to Academic Records (Transcripts)/ Transcript Supplements (Curriculum Supplements), Confirmation Letters, and Forms for Official Bodies, please email: <a href="mailto:transcripts@uj.ac.za">transcripts@uj.ac.za</a></p>
                                <br />
                                <p>&nbsp;&nbsp; <b>Please ensure that you have read through the Terms and Conditions for Refunds prior to placing your order for Academic Documentation.</b></p>
                                <%--  </div>--%>

                                <div id="Div14" class="form-inline" runat="server">
                                    <br />
                                    <%--  <div class="form-group">--%>
                                    <label class="col-md-1 control-label "></label>
                                    <span style="padding: 260px; height: 20px; align-content: center">
                                        <asp:Button ID="btnTnC" class="btn btn-primary" runat="server" Text="Order Here" CausesValidation="false" OnClick="btnTnC_Click" Width="153px" OnClientClick="myModalShowPopUp();" />
                                    </span>
                                    <%-- </div>--%>
                                </div>
                        </div>
                    </div>

                    <div runat="server" id="divTermsandConditionInfo">
                        <asp:UpdatePanel ID="popupModal" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                            <ContentTemplate>
                                <div class="modal fade" id="myModal" role="dialog" data-backdrop="static" data-keyboard="false">
                                    <div class="modal-dialog" style="padding-top: 20px;width: 1250px;">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h4 id="modalHead" runat="server" class="modal-title">Terms and Conditions</h4>
                                            </div>
                                            <div>
                                                <iframe id="ifmDefault" src="Terms1.pdf" style="width: 100%; height: 400px; margin: 0px; border: 15px; overflow: scroll;" onreadystatechange="IECheckbox();" onload="OtherCheckbox();"></iframe>
                                                <label class="col-md-2 control-label " style="height: 45px"></label>
                                                <table border="0" style="margin-right:140px;">
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="vertical-align: top; text-align: left"><span onchange="ujStudent()">
                                                            <asp:CheckBox ID="chkujStudent" runat="server" CausesValidation="false" CssClass="mycheckbox" Text="" /></span></td>
                                                        <td>
                                                            <label>I confirm that I am a currently registered student/ previously registered student at the University of Johannesburg (including RAU and TWR) requesting my academic information.</label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td style="text-align: center">
                                                            <label><b>OR </b></label>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td style="vertical-align: top; text-align: left"><span onchange="thirdparty()">
                                                            <asp:CheckBox ID="chkthirdparty" runat="server" CausesValidation="false" CssClass="mycheckbox" Text="" /></span></td>
                                                        <td>
                                                            <label>I confirm that I am a third party and have received consent by the currently registered/previously registered student at the University of Johannesburg (including RAU/TWR) to request his/her academic information on his/her behalf which I will use in a lawful manner. I confirm that I will not share or sell the personal information obtained through this platform with any other party.</label>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td></td>

                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <label>UJ will take all necessary steps to safeguard the personal and academic information of currently/previously registered students. The currently/previously registered student fully indemnifies UJ against any loss or damages suffered as a result of sharing their information. </label>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td style="text-align: center">
                                                            <label><b>AND</b></label></td>

                                                    </tr>
                                                    <tr>
                                                        <td style="vertical-align: top; text-align: left"><span onchange="test()">
                                                            <asp:CheckBox ID="chkAgree" runat="server" CausesValidation="false" CssClass="mycheckbox" Text="" /></span></td>
                                                        <td>
                                                            <label><b>By clicking on the two relevant tick boxes on this page you are accepting the terms and conditions.</b></label></td>

                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td></td>

                                                    </tr>
                                                </table>

                                                <%--<span style="padding: 25px; align-content: center" onchange="test()">
                                                    <asp:CheckBox ID="chkAgree" runat="server" CssClass="mycheckbox" Text="I have read and understood the terms and conditions." />
                                                </span>--%>
                                                <br />
                                                <div class="modal-footer">
                                                    <span style="padding: 50px; height: 20px; align-content: center">
                                                        <asp:Button ID="btnNext" runat="server" Text="Next" CssClass=" btn btn-primary" Enabled="false" OnClick="btnNext_Click" />
                                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass=" btn btn-primary" CausesValidation="false" OnClick="btnCancel_Click" OnClientClick="myModalCancelAndClose();" />
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <!-- Add this modal right after the Terms and Conditions modal -->
<asp:UpdatePanel ID="popupPrivacyModal" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
    <ContentTemplate>
       <div class="modal fade" id="privacyModal" role="dialog" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog" style="padding-top: 20px;width: 1250px;">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">University of Johannesburg Privacy Notice</h4>
            </div>
            <div>
                <iframe id="privacyIframe" src="PrivacyNotice.pdf" style="width: 100%; height: 400px; margin: 0px; border: 15px; overflow: scroll;"></iframe>
                <div style="padding: 50px;">
                    <p>By continuing, you acknowledge that you have read and understood the University of Johannesburg's Privacy Notice.</p>
                    <p><b>If you decline, you will be returned to the Terms and Conditions.</b></p>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnPrivacyAccept" runat="server" Text="Accept" CssClass="btn btn-primary" OnClick="btnPrivacyAccept_Click" />
                <asp:Button ID="btnPrivacyDecline" runat="server" Text="Decline" CssClass="btn btn-default" OnClick="btnPrivacyDecline_Click" />
            </div>
        </div>
    </div>
</div>
    </ContentTemplate>
</asp:UpdatePanel>
                    <asp:UpdatePanel ID="popupSpecialMessageModal" runat="server">
                        <ContentTemplate>
                            <div class="modal fade" id="mySpecialMessageModal" role="dialog" data-backdrop="static" data-keyboard="false">
                                <div class="modal-dialog" style="padding-top: 20px; height: 620px">
                                    <div class="modal-content" style="width: 120%">
                                        <div class="modal-header">
                                            <%--<a href="BusinessLogicLogFile.txt">BusinessLogicLogFile.txt</a>--%>
                                            <h4 id="SpecialMessageModalHead" runat="server" class="modal-title">Attention</h4>
                                        </div>
                                        <div style="padding: 10px; margin-left: 50px; align-content: center">
                                            <p>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <h3 style="color: #FF6400; font-weight: bold">DEADLINES TO ORDER TRANSCRIPT SUPPLEMENTS & ACADEMIC RECORDS</h3>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                    <%-- <tr >
                                             <td ><label>During the lockdown period students who are  <b style="text-decoration:underline"> due to graduate in 2020</b> request their Academic Record by sending an email to <a href="mailto:transcripts@uj.ac.za">transcripts@uj.ac.za</a> using the format indicated below. </label></td>
                                        </tr>--%>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <strong>Transcript Supplement</strong> : Deadline to place an order is <b style="color: #FF6400;">31<sup>st</sup> October 2020</b>. 
                                                 Any orders received after the stipulated date will only be processed and routed to the relevant document creators within January 2021. The Unit will endeavour to process your order as timeously as possible.</label></td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <strong>Academic Record</strong> : Deadline to place an order is <b style="color: #FF6400;">20<sup>th</sup> November 2020</b>. 
                                                 Any orders received after the stipulated date will only be processed within January 2021. The Unit will endeavour to process your order as timeously as possible.</label></td>
                                                    </tr>

                                                    <tr>
                                                        <td>
                                                            <label>Please note that the University officially closes on the 11<sup>th</sup> December 2020 and re-opens 04<sup>th</sup> January 2021</label></td>
                                                    </tr>

                                                </table>
                                            </p>
                                            <br />
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="btnRequestClose" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnRequestClose_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
