using QualificationVerificationWeb.Helper;
using QualificationVerificationWeb.Helper.PayGate;
using QualificationVerificationWeb.ViewModels;
//using BusinessLogic.Services;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;

namespace QualificationVerificationWeb
{

    public partial class ResponsePage : System.Web.UI.Page
    {
        private readonly string qualificationVerificationAPI = ConfigurationManager.AppSettings["QualificationVerificationAPI"].ToString();


        HttpContext context = HttpContext.Current;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.HttpMethod == "POST")
            {
                Logs.WriteErrorLog("Request.HttpMethod  Page_Load:");


                PaygatePaymentStep();

            }

        }
        protected void btnTryAgain_Click(object sender, EventArgs e)
        {
            Logs.WriteErrorLog("Request.HttpMethod  btnTryAgain_Click:");


            var checkTry = int.Parse(Session["TryAgain"].ToString());

            if (checkTry <= 2)
            {
                checkTry++;

                Session["TryAgain"] = checkTry.ToString();

                Logs.WriteErrorLog("Get  Session[TryAgain]  for try again :" + Session["TryAgain"].ToString());

                bool paymentResult = PayGateOnline();

            }
            else
            {
                btntryAgain.Visible = false;
            }


        }


        private void PaygatePaymentStep()
        {
            try
            {
                

                string requestBody;
                using (Stream body = Request.InputStream)
                {
                    using (StreamReader reader = new StreamReader(body, Request.ContentEncoding))
                    {
                        requestBody = reader.ReadToEnd();
                    }
                }


                Logs.WriteErrorLog("requestBody :" + requestBody);

                string payRequestId = GetRequestParamValue(requestBody, "PAY_REQUEST_ID");
                string transactionStatus = GetRequestParamValue(requestBody, "TRANSACTION_STATUS");
                //string transactionCode = GetRequestParamValue(requestBody, "RESULT_CODE");

                Logs.WriteErrorLog("Show payRequestId :" + payRequestId);
                Logs.WriteErrorLog("Show transactionStatus :" + transactionStatus);
                //Logs.WriteErrorLog("Show transactionCode :" + transactionCode);


                string url1 = qualificationVerificationAPI + "Transaction/GetTransactionByPayRequestId?payRequestId=" + payRequestId;
                var transaction = BusinessRules.HttpHelper.HttpCallJson<Transaction>(url1, WebRequestMethods.Http.Get, null);


                var queryService = new PayGateService
                {
                    PayGateId = Settings.GetPayGateId,
                    PayGateQueryUrl = Settings.GetPayGateQueryUrl,
                    PayRequestId = payRequestId,
                    Reference = transaction.REFERENCE,
                    EncryptionKey = Settings.GetEncryptionKey
                };

                LogValues("PayGateQueryUrl", queryService.PayGateQueryUrl);
                LogValues("PayRequestId", queryService.PayRequestId);
                LogValues("Reference", queryService.Reference);

                var queryResult = queryService.QueryPayment();


                LogValues("queryResult.Message", queryResult.Message);

                string url14 = qualificationVerificationAPI + "OnlinePaymentSession/GetOnlinePaymentSession?studentId=" + queryService.Reference;
                var getOnlinePaymentSession = BusinessRules.HttpHelper.HttpCallJson<OnlinePaymentSession>(url14, WebRequestMethods.Http.Get, null);

                Logs.WriteErrorLog(" getOnlinePaymentSession  after select: " + getOnlinePaymentSession.StudentID.ToString());

                Session["StudentID"] = getOnlinePaymentSession.StudentID.ToString();
                Session["txtTotalAmount"] = getOnlinePaymentSession.TotalAmount.ToString();
                Session["TryAgain"] = getOnlinePaymentSession.TryAgain.ToString();
                Session["StudentJwtToken"] = getOnlinePaymentSession.Token.ToString();

                Logs.WriteErrorLog(" Start: Session[TryAgain]: " + Session["TryAgain"].ToString());
                Logs.WriteErrorLog(" Start: Session[StudentID]: " + Session["StudentID"].ToString());
                Logs.WriteErrorLog(" Start: Session[txtTotalAmount]: " + Session["txtTotalAmount"].ToString());


                if (Session["StudentID"] == null || Session["txtTotalAmount"] == null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "expiredSession", "$('#expiredSessionModal').modal('show');", true);
                    return;
                }


                int studentIdInt = int.Parse(transaction.REFERENCE);

                var token = HttpContext.Current?.Session["StudentJwtToken"] as string;

                string url13 = qualificationVerificationAPI + "Student/GetStudent?studentId=" + studentIdInt;
                var student = BusinessRules.HttpHelper.HttpCallJson<Student>(url13, WebRequestMethods.Http.Get, token);


                if (transactionStatus != "1")
                {
                    student.StudentStatus = 8;

                    string url = qualificationVerificationAPI + "Student/UpdateStudent";
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(student);
                    BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url, WebRequestMethods.Http.Post, JSONString, token);


                    Logs.WriteErrorLog("Update student table status option one : " + student.StudentStatus.ToString());
                }
                else
                {
                    student.StudentStatus = 0;

                    string url = qualificationVerificationAPI + "Student/UpdateStudent";
                    string JSONString = string.Empty;
                    JSONString = JsonConvert.SerializeObject(student);
                    BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url, WebRequestMethods.Http.Post, JSONString, token);


                    string url12 = qualificationVerificationAPI + "Student/SendEmail?studentID=" + student.StudentID + "&emailType=Acknowledgement";

                    // string url12 = qualificationVerificationAPI + "Student/SendEmail?studentID=" + student.StudentID;

                    BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url12, WebRequestMethods.Http.Get, token);

                    Logs.WriteErrorLog("Update student table status option two : " + student.StudentStatus.ToString());
                }


                string tryAgainValue = "";

                if (Session["TryAgain"] != null)
                {

                    tryAgainValue = Session["TryAgain"].ToString();

                    Logs.WriteErrorLog("Show tryAgainValue :" + tryAgainValue);
                }
                else
                {
                    Session["TryAgain"] = "1";

                    tryAgainValue = Session["TryAgain"].ToString();
                }



                // Prepare the JavaScript call
                string script = $"ShowTransactionStatusModal('{TransactionStatus(transactionStatus)}', '{tryAgainValue}');";

                // Log for debugging
                Logs.WriteErrorLog("Final popup call: " + script);
                Logs.WriteErrorLog("Show transactionStatus :" + transactionStatus);
                Logs.WriteErrorLog("TransactionStatus() returns :" + TransactionStatus(transactionStatus));

                // Register the startup script once
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowStatusModal", script, true);

                if (Session["TryAgain"].ToString() == "3")
                {
                    btntryAgain.Visible = false;
                }

            }
            catch (Exception ex)
            { Logs.WriteErrorLog("An error occurred in PaygatePaymentStep(): " + ex.Message); }

        }


        protected void btnCancelPopUp_Click(object sender, EventArgs e)
        {
            Logs.WriteErrorLog("Request.HttpMethod  btnCancelPopUp_Click:");

            ScriptManager.RegisterStartupScript(sender as Control, this.GetType(), "failPopup", "$('#unSuccessfullTransactionStatusModal').removeClass('fade').modal('hide');", true);

            Session.Remove("TryAgain");

            CookiesClearOnDone.ClearCookies(HttpContext.Current.Response);

            Response.Redirect("~/Default.aspx");
        }

        private string GetRequestParamValue(string requestBody, string paramName)
        {
            string paramPrefix = paramName + "=";
            int startIndex = requestBody.IndexOf(paramPrefix);
            if (startIndex >= 0)
            {
                int endIndex = requestBody.IndexOf("&", startIndex);
                if (endIndex < 0)
                {
                    endIndex = requestBody.Length;
                }

                return requestBody.Substring(startIndex + paramPrefix.Length, endIndex - (startIndex + paramPrefix.Length));
            }

            return null;
        }

        //private string TransactionCode(string codeN)
        //{
        //    var getMessage = "";

        //    switch (codeN)
        //    {
        //        case "900003":
        //            getMessage = ": Insufficient Funds";
        //            break;
        //        case "900004":
        //            getMessage = ": Invalid Card Number";
        //            break;
        //        case "900007":
        //            getMessage = ": Transaction Declined";
        //            break;
        //        default:
        //            getMessage = "";
        //            break;
        //    }

        //    return getMessage;
        //}


        private string TransactionStatus(string id)
        {

            var status = "";

            switch (id)
            {
                case "-2":
                    status = "Unable to reconsile transaction";
                    break;
                case "-1":
                    status = "Checksum Error. The values have been altered";
                    break;
                case "0":
                    status = "Unprocessed Transactions";
                    break;
                case "1":
                    status = "Payment Successful";
                    break;

                case "2":
                    status = "Payment Unsuccessful";
                    break;
                case "3":
                    status = "Transaction Cancelled";
                    break;
                case "4":
                    status = "User Cancelled Transaction";
                    break;
                case "5":
                    status = "Received by PayGate";
                    break;
                case "7":
                    status = "Settlement Voided";
                    break;
                default:
                    status = $"Unprocessed Transactions ({id})";
                    break;
            }

            return status;
        }

        public void LogValues(string variable, string error)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ResponseLog.txt");

            string logEntry = $"{DateTime.Now} - {variable}: {error}";

            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }

        private bool PayGateOnline()
        {
            var token = HttpContext.Current?.Session["StudentJwtToken"] as string;

            int studentIdInt = int.Parse(Session["StudentID"].ToString());

            string url1 = qualificationVerificationAPI + "Student/GetStudent?studentId=" + studentIdInt;
            var student = BusinessRules.HttpHelper.HttpCallJson<Student>(url1, WebRequestMethods.Http.Get, token);

            var rootUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);


            var payGateService = new PayGateService
            {
                PayGateUrl = Settings.GetPayGateUrl,
                PayGateQueryUrl = Settings.GetPayGateQueryUrl,
                PayGateId = Settings.GetPayGateId,
                Reference = Session["StudentID"].ToString(),
                Amount = Session["txtTotalAmount"].ToString(),
                Currency = Constants.Currency,
                ReturnUrl = rootUrl + Settings.ReturnUrl,
                TransactionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Locale = Constants.Locale,
                Country = Constants.Country,
                CustomerEmail = student.EmailAddress,
                EncryptionKey = Settings.GetEncryptionKey,

            };

            LogValues("PayGateUrl", payGateService.PayGateUrl);
            LogValues("PayGateQueryUrl", payGateService.PayGateQueryUrl);
            LogValues("PayGateId", payGateService.PayGateId);
            LogValues("Reference", payGateService.Reference);
            LogValues("Amount", payGateService.Amount);
            LogValues("Currency", payGateService.Currency);
            LogValues("ReturnUrl", payGateService.ReturnUrl);
            LogValues("TransactionDate", payGateService.TransactionDate);
            LogValues("Locale", payGateService.Locale);
            LogValues("Country", payGateService.Country);
            LogValues("CustomerEmail", payGateService.CustomerEmail);
            LogValues("EncryptionKey", payGateService.EncryptionKey);


            var initiatePaymentResult = payGateService.InitiatePayment();

            LogValues("initiatePaymentResult", initiatePaymentResult.Message);


            if (initiatePaymentResult.Success)
            {
                string studentIdStr = Session["StudentID"].ToString();

                var token1 = Session["StudentJwtToken"].ToString();

                string url11 = qualificationVerificationAPI + "OnlinePaymentSession/GetOnlinePaymentSession?studentId=" + studentIdStr;
                var onlinePaymentSession = BusinessRules.HttpHelper.HttpCallJson<OnlinePaymentSession>(url11, WebRequestMethods.Http.Get, token1);


                onlinePaymentSession.TryAgain = int.Parse(Session["TryAgain"].ToString());

                onlinePaymentSession.LastUpdated = DateTime.Now;


                string url14 = qualificationVerificationAPI + "OnlinePaymentSession/UpdateOnlinePaymentSession";
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(onlinePaymentSession);
                var result = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url14, WebRequestMethods.Http.Post, JSONString, token1);

                payGateService.PayRequestId = initiatePaymentResult.Data["PAY_REQUEST_ID"];
                payGateService.ProcessPayment(this.Page, initiatePaymentResult.Data["PAYGATE_ID"], initiatePaymentResult.Data["PAY_REQUEST_ID"], initiatePaymentResult.Data["REFERENCE"], initiatePaymentResult.Data["CHECKSUM"]);


            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ShowStatusModal", "<script>ShowFailedPaymentStatusModal('" + initiatePaymentResult.Message + "');</script>");
                string script = $"ShowFailedPaymentStatusModal('{initiatePaymentResult.Message}');";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowStatusModal", $"<script>{script}</script>");


                return false;
            }

            return true;
        }
    }
}