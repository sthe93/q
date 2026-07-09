using Newtonsoft.Json;
using QualificationVerificationWeb.Admin.Models;
using QualificationVerificationWeb.Helper;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace BusinessRules
{
    public static class HttpHelper
    {
        private static readonly HttpClient Client = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(60)
        };

        static HttpHelper()
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // =====================
        // GET
        // =====================
        public static T HttpCallJson<T>(string url, string method, string token)
        {
            AddAuthorizationHeader(token);

            return HttpCallJson<T>(url, method, null, token);
        }

        // =====================
        // POST / PUT
        // =====================
        public static T HttpCallJson<T>(string url, string method, string jsonBody, string token)
        {
            AddAuthorizationHeader(token);

            HttpResponseMessage response;

            if (method == WebRequestMethods.Http.Get)
            {
                response = Client.GetAsync(url).Result;
            }
            else
            {
                var content = new StringContent(
                    jsonBody ?? string.Empty,
                    Encoding.UTF8,
                    "application/json");

                var request = new HttpRequestMessage(new HttpMethod(method), url)
                {
                    Content = content
                };

                response = Client.SendAsync(request).Result;
            }

    
            var responseContent = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest)
            {
               
                return JsonConvert.DeserializeObject<T>(responseContent);
            }

            Logs.WriteErrorLog($"HttpCallJson error1 :{method.ToString()}- {response.StatusCode.ToString()} - {responseContent.ToString()}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("JWT expired or invalid");

            if (!response.IsSuccessStatusCode)
            {
                Logs.WriteErrorLog($"HttpCallJson error2 :{method.ToString()}- {response.StatusCode.ToString()} - {responseContent.ToString()}");

                throw new Exception($"API Error: {response.StatusCode} - {responseContent}");
            }

            throw new Exception($"API Error: {response.StatusCode} - {responseContent}");
        }

        private static void AddAuthorizationHeader(string token)
        {
            Client.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(token))
            {
                Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}