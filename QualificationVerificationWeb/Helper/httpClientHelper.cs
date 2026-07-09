using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace QualificationVerificationWeb.Helper
{
    public static class HttpClientHelper
    {
        public static string GetToken(string url, string userName, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>( "grant_type", "password" ),
                        new KeyValuePair<string, string>( "username", userName ),
                        new KeyValuePair<string, string> ( "Password", password )
                    };
            var content = new FormUrlEncodedContent(pairs);
            
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url + "Token", content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public static System.Net.Http.HttpClient httpClient()
        {

            var authValue = new AuthenticationHeaderValue("Bearer", Tokenbuild());

            var client = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue },
                
            };
            return client;
        }

        public static string Tokenbuild()
        {
            return "";
        }


    }
}