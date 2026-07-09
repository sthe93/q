using Newtonsoft.Json;

namespace QualificationVerificationWeb.Admin.Models
{
    public class TokenResponse
    {
        // This represent the login response which contains the JWT token
        [JsonProperty("tokenString")]
        public string JWTToken { get; set; }
    }
}