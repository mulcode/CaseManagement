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
        public List<Complaint> Complaints { get; set; } = new List<Complaint>();

        public int DefendantId { get; set; }

        public IActionResult OnGet(int id)
        {
            try
            {
                DefendantId = id;

                Complaints = c_complaint.GetComplaintsByDefendant(id).ToList();

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

        public IActionResult OnPostDelete(int complaintId)
        {
            try
            {
                _logger.LogInformation(
                    "Deleting complaint with ID: {ComplaintId}", complaintId);

                var defendantId = c_complaint.DeleteComplaint(complaintId);

                if (defendantId != 0)
                {
                    TempData["SuccessMessage"] =
                        "Complaint deleted successfully.";

                    return RedirectToPage("/Complaints/ComplaintList");
                }

                TempData["ErrorMessage"] =
                    $"Complaint with ID {complaintId} was not found.";

                return RedirectToPage("/Complaints/ComplaintList");
            }
            catch (Exception ex)
            {
                // Actual error console-e dekhabe
                Console.WriteLine($"DELETE ERROR: {ex}");
                _logger.LogError(ex,
                    "Error deleting complaint {ComplaintId}", complaintId);

                TempData["ErrorMessage"] = ex.InnerException?.Message
                    ?? ex.Message;

                return RedirectToPage("/Complaints/ComplaintList");
            }
        }
    }
}

