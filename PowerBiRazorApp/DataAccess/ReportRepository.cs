using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.PowerBI.Api.V2;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using PowerBiRazorApp.Authentication.AuthenticationHandler;

namespace PowerBiRazorApp.DataAccess
{
    public class ReportRepository
    {
        private static readonly string Username = ConfigurationManager.AppSettings["pbiUsername"];
        private static readonly string Password = ConfigurationManager.AppSettings["pbiPassword"];
        private static readonly string AuthorityUrl = ConfigurationManager.AppSettings["authorityUrl"];
        private static readonly string ResourceUrl = ConfigurationManager.AppSettings["resourceUrl"];
        private static readonly string ApplicationId = ConfigurationManager.AppSettings["ApplicationId"];
        private static readonly string ApiUrl = ConfigurationManager.AppSettings["apiUrl"];
        private static readonly string WorkspaceId = ConfigurationManager.AppSettings["workspaceId"];
        private static readonly string ReportId = ConfigurationManager.AppSettings["reportId"];

        public async Task<IList<Microsoft.PowerBI.Api.V2.Models.Report>> GetAvailableReportsAsync()
        {
            var authHandler = new AuthenticationHandler();

            // Create a user password cradentials.
            var credential = await authHandler.GetAzureTokenDataAsync();

            using (var client = new PowerBIClient(new Uri(ApiUrl), credential.tokenCredentials))
            {
                var reports = await client.Reports.GetReportsInGroupAsync(WorkspaceId);
                return reports.Value;
            }
            
        }

    }
}
