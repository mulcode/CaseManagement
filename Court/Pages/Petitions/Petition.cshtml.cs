using Court.ViewModels;
using CourtDataAccess.Interfaces;
using CourtDomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Court.Pages.Petitions
{
    public class PetitionModel : PageModel
    {
        private readonly IPetition c_petiton;
        private readonly IUser _user;
        private readonly IComplaint c_complaint;
        private readonly IDefendant d_defendant;
        private readonly ILogger<PetitionModel> _logger;

        public PetitionModel(
            IPetition petition,
            IUser user,
            IComplaint complaint,
            IDefendant defendant,
            ILogger<PetitionModel> logger)
        {
            c_petiton = petition;
            _user = user;
            c_complaint = complaint;
            d_defendant = defendant;
            _logger = logger;
        }

        [BindProperty]
        public Petition PetitionInfo { get; set; } 
        public IList<User> Users { get; set; }
        public int ComplaintId { get; set; }
        public ComplaintTabsViewModels ComplaintTabs { get; set; }

        public IActionResult OnGet(int complaintId)
        {
            try
            {
                ComplaintId = complaintId;
                Users = _user.GetUsersByRole(2).ToList();

                PetitionInfo = c_petiton.GetPetitionByComplaintId(complaintId);

                if (PetitionInfo == null)
                {
                    PetitionInfo = new Petition
                    {
                        PetitionDate = DateTime.Today,
                        MagistrateDecisionDate = DateTime.Today,
                        ComplaintId = complaintId
                    };
                }

                LoadTabs();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error loading petition for ComplaintId {ComplaintId}",
                    complaintId);

                TempData["ErrorMessage"] =
                    "An error occurred while loading the petition.";

                return RedirectToPage(
                    "/Petitions/Petition",
                    new { complaintId = complaintId });
            }
        }

        public IActionResult OnPost()
        {
            try
            {
                ComplaintId = PetitionInfo.ComplaintId;

                if (!ModelState.IsValid)
                {
                    Users = _user.GetUsersByRole(2).ToList();
                    LoadTabs();
                    return Page();
                }

                if (PetitionInfo.Id == 0)
                {
                    PetitionInfo.IsPetitionSaved = true;

                    c_petiton.AddPetition(PetitionInfo);
                    TempData["SuccessMessage"] = "Petition Saved Successfully";
                }
                else
                {
                    var petition = c_petiton.GetPetition(PetitionInfo.Id);


                    petition.PetitionDate = PetitionInfo.PetitionDate;
                    petition.PetitionTime = PetitionInfo.PetitionTime;
                    petition.Disobedient = PetitionInfo.Disobedient;
                    petition.FoundInUnlawfulPlace = PetitionInfo.FoundInUnlawfulPlace;
                    petition.RanAway = PetitionInfo.RanAway;
                    petition.AbsentFromSchool = PetitionInfo.AbsentFromSchool;
                    petition.SupportingFacts = PetitionInfo.SupportingFacts;
                    petition.MagistrateDecision = PetitionInfo.MagistrateDecision;
                    petition.MagistrateDecisionDate = PetitionInfo.MagistrateDecisionDate;
                    petition.MagistrateDecisionTime = PetitionInfo.MagistrateDecisionTime;
                    petition.MagistrateId = PetitionInfo.MagistrateId;
                    petition.MagistrateSignature = PetitionInfo.MagistrateSignature;
                    petition.FinishAndLock = PetitionInfo.FinishAndLock;
                    petition.IsPetitionSaved = true;

                    c_petiton.UpdatePetition(petition);
                    TempData["SuccessMessage"] = "Petition Updated Successfully";
                }


                return RedirectToPage(
                    "/Petitions/Petition",
                    new { complaintId = PetitionInfo.ComplaintId });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error saving or updating petition for ComplaintId {ComplaintId}",
                    PetitionInfo.ComplaintId);

                TempData["ErrorMessage"] =
                    "An error occurred while saving the petition.";

                return RedirectToPage(
                    "/Petitions/Petition",
                    new { complaintId = PetitionInfo.ComplaintId });
            }
        }
        private void LoadTabs()
        {
            var complaint = c_complaint.GetComplaint(ComplaintId);
            var petition = c_petiton.GetPetitionByComplaintId(ComplaintId);
            var defendant = d_defendant.GetDefendant(complaint.DefendantId);

            ComplaintTabs = new ComplaintTabsViewModels
            {
                ComplaintId = complaint.Id,
                DefendantId = complaint.DefendantId,
                DefendantName = $"{defendant.FirstName} {complaint.Defendant?.LastName}".Trim(),
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

