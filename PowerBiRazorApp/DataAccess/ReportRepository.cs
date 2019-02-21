using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.PowerBI.Api.V2;
using Microsoft.PowerBI.Api.V2.Models;
using System.Threading.Tasks;
using PowerBiRazorApp.Authentication.AuthenticationHandler;
using PowerBiRazorApp.Models;

namespace PowerBiRazorApp.DataAccess
{
    /// <summary>
    /// Class responsible for retrieving Power BI report details from the Power BI API
    /// </summary>
    public class ReportRepository
    {
        private readonly PowerBiSettings _powerBiSettings;

        private readonly AuthenticationHandler _authenticationHandler;

        public ReportRepository(AuthenticationHandler authenticationHandler, IOptions<PowerBiSettings> powerBiOptions)
        {
            _authenticationHandler = authenticationHandler;
            _powerBiSettings = powerBiOptions.Value;
        }

        /// <summary>
        /// Retrieves a list of reports from Power BI api
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Report>> GetAvailableReportsAsync()
        {
            // Create a user password cradentials.
            var credential = await _authenticationHandler.GetAzureTokenDataAsync();

            using (var client = new PowerBIClient(new Uri(_powerBiSettings.MainAddress), credential.tokenCredentials))
            {
                var reports = await client.Reports.GetReportsInGroupAsync(_powerBiSettings.GroupId);
                return reports.Value;
            }
        }

        /// <summary>
        /// Retrieves the report details for the passed in report reportId
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="azureUser"></param>
        /// <returns></returns>
        public async Task<ReportEmbedConfig> GetReportForIdAsync(Guid reportId)
        {
            var azureTokenData = await _authenticationHandler.GetAzureTokenDataAsync();
            
            using (var powerBiClient = new PowerBIClient(new Uri(_powerBiSettings.MainAddress), azureTokenData.tokenCredentials))
            {
                var powerBiReport = await powerBiClient.Reports.GetReportAsync(_powerBiSettings.GroupId, reportId.ToString());
            
                var powerBiTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
            
                var powerBiTokenResponse = await powerBiClient.Reports.GenerateTokenInGroupAsync(_powerBiSettings.GroupId, powerBiReport.Id, powerBiTokenRequestParameters);
            
                return new ReportEmbedConfig
                {
                    ReportID = Guid.Parse(powerBiReport.Id),
                    Name = powerBiReport.Name,
                    EmbedUrl = powerBiReport.EmbedUrl,
                    AccessToken = powerBiTokenResponse.Token
                };
            }
        }

        /// <summary>
        /// Retrieves the report details for the passed in report reportId with a row level security name and role attached to it
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="azureUser"></param>
        /// <returns></returns>
        public async Task<ReportEmbedConfig> GetReportForIdAsync(Guid reportId, String name, String role)
        {
            var azureTokenData = await _authenticationHandler.GetAzureTokenDataAsync();

            using (var powerBiClient = new PowerBIClient(new Uri(_powerBiSettings.MainAddress), azureTokenData.tokenCredentials))
            {
                var powerBiReport = await powerBiClient.Reports.GetReportAsync(_powerBiSettings.GroupId, reportId.ToString());

                var rowLevelSecurityIdentity = new List<EffectiveIdentity>
                        {
                            new EffectiveIdentity(name,
                                roles: new List<string> {role},
                                datasets: new List<string> {powerBiReport.DatasetId})
                        };

                var powerBiTokenRequestParameters = new GenerateTokenRequest("view", null, identities: rowLevelSecurityIdentity);

                var powerBiTokenResponse = await powerBiClient.Reports.GenerateTokenInGroupAsync(_powerBiSettings.GroupId, powerBiReport.Id, powerBiTokenRequestParameters);

                return new ReportEmbedConfig
                {
                    ReportID = Guid.Parse(powerBiReport.Id),
                    Name = powerBiReport.Name,
                    EmbedUrl = powerBiReport.EmbedUrl,
                    AccessToken = powerBiTokenResponse.Token
                };
            }
        }

    }
}
