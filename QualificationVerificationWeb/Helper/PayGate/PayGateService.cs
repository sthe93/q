using QualificationVerificationWeb.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace QualificationVerificationWeb.Helper.PayGate
{
    public class PayGateService
    {
        public string PayGateUrl { get; set; }
        public string PayGateQueryUrl { get; set; }
        public string PayGateId { get; set; }
        public string Reference { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string ReturnUrl { get; set; }
        public string TransactionDate { get; set; }
        public string Locale { get; set; }
        public string Country { get; set; }
        public string CustomerEmail { get; set; }
        public string EncryptionKey { get; set; }
        public string PayRequestId { get; set; }

        private readonly PayGateHelper _payGateHelper = new PayGateHelper();

        public PayGateResponse InitiatePayment()
        {
            try
            {
                var postData = CreatePostData();
                var client = new RestClient();
                var request = new RestRequest(PayGateUrl, Method.Post);
                Logs.WriteErrorLog("CreatePostData");
                foreach (var item in postData)
                {
                    request.AddParameter(item.Key, item.Value);
                }

                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    var responseContent = response.Content;

                    var results = _payGateHelper.ToDictionary(responseContent);

                    if (results.TryGetValue("ERROR", out string error) && !string.IsNullOrEmpty(error))
                    {
                        Logs.WriteErrorLog("Execute");
                        return new PayGateResponse()
                        {
                            Success = false,
                            Message = $"An error occurred while initiating your request: {error}",
                        };
                    }

                    if (!_payGateHelper.VerifyMd5Hash(results, EncryptionKey, results["CHECKSUM"]))
                    {
                        Logs.WriteErrorLog("VerifyMd5Hash");
                        return new PayGateResponse()
                        {
                            Success = false,
                            Message = "MD5 verification failed",
                        };

                    }

                    var isRecorded = _payGateHelper.AddTransaction(postData, results["PAY_REQUEST_ID"]);
                    Logs.WriteErrorLog("isRecorded");

                    if (isRecorded)
                    {
                        Logs.WriteErrorLog("Transaction Recorded successfully & Payment initiated successfully");
                        return new PayGateResponse()
                        {
                            Success = true,
                            Message = "Transaction Recorded successfully & Payment initiated successfully",
                            Data = results,
                        };

                    }
                    else
                    {
                        Logs.WriteErrorLog("Failed to record transaction");
                        return new PayGateResponse()
                        {
                            Success = false,
                            Message = "Failed to record transaction",
                        };
                    }
                }
                else
                {

                    if (response.ErrorMessage == null && response.StatusCode.ToString() == "Forbidden")
                    {

                        return new PayGateResponse()
                        {
                            Success = false,
                            Message = $"Sorry we are unable to process your online payment at the moment. Kindly contact transcripts@uj.ac.za for further information.",
                        };
                    }
                    else
                    {
                        return new PayGateResponse()
                        {
                            Success = false,
                            Message = $"Error in making the payment request: {response.ErrorMessage}",
                        };
                    }
                }
            }
            catch (Exception ex)
            {  
                Logs.WriteErrorLog("Payment initiation failed: " + ex.ToString());

                return new PayGateResponse()
                {
                    Success = false,
                    Message = $"Payment initiation failed: {ex.Message}",
                    
                };
                
               

            }
        }

        public void ProcessPayment(Page page, string payGateId, string payRequestId, string reference, string checksum)
        {
            var scriptManager = page.ClientScript;


            scriptManager.RegisterStartupScript(GetType(), "processPayment", $"processPayment('{payGateId}','{payRequestId}','{reference}','{checksum}');", true);

        }

        public PayGateResponse QueryPayment()
        {
            try
            {
                var queryData = CreateQueryData();
                var client = new RestClient();
                var request = new RestRequest(PayGateQueryUrl, Method.Post);

                foreach (var item in queryData)
                {
                    request.AddParameter(item.Key, item.Value);
                }

                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    var responseContent = response.Content;
                    var results = _payGateHelper.ToDictionary(responseContent);

                    if (results.TryGetValue("ERROR", out string error) && !string.IsNullOrEmpty(error))
                    {
                        Logs.WriteErrorLog($"An error occurred while querying your payment: {error}");

                        return new PayGateResponse()
                        {
                            Success = false,
                            Message = $"An error occurred while querying your payment: {error}",
                        };
                    }

                    if (!_payGateHelper.VerifyMd5Hash(results, EncryptionKey, results["CHECKSUM"]))
                    {
                        return new PayGateResponse()
                        {
                            Success = false,
                            Message = "MD5 verification failed",
                        };

                    }
                    Logs.WriteErrorLog("before UpdateTransactionByPayRequest : PayRequestId :" + PayRequestId);

                    var isUpdated = _payGateHelper.UpdateTransactionByPayRequest(results, PayRequestId);

                    Logs.WriteErrorLog("after UpdateTransactionByPayRequest :  isUpdated :" + isUpdated);

                    if (isUpdated)
                    {
                        Logs.WriteErrorLog("Transaction Updated successfully & Payment Completed");

                        return new PayGateResponse()
                        {
                            Success = true,
                            Message = "Transaction Updated successfully & Payment Completed ",
                            Data = results,
                        };
                    }
                    else
                    {
                        Logs.WriteErrorLog("Failed to update transaction");

                        return new PayGateResponse()
                        {
                            Success = false,
                            Message = "Failed to update transaction",
                        };
                    }
                }
                else
                {
                    return new PayGateResponse()
                    {
                        Success = false,
                        Message = $"Error in making request to query transaction: {response.ErrorMessage}",
                    };
                }
            }
            catch (Exception ex)
            {
                return new PayGateResponse()
                {
                    Success = false,
                    Message = $"Payment Query failed: {ex.Message}",
                };
            }

        }

        private Dictionary<string, string> CreatePostData()
        {
            var postData = new Dictionary<string, string>
            {
                { "PAYGATE_ID", PayGateId },
                { "REFERENCE", Reference },
                { "AMOUNT", Amount },
                { "CURRENCY", Currency },
                { "RETURN_URL", ReturnUrl },
                { "TRANSACTION_DATE", TransactionDate },
                { "LOCALE", Locale },
                { "COUNTRY", Country },
                { "EMAIL", CustomerEmail },
            };

            var payGateHelper = new PayGateHelper();
            postData["CHECKSUM"] = payGateHelper.CalculateMd5Hash(postData, EncryptionKey);

            return postData;
        }


        private Dictionary<string, string> CreateQueryData()
        {
            var queryData = new Dictionary<string, string>
            {
                { "PAYGATE_ID", PayGateId },
                { "PAY_REQUEST_ID", PayRequestId },
                { "REFERENCE", Reference },
            };

            var payGateHelper = new PayGateHelper();
            queryData["CHECKSUM"] = payGateHelper.CalculateMd5Hash(queryData, EncryptionKey);

            return queryData;
        }

    }
}