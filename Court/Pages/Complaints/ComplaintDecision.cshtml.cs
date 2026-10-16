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
        private readonly IPetition d_petition;
        private readonly ILogger<ComplaintDecisionModel> _logger;

        public ComplaintDecisionModel(
            IComplaint complaint,
            IUser user,
            IPetition petition,
            ILogger<ComplaintDecisionModel> logger)
        {
            c_complaint = complaint;
            _user = user;
            d_petition = petition;
            _logger = logger;
        }

        [BindProperty]
        public Complaint ComplaintInfo { get; set; }

        public IEnumerable<User> Users { get; set; }

        public string ErrorMessage { get; set; }
        public int ComplaintId { get; set; }
        public int PetitionId { get; set; }
        public bool IsDecisionSaved { get; set; }
        public bool IsPetitionFinished { get; set; }


        public IActionResult OnGet(int complaintId)
        {
            try
            {
                ComplaintInfo = c_complaint.GetComplaint(complaintId);

                ComplaintId = complaintId;

                if (d_petition == null)
                {
                    throw new InvalidOperationException("IPetition dependency is null.");
                }

                

                var petition = d_petition.GetPetitionByComplaintId(complaintId);

                if (petition != null)
                {
                    PetitionId = petition.Id;
                    IsPetitionFinished = petition.FinishAndLock;
                }

                IsDecisionSaved = ComplaintInfo?.MagistrateDecision.HasValue == true;

                ComplaintInfo.ReceivedDateByMagistrate = DateTime.Today;

                if (!User.IsInRole("Magistrate"))
                {
                    ErrorMessage =
                        "You are not authorized to give decision.";
                }
                else if (!ComplaintInfo.ReadyForApproval)
                {
                    ErrorMessage = "Please submit the complaint first.";
                }
             
                Users = _user.GetUsersByRoleName("Magistrate").ToList();

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
                    ErrorMessage = "You are not authorized to give decision.";
                    Users = _user.GetUsersByRoleName("Magistrate").ToList();
                    return Page();
                }

                var complaint = c_complaint.GetComplaint(ComplaintInfo.Id);

                if (complaint == null)
                {
                    return NotFound();
                }

                if (!complaint.ReadyForApproval)
                {
                    ErrorMessage = "Please submit the complaint first.";
                    ComplaintInfo = complaint;
                    Users = _user.GetUsersByRoleName("Magistrate").ToList();
                    return Page();
                }

                if (complaint.MagistrateDecision.HasValue)
                {
                    ErrorMessage = "A decision has already been saved.";
                    ComplaintInfo = complaint;
                    Users = _user.GetUsersByRoleName("Magistrate").ToList();
                    return Page();
                }

                // Validate Magistrate Name
                if (ComplaintInfo.MagistrateId <= 0)
                {
                    ModelState.AddModelError(
                        "ComplaintInfo.MagistrateId",
                        "Please select a Magistrate.");
                }

                // Validate Magistrate Signature
                if (string.IsNullOrWhiteSpace(ComplaintInfo.MagistrateSignature))
                {
                    ModelState.AddModelError(
                        "ComplaintInfo.MagistrateSignature",
                        "Magistrate signature is required.");
                }

                // Validate Decision
                if (!ComplaintInfo.MagistrateDecision.HasValue)
                {
                    ModelState.AddModelError(
                        "ComplaintInfo.MagistrateDecision",
                        "Please select a decision.");
                }

                // If validation fails, do not save
                bool hasDecisionErrors =
                    ComplaintInfo.MagistrateId <= 0 ||
                    string.IsNullOrWhiteSpace(ComplaintInfo.MagistrateSignature) ||
                    !ComplaintInfo.MagistrateDecision.HasValue;

                if (hasDecisionErrors)
                {
                    ComplaintId = complaint.Id;

                    ComplaintInfo.ReadyForApproval = complaint.ReadyForApproval;
                    ComplaintInfo.MagistrateDecision = complaint.MagistrateDecision;

                    var petition = d_petition.GetPetitionByComplaintId(complaint.Id);

                    if (petition != null)
                    {
                        PetitionId = petition.Id;
                        IsPetitionFinished = petition.FinishAndLock;
                    }

                    IsDecisionSaved = complaint.MagistrateDecision.HasValue;
                    Users = _user.GetUsersByRoleName("Magistrate").ToList();

                    return Page();
                }

                // Save only after validation passes
                complaint.MagistrateId = ComplaintInfo.MagistrateId;
                complaint.MagistrateSignature = ComplaintInfo.MagistrateSignature;
                complaint.ReceivedDateByMagistrate =
                    ComplaintInfo.ReceivedDateByMagistrate;
                complaint.MagistrateDecision = ComplaintInfo.MagistrateDecision;

                c_complaint.UpdateComplaint(complaint);

                TempData["SuccessMessage"] = "Decision Saved Successfully";

                return RedirectToPage(
                    "/Complaints/ComplaintDecision",
                    new { complaintId = ComplaintInfo.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error saving complaint decision for complaint {ComplaintId}",
                    ComplaintInfo?.Id);

                ErrorMessage = "An error occurred while saving the decision.";
                Users = _user.GetUsersByRoleName("Magistrate").ToList();

                return Page();
            }
        }

    }
}

