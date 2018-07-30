using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Rest;
using Newtonsoft.Json;
using System.Configuration;

namespace PowerBiRazorApp.Authentication.AuthenticationHandler
{
    public class AuthenticationHandler
    {
        private static readonly string Username = ConfigurationManager.AppSettings["pbiUsername"];
        private static readonly string Password = ConfigurationManager.AppSettings["pbiPassword"];
        private static readonly string AuthorityUrl = ConfigurationManager.AppSettings["authorityUrl"];
        private static readonly string ResourceUrl = ConfigurationManager.AppSettings["resourceUrl"];
        private static readonly string ApplicationId = ConfigurationManager.AppSettings["ApplicationId"];
        private static readonly string ApiUrl = ConfigurationManager.AppSettings["apiUrl"];
        private static readonly string WorkspaceId = ConfigurationManager.AppSettings["workspaceId"];
        private static readonly string ReportId = ConfigurationManager.AppSettings["reportId"];
        private static readonly string AzureTenantID = ConfigurationManager.AppSettings["tenantId"];
        private static readonly string AzureInstance = ConfigurationManager.AppSettings["instanceURL"];
        private static readonly string AzureTokenType = ConfigurationManager.AppSettings["tokenType"];

        /// <returns></returns>
        public async Task<(TokenCredentials tokenCredentials, string accessToken)> GetAzureTokenDataAsync()
        {

             var authorityUrl = $"{AzureInstance}{AzureTenantID}/oauth2/token";

             var oauthEndpoint = new Uri(authorityUrl);

             using (var client = new HttpClient())
             {
                 var result = await client.PostAsync(oauthEndpoint, new FormUrlEncodedContent(new[]
                 {
                     new KeyValuePair<string, string>("resource", ResourceUrl),
                     new KeyValuePair<string, string>("client_id", ApplicationId),
                     new KeyValuePair<string, string>("grant_type", "password"),
                     new KeyValuePair<string, string>("username", Username),
                     new KeyValuePair<string, string>("password", Password),
                     new KeyValuePair<string, string>("scope", "openid"),
                 }));

                 var content = await result.Content.ReadAsStringAsync();

                 var authenticationResult = JsonConvert.DeserializeObject<OAuthResult>(content);
                 return (new TokenCredentials(authenticationResult.AccessToken, "Bearer"), authenticationResult.AccessToken);
             }
            
        }

        private class OAuthResult
        {
            [JsonProperty("token_type")]
            public string TokenType { get; set; }
            [JsonProperty("scope")]
            public string Scope { get; set; }
            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }
            [JsonProperty("ext_expires_in")]
            public int ExtExpiresIn { get; set; }
            [JsonProperty("expires_on")]
            public int ExpiresOn { get; set; }
            [JsonProperty("not_before")]
            public int NotBefore { get; set; }
            [JsonProperty("resource")]
            public Uri Resource { get; set; }
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }
        }
    }
}