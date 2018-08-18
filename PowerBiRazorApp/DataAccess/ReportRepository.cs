using Microsoft.PowerBI.Api.V2;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using PowerBiRazorApp.Authentication.AuthenticationHandler;

namespace PowerBiRazorApp.DataAccess
{
    public class ReportRepository
    {
        private readonly PowerBiSettings _powerBiSettings;

        private readonly AuthenticationHandler _authenticationHandler;

        public ReportRepository(AuthenticationHandler authenticationHandler, IOptions<PowerBiSettings> powerBiOptions)
        {
            _authenticationHandler = authenticationHandler;
            _powerBiSettings = powerBiOptions.Value;
        }

        public async Task<IList<Microsoft.PowerBI.Api.V2.Models.Report>> GetAvailableReportsAsync()
        {
            // Create a user password cradentials.
            var credential = await _authenticationHandler.GetAzureTokenDataAsync();

            using (var client = new PowerBIClient(new Uri(_powerBiSettings.MainAddress), credential.tokenCredentials))
            {
                var reports = await client.Reports.GetReportsInGroupAsync(_powerBiSettings.GroupId);
                return reports.Value;
            }
        }

    }
}
