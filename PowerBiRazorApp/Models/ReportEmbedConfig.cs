using System;

namespace PowerBiRazorApp.Models
{
    /// <summary>
    /// An object of the necessary details for the client to embed the report
    /// </summary>
    public class ReportEmbedConfig
    {
        /// <summary>
        /// The Power BI report ID
        /// </summary>
        public Guid ReportID { get; set; }

        /// <summary>
        /// Name of report
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Access token associated with report
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Embed Url for report
        /// </summary>
        public string EmbedUrl { get; set; }
    }
}