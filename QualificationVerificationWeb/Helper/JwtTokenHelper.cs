using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;
using QualificationVerificationWeb.Admin.Models;

public class JwtTokenHelper
{
    public static UserApplicationListModelV2 ParseUserProfileFromToken(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);

        var userDataJson = token.Claims.FirstOrDefault(c => c.Type == "userData")?.Value;

        if (string.IsNullOrWhiteSpace(userDataJson))
        {
            System.Diagnostics.Debug.WriteLine("userData claim not found in token.");

            return null;
        }
        System.Diagnostics.Debug.WriteLine("userDataJson: " + userDataJson);

        // Deserialize userData JSON to a model
        var userProfile = JsonConvert.DeserializeObject<UserApplicationListModelV2>(userDataJson);


        return userProfile;
    }
}