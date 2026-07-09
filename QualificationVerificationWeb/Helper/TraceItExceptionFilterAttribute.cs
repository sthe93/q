using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace QualificationVerificationWeb.Helper
{
    public class ExceptionHandlingModule : IHttpModule
    {
        private static readonly string _baseUrl = ConfigurationManager.AppSettings["TraceIt:BaseUrl"];


        //Environment variables
        private static readonly string _username = Environment.GetEnvironmentVariable("TRACEIT_TOKEN_USER");
        private static readonly string _password = Environment.GetEnvironmentVariable("TRACEIT_TOKEN_PASSWORD");


        public void Init(HttpApplication context)
        {
            context.Error += OnError;
        }

     

        private void OnError(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var ex = app.Server.GetLastError()?.GetBaseException();

            if (ex is System.Threading.ThreadAbortException)
                return;
          
            Task.Run(() => LogExceptionAsync(ex, app.Context));
        }

        private async Task LogExceptionAsync(Exception ex, HttpContext context)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);

                    var token = await GetTokenAsync(client);

                    var logData = new
                    {
                        Level = 2,
                        Message = $"{ex.Message} {ex.InnerException?.Message}",
                        Exception = ex.ToString(),
                        Source = "QualificationVerificationWeb",
                        UserName = "QualificationVerification",
                        Method = context.Request.Url?.ToString(),
                        //Method = context.Request.HttpMethod
                    };

                    var content = new StringContent(
                        JsonConvert.SerializeObject(logData),
                        Encoding.UTF8,
                        "application/json"
                    );

                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                    await client.PostAsync($"{_baseUrl}audit-log/create", content);
                }
            }
            catch (Exception logEx)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to log to TraceIt: {logEx.Message}");
            }
        }

        private async Task<string> GetTokenAsync(HttpClient client)
        {
            try
            {
                var authData = new { Username = _username, Password = _password };
                var authContent = new StringContent(
                    JsonConvert.SerializeObject(authData),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync($"{_baseUrl}Auth/login", authContent);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseBody);
                return tokenResponse?.token ?? "";
            }
            catch
            {
                return "";
            }
        }

        private class TokenResponse
        {
            public string token { get; set; }
        }

        public void Dispose()
        {
        }
    }
}