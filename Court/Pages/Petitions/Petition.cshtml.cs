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
        private readonly ILogger<PetitionModel> _logger;

        public PetitionModel(
            IPetition petition,
            IUser user,
            IComplaint complaint,
            ILogger<PetitionModel> logger)
        {
            c_petiton = petition;
            _user = user;
            c_complaint = complaint;
            _logger = logger;
        }

        [BindProperty]
        public Petition PetitionInfo { get; set; } = new Petition();

        public IEnumerable<User> Users { get; set; } = new List<User>();

        public bool IsDecisionSaved { get; set; }
        public IActionResult OnGet(int complaintId)
        {
            try
            {
                Users = _user.GetUsersByRole(2).ToList();

                var petition =
                    c_petiton.GetPetitionByComplaintId(complaintId);

                if (petition != null)
                {
                    PetitionInfo = petition;
                }
                else
                {
                    PetitionInfo.PetitionDate = DateTime.Today;
                    PetitionInfo.MagistrateDecisionDate = DateTime.Today;
                    PetitionInfo.ComplaintId = complaintId;
                }

                var complaint = c_complaint.GetComplaint(complaintId);

                IsDecisionSaved = complaint != null
                   && complaint.MagistrateDecision.HasValue;

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
                if (!ModelState.IsValid)
                {
                    Users = _user.GetUsersByRole(2).ToList();
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
                    var petition =
                        c_petiton.GetPetition(PetitionInfo.Id);


                    petition.PetitionDate = PetitionInfo.PetitionDate;
                    petition.PetitionTime = PetitionInfo.PetitionTime;

                    petition.Disobedient = PetitionInfo.Disobedient;
                    petition.FoundInUnlawfulPlace =
                        PetitionInfo.FoundInUnlawfulPlace;
                    petition.RanAway = PetitionInfo.RanAway;
                    petition.AbsentFromSchool =
                        PetitionInfo.AbsentFromSchool;
                    petition.SupportingFacts = PetitionInfo.SupportingFacts;

                    petition.MagistrateDecision =
                        PetitionInfo.MagistrateDecision;
                    petition.MagistrateDecisionDate =
                        PetitionInfo.MagistrateDecisionDate;
                    petition.MagistrateDecisionTime =
                        PetitionInfo.MagistrateDecisionTime;
                    petition.MagistrateId = PetitionInfo.MagistrateId;
                    petition.MagistrateSignature =
                        PetitionInfo.MagistrateSignature;

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
    }
}

