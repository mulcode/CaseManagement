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
        private readonly ILogger<ComplaintSubmitModel> _logger;

        public ComplaintSubmitModel(IComplaint complaint,IUser user, ILogger<ComplaintSubmitModel> logger)
        {
            c_complaint = complaint;
            _user = user;
            _logger = logger;
        }

        [BindProperty]
        public Complaint ComplaintInfo { get; set; } = new Complaint();

        public IEnumerable<User> ComplainantUsers { get; set; } = new List<User>();

        public string ErrorMessage { get; set; }

        public IActionResult OnGet(int complaintId)
        {
            try
            {

                if (!User.IsInRole("CIPD"))
                {
                    ErrorMessage =
                       "You are not authorized to Submit.";
                }

                ComplaintInfo = c_complaint.GetComplaint(complaintId); 
                ComplainantUsers = _user.GetUsersByRoleName("CIPD").ToList();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading complaint submit page for ComplaintId {ComplaintId}",
                    complaintId);

                TempData["ErrorMessage"] =
                    "An error occurred while loading the complaint.";

                return RedirectToPage("/Complaints/ComplaintList");
            }
        }

        public IActionResult OnPost()
        {
            try
            {
                if (!User.IsInRole("CIPD"))
                {
                    return Forbid();
                }

                var complaint = c_complaint.GetComplaint(ComplaintInfo.Id);

               

                complaint.ComplainantId = ComplaintInfo.ComplainantId;
                complaint.ComplainantSignature = ComplaintInfo.ComplainantSignature;
                complaint.ComplainantPhone = ComplaintInfo.ComplainantPhone;
                complaint.BadgeNumber = ComplaintInfo.BadgeNumber;


                ComplaintInfo.ComplaintType = complaint.ComplaintType;

                ModelState.Remove("ComplaintInfo.ComplaintType");

                if (!ModelState.IsValid)
                {
                    ComplainantUsers = _user.GetUsersByRoleName("CIPD").ToList();
                    return Page();
                }

                complaint.ReadyForApproval = true;

                c_complaint.UpdateComplaint(complaint);

                TempData["SuccessMessage"] = "Saved Successfully";

                return RedirectToPage(
                    "/Complaints/ComplaintSubmit",
                    new { complaintId = ComplaintInfo.Id }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error saving complaint submission for ComplaintId {ComplaintId}",
                    ComplaintInfo.Id);

                TempData["ErrorMessage"] =
                    "An error occurred while saving the complaint.";

                return RedirectToPage(
                    "/Complaints/ComplaintSubmit",
                    new { complaintId = ComplaintInfo.Id }
                );
            }
        }
    }
}