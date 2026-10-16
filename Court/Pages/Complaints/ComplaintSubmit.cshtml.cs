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
        private readonly IPetition c_petition;

        private readonly ILogger<ComplaintSubmitModel> _logger;

        public ComplaintSubmitModel(IComplaint complaint,IUser user,IPetition petition, ILogger<ComplaintSubmitModel> logger)
        {
            c_complaint = complaint;
            _user = user;
            c_petition = petition;
            _logger = logger;
        }

        [BindProperty]
        public Complaint ComplaintInfo { get; set; } = new Complaint();

        public IEnumerable<User> ComplainantUsers { get; set; } = new List<User>();

        public string ErrorMessage { get; set; }
        public int ComplaintId { get; set; }
        public int PetitionId { get; set; }
        public bool IsDecisionSaved { get; set; }
        public bool IsPetitionFinished { get; set; }

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

                ComplaintId = complaintId;

                var petition = c_petition.GetPetitionByComplaintId(complaintId);

                if (petition != null)
                {
                    PetitionId = petition.Id;
                    IsPetitionFinished = petition.FinishAndLock;
                }

                IsDecisionSaved = ComplaintInfo?.MagistrateDecision.HasValue == true;

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



                var complaint = c_complaint.GetComplaint(ComplaintInfo.Id);

                if (complaint == null)
                {
                    return NotFound();
                }

                ComplaintInfo.ComplaintType = complaint.ComplaintType;
                ModelState.Remove("ComplaintInfo.ComplaintType");

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
                    ComplaintId = complaint.Id;

                    var petition = c_petition.GetPetitionByComplaintId(complaint.Id);

                    if (petition != null)
                    {
                        PetitionId = petition.Id;
                        IsPetitionFinished = petition.FinishAndLock;
                    }

                    IsDecisionSaved = complaint.MagistrateDecision.HasValue;

                    ComplainantUsers = _user.GetUsersByRoleName("CIPD").ToList();

                    return Page();
                }


                // Save only after validation passes
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
    }
}