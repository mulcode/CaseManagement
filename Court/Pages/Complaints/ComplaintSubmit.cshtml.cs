using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Complaints
{
    public class ComplaintSubmitModel : PageModel
    {
        private readonly IComplaint c_complaint;
        private readonly IUser _user;
        public ComplaintSubmitModel(IComplaint complaint, IUser user)
        {
            c_complaint = complaint;
            _user = user;
        }

        [BindProperty]
        public Complaint ComplaintInfo { get; set; } = new Complaint();

        public IEnumerable<User> ComplainantUsers { get; set; } = new List<User>();

        public IActionResult OnGet(int complaintId)
        {
            var complaint = c_complaint.GetComplaint(complaintId);

            if (complaint == null)
            {
                return NotFound();
            }
            ComplaintInfo = complaint;
            ComplainantUsers = _user.GetUsersByRole(1).ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ComplainantUsers = _user.GetUsersByRole(1).ToList();
                return Page();
            }

            c_complaint.UpdateComplaint(ComplaintInfo);

            TempData["SuccessMessage"] = "Complaint submitted successfully.";

            return RedirectToPage(
                "/Complaints/ComplaintSubmit",
                new { complaintId = ComplaintInfo.Id }
            );
        }
    
    }
}
