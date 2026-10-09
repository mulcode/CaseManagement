using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages
{
    [Authorize(Policy = "AuthorizedUsers")]
    public class DashboardModel : PageModel
    {
        private readonly IComplaint d_complaint;
        private readonly ILogger<DashboardModel> _logger;

        public DashboardModel(
            IComplaint complaint,
            ILogger<DashboardModel> logger)
        {
            d_complaint = complaint;
            _logger = logger;
        }

        public IEnumerable<DefendantComplaintViewModel> Defendants
        {
            get;
            set;
        } = new List<DefendantComplaintViewModel>();

        public IActionResult OnGet()
        {
            try
            {
                Defendants = d_complaint.GetAwatingComplaintSubmision();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading dashboard and awaiting complaint submissions.");

                TempData["ErrorMessage"] =
                    "An error occurred while loading the dashboard.";

                return Page();
            }
        }
    }
}

