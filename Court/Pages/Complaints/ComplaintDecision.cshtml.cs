using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Complaints
{
    public class ComplaintDecisionModel : PageModel
    {
    private readonly IComplaint c_complaint;
    private readonly IUser _user;
    public ComplaintDecisionModel(IComplaint complaint, IUser user)
    {
        c_complaint = complaint;
        _user = user;
    }

    [BindProperty]
    public Complaint ComplaintInfo { get; set; }

    public IEnumerable<User> Users { get; set; }

        public IActionResult OnGet(int complaintId)
        {
            var complaint = c_complaint.GetComplaint(complaintId);

            if (complaint == null)
            {
                return NotFound();
            }

            if (!complaint.ReadyForApproval)
            {
                TempData["ErrorMessage"] = "Please submit the complaint first.";

                return RedirectToPage(
                    "/Complaints/Complaint",   // redirect hobena
                    new
                    {
                        complaintId = complaintId
                    }
                );
            }

            ComplaintInfo = complaint;

            Users = _user.GetUsersByRole(2).ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Users = _user.GetUsersByRole(2).ToList();
                return Page();
            }

         

            c_complaint.UpdateComplaint(ComplaintInfo);

            TempData["SuccessMessage"] = "Decision Approved";

            return RedirectToPage(
                "/Complaints/ComplaintDecision",
                new { complaintId = ComplaintInfo.Id }
            );
        }
    }
}
