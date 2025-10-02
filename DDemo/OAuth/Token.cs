using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DDemo.OAuth
{
    internal static class Token
    {
        internal static async Task<string> Obtain(string baseUrl, Guid integrationKey, Guid userId)
        {
            HttpClient httpClient = new() { BaseAddress = new Uri(baseUrl) };

            string jwt = JWT(baseUrl, integrationKey, userId);
            string postMessage = "grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer&assertion=" + jwt;

            using HttpContent stringContent = new StringContent(postMessage, Encoding.UTF8, "application/x-www-form-urlencoded");

            using HttpResponseMessage response = await httpClient.PostAsync("oauth/token", stringContent);
            string reply = await response.Content.ReadAsStringAsync();

            ReplyObject replyObject = reply.Deserialize<ReplyObject>() ?? new ReplyObject();
            if (!string.IsNullOrWhiteSpace(replyObject.Error))
            {
                if (replyObject.Error == "consent_required")
                    throw new ApplicationException($"{replyObject.Error_Description}, visit {baseUrl}/oauth/auth?response_type=code&scope=signature%20impersonation&client_id={integrationKey}&redirect_uri=the_redirect_uri_you_added_on_apps_and_keys");
                else
                    throw new ApplicationException("Token.Obtain " + replyObject.Error_Description);
            }

            return replyObject.Access_token;
        }
        private static string JWT(string baseUrl, Guid integrationKey, Guid userId)
        {
            Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler handler = new();

            string jwt = handler.CreateToken(new SecurityTokenDescriptor()
            {
                Claims = new Dictionary<string, object>
                {
                    ["sub"] = userId.ToString(),
                    ["scope"] = "signature impersonation"
                },

                Expires = DateTime.Now.AddSeconds(6000),

                SigningCredentials = new(Key.RsaSecurityKey, SecurityAlgorithms.RsaSha256),
                Issuer = integrationKey.ToString(),
                Audience = new Uri(baseUrl).Host
            });

            return jwt;
        }
        private sealed class ReplyObject
        {
            public string Access_token { get; init; } = string.Empty;
            public string Token_type { get; init; } = string.Empty;
            public int Expires_in { get; init; } = 0;
            public string Ccope { get; init; } = string.Empty;
            public string Error { get; init; } = string.Empty;
            public string Error_Description { get; init; } = string.Empty;
        }

    }
}
