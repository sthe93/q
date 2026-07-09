using QualificationVerificationWeb.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace QualificationVerificationWeb.Helper.PayGate
{
    public class PayGateHelper
    {
        //private readonly TransactionRepository _transactionRepository = new TransactionRepository();

        private readonly string qualificationVerificationAPI = ConfigurationManager.AppSettings["QualificationVerificationAPI"].ToString();

        public string CalculateMd5Hash(Dictionary<string, string> data, string encryptionKey)
        {
            using (var md5Hash = MD5.Create())
            {
                var input = new StringBuilder();
                foreach (string value in data.Values)
                {
                    input.Append(value);
                }

                input.Append(encryptionKey);

                var hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input.ToString()));

                var sBuilder = new StringBuilder();

                for (var i = 0; i < hash.Length; i++)
                {
                    sBuilder.Append(hash[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public bool VerifyMd5Hash(Dictionary<string, string> data, string encryptionKey, string hash)
        {
            Dictionary<string, string> hashDict = new Dictionary<string, string>();

            foreach (string key in data.Keys)
            {
                if (key != "CHECKSUM")
                {
                    hashDict.Add(key, data[key]);
                }
            }

            string hashOfInput = CalculateMd5Hash(hashDict, encryptionKey);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ToUrlEncodedString(Dictionary<string, string> request)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string key in request.Keys)
            {
                builder.Append("&");
                builder.Append(key);
                builder.Append("=");
                builder.Append(HttpUtility.UrlEncode(request[key]));
            }

            string result = builder.ToString().TrimStart('&');

            return result;
        }

        public Dictionary<string, string> ToDictionary(string response)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            string[] valuePairs = response.Split('&');
            foreach (string valuePair in valuePairs)
            {
                string[] values = valuePair.Split('=');
                result.Add(values[0], HttpUtility.UrlDecode(values[1]));
            }

            return result;
        }
        public bool AddTransaction(Dictionary<string, string> request, string payRequestId)
        {
            var token = HttpContext.Current?.Session["StudentJwtToken"] as string;

            try
            {
                Transaction transaction = new Transaction
                {
                    DATE = DateTime.Now,
                    PAY_REQUEST_ID = payRequestId,
                    REFERENCE = request["REFERENCE"],
                    AMOUNT = int.Parse(request["AMOUNT"]),
                    CUSTOMER_EMAIL_ADDRESS = request["EMAIL"]
                };

                string url14 = qualificationVerificationAPI + "Transaction/AddTransaction";
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(transaction);
                BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url14, WebRequestMethods.Http.Post, JSONString, token);

                return true;
            }
            catch (Exception e)
            {
                Logs.WriteErrorLog("Failed on method AddTransaction" + e.ToString());
                return false;
            }
        }



        public bool UpdateTransactionByPayRequest(Dictionary<string, string> request, string payRequestId)
        {
            try
            {

                var isUpdated = false;

                string url1 = qualificationVerificationAPI + "Transaction/GetTransactionByPayRequestId?payRequestId=" + payRequestId;
                var transaction = BusinessRules.HttpHelper.HttpCallJson<Transaction>(url1, WebRequestMethods.Http.Get, null);



                if (transaction == null || string.IsNullOrWhiteSpace(payRequestId))
                    return false;


                if (request["TRANSACTION_STATUS"] != null)
                    transaction.TRANSACTION_STATUS = request["TRANSACTION_STATUS"];

                if (request["RESULT_DESC"] != null)
                    transaction.RESULT_DESC = request["RESULT_DESC"];

                if (request["RESULT_CODE"] != null)
                    transaction.RESULT_CODE = (int?)(ResultCodes)int.Parse(request["RESULT_CODE"]);

                Logs.WriteErrorLog("TRANSACTION_STATUS: " + request["TRANSACTION_STATUS"].ToString());
                Logs.WriteErrorLog("RESULT_DESC: " + request["RESULT_DESC"].ToString());
                Logs.WriteErrorLog("RESULT_CODE: " + request["RESULT_CODE"].ToString());


                string url14 = qualificationVerificationAPI + "Transaction/UpdateTransaction";
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(transaction);
                var result = BusinessRules.HttpHelper.HttpCallJson<ViewModels.Validator>(url14, WebRequestMethods.Http.Post,JSONString, null);

                switch (result.Status)
                {
                    case Constants.Success:
                        isUpdated = true;
                        break;
                    case Constants.Error:
                        isUpdated = false;
                        break;
                }

                return isUpdated;

            }
            catch (Exception e)
            {
                Logs.WriteErrorLog("Failed on method UpdateTransactionByPayRequest" + e.ToString());

                return false;
            }
        }
    }
}