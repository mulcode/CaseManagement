using CourtDataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CourtDomain.Models;

namespace Court.Pages.Complaints
{
    public class ComplaintListModel : PageModel
    {
        private readonly IComplaint c_complaint;
        private readonly ILogger<ComplaintListModel> _logger;

        public ComplaintListModel(
            IComplaint complaint,
            ILogger<ComplaintListModel> logger)
        {
            c_complaint = complaint;
            _logger = logger;
        }

        [BindProperty]
        public List<Complaint> Complaints { get; set; }
            = new List<Complaint>();

        public int DefendantId { get; set; }

        public IActionResult OnGet(int id)
        {
            try
            {
                DefendantId = id;

                Complaints = c_complaint
                    .GetComplaintsByDefendant(id)
                    .ToList();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading complaints for defendant {DefendantId}",
                    id);

                TempData["ErrorMessage"] =
                    "An error occurred while loading complaints.";

                return Page();
            }
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                var defendantId = c_complaint.DeleteComplaint(id);

                if (defendantId != 0)
                {
                    TempData["SuccessMessage"] =
                        "Complaint deleted successfully.";

                    return RedirectToPage(
                        "/Complaints/ComplaintList",
                        new { id = defendantId }
                    );
                }

                TempData["ErrorMessage"] =
                    "Complaint could not be deleted.";

                return RedirectToPage("/Complaints/ComplaintList");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error deleting complaint {ComplaintId}",
                    id);

                TempData["ErrorMessage"] =
                    "An error occurred while deleting the complaint.";

                return RedirectToPage("/Complaints/ComplaintList");
            }
        }
    }
}

