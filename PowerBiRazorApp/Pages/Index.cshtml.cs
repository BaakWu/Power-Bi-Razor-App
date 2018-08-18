using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PowerBiRazorApp.DataAccess;

namespace PowerBiRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        public List<Microsoft.PowerBI.Api.V2.Models.Report> AvailableReports { get; private set; }
        private ReportRepository _reportRepo;

        public IndexModel(ReportRepository reportRepo)
        {
            _reportRepo = reportRepo;
        }

        public async Task OnGetAsync()
        {
            var reports = await _reportRepo.GetAvailableReportsAsync();
            AvailableReports = reports.ToList();
        }

    }

}
