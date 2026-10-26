using Court.ViewModels;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Complaints
{
    public class ComplaintDecisionModel : PageModel
    {
        private readonly IComplaint c_complaint;
        private readonly IDefendant d_defendant;
        private readonly IUser _user;
        private readonly IPetition d_petition;
        private readonly ILogger<ComplaintDecisionModel> _logger;

        public ComplaintDecisionModel(
            IComplaint complaint,
            IUser user,
            IPetition petition,
            IDefendant defendant,
            ILogger<ComplaintDecisionModel> logger)
        {
            c_complaint = complaint;
            _user = user;
            d_petition = petition;
            d_defendant = defendant;
            _logger = logger;
        }

        [BindProperty]
        public Complaint ComplaintInfo { get; set; }
        public ComplaintTabsViewModels ComplaintTabs { get; set; }
        public IList<User> Users { get; set; }

        public string ErrorMessage { get; set; }
        public int ComplaintId { get; set; }

        public IActionResult OnGet(int complaintId)
        {
            try
            {
                ComplaintId = complaintId;

                ComplaintInfo = c_complaint.GetComplaint(complaintId);
                ComplaintInfo.ReceivedDateByMagistrate = DateTime.Today;

                Users = _user.GetUsersByRoleName("Magistrate").ToList();

                if (!User.IsInRole("Magistrate") && ComplaintInfo.MagistrateDecision == null)
                {
                    ErrorMessage =
                        "You are not authorized to give decision.";
                }

                else if (!ComplaintInfo.ReadyForApproval)
                {
                    ErrorMessage = "Please submit the complaint first.";
                }
             

                LoadTabs();

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
                var complaint = c_complaint.GetComplaint(ComplaintInfo.Id);

                ComplaintId = complaint.Id;

                if (!User.IsInRole("Magistrate"))
                {
                    ErrorMessage = "You are not authorized to give decision.";
                    Users = _user.GetUsersByRoleName("Magistrate").ToList();
                    LoadTabs();
                    return Page();
                }

                if (!complaint.ReadyForApproval)
                {
                    ErrorMessage = "Please submit the complaint first.";
                    ComplaintInfo = complaint;
                    Users = _user.GetUsersByRoleName("Magistrate").ToList();

                    LoadTabs();
                    return Page();
                }

                if (complaint.MagistrateDecision.HasValue)
                {
                    ErrorMessage = "A decision has already been saved.";
                    ComplaintInfo = complaint;
                    Users = _user.GetUsersByRoleName("Magistrate").ToList();

                    LoadTabs();
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
                bool hasDecisionErrors = ComplaintInfo.MagistrateId <= 0 ||
                    string.IsNullOrWhiteSpace(ComplaintInfo.MagistrateSignature) ||
                    !ComplaintInfo.MagistrateDecision.HasValue;

                if (hasDecisionErrors)
                {
                    Users = _user.GetUsersByRoleName("Magistrate").ToList();

                    ComplaintInfo.ReadyForApproval = complaint.ReadyForApproval;
                    ComplaintInfo.MagistrateDecision = complaint.MagistrateDecision;

                    LoadTabs();
                    return Page();
                }


                complaint.MagistrateId = ComplaintInfo.MagistrateId;
                complaint.MagistrateSignature = ComplaintInfo.MagistrateSignature;
                complaint.ReceivedDateByMagistrate = ComplaintInfo.ReceivedDateByMagistrate;
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

                LoadTabs();
                return Page();
            }
        }

        private void LoadTabs()
        {
            var complaint = c_complaint.GetComplaint(ComplaintId);

            var petition = d_petition.GetPetitionByComplaintId(ComplaintId);

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

