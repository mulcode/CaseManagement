using Court.ViewModels;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Complaints
{
    public class ComplaintSubmitModel : PageModel
    {
        private readonly IComplaint c_complaint;
        private readonly IDefendant d_defendant;
        private readonly IUser _user;
        private readonly IPetition c_petition;

        private readonly ILogger<ComplaintSubmitModel> _logger;

        public ComplaintSubmitModel(IComplaint complaint,IUser user,IPetition petition, ILogger<ComplaintSubmitModel> logger, IDefendant defendant)
        {
            c_complaint = complaint;
            _user = user;
            c_petition = petition;
            d_defendant = defendant;
            _logger = logger;
        }

        [BindProperty]
        public Complaint ComplaintInfo { get; set; }

        public IList<User> ComplainantUsers { get; set; }

        public string ErrorMessage { get; set; }
        public int ComplaintId { get; set; }
   
        public ComplaintTabsViewModels ComplaintTabs { get; set; }

        public IActionResult OnGet(int complaintId)
        {
            try
            {
              ComplaintId = complaintId;
              ComplainantUsers = _user.GetUsersByRoleName("CIPD").ToList();
              ComplaintInfo = c_complaint.GetComplaint(complaintId);
               
                if (!User.IsInRole("CIPD"))
                {
                    ErrorMessage =
                       "You are not authorized to Submit.";
                }

                LoadTabs();
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
                var complaint = c_complaint.GetComplaint(ComplaintInfo.Id);

                if (ComplaintInfo.ComplainantId <= 0)
                {
                    ModelState.AddModelError(
                        "ComplaintInfo.ComplainantId",
                        "Please select a complainant.");
                }
            

                if (string.IsNullOrWhiteSpace(ComplaintInfo.ComplainantSignature))
                {
                    ModelState.AddModelError(
                        "ComplaintInfo.ComplainantSignature",
                        "Complainant signature is required.");
                }


                if (!ModelState.IsValid)
                {
                    


                    ComplainantUsers = _user.GetUsersByRoleName("CIPD").ToList();

                    ComplaintId = complaint.Id;
                    LoadTabs();
                    return Page();
                }

                complaint.ComplainantId = ComplaintInfo.ComplainantId;
                complaint.ComplainantSignature = ComplaintInfo.ComplainantSignature;
                complaint.ComplainantPhone = ComplaintInfo.ComplainantPhone;
                complaint.BadgeNumber = ComplaintInfo.BadgeNumber;
                complaint.ReadyForApproval = true;


                c_complaint.UpdateComplaint(complaint);

                TempData["SuccessMessage"] = "Complaint Submission Successfully";

                return RedirectToPage(
                    "/Complaints/ComplaintSubmit",
                    new { complaintId = ComplaintInfo.Id });

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

        private void LoadTabs()
        {
            var complaint = c_complaint.GetComplaint(ComplaintId);

            var petition = c_petition.GetPetitionByComplaintId(ComplaintId);

            var defendant = d_defendant.GetDefendant(complaint.DefendantId);

            ComplaintTabs = new ComplaintTabsViewModels
            {
                ComplaintId = complaint.Id,
                DefendantId = complaint.DefendantId,
                DefendantName = $"{defendant.FirstName} {defendant.LastName}".Trim(),
                EnrolledNumber = defendant.EnrolledNumber,
                Address = defendant.Address,
                Gender = defendant.Gender,
                IsDecisionSaved = complaint.MagistrateDecision.HasValue,
                PetitionId = petition?.Id ?? 0,
                IsPetitionFinished = petition?.FinishAndLock ?? false
            };
        }
    }
}