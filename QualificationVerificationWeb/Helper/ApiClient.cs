using Newtonsoft.Json;
using QualificationVerificationWeb.Admin.Models;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QualificationVerificationWeb.Helper
{
    public  class ApiClient
    {
        private readonly HttpClient _httpClient;
        private const string ApplicationJson = "application/json";

        public ApiClient()
        {
            string baseAddress = ConfigurationManager.AppSettings["QualificationVerificationAPI"];

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            ConfigureDefaultHeaders();
        }

        private void ConfigureDefaultHeaders()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(ApplicationJson));
        }

        public static async Task<ApiAuthResponse> GetJsonToken(string username, string password)
        {
            string apiBaseUrl = BaseUrl();

            using (var authClient = CreateAuthClient(apiBaseUrl))
            {
                var loginData = new
                {
                    Username = username,
                    Password = password
                };

                var content = CreateJsonContent(loginData);
                var response = await authClient.PostAsync("auth/login", content);

                return await HandleAuthResponse(response);
            }
        }


        public static async Task<StudentApiAuthResponse> GetStudentJsonToken(string studentIdNumber)
        {
            string apiBaseUrl = BaseUrl();

            using (var authClient = CreateAuthClient(apiBaseUrl))
            {
                var loginData = new
                {
                    StudentIdNumber = studentIdNumber
                };

                var content = CreateJsonContent(loginData);
                var response = await authClient.PostAsync("auth/studentLogin", content);

                return await HandleStudentAuthResponse(response);
            }
        }

        private static HttpClient CreateAuthClient(string baseAddress)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(ApplicationJson));

            return client;
        }

        private static StringContent CreateJsonContent(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return new StringContent(json, Encoding.UTF8, ApplicationJson);
        }

  

        private static async Task<ApiAuthResponse> HandleAuthResponse(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest)
            {
                return JsonConvert.DeserializeObject<ApiAuthResponse>(responseContent);
            }

            Logs.WriteErrorLog($"Login error: {(int)response.StatusCode} - {responseContent}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new HttpException(401, "Unauthorized");

            throw new HttpException((int)response.StatusCode, "API Error");
        }

        private static async Task<StudentApiAuthResponse> HandleStudentAuthResponse(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest)
            {
                return JsonConvert.DeserializeObject<StudentApiAuthResponse>(responseContent);
            }

            Logs.WriteErrorLog($"Login error: {(int)response.StatusCode} - {responseContent}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new HttpException(401, "Unauthorized");

            throw new HttpException((int)response.StatusCode, "API Error");
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            string s = BaseUrl(); 
            AddAuthorizationHeader();

            return await _httpClient.GetAsync(s+endpoint);
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content)
        {
            string s = BaseUrl(); 
            AddAuthorizationHeader();

            return await _httpClient.PostAsync(s+endpoint, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            string s = BaseUrl();
            
            AddAuthorizationHeader();

            return await _httpClient.DeleteAsync(s+endpoint);
        }

        private void AddAuthorizationHeader()
        {
            var token = HttpContext.Current?.Session["JwtToken"] as string;

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private static string BaseUrl()
        {
            var configuration = ConfigurationManager.AppSettings["QualificationVerificationAPI"];
            return configuration;
        }
    }
}