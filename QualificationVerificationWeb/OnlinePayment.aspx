<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnlinePayment.aspx.cs" Inherits="QualificationVerificationWeb.ResponsePage" MasterPageFile="~/Master.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Content/bootstrap/jquery/jquery.js"></script>
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-timepicker/css/bootstrap-timepicker.min.css")%>" rel="stylesheet" />
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-timepicker/css/bootstrap-timepicker.css")%>" rel="stylesheet" />
    <link href="<%= ResolveUrl("/Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.css")%>" rel="stylesheet" />
    <script src="Content/bootstrap/bootstrap-multiselect/bootstrap-multiselect.js"></script>
    <script src="Scripts/Common/Common.js"></script>

       <style>
        .hrefUJholidays {
            color:black;
        }
        </style>

        <script type="text/javascript">

                function ShowTransactionStatusModal(message, tryAgainNum)
                {
                    //alert(message);

                    if (message === "Payment Successful")
                    {
                        document.getElementById('statusMessageSuccess').innerText = message;
                        document.getElementById('successfullTransactionStatusModal').style.display = 'block';

                    }
                    else
                    {
                            if (tryAgainNum === "3")
                            {
                                document.getElementById('blockTryOption').style.display = 'block';
                                
                            }

                        document.getElementById('statusMessageUnsuccessfull').innerText = message;
                        document.getElementById('unSuccessfullTransactionStatusModal').style.display = 'block';

                       
                    }
                   
                }

                function closeStatusTransSuccessModal()
                {
                        document.getElementById('successfullTransactionStatusModal').style.display = 'none';

                        window.location.href = "Submitted.aspx";
                }


                function ShowFailedPaymentStatusModal(message) {

                    document.getElementById('failedPaymentMessage').innerText = message;
                    document.getElementById('failedPaymentModal').style.display = 'block';


                }

                function closeStatusModal() {

                    document.getElementById('failedPaymentModal').style.display = 'none';

                    window.location.href = "Default.aspx";
                }

                function processPayment(payGateId, payRequestId, reference, checksum, postData)
                {
                    localStorage.setItem("PAYGATE_ID", payGateId);
                    localStorage.setItem("PAY_REQUEST_ID", payRequestId);
                    localStorage.setItem("REFERENCE", reference);
                    localStorage.setItem("CHECKSUM", checksum);

                    window.location.href = config.serverPath + "PagGateRedirect.aspx";
                    //window.open(config.serverPath + "PagGateRedirect.aspx", "_blank");
                }

               
           
            function redirectToDefault()
            {
             window.location.href = "Default.aspx";
            }
      
        </script>


    <div class="tab-content">
           
         <asp:UpdatePanel ID="PopupUnSuccessfullTransactionStatus" runat="server">
            <ContentTemplate>
                <div id="unSuccessfullTransactionStatusModal" class="modal" style="display: none">
                    <div class="modal-dialog" style="padding-top: 20px">
                        <div class="modal-content" style="width: 65%">
                            <div class="modal-header">
                                <h4 id="H4" runat="server" class="modal-title">Transaction Status</h4>
                            </div>
                            <div style="padding: 25px; text-align: center">
                                <!-- Logo for unsuccessful payment -->
                                <img src="Content/images/PaymentUnsuccess.gif" alt="Unsuccessful Payment Logo" style="max-width: 350px; margin-bottom: 15px; background-color:none;">
                             
                                <br />
                                <b><p id="statusMessageUnsuccessfull" style="font-weight:bold;font-size:25px;"></p></b>
                                <br />
                                <b><p id="blockTryOption" style="display:none;">We are unable to process your payment. Kindly contact <a href="mailto:transcripts@uj.ac.za">transcripts@uj.ac.za</a> for further information.</p></b>
                            </div>
                            <div class="modal-footer">
                              <div class="row">
                                    <div class="col-md-6">
                                       </div>
                                    <div class="col-md-6">
                                         <asp:Button ID="btntryAgain" CssClass="btn btn-primary" runat="server" UseSubmitBehavior="false" OnClick="btnTryAgain_Click" Text="Try Again"/>
        
                                        <asp:Button runat="server" id="btnbackHomeButton" class="btn btn-primary" UseSubmitBehavior="false" Text="Back Home" CausesValidation="false" OnClick="btnCancelPopUp_Click" />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
               </div>
           </ContentTemplate>
        </asp:UpdatePanel>


        <asp:UpdatePanel ID="PopupSuccessfullTransactionStatus" runat="server">
            <ContentTemplate>
                  <div id="successfullTransactionStatusModal" class="modal" style="display: none">
                    <div class="modal-dialog" style="padding-top: 20px">
                        <div class="modal-content" style="width: 65%">
                            <div class="modal-header">
                                <h4 id="H5" runat="server" class="modal-title">Transaction Status</h4>
                            </div>
                            <div style="padding: 25px; text-align: center">
                                <!-- Logo for successful payment -->
                                <img src="Content/images/SuccessfulPayment_Gif.gif" alt="Successful Payment Logo" style="max-width: 350px; margin-bottom: 15px; background-color:none;">
                                <br />
                                <b><p id="statusMessageSuccess" style="font-weight:bold;font-size:25px;"></p></b>
                            </div>
                            <div class="modal-footer">
                                
                                <!-- Button for going back home -->
                                <button type="button" class="btn btn-primary" id="backHomeButtonSuccess" onclick="closeStatusTransSuccessModal()">Back to home</button>
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
                                 <p id="failedPaymentMessage"></p>  </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-primary" id="btnFailPay" onclick="closeStatusModal();">Close</button>
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

    </div>

</asp:Content>


