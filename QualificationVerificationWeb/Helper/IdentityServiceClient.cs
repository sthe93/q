using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QualificationVerificationWeb.Admin.Models;

namespace QualificationVerificationWeb.Helper
{
    public class IdentityServiceClient
    {
        private readonly HttpClient _httpClient;

        public IdentityServiceClient()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
        }

        public async Task<UserApplicationListModelV2> LoginAndGetProfileAsync(string username, string password)
        {
            
            string loginUrl = ConfigurationManager.AppSettings["IdentityServiceLoginUrl"];

            //sending raw data to the API endpoint
            var keyValues = new List<KeyValuePair<string, string>>
{
            new KeyValuePair<string, string>("Username", username),
            new KeyValuePair<string, string>("Password", password)
};

            var content = new FormUrlEncodedContent(keyValues);


            //Code to sending request to API as a Json data/structure
            //var json = JsonConvert.SerializeObject(loginData);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            System.Diagnostics.Debug.WriteLine("Sending login request to API...");


            System.Diagnostics.Debug.WriteLine($"Calling login API: {loginUrl} with user: {username}");
            var logInResponse = await _httpClient.PostAsync(loginUrl, content);
            if (!logInResponse.IsSuccessStatusCode)
            {
                Debug.WriteLine("Login failed: " + logInResponse.StatusCode);
                return null;

            }
            
            string tokenBody = await logInResponse.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($" Status Code: {logInResponse.StatusCode}");
            System.Diagnostics.Debug.WriteLine($" Token Response Body: {tokenBody}");

            var tokenResponse = JsonConvert.DeserializeObject<JObject>(tokenBody);
            var token = tokenResponse?["token"]?.ToString() ?? tokenResponse?["tokenString"]?.ToString();

            if (string.IsNullOrWhiteSpace(token))
            {
                Debug.WriteLine("Token was null or empty.");
                return null;
            }

            // Deserialize JWT and extract user info
            var userProfile = JwtTokenHelper.ParseUserProfileFromToken(token);
            if (userProfile?.Applications != null)
            {
                foreach (var app in userProfile.Applications)
                {
                    Debug.WriteLine($"App: {app.AppName}, Role(s): {app.Roles}");
                }
            }
            else
            {
                Debug.WriteLine("No applications found in user profile.");
            }
            return userProfile;
        }   
    }
}