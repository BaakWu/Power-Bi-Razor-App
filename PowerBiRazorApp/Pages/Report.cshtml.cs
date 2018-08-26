using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PowerBiRazorApp.DataAccess;
using PowerBiRazorApp.Models;

namespace PowerBiRazorApp.Pages
{
    public class ReportModel : PageModel
    {
        public ReportEmbedConfig ReportDetail { get; private set; }

        private readonly ReportRepository _reportRepo;

        /// <param name="reportRepo"></param>
        public ReportModel(ReportRepository reportRepo)
        {
            _reportRepo = reportRepo;
        }

        /// <returns></returns>
        public async Task OnGetAsync(Guid reportID)
        {
            ReportDetail = await _reportRepo.GetReportForIdAsync(reportID);
        }
    }
}