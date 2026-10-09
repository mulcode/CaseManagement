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
        private readonly ILogger<ComplaintDecisionModel> _logger;

        public ComplaintDecisionModel(
            IComplaint complaint,
            IUser user,
            ILogger<ComplaintDecisionModel> logger)
        {
            c_complaint = complaint;
            _user = user;
            _logger = logger;
        }

        [BindProperty]
        public Complaint ComplaintInfo { get; set; }

        public IEnumerable<User> Users { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet(int complaintId)
        {
            try
            {
                if (!User.IsInRole("Magistrate"))
                {
                    ErrorMessage =
                        "You are not authorized to give decision.";
                }

                ComplaintInfo = c_complaint.GetComplaint(complaintId);
                Users = _user.GetUsersByRoleName("CIPD").ToList();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading complaint decision for complaint {ComplaintId}",
                    complaintId);

                return StatusCode(
                    500,
                    "An error occurred while loading the complaint decision.");
            }
        }

        public IActionResult OnPost()
        {
            try
            {
                if (!User.IsInRole("Magistrate"))
                {
                    ErrorMessage =
                        "You are not authorized to give decision.";

                    Users = _user.GetUsersByRoleName("Magistrate").ToList();

                    ComplaintInfo = c_complaint.GetComplaint(ComplaintInfo.Id);

                    return Page();
                }

                var complaint = c_complaint.GetComplaint(ComplaintInfo.Id);
                    

               

                if (!complaint.ReadyForApproval)
                {
                    ErrorMessage =
                        "Please submit the complaint first.";

                    Users = _user.GetUsersByRoleName("Magistrate").ToList();
                    ComplaintInfo = complaint;

                    return Page();
                }

                if (complaint.MagistrateDecision.HasValue)
                {
                    ErrorMessage =
                        "A decision has already been saved.";

                    Users = _user.GetUsersByRoleName("Magistrate").ToList();
                    ComplaintInfo = complaint;

                    return Page();
                }

                complaint.MagistrateId = ComplaintInfo.MagistrateId;
                complaint.MagistrateSignature =
                    ComplaintInfo.MagistrateSignature;
                complaint.ReceivedDateByMagistrate =
                    ComplaintInfo.ReceivedDateByMagistrate;
                complaint.MagistrateDecision =
                    ComplaintInfo.MagistrateDecision;

                c_complaint.UpdateComplaint(complaint);

                TempData["SuccessMessage"] =
                    "Decision Saved Successfully";

                return RedirectToPage(
                    "/Complaints/ComplaintDecision",
                    new { complaintId = ComplaintInfo.Id }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error saving complaint decision for complaint {ComplaintId}",
                    ComplaintInfo?.Id);

                ErrorMessage =
                    "An error occurred while saving the decision.";

                Users = _user.GetUsersByRoleName("Magistrate").ToList();

                if (ComplaintInfo != null)
                {
                    ComplaintInfo =
                        c_complaint.GetComplaint(ComplaintInfo.Id)
                        ?? ComplaintInfo;
                }

                return Page();
            }
        }
    }
}

