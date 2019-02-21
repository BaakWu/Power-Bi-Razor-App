using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PowerBiRazorApp.Pages
{
    public class RowLevelSecurityTesterModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Role { get; set; }

        [BindProperty]
        public Guid ReportID { get; set; }

        public RowLevelSecurityTesterModel()
        {
        }

        public void OnGet(Guid reportID)
        {
            ReportID = reportID;
        }

        /// <summary>
        /// Sets the row level security permissions towards the report
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostSetReportRLS()
        {
            return RedirectToPage("Report", new { reportID = ReportID, name = Name, role = Role });
        }

    }
}